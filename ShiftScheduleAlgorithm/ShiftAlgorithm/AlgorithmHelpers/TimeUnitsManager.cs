using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class TimeUnitsManager
    {
        public AlgorithmInput AlgorithmInput { get; }

        public List<TimeUnit> AllTimeUnits { get; }

        public List<ScheduledPerson> ScheduledPersons { get; private set; }

        public List<SchedulableWork> SchedulableWork { get; private set; }

        private readonly IDictionary<int, IDictionary<int, TimeUnit>> _requiredDaysToUnits;

        public TimeUnitsManager(AlgorithmInput algorithmInput)
        {
            AlgorithmInput = algorithmInput;
            _requiredDaysToUnits = new Dictionary<int, IDictionary<int, TimeUnit>>();
            AllTimeUnits = new List<TimeUnit>();
            FillMap();
        }

        private void FillMap()
        {
            ScheduledPersons = AlgorithmInput.Persons.Select(person => new ScheduledPerson(person)).ToList();

            SchedulableWork = ScheduledPersons.SelectMany
            (
                person => person.Person.DailyAvailabilities.Keys.Select
                (
                    dayId => new SchedulableWork(person, dayId)
                )
            ).ToList();

            foreach (var dayId in AlgorithmInput.Requirements.DaysToRequirements.Keys)
            {
                _requiredDaysToUnits.Add(dayId, new Dictionary<int, TimeUnit>());
            }

            foreach (var schedulableWork in SchedulableWork)
            {
                var dayId = schedulableWork.DayId;

                if (!_requiredDaysToUnits.ContainsKey(dayId))
                {
                    // TODO: Someone has scheduled work for a day that has no requirements. We should log it
                    continue;
                }

                var unitIdToUnit = _requiredDaysToUnits[schedulableWork.DayId];

                foreach (var unitOfDay in schedulableWork.DailyAvailability.Availability)
                {
                    var dailyRequirement = AlgorithmInput.Requirements.DaysToRequirements[dayId].HourToWorkers;

                    if (!dailyRequirement.ContainsKey(unitOfDay))
                    {
                        // TODO: Someone has scheduled work for a time unit that has no requirements. We should log it
                        continue;
                    }

                    var requiredWork = dailyRequirement[unitOfDay];

                    TimeUnit timeUnit;

                    if (unitIdToUnit.ContainsKey(unitOfDay))
                    {
                        timeUnit = unitIdToUnit[unitOfDay];
                    }
                    else
                    {
                        timeUnit = new TimeUnit(dayId, unitOfDay, requiredWork);
                        AllTimeUnits.Add(timeUnit);
                        unitIdToUnit.Add(unitOfDay, timeUnit);
                    }

                    timeUnit.PersonIdToPotentionalWork.Add(schedulableWork.ScheduledPerson.Person.Id, schedulableWork);
                }
            }
        }

        public IEnumerable<TimeUnit> GetTimeUnits(int dayId, Interval interval)
        {
            return _requiredDaysToUnits[dayId].Where(pair => interval.Contains(pair.Key)).Select(pair => pair.Value);
        }
    }
}