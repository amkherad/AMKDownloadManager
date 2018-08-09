using System;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.JobScheduler
{
    //TODO: Implement priority later.
    public class DefaultJobScheduler : IScheduler
    {
        public IApplicationContext ApplicationContext { get; }

        public DefaultJobScheduler(IApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }


        #region IScheduler implementation

        public IJob SelectJob(IDownloadManager downloadManager, IEnumerable<IJob> freeJobs, IEnumerable<IJob> runningJobs)
        {
            return freeJobs.FirstOrDefault();
        }

        public int Order => 0;
        
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
        
        #endregion
    }
}