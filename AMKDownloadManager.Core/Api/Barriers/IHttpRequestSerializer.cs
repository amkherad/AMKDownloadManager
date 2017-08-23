namespace AMKDownloadManager.Core.Api.Barriers
{
    public interface IHttpRequestSerializer : IRequestBarrier
    {
        string SerializeString(IRequest request);
        byte[] SerializeBinary(IRequest request);
    }
}