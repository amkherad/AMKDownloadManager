using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;

namespace AMKDownloadManager.Defaults.Segmentation
{
    public class BinarySegmentProvider : ISegmentDivider
    {
        //private const string DivisionLevelSegmentationContextPropertyName = "BinarySegmentProvider.DivisionLevel";
        //private const string CurrentDivisionIndexSegmentationContextPropertyName = "BinarySegmentProvider.CurrentDivisionIndex";

        private long _minSegmentSize = KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue;
        private long _maxSegmentSize = KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue;

        public PartialBlockDescriptor GetPart(
            IAppContext appContext,
            IJob job,
            SegmentationContext segmentationContext)
        {
            if (appContext == null) throw new ArgumentNullException(nameof(appContext));
            //if (job == null) throw new ArgumentNullException(nameof(job));
            if (segmentationContext == null) throw new ArgumentNullException(nameof(segmentationContext));

            //var config = appContext.GetFeature<IConfigProvider>();

            //var maxSimultaneousConnections = config.GetInt(this,
            //    KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections,
            //    KnownConfigs.DownloadManager.Download.MaxSimultaneousConnectionsDefaultValue
            //);

//            var optimumSegmentSize = segmentationContext.TotalSize / maxSimultaneousJobs;
//            while (optimumSegmentSize < _minSegmentSize)
//                optimumSegmentSize += _minSegmentSize;
//            if (optimumSegmentSize > _maxSegmentSize)
//                optimumSegmentSize = _maxSegmentSize;

            lock (segmentationContext.SynchronizationLock)
            {
                Segment segment = null;
                if (segmentationContext.FilledRanges.All(x => x.Min != 0) && segmentationContext.ReservedRanges.All(x => x.Min != 0))
                {
                    segment = new Segment(0, _minSegmentSize - 1);
                }
                else
                {
                    var freeRange = segmentationContext.Reverse().OrderByDescending(x => x.Length).FirstOrDefault();
                    if (freeRange == null) return null;

                    if (freeRange.Length > (_minSegmentSize * 2))
                    {
                        var min = freeRange.Min + (freeRange.Length / 2);
                        segment = new Segment(min, min + _minSegmentSize - 1);
                    }
                    else if (freeRange.Length > 0)
                    {
                        segment = freeRange;
                    }
                }

                if (segment == null)
                {
#if DEBUG
                    Trace.WriteLine("BinarySegmentProvider.GetPart(): segment == null returning null part.");
#endif
                    return null;
                }

#if DEBUG
                Trace.WriteLine($"BinarySegmentProvider.GetPart(): segment({segment.Min}, {segment.Max}) part created with length of {segment.Length}");
#endif

                segmentationContext.ReservedRanges.Add(segment);
                
                return new PartialBlockDescriptor(segment);
            }
        }

        public PartialBlockDescriptor GetPart(IAppContext appContext, IJob job, SegmentationContext segmentationContext,
            long contiguousLeftOffset)
        {
            return GetPart(appContext, job, segmentationContext);
        }

        public int Order => 0;

        public void LoadConfig(IAppContext appContext, IConfigProvider configProvider, HashSet<string> changes)
        {
            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MinSegmentSize))
            {
                _minSegmentSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue
                );
            }

            if (changes == null || changes.Contains(KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize))
            {
                _maxSegmentSize = configProvider.GetLong(this,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSize,
                    KnownConfigs.DownloadManager.Segmentation.MaxSegmentSizeDefaultValue
                );
            }
        }
    }
}