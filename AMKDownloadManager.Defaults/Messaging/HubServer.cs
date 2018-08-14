using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.IO;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.Messaging
{
    public class HubServer : IHubEndpoint, IDisposable
    {
        public const int BufferSize = Helper.KiB;

        public const int MetaMessageMaxLength = 3;
        public const string MetaMessageAcknowledge = "ACK";
        public const string MetaMessageAcknowledgeAnswer = "ACA";

        public static readonly TimeSpan ClientAcknowledgeInterval = TimeSpan.FromSeconds(3);
        public static readonly TimeSpan ClientDisconnectTimeout = TimeSpan.FromSeconds(9);

        public static readonly TimeSpan ClientConnectWait = TimeSpan.FromSeconds(3);

        public event DataReceivedEventHandler<string> DataReceived;
        public event EventHandler<ClientEventArgs> ClientConnected;
        public event EventHandler<ClientEventArgs> ClientDisconnected;

        public string Name { get; }
        public string LockName { get; }

        public List<ConnectionContext> Connections { get; }
        private ReaderWriterLockSlim _connectionsLock;
        private bool _isClosing = false;
        private Encoding _encoding;

        protected IInterProcessLockService InterProcessLock { get; }

        public IThreadFactory ThreadFactory { get; }


        public HubServer(string name, string lockName, IInterProcessLockService interProcessLock, IThreadFactory threadFactory)
        {
            Name = name;
            LockName = lockName;
            InterProcessLock = interProcessLock;
            ThreadFactory = threadFactory;
            Connections = new List<ConnectionContext>();
            _connectionsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _encoding = Encoding.Unicode;
        }


        public void Listen()
        {
            NamedPipeServerStream serverPipe = null;
            while (!_isClosing)
            {
                var lockTaken = InterProcessLock.AcquireLock(LockName);
                try
                {
                    if (serverPipe == null)
                    {
                        serverPipe = new NamedPipeServerStream(Name, PipeDirection.Out, 1,
                            PipeTransmissionMode.Byte,
                            PipeOptions.Asynchronous);
                    }

#if DEBUG
                    Trace.WriteLine("Waiting for named pipe connection.");
#endif

                    //this method should be blocking, cannot use BeginWaitForConnection.
                    serverPipe.WaitForConnection();

#if DEBUG
                    Trace.WriteLine("Named pipe connection established.");
#endif

                    _handleConnection(serverPipe);
                }
                finally
                {
                    if (lockTaken != null)
                        InterProcessLock.ReleaseLock(lockTaken);
                }
            }
        }

        private void _handleConnection(NamedPipeServerStream serverPipe)
        {
            var reader = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.None);
            var writer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.None);

            var readerClientHandle = reader.GetClientHandleAsString();
            var writerClientHandle = writer.GetClientHandleAsString();

            var info = $"{Process.GetCurrentProcess().Id}-{readerClientHandle}-{writerClientHandle}";

#if DEBUG
            Trace.WriteLine(
                $"Messaging channel established with info: pid:{Process.GetCurrentProcess().Id}, readerClientHandle:{readerClientHandle}, writerClientHandle:{writerClientHandle}");
#endif

            var streamWriter = new StreamWriter(serverPipe)
            {
                AutoFlush = true
            };
            streamWriter.WriteLine(info);
            //streamWriter.Flush();

            serverPipe.Flush();

            SpinWait.SpinUntil(() =>
            {
                //streamWriter.WriteLine(info);
                return reader.IsConnected && writer.IsConnected;
            }, ClientConnectWait);

            //serverPipe.WaitForPipeDrain();
            //serverPipe.Close(); //re-use

            //disconnecting client from named pipe to listen to another client.
            serverPipe.Disconnect();

            var connection = new ConnectionContext(reader, writer);

            _connectionsLock.EnterWriteLock();
            try
            {
                Connections.Add(connection);
            }
            finally
            {
                _connectionsLock.ExitWriteLock();
            }

            //now read client messages.
            BeginRead(connection);
        }

        public void BeginRead(ConnectionContext connection)
        {
            try
            {
                connection.InputPipe.BeginRead(connection.ReadBuffer, 0, BufferSize,
                    ConnectionInputPipeBeginReadCallback,
                    connection);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                PeerErrorDetected(ex, connection);
            }
        }

        protected void ConnectionInputPipeBeginReadCallback(IAsyncResult result)
        {
            var connection = result.AsyncState as ConnectionContext;
            if (connection == null) return;

            var data = connection.Data;

            int bytesRead = 0;
            try
            {
                bytesRead = connection.InputPipe.EndRead(result);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                if (PeerErrorDetected(ex, connection))
                {
                    //PeerErrorDetected let us continue.
                    BeginRead(connection);
                    return;
                }
            }

            if (bytesRead == 0)
            {
//                if (data.Length > 0)
//                {
//                    MessageReceived(connection);
//
//                    //message has been read, clearing for new message.
//                    data.Clear();
//                }

                //Message completely received, now we need to listen to another message.
                BeginRead(connection);
            }
            else
            {
                var bytes = connection.ReadBuffer;

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
                            MessageReceived(data.ToString(), connection);

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
                BeginRead(connection);
            }
        }

        public void MessageReceived(string message, ConnectionContext connection)
        {
            message = MessageHelpers.UnEscapeCrLf(message.TrimEnd('\r', '\n'));

            if (message.Length <= MetaMessageMaxLength)
            {
                switch (message)
                {
                    case MetaMessageAcknowledge:
                    {
                        connection.LastAcknowledge = DateTime.Now;
#if DEBUG
                        Trace.WriteLine($"Acknowledge received from client: {connection.ProcessId}");
#endif
                        SendToWithoutSeparatorUnEscaped(MetaMessageAcknowledgeAnswer + MessageHelpers.MessageSeparator,
                            connection);
                        break;
                    }
                    case MetaMessageAcknowledgeAnswer:
                    {
                        connection.LastAcknowledge = DateTime.Now;
#if DEBUG
                        Trace.WriteLine($"Acknowledge answer received from client: {connection.ProcessId}");
#endif
                        break;
                    }
                }
            }

            DataReceived?.Invoke(this, new DataReceivedEventArgs<string>(message));

            //DataReceived event triggered, now we need to notify other clients in the hub.

            ConnectionContext[] connections;
            _connectionsLock.EnterReadLock();
            try
            {
                connections = Connections.Where(c => c != connection)
                    .ToArray();
            }
            finally
            {
                _connectionsLock.ExitReadLock();
            }

            message = MessageHelpers.EscapeCrLf(message) + MessageHelpers.MessageSeparator;
            foreach (var c in connections)
            {
                SendToWithoutSeparatorUnEscaped(message, c);
            }
        }


        protected bool PeerErrorDetected(Exception ex, ConnectionContext connection)
        {
            if ((DateTime.Now - connection.LastAcknowledge) > ClientDisconnectTimeout)
            {
                ClientDisconnected?.Invoke(this, new ClientEventArgs(connection.ProcessId));
                return false;
            }

            return true;
        }


        public void Send(string text)
        {
            ConnectionContext[] connections;
            _connectionsLock.EnterReadLock();
            try
            {
                connections = Connections.ToArray();
            }
            finally
            {
                _connectionsLock.ExitReadLock();
            }

            text = MessageHelpers.EscapeCrLf(text) + MessageHelpers.MessageSeparator;
            foreach (var connection in connections)
            {
                SendToWithoutSeparatorUnEscaped(text, connection);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SendToWithoutSeparatorUnEscaped(string text, ConnectionContext connection)
        {
            var bytes = _encoding.GetBytes(text);
            try
            {
                connection.OutputPipe.BeginWrite(bytes, 0, bytes.Length, ConnectionOutputPipeBeginWriteCallback,
                    connection);
            }
            catch (Exception ex)
                when (ex is ObjectDisposedException ||
                      ex is InvalidOperationException || ex is IOException)
            {
                PeerErrorDetected(ex, connection);
            }
        }

        protected void ConnectionOutputPipeBeginWriteCallback(IAsyncResult result)
        {
            var connection = result.AsyncState as ConnectionContext;
            if (connection == null) return;

            connection.OutputPipe.EndWrite(result);

            //NOTHING NOW!
        }


        public void Dispose()
        {
            _connectionsLock.EnterReadLock();
            try
            {
                foreach (var server in Connections)
                {
                    server.Dispose();
                }
            }
            finally
            {
                _connectionsLock.ExitReadLock();
            }
        }
    }
}