﻿using System;
using System.IO;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Automation;
using AMKsGear.Core.Automation.LifetimeManagers;
using AMKsGear.Core.Collections;
using AMKsGear.Web.Core.Http;

namespace AMKDownloadManager.Core.Api.Transport
{
    public class HttpRequest : IRequest
    {
        public DownloadItem DownloadItem { get; }

        private System.Net.HttpWebRequest _request;

        protected HttpRequest(DownloadItem downloadItem)
        {
            Disposer = new DisposableContainer();
            
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
        public IDisposableContainer Disposer { get; }


        public static HttpRequest FromDownloadItem(
            IApplicationContext applicationContext,
            DownloadItem downloadItem)
        {
            var configProvider = applicationContext.GetFeature<IConfigProvider>();
            
            var req = new HttpRequest(downloadItem)
            {
                Uri = downloadItem.Uri,
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