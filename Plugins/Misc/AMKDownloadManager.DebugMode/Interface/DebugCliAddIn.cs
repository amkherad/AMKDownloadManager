using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Cli;
using AMKDownloadManager.Core.Api.Configuration;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.DebugMode.Interface
{
    public class DebugCliAddIn : ICliEngine
    {
        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }

        public ICommandResult Execute(IApplicationContext applicationContext, CommandParser parser, ICliInterface @interface, string commandName,
            object[] parameters)
        {

            return null;
        }
    }
}