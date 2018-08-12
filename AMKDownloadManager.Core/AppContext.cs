using System;
using System.IO;
using System.Reflection;
using AMKDownloadManager.Core.Api;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.Core
{
    public partial class ApplicationContext : AppModelContext, IApplicationContext
    {
        public const string ApplicationProfileDirectoryName = "amkdownloadmanager";
        public const string PluginRepositoryEnvironmentVariableName = "AMKDM_PLUGIN_REPOSITORY";

        public const string ApplicationPluginsSubDirectoryName = "plugins";
        public const string ApplicationLocksSubDirectoryName = "_locks";

        public const string ApplicationConfigurationFileName = "config.json";


        public static bool ReadOnlyConfiguration { get; set; }

        public string ApplicationDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public string ApplicationLockDirectory { get; set; }

        public string ApplicationProfileDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ApplicationProfileDirectoryName);

        public string ApplicationSharedProfileDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            ApplicationProfileDirectoryName);

        public string ApplicationPluginRepository =>
            Environment.GetEnvironmentVariable(PluginRepositoryEnvironmentVariableName);


        public string ApplicationConfigurationFilePath =>
            Path.Combine(ApplicationProfileDirectory, ApplicationConfigurationFileName);

        public string ApplicationSharedConfigurationFilePath =>
            Path.Combine(ApplicationSharedProfileDirectory, ApplicationConfigurationFileName);


        public ApplicationContext()
        {
            ApplicationLockDirectory = Path.Combine(ApplicationProfileDirectory, ApplicationLocksSubDirectoryName);
        }
    }
}