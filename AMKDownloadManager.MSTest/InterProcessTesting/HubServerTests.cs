using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Threading;
using AMKDownloadManager.Defaults.Messaging;
using AMKDownloadManager.Defaults.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.InterProcessTesting
{
    [TestClass]
    public class HubServerTests
    {
        [TestMethod]
        public void TestClientToServerConnection()
        {
            var appContext = ApplicationHost.Instance.Pool;

            appContext.ScheduleBackgroundTask("jack", () =>
            {
                var hub = new DefaultMessagingHost.HubServer("TESTHub1", new InterProcessLockService(appContext),
                    appContext.GetFeature<IThreadFactory>());

                hub.Listen();
            });

            var client = new DefaultMessagingHost.HubClient("TESTHub1");

            client.Connect();
        }
    }
}