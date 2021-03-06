﻿using System;
using System.IO;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Automation;
using AMKsGear.Core.Collections;
using AMKsGear.Web.Core.Http;

namespace AMKDownloadManager.Core.Api.Transport
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
		
		IDisposableContainer Disposer { get; }
	}
}