using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Threading
{
    public partial class InterProcessLockService : IInterProcessLockService
    {
        public IApplicationContext AppContext { get; private set; }
        
        private readonly IInterProcessLockService _lock;
        
        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            AppContext = appContext;
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider,
            HashSet<string> changes)
        {
        }

        public InterProcessLockService()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //named mutex is faster so we prefer it but it's only supported in windows (for true inter-process lock).
                _lock = new NamedMutex();
            }
            else
            {
                _lock = new FsLock(AppContext.ApplicationLockDirectory);
            }
        }


        public bool TryAcquireLock(string name, out object lockHandle)
            => _lock.TryAcquireLock(name, out lockHandle);

        public bool TryAcquireLock(string name, TimeSpan waitTimeout, out object lockHandle)
            => _lock.TryAcquireLock(name, waitTimeout, out lockHandle);

        public object AcquireLock(string name)
            => _lock.AcquireLock(name);

        public object AcquireLock(string name, TimeSpan waitTimeout)
            => _lock.AcquireLock(name, waitTimeout);

        public void ReleaseLock(object lockHandle)
            => _lock.ReleaseLock(lockHandle);

        public void Clean()
            => _lock.Clean();
    }
}