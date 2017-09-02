using System;
using System.IO;
using System.Net;
using ir.amkdp.gear.core.Automation;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.Core.Api.Barriers
{
    public interface IResponse : IDisposable
    {
        HttpStatusCode StatusCode { get; }
        
        HeaderCollection Headers { get; }
        HeaderCookieCollection Cookies { get; }
        
        Stream ResponseStream { get; }
        
        IDisposer Disposer { get; }
    }
}