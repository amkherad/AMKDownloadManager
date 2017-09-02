using System;
using System.Threading.Tasks;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.Listeners;
using ir.amkdp.gear.core.Automation;

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
        JobInfo TriggerJobAndGetInfo(/*IDownloadProgressListener downloadProgressListener*/);
        // <param name="downloadProgressListener">Listener to listen to progression information.</param>
        /// <summary>
        /// Sends a get request to the target, if resource supports ranges returns immediately after response,
        /// if resouse does not support ranges this method will freeze until download progress finishes.
        /// </summary>
        /// <returns></returns>
        Task<JobInfo> TriggerJobAndGetInfoAsync(/*IDownloadProgressListener downloadProgressListener*/);

        // <param name="downloadProgressListener">Listener to listen to progression information.</param>
        /// <summary>
        /// Gets a new chunk.
        /// </summary>
        /// <returns></returns>
        IJobChunk GetJobChunk(JobInfo jobInfo/*IDownloadProgressListener downloadProgressListener*/);
        
        void Clean();
        void Reset();
    }

    public class JobInfo : IDisposable 
    {
        /// <summary>
        /// Gets the size of the download if available.
        /// </summary>
        /// <value>The size of the download.</value>
        public long? DownloadSize { get; }

        /// <summary>
        /// Gets the size of the first request content if available.
        /// </summary>
        public long? FirstHttpPacketSize { get; }
        
        public Action TriggerJobAndGetInfoLateAction { get; } 
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="AMKDownloadManager.Core.Api.DownloadManagement.IJob"/>
        /// supports concurrency.
        /// </summary>
        /// <value><c>true</c> if supports concurrency; otherwise, <c>false</c>.</value>
        public bool SupportsConcurrency { get; }

        /// <summary>
        /// Gets response of first request. if available.
        /// </summary>
        public IResponse Response { get; }

        public IDisposer Disposer { get; }
        
        /// <summary>
        /// Gets a value indicating whether this job is finished. typically becomes true when job does not support concurrency. (resume capability)
        /// </summary>
        //public bool IsFinished { get; set; }

        /// <summary>
        /// JobInfo constructor
        /// </summary>
        /// <param name="downloadSize">Size of download resource, (if available)</param>
        /// <param name="firstHttpPacketSize">Size of first request's response content</param>
        /// <param name="supportsConcurrency">Determines download resource supports concurrency (resume capability)</param>
        /// <param name="response">IResponse if available</param>
        ///// <param name="isFinished">Determines if download is finished or not</param>
        public JobInfo(
            long? downloadSize,
            long? firstHttpPacketSize,
            bool supportsConcurrency,
            IResponse response,
            //bool isFinished,
            Action triggerJobAndGetInfoLateAction)
        {
            Disposer = new Disposer();
            
            DownloadSize = downloadSize;
            FirstHttpPacketSize = firstHttpPacketSize;
            SupportsConcurrency = supportsConcurrency;
            Response = response;
            //IsFinished = isFinished;
            TriggerJobAndGetInfoLateAction = triggerJobAndGetInfoLateAction;
        }

        public void Dispose()
        {
            Disposer.Dispose();
        }
    }
}