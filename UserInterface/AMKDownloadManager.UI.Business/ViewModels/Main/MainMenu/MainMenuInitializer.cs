using System.Collections.Generic;

namespace AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu
{
    public static class MainMenuInitializer
    {
        public static IList<MenuItemViewModel> CreateMainMenu()
        {
            var items = new List<MenuItemViewModel>();

            items.Add(new MenuItemViewModel("File")
            {
                Children = new List<MenuItemViewModel>(new []
                {
                    new MenuItemViewModel("Test"), 
                    new MenuItemViewModel("Jack"), 
                })
            });
            items.Add(new MenuItemViewModel("Edit")
            {
                Children = new List<MenuItemViewModel>(new []
                {
                    new MenuItemViewModel("Test1"), 
                    new MenuItemViewModel("Jack2"), 
                })
            });

            return items; //new List<MenuItemViewModel>(items);
        }
    }
}