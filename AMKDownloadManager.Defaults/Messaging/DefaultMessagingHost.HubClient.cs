using System;
using System.IO;
using System.IO.Pipes;
using AMKsGear.Core.Text;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        public class HubClient
        {
            public string Name { get; }
            
            private bool _isClosing = false;

            protected AnonymousPipeClientStream ClientInput { get; private set; }
            protected AnonymousPipeClientStream ClientOutput { get; private set; }

            public HubClient(string name)
            {
                Name = name;
            }

            public void Connect()
            {
                while (!_isClosing)
                {
                    ClientInput?.Dispose();
                    ClientOutput?.Dispose();

                    var clientPipe = new NamedPipeClientStream(".", Name, PipeDirection.In);

                    clientPipe.Connect();

                    string info = null;

                    using (var reader = new StreamReader(clientPipe))
                    {
                        while (info == null)
                        {
                            info = reader.ReadLine();
                        }

                        Console.Write($"HubServer said: {info}");
                    }

                    if (info != null)
                    {
                        var parts = info.Split('-');

                        if (parts.Length != 3)
                        {
                            return;
                        }

                        var processId = parts[0].ToInt32();
                        var sReader = parts[1];
                        var sWriter = parts[2];

                        ClientInput = new AnonymousPipeClientStream(PipeDirection.In, sWriter);
                        ClientOutput = new AnonymousPipeClientStream(PipeDirection.Out, sReader);
                        
                        
                    }
                }
            }
        }
    }
}