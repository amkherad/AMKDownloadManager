using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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


        public event EventHandler Closing;


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

        public void SignalReceived(ApplicationSignals signal)
        {
            switch (signal)
            {
                case ApplicationSignals.Quit:
                case ApplicationSignals.Terminate:
                {
                    Closing?.Invoke(this, EventArgs.Empty);
                    DisposableContainer.Dispose();
                    
                    AbortBackgroundThreads();

                    if (signal != ApplicationSignals.Terminate)
                    {
                        JoinForegroundThreads();
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(signal), signal, null);
            }
        }


        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}