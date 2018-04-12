using System;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Api.Transport;
using AMKDownloadManager.Core.Extensions;
using AMKDownloadManager.Core.Impl;
using AMKDownloadManager.Defaults.DownloadManager;
using AMKDownloadManager.Defaults.FileSystem;
using AMKDownloadManager.Defaults.JobScheduler;
using AMKDownloadManager.Defaults.Network;
using AMKDownloadManager.Defaults.Segmentation;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Defaults
{
    /// <summary>
    /// Shared layer helpers.
    /// </summary>
    public class AppHelpers
    {
        /// <summary>
        /// Load components for <see cref="IAppContext"/>.
        /// </summary>
        /// <param name="appContext">The context which components are loaded for.</param>
        public static void LoadComponents(IAppContext appContext)
        {
            var importer = new ComponentImporter();
            importer.Compose();
            Console.WriteLine("{0} component(s) are imported successfully.", importer.NumberOfAvailableComponents);
            
            importer.InitializeAll(appContext);
        }

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="app">The <see cref="IAppContext"/> which services will being injected to.</param>
        private static void _buildDefaults(IAppContext app)
        {
            var configProvider = new DefaultConfigProvider();
            
            app.AddFeature<IConfigProvider>(configProvider);
            app.AddFeature<INetworkMonitor>(new DefaultNetworkMonitor());

            var scheduler = new DefaultJobScheduler(app);

            app.AddFeature<IScheduler>(scheduler);
            app.AddFeature<IDownloadManager>(new DefaultDownloadManager(app, scheduler));

            app.AddFeature<IJobDivider>(new DefaultSegmentProvider());
            app.AddFeature<IFileProvider>(new DefaultFileProvider(
                configProvider.GetInt(
                    app,
                    KnownConfigs.FileSystem.DuplicityResolvationStart,
                    KnownConfigs.FileSystem.DuplicityResolvationStartDefaultValue
                    )
                ));
            app.AddFeature<IDownloadPathProvider>(new DefaultDownloadPathProvider());
            
            app.AddFeature<IStreamSaver>(new DefaultProgressiveStreamSaver());
        }

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="appContext">The <see cref="IAppContext"/> which services will be injected to.</param>
        public static void InjectTopLayerFeatures(IAppContext appContext)
        {
            _buildDefaults(appContext);

            var httpTransport = new DefaultTcpHttpRequestTransport();
            appContext.AddFeature<IRequestTransport>(httpTransport);
            appContext.AddFeature<IHttpTransport>(httpTransport);
            appContext.AddFeature<INetworkInterfaceProvider>(new NetworkInterfaceProvider());
        }

        /// <summary>
        /// Calls <see cref="IFeature.LoadConfig"/> on all features of <see cref="IAppContext"/>
        /// </summary>
        /// <param name="appContext">The <see cref="IAppContext"/> which features will be configured.</param>
        public static void ConfigureFeatures(IAppContext appContext)
        {
            var configProvider = appContext.GetFeature<IConfigProvider>();

            var features = appContext.GetTypedValues().OfType<IFeature>();
            features.ForEach(x => x.LoadConfig(appContext, configProvider, null));
        }
    }
}