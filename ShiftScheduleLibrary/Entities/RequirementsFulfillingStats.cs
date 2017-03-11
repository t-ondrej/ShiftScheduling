using System.Collections.Generic;

namespace ShiftScheduleLibrary.Entities
{
    public class RequirementsFulfillingStats
    {
        public IDictionary<int, PersonStats> PersonsStats { get; }

        public RequirementsFulfillingStats(IDictionary<int, PersonStats> personsStats)
        {
            PersonsStats = personsStats;
        }

        public class PersonStats
        {
            public IDictionary<int, double> PeriodToFulfilling { get; }

            public PersonStats(IDictionary<int, double> periodToFulfilling)
            {
                PeriodToFulfilling = periodToFulfilling;
            }
        }
    }
}