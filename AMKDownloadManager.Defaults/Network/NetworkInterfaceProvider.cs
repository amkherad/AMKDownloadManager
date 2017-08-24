using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Defaults.Network
{
    public class NetworkInterfaceProvider : INetworkInterfaceProvider
    {
        public IEnumerable<INetworkInterface> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up &&
                            x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(x => new NetworkInterfaceWrapper(x));
        }

        public int Order => 0;
    }
}