using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.IO;
using AMKsGear.Core.Text;

namespace AMKDownloadManager.Defaults.Messaging
{
    public class HubClient : IHubEndpoint
    {
        public const int BufferSize = HubServer.BufferSize;

        public const int MetaMessageMaxLength = HubServer.MetaMessageMaxLength;
        public const string MetaMessageAcknowledge = HubServer.MetaMessageAcknowledge;
        public const string MetaMessageAcknowledgeAnswer = HubServer.MetaMessageAcknowledgeAnswer;

        public static readonly TimeSpan AcknowledgeInterval = HubServer.ClientAcknowledgeInterval;
        public static readonly TimeSpan ServerDisconnectTimeout = HubServer.ClientDisconnectTimeout;

        public static readonly TimeSpan ServerConnectWait = HubServer.ClientConnectWait;

        public event DataReceivedEventHandler<string> DataReceived;
        public event EventHandler<ClientEventArgs> ServerConnected;
        public event EventHandler<ClientEventArgs> ServerDisconnected;

        public string Name { get; }

        protected AnonymousPipeClientStream ClientInput { get; private set; }
        protected AnonymousPipeClientStream ClientOutput { get; private set; }

        public int ServerProcessId { get; private set; }

        protected byte[] ReadBuffer { get; }
        protected StringBuilder Data { get; }

        private bool _isClosing = false;
        private Encoding _encoding;

        public DateTime LastAcknowledge { get; private set; }

        public IThreadFactory ThreadFactory { get; }


        public HubClient(string name, IThreadFactory threadFactory)
        {
            Name = name;
            ThreadFactory = threadFactory;
            ReadBuffer = new byte[BufferSize];
            Data = new StringBuilder(BufferSize);
            _encoding = Encoding.Unicode;
        }


        public bool IsConnected => ClientInput != null && ClientOutput != null;


        public void Connect() => Connect(Timeout.Infinite);

        public void Connect(int timeout)
        {
            var success = TryConnect(timeout);

            if (success)
            {
                for (;;)
                {
                    try
                    {
                        var ack = _encoding.GetBytes(MetaMessageAcknowledge + MessageHelpers.MessageSeparator);
                        ClientOutput.Write(ack, 0, ack.Length);
                    }
                    catch (Exception ex)
                        when (ex is ObjectDisposedException ||
                              ex is InvalidOperationException || ex is IOException)
                    {
                        //pipe connection problems.
                        if (!PeerErrorDetected(ex))
                        {
                            //try to re-connect.
                            break;
                        }
                    }

                    ThreadFactory.Sleep(AcknowledgeInterval);
                }
            }
        }

        public bool TryConnect(int timeout)
        {
            ClientInput?.Dispose();
            ClientOutput?.Dispose();

            var clientPipe = new NamedPipeClientStream(".", Name, PipeDirection.In);

            clientPipe.Connect();

            if (!clientPipe.IsConnected)
            {
                return false;
            }

            string info = null;

            using (var reader = new StreamReader(clientPipe))
            {
                SpinWait.SpinUntil(() => { return (info = reader.ReadLine()) != null; }, ServerConnectWait);

                Trace.WriteLine($"HubServer said: {info}");
            }

            var readBegan = false;
            if (info != null)
            {
                var parts = info.Split('-');

                if (parts.Length != 3)
                {
                    return false;
                }

                var processId = parts[0].ToInt32();
                var serverReader = parts[1];
                var serverWriter = parts[2];

                ServerProcessId = processId;
                ClientInput = new AnonymousPipeClientStream(PipeDirection.In, serverWriter);
                ClientOutput = new AnonymousPipeClientStream(PipeDirection.Out, serverReader);

                ServerConnected?.Invoke(this, new ClientEventArgs(processId));

                BeginRead();

                clientPipe.Close();
                clientPipe.Dispose();

                return true;
            }

            clientPipe.Close();
            clientPipe.Dispose();

            return false;
        }

        public void BeginRead()
        {
            try
            {
                ClientInput.BeginRead(ReadBuffer, 0, BufferSize,
                    InputPipeBeginReadCallback,
                    this);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                PeerErrorDetected(ex);
            }
        }

        private bool PeerErrorDetected(Exception ex)
        {
            if ((DateTime.Now - LastAcknowledge) > ServerDisconnectTimeout)
            {
                ServerDisconnected?.Invoke(this, new ClientEventArgs(ServerProcessId));
                return false;
            }

            return true;
        }

        protected void InputPipeBeginReadCallback(IAsyncResult result)
        {
            var that = result.AsyncState as HubClient;
            if (that == null) return;

            int bytesRead = 0;
            try
            {
                bytesRead = ClientInput.EndRead(result);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                if (PeerErrorDetected(ex))
                {
                    //PeerErrorDetected let us continue.
                    BeginRead();
                    return;
                }

                //the error is persistence we have to halt the reading.
                //the ack will later know that peer is dead and tries to re-connect.
                return;
            }

            if (bytesRead == 0)
            {
//                if (Data.Length > 0)
//                {
//                    MessageReceived();
//
//                    //message has been read, clearing for new message.
//                    Data.Clear();
//                }

                //Message completely received, now we need to listen to another message.
                BeginRead();
            }
            else
            {
                var data = Data;

                var bytes = ReadBuffer;

                var text = _encoding.GetString(bytes, 0, bytesRead);

                var startIndex = 0;
                var count = 0;
                for (var i = 0; i < bytesRead - 1; i++, count++)
                {
                    if (text[i] == '\r' && text[i + 1] == '\n') //CrLf
                    {
                        if (count > 0)
                        {
                            //appending read bytes to the message.
                            data.Append(text, startIndex, count);
                            count = 0;
                            startIndex = i + 2;
                        }

                        //a message received.
                        if (data.Length > 0)
                        {
                            MessageReceived(data.ToString());

                            //message has been read, clearing for new message.
                            data.Clear();
                        }

                        //skip \n
                        i++;
                    }
                }

                if (count > 0)
                {
                    //appending remaining bytes to the message.
                    data.Append(text, startIndex, count);
                }


                //A chunk of message has been received, now read next chunk.
                BeginRead();
            }
        }

        private void MessageReceived(string message)
        {
            message = MessageHelpers.UnEscapeCrLf(message.TrimEnd('\r', '\n'));

            if (message.Length <= MetaMessageMaxLength)
            {
                switch (message)
                {
                    case MetaMessageAcknowledge: //server is not sending ack right now!
                    {
                        LastAcknowledge = DateTime.Now;
                        break;
                    }
                    case MetaMessageAcknowledgeAnswer:
                    {
                        LastAcknowledge = DateTime.Now;
#if DEBUG
                        Trace.WriteLine($"Acknowledge answer received from client: {ServerProcessId}");
#endif
                        break;
                    }
                }
            }

            DataReceived?.Invoke(this, new DataReceivedEventArgs<string>(message));
        }

        public void Send(string text)
        {
            text = MessageHelpers.EscapeCrLf(text);
            var bytes = _encoding.GetBytes(text + MessageHelpers.MessageSeparator);

            if (ClientOutput == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                ClientOutput.BeginWrite(bytes, 0, bytes.Length, OutputPipeBeginWriteCallback,
                    this);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                PeerErrorDetected(ex);
            }
        }


        private void OutputPipeBeginWriteCallback(IAsyncResult result)
        {
            var that = result.AsyncState as HubClient;
            if (that == null) return;

            ClientOutput.EndWrite(result);

            //NOTHING NOW!
        }
    }
}