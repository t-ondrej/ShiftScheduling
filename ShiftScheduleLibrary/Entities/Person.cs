using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleLibrary.Entities
{
    public class Person : IEquatable<Person>
    {
        public int Id { get; }

        public int MaxWork { get; }

        public IDictionary<int, DailyAvailability> DailyAvailabilities { get; }

        public Person(int id, int maxWork, IDictionary<int, DailyAvailability> dailyAvailabilities)
        {
            Id = id;
            MaxWork = maxWork;
            DailyAvailabilities = dailyAvailabilities;
        }

        public class DailyAvailability
        {
            public Interval Availability { get; }

            public int LeftTolerance { get; }

            public int RightTolerance { get; }

            public double ShiftWeight { get; }

            public DailyAvailability(Interval availability, int leftTolerance, int rightTolerance, double shiftWeight)
            {
                Availability = availability;
                LeftTolerance = leftTolerance;
                RightTolerance = rightTolerance;
                ShiftWeight = shiftWeight;
            }
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}