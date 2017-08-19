using System;
using AMKDownloadManager.Core.Api.Network;

namespace AMKDownloadManager.Core.Impl
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
    }
}