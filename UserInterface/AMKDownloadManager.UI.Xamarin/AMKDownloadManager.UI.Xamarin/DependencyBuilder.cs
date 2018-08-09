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
                    break;
                case TargetIdiom.Phone:
                    break;
                case TargetIdiom.Tablet:
                    break;
                case TargetIdiom.Desktop:
                    BuildContainerDesktop(appContext, container);
                    break;
                case TargetIdiom.TV:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
        private static void BuildContainerDesktop(
            IApplicationContext appContext,
            ITypeResolverContainer container)
        {
            container.RegisterSingleton<IApplicationContext>(appContext);
            container.RegisterSingleton<ITypeResolver>(container);
            
            //container.RegisterType<App>();
            //container.BindProperty<App, ViewLocator>(x => x.DataTemplates);
            
            container.RegisterType<MainBody>(MainPageKey);
            container.BindProperty<MainBody, MainWindowViewModel>(x => x.BindingContext);
        }
    }
}