using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadProgressListener
    {
        void OnProgress(
            IAppContext appContext,
            IJob job, DownloadItem downloadItem,
            IHttpRequestTransport transport,
            long totalSize,
            long progress);
        
        void OnFinished(
            IAppContext appContext,
            IJob job, DownloadItem downloadItem,
            IHttpRequestTransport transport);
    }
}