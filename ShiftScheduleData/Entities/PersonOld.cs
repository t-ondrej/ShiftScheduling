using System;

namespace ShiftScheduleData.Entities
{
    public class PersonOld : IEquatable<PersonOld>
    {
        public int Id { get; }

        public Schedule Schedule { get; }

        public int MaxHoursPerMonth { get; }

        public PersonOld(int id, Schedule schedule, int maxHoursPerMonth)
        {
            Id = id;
            Schedule = schedule;
            MaxHoursPerMonth = maxHoursPerMonth;
        }

        public bool Equals(PersonOld other)
        {
            // generated
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || Id == other.Id);
        }

        public override bool Equals(object obj)
        {
            // generated
            return !ReferenceEquals(null, obj) && (
                       ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((PersonOld) obj));
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}