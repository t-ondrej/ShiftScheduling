using System;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.Entities
{
    public class Person : IEquatable<Person>
    {
        public int Id { get; }

        public MonthlySchedule MonthlySchedule { get; }

        public int MaxHoursPerMonth { get; }

        public Person(int id, MonthlySchedule monthlySchedule, int maxHoursPerMonth)
        {
            Id = id;
            MonthlySchedule = monthlySchedule;
            MaxHoursPerMonth = maxHoursPerMonth;
        }

        public bool Equals(Person other)
        {
            // generated
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            // generated
            return !ReferenceEquals(null, obj) && (
                       ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Person) obj));
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}