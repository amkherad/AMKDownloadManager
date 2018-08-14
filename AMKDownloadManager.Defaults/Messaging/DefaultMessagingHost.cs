using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Messaging;
using AMKDownloadManager.Core.Api.Threading;
using AMKDownloadManager.Defaults.Threading;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost : IMessagingHost
    {
        private const string InterProcessPipeName = "DefaultMessagingHost";

        private static readonly NameValuesCollection<IMessageListener> _listeners;
        
        static DefaultMessagingHost()
        {
            _listeners = new NameValuesCollection<IMessageListener>();
        }
        
        public IApplicationContext AppContext { get; }
        public IThreadFactory ThreadFactory { get; private set; }
        
        protected IHub Hub { get; private set; }

        private bool _isInitialized = false;


        public DefaultMessagingHost(IApplicationContext appContext)
        {
            AppContext = appContext;
        }
        
        public int Order => 0;
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            ThreadFactory = appContext.GetFeature<IThreadFactory>();
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
        }


        private void _initialize()
        {
            if (_isInitialized) return;

            _isInitialized = true;

            var interLock = new InterProcessLockService(AppContext);
            
            Hub = new HubController(InterProcessPipeName, this, interLock, ThreadFactory);
            AppContext.ScheduleBackgroundTask(
                nameof(DefaultMessagingHost) + '.' + nameof(HubController),
                Hub.JoinHub);
        }
        
        
        public void Subscribe(string name, IMessageListener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            if (!_isInitialized)
                _initialize();
            
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
            if (!_isInitialized)
                _initialize();
            
            lock (_listeners)
            {
                _listeners.Remove(name, listener);
            }
        }

        public void FreeListener(IMessageListener listener)
        {
            if (!_isInitialized)
                _initialize();
            
            lock (_listeners)
            {
                _listeners.RemoveAllValues(new[] {listener});
            }
        }
        
        
        public void Send(string name, object state, MessageSource targetBoundary)
        {
            if (!_isInitialized)
                _initialize();
            
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
                Hub.Send(name, state);
            }
        }

        internal void InterProcessMessageReceived(string name, object state)
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