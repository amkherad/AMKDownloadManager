using System;

namespace AMKDownloadManager.Defaults.Messaging
{
    public class ClientEventArgs : EventArgs
    {
        public int ProcessId { get; }


        public ClientEventArgs(int processId)
        {
            ProcessId = processId;
        }
    }
}