using System;
using System.Collections.Generic;
using System.Linq;
using ir.amkdp.gear.arch.Modeling.Annotations;
using ir.amkdp.gear.arch.Parallelism;
using ir.amkdp.gear.core.Collections;
using ir.amkdp.gear.core.Parallelism;
using ir.amkdp.gear.data.Models;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    public delegate void SegmentationContextSegmentChangedEvent(SegmentationContext segmentationContext, Segment segment);
    
    public class Segment : Int64RangeModel
    {
        public bool LimitedSegment { get; set; }
        
        public Segment() { }
        public Segment(long min, long max)
        {
            Min = min;
            Max = max;
        }
    }

    public class SegmentationContext
    {
        public event SegmentationContextSegmentChangedEvent SegmentAdded;
        public event SegmentationContextSegmentChangedEvent SegmentRemoved;
        public event EventHandler TotalSizeChanged;
        
        public object SyncRoot { get; }
        public long TotalSize { get; }
        public SortedSet<Segment> FilledRanges { get; }
        public SortedSet<Segment> ReservedRanges { get; }

        //public Segment LastRange { get; set; }
        public PropertyBag Properties { get; } = new PropertyBag();

        private readonly MinRangeComparer _comparer;

        public SegmentationContext(long totalSize)
        {
            SyncRoot = new object();
            TotalSize = totalSize;
            _comparer = new MinRangeComparer();
            FilledRanges = new SortedSet<Segment>(_comparer);
            ReservedRanges = new SortedSet<Segment>(_comparer);
        }

        protected class MinRangeComparer : Comparer<Segment>
        {
            private readonly Comparer<long> _comparer = Comparer<long>.Default;

            public override int Compare(Segment x, Segment y)
                => _comparer.Compare(x.Min, y.Min);
        }

        public ISyncBlock BeginMonitor()
        {
            return new MonitorBlock(SyncRoot);
        }

        public void Clean()
        {
            var mergePairs = new List<Tuple<Segment, Segment, IList<Segment>>>();

            //Known bug: 0-50,51-100,51-150 result: 0-100,51-150
            
            lock (SyncRoot)
            {
                foreach (var segment in FilledRanges)
                {
                    var nears = FilledRanges.Where(x => Math.Abs(x.Max - segment.Min) <= 1);

                    foreach (var n in nears)
                    {
                        var exists = mergePairs.FirstOrDefault(x => x.Item3.Any(i => i == segment || i == n));

                        if (exists != null)
                        {
                            var sExists = exists.Item3.FirstOrDefault(x => x == segment);
                            if (sExists != null)
                            {
                                sExists.Min = Math.Min(segment.Min, n.Min);
                                sExists.Max = Math.Max(segment.Max, n.Max);
                                exists.Item3.Add(segment);
                            }
                            else
                            {
                                var nExists = exists.Item3.FirstOrDefault(x => x == n);
                                if (nExists != null)
                                {
                                    nExists.Min = Math.Min(segment.Min, n.Min);
                                    nExists.Max = Math.Max(segment.Max, n.Max);
                                    exists.Item3.Add(segment);
                                }
                            }
                        }
                        else
                        {
                            mergePairs.Add(new Tuple<Segment, Segment, IList<Segment>>(segment, n, new List<Segment>
                            {
                                segment,
                                n
                            }));
                        }
                    }
                }

                foreach (var merge in mergePairs)
                {
                    FilledRanges.RemoveAll(merge.Item3);
                    FilledRanges.Add(new Segment
                    {
                        Min = merge.Item3.Min(i => i.Min),
                        Max = merge.Item3.Max(i => i.Max)
                    });
                }
            }
        }

        public IEnumerable<Segment> Reverse()
        {
            lock (SyncRoot)
            {
                var ranges = new SortedSet<Segment>(FilledRanges, _comparer);
                ranges.AddRange(ReservedRanges);

                var reversed = new List<Segment>();

                //var lastInsertMin = 0L;
                //var lastInsertMax = 0L;
                var min = 0L;
                var max = 0L;
                foreach (var range in ranges)
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
        }

        /// <summary>
        /// Tracks the maximum growth limit of a segment (i.e. start of another segment)
        /// </summary>
        /// <param name="offset">Any random offset inside a segment.</param>
        /// <returns></returns>
//        public long GetSegmentGrowthRightLimit(long offset)
//        {
//            
//        }
    }
}