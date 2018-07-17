using AMKDownloadManager.UI.AvaloniaUI.Views.Main;
using AMKDownloadManager.UI.Business.ViewModels.Main;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Automation.IoC.Builder;
using AMKsGear.Core.Automation.IoC.TypeBindings;

namespace AMKDownloadManager.UI.AvaloniaUI
{
    public static class DependencyGraph
    {
        public static void BuildContainer(ITypeResolverContainer container)
        {
            container.RegisterSingleton<ITypeResolver>(container);
            
            //container.RegisterType<App>();
            //container.BindProperty<App, ViewLocator>(x => x.DataTemplates);
            
            container.RegisterType<MainWindow>();
            container.BindProperty<MainWindow, MainWindowViewModel>(x => x.DataContext);
            
            
        }
    }
}