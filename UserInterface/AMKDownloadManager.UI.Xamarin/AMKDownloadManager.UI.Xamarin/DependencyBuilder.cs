using System;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.UI.Business.ViewModels.Main;
using AMKDownloadManager.UI.Xamarin.Views.Main.Desktop;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Automation.IoC.TypeBindings;
using Xamarin.Forms;

namespace AMKDownloadManager.UI.Xamarin
{
    public static class DependencyBuilder
    {
        public const string MainPageKey = "MainPage";
        
        public static void BuildContainer(
            IApplicationContext appContext,
            ITypeResolverContainer container)
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Unsupported:
                    throw new NotSupportedException();
                    break;
                case TargetIdiom.Phone:
                    throw new NotSupportedException();
                    break;
                case TargetIdiom.Tablet:
                    throw new NotSupportedException();
                    break;
                case TargetIdiom.Desktop:
                    BuildContainerDesktop(appContext, container);
                    break;
                case TargetIdiom.TV:
                    throw new NotSupportedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
        private static void BuildContainerDesktop(
            IApplicationContext appContext,
            ITypeResolverContainer container)
        {
            container.RegisterType<Page, MainPage>(); //temporary to fix the ioc container. 
            
            //container.RegisterType<App>();
            //container.BindProperty<App, ViewLocator>(x => x.DataTemplates);
            
            container.RegisterType<Sidebar>(MainPageKey);
            container.BindProperty<Sidebar, MainWindowViewModel>(x => x.BindingContext);
        }
    }
}