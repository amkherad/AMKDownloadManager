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

        /// <summary>
        /// Creates a new thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="name">Debug name of the thread.</param>
        IThread Create(Action action, string name);
        /// <summary>
        /// Creates a new thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="name">Debug name of the thread.</param>
        IThread Create(Action<object> action, string name);

        /// <summary>
        /// Creates a new background thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="name">Debug name of the thread.</param>
        IThread CreateBackground(Action action, string name);
        /// <summary>
        /// Creates a new background thread instance using given action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="name">Debug name of the thread.</param>
        IThread CreateBackground(Action<object> action, string name);

        void JoinAll(IEnumerable<IThread> threads);
    }
}