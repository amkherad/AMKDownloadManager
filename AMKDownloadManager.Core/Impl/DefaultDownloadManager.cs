using System;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Threading;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using ir.amkdp.gear.core.Utils;
using System.Linq;

namespace AMKDownloadManager.Core.Impl
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
            var context = new DefaultDownloadManagerJobContext(this, AppContext, Configuration, job)
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

                //Do not abort the innocent.
            }

            _schedulerThread = ThreadFactory.Create(_scheduleCallback);

            _downloadManagerState = true;

            _schedulerThread.Start();

            OnStarted(EventArgs.Empty);
        }
        private void _scheduleCallback()
        {
            var config = Configuration;

            var loop = new LoopCountLimiter(10);
            while (_downloadManagerState)
            {
                _maxSimultaneousJobs = config.GetInt(
                    KnownConfigs.DownloadManager.MaxSimultaneousJobs,
                    KnownConfigs.DownloadManager.MaxSimultaneousJobs_DefaultValue
                );

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
                foreach (var thread in job.Threads)
                {
                    thread.Join();
                }
            }
        }

        public void Join(IJob job)
        {
            var threads = FindJob(job)?.Threads;
            if (threads == null) throw new InvalidOperationException();
            
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
                
                DispatcherThread.Start();
            }

            private void _dispatcher()
            {
                var jobInfo = Job.TriggerJobAndGetInfo();

                if (jobInfo.SupportsConcurrency)
                {
                    var loop = new LoopCountLimiter(5);
                    while (_downloadJobState)
                    {
                        if (Threads.Count < _maxSimultaneousConnections)
                        {
                            var chunk = Job.GetJobChunk();
                            var thread = DownloadManager.ThreadFactory.Create(_processChunk);
                            thread.Start(chunk);
                        }
                        else
                        {
                            loop.Count();
                        }
                    }
                }
            }
            private void _processChunk(object state)
            {
                var chunk = state as IJobChunk;

                var tries = 0;
                JobChunkState result;
                do
                {
                    result = chunk.Cycle();
                }
                while(result == JobChunkState.RequestMoreCycle ||
                    (result == JobChunkState.ErrorCanTry && (tries < _maxFailureRetries)));
                
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