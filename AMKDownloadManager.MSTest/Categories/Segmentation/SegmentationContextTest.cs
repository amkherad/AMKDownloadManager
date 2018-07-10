using System.Diagnostics;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKsGear.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            sc.FilledRanges.Add(new Segment
            {
                Min = 300,
                Max = 360
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 330,
                Max = 365
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 360,
                Max = 370
            });
            
            sc.Clean();

            foreach (var range in sc.FilledRanges)
            {
                Trace.WriteLine($"Range= {range.Min}:{range.Max}");
            }
        }

        [TestMethod]
        public void TestGetSegmentGrowthRightLimit()
        {
            var sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 41,
                Max = 100
            });

            var range = sc.GetSegmentGrowthRightLimit(50, 4 * Helper.KiB);
            Assert.IsNull(range);

            Trace.WriteLine($"Range= (range is null := {range == null}){range?.Min ?? 0}:{range?.Max ?? 0}");
            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 71,
                Max = 100
            });

            range = sc.GetSegmentGrowthRightLimit(50, 1000);
            Assert.IsNotNull(range);
            Assert.AreEqual(range.Min, 51);
            Assert.AreEqual(range.Max, 70);

            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 71,
                Max = 100
            });

            range = sc.GetSegmentGrowthRightLimit(50, 10);
            Assert.IsNotNull(range);
            Assert.AreEqual(range.Min, 51);
            Assert.AreEqual(range.Max, 60);

            Trace.WriteLine($"Range= {range.Min}:{range.Max} ({range.Length})");
            
            sc.FilledRanges.Add(new Segment
            {
                Min = 387,
                Max = 395
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 400,
                Max = 999
            });
            range = sc.GetSegmentGrowthRightLimit(124, 10000);
            Assert.IsNotNull(range);

            Trace.WriteLine($"Range= {range.Min}:{range.Max} ({range.Length})");
            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
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

            range = sc.GetSegmentGrowthRightLimit(50, 4 * Helper.KiB);
            Assert.IsNull(range);
            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });

            range = sc.GetSegmentGrowthRightLimit(50, 4 * Helper.KiB);
            Assert.IsNotNull(range);
            Assert.AreEqual(range.Min, 51);
            Assert.AreEqual(range.Max, 999);
            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 51,
                Max = 70
            });

            range = sc.GetSegmentGrowthRightLimit(70, 1000);
            Assert.IsNotNull(range);
            Assert.AreEqual(range.Min, 71);
            Assert.AreEqual(range.Max, 999);
            
            
            //================================================
            
            
            sc = new SegmentationContext(1000);
            sc.FilledRanges.Add(new Segment
            {
                Min = 0,
                Max = 50
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 25,
                Max = 60
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 70,
                Max = 90
            });
            sc.FilledRanges.Add(new Segment
            {
                Min = 100,
                Max = 300
            });

            range = sc.GetSegmentGrowthRightLimit(50, 4 * Helper.KiB);
            Assert.IsNull(range);
        }

        [TestMethod]
        public void CheckIfAllFilledTests()
        {
            var sc = new SegmentationContext(1000);

            sc.FilledRanges.Add(new Segment(0, 999));
            
            Assert.IsTrue(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(0, 400));
            sc.FilledRanges.Add(new Segment(401, 999));
            
            Assert.IsTrue(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(0, 400));
            sc.FilledRanges.Add(new Segment(402, 999));
            
            Assert.IsFalse(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(0, 400));
            sc.FilledRanges.Add(new Segment(401, 998));
            
            Assert.IsFalse(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(1, 400));
            sc.FilledRanges.Add(new Segment(401, 999));
            
            Assert.IsFalse(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(0, 1));
            sc.FilledRanges.Add(new Segment(2, 999));
            
            Assert.IsTrue(sc.CheckIfAllFilled());
            
            sc.Reset();
            
            
            //================================================
            
            
            sc.FilledRanges.Add(new Segment(0, 1));
            sc.FilledRanges.Add(new Segment(3, 999));
            
            Assert.IsFalse(sc.CheckIfAllFilled());
            
            sc.Reset();
        }
    }
}