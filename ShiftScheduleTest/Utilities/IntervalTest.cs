using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleTest.Utilities
{
    [TestClass]
    class IntervalTest
    {
        [TestMethod]
        public void OverlappingTest()
        {
            var intervals = new List<Interval>
            {
                new Interval(1, 2),
                new Interval(3, 5),
                new Interval(4, 6),
                new Interval(1, 6)
            };

            Assert.IsFalse(Interval.AreOverlapping(intervals[0], intervals[1]));
            Assert.IsFalse(Interval.AreOverlapping(intervals[0], intervals[2]));
            Assert.IsTrue(Interval.AreOverlapping(intervals[1], intervals[2]));
            Assert.IsFalse(Interval.AreOverlapping(intervals[1], intervals[3]));
        }
    }
}
