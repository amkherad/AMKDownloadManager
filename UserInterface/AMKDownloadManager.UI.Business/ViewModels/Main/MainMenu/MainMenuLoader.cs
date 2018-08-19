using System.Collections.Generic;
using System.Collections.ObjectModel;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Messaging;
using AMKDownloadManager.UI.Business.Models;
using AMKDownloadManager.UI.Business.Services;

namespace AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu
{
    public class MainMenuLoader : IDataLoader<MenuItemModel>
    {
        public IApplicationContext AppContext { get; }
        public IMessagingHost MessagingHost { get; }

        public MainMenuLoader(
            IApplicationContext appContext,
            IMessagingHost messagingHost
            )
        {
            AppContext = appContext;
            MessagingHost = messagingHost;
        }

        
        public IEnumerable<MenuItemModel> Load()
        {
            var items = new List<MenuItemModel>();

            items.Add(new MenuItemModel("File")
            {
                Children = new List<MenuItemModel>(new []
                {
                    new MenuItemModel("Test"), 
                    new MenuItemModel("Jack"), 
                })
            });
            items.Add(new MenuItemModel("Edit")
            {
                Children = new List<MenuItemModel>(new []
                {
                    new MenuItemModel("Test1"), 
                    new MenuItemModel("Jack2"), 
                })
            });

            return items; //new List<MenuItemModel>(items);
        }

        public ObservableCollection<MenuItemModel> LoadAndTrack()
        {
            throw new System.NotImplementedException();
        }
    }
}