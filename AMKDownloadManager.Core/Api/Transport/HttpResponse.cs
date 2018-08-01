using System.IO;
using System.Net;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Automation;
using AMKsGear.Core.Automation.LifetimeManagers;
using AMKsGear.Web.Core.Http;

namespace AMKDownloadManager.Core.Api.Transport
{
    public class HttpResponse : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HeaderCollection Headers { get; }
        public HeaderCookieCollection Cookies { get; }
        public Stream ResponseStream { get; set; }
        public IDisposableContainer Disposer { get; }

        public HttpResponse()
        {
            Disposer = new DisposableContainer();
            
            Headers = new HeaderCollection();
            Cookies = new HeaderCookieCollection(Headers);
        }

        public void Dispose() => Disposer.Dispose();
    }
}