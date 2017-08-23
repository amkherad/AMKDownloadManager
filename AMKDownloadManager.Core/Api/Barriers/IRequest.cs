using System;
using System.IO;
using ir.amkdp.gear.core.Collections;
using ir.amkdp.gear.web.Http;

namespace AMKDownloadManager.Core.Api.Barriers
{
    /// <summary>
    /// I request.
    /// </summary>
	public interface IRequest
	{
		Uri Uri { get; }
		
		HeaderCollection Headers { get; }
        HeaderCookieCollection Cookies { get; }
        NameObjectCollection FormData { get; }
        NameStringCollection QueryString { get; }
        byte[] RequestBody { get; set; }
		Action<Stream> RequestBodyWriter { get; set; } 
		string Method { get; set; }
	}
}