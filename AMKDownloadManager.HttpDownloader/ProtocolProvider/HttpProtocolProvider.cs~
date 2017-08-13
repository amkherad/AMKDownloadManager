using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;

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

        public bool CanHandle(DownloadItem downloadItem)
        {
            if (downloadItem == null)
                throw new ArgumentNullException(nameof(downloadItem));

            var uri = downloadItem.Uri;
            if (uri == null)
            {
                return false;
            }

            var scheme = uri.Scheme;
            if (String.IsNullOrWhiteSpace(scheme) || SupportedProtocols.Any(p => p == scheme.ToLower()))
            {
                return true;
            }
        }

        public IJob CreateJob(DownloadItem downloadItem, IAppContext app, JobParameters jobParameters)
        {
            
        }

        #endregion
    }
}