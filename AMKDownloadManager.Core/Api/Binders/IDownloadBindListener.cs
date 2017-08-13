using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.Binders
{
    public interface IDownloadBindListener : IFeature
    {
        void NotifyBind(DownloadItem downloadItem);
        void NotifyUnBind(DownloadItem downloadItem);
    }
}