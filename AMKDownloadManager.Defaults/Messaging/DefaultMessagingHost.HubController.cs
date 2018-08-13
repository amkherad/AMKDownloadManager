using System.IO.Pipes;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        // ReSharper disable once InconsistentNaming
        public class HubController : IHub
        {
            public DefaultMessagingHost Host { get; }

            protected IInterProcessLockService LockService;
            
            protected NamedPipeServerStream ServerPipe;
            protected NamedPipeClientStream ClientPipe;

            
            public HubController(DefaultMessagingHost host, IInterProcessLockService lockService)
            {
                Host = host;
                LockService = lockService;
            }
            
            
            public void JoinHub()
            {
                
            }

            public void Send(string name, object state)
            {
                
            }

            public void Dispose()
            {
                
            }
        }
    }
}