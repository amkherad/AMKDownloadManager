using System.Collections.Generic;
using AMKDownloadManager.UI.Business.Models;

namespace AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu
{
    public static class MainMenuInitializer
    {
        public static IList<MenuItemModel> CreateMainMenu()
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
    }
}