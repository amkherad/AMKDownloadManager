using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using AMKsGear.Architecture.Modeling.Annotations;
using AMKsGear.Architecture.Parallelism;
using AMKsGear.Core.Collections;
using AMKsGear.Core.Parallelism;
using AMKsGear.Core.Data.Models;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public delegate void SegmentationContextSegmentChangedEvent(SegmentationContext segmentationContext,
        Segment segment);

    public class Segment : Int64RangeModel
    {
        public bool LimitedSegment { get; set; }

        public Segment()
        {
        }

        public Segment(long min, long max)
        {
            if (max <= min)
                throw
                    new InvalidOperationException(); //this line used to remove some extra checks in SegmentationContext codes.

            Min = min;
            Max = max;
        }
    }

    public class SegmentationContext : ISegmentation
    {
        public event SegmentationContextSegmentChangedEvent SegmentAdded;
        public event SegmentationContextSegmentChangedEvent SegmentRemoved;
        public event EventHandler TotalSizeChanged;

        /// <summary>
        /// Synchronization lock for this instance of <see cref="SegmentationContext"/>
        /// </summary>
        public object SynchronizationLock { get; }

        /// <summary>
        /// Total download size.
        /// </summary>
        public long TotalSize { get; }

        /// <summary>
        /// All segments with downloaded data.
        /// </summary>
        public SortedSet<Segment> FilledRanges { get; }

        /// <summary>
        /// All segments that currently is downloading.
        /// </summary>
        public SortedSet<Segment> ReservedRanges { get; }

        IEnumerable<Segment> ISegmentation.FilledRanges => FilledRanges;
        IEnumerable<Segment> ISegmentation.ReservedRanges => ReservedRanges;

        //public Segment LastRange { get; set; }

        /// <summary>
        /// Extended property bag provided for modules to store desired data.
        /// </summary>
        public PropertyBag Properties { get; } = new PropertyBag();

        private readonly MinRangeComparer _comparer;

        public SegmentationContext(long totalSize)
        {
            SynchronizationLock = new object();
            TotalSize = totalSize;
            _comparer = new MinRangeComparer();
            FilledRanges = new SortedSet<Segment>(_comparer);
            ReservedRanges = new SortedSet<Segment>(_comparer);
        }

        protected class MinRangeComparer : Comparer<Segment>
        {
            public override int Compare(Segment x, Segment y)
            {
                var xMin = x.Min;
                var yMin = y.Min;

                if (xMin < yMin)
                {
                    return -1;
                }

                if (xMin > yMin)
                {
                    return 1;
                }

                var xMax = x.Max;
                var yMax = y.Max;

                if (xMax < yMax)
                {
                    return -1;
                }

                if (xMax > yMax)
                {
                    return 1;
                }

                return 0;
            }
        }

        public ISyncBlock BeginMonitor()
        {
            return new MonitorBlock(SynchronizationLock);
        }

        public void CleanWithLock()
        {
            lock (SynchronizationLock)
            {
                Clean();
            }
        }

        public void Clean()
        {
            var mergePairs = new List<Segment[]>();

            var max = -1L;
            var continuousSegments = new List<Segment>();

            foreach (var segment in FilledRanges)
            {
                var segmentMin = segment.Min;
                var segmentMax = segment.Max;

                if (segmentMin < max)
                {
                    if (max < segmentMax) //handles overlaps.
                    {
                        max = segmentMax;
                    }
                }
                else if (Math.Abs(segmentMin - max) <= 1)
                {
                    //if (segmentMax > max) //rare condition?? or impossible??
                    //I made Segment's constructor to check for (max <= min)
                    {
                        max = segmentMax;
                    }
                }
                else if (max == -1L)
                {
                    max = segmentMax;
                }
                else
                {
                    max = segmentMax;

                    if (continuousSegments.Count > 1)
                    {
                        mergePairs.Add(continuousSegments.ToArray());
                    }

                    continuousSegments.Clear();
                }

                continuousSegments.Add(segment);
            }

            if (continuousSegments.Count > 1)
            {
                mergePairs.Add(continuousSegments.ToArray());
            }

            foreach (var merge in mergePairs)
            {
                FilledRanges.RemoveWhere(p => merge.Any(m => ReferenceEquals(p, m)));
                FilledRanges.Add(new Segment
                {
                    Min = merge.Min(i => i.Min),
                    Max = merge.Max(i => i.Max)
                });
            }
        }

        public IEnumerable<Segment> ReverseWithLock()
        {
            lock (SynchronizationLock)
            {
                return Reverse();
            }
        }

        public IEnumerable<Segment> Reverse()
        {
            var ranges = new SortedSet<Segment>(FilledRanges, _comparer);
            ranges.AddRange(ReservedRanges);

            var reversed = new List<Segment>();

            //var lastInsertMin = 0L;
            //var lastInsertMax = 0L;
            var min = 0L;
            var max = 0L;
            foreach (var range in ranges) //order guaranteed?
            {
                if (range.Min > min)
                {
                    //lastInsertMin = min;
                    //lastInsertMax = ;
                    reversed.Add(new Segment
                    {
                        Min = min,
                        Max = range.Min - 1
                    });
                    min = range.Max + 1;
                }
                else if (range.Min == min && range.Max > min && range.Max < TotalSize - 1)
                {
                    min = range.Max + 1;
                }
                else if (range.Max > min) //range.Min < min
                {
                    min = range.Max + 1;
                }

                max = range.Max;
            }

            if (max < TotalSize - 1)
            {
                //lastInsertMin = min;
                reversed.Add(new Segment
                {
                    Min = min,
                    Max = TotalSize - 1
                });
            }

            return reversed;
        }

        public Segment GetSegmentGrowthRightLimitWithLock(long prevSegmentMax, long maxSegmentLength)
        {
            lock (SynchronizationLock)
            {
                return GetSegmentGrowthRightLimit(prevSegmentMax, maxSegmentLength);
            }
        }

        /// <summary>
        /// Tracks the maximum growth limit of a segment (i.e. start of another segment)
        /// </summary>
        /// <param name="prevSegmentMax">Previous segment max value.</param>
        /// <param name="maxSegmentLength">Maximum segment length to grow.</param>
        /// <returns></returns>
        public Segment GetSegmentGrowthRightLimit(long prevSegmentMax, long maxSegmentLength)
        {
            var totalSize = TotalSize;
            var closestValue = totalSize;

            foreach (var range in ReservedRanges.Where(x => x.Max > prevSegmentMax)) //order guaranteed
            {
                var rangeMin = range.Min;
                if (rangeMin <= prevSegmentMax && range.Max > prevSegmentMax)
                {
                    //SortedSet so there is no continuous segment!
                    closestValue = -1;
                    break;
                }

                if (rangeMin > prevSegmentMax && rangeMin < closestValue)
                {
                    closestValue = rangeMin;
                }
            }

            foreach (var range in FilledRanges.Where(x => x.Max > prevSegmentMax)) //order guaranteed
            {
                var rangeMin = range.Min;
                if (rangeMin <= prevSegmentMax && range.Max > prevSegmentMax)
                {
                    //SortedSet so there is no continuous segment!
                    closestValue = -1;
                    break;
                }

                if (rangeMin > prevSegmentMax && rangeMin < closestValue)
                {
                    closestValue = rangeMin;
                }
            }

            if (closestValue == -1 || closestValue - prevSegmentMax <= 1)
            {
                return null;
            }

            maxSegmentLength += prevSegmentMax;
            prevSegmentMax++;
            closestValue--;

            if (maxSegmentLength < closestValue)
            {
                closestValue = maxSegmentLength;
            }

            return new Segment(prevSegmentMax, closestValue);
        }

        public bool CheckIfAllFilledWithLock()
        {
            lock (SynchronizationLock)
            {
                return CheckIfAllFilled();
            }
        }

        /// <summary>
        /// Checks for all the SegmentationContext is covered with FilledRanges segments.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfAllFilled()
        {
            var totalSize = TotalSize;
            var max = -1L;

            foreach (var segment in FilledRanges) //order guaranteed
            {
                var segmentMin = segment.Min;
                var segmentMax = segment.Max;

                if (segmentMin < max)
                {
                    if (max < segmentMax) //handles overlaps.
                    {
                        max = segmentMax;
                    }
                }
                else if (Math.Abs(segmentMin - max) <= 1)
                {
                    max = segmentMax;
                }

                if (max >= totalSize - 1)
                {
                    return true;
                }
            }

            return false;
        }

        public void MarkReservedAsFilledWithLock(Segment segment)
        {
            lock (SynchronizationLock)
            {
                MarkReservedAsFilled(segment);
            }
        }

        public void MarkReservedAsFilled(Segment segment)
        {
            ReservedRanges.Remove(segment);
            FilledRanges.Add(segment);
        }

        public void ResetWithLock()
        {
            lock (SynchronizationLock)
            {
                Reset();
            }
        }

        public void Reset()
        {
            FilledRanges.Clear();
            ReservedRanges.Clear();
        }
    }
}