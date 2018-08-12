using System.Collections.Generic;
using System.IO.Pipes;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.Utils;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        protected class IPCController
        {
            private const string InterProcessPipeName = "AMKDownloadManager.DefaultMessagingHost";
            private const string InterProcessPipeLockName = "AMKDownloadManagerDefaultMessagingHostPipeLock";
            
            public static bool IsHubJoinCalled { get; private set; }
            
            private static IList<DefaultMessagingHost> _hosts = new List<DefaultMessagingHost>(1);

            private static IInterProcessLockService LockService;
            
            private static NamedPipeServerStream ServerPipe;
            private static NamedPipeClientStream ClientPipe;

            public static void AddHost(DefaultMessagingHost host)
            {
                lock (_hosts)
                {
                    _hosts.Add(host);
                }
            }

            public static void Initialize(IInterProcessLockService lockService)
            {
                LockService = lockService;
            }

            public static void SignalExit()
            {
                
            }
            
            public static void JoinHub()
            {
                IsHubJoinCalled = true;
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