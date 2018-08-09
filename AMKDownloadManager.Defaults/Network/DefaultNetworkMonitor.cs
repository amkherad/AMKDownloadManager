using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.Network;
using AMKsGear.Architecture.Automation.IoC;

namespace AMKDownloadManager.Defaults.Network
{
    public class DefaultNetworkMonitor : INetworkMonitor
    {
        public event EventHandler NetworkAvailabilityChanged;
        public bool CheckNetworkAvailability()
        {

            return true;
        }

        public bool CheckInternetAvailability()
        {

            return true;
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