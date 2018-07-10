using AMKDownloadManager.AvaloniaUI.ViewModels;
using AMKDownloadManager.AvaloniaUI.Views;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Automation.IoC.TypeBindings;

namespace AMKDownloadManager.AvaloniaUI
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