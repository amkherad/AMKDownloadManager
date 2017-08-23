using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.HttpDownloader.DownloadManagement;

namespace AMKDownloadManager.HttpDownloader.ProtocolProvider
{
    public class HttpProtocolProvider : IProtocolProvider
    {
        public static string[] SupportedProtocols = new []
        {
            "http",
            "https"
        };

        public HttpProtocolProvider()
        {
        }

        #region IProtocolProvider implementation

        public bool CanHandle(IAppContext appContext, DownloadItem downloadItem)
        {
            if (downloadItem == null)
                throw new ArgumentNullException(nameof(downloadItem));

            var uri = downloadItem.Uri;
            if (uri == null)
            {
                return false;
            }

            var scheme = uri.Scheme;
            if (string.IsNullOrWhiteSpace(scheme) || SupportedProtocols.Any(p => p == scheme.ToLower()))
            {
                return true;
            }

            return false;
        }

        public IRequest CreateRequest(IAppContext appContext, DownloadItem downloadItem)
        {
            var request = HttpRequest.FromDownloadItem(
                appContext,
                appContext.GetFeature<IConfigProvider>(),
                downloadItem);

            appContext.SignalFeatures<IProtocolProviderListener>(x => x.RequestCreated(
                appContext,
                downloadItem,
                request,
                this
                ));
            
            return request;
        }

        public IJob CreateJob(IAppContext appContext, DownloadItem downloadItem, JobParameters jobParameters)
        {
            var httpDownload = new HttpDownloadJob(appContext, this, downloadItem, jobParameters);

            appContext.SignalFeatures<IProtocolProviderListener>(x => x.JobCreated(
                appContext,
                downloadItem,
                httpDownload,
                jobParameters,
                this
            ));
            
            return httpDownload;
        }

        public int Order => 0;

        #endregion
    }
}