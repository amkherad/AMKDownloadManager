using AMKsGear.Core.IO;

namespace AMKDownloadManager.Defaults.Messaging
{
    public interface IHubEndpoint
    {
        event DataReceivedEventHandler<string> DataReceived;

        //void Send(byte[] bytes);
        void Send(string text);
    }
}