using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Automation.LifetimeManagers;

namespace AMKDownloadManager.Core
{
    public partial class ApplicationContext
    {
        private IList<IThread> _backgroundThreads = new List<IThread>();
        private IList<IThread> _foregroundThreads = new List<IThread>();


        public IDisposableContainer DisposableContainer { get; } = new DisposableContainer();
        

        private IThreadFactory _threadFactory;
        protected IThreadFactory ThreadFactory
        {
            get
            {
                if (_threadFactory != null) return _threadFactory;

                var threadFactory = _threadFactory = this.GetFeature<IThreadFactory>();
                
                if (threadFactory == null)
                    throw new InvalidOperationException();

                return threadFactory;
            }
        }

        public bool IsAbortingBackgroundThreads { get; private set; }


        public void AddForegroundThread(IThread thread)
        {
            _foregroundThreads.Add(thread);
        }

        public void RemoveForegroundThread(IThread thread)
        {
            _foregroundThreads.Remove(thread);
        }

        public void ScheduleForegroundTask(IThread thread)
        {
            AddForegroundThread(thread);
            thread.Start();
        }

        public IThread ScheduleForegroundTask(string name, Action action)
        {
            var thread = ThreadFactory.Create(action, name);
            AddForegroundThread(thread);
            thread.Start();
            return thread;
        }

        public IThread ScheduleForegroundTask(string name, Action<object> action, object state)
        {
            var thread = ThreadFactory.Create(action, name);
            AddForegroundThread(thread);
            thread.Start(state);
            return thread;
        }

        public void AddBackgroundThread(IThread thread)
        {
            _backgroundThreads.Add(thread);
        }

        public void RemoveBackgroundThread(IThread thread)
        {
            _backgroundThreads.Remove(thread);
        }

        public void ScheduleBackgroundTask(IThread thread)
        {
            AddBackgroundThread(thread);
            thread.Start();
        }

        public IThread ScheduleBackgroundTask(string name, Action action)
        {
            var thread = ThreadFactory.CreateBackground(action, name);
            AddBackgroundThread(thread);
            thread.Start();
            return thread;
        }

        public IThread ScheduleBackgroundTask(string name, Action<object> action, object state)
        {
            var thread = ThreadFactory.CreateBackground(action, name);
            AddBackgroundThread(thread);
            thread.Start(state);
            return thread;
        }


        public void JoinForegroundThreads()
        {
            ThreadFactory.JoinAll(_foregroundThreads);
        }

        public void AbortBackgroundThreads()
        {
            IsAbortingBackgroundThreads = true;
            foreach (var thread in _backgroundThreads)
            {
                thread.Abort();
            }

            IsAbortingBackgroundThreads = false;
        }
    }
}