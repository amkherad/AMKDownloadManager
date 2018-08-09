using AMKDownloadManager.Core.Api;
using AMKDownloadManager.UI.AvaloniaUI.Views.Main;
using AMKDownloadManager.UI.Business.ViewModels.Main;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Automation.IoC.TypeBindings;

namespace AMKDownloadManager.UI.AvaloniaUI
{
    public static class DependencyBuilder
    {
        public static void BuildContainer(
            IApplicationContext appContext,
            ITypeResolverContainer container)
        {
            container.RegisterSingleton<IApplicationContext>(appContext);
            container.RegisterSingleton<ITypeResolver>(container);
            
            //container.RegisterType<App>();
            //container.BindProperty<App, ViewLocator>(x => x.DataTemplates);
            
            container.RegisterType<MainWindow>();
            container.BindProperty<MainWindow, MainWindowViewModel>(x => x.DataContext);
            
            
        }
    }
}