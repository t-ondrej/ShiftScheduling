using System.Collections.Generic;

namespace ShiftScheduleData.Entities
{
    public class Requirements
    {
        public IDictionary<int, DailyRequirement> DayToRequirement { get; }

        public Requirements(IDictionary<int, DailyRequirement> dayToRequirement)
        {
            DayToRequirement = dayToRequirement;
        }

        public class DailyRequirement
        {
            public IDictionary<int, int> HourToWorkers { get; }

            public DailyRequirement(IDictionary<int, int> hourToWorkers)
            {
                HourToWorkers = hourToWorkers;
            }
        }
    }
}