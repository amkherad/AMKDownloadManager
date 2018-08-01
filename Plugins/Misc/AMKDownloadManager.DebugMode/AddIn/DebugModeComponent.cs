using System;
using System.Composition;
using System.Reflection;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Cli;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Extensions;
using AMKDownloadManager.DebugMode.Interface;

namespace AMKDownloadManager.DebugMode.AddIn
{
    [Export(typeof(IComponent))]
    public class DebugModeComponent : IComponent
    {
        public const string ComponentGuid = "fa443c67-0faa-4555-8347-55068aefffff";

        private static bool _isLoaded = false;
        
        public string Name => "DebugMode";

        public string Description => "Provides debug mode for application including self debug helpers and resource download debugging.";

        public string Author => "Ali Mousavi Kherad";

        public Version Version
        {
            get
            {
                var assembly = typeof(DebugModeComponent).GetTypeInfo().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
        }



        #region IComponent implementation

        public void Install(IAppContext app)
        {
            var config = app.GetFeature<IConfigProvider>();

//            config.InstallInt(this,
//                ComponentGuid,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnectionsDefaultValue
//            );
//            config.InstallInt(this,
//                ComponentGuid,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobsDefaultValue
//            );
        }

        public void Uninstall(IAppContext app)
        {
            var config = app.GetFeature<IConfigProvider>();

//            config.UnInstallInt(this,
//                ComponentGuid,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections
//            );
//            config.UnInstallInt(this,
//                ComponentGuid,
//                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs
//            );
        }

        public void Initialize(IAppContext app)
        {
            if (_isLoaded)
            {
                return;
            }

            app.AddFeature<ICliEngine>(new DebugCliAddIn());
        }

        public void Unload(IAppContext app)
        {
            app.RemoveFeature(new DebugCliAddIn());
        }
        
        #endregion
    }
}