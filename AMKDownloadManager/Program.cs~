using System;
using Gtk;
using System.Collections.Generic;
using System.Globalization;
using ir.amkdp.gear.core.Text.Formatters;
using AMKDownloadManager.Core;
using AMKDownloadManager.Threading;
using AMKDownloadManager.Shell;
using System.Linq;

namespace AMKDownloadManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());

            if (args.Any(x => ShellCommands.ShellActivatorCommand.Contains(x.ToLower())))
            {
                var host = new ShellHost(pool);
                host.ExecuteCommand(args);
            }
            else
            {
                Application.Init();
                Application.Run();
            }
        }
    }
}