using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Main
{
    public class MainToolbar : UserControl
    {
        public MainToolbar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}