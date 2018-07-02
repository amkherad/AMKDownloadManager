using System.Collections.Generic;
using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKsGear.Core.Utils;
using NUnit.Framework;

namespace AMKDownloadManager.NUnit.Categories.DownloadManager
{
    [TestFixture]
    public class DefaultSegmentProviderTests
    {
        private void _divideAndAssert(
            SegmentationContext context,
            IEnumerable<Segment> expectedSegments,
            bool finished = true)
        {
            var pool = AppContext.Context as IAppContext;
            var divider = pool.GetFeature<ISegmentDivider>();

            foreach (var seg in expectedSegments)
            {
                var cd = divider.GetPart(pool, null, context);

                var minEq = cd.Segment.Min == seg.Min;
                var maxEq = cd.Segment.Max == seg.Max;
                
                Assert.IsTrue(minEq, $"Expected min value {seg.Min} does not satisfied.");
                Assert.IsTrue(maxEq, $"Expected max value {seg.Max} does not satisfied.");
            }

            if (finished)
            {
                var cd = divider.GetPart(pool, null, context);
                Assert.IsNull(cd, "More segments than expected.");
            }
        }
        
        private void _divideAndShow(SegmentationContext context)
        {
            var pool = AppContext.Context as IAppContext;
            var divider = pool.GetFeature<ISegmentDivider>();

            PartialBlockDescriptor cd = divider.GetPart(pool, null, context);
            if (cd == null) return;
            do
            {
                Trace.WriteLine($"part= {cd.Segment.Min}:{cd.Segment.Max}");
                cd = divider.GetPart(pool, null, context);
            } while (cd != null);
        }
        
        [Test]
        public void Test1()
        {
            var sc = new SegmentationContext(1000); //1KB

            _divideAndAssert(sc, new []
                {
                    new Segment(0, 999), //1KB
                });
        }
        
        [Test]
        public void Test2()
        {
            var sc = new SegmentationContext(100000000); //1GB
            
            _divideAndAssert(sc, new []
            {
                new Segment(0, 10485759), //10MiB
                new Segment(10485760, 20971519), //10MiB
                new Segment(20971520, 31457279), //10MiB
                new Segment(31457280, 41943039), //10MiB
                new Segment(41943040, 52428799), //10MiB
                new Segment(52428800, 62914559), //10MiB
                new Segment(62914560, 73400319), //10MiB
                new Segment(73400320, 83886079), //10MiB
                new Segment(83886080, 99999999), //etc
            });
        }
        
        [Test]
        public void Test3()
        {
            var sc = new SegmentationContext(100000000); //1GB

            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 1000,
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 10001,
                Max = 70000,
            });
            
            _divideAndAssert(sc, new []
            {
                new Segment(1001, 10000),
                new Segment(70001, 10555760),    //10MiB
                new Segment(10555761, 21041520), //10MiB
                new Segment(21041521, 31527280), //10MiB
                new Segment(31527281, 42013040), //10MiB
                new Segment(42013041, 52498800), //10MiB
                new Segment(52498801, 62984560), //10MiB
                new Segment(62984561, 73470320), //10MiB
                new Segment(73470321, 83956080), //10MiB
                new Segment(83956081, 99999999)  //etc : more than 10MiB
            });
        }
        
        [Test]
        public void Test4()
        {
            var sc = new SegmentationContext(1000);

            sc.FilledRanges.Add(new Segment
            {
                Min = 2,
                Max = 200,
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 700,
                Max = 1000,
            });
            
            _divideAndAssert(sc, new []
            {
                new Segment(0, 1), 
                new Segment(201, 699),
            });
        }
        
        [Test]
        public void Test5()
        {
            var sc = new SegmentationContext(50 * Helper.MiB);

            sc.FilledRanges.Add(new Segment
            {
                Min = 2,
                Max = 200,
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 30 * Helper.MiB + 1,
                Max = 40 * Helper.MiB + 1,
            });
            
            _divideAndAssert(sc, new []
            {
                new Segment(0, 1),
                new Segment(201, 10485960),
                new Segment(10485961, 31457280),
                new Segment(41943042, 52428799),
            });
        }
        
        [Test]
        public void Test6()
        {
            var sc = new SegmentationContext(50 * Helper.MiB);

            sc.FilledRanges.Add(new Segment
            {
                Min = 30 * Helper.MiB + 1,
                Max = 40 * Helper.MiB + 1,
            });

            _divideAndAssert(sc, new []
            {
                new Segment(0, 10485759), 
                new Segment(10485760, 20971519), 
                new Segment(20971520, 31457280), 
                new Segment(41943042, 52428799), 
            });
        }
        
        [Test]
        public void Test7()
        {
            var sc = new SegmentationContext(50 * Helper.MiB);

            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50 * Helper.MiB,
            });

            _divideAndAssert(sc, new Segment[0]);
        }
        
        [Test]
        public void Test8()
        {
            var sc = new SegmentationContext(9 * Helper.KiB);

            _divideAndAssert(sc, new []
            {
                new Segment(0, 9 * Helper.KiB - 1), 
            });
        }
    }
}