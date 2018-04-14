using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Defaults.Network
{
    public class NetworkInterfaceProvider : INetworkInterfaceProvider
    {
        public IEnumerable<NetworkInterfaceInfo> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up &&
                            x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(x => new NetworkInterfaceInfo
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    
                    IsReceiveOnly = x.IsReceiveOnly,
                    InterfaceType = (int) x.NetworkInterfaceType,
                    Speed = x.Speed,
                    SupportsMulticast = x.SupportsMulticast,
                    
                    SupportsIPv4 = x.Supports(NetworkInterfaceComponent.IPv4),
                    SupportsIPv6 = x.Supports(NetworkInterfaceComponent.IPv6),
                });
        }

        public static NetworkInterface GetNetworkInterfaceByName(string name, StringComparer comparer = null)
        {
            if (comparer == null) comparer = StringComparer.CurrentCultureIgnoreCase;

            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => comparer.Equals(x.Name, name));
        }
        
        public int Order => 0;
        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
    }
}