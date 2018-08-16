using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using AMKDownloadManager.Core;
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
            public const string UnixLockDir = "/run/lock";
            public const string UnixShmDir = "/dev/shm";
            public const string UnixTmpDir = "/tmp";
            public const string UnixRunUserDir = "/run/user/$uid";

            public static readonly string[] UnixLockDirectories = new[]
            {
                UnixLockDir,
                UnixShmDir,
                UnixTmpDir,
                UnixRunUserDir
            };
            
            
            public string LockDirectoryPath { get; }

            private bool _useVarLock = false;

            public FsLock(string path)
            {
                if (ApplicationContext.IsLinux)
                {
                    //var dirSec = new DirectorySecurity(UnixTmpDir, AccessControlSections.Access);
                    //dirSec.GetAccessRules()
                    
                    try
                    {
                        if (Directory.Exists("/var/lock"))
                        {
                            LockDirectoryPath = "/var/lock";
                        }
                    }
                    catch (IOException)
                    {
                        LockDirectoryPath = path;
                    }
                }
                else
                {
                    LockDirectoryPath = path;
                }

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
                    var file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    file.Lock(0, 0);
                    lockHandle = new LockContext
                    {
                        Name = name,
                        State = file
                    };
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

                //busy wait to acquire the lock.
                //tries to acquire the lock until it's successful or timeout reached.
                SpinWait.SpinUntil(() => result = TryAcquireLock(name, out state), waitTimeout);

                lockHandle = state;
                return result;
            }

            /// <inheritdoc />
            public object AcquireLock(string name)
            {
                if (!TryAcquireLock(name, TimeSpan.FromMilliseconds(int.MaxValue), out var lockHandle))
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

                var file = context.State as FileStream;
                if (file == null) throw new ArgumentException(nameof(lockHandle));

                file.Unlock(0, 0);
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
                        var fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                        var deleted = false;
                        
                        //trying to delete the file while it's open to prevent another process to open the file.
                        //it's acceptable in *nix
                        try
                        {
                            File.Delete(file);
                            deleted = true;
                        }
                        catch (IOException)
                        {
                            
                        }

                        fs.Close();

                        //if deleting the file while it's open is not successful, trying to delete the file here.
                        if (!deleted)
                        {
                            try
                            {
                                File.Delete(file);
                                deleted = true;
                            }
                            catch (IOException)
                            {
                            }
                        }
                    }
                    catch (IOException)
                    {
                        //continue;
                    }
                }
            }

            public bool IsCorruptionPossible(string name)
            {
                return true;
            }

            public void ForceRemoveCorruptedLock(string name)
            {
                var filePath = _getFilePathEnsuredDirectory(name);

                try
                {
                    File.Delete(filePath);
                }
                catch (IOException)
                {
                    //continue;
                }
            }
        }
    }
}