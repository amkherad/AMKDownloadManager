using System.Net.NetworkInformation;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Defaults.Network
{
    public class NetworkInterfaceWrapper : INetworkInterface
    {
        public NetworkInterface NetworkInterface { get; }
        
        public NetworkInterfaceWrapper(NetworkInterface networkInterface)
        {
            NetworkInterface = networkInterface;
        }

        public string Name => NetworkInterface.Name;
        public string Description => NetworkInterface.Description;
        
        public object GetUnderlyingContext() => NetworkInterface;
    }
}