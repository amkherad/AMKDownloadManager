using System;
using AMKDownloadManager.Core.Api.Barriers;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public enum JobChunkState
    {
        RequestMoreCycle,
        Finished,
        Error,
    }

    public interface IJobChunk
    {
        JobChunkState Cycle();
        void Yield();
    }
}