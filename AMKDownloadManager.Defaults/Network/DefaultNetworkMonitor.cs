using System;
using System.Collections.Generic;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Network;

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

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            
        }
    }
}