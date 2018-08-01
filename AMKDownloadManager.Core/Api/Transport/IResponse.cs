using System;
using System.IO;
using System.Net;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Automation;
using AMKsGear.Web.Core.Http;

namespace AMKDownloadManager.Core.Api.Transport
{
    public interface IResponse : IDisposable
    {
        HttpStatusCode StatusCode { get; }
        
        HeaderCollection Headers { get; }
        HeaderCookieCollection Cookies { get; }
        
        Stream ResponseStream { get; }
        
        IDisposableContainer Disposer { get; }
    }
}