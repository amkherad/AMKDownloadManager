using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Main
{
    public class MainBody : UserControl
    {
        public MainBody()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}