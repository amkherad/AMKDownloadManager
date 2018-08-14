using System;
using System.Collections.Generic;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Threading
{
    public class AbstractThreadFactory : IThreadFactory
    {
        public AbstractThreadFactory()
        {
            
        }

        #region IThreadFactory implementation

        public IThread Create(Action action)
        {
            return new AbstractThread(new Thread(new ThreadStart(action)));
        }
        public IThread Create(Action<object> action)
        {
            return new AbstractThread(new Thread(new ParameterizedThreadStart(action)));
        }
        
        public IThread Create(Action action, string name)
        {
            return new AbstractThread(new Thread(new ThreadStart(action))
            {
                Name = name
            });
        }
        public IThread Create(Action<object> action, string name)
        {
            return new AbstractThread(new Thread(new ParameterizedThreadStart(action))
            {
                Name = name
            });
        }
        
        
        public IThread CreateBackground(Action action, string name)
        {
            return new AbstractThread(new Thread(new ThreadStart(action))
            {
                Name = name,
                IsBackground = true
            });
        }
        public IThread CreateBackground(Action<object> action, string name)
        {
            return new AbstractThread(new Thread(new ParameterizedThreadStart(action))
            {
                Name = name,
                IsBackground = true
            });
        }

        public void JoinAll(IEnumerable<IThread> threads)
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public void Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        public int Order => 0;
        
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }

        #endregion
    }
}