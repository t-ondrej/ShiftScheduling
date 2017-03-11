using System.Collections.Generic;
using ShiftScheduleDataAccess.OldEntities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal class ScheduledPerson
    {
        public PersonOld PersonOld { get; }

        public IDictionary<int, int> NumberOfWorkedUnitsPerDays { get; }

        public int TotalNumberOfWorkedUnits { get; set; }

        public ScheduledPerson(PersonOld personOld)
        {
            PersonOld = personOld;
            NumberOfWorkedUnitsPerDays = new Dictionary<int, int>();
        }
    }
}
