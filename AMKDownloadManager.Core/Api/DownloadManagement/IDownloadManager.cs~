using System;
using System.Threading.Tasks;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface IDownloadManager : IFeature
    {
        IDownloadManagerHandle Schedule(IJob job);
        IDownloadManagerHandle ScheduleAsync(IJob job, Action<IJob> callback);
    }
}