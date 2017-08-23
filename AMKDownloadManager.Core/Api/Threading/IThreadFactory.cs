using System;
using System.Collections.Generic;

namespace AMKDownloadManager.Core.Api.Threading
{
    /// <summary>
    /// Abstraction layer for threading.
    /// </summary>
    public interface IThreadFactory : IFeature
    {
        /// <summary>
        /// Creates a new thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        IThread Create(Action action);
        /// <summary>
        /// Creates a new thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        IThread Create(Action<object> action);

        void WaitAll(IEnumerable<IThread> threads);
        void WhenAll(IEnumerable<IThread> threads);
    }
}