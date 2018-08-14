using System;

namespace AMKDownloadManager.Core.Api.Threading
{
    /// <summary>
    /// Provides a general cross-platform inter-process lock/mutex.
    /// </summary>
    public interface IInterProcessLockService : IFeature
    {
        /// <summary>
        /// Tries to acquire a lock.
        /// </summary>
        /// <param name="name">Case-insensitive name of the lock.</param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        bool TryAcquireLock(string name, out object lockHandle);

        /// <summary>
        /// Tries to acquire a lock within a timeout.
        /// NOTE: The timeout is not for the lock itself.
        /// </summary>
        /// <param name="name">Case-insensitive name of the lock.</param>
        /// <param name="waitTimeout"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        bool TryAcquireLock(string name, TimeSpan waitTimeout, out object lockHandle);

        /// <summary>
        /// Acquires a lock.
        /// </summary>
        /// <param name="name">Case-insensitive name of the lock.</param>
        /// <returns></returns>
        object AcquireLock(string name);

        /// <summary>
        /// Tries to acquire the lock if it is not successful withing the waitTimeout an exception will be thrown.
        /// NOTE: The timeout is not for the lock itself.
        /// </summary>
        /// <param name="name">Case-insensitive name of the lock.</param>
        /// <param name="waitTimeout"></param>
        /// <returns></returns>
        object AcquireLock(string name, TimeSpan waitTimeout);

        /// <summary>
        /// Releases the lock.
        /// </summary>
        /// <param name="lockHandle"></param>
        void ReleaseLock(object lockHandle);

        /// <summary>
        /// Cleans all resources used by this service except open mutexes (i.e. files)
        /// </summary>
        void Clean();


        /// <summary>
        /// Determines whether corruption is possible on this type of lock.
        /// </summary>
        bool IsCorruptionPossible(string name);


        /// <summary>
        /// Handles a broken resource such as abandoned mutex or a lock file that is not removed properly.
        /// NOTE: If lock is being open in current process this will throw an exception.
        /// </summary>
        /// <param name="name"></param>
        void ForceRemoveCorruptedLock(string name);
    }
}