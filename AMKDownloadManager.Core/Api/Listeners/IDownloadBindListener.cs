using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Listeners
{
    /// <summary>
    /// Callback listener for download binding.
    /// </summary>
    public interface IDownloadBindListener : IListenerFeature
    {
        /// <summary>
        /// Calls when a binding on DownloadItem occures.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        void NotifyBind(DownloadItem downloadItem);

        /// <summary>
        /// Calls when an unbinding on DownloadItem occures.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        void NotifyUnBind(DownloadItem downloadItem);
    }
}