﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Network;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Network
{
    public class NetworkInterfaceProvider : INetworkInterfaceProvider
    {
        public IEnumerable<INetworkInterfaceInfo> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up &&
                            x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(x => new NetworkInterfaceInfo(x));
        }

        public static NetworkInterface GetNetworkInterfaceByName(string name, StringComparer comparer = null)
        {
            if (comparer == null) comparer = StringComparer.CurrentCultureIgnoreCase;

            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => comparer.Equals(x.Name, name));
        }
        
        public int Order => 0;
        
        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
    }
}