using System.IO;

namespace AMKDownloadManager.Core.Api.FileSystem
{
    public interface IStreamSaver : IFeature
    {
        void SaveStream(
            Stream stream,
            IFileManager fileManager,
            long start,
            long? end,
            long? chunkSize,
            long? limit);
    }
}