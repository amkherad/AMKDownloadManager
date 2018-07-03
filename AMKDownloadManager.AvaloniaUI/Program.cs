using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using AMKDownloadManager.AvaloniaUI.ViewModels;
using AMKDownloadManager.AvaloniaUI.Views;

namespace AMKDownloadManager.AvaloniaUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("test");
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
