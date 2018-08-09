using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKsGear.Architecture.Patterns;
using AMKsGear.Core.Patterns.AppModel;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Core.Api.Binders
{
    /// <summary>
    /// Makes download workflow from application services and DownloadItem.
    /// </summary>
    public static class DownloadBuilder
    {
        /// <summary>
        /// Creates a new instance of DownloadItem from the given Uri.
        /// </summary>
        /// <returns>The DownloadItem.</returns>
        /// <param name="uri">URI.</param>
        public static DownloadItem FromUri(Uri uri)
        {
            var downloadItem = new DownloadItem(uri);

            return downloadItem;
        }
        /// <summary>
        /// Creates a new instance of DownloadItem from the given Uri.
        /// </summary>
        /// <returns>The DownloadItem.</returns>
        /// <param name="uri">URI.</param>
        /// <param name="localFileName">Local path to store the file.</param>
        public static DownloadItem FromUri(Uri uri, string localFileName)
        {
            var downloadItem = new DownloadItem(uri)
            {
                LocalFileName = localFileName
            };
            return downloadItem;
        }

        /// <summary>
        /// Binds the specified application services to DownloadItem and find the best protocol.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="application">App.</param>
        public static IProtocolProvider Bind(DownloadItem downloadItem, IApplicationContext application)
        {
            if (downloadItem == null) throw new ArgumentNullException(nameof(downloadItem));
            if (application == null) throw new ArgumentNullException(nameof(application));

            var protocolProvider = application.GetFeatures<IProtocolProvider>()?
                .FirstOrDefault(p => p.CanHandle(application, downloadItem));

            if (protocolProvider != null)
            {
                var bindListeners = application.GetFeatures<IDownloadBindListener>();
                bindListeners?.ForEach(x => x.NotifyBind(downloadItem, protocolProvider));
            }

            return protocolProvider;
        }

        /// <summary>
        /// Unbinds the specified application services fom DownloadItem and releases any used resources.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="application">App.</param>
        public static void UnBind(DownloadItem downloadItem, IApplicationContext application)
        {


            var bindListeners = application.GetFeatures<IDownloadBindListener>();
            bindListeners.ForEach(x => x.NotifyUnBind(downloadItem));
        }
    }
}