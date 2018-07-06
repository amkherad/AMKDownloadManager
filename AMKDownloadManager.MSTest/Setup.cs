using System;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Defaults;
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
            Logger.RegisterLogger(new MethodLogger(System.Console.Write, f =>
            {
                for (var i = 0; i < f; i++) System.Console.WriteLine();
            }));

            var pool = ApplicationHost.Instance.Initialize(new AbstractThreadFactory());
            AppHelpers.InjectTopLayerFeatures(pool);
            //AppHelpers.LoadComponents(app);
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