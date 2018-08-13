using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.IO;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        public class HubServer : IHubEndpoint, IDisposable
        {
            public event DataReceivedEventHandler<BytesReceivedEventArgs> DataReceived;

            public string Name { get; }

            public IList<ConnectionContext> Connections { get; }
            private ReaderWriterLockSlim _connectionsLock;
            private bool _isClosing = false;

            protected IInterProcessLockService InterProcessLock { get; }

            public IThreadFactory ThreadFactory { get; }


            public HubServer(string name, IInterProcessLockService interProcessLock, IThreadFactory threadFactory)
            {
                Name = name;
                InterProcessLock = interProcessLock;
                ThreadFactory = threadFactory;
                Connections = new List<ConnectionContext>();
                _connectionsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            }


            public void Listen()
            {
                NamedPipeServerStream serverPipe = null;
                while (!_isClosing)
                {
                    var lockTaken = InterProcessLock.AcquireLock(Name);
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

                var streamWriter = new StreamWriter(serverPipe);
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine(info);
                //streamWriter.Flush();

                serverPipe.Flush();

                SpinWait.SpinUntil(() =>
                {
                    //streamWriter.WriteLine(info);
                    return !reader.IsConnected || !writer.IsConnected;
                }, TimeSpan.FromSeconds(3));

                //serverPipe.WaitForPipeDrain();
                //serverPipe.Close(); //re-use
                serverPipe.Disconnect();

                _connectionsLock.EnterWriteLock();
                try
                {
                    Connections.Add(new ConnectionContext(
                        ThreadFactory.CreateBackground(ListenDataReceived,
                            $"DefaultMessagingHost.HubServer.ConnectionListenDataReceived-{info}"),
                        reader, writer));
                }
                finally
                {
                    _connectionsLock.ExitWriteLock();
                }
            }


            public void ListenDataReceived(object state)
            {
                var connection = state as ConnectionContext;
                if (connection == null) return;

                using (var sr = new BinaryReader(connection.InputPipe))
                {
                    SpinWait.SpinUntil(() =>
                    {
                        sr.Read();

                        return _isClosing;
                    });
                }
            }

            public void Send(byte[] bytes)
            {
                foreach (var connection in Connections)
                {
                    var binaryWriter = new BinaryWriter(connection.OutputPipe);
                    binaryWriter.Write(bytes);
                }
            }
//            private void BeginWaitForConnectionCallback(IAsyncResult result)
//            {
//                var serverPipe = result?.AsyncState as NamedPipeServerStream;
//                if (serverPipe == null) return;
//
//                serverPipe.EndWaitForConnection(result);
//
//                _handleConnection(serverPipe);
//            }


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


            public class ConnectionContext : IDisposable
            {
                public PipeStream InputPipe { get; }
                public PipeStream OutputPipe { get; }

                public IThread Thread { get; }

                public ConnectionContext(IThread thread, PipeStream inputPipe, PipeStream outputPipe)
                {
                    Thread = thread;
                    InputPipe = inputPipe;
                    OutputPipe = outputPipe;
                }

                public void Dispose()
                {
                    InputPipe?.Dispose();
                    OutputPipe?.Dispose();
                    
                    Thread.Abort();
                }
            }
        }
    }
}