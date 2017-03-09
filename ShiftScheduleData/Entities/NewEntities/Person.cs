using System.Collections.Generic;
using ShiftScheduleData.Entities.NewEntities.Helpers;

namespace ShiftScheduleData.Entities.NewEntities
{
    public class Person
    {
        public int Id { get; }

        public IDictionary<int, DailyAvailability> DailyAvailabilities { get; }

        public Person(int id, IDictionary<int, DailyAvailability> dailyAvailabilities)
        {
            Id = id;
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

    }
}