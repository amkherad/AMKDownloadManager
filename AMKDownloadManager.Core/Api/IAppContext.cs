using System;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.LifetimeManagers;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application service pool.
    /// </summary>
    public interface IApplicationContext : IStorageAppContext, ITypeResolverAppContext
    {
        string ApplicationDirectory { get; }
        string ApplicationLockDirectory { get; }
        string ApplicationProfileDirectory { get; }
        string ApplicationSharedProfileDirectory { get; }
        string ApplicationPluginRepository { get; }
        string ApplicationConfigurationFilePath { get; }
        string ApplicationSharedConfigurationFilePath { get; }


        IDisposableContainer DisposableContainer { get; }


        void AddForegroundThread(IThread thread);
        void RemoveForegroundThread(IThread thread);

        void ScheduleForegroundTask(IThread thread);
        IThread ScheduleForegroundTask(string name, Action action);
        IThread ScheduleForegroundTask(string name, Action<object> action, object state);

        void AddBackgroundThread(IThread thread);
        void RemoveBackgroundThread(IThread thread);

        void ScheduleBackgroundTask(IThread thread);
        IThread ScheduleBackgroundTask(string name, Action action);
        IThread ScheduleBackgroundTask(string name, Action<object> action, object state);

        void JoinForegroundThreads();
        void AbortBackgroundThreads();
    }
}