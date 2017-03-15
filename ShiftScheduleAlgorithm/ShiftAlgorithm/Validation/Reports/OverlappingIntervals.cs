using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class OverlappingIntervals : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Intervals<ResultingSchedule.ShiftInterval> Intervals { get; }
        public int Day { get; }

        public OverlappingIntervals(Intervals<ResultingSchedule.ShiftInterval> intervals, int day)
        {
            ReportSeriousness = Seriousness.Error;
            Intervals = intervals;
            Day = day;
        }

        public override string GetReportMessage()
        {
            var message = Intervals.Aggregate("Intervals ",
                (current, interval) => current + $"({interval.Start}, {interval.End}) ");

            return $"{message}overlap on day {Day}";
        }
    }
}