using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Main
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}