using System;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Listeners;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public delegate void JobEventHandler(IJob job, EventArgs eventArgs);
    public delegate void JobProgressEventHandler(IJob job, int progress);
    public delegate void JobPriorityChangedEventHandler(IJob job, SchedulerPriority priority);
    public delegate void JobStateChangedEventHandler(IJob job, JobState state);

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

        event JobStateChangedEventHandler StateChanged;
        
        
        JobState State { get; }
        

        JobInfo TriggerJobAndGetInfo(IDownloadProgressListener downloadProgressListener);
        Task<JobInfo> TriggerJobAndGetInfoAsync(IDownloadProgressListener downloadProgressListener);

        IJobChunk GetJobChunk(IDownloadProgressListener downloadProgressListener);

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