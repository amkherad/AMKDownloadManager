﻿using System;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Binders
{
    /// <summary>
    /// Service to create download workflow from DownloadItem.
    /// </summary>
    public interface IProtocolProvider : IFeature
    {
        /// <summary>
        /// Determines whether this instance can handle the specified downloadItem.
        /// </summary>
        /// <returns><c>true</c> if this instance can handle the specified downloadItem; otherwise, <c>false</c>.</returns>
        /// <param name="appContext">The <see cref="IAppContext"/></param>
        /// <param name="downloadItem">Download item</param>
        bool CanHandle(IAppContext appContext, DownloadItem downloadItem);

        IRequest CreateRequest(
            IAppContext appContext,
            DownloadItem downloadItem,
            SegmentationContext segmentationContext,
            Segment segment
        );

        /// <summary>
        /// Creates the download job.
        /// </summary>
        /// <returns>The job.</returns>
        /// <param name="app">App.</param>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="fileProvider">File provider.</param>
        /// <param name="jobParameters">Job parameters.</param>
        IJob CreateJob(
            IAppContext app,
            DownloadItem downloadItem,
            IFileProvider fileProvider,
            JobParameters jobParameters);
    }
}