using System;
using System.IO;
using System.Reflection;
using AMKDownloadManager.Core.Api;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core
{
    public class ApplicationContext : AppModelContext, IApplicationContext
    {
        public const string ApplicationProfileDirectoryName = "amkdownloadmanager";
        public const string PluginRepositoryEnvironmentVariableName = "AMKDM_PLUGIN_REPOSITORY";
        
        public const string ApplicationPluginsSubDirectoryName = "plugins";

        public const string ApplicationConfigurationFileName = "config.json";


        public static bool ReadOnlyConfiguration { get; set; }

        public static string ApplicationDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string ApplicationProfileDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ApplicationProfileDirectoryName);

        public static string ApplicationSharedProfileDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            ApplicationProfileDirectoryName);

        public static string ApplicationPluginRepository =>
            Environment.GetEnvironmentVariable(PluginRepositoryEnvironmentVariableName);


        public static string ApplicationConfigurationFilePath =>
            Path.Combine(ApplicationProfileDirectory, ApplicationConfigurationFileName);

        public static string ApplicationSharedConfigurationFilePath =>
            Path.Combine(ApplicationSharedProfileDirectory, ApplicationConfigurationFileName);
    }
}