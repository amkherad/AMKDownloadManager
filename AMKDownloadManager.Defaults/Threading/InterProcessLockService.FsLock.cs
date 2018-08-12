using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Threading
{
    public partial class InterProcessLockService
    {
        public class FsLock : IInterProcessLockService
        {
            public string LockDirectoryPath { get; }

            public FsLock(string path)
            {
                LockDirectoryPath = path;
            }

            public int Order => 0;

            public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
            {
            }

            public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider,
                HashSet<string> changes)
            {
            }


            private string _getFilePathEnsuredDirectory(string name)
            {
                var dir = LockDirectoryPath;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                
                //ToLower is used to achieve same behavior on case-sensitive and case-insensitive file systems.
                return Path.Combine(dir, $"_{name.ToLower()}.lock");
            }


            /// <inheritdoc />
            public bool TryAcquireLock(string name, out object lockHandle)
            {
                try
                {
                    var filePath = _getFilePathEnsuredDirectory(name);
                    var file = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
                    lockHandle = file;
                    return true;
                }
                catch (IOException)
                {
                    lockHandle = false;
                    return false;
                }
            }

            /// <inheritdoc />
            public bool TryAcquireLock(string name, TimeSpan waitTimeout, out object lockHandle)
            {
                var result = false;
                object state = null;

                //try to acquire the lock until it's successful or timeout reached.
                SpinWait.SpinUntil(() => result = TryAcquireLock(name, out state), waitTimeout);

                lockHandle = state;
                return result;
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
                var file = lockHandle as FileStream;
                if (file == null) throw new ArgumentException(nameof(lockHandle));
                
                file.Close();
                file.Dispose();

                try
                {
                    //tries to clean the file. [it may fail if another thread has opened the file]
                    File.Delete(file.Name);
                }
                catch (IOException)
                {
                    //NOTHING!
                }
            }

            /// <inheritdoc />
            public void Clean()
            {
                var files = Directory.GetFiles(LockDirectoryPath);

                //Close all abandoned files.
                
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (IOException)
                    {
                        //continue;
                    }
                }
            }
        }
    }
}