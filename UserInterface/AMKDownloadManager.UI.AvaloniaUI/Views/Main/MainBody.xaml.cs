using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Main
{
    public class MainContent : UserControl
    {
        public MainContent()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}