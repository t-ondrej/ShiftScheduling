using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal class ScheduledPerson
    {
        public Person Person { get; }

        public IDictionary<int, int> NumberOfWorkedUnitsPerDays { get; }

        public int TotalNumberOfWorkedUnits { get; set; }

        public ScheduledPerson(Person person)
        {
            Person = person;
            NumberOfWorkedUnitsPerDays = new Dictionary<int, int>();
        }
    }
}
