using System;
using System.Diagnostics;
using System.IO;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.ConfigProvider;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.HttpDownloader.AddIn;
using AMKsGear.Core.Trace;
using AMKsGear.Core.Trace.LoggerEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest
{
    [TestClass]
    public class Setup
    {
        private static bool _inited = false;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            if (_inited) return;
            _inited = true;
            
            Trace.Listeners.Add(new ConsoleTraceListener());
            Logger.RegisterDefaultLogChannel(new MethodLogger(Console.Write));

            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            pool.AddFeature<IConfigProvider>(new DefaultConfigProvider());
            AppHelpers.InjectTopLayerFeatures(pool);
            AppHelpers.LoadComponents(pool,
                Path.Combine(pool.ApplicationProfileDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                Path.Combine(pool.ApplicationSharedProfileDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                new []
                {
                    Path.Combine(pool.ApplicationDirectory, ApplicationContext.ApplicationPluginsSubDirectoryName),
                    pool.ApplicationPluginRepository
                }
            );
            var component = new HttpDownloaderComponent();
            component.Initialize(pool);
            AppHelpers.ConfigureFeatures(pool);
        }

        //[TearDown]
        public void TearDown()
        {
            
        }
    }

    public class ConsoleTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}