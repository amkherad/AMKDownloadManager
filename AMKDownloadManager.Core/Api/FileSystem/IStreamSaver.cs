using System;
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
            Segment segment,
            long? bufferSize,
            long? limit);
    }
}