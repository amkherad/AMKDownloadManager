using Avalonia;
using Avalonia.Markup.Xaml;

namespace AMKDownloadManager.AvaloniaUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}