namespace AMKDownloadManager.Core.Api.Transport
{
    public interface IHttpRequestSerializer : IRequestTransport
    {
        string SerializeString(IRequest request);
        byte[] SerializeBinary(IRequest request);
    }
}