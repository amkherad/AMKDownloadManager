using System.IO;
using System.Net;
using AMKsGear.Core.Automation;
using AMKsGear.Web.Core.Http;

namespace AMKDownloadManager.Core.Api.Transport
{
    public class HttpResponse : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HeaderCollection Headers { get; }
        public HeaderCookieCollection Cookies { get; }
        public Stream ResponseStream { get; set; }
        public IDisposer Disposer { get; }

        public HttpResponse()
        {
            Disposer = new Disposer();
            
            Headers = new HeaderCollection();
            Cookies = new HeaderCookieCollection(Headers);
        }

        public void Dispose() => Disposer.Dispose();
    }
}