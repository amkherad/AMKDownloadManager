using AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu;

namespace AMKDownloadManager.UI.Business.ViewModels.Main
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        public MenuItemViewModel[] MainMenuItems { get; set; }

        public MainWindowViewModel()
        {
            Title = "AMKDownloadManager";
        }
    }
}
