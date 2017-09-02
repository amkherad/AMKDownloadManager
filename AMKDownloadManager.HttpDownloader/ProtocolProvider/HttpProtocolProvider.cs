using System;
using System.IO;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.HttpDownloader.DownloadManagement;

namespace AMKDownloadManager.HttpDownloader.ProtocolProvider
{
    public class HttpProtocolProvider : IProtocolProvider
    {
        public static string[] SupportedProtocols = new[]
        {
            "http",
            "https"
        };

        private string _defaultMethod = "GET";

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

        public IRequest CreateRequest(
            IAppContext appContext,
            DownloadItem downloadItem,
            SegmentationContext segmentationContext,
            Segment segment)
        {
            var request = HttpRequest.FromDownloadItem(
                appContext,
                downloadItem);

            if (segment != null)
            {
                
            }

            appContext.SignalFeatures<IProtocolProviderListener>(x => x.RequestCreated(
                appContext,
                downloadItem,
                request,
                this
            ));

            request.Method = _defaultMethod;

            return request;
        }

        public IJob CreateJob(IAppContext appContext, DownloadItem downloadItem, JobParameters jobParameters)
        {
            var fileProvider = appContext.GetFeature<IFileProvider>();

            var fileManager = fileProvider.CreateFile(
                appContext,
                downloadItem.LocalFileName ??
                (downloadItem.Uri == null ? null : Path.GetFileName(downloadItem.Uri.AbsolutePath)),
                null,
                null
            );
            
            var httpDownload = new HttpDownloadJob(
                appContext,
                fileManager,
                this,
                downloadItem,
                jobParameters
            );

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

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider)
        {
        }

        #endregion
    }
}