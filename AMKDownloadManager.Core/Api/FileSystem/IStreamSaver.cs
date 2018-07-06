using System.IO;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Core.Api.FileSystem
{
    public interface IStreamSaver : IFeature
    {
        void SaveStream(
            Stream stream,
            IFileManager fileManager,
            SegmentationContext segmentationContext,
            long start,
            long? end,
            long? partSize,
            long? limit);
    }
}