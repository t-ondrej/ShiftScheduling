using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class ConsecutiveIntervals : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Interval First { get; }
        public Interval Second { get; }

        public ConsecutiveIntervals(Interval first, Interval second)
        {
            ReportSeriousness = Seriousness.Warning;
            First = first;
            Second = second;
        }

        public override string GetReportMessage()
        {
            return $"Intervals ({First.Start}, {First.End}) and ({Second.Start}, {Second.End}) " +
                   $"can be concatenated into a single hour";
        }
    }
}