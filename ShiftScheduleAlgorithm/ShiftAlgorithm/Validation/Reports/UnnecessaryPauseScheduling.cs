using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class UnnecessaryPauseScheduling : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public UnnecessaryPauseScheduling(Person person, int day)
        {
            Person = person;
            Day = day;
            ReportSeriousness = Seriousness.Warning;
        }

        public override string GetReportMessage()
        {
            return $"Detected an unneccessary Pause Scheduling on day {Day} for Person {Person.Id}, " +
                   $"the person could be working";
        }
    }
}