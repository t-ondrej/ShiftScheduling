using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal class TimeUnitsManager
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public List<TimeUnit> AllTimeUnits { get; }

        public List<ScheduledPerson> ScheduledPersons { get; private set; }

        public List<SchedulableWork> SchedulableWork { get; private set; }

        private readonly IDictionary<int, IDictionary<int, TimeUnit>> _daysToUnits;

        public TimeUnitsManager(ShiftAlgorithm.Input algorithmInput)
        {
            AlgorithmInput = algorithmInput;
            _daysToUnits = new Dictionary<int, IDictionary<int, TimeUnit>>();
            AllTimeUnits = new List<TimeUnit>();
            FillMap();
        }

        private void FillMap()
        {
            ScheduledPersons = AlgorithmInput.Persons.Select(person => new ScheduledPerson(person)).ToList();

            SchedulableWork = ScheduledPersons.SelectMany
            (
                person => person.Person.MonthlySchedule.DailySchedules.SelectMany
                (
                    pair => pair.Value.IntervalsList.Select
                    (
                        interval => new SchedulableWork(person, pair.Key, interval)
                    )
                )
            ).ToList();

            foreach (var dayId in AlgorithmInput.MonthlyRequirements.DaysToRequirements.Keys)
            {
                _daysToUnits.Add(dayId, new Dictionary<int, TimeUnit>());
            }

            foreach (var schedulableWork in SchedulableWork)
            {
                var dayId = schedulableWork.DayId;

                if (!_daysToUnits.ContainsKey(dayId))
                {
                    // TODO: Someone has scheduled work for a day that has no monthlyRequirements. We should log it
                    continue;
                }

                var unitIdToUnit = _daysToUnits[schedulableWork.DayId];

                foreach (var unitOfDay in schedulableWork.Interval)
                {
                    var dailyRequirement = AlgorithmInput.MonthlyRequirements.DaysToRequirements[dayId].HourToWorkers;

                    if (!dailyRequirement.ContainsKey(unitOfDay))
                    {
                        // TODO: Someone has scheduled work for a time unit that has no monthlyRequirements. We should log it
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

                    timeUnit.IdToPotentionalWork.Add(schedulableWork.ScheduledPerson.Person.Id, schedulableWork);
                }
            }
        }

        public IEnumerable<TimeUnit> GetTimeUnits(int dayId, Interval interval)
        {
            return _daysToUnits[dayId].Where(pair => interval.Contains(pair.Key)).Select(pair => pair.Value);
        }
    }
}