using AMKDownloadManager.Core.Api.Transport;

namespace AMKDownloadManager.Core.Api.Network
{
    public interface INetworkInterfaceSelector : IFeature
    {
        /// <summary>
        /// Selects the desired network interface depending on <see cref="DownloadItem"/> properties.
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="transport"></param>
        /// <param name="downloadItem"></param>
        /// <returns>
        /// It can either return IPEndPoint or NetworkInterface, also NetworkInterfaceInfo is accepted if caller supported it
        /// with a interface lookup overhead.
        /// </returns>
        object SelectInterface(
            IApplicationContext applicationContext,
            IRequestTransport transport,
            DownloadItem downloadItem);
    }
}