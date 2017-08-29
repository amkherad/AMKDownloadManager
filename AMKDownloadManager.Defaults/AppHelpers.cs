using System;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Impl;
using AMKDownloadManager.Defaults.DownloadManager;
using AMKDownloadManager.Defaults.JobScheduler;
using AMKDownloadManager.Defaults.Network;
using AMKDownloadManager.Defaults.Segmentation;
using ir.amkdp.gear.core.Collections;

namespace AMKDownloadManager.Defaults
{
    public class AppHelpers
    {
        public static void LoadComponents(IAppContext appContext)
        {
            var importer = new ComponentImporter();
            importer.Compose();
            Console.WriteLine("{0} component(s) are imported successfully.", importer.AvailableNumberOfComponents);
            
            importer.InitializeAll(appContext);
        }

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="app">App.</param>
        private static void _buildDefaults(IAppContext app)
        {
            app.AddFeature<IConfigProvider>(new DefaultConfigProvider());
            app.AddFeature<INetworkMonitor>(new DefaultNetworkMonitor());

            var scheduler = new DefaultJobScheduler(app);

            app.AddFeature<IScheduler>(scheduler);
            app.AddFeature<IDownloadManager>(new DefaultDownloadManager(app, scheduler));

            app.AddFeature<IJobDivider>(new DefaultSegmentProvider());
        }

        public static void InjectTopLayerFeatures(IAppContext appContext)
        {
            _buildDefaults(appContext);

            var httpBarrier = new DefaultHttpRequestBarrier();
            appContext.AddFeature<IRequestBarrier>(httpBarrier);
            appContext.AddFeature<IHttpRequestBarrier>(httpBarrier);
            appContext.AddFeature<INetworkInterfaceProvider>(new NetworkInterfaceProvider());
        }

        public static void ConfigureFeatures(IAppContext appContext)
        {
            var configProvider = appContext.GetFeature<IConfigProvider>();

            var features = appContext.GetTypedValues().OfType<IFeature>();
            features.ForEach(x => x.LoadConfig(appContext, configProvider));
        }
    }
}