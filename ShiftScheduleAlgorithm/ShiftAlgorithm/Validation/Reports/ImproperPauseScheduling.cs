using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class ImproperPauseScheduling : Report
    {
        public Person Person { get; }
        public int Day { get; }

        public override Seriousness ReportSeriousness { get; }

        public ImproperPauseScheduling(Person person, int day)
        {
            Person = person;
            Day = day;
            ReportSeriousness = Seriousness.Error;
        }

        public override string GetReportMessage()
        {
            return $"Improper pause scheduling on day {Day} for Person {Person.Id}";
        }
    }
}