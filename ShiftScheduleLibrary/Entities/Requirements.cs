using System.Collections.Generic;

namespace ShiftScheduleLibrary.Entities
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
            public IList<double> HourToWorkers { get; }

            public DailyRequirement(IList<double> hourToWorkers)
            {
                HourToWorkers = hourToWorkers;
            }
        }
    }
}