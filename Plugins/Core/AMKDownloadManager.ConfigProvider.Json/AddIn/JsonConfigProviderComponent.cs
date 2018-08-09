using System;
using System.Composition;
using System.Reflection;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Extensions;

namespace AMKDownloadManager.ConfigProvider.Json.AddIn
{
    [Export(typeof(IComponent))]
    public class JsonConfigProviderComponent : IComponent
    {
        public const string ComponentGuid = "fa553c67-0faa-4544-8347-53758fd5993f";

        private static JsonConfigProvider JsonConfigProviderInstance;
        
        private static bool _isLoaded = false;
        
        public string Name => "JsonConfigProvider";

        public string Description => "Loads and saves json configuration files.";

        public string Author => "Ali Mousavi Kherad";

        public Version Version
        {
            get
            {
                var assembly = typeof(JsonConfigProviderComponent).GetTypeInfo().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
        }


        #region IComponent implementation

        public void Install(IApplicationContext application)
        {
            var config = application.GetFeature<IConfigProvider>();

            config.InstallInt(this,
                ComponentGuid,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnectionsDefaultValue
            );
            config.InstallInt(this,
                ComponentGuid,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobsDefaultValue
            );
        }

        public void Uninstall(IApplicationContext application)
        {
            
        }

        public void Initialize(IApplicationContext application)
        {
            if (_isLoaded)
            {
                return;
            }

            _isLoaded = true;
            
            var jsonConfigProvider = new JsonConfigProvider(application, new []
            {
                ApplicationContext.ApplicationConfigurationFilePath,
                ApplicationContext.ApplicationSharedConfigurationFilePath,
            });

            JsonConfigProviderInstance = jsonConfigProvider;
            
            jsonConfigProvider.Load();
            
            application.AddFeature<IConfigProvider>(jsonConfigProvider);
        }

        public void AfterInitialize(IApplicationContext application)
        {
            
        }

        public void Unload(IApplicationContext application)
        {
            application.RemoveFeature<JsonConfigProvider>();
        }


        #endregion
    }
}