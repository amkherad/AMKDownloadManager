using System;
using System.Collections.Generic;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Threading
{
    public partial class InterProcessLockService
    {
        public class NamedMutex : IInterProcessLockService
        {
            private readonly Dictionary<string, Mutex> _mutexes;


            public NamedMutex()
            {
                _mutexes = new Dictionary<string, Mutex>();
            }
            
            
            public int Order => 0;
            public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
            {
            }

            public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
            {
            }

            /// <inheritdoc />
            public bool TryAcquireLock(string name, out object lockHandle)
            {
                if (_mutexes.ContainsKey(name))
                {
                    var mutex = _mutexes[name];
                    if (mutex.WaitOne(TimeSpan.Zero, true))
                    {
                        lockHandle = new LockContext
                        {
                            Name = name
                        };
                        return true;
                    }

                    lockHandle = null;
                    return false;
                }
                else
                {
                    var mutex = new Mutex(true, $"Local\\{name}", out var createdNew);
                    if (createdNew)
                    {
                        _mutexes.Add(name, mutex);
                        lockHandle = new LockContext
                        {
                            Name = name
                        };
                        return true;
                    }

                    lockHandle = null;
                    return false;
                }
            }

            /// <inheritdoc />
            public bool TryAcquireLock(string name, TimeSpan waitTimeout, out object lockHandle)
            {
                if (_mutexes.ContainsKey(name))
                {
                    var mutex = _mutexes[name];
                    if (mutex.WaitOne(waitTimeout, true))
                    {
                        lockHandle = new LockContext
                        {
                            Name = name
                        };
                        return true;
                    }

                    lockHandle = null;
                    return false;
                }
                else
                {
                    var mutex = new Mutex(true,  $"Local\\{name}", out var createdNew);
                    if (createdNew)
                    {
                        _mutexes.Add(name, mutex);
                        lockHandle = new LockContext
                        {
                            Name = name
                        };
                        return true;
                    }

                    lockHandle = null;
                    return false;
                }
            }

            /// <inheritdoc />
            public object AcquireLock(string name)
            {
                if (!TryAcquireLock(name, TimeSpan.MaxValue, out var lockHandle))
                {
                    throw new InvalidOperationException();
                }

                return lockHandle;
            }

            /// <inheritdoc />
            public object AcquireLock(string name, TimeSpan waitTimeout)
            {
                if (!TryAcquireLock(name, waitTimeout, out var lockHandle))
                {
                    throw new InvalidOperationException();
                }

                return lockHandle;
            }

            /// <inheritdoc />
            public void ReleaseLock(object lockHandle)
            {
                if (lockHandle == null) throw new ArgumentNullException(nameof(lockHandle));
                var context = lockHandle as LockContext;
                if (context == null) throw new ArgumentException(nameof(lockHandle));

                if (!_mutexes.TryGetValue(context.Name, out var mutex))
                {
                    throw new InvalidOperationException();
                }
                
                mutex.ReleaseMutex();
            }

            /// <inheritdoc />
            public void Clean()
            {
                //NOTHING!
            }

            public bool IsCorruptionPossible(string name)
            {
                return false;
            }

            public void ForceRemoveCorruptedLock(string name)
            {
                
            }
        }
    }
}