using System;
using System.IO.Pipes;
using System.Text;

namespace AMKDownloadManager.Defaults.Messaging
{
    public class ConnectionContext : IDisposable
    {
        public int ProcessId { get; }

        public StringBuilder Data { get; }
        public byte[] ReadBuffer { get; }
        public PipeStream InputPipe { get; }
        public PipeStream OutputPipe { get; }

        public DateTime LastAcknowledge { get; set; }


        public ConnectionContext(PipeStream inputPipe, PipeStream outputPipe)
        {
            InputPipe = inputPipe;
            OutputPipe = outputPipe;

            Data = new StringBuilder(HubServer.BufferSize);
            ReadBuffer = new byte[HubServer.BufferSize];

            ProcessId = 0;
        }

        public void Dispose()
        {
            InputPipe?.Dispose();
            OutputPipe?.Dispose();
        }
    }
}