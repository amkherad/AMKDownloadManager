using System.Diagnostics;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKsGear.Architecture.Trace.Annotations;

namespace AMKDownloadManager.MSTest.Categories.Segmentation
{
    [TestClass]
    public class SegmentationContextTest
    {
        [TestMethod]
        public void Test1()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 100,
                Max = 200
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 400,
                Max = 500
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 460,
                Max = 530
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 501,
                Max = 550
            });

            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void Test2()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 200
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 700,
                Max = 1000
            });

            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void Test3()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 1,
                Max = 200
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 700,
                Max = 1000
            });

            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void Test4()
        {
            var sc = new SegmentationContext(1000);

            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void Test5()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 1000
            });
            
            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void Test6()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 999
            });
            
            var ranges = sc.Reverse();

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
        
        [TestMethod]
        public void TestClean()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 51,
                Max = 100
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 51,
                Max = 150
            });
            
            sc.Clean();
            var ranges = sc.FilledRanges;

            foreach (var range in ranges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }
    }
}