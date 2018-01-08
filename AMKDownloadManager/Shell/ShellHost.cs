using System;
using System.Collections.Generic;
using System.Diagnostics;
using AMKDownloadManager.Core.Api;
using System.Linq;
using System.Text.RegularExpressions;
using AMKDownloadManager.Shell.Commands;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Shell
{
    public class ShellHost
    {
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
            var regex = new Regex(
                "^(?:(?:[, ]+)?(?\'q\'\")?(?\'key\'[^=\"]*?)(?:\\k\'q\'(?\'-q\'))?=(?\'q\'\")?(?\'value\'(?:[^\"]|(?<=\\\\)\")*)(?:\\k\'q\'(?\'-q\'))?)*(?(q)(?!))$",
                RegexOptions.Compiled);

            var argValues = new PropertyBag<string>();
            
            foreach (var arg in args)
            {
                var match = regex.Match(arg);

                if (match.Success)
                {
                    var keys = match.Groups["key"].Captures;
                    var values = match.Groups["value"].Captures;

                    for (int i = 0; i < keys.Count; i++)
                    {
                        argValues.Add(keys[i].Value, values[i].Value);
                    }
                }
            }

            IShellCommand command = null;
            
            if (argValues.ContainsKey(ShellCommands.ReportCommand[0]))
            {
                command = new ShellReporter();
            }
            
            if (args.Any(x => ShellCommands.HelpCommand.Contains(x.ToLower())))
            {
                command = new HelpCommand();
            }

            if (command != null)
            {
                command.Handle(this, args, argValues);
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

