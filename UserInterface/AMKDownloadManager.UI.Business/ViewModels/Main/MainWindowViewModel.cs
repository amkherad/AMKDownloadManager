using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.UI.Business.ViewModels.Main.Layout;
using AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu;

namespace AMKDownloadManager.UI.Business.ViewModels.Main
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        public IApplicationContext AppContext { get; }


        public IList<MenuItemViewModel> MainMenuItems { get; set; }
        public MainWindowLayoutSettings LayoutSettings { get; }

        
        public MainWindowViewModel(IApplicationContext appContext)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

            Title = "AMKDownloadManager";

            MainMenuItems = MainMenuInitializer.CreateMainMenu();
            LayoutSettings = new MainWindowLayoutSettings(appContext);
        }
    }
}