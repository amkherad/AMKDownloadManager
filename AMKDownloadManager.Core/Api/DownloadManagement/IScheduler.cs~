using System;
using System.Collections.Generic;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public enum SchedulerPriority
    {
        Hight = 100,
        Normal = 0,
        Low = -100
    }

    public interface IScheduler : IFeature
    {
        IJob SelectJob(
            IDownloadManager downloadManager,
            IEnumerable<IJob> freeJobs,
            IEnumerable<IJob> runningJobs);
    }
}