using System;
using System.IO;
using System.Text;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.IO;
using Newtonsoft.Json;

namespace AMKDownloadManager.Defaults.Messaging
{
    // ReSharper disable once InconsistentNaming
    public class HubController : IHub
    {
        public const int JoinHubTryInterval = 1000;
        
        public DefaultMessagingHost Host { get; }

        protected IInterProcessLockService InterProcessLock;

        protected IHubEndpoint Endpoint;
        protected HubServer Server;
        protected HubClient Client;
        
        public IThreadFactory ThreadFactory { get; }
        
        public string Name { get; }
        public string LockName => ApplicationContext.ApplicationInterProcessLockNamePrefix + Name;

        private JsonSerializer _serializer;
        

        public HubController(string name, DefaultMessagingHost host, IInterProcessLockService lockService, IThreadFactory threadFactory)
        {
            Name = name;
            Host = host;
            InterProcessLock = lockService;
            ThreadFactory = threadFactory;
            
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }


        public void JoinHub()
        {
            var lockName = LockName;
            if (InterProcessLock.TryAcquireLock(lockName, TimeSpan.FromMilliseconds(JoinHubTryInterval), out var lockHandle))
            {
                try
                {
                    Server = new HubServer(Name, lockName, InterProcessLock, ThreadFactory);
                    Endpoint = Server;
                    Server.Listen();
                }
                finally
                {
                    InterProcessLock.ReleaseLock(lockHandle);
                }
            }
            else
            {
                try
                {
                    Client = new HubClient(Name, ThreadFactory);
                    Endpoint = Client;
                    Client.Connect(JoinHubTryInterval);
                }
                catch (Exception ex)
                {
                    if (InterProcessLock.IsCorruptionPossible(lockName))
                    {
                        if (!ApplicationContext.TryToFindAnotherInstanceOfApplication(out var processId, out var processName))
                        {
                            InterProcessLock.ForceRemoveCorruptedLock(lockName);
                        }
                    }
                }
            }
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs<string> eventArgs)
        {
            var context = _deserializeContext(eventArgs.Buffer);

            Host.InterProcessMessageReceived(context.Name, context.State);
        }

        public void Send(string name, object state)
        {
            if (Endpoint == null) return;
            
            Endpoint.Send(_serializeContext(name, state));
        }


        private class Context
        {
            public string Name { get; set; }
            public object State { get; set; }
        }

        private string _serializeContext(string name, object state)
        {
            var context = new Context
            {
                Name = name,
                State = state
            };

            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, context);

                return writer.ToString();
            }
        }
        private Context _deserializeContext(string message)
        {
            using (var reader = new JsonTextReader(new StringReader(message)))
            {
                return _serializer.Deserialize<Context>(reader);
            }
        }
        

        public void Dispose()
        {
        }
    }
}