using System.Diagnostics;
using System.Net.Configuration;
using AMKDownloadManager.Core;
using AMKDownloadManager.Defaults;
using AMKDownloadManager.Defaults.Threading;
using AMKDownloadManager.HttpDownloader.AddIn;
using ir.amkdp.gear.core.Trace;
using ir.amkdp.gear.core.Trace.LoggerEngines;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit
{
    [SetUpFixture]
    public class Setup
    {
        private static bool _inited = false;

        [OneTimeSetUp]  // [SetUp] => [OneTimeSetUp] for NUnit 3.0 and up; see http://bartwullems.blogspot.com/2015/12/upgrading-to-nunit-30-onetimesetup.html
        public void SetUp()
        {
            if (_inited) return;
            _inited = true;
            
            Trace.Listeners.Add(new ConsoleTraceListener());
            Logger.RegisterLogger(new MethodLogger(System.Console.Write, f =>
            {
                for (var i = 0; i < f; i++) System.Console.WriteLine();
            }));
            Logger.RegisterLogger(new MethodLogger(TestContext.Write, f =>
            {
                for (var i = 0; i < f; i++) TestContext.WriteLine();
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
}