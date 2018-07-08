using System.Diagnostics;
using AMKDownloadManager.Defaults.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMKDownloadManager.MSTest.Categories.Network
{
    [TestClass]
    public class NetworkInterfaceTests
    {
        [TestMethod]
        public void TestNetworkInterfaceProvider()
        {
            var provider = new NetworkInterfaceProvider();
            
            foreach(var iface in provider.GetNetworkInterfaces())
            {
                Trace.WriteLine(iface.Name);
            }
        }
    }
}