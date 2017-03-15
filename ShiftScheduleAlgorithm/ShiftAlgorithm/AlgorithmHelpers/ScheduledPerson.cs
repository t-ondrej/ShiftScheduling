using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class ScheduledPerson
    {
        public Person Person { get; }

        public IDictionary<int, int> NumberOfWorkedUnitsPerDays { get; }

        public int NumberOfAssignedUnits { get; private set; }

        public ScheduledPerson(Person person)
        {
            Person = person;
            NumberOfWorkedUnitsPerDays = new Dictionary<int, int>();
        }

        public void AddWorkForDay(int dayId, int numberOfTimeUnits)
        {
            if (!NumberOfWorkedUnitsPerDays.ContainsKey(dayId))
                NumberOfWorkedUnitsPerDays.Add(dayId, numberOfTimeUnits);
            else
                NumberOfWorkedUnitsPerDays[dayId] += numberOfTimeUnits;

            NumberOfAssignedUnits += numberOfTimeUnits;
        }
    }
}