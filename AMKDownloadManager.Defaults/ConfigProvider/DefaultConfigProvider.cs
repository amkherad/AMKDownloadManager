using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Architecture.Patterns;

namespace AMKDownloadManager.Defaults.ConfigProvider
{
    /// <summary>
    /// Default application configuration manager.
    /// </summary>
    public class DefaultConfigProvider : IConfigProvider
    {
        public DefaultConfigProvider()
        {
        }

        #region IConfigProvider implementation

        public ITransaction BeginTransaction()
        {
            return null;
        }

        public void SyncConfig()
        {
        }

        public bool GetBool(object context, string fqn, bool? defaultValue = null)
        {
            return defaultValue ?? false;
        }

        public int GetInt(object context, string fqn, int? defaultValue = null)
        {
            return defaultValue ?? 3;
        }

        public long GetLong(object context, string fqn, long? defaultValue = null)
        {
            return defaultValue ?? 4;
        }

        public float GetFloat(object context, string fqn, float? defaultValue = null)
        {
            return defaultValue ?? 4;
        }

        public double GetDouble(object context, string fqn, double? defaultValue = null)
        {
            return defaultValue ?? 4;
        }

        public string GetString(object context, string fqn, string defaultValue = null)
        {
            return defaultValue;
        }

        public void SetBool(object context, string fqn, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetInt(object context, string fqn, int value, int[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetLong(object context, string fqn, long value, long[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetFloat(object context, string fqn, float value, float[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetDouble(object context, string fqn, double value, double[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetString(object context, string fqn, string value, string[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallBool(object context, string componentGuid, string fqn, bool value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup)
        {
            throw new NotImplementedException();
        }

        public void InstallInt(object context, string componentGuid, string fqn, int value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, int[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallLong(object context, string componentGuid, string fqn, long value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, long[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallFloat(object context, string componentGuid, string fqn, float value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, float[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallDouble(object context, string componentGuid, string fqn, double value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, double[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallString(object context, string componentGuid, string fqn, string value,
            ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, string[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void UnInstallBool(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

        public void UnInstallInt(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

        public void UnInstallLong(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

        public void UnInstallFloat(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

        public void UnInstallDouble(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

        public void UnInstallString(object context, string componentGuid, string fqn)
        {
            throw new NotImplementedException();
        }

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