namespace AMKDownloadManager.Core.Api.FileSystem
{
    public interface IDownloadPathProvider : IFeature
    {
        PathInfo GetPathForMedia(
            IApplicationContext applicationContext,
            string mediaType,
            string fileName
        );
    }
}