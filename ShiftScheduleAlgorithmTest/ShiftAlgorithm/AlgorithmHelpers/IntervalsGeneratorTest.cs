using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithmTest.ShiftAlgorithm.AlgorithmHelpers
{
    [TestClass]
    public class IntervalsGeneratorTest
    {
        [TestMethod]
        public void GeneratorTestMethod()
        {
            var configuration = new AlgorithmConfiguration
            {
                MaxDailyWork = 4,
                MaxConsecutiveWorkHours = 2,
                WorkerPauseLength = 1
            };

            var generator = new IntervalsGenerator(5, configuration);
            const ShiftInterval.IntervalType work = ShiftInterval.IntervalType.Work;
            const ShiftInterval.IntervalType pause = ShiftInterval.IntervalType.Pause;

            for (var i = 0; i < 6; i++)
            {
                var intervals = generator.GetIntervalsWithLength(i).ToList();
                Assert.IsNotNull(intervals);

                switch (i)
                {
                    case 0:
                        Assert.AreEqual(intervals.Count, 0);
                        continue;
                    case 1:
                        Assert.AreEqual(intervals.Count, 5);
                        Assert.AreEqual(intervals[0].IntervalsList.Count, 1);
                        Assert.AreEqual(intervals[0].IntervalsList[0].Type, work);
                        break;
                    case 2:
                        Assert.AreEqual(intervals.Count, 4);
                        Assert.IsFalse(intervals.Any(interval => interval.IntervalsList.Count > 1));
                        Assert.IsFalse(intervals.Any(interval => interval.IntervalsList[0].Type == pause));
                        break;
                    case 3:
                        Assert.AreEqual(intervals.Count, 0);
                        break;
                    case 4:
                        Assert.AreEqual(intervals.Count, 2);
                        Assert.IsTrue(intervals.Select(intervals1 => intervals1.IntervalsList).All(l => l.Count == 3));
                        var oneIntervals = intervals.FirstOrDefault(l => l.IntervalsList[0].Start == 1);
                        Assert.IsNotNull(oneIntervals);
                        Assert.IsTrue(oneIntervals.IntervalsList[0].Equals(new ShiftInterval(1, 2, work)));
                        Assert.IsTrue(oneIntervals.IntervalsList[1].Equals(new ShiftInterval(3, 3, pause)));
                        Assert.IsTrue(oneIntervals.IntervalsList[2].Equals(new ShiftInterval(4, 4, work)));
                        break;
                    case 5:
                        Assert.AreEqual(intervals.Count, 0);
                        break;
                }
            }
        }
    }
}