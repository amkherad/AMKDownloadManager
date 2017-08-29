using System;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Application configuration manager.
    /// </summary>
    public interface IConfigProvider : IFeature
    {
        bool GetBool(object context, string fqn, bool? defaultValue = null);
        int GetInt(object context, string fqn, int? defaultValue = null);
        long GetLong(object context, string fqn, long? defaultValue = null);
        float GetFloat(object context, string fqn, float? defaultValue = null);
        double GetDouble(object context, string fqn, double? defaultValue = null);
        string GetString(object context, string fqn, string defaultValue = null);

        void SetBool(object context, string fqn, bool value);
        void SetInt(object context, string fqn, int value, int[] availableValues = null);
        void SetLong(object context, string fqn, long value, long[] availableValues = null);
        void SetFloat(object context, string fqn, float value, float[] availableValues = null);
        void SetDouble(object context, string fqn, double value, double[] availableValues = null);
        void SetString(object context, string fqn, string value, string[] availableValues = null);

        void InstallBool(object context, string componentGuid, string fqn, bool value);
        void InstallInt(object context, string componentGuid, string fqn, int value, int[] availableValues = null);
        void InstallLong(object context, string componentGuid, string fqn, long value, long[] availableValues = null);
        void InstallFloat(object context, string componentGuid, string fqn, float value, float[] availableValues = null);
        void InstallDouble(object context, string componentGuid, string fqn, double value, double[] availableValues = null);
        void InstallString(object context, string componentGuid, string fqn, string value, string[] availableValues = null);
    }
}