using System;
using System.IO;
using AMKDownloadManager.Core;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.Threading;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Patterns.AppModel;
using AppContext = AMKDownloadManager.Core.AppContext;

namespace AMKDownloadManager.UI.Business
{
    public static class AppInitializer
    {
        public static bool InitializeApplication(string[] args)
        {
            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            
            AppHelpers.InjectTopLayerFeatures(pool);
            
            AppHelpers.LoadComponents(pool,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppContext.ApplicationProfileDirectoryName),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppContext.ApplicationProfileDirectoryName),
                new [] { Environment.GetEnvironmentVariable(AppContext.PluginRepositoryEnvironmentVariableName) }
                );
            
            AppHelpers.ConfigureFeatures(pool);

            return true;
        }
    }
}