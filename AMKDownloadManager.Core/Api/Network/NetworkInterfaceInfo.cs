namespace AMKDownloadManager.Core.Api.Network
{
    public class NetworkInterfaceInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public bool IsReceiveOnly { get; set; }
        public int InterfaceType { get; set; }
        
        public bool SupportsIPv4 { get; set; }
        public bool SupportsIPv6 { get; set; }
        public long Speed { get; set; }
        public bool SupportsMulticast { get; set; }
    }
}