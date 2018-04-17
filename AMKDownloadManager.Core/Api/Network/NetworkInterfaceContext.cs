using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api.Types;
using ir.amkdp.gear.arch.Patterns;
using ir.amkdp.gear.data.Models;

namespace AMKDownloadManager.Core.Api.Network
{
    /// <summary>
    /// Interface traffic load and load balancing behavior.
    /// </summary>
    public enum NetworkInterfaceLoadBehavior
    {
        /// <summary>
        /// Use default interface only.
        /// </summary>
        UseDefault,
        
        /// <summary>
        /// Switch to a working interface recursively. 
        /// </summary>
        Backup,
        
        /// <summary>
        /// Switch to next interface in the list when the interface goal reached.
        /// </summary>
        MultipleCircle,
        
        /// <summary>
        /// Load balance interfaces. (main interface ignored)
        /// </summary>
        MultipleBalance,
    }
    
    public class NetworkInterfaceLoadContext
    {
        public NetworkInterfaceInfo Info { get; set; }
        public Int64RangeModel TrafficBoundaries { get; set; }
        public DateTimeRange TimeLimits { get; set; }
        
        
    }
    
    /// <summary>
    /// Provides information on usage of network interface devices.
    /// </summary>
    public class NetworkInterfaceContext
    {
        public NetworkInterfaceLoadBehavior LoadBehavior { get; }
        public IEnumerable<NetworkInterfaceLoadContext> Interfaces { get; set; }
        public bool UseDefaultAsBackup { get; set; }
        
        public NetworkInterfaceContext()
        {
            LoadBehavior = NetworkInterfaceLoadBehavior.UseDefault;
        }
        public NetworkInterfaceContext(NetworkInterfaceLoadBehavior loadBehavior)
        {
            LoadBehavior = loadBehavior;
        }
    }
}