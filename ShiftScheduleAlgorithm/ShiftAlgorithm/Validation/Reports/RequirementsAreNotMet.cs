namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class RequirementsAreNotMet : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public int Day { get; }
        public int Hour { get; }
        public double WorkersCount { get; }

        public RequirementsAreNotMet(int day, int hour, double workersCount)
        {
            ReportSeriousness = Seriousness.Error;
            Day = day;
            Hour = hour;
            WorkersCount = workersCount;
        }

        public override string GetReportMessage()
        {
            return $"Insufficient workers for day {Day} at hour {Hour}. Missing {WorkersCount} shiftWeight.";
        }
    }
}