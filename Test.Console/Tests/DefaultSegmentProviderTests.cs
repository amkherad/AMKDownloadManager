using System.Diagnostics;
using AMKDownloadManager.Core;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using ir.amkdp.gear.arch.Trace.Annotations;
using ir.amkdp.gear.core.Utils;

namespace Test.Console.Tests
{
    //[TestClass(Order = 1)]
    public class DefaultSegmentProviderTests
    {
        private void _divideAndShow(SegmentationContext context)
        {
            var pool = AppContext.Context as IAppContext;
            var divider = pool.GetFeature<IJobDivider>();

            ChunkDescriptor cd = divider.GetChunk(pool, null, context);
            if (cd == null) return;
            do
            {
                Trace.WriteLine($"chunk= {cd.Segment.Min}:{cd.Segment.Max}");
                cd = divider.GetChunk(pool, null, context);
            } while (cd != null);
        }
        //[TestMethod]
        public void Test1()
        {
            var sc = new SegmentationContext(1000);

            _divideAndShow(sc);
        }
        //[TestMethod]
        public void Test2()
        {
            var sc = new SegmentationContext(1000000000);

            _divideAndShow(sc);
        }
        //[TestMethod]
        public void Test3()
        {
            var sc = new SegmentationContext(1000000000);

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
            
            _divideAndShow(sc);
        }
        [TestMethod]
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
            
            _divideAndShow(sc);
        }
        [TestMethod]
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
            
            _divideAndShow(sc);
        }
        [TestMethod]
        public void Test6()
        {
            var sc = new SegmentationContext(50 * Helper.MiB);

            sc.FilledRanges.Add(new Segment
            {
                Min = 30 * Helper.MiB + 1,
                Max = 40 * Helper.MiB + 1,
            });
            
            _divideAndShow(sc);
        }
        [TestMethod]
        public void Test7()
        {
            var sc = new SegmentationContext(50 * Helper.MiB);

            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50 * Helper.MiB,
            });
            
            _divideAndShow(sc);
        }
    }
}