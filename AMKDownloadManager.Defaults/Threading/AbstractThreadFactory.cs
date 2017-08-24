using System;
using System.Collections.Generic;
using System.Threading;
using AMKDownloadManager.Core.Api.Threading;

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

        public void JoinAll(IEnumerable<IThread> threads)
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public int Order => 0;

        #endregion
    }
}