using System.Threading;
using AMKsGear.Architecture.Patterns;

namespace AMKDownloadManager.Core.Api.Threading
{
    /// <summary>
    /// Abstraction layer for thread.
    /// </summary>
    public interface IThread : IWrapper
    {
        void Start();
        void Start(object state);

        void Join();
        void Abort();
        
        string Name { get; set; }
        int ManagedThreadId { get; }
        
        ThreadState ThreadState { get; }
        bool IsAlive { get; }
    }
}