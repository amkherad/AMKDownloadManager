using System;
using Gtk;
using System.Collections.Generic;
using System.Globalization;
using ir.amkdp.gear.core.Text.Formatters;
using AMKDownloadManager.Core;
using AMKDownloadManager.Threading;
using AMKDownloadManager.Shell;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Network;

namespace AMKDownloadManager
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            InjectTopLayerFeatures(pool);
            LoadComponents(pool);
            
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

        public static void LoadComponents(IAppContext appContext)
        {
            var importer = new ComponentImporter();
            importer.Compose();
            Console.WriteLine("{0} component(s) are imported successfully.", importer.AvailableNumberOfComponents);
            
            importer.InitializeAll(appContext);
        }

        public static void InjectTopLayerFeatures(IAppContext appContext)
        {
            appContext.AddFeature<IRequestBarrier>(new DefaultHttpRequestBarrier());
            appContext.AddFeature<IHttpRequestBarrier>(new DefaultHttpRequestBarrier());
            appContext.AddFeature<INetworkInterfaceProvider>(new NetworkInterfaceProvider());
        }
    }
}