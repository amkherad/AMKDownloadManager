using System;
using AMKDownloadManager.Core.Api.Threading;
using System.Threading;

namespace AMKDownloadManager.Threading
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

        public int Order => 0;

        #endregion
    }
}