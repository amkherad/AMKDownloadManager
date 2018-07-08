using System;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Job part state.
    /// </summary>
    public enum JobPartState
    {
        RequestMoreCycle,
        ErrorCanRetry,
        Finished,
        Error,
    }

    /// <summary>
    /// A single part of download progress job.
    /// </summary>
    public interface IJobPart : IDisposable
    {
        /// <summary>
        /// Provides access to master job which this job part is derived from.
        /// </summary>
        IJob Job { get; }

        /// <summary>
        /// Gives an execution cycle to job part to finish it's job. if RequestMoreCycle returned it schedules for one more cycle.
        /// </summary>
        JobPartState Cycle();

        /// <summary>
        /// Notifies for ThreadAbortException.
        /// </summary>
        void NotifyAbort();

#if DEBUG
        string DebugName { get; set; }
        
        string GetDebugName();
#endif
    }
}