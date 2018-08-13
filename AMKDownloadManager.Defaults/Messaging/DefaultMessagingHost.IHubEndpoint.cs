using AMKsGear.Core.IO;

namespace AMKDownloadManager.Defaults.Messaging
{
    public partial class DefaultMessagingHost
    {
        public interface IHubEndpoint
        {
            event DataReceivedEventHandler<BytesReceivedEventArgs> DataReceived;

            void Send(byte[] bytes);
        }
    }
}