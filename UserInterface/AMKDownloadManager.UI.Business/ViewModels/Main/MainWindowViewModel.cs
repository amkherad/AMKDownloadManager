using System;
using System.Collections.ObjectModel;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.UI.Business.Models;
using AMKDownloadManager.UI.Business.Models.Downloads;
using AMKDownloadManager.UI.Business.Services;
using AMKDownloadManager.UI.Business.ViewModels.Downloads;
using AMKDownloadManager.UI.Business.ViewModels.Main.Layout;

namespace AMKDownloadManager.UI.Business.ViewModels.Main
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        public IApplicationContext AppContext { get; }


        public ObservableCollection<MenuItemModel> MainMenuItems { get; set; }
        public MainWindowLayoutSettings LayoutSettings { get; }
        
        public DownloadManagerContentViewModel Content { get; }

        public MainWindowViewModel(
            IApplicationContext appContext,
            IDataLoader<MenuItemModel> menuItemsLoader,
            MainWindowLayoutSettings layoutSettings,
            
            IConfigDataEntryService<DownloadCategoryItem> categoryService,
            DownloadManagerContentViewModel content
            )
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

            Title = "AMKDownloadManager";
            
            MainMenuItems = new ObservableCollection<MenuItemModel>(menuItemsLoader.Load());
            LayoutSettings = layoutSettings;
            
            Content = content;
        }
    }
}