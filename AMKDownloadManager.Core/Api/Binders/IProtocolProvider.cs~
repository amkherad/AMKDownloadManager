using System;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Binders
{
    public interface IProtocolProvider : IFeature
    {
        bool CanHandle(DownloadItem downloadItem);

        IJob CreateJob(DownloadItem downloadItem, IAppContext app, JobParameters jobParameters);
    }
}