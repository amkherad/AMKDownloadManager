using System.Collections.Generic;
using System.IO.Pipes;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        protected class IPCController
        {
            private const string InterProcessPipeName = "AMKDownloadManager.DefaultMessagingHost";
            
            private static IList<DefaultMessagingHost> _hosts = new List<DefaultMessagingHost>(1);

            private static NamedPipeClientStream ReaderPipe;

            public static void AddHost(DefaultMessagingHost host)
            {
                lock (_hosts)
                {
                    _hosts.Add(host);
                }
            }
            
            public static void Listen()
            {
                var loop = new LoopCountLimiter(5);
                for (;;)
                {
                    
                    
                    loop.Count();
                }
            }

            public static void Send(string name, object state)
            {
                
            }
        }
    }
}