using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class MaxConsecutiveWorkHoursNotMet : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public MaxConsecutiveWorkHoursNotMet(Person person, int day)
        {
            ReportSeriousness = Seriousness.Error;
            Person = person;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than maximum consecutive working " +
                   $"hours on day {Day}";
        }
    }
}