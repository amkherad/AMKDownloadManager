using System;
using System.IO;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using ir.amkdp.gear.core.Collections;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.HttpDownloader.DownloadManagement
{
    public class HttpRequest : IRequest
    {
        public DownloadItem DownloadItem { get; }

        private System.Net.HttpWebRequest _request;

        protected HttpRequest(DownloadItem downloadItem)
        {
            DownloadItem = downloadItem;
        }


        public Uri Uri { get; protected set; }
        public HeaderCollection Headers { get; }
        public HeaderCookieCollection Cookies { get; }
        public NameObjectCollection FormData { get; }
        public NameStringCollection QueryString { get; }
        public byte[] RequestBody { get; set; }
        public Action<Stream> RequestBodyWriter { get; set; }
        public string Method { get; set; }


        public static HttpRequest FromDownloadItem(
            IAppContext appContext,
            IConfigProvider configProvider,
            DownloadItem downloadItem)
        {
            var req = new HttpRequest(downloadItem)
            {
                Uri = downloadItem.Uri
            };

            req.Headers.ContentType = downloadItem.Properties[HeaderCollection.ContentTypeHeaderName] as string;
            req.Method = downloadItem.Properties[DownloadItem.KnownProperties.Method] as string ??
                         configProvider.GetString(
                             KnownConfigs.DownloadManager.RequestMethod,
                             KnownConfigs.DownloadManager.RequestMethod_DefaultValue);
            
            return req;
        }
    }
}