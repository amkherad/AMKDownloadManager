using System;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public interface IJob : IFeature
    {
        event EventHandler Finished;
        event EventHandler Progress;
        event EventHandler Paused;
        event EventHandler Started;

        IJobChunk StartChunk();
        void Pause(IJobChunk[] chunks);

        void Clean(IJobChunk[] chunks);
    }
}