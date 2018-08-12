using System;
using AMKDownloadManager.UI.Business;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;

namespace AMKDownloadManager.UI.Xamarin
{
    // ReSharper disable once InconsistentNaming
    public static class UIInitializer
    {
        public static App CreateApp(string[] args)
        {
            if (AppInitializer.InitializeApplication(args, out var appContext))
            {
                if (!(appContext.TypeResolver is ITypeResolverContainer typeResolver))
                {
                    throw new InvalidOperationException();
                }

                AppInitializer.BuildDependencies(appContext, typeResolver);
                DependencyBuilder.BuildContainer(appContext, typeResolver);

                var app = typeResolver.Resolve<App>();
                
                return app;
            }

            return null;
        }
    }
}