using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadProgressListener
    {
        void OnProgress(
            IAppContext appContext,
            IJob job, DownloadItem downloadItem,
            IHttpRequestBarrier barrier,
            long totalSize,
            long progress);
        
        void OnFinished(
            IAppContext appContext,
            IJob job, DownloadItem downloadItem,
            IHttpRequestBarrier barrier);
    }
}