using System; 
using System.Threading;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Shell
{
    /// <summary>
    /// Provides presentation abilities inside shell mode. (progressbar...)
    /// </summary>
    public class ShellReporter : IShellCommand
    {
        public void Handle(ShellHost host, string[] args, PropertyBag<string> dictionary)
        {
            switch (dictionary["-report"])
            {
                case "h":
                case "r":
                case "hr":
                case "human":
                case "readable":
                {
                    for (var i = 0; i < 500; i++)
                    {
                        Console.Write($"\r{new string('=', i % 100)}{new string(' ', 100 - i % 100)}");
                        Thread.Sleep(30);
                    }
                    break;
                }
            }
        }
    }
}