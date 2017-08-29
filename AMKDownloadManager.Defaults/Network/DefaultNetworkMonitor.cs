using System;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Defaults.Network
{
    public class DefaultNetworkMonitor : INetworkMonitor
    {
        public DefaultNetworkMonitor()
        {
            
        }

        public event EventHandler NetworkAvailabilityChanged;
        public bool CheckNetworkAvailability()
        {
            throw new NotImplementedException();
        }

        public bool CheckInternetAvailability()
        {
            throw new NotImplementedException();
        }

        public int Order
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider)
        {
            
        }
    }
}