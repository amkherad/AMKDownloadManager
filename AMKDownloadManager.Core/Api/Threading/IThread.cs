using System;

namespace AMKDownloadManager.Core.Api.Threading
{
    /// <summary>
    /// Abstraction layer for thread.
    /// </summary>
    public interface IThread
    {
        void Start();
        void Start(object state);

        void Join();
        void Abort();
    }
}