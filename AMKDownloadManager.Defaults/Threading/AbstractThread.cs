using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AMKDownloadManager.Core.Api.Threading;

namespace AMKDownloadManager.Defaults.Threading
{
    public class AbstractThread : IThread
    {
        private Thread _thread;

        public AbstractThread(Thread thread)
        {
            if (thread == null) throw new ArgumentNullException(nameof(thread));

            _thread = thread;
        }

        #region IThread implementation
        public void Start()
        {
            ThrowIfNull();

            _thread.Start();
        }
        public void Start(object state)
        {
            ThrowIfNull();

            _thread.Start(state);
        }

        public void Join()
        {
            ThrowIfNull();

            _thread.Join();
        }
        public void Abort()
        {
            ThrowIfNull();

            _thread.Abort();
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ThrowIfNull()
        {
            if (_thread == null)
                throw new InvalidOperationException();
        }
    }
}