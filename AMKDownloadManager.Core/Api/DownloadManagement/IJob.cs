using System;
using System.Threading.Tasks;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public delegate void JobEventHandler(IJob job, EventArgs eventArgs);
    public delegate void JobProgressEventHandler(IJob job, int progress);
    public delegate void JobPriorityChangedEventHandler(IJob job, SchedulerPriority priority);

    /// <summary>
    /// Download job.
    /// </summary>
    public interface IJob : IFeature
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


        JobInfo TriggerJobAndGetInfo();
        Task<JobInfo> TriggerJobAndGetInfoAsync();

        IJobChunk GetJobChunk();

        void Clean();
        void Reset();
    }

    public class JobInfo 
    {
        /// <summary>
        /// Gets the size of the download if available.
        /// </summary>
        /// <value>The size of the download.</value>
        public int? DownloadSize { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AMKDownloadManager.Core.Api.DownloadManagement.IJob"/>
        /// supports concurrency.
        /// </summary>
        /// <value><c>true</c> if supports concurrency; otherwise, <c>false</c>.</value>
        public bool SupportsConcurrency { get; }
    }
}