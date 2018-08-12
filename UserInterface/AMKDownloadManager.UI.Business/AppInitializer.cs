using System;
using System.IO;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.UI.Business.Services;
using AMKDownloadManager.UI.Business.ViewModels.Downloads;
using AMKDownloadManager.UI.Business.ViewModels.Main;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Trace;

namespace AMKDownloadManager.UI.Business
{
    public static class AppInitializer
    {
        public static bool InitializeApplication(string[] args, out IApplicationContext appContext)
        {
            appContext = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            
            AppHelpers.InjectTopLayerFeatures(appContext);

            var logger = appContext.GetFeature<ILogger>();
            Logger.RegisterDefaultLogChannel(logger);
            
            AppHelpers.LoadComponents(appContext,
                Path.Combine(ApplicationContext.ApplicationProfileDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                Path.Combine(ApplicationContext.ApplicationSharedProfileDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                new []
                {
                    Path.Combine(ApplicationContext.ApplicationDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                    ApplicationContext.ApplicationPluginRepository
                }
                );
            
            AppHelpers.ConfigureFeatures(appContext);

            return true;
        }

        public static void BuildDependencies(IApplicationContext appContext, ITypeResolverContainer container)
        {
            //Singletons
            container.RegisterSingleton<IApplicationContext>(appContext);
            container.RegisterSingleton<ITypeResolver>(container);

            //Services
            container.RegisterType<IDownloadStateService, DownloadStateService>();
            
            //ViewModels
            container.RegisterType<MainWindowViewModel>();
            container.RegisterType<DownloadCategoriesViewModel>();
        }
    }
}