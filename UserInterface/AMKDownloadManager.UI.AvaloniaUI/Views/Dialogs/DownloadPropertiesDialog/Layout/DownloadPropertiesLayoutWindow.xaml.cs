using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.UI.AvaloniaUI.Views.Dialogs.DownloadPropertiesDialog.Layout
{
    public class DownloadPropertiesLayoutWindow : Window
    {
        public DownloadPropertiesLayoutWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}