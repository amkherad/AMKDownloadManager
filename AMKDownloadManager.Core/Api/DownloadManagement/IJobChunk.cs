using System;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Job chunk state.
    /// </summary>
    public enum JobChunkState
    {
        RequestMoreCycle,
        ErrorCanRetry,
        Finished,
        Error,
    }

    /// <summary>
    /// A single chunk of download progress job.
    /// </summary>
    public interface IJobChunk : IDisposable
    {
        /// <summary>
        /// Gives an execution cycle to job chunk to finish it's job. if RequestMoreCycle returned it schedules for one more cycle.
        /// </summary>
        JobChunkState Cycle();

        /// <summary>
        /// Notifies for ThreadAbortException.
        /// </summary>
        void NotifyAbort();
    }
}