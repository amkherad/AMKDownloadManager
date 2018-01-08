using System;
using System.IO;
using System.Reflection;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Shell.Commands
{
    public class HelpCommand : IShellCommand
    {
        public const string HelpFileResourceName = "Help-Cli.txt";
        
        public void Handle(ShellHost host, string[] args, PropertyBag<string> dictionary)
        {
            using (var file = Assembly.GetExecutingAssembly().GetManifestResourceStream(HelpFileResourceName))
            {
                using (var reader = new StreamReader(file))
                {
                    var help = reader.ReadToEnd();

                    if (help.Contains("[VERSION]"))
                    {
                        help = help.Replace("[VERSION]", "1.0.0.1");
                    }
                    
                    Console.Write(help);
                }
            }
        }
    }
}