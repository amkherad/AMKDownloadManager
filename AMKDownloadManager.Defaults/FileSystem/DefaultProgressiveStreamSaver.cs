using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.FileSystem;

namespace AMKDownloadManager.Defaults.FileSystem
{
    public class DefaultProgressiveStreamSaver : IStreamSaver
    {
        private long _defaultBufferSize = KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSizeDefaultValue;

        private int _minSegmentSize = KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue;
        private int _maxSegmentSize = KnownConfigs.DownloadManager.Segmentation.MaxSegmentSizeDefaultValue;

        public void SaveStream(
            Stream stream,
            IFileManager fileManager,
            SegmentationContext segmentationContext,
            Segment segment,
            long? partsSize,
            long? limit)
        {
            var segmentLength = segment.Length;
            var bufferSizeFrame = partsSize ?? _defaultBufferSize;
            if (bufferSizeFrame > segmentLength)
            {
                bufferSizeFrame = (int) segmentLength;
            }

            if (bufferSizeFrame > _maxSegmentSize)
            {
                bufferSizeFrame = _maxSegmentSize;
            }

            var bufferSize = (int) bufferSizeFrame;

            var resourceOffset = segment.Min;
            var segmentOffset = 0L;

            var buffer = new byte[bufferSizeFrame];

//#if DEBUG
//            var totalBytesWritten = 0L;
//#endif

            for (;;)
            {
                //reads the stream for partSizeLng bytes.
                var length = (long) stream.Read(buffer, 0, bufferSize);
                if (length == 0)
                {
                    break;
                }
                
//#if DEBUG
//                Trace.WriteLine(
//                    $"Writing to file , thread.name: {Thread.CurrentThread.Name}, buffer.length:{length}, resourceOffset: {resourceOffset}");
//#endif
                //writing buffer to file.
                fileManager.SaveBinary(buffer, resourceOffset, 0, length);

//#if DEBUG
//                totalBytesWritten += length;
//#endif
                segmentOffset += length;
                resourceOffset += length;

                if (segmentOffset >= segmentLength)
                {
                    //move to next segment

                    lock (segmentationContext.SynchronizationLock)
                    {
                        //Marking segment as finished
                        segmentationContext.MarkReservedAsFilled(segment);

                        //segment is marked as limited and cannot grow, breaking the loop. (rare condition depending on ISegmentDivider)
                        if (segment.LimitedSegment)
                        {
                            break;
                        }

                        var segmentMax = segment.Max;
                        segment = segmentationContext.GetSegmentGrowthRightLimit(segmentMax, _defaultBufferSize);
                        //No empty continuous segment reserved.
                        if (segment == null)
                        {
                            break;
                        }

                        segmentationContext.ReservedRanges.Add(segment);
                    }

                    bufferSize = (int) bufferSizeFrame;
                    segmentOffset = 0;
                }
                else if (segmentOffset + bufferSize <= segmentLength)
                {
                    //another buffer can fit into segment.
                }
                else
                {
                    //20 - 10 = 10
                    bufferSize = (int) (segmentLength - segmentOffset);
                }
            }
            
//#if DEBUG
//            Trace.WriteLine(
//                $"TBW-Total bytes written: thread.name: {Thread.CurrentThread.Name}/{Thread.CurrentThread.ManagedThreadId}, totalBytesWritten:{totalBytesWritten}");
//#endif

            //if (partsSize != null)
            //{
            //    int partSizeLng = (int) partsSize.Value;
            //    var offset = start;
            //    var part = new byte[partSizeLng];
            //    long length = stream.Read(part, 0, partSizeLng);
            //    long totalRead = 0L;
            //    try
            //    {
            //        while (length > 0)
            //        {
            //            fileManager.SaveBinary(part, offset, 0, length);
            //            offset += length;
            //            totalRead += length;
//
            //            if (limit != null && totalRead >= limit.Value)
            //            {
            //                break;
            //            }
//
            //            length = stream.Read(part, 0, partSizeLng);
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        Trace.WriteLine(ex.Message);
            //        Debugger.Break();
            //    }
            //}
            //else
            //{
            //    fileManager.SaveStream(stream, start, 0, stream.Length);
            //}
        }

        #region DefaultProgressiveStreamSaver

        public int Order => 0;

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSize))
            {
                _defaultBufferSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSize,
                    KnownConfigs.DownloadManager.Download.DefaultReceiveBufferSizeDefaultValue
                );
            }

            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MinSegmentSize))
            {
                _minSegmentSize = configProvider.GetInt(this,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue
                );
            }

            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize))
            {
                _maxSegmentSize = configProvider.GetInt(this,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSizeDefaultValue
                );
            }
        }

        #endregion
    }
}