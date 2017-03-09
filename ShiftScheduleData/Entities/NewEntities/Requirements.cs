using System.Collections.Generic;

namespace ShiftScheduleData.Entities.NewEntities
{
    public class Requirements
    {
        public IDictionary<int, DailyRequirement> DaysToRequirements { get; }

        public Requirements(IDictionary<int, DailyRequirement> daysToRequirements)
        {
            DaysToRequirements = daysToRequirements;
        }

        public class DailyRequirement
        {
            public IDictionary<int, double> HourToWorkers { get; }

            public DailyRequirement(IDictionary<int, double> hourToWorkers)
            {
                HourToWorkers = hourToWorkers;
            }
        }
    }
}