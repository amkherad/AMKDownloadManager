using AMKsGear.Architecture.Patterns;

namespace AMKDownloadManager.Core.Api.Network
{
    public interface INetworkInterfaceInfo : IWrapper
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        
        bool IsReceiveOnly { get; }
        int InterfaceType { get; }
        
        bool SupportsIPv4 { get; }
        bool SupportsIPv6 { get; }
        long Speed { get; }
        bool SupportsMulticast { get; }
    }
}