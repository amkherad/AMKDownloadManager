using AMKDownloadManager.Core.Api;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using Xamarin.Forms;

namespace AMKDownloadManager.UI.Xamarin
{
    public partial class App : Application
    {
        public App(
            IApplicationContext applicationContext,
            ITypeResolver typeResolver
            )
        {
            InitializeComponent();

            var mainPage = typeResolver.Resolve<Page>(DependencyBuilder.MainPageKey);
            MainPage = mainPage;

            //MainPage = new NavigationPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}