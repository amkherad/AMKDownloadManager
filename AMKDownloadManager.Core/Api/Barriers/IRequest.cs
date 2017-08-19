﻿using System;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Core.Api.Barriers
{
    /// <summary>
    /// I request.
    /// </summary>
	public interface IRequest
	{
        NameValueCollection Headers { get; }
        NameValueCollection Cookies { get; }
        NameValueCollection FormData { get; }
        NameValueCollection QueryString { get; }
        NameValueCollection QueryString { get; }
	}
}