using System;
using System.Collections.Generic;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Messaging;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost : IMessagingHost
    {
        private static readonly NameValuesCollection<IMessageListener> _listeners;

        static DefaultMessagingHost()
        {
            _listeners = new NameValuesCollection<IMessageListener>();
        }
        
        public IApplicationContext AppContext { get; }
        public IThreadFactory ThreadFactory { get; private set; }


        public DefaultMessagingHost(IApplicationContext appContext)
        {
            AppContext = appContext;
            IPCController.AddHost(this);
        }
        
        public int Order => 0;
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            if (!IPCController.IsHubJoinCalled)
            {
                appContext.ScheduleBackgroundTask(
                    nameof(DefaultMessagingHost) + '.' + nameof(IPCController),
                    IPCController.JoinHub);
            }
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
        }
        
        
        public void Subscribe(string name, IMessageListener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            lock (_listeners)
            {
                if (!_listeners.ContainsKeyValue(name, listener))
                {
                    _listeners.Add(name, listener);
                }
            }
        }

        public void Unsubscribe(string name, IMessageListener listener)
        {
            lock (_listeners)
            {
                _listeners.Remove(name, listener);
            }
        }

        public void FreeListener(IMessageListener listener)
        {
            lock (_listeners)
            {
                _listeners.RemoveAllValues(new[] {listener});
            }
        }
        
        
        public void Send(string name, object state, MessageSource targetBoundary)
        {
            lock (_listeners)
            {
                if(_listeners.TryGetValues(name, out var listeners))
                {
                    foreach (var listener in listeners)
                    {
                        listener.MessageReceived(name, state, MessageSource.LocalProcess);
                    }
                }
            }

            if (targetBoundary == MessageSource.InterProcess)
            {
                IPCController.Send(name, state);
            }
        }

        private void _interProcessMessageReceived(string name, object state)
        {
            lock (_listeners)
            {
                if(_listeners.TryGetValues(name, out var listeners))
                {
                    foreach (var listener in listeners)
                    {
                        listener.MessageReceived(name, state, MessageSource.InterProcess);
                    }
                }
            }
        }
    }
}