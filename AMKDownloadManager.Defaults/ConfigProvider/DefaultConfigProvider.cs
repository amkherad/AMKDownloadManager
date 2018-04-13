using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;

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

        public void InstallBool(object context, string componentGuid, string fqn, bool value)
        {
            throw new NotImplementedException();
        }

        public void InstallInt(object context, string componentGuid, string fqn, int value, int[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallLong(object context, string componentGuid, string fqn, long value, long[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallFloat(object context, string componentGuid, string fqn, float value, float[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallDouble(object context, string componentGuid, string fqn, double value, double[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallString(object context, string componentGuid, string fqn, string value, string[] availableValues = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IFeature implementation

        public int Order => 0;

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            //DONT WRITE ANYTHING HERE.
        }

        #endregion
    }
}