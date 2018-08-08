using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Main.Content
{
    public class DownloadGrid : UserControl
    {
        public DownloadGrid()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}