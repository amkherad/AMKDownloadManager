using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Api.Threading;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.Defaults.DownloadManager;
using AMKDownloadManager.Defaults.FileSystem;
using AMKDownloadManager.Defaults.JobScheduler;
using AMKDownloadManager.Defaults.Network;
using AMKDownloadManager.Defaults.Segmentation;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.Defaults.Transport;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Defaults
{
    /// <summary>
    /// Shared layer helpers.
    /// </summary>
    public static class AppHelpers
    {
        /// <summary>
        /// Load components for <see cref="IApplicationContext"/>.
        /// </summary>
        /// <param name="applicationContext">The context which components are loaded for.</param>
        /// <param name="appLocalStoragePath">The path to application storage for current user only. (~/.local/share/XXXX | C:\Users\USERNAME\AppData\Local\XXXX)</param>
        /// <param name="appSharedStoragePath">The path to application storage for all users. (/usr/share | C:\ProgramData)</param>
        /// <param name="additionalPluginPaths">Additional plugins paths</param>
        public static void LoadComponents(
            IApplicationContext applicationContext,
            string appLocalStoragePath,
            string appSharedStoragePath,
            string[] additionalPluginPaths)
        {
            var importer = new ComponentImporter();
            
            importer.Compose(new [] { appLocalStoragePath, appSharedStoragePath }.Merge(additionalPluginPaths).ToArray());
            Console.WriteLine("{0} component(s) are imported successfully.", importer.NumberOfAvailableComponents);
            
            importer.InitializeAll(applicationContext);
        }

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="application">The <see cref="IApplicationContext"/> which services will being injected to.</param>
        private static void _buildDefaults(IApplicationContext application)
        {
//            var configProvider = application.GetFeature<IConfigProvider>();
//            if (configProvider == null)
//            {
//                throw new InvalidOperationException();
//            }

            application.AddFeature<INetworkMonitor>(new DefaultNetworkMonitor());

            application.AddFeature<IScheduler>(new DefaultJobScheduler(application));
            application.AddFeature<IDownloadManager>(new DefaultDownloadManager(application));
            
            application.AddFeature<ISegmentDivider>(new BinarySegmentProvider());
            application.AddFeature<IFileProvider>(new DefaultFileProvider(application));
            application.AddFeature<IDownloadPathProvider>(new DefaultDownloadPathProvider());
            
            application.AddFeature<IStreamSaver>(new DefaultProgressiveStreamSaver());
            
            application.AddFeature<ILogger>(new DefaultLoggerChannel());
            
            application.AddFeature<IInterProcessLockService>(new InterProcessLockService(application));
        }

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="applicationContext">The <see cref="IApplicationContext"/> which services will be injected to.</param>
        public static void InjectTopLayerFeatures(IApplicationContext applicationContext)
        {
            _buildDefaults(applicationContext);

            var httpTransport = new DefaultHttpRequestTransport();
            applicationContext.AddFeature<IRequestTransport>(httpTransport);
            applicationContext.AddFeature<IHttpTransport>(httpTransport);
            applicationContext.AddFeature<INetworkInterfaceProvider>(new NetworkInterfaceProvider());
        }

        /// <summary>
        /// Calls <see cref="IFeature.LoadConfig"/> on all features of <see cref="IApplicationContext"/>
        /// </summary>
        /// <param name="applicationContext">The <see cref="IApplicationContext"/> which features will be configured.</param>
        public static void ConfigureFeatures(IApplicationContext applicationContext)
        {
            var configProvider = applicationContext.GetFeature<IConfigProvider>();

            var features = applicationContext.GetTypedValues().OfType<IFeature>().ToArray();
            foreach (var feature in features)
            {
                feature.ResolveDependencies(applicationContext, applicationContext.TypeResolver);
            }
            foreach (var feature in features)
            {
                feature.LoadConfig(applicationContext, configProvider, null);
            }
        }
    }
}