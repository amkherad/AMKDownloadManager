using System;
using System.Collections.Generic;
using System.Net;

namespace AMKDownloadManager.Core.Api.Transport
{
    public class HttpProxyDescriptor
    {
        public Uri Uri { get; }
        public bool BypassOnLocal { get; set; } = true;
        public IEnumerable<string> BypassList { get; set; }
        
        public ICredentials Credentials { get; }
        
        public HttpProxyDescriptor(int port)
        {
            Uri = new Uri($"http://localhost:{port}/");
        }
        public HttpProxyDescriptor(string uri)
        {
            Uri = new Uri(uri);
        }
        public HttpProxyDescriptor(Uri uri)
        {
            Uri = uri;
        }
        public HttpProxyDescriptor(Uri uri, ICredentials credentials)
        {
            Uri = uri;
            Credentials = credentials;
        }
    }
}