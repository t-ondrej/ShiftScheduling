using System.Collections.Generic;

namespace ShiftScheduleData.Entities
{
    public class MonthlyRequirements
    {
        public IDictionary<int, DailyRequirement> DaysToRequirements { get; }

        public MonthlyRequirements(IDictionary<int, DailyRequirement> daysToRequirements)
        {
            DaysToRequirements = daysToRequirements;
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