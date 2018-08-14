using System;

namespace AMKDownloadManager.Defaults.Messaging
{
    public interface IHub : IDisposable
    {
        void JoinHub();

        void Send(string name, object state);
    }
}