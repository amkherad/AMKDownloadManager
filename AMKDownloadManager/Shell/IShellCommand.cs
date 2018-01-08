using System.Collections.Generic;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Shell
{
    public interface IShellCommand
    {
        void Handle(ShellHost host, string[] args, PropertyBag<string> dictionary);
    }
}