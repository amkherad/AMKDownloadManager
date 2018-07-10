using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Listeners
{
    /// <summary>
    /// Callback listener for download binding.
    /// </summary>
    public interface IDownloadBindListener : IListenerFeature
    {
        /// <summary>
        /// Calls when a binding on DownloadItem occurs.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="protocolProvider">The bound protocol.</param>
        void NotifyBind(DownloadItem downloadItem, IProtocolProvider protocolProvider);

        /// <summary>
        /// Calls when an unbinding on DownloadItem occurs.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        void NotifyUnBind(DownloadItem downloadItem);
    }
}