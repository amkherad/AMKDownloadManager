using System;
using System.Collections.Generic;
using System.Linq;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Configuration;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Defaults.Segmentation
{
    public class LeftBalancingSegmentProvider : ISegmentDivider
    {
        private long _minSegmentSize = KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue; 
        private long _maxSegmentSize = KnownConfigs.DownloadManager.Segmentation.MinSegmentSizeDefaultValue; 

        public PartialBlockDescriptor GetPart(
            IApplicationContext applicationContext,
            IJob job,
            SegmentationContext segmentationContext)
        {
            if (applicationContext == null) throw new ArgumentNullException(nameof(applicationContext));
            //if (job == null) throw new ArgumentNullException(nameof(job));
            if (segmentationContext == null) throw new ArgumentNullException(nameof(segmentationContext));

            var config = applicationContext.GetFeature<IConfigProvider>();
            
            var maxSimultaneousConnections = config.GetInt(this,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnectionsDefaultValue
            );

            var optimumSegmentSize = segmentationContext.TotalSize / maxSimultaneousConnections;
            while (optimumSegmentSize < _minSegmentSize)
                optimumSegmentSize += _minSegmentSize;
            if (optimumSegmentSize > _maxSegmentSize)
                optimumSegmentSize = _maxSegmentSize;
            
            lock (segmentationContext.SynchronizationLock)
            {
                var freeRanges = segmentationContext.Reverse().ToList();
                if (!freeRanges.Any())
                {
                    return null;
                }

                Segment segment = null;
                while (segment == null)
                {
                    foreach (var range in freeRanges)
                    {
                        var length = range.Length;
                        if (length / optimumSegmentSize <= 1)
                        {
                            optimumSegmentSize = length;
                        }
                        if (length >= optimumSegmentSize)
                        {
                            segment = new Segment
                            {
                                Min = range.Min,
                                Max = range.Min + Math.Min(optimumSegmentSize, length) - 1
                            };
                            break;
                        }
                    }
					//#error this is not IDM behavior... its sequential. IDM is binary.

                    if (segment == null)
                    {
                        optimumSegmentSize -= _minSegmentSize;
                        if (optimumSegmentSize <= 0)
                        {
                            segment = freeRanges.FirstOrDefault();
                            break;
                        }
                    }
                }

                if (segment == null)
                {
                    return null;
                }
                segmentationContext.ReservedRanges.Add(segment);

                return new PartialBlockDescriptor(segment);
            }
        }

        public PartialBlockDescriptor GetPart(IApplicationContext applicationContext, IJob job, SegmentationContext segmentationContext,
            long contiguousLeftOffset)
        {
            throw new NotImplementedException();
        }

        public int Order => 0;

        public void ResolveDependencies(IApplicationContext appContext, ITypeResolver typeResolver)
        {
            
        }

        public void LoadConfig(IApplicationContext applicationContext, IConfigProvider configProvider, HashSet<string> changes)
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