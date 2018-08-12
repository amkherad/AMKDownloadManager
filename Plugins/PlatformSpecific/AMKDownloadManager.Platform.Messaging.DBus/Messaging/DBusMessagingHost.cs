using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Messaging;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Platform.Messaging.DBus.Messaging
{
    public class DBusMessagingHost : IMessagingHost
    {
        public int Order => -1;
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }
        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }

        
        public void Subscribe(string name, IMessageListener listener)
        {
            
        }

        public void Unsubscribe(string name, IMessageListener listener)
        {
            
        }

        public void FreeListener(IMessageListener listener)
        {
            
        }

        public void Send(string name, object state, MessageSource targetBoundary)
        {
            
        }
    }
}