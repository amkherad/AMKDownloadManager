using System;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application configuration manager.
    /// </summary>
    public interface IConfigProvider : IFeature
    {
        bool GetBool(string fqn, bool? defaultValue = null);
        int GetInt(string fqn, int? defaultValue = null);
        long GetLong(string fqn, long? defaultValue = null);
        float GetFloat(string fqn, float? defaultValue = null);
        double GetDouble(string fqn, double? defaultValue = null);
        string GetString(string fqn, string defaultValue = null);

        void SetBool(string fqn, bool value);
        void SetInt(string fqn, int value, int[] availableValues = null);
        void SetLong(string fqn, long value, long[] availableValues = null);
        void SetFloat(string fqn, float value, float[] availableValues = null);
        void SetDouble(string fqn, double value, double[] availableValues = null);
        void SetString(string fqn, string value, string[] availableValues = null);

        void InstallBool(string componentGUID, string fqn, bool value);
        void InstallInt(string componentGUID, string fqn, int value, int[] availableValues = null);
        void InstallLong(string componentGUID, string fqn, long value, long[] availableValues = null);
        void InstallFloat(string componentGUID, string fqn, float value, float[] availableValues = null);
        void InstallDouble(string componentGUID, string fqn, double value, double[] availableValues = null);
        void InstallString(string componentGUID, string fqn, string value, string[] availableValues = null);
    }
}