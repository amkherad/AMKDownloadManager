using System;
using AMKDownloadManager.Core.Api.DownloadManagement;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.Impl
{
    //TODO: Implement priority later.
    public class DefaultJobScheduler : IScheduler
    {
        public IAppContext AppContext { get; }

        private IConfigProvider _configProvider;

        private List<IJob> _jobs;
        private List<IJob> _activeJobs;
        private List<IJob> _readyJobs;
        private List<IJob> _finishedJobs;

        private int _maxSimultaneousConnections = 10;


        public DefaultJobScheduler(IAppContext appContext)
        {
            AppContext = appContext;

            _jobs = new List<IJob>();
            _activeJobs = new List<IJob>();
            _readyJobs = new List<IJob>();
            _finishedJobs = new List<IJob>();
        }


        protected IConfigProvider Configuration
        {
            get
            {
                if (_configProvider == null)
                {
                    AppContext.GetFeature<IConfigProvider>();
                }
                return _configProvider;
            }
        }

        #region IScheduler implementation

        public IJob SelectJob(IDownloadManager downloadManager, IEnumerable<IJob> freeJobs, IEnumerable<IJob> runningJobs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IScheduler implementation

        public void EnqueueJob(IJob job)
        {
            job.Started += Job_Started;
            job.Paused += Job_Paused;
            //job.Progress += Job_Progress;
            job.Finished += Job_Finished;
            job.PriorityChanged += Job_PriorityChanged;

            lock (_jobs)
            {
                _jobs.Add(job);
            }
        }

        public void EnqueueJobs(IEnumerable<IJob> jobs)
        {
            foreach (var job in jobs)
            {
                job.Started += Job_Started;
                job.Paused += Job_Paused;
                //job.Progress += Job_Progress;
                job.Finished += Job_Finished;
                job.PriorityChanged += Job_PriorityChanged;
            }

            lock (_jobs)
            {
                _jobs.AddRange(jobs);
            }
        }

        public void DequeueJob(IJob job)
        {
            throw new NotImplementedException();
        }

        public void DequeueJobs(IEnumerable<IJob> jobs)
        {
            throw new NotImplementedException();
        }

        public void ClearJobs()
        {
            throw new NotImplementedException();
        }

        private void Job_Started(IJob job, EventArgs eventArgs)
        {

        }

        private void Job_Paused(IJob job, EventArgs eventArgs)
        {
            lock (_jobs)
            {
                _activeJobs.Remove(job);
                _readyJobs.Add(job);
            }
        }

        private void Job_Finished(IJob job, EventArgs eventArgs)
        {
            lock (_jobs)
            {
                _jobs.Remove(job);
                _activeJobs.Remove(job);
                _readyJobs.Remove(job);

                _finishedJobs.Add(job);
            }
        }

        private void Job_Progress(IJob job, int progress)
        {

        }

        private void Job_PriorityChanged(IJob job, SchedulerPriority priority)
        {

        }

//
//        public JobChunkDescriptor PopJobChunk()
//        {
//            var config = Configuration;
//
//            _maxSimultaneousJobs = config.GetInt(
//                KnownConfigs.DownloadManager.MaxSimultaneousJobs,
//                KnownConfigs.DownloadManager.MaxSimultaneousJobs_DefaultValue
//            );
//            _maxSimultaneousConnections = config.GetInt(
//                KnownConfigs.DownloadManager.MaxSimultaneousConnections,
//                KnownConfigs.DownloadManager.MaxSimultaneousConnections_DefaultValue
//            );
//
//            IJob job;
//            IJobChunk chunk;
//            var jobsTested = 0;
//            lock (_jobs)
//            {
//                while (chunk == null && jobsTested < _jobs.Count)
//                {
//                    ++jobsTested;
//
//                    if (_maxSimultaneousJobs > _activeJobs.Count)
//                    {
//                        job = _activateReadyJob();
//                    }
//                    if (job == null)
//                    {
//                        job = _activeJobs.FirstOrDefault();
//                        if (job == null)
//                        {
//                            return null;
//                        }
//                        else
//                        {
//                            //TODO: Priority action needed.
//                            _activeJobs.Remove(job);
//                            _activeJobs.Add(job);
//                        }
//                    }
//
//                    chunk = job.GetChunk();
//                }
//            }
//        }
        // !!!! CALL INSIDE LOCK.
//        private IJob _activateReadyJob()
//        {
//            var job = _readyJobs.FirstOrDefault();
//            _readyJobs.Remove(job);
//            //TODO: Priority action needed.
//            _activeJobs.Add(job);
//            return job;
//        }

//        public void SetJobChunkState(JobChunkDescriptor jobChunkDescriptor)
//        {
//            throw new NotImplementedException();
//        }

        #endregion
        
        public int Order => 0;
    }
}