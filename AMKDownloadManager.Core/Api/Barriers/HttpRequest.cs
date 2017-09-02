using System;
using System.IO;
using ir.amkdp.gear.core.Automation;
using ir.amkdp.gear.core.Collections;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public class HttpRequest : IRequest
    {
        public DownloadItem DownloadItem { get; }

        private System.Net.HttpWebRequest _request;

        protected HttpRequest(DownloadItem downloadItem)
        {
            Disposer = new Disposer();
            
            DownloadItem = downloadItem;
            
            Headers = new HeaderCollection();
            Cookies = new HeaderCookieCollection(Headers);
            FormData = new NameObjectCollection();
            QueryString = new NameStringCollection();
        }


        public Uri Uri { get; protected set; }
        public HeaderCollection Headers { get; }
        public HeaderCookieCollection Cookies { get; }
        public NameObjectCollection FormData { get; }
        public NameStringCollection QueryString { get; }
        public byte[] RequestBody { get; set; }
        public Action<Stream> RequestBodyWriter { get; set; }
        public string Method { get; set; }
        public IDisposer Disposer { get; }


        public static HttpRequest FromDownloadItem(
            IAppContext appContext,
            DownloadItem downloadItem)
        {
            var configProvider = appContext.GetFeature<IConfigProvider>();
            
            var req = new HttpRequest(downloadItem)
            {
                Uri = downloadItem.Uri
            };

            req.Headers.ContentType = downloadItem.Properties[HeaderCollection.ContentTypeHeaderName] as string;
            req.Method = downloadItem.Properties[DownloadItem.KnownProperties.Method] as string ??
                         configProvider.GetString(
                             KnownConfigs.DownloadManager.Download.RequestMethod,
                             KnownConfigs.DownloadManager.Download.RequestMethodDefaultValue);
            
            return req;
        }

        public void Dispose()
        {
            Disposer.Dispose();
            DownloadItem.Dispose();
        }
    }

    public static class RequestExtensions
    {
        public static bool IsGetRequest(this IRequest request)
        {
            return request.Method?.ToLower() == "get";
        }
        public static bool IsPostRequest(this IRequest request)
        {
            return request.Method?.ToLower() == "post";
        }
    }
}