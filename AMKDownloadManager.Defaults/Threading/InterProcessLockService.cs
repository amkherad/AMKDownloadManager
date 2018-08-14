using System;
using System.Collections.Generic;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Threading
{
    public partial class InterProcessLockService : IInterProcessLockService
    {
        public IApplicationContext AppContext { get; private set; }

        private readonly IInterProcessLockService _lockImpl;

        private readonly Dictionary<string, int> _lockRecursionCount;


        private class LockContext
        {
            public string Name;
            public object State;
        }


        public InterProcessLockService(IApplicationContext appContext)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

            _lockRecursionCount = new Dictionary<string, int>();

            if (ApplicationContext.IsWindows)
            {
                //named mutex is faster so we prefer it but it's only supported in windows (for true inter-process lock).
                _lockImpl = new NamedMutex();
            }
            else
            {
                _lockImpl = new FsLock(appContext.ApplicationLockDirectory);
            }
        }


        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider,
            HashSet<string> changes)
        {
        }


        private int? _getCount(string name)
        {
            lock (_lockRecursionCount)
            {
                if (_lockRecursionCount.TryGetValue(name, out var value))
                {
                    return value;
                }
            }

            return null;
        }

        private void _incrementOrAddCount(string name)
        {
            lock (_lockRecursionCount)
            {
                if (_lockRecursionCount.TryGetValue(name, out var value))
                {
                    _lockRecursionCount[name] = value + 1;
                }

                _lockRecursionCount.Add(name, 1);
            }
        }

        private bool _decrementOrRemoveCount(string name)
        {
            lock (_lockRecursionCount)
            {
                if (_lockRecursionCount.TryGetValue(name, out var value))
                {
                    value--;
                    if (value > 0)
                    {
                        _lockRecursionCount[name] = value;
                        return true;
                    }

                    _lockRecursionCount.Remove(name);
                    return false;
                }

                return false;
            }
        }


        public bool TryAcquireLock(string name, out object lockHandle)
        {
            var result = _lockImpl.TryAcquireLock(name, out lockHandle);
            if (result)
                _incrementOrAddCount(name);
            return result;
        }

        public bool TryAcquireLock(string name, TimeSpan waitTimeout, out object lockHandle)
        {
            var result = _lockImpl.TryAcquireLock(name, waitTimeout, out lockHandle);
            if (result)
                _incrementOrAddCount(name);
            return result;
        }

        public object AcquireLock(string name)
        {
            var result = _lockImpl.AcquireLock(name);
            if (result != null)
                _incrementOrAddCount(name);
            return result;
        }

        public object AcquireLock(string name, TimeSpan waitTimeout)
        {
            var result = _lockImpl.AcquireLock(name, waitTimeout);
            if (result != null)
                _incrementOrAddCount(name);
            return result;
        }

        public void ReleaseLock(object lockHandle)
        {
            var context = lockHandle as LockContext;
            if (context == null)
            {
                throw new InvalidOperationException();
            }

            var result = _decrementOrRemoveCount(context.Name);
            if (result)
            {
                _lockImpl.ReleaseLock(context);
            }
        }

        public void Clean()
            => _lockImpl.Clean();

        public bool IsCorruptionPossible(string name) => _lockImpl.IsCorruptionPossible(name);


        public void ForceRemoveCorruptedLock(string name)
        {
            var count = _getCount(name);

            if (count.HasValue)
            {
                throw new InvalidOperationException();
            }

            _lockImpl.ForceRemoveCorruptedLock(name);
        }
    }
}