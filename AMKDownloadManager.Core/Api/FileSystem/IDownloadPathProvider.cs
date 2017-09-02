namespace AMKDownloadManager.Core.Api.FileSystem
{
    public interface IDownloadPathProvider : IFeature
    {
        PathInfo GetPathForMedia(
            IAppContext appContext,
            string mediaType,
            string fileName
        );
    }
}