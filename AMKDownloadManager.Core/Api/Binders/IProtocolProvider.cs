using System;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;

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
        /// <param name="downloadItem">Download item.</param>
        bool CanHandle(DownloadItem downloadItem);

        /// <summary>
        /// Creates the download job.
        /// </summary>
        /// <returns>The job.</returns>
        /// <param name="downloadItem">Download item.</param>
        /// <param name="app">App.</param>
        /// <param name="jobParameters">Job parameters.</param>
        IJob CreateJob(DownloadItem downloadItem, IAppContext app, JobParameters jobParameters);
    }
}