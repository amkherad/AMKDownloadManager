using AMKDownloadManager.AvaloniaUI.ViewModels;
using Avalonia;
using Avalonia.Logging.Serilog;
using AMKDownloadManager.AvaloniaUI.Views;
using AMKDownloadManager.Core;
using AMKDownloadManager.UI.Business;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Patterns.AppModel;

namespace AMKDownloadManager.AvaloniaUI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (AppInitializer.InitializeApplication(args))
            {
                var typeResolver = AppContext.Context.GetTypeResolver() as ITypeResolverContainer;
                DependencyGraph.BuildContainer(typeResolver);

                var app = BuildAvaloniaApp(typeResolver);
                app.Start<MainWindow>(() => typeResolver.Resolve<MainWindowViewModel>());
            }
        }

        public static AppBuilder BuildAvaloniaApp(ITypeResolver typeResolver)
            => AppBuilder.Configure<App>()//(typeResolver.Resolve<App>())
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
