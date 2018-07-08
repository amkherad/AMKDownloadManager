using System;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKsGear.Core.Automation;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Job state.
    /// </summary>
    public enum JobState
    {
        Stopped,
        Paused,
        Preparing,
        GettingJobInfo,
        Downloading,
        Finished
    }

    /// <summary>
    /// Download job.
    /// </summary>
    public interface IJob : IDisposable
    {
        /// <summary>
        /// Get job state.
        /// </summary>
        JobState State { get; }

        /// <summary>
        /// Gets DownloadItem.
        /// </summary>
        DownloadItem DownloadItem { get; }

        // <param name="downloadProgressListener">Listener to listen to progression information.</param>
        /// <summary>
        /// Sends a get request to the target, if resource supports ranges returns immediately after response,
        /// if resouse does not support ranges this method will freeze until download progress finishes.
        /// </summary>
        /// <returns></returns>
        JobInfo TriggerJobAndGetInfo(/*IDownloadProgressListener downloadProgressListener*/);

        // <param name="downloadProgressListener">Listener to listen to progression information.</param>
        /// <summary>
        /// Sends a get request to the target, if resource supports ranges returns immediately after response,
        /// if resouse does not support ranges this method will freeze until download progress finishes.
        /// </summary>
        /// <returns></returns>
        Task<JobInfo> TriggerJobAndGetInfoAsync( /*IDownloadProgressListener downloadProgressListener*/);

        // <param name="downloadProgressListener">Listener to listen to progression information.</param>
        /// <summary>
        /// Gets a new part.
        /// </summary>
        /// <returns></returns>
        IJobPart GetJobPart(JobInfo jobInfo /*IDownloadProgressListener downloadProgressListener*/);

        void Clean();
        void Reset();

#if DEBUG
        string GetDebugName();
#endif
    }
}