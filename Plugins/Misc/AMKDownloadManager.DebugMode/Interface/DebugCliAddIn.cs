using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Cli;
using AMKDownloadManager.Core.Api.Configuration;

namespace AMKDownloadManager.DebugMode.Interface
{
    public class DebugCliAddIn : ICliEngine
    {
        public int Order => 0;
        
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }

        public ICommandResult Execute(IAppContext appContext, CommandParser parser, ICliInterface @interface, string commandName,
            object[] parameters)
        {

            return null;
        }
    }
}