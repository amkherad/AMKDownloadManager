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
        /// Binds the specified application services to DownloadItem and find the best protocol.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="app">App.</param>
        public static IProtocolProvider Bind(DownloadItem downloadItem, IAppContext app)
        {
            if (downloadItem == null) throw new ArgumentNullException(nameof(downloadItem));
            if (app == null) throw new ArgumentNullException(nameof(app));
            
            var bindListeners = app.GetFeatures<IDownloadBindListener>();
            bindListeners?.ForEach(x => x.NotifyBind(downloadItem));

            var protocolProvider = app.GetFeatures<IProtocolProvider>()?
                .FirstOrDefault(p => p.CanHandle(app, downloadItem));

            return protocolProvider;
        }

        /// <summary>
        /// Unbinds the specified application services fom DownloadItem and releases any used resources.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="app">App.</param>
        public static void UnBind(DownloadItem downloadItem, IAppContext app)
        {


            var bindListeners = app.GetFeatures<IDownloadBindListener>();
            bindListeners.ForEach(x => x.NotifyUnBind(downloadItem));
        }
    }
}