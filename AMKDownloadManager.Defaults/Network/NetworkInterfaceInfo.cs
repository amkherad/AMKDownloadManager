using System.Net.NetworkInformation;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Defaults.Network
{
    public class NetworkInterfaceInfo : INetworkInterfaceInfo
    {
        public NetworkInterface NetworkInterface { get; }

        public NetworkInterfaceInfo(NetworkInterface networkInterface)
        {
            NetworkInterface = networkInterface;
        }

        public object GetUnderlyingContext() => NetworkInterface;


        public string Id => NetworkInterface.Id;
        public string Name  => NetworkInterface.Name;
        public string Description => NetworkInterface.Description;
        public bool IsReceiveOnly => NetworkInterface.IsReceiveOnly;
        public int InterfaceType => (int) NetworkInterface.NetworkInterfaceType;
        public bool SupportsIPv4 => NetworkInterface.Supports(NetworkInterfaceComponent.IPv4);
        public bool SupportsIPv6 => NetworkInterface.Supports(NetworkInterfaceComponent.IPv6);
        public long Speed => NetworkInterface.Speed;
        public bool SupportsMulticast  => NetworkInterface.SupportsMulticast;
    }
}