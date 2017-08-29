using System.IO;
using System.Net;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public class HttpResponse : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HeaderCollection Headers { get; }
        public HeaderCookieCollection Cookies { get; }
        public Stream ResponseStream { get; set; }

        public HttpResponse()
        {
            Headers = new HeaderCollection();
            Cookies = new HeaderCookieCollection(Headers);
        }
    }
}