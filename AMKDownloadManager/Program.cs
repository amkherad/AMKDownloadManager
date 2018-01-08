using Gtk;
using AMKDownloadManager.Core;
using AMKDownloadManager.Shell;
using System.Linq;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.HttpDownloader.AddIn;

namespace AMKDownloadManager
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            args = new []
            {
                "-cli",
                "-report=h",
                //"--help"
            };
            
            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            AppHelpers.InjectTopLayerFeatures(pool);
            //AppHelpers.LoadComponents(pool);
            var c = new HttpDownloaderComponent();
            c.Initialize(pool);
            AppHelpers.ConfigureFeatures(pool);
            
            if (args.Any(x => ShellCommands.ShellActivatorCommand.Contains(x.ToLower())))
            {
                var host = new ShellHost(pool);
                host.ExecuteCommand(args);
            }
            else
            {
                Application.Init();
                
                var mainWindow = new MainWindow();
                mainWindow.Show();
                
                Application.Run();
            }
        }
    }
}