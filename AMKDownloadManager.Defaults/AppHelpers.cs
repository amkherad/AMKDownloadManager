using System;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Barriers;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Defaults.Network;

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

        public static void InjectTopLayerFeatures(IAppContext appContext)
        {
            appContext.AddFeature<IRequestBarrier>(new DefaultHttpRequestBarrier());
            appContext.AddFeature<IHttpRequestBarrier>(new DefaultHttpRequestBarrier());
            appContext.AddFeature<INetworkInterfaceProvider>(new NetworkInterfaceProvider());
        }
    }
}