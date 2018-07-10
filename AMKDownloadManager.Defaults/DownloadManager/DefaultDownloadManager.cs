using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.Utils;
using ThreadState = System.Threading.ThreadState;

namespace AMKDownloadManager.Defaults.DownloadManager
{
    /// <summary>
    /// Default application download manager.
    /// </summary>
    public class DefaultDownloadManager : IDownloadManager
    {
        public IAppContext AppContext { get; }
        public IConfigProvider Configuration { get; }
        public IThreadFactory ThreadFactory { get; }

        private volatile bool _downloadManagerState = false;

        private IScheduler _scheduler;

        private IThread _schedulerThread;
        private int _maxSimultaneousJobs = 3;

        private readonly List<DefaultDownloadManagerJobContext> _jobs;
        private readonly List<DefaultDownloadManagerJobContext> _pendingJobs;
        private readonly List<DefaultDownloadManagerJobContext> _runningJobs;

        private readonly ManualResetEvent _finishEvent;

        public DefaultDownloadManager(
            IAppContext appContext, IScheduler scheduler)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

            Configuration = appContext.GetFeature<IConfigProvider>();
            ThreadFactory = appContext.GetFeature<IThreadFactory>();


            _jobs = new List<DefaultDownloadManagerJobContext>();
            _pendingJobs = new List<DefaultDownloadManagerJobContext>();
            _runningJobs = new List<DefaultDownloadManagerJobContext>();

            _finishEvent = new ManualResetEvent(false);
        }

        #region IDownloadManager implementation

        /// <inheritdoc />
        public IDownloadManagerHandle Schedule(IJob job) => Schedule(job, null);

        /// <inheritdoc />
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


        /// <inheritdoc />
        public void Start()
        {
            if (_schedulerThread != null)
            {
                _schedulerThread.Abort();
                _schedulerThread.Join();
                //#error Check for safety of join after abort...

                //Do not abort the innocent.
            }

            _schedulerThread = ThreadFactory.Create(_scheduleCallback, "Download Manager Scheduler Thread");

            _downloadManagerState = true;

            _schedulerThread.Start();

            //OnStarted(EventArgs.Empty);
        }

        private void _scheduleCallback()
        {
            _finishEvent.Reset();

            try
            {
                var loop = new LoopCountLimiter(10);
                while (_downloadManagerState)
                {
                    IJob[] pendingJobs;
                    IJob[] runningJobs;
                    lock (_jobs)
                    {
                        if (!(_runningJobs.Any() || _pendingJobs.Any()))
                        {
                            break;
                        }

                        pendingJobs = _pendingJobs.Select(x => x.Job).ToArray();
                        runningJobs = _runningJobs.Select(x => x.Job).ToArray();
                    }

                    if (_maxSimultaneousJobs > runningJobs.Length)
                    {
                        var job = _scheduler.SelectJob(
                            this,
                            pendingJobs,
                            runningJobs);

                        if (job != null)
                        {
                            var jobHandle = _jobs.FirstOrDefault(x => x.Job == job);
                            jobHandle?.Start();
                        }
                    }

                    loop.Count();
                }
            }
            finally
            {
                _finishEvent.Set();

                _downloadManagerState = false;

                //#error while (_downloadManagerState || {{{{{ _runningJobs.Any() }}}}} ) | _runningJobs.Any() ??? Stop the download manager and signal to all.
            }
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
            //OnStopping(state);
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

        public void WaitToFinish()
        {
            //wait until scheduler finish the job queue.
            _finishEvent.WaitOne();

            //wait until scheduler thread exit.
            //SpinWait.SpinUntil(() => _schedulerThread.IsAlive);
        }

        public void WaitToFinish(TimeSpan timeout)
        {
            _finishEvent.WaitOne(timeout);
            //SpinWait.SpinUntil(() => _schedulerThread.IsAlive);
        }

        public void WaitToFinish(IJob job)
        {
        }

        public void WaitToFinish(IJob job, TimeSpan timeout)
        {
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
                DownloadManager._setJobAsRunning(this);
                _downloadJobState = true;
                
#if DEBUG
                DispatcherThread =
                    DownloadManager.ThreadFactory.Create(_dispatcher, $"Download Dispatcher (TriggerJobAndGetInfo) {Job.GetDebugName()}");
#else
                DispatcherThread =
                    DownloadManager.ThreadFactory.Create(_dispatcher, "Download Dispatcher (TriggerJobAndGetInfo)");
#endif

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
#if DEBUG
                    var guide = "Job Main Thread";
                    jobInfo.MainJobPart.DebugName = guide;

                    var thread = DownloadManager.ThreadFactory.Create(_processPart, guide);

//                    Trace.WriteLine(
//                        $"Partial download thread created with id of: '{thread.ManagedThreadId}' - '{guide}'");
#else
                    var thread = DownloadManager.ThreadFactory.Create(_processPart);
#endif
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
                    var threadCount = 0;
                    if (jobInfo.SupportsConcurrency)
                    {
                        var loop = new LoopCountLimiter(5);
                        while ((_downloadJobState) &&
                               (Threads.Count == 0 || Threads.Any(x => x.ThreadState != ThreadState.Stopped)) &&
                               (!jobInfo.SegmentationContext.CheckIfAllFilledWithLock()))
                        {
                            if (Threads.Count <= _maxSimultaneousConnections)
                            {
                                var dispatchRetry = new RetryHelper(_maxFailureRetries);
                                while (!dispatchRetry.IsDone())
                                {
                                    IJobPart part = null;
                                    try
                                    {
                                        part = Job.GetJobPart(jobInfo);
                                        //if ()
                                        dispatchRetry.Done();
                                    }
                                    catch (Exception ex)
                                    {
                                        dispatchRetry.Catch(ex);
                                    }

                                    if (part == null)
                                    {
                                        dispatchRetry.Fail();
                                    }
                                    else
                                    {
                                        threadCount++;

#if DEBUG
                                        var guide = $"Partial Thread : {threadCount}";
                                        part.DebugName = guide;

                                        var thread = DownloadManager.ThreadFactory.Create(_processPart, guide);

//                                        Trace.WriteLine(
//                                            $"Partial download thread created with id of: '{thread.ManagedThreadId}' - '{guide}'");
#else
                                        var thread = DownloadManager.ThreadFactory.Create(_processPart);
#endif
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

#warning Error check!

                DownloadManager._runningJobs.Remove(this);
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
#if DEBUG
//                        Trace.WriteLine($"Cycling the job part: {part.GetDebugName()}");
#endif
                        result = part.Cycle();
                        retry.Done();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
//                        Trace.WriteLine(part.GetDebugName() + ex.ToString());
#endif

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
            }

            public void Resume()
            {
            }

            public void Clean()
            {
            }


            public void WhenFinished(Action<IDownloadManagerHandle> handle)
            {
            }

            #endregion
        }
    }
}