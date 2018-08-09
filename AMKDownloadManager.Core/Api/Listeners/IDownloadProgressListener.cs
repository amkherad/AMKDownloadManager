using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IDownloadProgressListener : IListenerFeature
    {
        void OnProgress(
            IApplicationContext applicationContext,
            IJobPart job,
            DownloadItem downloadItem,
            IHttpTransport transport,
            ISegmentation segmentation,
            Segment segment);
        
        void OnFinished(
            IApplicationContext applicationContext,
            IJob job, DownloadItem downloadItem,
            IHttpTransport transport);
    }
}