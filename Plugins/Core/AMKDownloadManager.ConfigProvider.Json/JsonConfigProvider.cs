using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Architecture.Data;
using AMKsGear.Architecture.Patterns;
using AMKsGear.Architecture.Trace;
using AMKsGear.Core.Data;
using AMKsGear.Core.Trace;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMKDownloadManager.ConfigProvider.Json
{
    public class JsonConfigProvider : IConfigProvider
    {
        public const string InterSynchronizationName = "AMKDownloadManager.InterSynchronizationName";

        public IApplicationContext AppContext { get; }

        public IEnumerable<string> ConfigurationFilePaths { get; }


        /// <summary>
        /// A cache context to store all configuration keys in memory..
        /// </summary>
        private ICacheContext<string, string> _sourceValues;

        /// <summary>
        /// A second copy of source values to enable transactions.
        /// </summary>
        private ICacheContext<string, string> _currentValues;

        /// <summary>
        /// Determines whether changing values should apply to config file immediately.
        /// </summary>
        private bool _commitChangesToConfigFile = false;


        private ReaderWriterLockSlim _lock;
        private EventWaitHandle _interLock;
        private TimeSpan _interLockTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Creates a new instance of <see cref="JsonConfigProvider"/>.
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="paths">An ordered list of configuration file paths to load, duplicates will override (from first to last).</param>
        public JsonConfigProvider(IApplicationContext appContext, string[] paths)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            ConfigurationFilePaths = paths ?? throw new ArgumentNullException(nameof(paths));

            _sourceValues = new CacheContext<string, string>();
            _currentValues = _sourceValues;
            
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            
            //not supported in *nix.
            //Named synchronization primitives are not supported on this platform
            //_interLock = new EventWaitHandle(false, EventResetMode.AutoReset, InterSynchronizationName);
        }


        public void Load()
        {
            //_interLock.WaitOne(_interLockTimeout);

            try
            {
                if (ApplicationContext.ReadOnlyConfiguration)
                {
                    //TODO: load all config files.
                    OverrideConfiguration(new FileStream(ConfigurationFilePaths.First(), FileMode.Open, FileAccess.Read, FileShare.Read));
                }
                else
                {
                    //TODO: load all config files.
                    OverrideConfiguration(new FileStream(ConfigurationFilePaths.First(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read));
                }
            }
            catch (IOException exception)
            {
                //LogChannel.Log(exception);
            }
            //finally
            {
                //_interLock.Set();
            }
        }

        /// <summary>
        /// Loads the configuration **-WITH-NO-LOCK-** !!
        /// </summary>
        /// <param name="stream"></param>
        protected void OverrideConfiguration(Stream stream)
        {
            var jsonObject = JObject.Load(new JsonTextReader(new StreamReader(stream)));
            var jTokens = jsonObject.Descendants().Where(p => !p.Any());
            var results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
            {
                properties.Add(jToken.Path, jToken.ToString());
                return properties;
            });

            _lock.EnterWriteLock();

            try
            {
                _sourceValues = new CacheContext<string, string>(results);
                _currentValues = _sourceValues;
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            GC.KeepAlive(results);
        }


        public void WriteToFile()
        {
            //_interLock.WaitOne(_interLockTimeout);

            //try
            {
            }
            //finally
            {
                
                //_interLock.Set();
            }
        }


        public bool TryGetValue(object context, string fqn, out string value)
        {
            _lock.EnterReadLock();

            try
            {
                return _currentValues.TryGetValue(fqn, out value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        public TValue GetValue<TValue>(object context, string fqn, TValue? defaultValue = null)
            where TValue : struct
        {
            _lock.EnterReadLock();

            try
            {
                return _currentValues.TryGetValue(fqn, out var value)
                    ? (TValue) Convert.ChangeType(value, typeof(TValue))
                    : defaultValue ?? throw new InvalidOperationException();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public TValue GetValue<TValue>(object context, string fqn, TValue defaultValue = null)
            where TValue : class
        {
            _lock.EnterReadLock();

            try
            {
                return _currentValues.TryGetValue(fqn, out var value)
                    ? (TValue) Convert.ChangeType(value, typeof(TValue))
                    : defaultValue ?? throw new InvalidOperationException();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        public void SetValue<TValue>(object context, string fqn, TValue value, TValue[] availableValues = null)
        {
            _lock.EnterWriteLock();

            try
            {
                _currentValues.Cache(fqn, value.ToString());

                if (_commitChangesToConfigFile)
                {
                    WriteToFile();
                }
            }
            finally
            {
                _lock.EnterWriteLock();
            }
        }


        public void SetAllValues(object context, IDictionary<string, object> values)
        {
            _lock.EnterWriteLock();

            try
            {
                foreach (var kv in values)
                {
                    _currentValues.Cache(kv.Key, kv.Value?.ToString());
                }

                if (_commitChangesToConfigFile)
                {
                    WriteToFile();
                }
            }
            finally
            {
                _lock.EnterWriteLock();
            }
        }


        private class ConfigTransaction : ITransaction
        {
            public JsonConfigProvider JsonConfigProvider { get; }


            public ConfigTransaction(JsonConfigProvider jsonConfigProvider)
            {
                JsonConfigProvider = jsonConfigProvider;
            }


            public void Commit()
            {
                JsonConfigProvider._lock.EnterWriteLock();

                try
                {
                    JsonConfigProvider._currentValues = JsonConfigProvider._sourceValues;
                    JsonConfigProvider._commitChangesToConfigFile = true;
                }
                finally
                {
                    JsonConfigProvider._lock.EnterWriteLock();
                }
            }

            public void Rollback()
            {
            }

            public void Dispose()
            {
                Commit();
            }
        }

        public void InstallValue<TValue>(object context, string componentGuid, string fqn, TValue value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, TValue[] availableValues = null)
        {
        }

        public void UnInstallValue(object context, string componentGuid, string fqn)
        {
        }


        #region IConfigProvider implementation

        public ITransaction BeginTransaction()
        {
            _lock.EnterWriteLock();

            try
            {
                return new ConfigTransaction(this);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void SyncConfig()
        {
            Load();
        }

        public bool GetBool(object context, string fqn, bool? defaultValue = null)
            => GetValue(context, fqn, defaultValue);

        public int GetInt(object context, string fqn, int? defaultValue = null)
            => GetValue(context, fqn, defaultValue);

        public long GetLong(object context, string fqn, long? defaultValue = null)
            => GetValue(context, fqn, defaultValue);

        public float GetFloat(object context, string fqn, float? defaultValue = null)
            => GetValue(context, fqn, defaultValue);

        public double GetDouble(object context, string fqn, double? defaultValue = null)
            => GetValue(context, fqn, defaultValue);

        public string GetString(object context, string fqn, string defaultValue = null)
            => GetValue(context, fqn, defaultValue);


        public void SetBool(object context, string fqn, bool value)
            => SetValue(context, fqn, value);

        public void SetInt(object context, string fqn, int value, int[] availableValues = null)
            => SetValue(context, fqn, value, availableValues);

        public void SetLong(object context, string fqn, long value, long[] availableValues = null)
            => SetValue(context, fqn, value, availableValues);

        public void SetFloat(object context, string fqn, float value, float[] availableValues = null)
            => SetValue(context, fqn, value, availableValues);

        public void SetDouble(object context, string fqn, double value, double[] availableValues = null)
            => SetValue(context, fqn, value, availableValues);

        public void SetString(object context, string fqn, string value, string[] availableValues = null)
            => SetValue(context, fqn, value, availableValues);


        public void InstallBool(object context, string componentGuid, string fqn, bool value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup)
            => InstallValue(context, componentGuid, fqn, value, strategy);

        public void InstallInt(object context, string componentGuid, string fqn, int value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, int[] availableValues = null)
            => InstallValue(context, componentGuid, fqn, value, strategy, availableValues);

        public void InstallLong(object context, string componentGuid, string fqn, long value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, long[] availableValues = null)
            => InstallValue(context, componentGuid, fqn, value, strategy, availableValues);

        public void InstallFloat(object context, string componentGuid, string fqn, float value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, float[] availableValues = null)
            => InstallValue(context, componentGuid, fqn, value, strategy, availableValues);

        public void InstallDouble(object context, string componentGuid, string fqn, double value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, double[] availableValues = null)
            => InstallValue(context, componentGuid, fqn, value, strategy, availableValues);

        public void InstallString(object context, string componentGuid, string fqn, string value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, string[] availableValues = null)
            => InstallValue(context, componentGuid, fqn, value, strategy, availableValues);


        public void UnInstallBool(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        public void UnInstallInt(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        public void UnInstallLong(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        public void UnInstallFloat(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        public void UnInstallDouble(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        public void UnInstallString(object context, string componentGuid, string fqn)
            => UnInstallValue(context, componentGuid, fqn);

        #endregion

        #region IFeature implementation

        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            //configProvider is this same class.
            //DONT WRITE ANYTHING HERE.
        }

        #endregion
    }
}