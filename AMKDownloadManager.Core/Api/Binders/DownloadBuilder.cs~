using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using ir.amkdp.gear.arch.Patterns;
using ir.amkdp.gear.core.Patterns.AppModel;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Core.Api.Binders
{
    public static class DownloadBuilder
    {
        public static DownloadItem FromUri(Uri uri)
        {
            var downloadItem = new DownloadItem
            {
                Uri = uri
            };

            return downloadItem;
        }

        public static IProtocolProvider Bind(DownloadItem downloadItem, IAppContext app)
        {
            var bindListeners = app.GetFeatures<IDownloadBindListener>();
            bindListeners.ForEach(x => x.NotifyBind(downloadItem));

            var protocolProvider = app.GetFeatures<IProtocolProvider>()
                .FirstOrDefault(p => p.CanHandle(downloadItem));

            return protocolProvider;
        }

        public static void UnBind(DownloadItem downloadItem, IAppContext app)
        {


            var bindListeners = app.GetFeatures<IDownloadBindListener>();
            bindListeners.ForEach(x => x.NotifyUnBind(downloadItem));
        }
    }
}