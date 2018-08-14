using System;

namespace AMKDownloadManager.Defaults.Messaging
{
    public class ServerEventArgs : EventArgs
    {
        public int ProcessId { get; }


        public ServerEventArgs(int processId)
        {
            ProcessId = processId;
        }
    }
}