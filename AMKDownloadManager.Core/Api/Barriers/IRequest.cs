using System;
using System.IO;
using ir.amkdp.gear.core.Automation;
using ir.amkdp.gear.core.Collections;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.Core.Api.Barriers
{
    /// <summary>
    /// Request.
    /// </summary>
	public interface IRequest : IDisposable
	{
		Uri Uri { get; }
		
		HeaderCollection Headers { get; }
        HeaderCookieCollection Cookies { get; }
        NameObjectCollection FormData { get; }
        NameStringCollection QueryString { get; }
        byte[] RequestBody { get; set; }
		Action<Stream> RequestBodyWriter { get; set; }
		string Method { get; set; }
		
		IDisposer Disposer { get; }
	}
}