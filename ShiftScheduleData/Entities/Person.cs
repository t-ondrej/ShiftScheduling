using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.Entities
{
    public class Person
    {
        public int Id { get; }

        public Schedule MonthlySchedule { get; }

        public int MaxHoursPerMonth { get; }

        public Person(int id, Schedule monthlySchedule, int maxHoursPerMonth)
        {
            Id = id;
            MonthlySchedule = monthlySchedule;
            MaxHoursPerMonth = maxHoursPerMonth;
        }
    }
}