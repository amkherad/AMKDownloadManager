namespace AMKDownloadManager.Core.Api.Configuration
{
    public enum ConfigExistenceStrategy
    {
        /// <summary>
        /// Overwrite previous value.
        /// </summary>
        Overwrite,
        
        /// <summary>
        /// Ignore this value and use previous value.
        /// </summary>
        IgnoreValue,
        
        /// <summary>
        /// Shift previous value to available values and use this as current value.
        /// </summary>
        UseThis_Backup
    }
    
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

        void InstallBool(object context, string componentGuid, string fqn, bool value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup);
        void InstallInt(object context, string componentGuid, string fqn, int value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, int[] availableValues = null);
        void InstallLong(object context, string componentGuid, string fqn, long value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, long[] availableValues = null);
        void InstallFloat(object context, string componentGuid, string fqn, float value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, float[] availableValues = null);
        void InstallDouble(object context, string componentGuid, string fqn, double value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, double[] availableValues = null);
        void InstallString(object context, string componentGuid, string fqn, string value, ConfigExistenceStrategy strategy = ConfigExistenceStrategy.UseThis_Backup, string[] availableValues = null);
        
        void UnInstallBool(object context, string componentGuid, string fqn);
        void UnInstallInt(object context, string componentGuid, string fqn);
        void UnInstallLong(object context, string componentGuid, string fqn);
        void UnInstallFloat(object context, string componentGuid, string fqn);
        void UnInstallDouble(object context, string componentGuid, string fqn);
        void UnInstallString(object context, string componentGuid, string fqn);
    }
}