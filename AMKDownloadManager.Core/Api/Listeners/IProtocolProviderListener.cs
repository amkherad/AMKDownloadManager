using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IProtocolProviderListener : IListenerFeature
    {
        void JobCreated(
            IApplicationContext applicationContext,
            DownloadItem downloadItem,
            IJob job,
            JobParameters jobParameters,
            IProtocolProvider protocolProvider);
        
        void RequestCreated(
            IApplicationContext applicationContext,
            DownloadItem downloadItem,
            IRequest request,
            IProtocolProvider protocolProvider);
    }
}