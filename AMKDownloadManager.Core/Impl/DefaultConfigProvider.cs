using System;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core.Impl
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

        public bool GetBool(string fqn, bool? defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string fqn, int? defaultValue = null)
        {
            return defaultValue ?? 3;
        }

        public long GetLong(string fqn, long? defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(string fqn, float? defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(string fqn, double? defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public string GetString(string fqn, string defaultValue = null)
        {
            return defaultValue;
        }

        public void SetBool(string fqn, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetInt(string fqn, int value, int[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetLong(string fqn, long value, long[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetFloat(string fqn, float value, float[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetDouble(string fqn, double value, double[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void SetString(string fqn, string value, string[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallBool(string componentGUID, string fqn, bool value)
        {
            throw new NotImplementedException();
        }

        public void InstallInt(string componentGUID, string fqn, int value, int[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallLong(string componentGUID, string fqn, long value, long[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallFloat(string componentGUID, string fqn, float value, float[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallDouble(string componentGUID, string fqn, double value, double[] availableValues = null)
        {
            throw new NotImplementedException();
        }

        public void InstallString(string componentGUID, string fqn, string value, string[] availableValues = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IFeature implementation

        public int Order => 0;

        #endregion
    }
}