using System.Collections.Generic;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Data;
using AMKsGear.Core.Data;

namespace AMKDownloadManager.ConfigProvider.Json
{
    public class JsonConfigProvider : IConfigProvider
    {
        private ICacheContext<string, string> _cachedValues;
        private ReaderWriterLockSlim _lock;

        
        public JsonConfigProvider()
        {
            _cachedValues = new CacheContext<string, string>();
            _lock = new ReaderWriterLockSlim();
        }
        
        

        public bool TryGetValue(object context, string fqn, out string value)
        {
            _lock.EnterReadLock();

            try
            {
                if(_cachedValues.TryGet())
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return default(TValue);
        }
        

        public TValue GetValue<TValue>(object context, string fqn, TValue? defaultValue = null)
            where TValue : struct
        {
            _lock.EnterReadLock();

            try
            {
                
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return default(TValue);
        }
        
        public TValue GetValue<TValue>(object context, string fqn, TValue defaultValue = null)
            where TValue : class
        {

            return default(TValue);
        }

        
        public TValue SetValue<TValue>(object context, string fqn, TValue value, TValue[] availableValues = null)
        {
            _lock.EnterWriteLock();

            try
            {

            }
            finally
            {
                _lock.EnterWriteLock();
            }
            return default(TValue);
        }

        public void InstallValue<TValue>(object context, string componentGuid, string fqn, TValue value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, TValue[] availableValues = null)
        {
            
        }

        public void UnInstallValue(object context, string componentGuid, string fqn)
        {
            
        }
        
        
        #region IConfigProvider implementation

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

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            //configProvider is this same class.
            //DONT WRITE ANYTHING HERE.
        }

        #endregion
    }
}