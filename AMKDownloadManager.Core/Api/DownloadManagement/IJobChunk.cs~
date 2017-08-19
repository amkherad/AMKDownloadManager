using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Job chunk state.
    /// </summary>
    public enum JobChunkState
    {
        RequestMoreCycle,
        ErrorCanTry,
        Finished,
        Error,
    }

    /// <summary>
    /// A single chunk of download progress job.
    /// </summary>
    public interface IJobChunk
    {
        /// <summary>
        /// Gives an execution cycle to job chunk to finish it's job. if RequestMoreCycle returned it schedules for one more cycle.
        /// </summary>
        JobChunkState Cycle();

        /// <summary>
        /// Yield this instance.
        /// </summary>
        void Yield();
    }
}