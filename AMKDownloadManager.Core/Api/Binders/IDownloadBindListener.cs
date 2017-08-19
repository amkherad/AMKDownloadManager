﻿using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.Binders
{
    /// <summary>
    /// Callback listener for download binding.
    /// </summary>
    public interface IDownloadBindListener : IFeature
    {
        /// <summary>
        /// Calls when a binding on DownloadItem occures.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        void NotifyBind(DownloadItem downloadItem);

        /// <summary>
        /// Calls when an unbinding on DownloadItem occures.
        /// </summary>
        /// <param name="downloadItem">Download item.</param>
        void NotifyUnBind(DownloadItem downloadItem);
    }
}