using System;
using AMKDownloadManager.Core.Api;
using System.Linq;
using System.Reflection;
using Gtk;
using System.IO;

namespace AMKDownloadManager.Shell
{
    public class ShellHost
    {
        public const string HelpFileResourceName = "Help-Cli.txt";

        public IAppContext AppContext { get; }

        public ShellHost(IAppContext appContext)
        {
            AppContext = appContext;
        }

        public void ExecuteCommand(string[] args)
        {
            try
            {
                _executeCommand(args);
            }
            catch (Exception ex)
            {
                EchoException(ex);
            }
        }

        public void _executeCommand(string[] args)
        {
            if (args.Any(x => ShellCommands.HelpCommand.Contains(x.ToLower())))
            {
                EchoHelp();
            }
        }

        public void EchoHelp()
        {
            using (var file = Assembly.GetExecutingAssembly().GetManifestResourceStream(HelpFileResourceName))
            {
                using (var reader = new StreamReader(file))
                {
                    Console.Write(reader.ReadToEnd());
                }
            }
        }

        public void EchoError(string message)
        {
            Console.Write($"Error: {message}");
        }

        public void EchoException(Exception exception)
        {
            Console.Write($"Error: {exception.Message}");
        }
    }
}

