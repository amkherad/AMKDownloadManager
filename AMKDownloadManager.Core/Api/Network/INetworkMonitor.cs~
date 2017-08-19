using System;

namespace AMKDownloadManager.Core.Api.Network
{
    /// <summary>
    /// Service to provide network monitoring and state control.
    /// </summary>
    public interface INetworkMonitor : IFeature
    {
        /// <summary>
        /// Occurs when network availability changed.
        /// </summary>
        event EventHandler NetworkAvailabilityChanged;

        /// <summary>
        /// Checks the network availability.
        /// </summary>
        /// <returns><c>true</c>, if there is network connection, <c>false</c> otherwise.</returns>
        bool CheckNetworkAvailability();

        /// <summary>
        /// Checks the internet availability.
        /// </summary>
        /// <returns><c>true</c>, if there is internet connection, <c>false</c> otherwise.</returns>
        bool CheckInternetAvailability();
    }
}