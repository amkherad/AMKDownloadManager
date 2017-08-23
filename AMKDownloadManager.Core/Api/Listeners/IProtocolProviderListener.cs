using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.Listeners
{
    public interface IProtocolProviderListener : IListenerFeature
    {
        void JobCreated(
            IAppContext appContext,
            DownloadItem downloadItem,
            IJob job,
            JobParameters jobParameters,
            IProtocolProvider protocolProvider);
        
        void RequestCreated(
            IAppContext appContext,
            DownloadItem downloadItem,
            IRequest request,
            IProtocolProvider protocolProvider);
    }
}