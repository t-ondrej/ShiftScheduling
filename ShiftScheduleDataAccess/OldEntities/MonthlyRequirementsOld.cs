using System.Collections.Generic;

namespace ShiftScheduleDataAccess.OldEntities
{
    public class MonthlyRequirementsOld
    {
        public IDictionary<int, DailyRequirement> DaysToRequirements { get; }

        public MonthlyRequirementsOld(IDictionary<int, DailyRequirement> daysToRequirements)
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