using System;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Defaults.JobScheduler
{
    //TODO: Implement priority later.
    public class DefaultJobScheduler : IScheduler
    {
        public IAppContext AppContext { get; }

        public DefaultJobScheduler(IAppContext appContext)
        {
            AppContext = appContext;
        }


        #region IScheduler implementation

        public IJob SelectJob(IDownloadManager downloadManager, IEnumerable<IJob> freeJobs, IEnumerable<IJob> runningJobs)
        {
            return freeJobs.FirstOrDefault();
        }

        public int Order => 0;
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
        
        #endregion
    }
}