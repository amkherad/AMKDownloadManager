using ir.amkdp.gear.arch.Patterns;

namespace AMKDownloadManager.Core.Api.Network
{
    public interface INetworkInterface : IWrapper
    {
        string Name { get; }
        string Description { get; }
    }
}