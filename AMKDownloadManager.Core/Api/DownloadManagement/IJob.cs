using System;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;
using AMKDownloadManager.Core.Api.Transport;
using AMKsGear.Core.Automation;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public delegate void JobEventHandler(IJob job, EventArgs eventArgs);

    public delegate void JobProgressEventHandler(IJob job, long progress);

    public delegate void JobPriorityChangedEventHandler(IJob job, SchedulerPriority priority);

    public delegate void JobStateChangedEventHandler(IJob job, JobState state);

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
        /// Occurs when finished.
        /// </summary>
        event JobEventHandler Finished;

        /// <summary>
        /// Occurs when paused.
        /// </summary>
        event JobEventHandler Paused;

        /// <summary>
        /// Occurs when started.
        /// </summary>
        event JobEventHandler Started;

        /// <summary>
        /// Occurs when any download progress happens.
        /// </summary>
        event JobProgressEventHandler Progress;

        /// <summary>
        /// Occurs when priority changed.
        /// </summary>
        event JobPriorityChangedEventHandler PriorityChanged;

        /// <summary>
        /// Occurs when job state changed.
        /// </summary>
        event JobStateChangedEventHandler StateChanged;

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
        JobInfo TriggerJobAndGetInfo( /*IDownloadProgressListener downloadProgressListener*/);

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