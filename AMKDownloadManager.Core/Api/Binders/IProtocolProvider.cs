using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.Core.Api.Types;

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

        /// <summary>
        /// Creates a <see cref="IRequest"/> for this protocol.
        /// </summary>
        /// <param name="appContext">The AppContext</param>
        /// <param name="downloadItem">The download info which the request is for</param>
        /// <param name="segmentationContext">A nullable segmentation context.</param>
        /// <param name="segment">A segment to determine if request is partial or is for whole resource</param>
        /// <param name="parameters">Extended request parameters (i.e. caching, ...)</param>
        /// <returns></returns>
        IRequest CreateRequest(
            IAppContext appContext,
            DownloadItem downloadItem,
            SegmentationContext segmentationContext,
            Segment segment,
            RequestParameters parameters
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