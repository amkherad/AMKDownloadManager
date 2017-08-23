using System.Collections.Generic;

namespace AMKDownloadManager.Core.Api.Network
{
    public interface INetworkInterfaceProvider : IFeature
    {
        IEnumerable<INetworkInterface> GetNetworkInterfaces();
    }
}