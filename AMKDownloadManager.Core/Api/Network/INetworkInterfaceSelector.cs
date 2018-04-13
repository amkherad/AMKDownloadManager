using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Network
{
    public interface INetworkInterfaceSelector : IFeature
    {
        object SelectEndPoint(
            IAppContext appContext,
            IRequestTransport transport,
            DownloadItem downloadItem);
    }
}