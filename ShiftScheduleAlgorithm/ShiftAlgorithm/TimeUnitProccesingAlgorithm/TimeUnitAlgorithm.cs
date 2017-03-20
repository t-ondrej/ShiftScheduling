using System.Collections.Generic;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal class TimeUnitAlgorithm : Algorithm
    {
        private TimeUnitsManager _timeUnitsManager;

        private readonly TimeUnitStrategy _timeUnitStrategy;

        public TimeUnitAlgorithm(AlgorithmInput algorithmInput): base(algorithmInput)
        {
            _timeUnitStrategy = algorithmInput.AlgorithmConfiguration.AlgorithmStrategy as TimeUnitStrategy;
        }

        public override ResultingSchedule CreateScheduleForPeople()
        {
            _timeUnitsManager = new TimeUnitsManager(AlgorithmInput);

            while (true)
            {
                // First we let the algoritm to find the time unit to be proccessed
                var timeUnit = _timeUnitStrategy.TimeUnitChooser.FindTimeUnitToBeProccessed(_timeUnitsManager);

                // If there's no such unit, we can celebrate. We're done
                if (timeUnit == null)
                {
                    break;
                }

                var schedule = _timeUnitStrategy.ScheduleChooser.FindScheduleToCoverUnit(_timeUnitsManager, timeUnit);

                if (schedule == null)
                {
                    // We can't fulfil the given time unit. But we can mark it as unsucessful and move on.
                    timeUnit.Fulfillable = false;
                    continue;
                }

                timeUnit.AssignSchedule(schedule);
            }

            _timeUnitStrategy.RemainingPeopleChooser.AssignScheduleToRemainingPeople(_timeUnitsManager);

            return CreateResultingSchedule();
        }

        private ResultingSchedule CreateResultingSchedule()
        {
            var dailySchedules = new Dictionary<int, ResultingSchedule.DailySchedule>();

            _timeUnitsManager.ScheduledPersons.ForEach(scheduledPerson =>
            {
                var assignedDays = scheduledPerson.AssignedDays;
                var personId = scheduledPerson.Person.Id;

                foreach (var scheduleForDay in assignedDays)
                {
                    var dayId = scheduleForDay.Key;
                    var intervals = scheduleForDay.Value.Intervals;

                    if (!dailySchedules.ContainsKey(dayId))
                    {
                        var personIdToIntervals = new Dictionary<int, Intervals<ShiftInterval>>();
                        dailySchedules.Add(dayId, new ResultingSchedule.DailySchedule(personIdToIntervals));
                    }

                    dailySchedules[dayId].PersonIdToDailySchedule.Add(personId, intervals);
                }
            });

            return new ResultingSchedule(dailySchedules);
        }
    }
}