using System;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.DownloadManager
{
    /// <summary>
    /// Default application download manager.
    /// </summary>
    public class DefaultDownloadManager : IDownloadManager
    {
        #region Events

        public event DownloadManagerEvent AllFinished;
        public event DownloadManagerEvent Started;
        public event DownloadManagerEvent Stopped;
        public event DownloadManagerEvent StopCanceled;
        public event DownloadManagerCancellableEvent Stopping;
        public event DownloadManagerJobEvent DownloadStarted;
        public event DownloadManagerJobEvent DownloadFinished;

        #endregion

        public IAppContext AppContext { get; }
        public IConfigProvider Configuration { get; }
        public IThreadFactory ThreadFactory { get; }
        
        private volatile bool _downloadManagerState = false;

        private IScheduler _scheduler;

        private IThread _schedulerThread;
        private int _maxSimultaneousJobs = 3;

        private List<DefaultDownloadManagerJobContext> _jobs;
        private List<DefaultDownloadManagerJobContext> _pendingJobs;
        private List<DefaultDownloadManagerJobContext> _runningJobs;


        public DefaultDownloadManager(
            IAppContext appContext, IScheduler scheduler)
        {
            if (appContext == null) throw new ArgumentNullException(nameof(appContext));
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));

            AppContext = appContext;
            Configuration = appContext.GetFeature<IConfigProvider>();
            ThreadFactory = appContext.GetFeature<IThreadFactory>();

            _scheduler = scheduler;

            _jobs = new List<DefaultDownloadManagerJobContext>();
            _pendingJobs = new List<DefaultDownloadManagerJobContext>();
            _runningJobs = new List<DefaultDownloadManagerJobContext>();
        }

        #region IDownloadManager implementation

        protected void OnAllFinished(EventArgs eventArgs) => AllFinished?.Invoke(this, eventArgs);
        protected void OnStarted(EventArgs eventArgs) => Started?.Invoke(this, eventArgs);
        protected void OnStopped(EventArgs eventArgs) => Stopped?.Invoke(this, eventArgs);
        protected void OnStopCanceled(EventArgs eventArgs) => StopCanceled?.Invoke(this, eventArgs);
        protected void OnStopping(CancelEventArgs eventArgs) => Stopping?.Invoke(this, eventArgs);
        protected void OnDownloadStarted(IJob job) => DownloadStarted?.Invoke(this, job);
        protected void OnDownloadFinished(IJob job) => DownloadFinished?.Invoke(this, job);


        public IDownloadManagerHandle Schedule(IJob job) => Schedule(job, null);

        public IDownloadManagerHandle Schedule(IJob job, Action<IJob> callback)
        {
            var context = new DefaultDownloadManagerJobContext(
                this,
                AppContext,
                Configuration,
                job)
            {
                FinishCallback = callback
            };

            lock (_jobs)
            {
                _jobs.Add(context);
                _pendingJobs.Add(context);
            }

            return context;
        }


        public void Start()
        {
            if (_schedulerThread != null)
            {
                _schedulerThread.Abort();
				_schedulerThread.Join();
				//#error Check for safety of join after abort...

                //Do not abort the innocent.
            }

            _schedulerThread = ThreadFactory.Create(_scheduleCallback);

            _downloadManagerState = true;

            _schedulerThread.Start();

            OnStarted(EventArgs.Empty);
        }

        private void _scheduleCallback()
        {
            var loop = new LoopCountLimiter(10);
            while (_downloadManagerState || _runningJobs.Any())
            {
                if (_pendingJobs.Any() &&
                    _maxSimultaneousJobs > _runningJobs.Count)
                {
                    var job = _scheduler.SelectJob(
                        this,
                        _pendingJobs.Select(x => x.Job),
                        _runningJobs.Select(x => x.Job));

                    var jobHandle = _jobs.FirstOrDefault(x => x.Job == job);
                    jobHandle?.Start();
                }

                loop.Count();
            }
			
			_downloadManagerState = false;
			//#error while (_downloadManagerState || {{{{{ _runningJobs.Any() }}}}} ) | _runningJobs.Any() ??? Stop the download manager and signal to all.
        }

        private void _setJobAsRunning(DefaultDownloadManagerJobContext jobContext)
        {
            lock (_jobs)
            {
                _pendingJobs.Remove(jobContext);
                _runningJobs.Add(jobContext);
            }
        }

        private void _setJobAsFree(DefaultDownloadManagerJobContext jobContext)
        {
            lock (_jobs)
            {
                _runningJobs.Remove(jobContext);
                _pendingJobs.Add(jobContext);
            }
        }

        public void Stop()
        {
            var state = new CancelEventArgs();
            OnStopping(state);
            if (state.IsCanceled)
            {
                return;
            }

            _downloadManagerState = false;
            _schedulerThread = null;
        }

        public void ForceStop()
        {
            _downloadManagerState = false;
            _schedulerThread.Abort();
            _schedulerThread = null;
        }

        public void Join()
        {
            foreach (var job in _runningJobs)
            {
                job.DispatcherThread.Join();
                
                foreach (var thread in job.Threads)
                {
                    thread.Join();
                }
            }
            
            _schedulerThread.Join();
        }

        public void Join(IJob job)
        {
            var jobContext = FindJob(job);
            if (jobContext == null) throw new InvalidOperationException();
            
            var threads = jobContext.Threads;
            if (threads == null) throw new InvalidOperationException();

            jobContext.DispatcherThread.Join();
		    
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        protected DefaultDownloadManagerJobContext FindJob(IJob job)
        {
            return _jobs.FirstOrDefault(x => x.Job == job);
        }

        #endregion

        #region IFeature implementation

        public int Order => 0;

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            _maxSimultaneousJobs = configProvider.GetInt(this,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobsDefaultValue
            );
        }

        #endregion

        public class DefaultDownloadManagerJobContext : IDownloadManagerHandle
        {
            public DefaultDownloadManager DownloadManager { get; }

            public IAppContext AppContext { get; }
            public IConfigProvider Configuration { get; }

            public IJob Job { get; }
            public IThread DispatcherThread { get; private set; }
            public List<IThread> Threads { get; }

            public Action<IJob> FinishCallback { get; set; }

            private int _maxSimultaneousConnections = 4;
            private int _maxFailureRetries = 4;
            private volatile bool _downloadJobState = false;


            public DefaultDownloadManagerJobContext(
                DefaultDownloadManager downloadManager,
                IAppContext appContext,
                IConfigProvider configProvider,
                IJob job)
            {
                DownloadManager = downloadManager;

                AppContext = appContext;
                Configuration = configProvider;
                Job = job;
                Threads = new List<IThread>();
            }

            #region IDownloadManagerHandle implementation

            public void Start()
            {
                DispatcherThread = DownloadManager.ThreadFactory.Create(_dispatcher);

                DownloadManager._setJobAsRunning(this);
                _downloadJobState = true;

                DispatcherThread.Start();
            }

            private void _dispatcher()
            {
                JobInfo jobInfo = null;

                var retry = new RetryHelper(_maxFailureRetries);
                while (!retry.IsDone())
                {
                    try
                    {
                        jobInfo = Job.TriggerJobAndGetInfo();
                        retry.Done();
                    }
                    catch (Exception ex)
                    {
                        retry.Catch(ex);
                        AppContext.SignalFeatures<IDownloadErrorListener>(x => x.OnGetInfoError(
                            AppContext,
                            Job,
                            DownloadManager,
                            !retry.IsDone()
                        ));
                    }
                }

                if (jobInfo == null)
                {
                    AppContext.SignalFeatures<IDownloadErrorListener>(x => x.OnDeadError(
                        AppContext,
                        Job,
                        DownloadManager
                    ));
                    return;
                }

                var mainJobPart = jobInfo.MainJobPart;
                if (mainJobPart != null)
                {
                    var thread = DownloadManager.ThreadFactory.Create(_processPart);
                    thread.Start(mainJobPart);
                    lock (Threads)
                    {
                        Threads.Add(thread);
                    }
                }
                
                //if (jobInfo.IsFinished)
                {
                    //TODO:
                    #warning Do something
                }
                //else
                {
                    if (jobInfo.SupportsConcurrency)
                    {
                        var loop = new LoopCountLimiter(5);
                        while (_downloadJobState)
                        {
                            if (Threads.Count < _maxSimultaneousConnections)
                            {
                                var dispathRetry = new RetryHelper(_maxFailureRetries);
                                while (!dispathRetry.IsDone())
                                {
                                    IJobPart part = null;
                                    try
                                    {
                                        part = Job.GetJobPart(jobInfo);
                                        //if ()
                                        dispathRetry.Done();
                                    }
                                    catch (Exception ex)
                                    {
                                        dispathRetry.Catch(ex);
                                    }

                                    if (part == null)
                                    {
                                        dispathRetry.Fail();
                                    }
                                    else
                                    {
                                        var thread = DownloadManager.ThreadFactory.Create(_processPart);
                                        thread.Start(part);
                                        lock (Threads)
                                        {
                                            Threads.Add(thread);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                loop.Count();
                            }
                        }
                    }
                }
            }

            private void _processPart(object state)
            {
                var part = state as IJobPart;
                if (part == null) return;

                var retry = new RetryHelper(_maxFailureRetries);
                JobPartState result;
                do
                {
                    try
                    {
                        result = part.Cycle();
                        retry.Done();
                    }
                    catch (Exception ex)
                    {
                        retry.Catch(ex);
                        result = JobPartState.ErrorCanRetry;
                        AppContext.SignalFeatures<IDownloadErrorListener>(x => x.OnPartError(
                            AppContext,
                            Job,
                            part,
                            DownloadManager,
                            !retry.IsDone()
                        ));
                    }
                } while (result == JobPartState.RequestMoreCycle ||
                         result == JobPartState.ErrorCanRetry && !retry.IsDone());
                
                part.Dispose();
            }

            public void Pause()
            {
                throw new NotImplementedException();
            }

            public void Resume()
            {
                throw new NotImplementedException();
            }

            public void Clean()
            {
                throw new NotImplementedException();
            }


            public void WhenFinished(Action<IDownloadManagerHandle> handle)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}