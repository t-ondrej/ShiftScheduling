using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers;
using ShiftScheduleDataAccess.OldEntities;
using ShiftScheduleLibrary.Utilities;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    internal class TestAlgorithm : ShiftAlgorithm.Algorithm
    {
        private static readonly Comparer<TimeUnit> UnitsComparer;

        static TestAlgorithm()
        {
            UnitsComparer = Comparer<TimeUnit>.Create((t1, t2) => t1.WorkersToSpare.CompareTo(t2.WorkersToSpare));
        }

        public TestAlgorithm(ShiftAlgorithm.Input algorithmInput) : base(algorithmInput)
        {
        }

        public override ResultingScheduleOld CreateScheduleForPeople()
        {
            var timeUnitsManager = new TimeUnitsManager(AlgorithmInput);

            while (true)
            {
                // We have the enemerable of all time units that are not fulfiled
                var allTimeUnits = timeUnitsManager.AllTimeUnits.Where(u => !u.Fulfilled).ToList();

                // If there are no unfulfilled time units, we are done
                if (!allTimeUnits.Any())
                    break;

                // We pick the element where there are fewest workest to spare
                var fewestUnit = allTimeUnits.MinBy(unit => unit, UnitsComparer);

                // We pick some random scheduled work that can cover this unit
                var scheduledWork = fewestUnit.IdToPotentionalWork.Values.FirstOrDefault(work => !work.Scheduled);

                if (scheduledWork == null)
                {
                    Console.WriteLine("This is not good. Our great algorithm has FAILED! " +
                                      "But maybe it's just proven the solution doesn't exist.");
                    return null;
                }

                // We obtain all the time units that covers this random work
                var timeUnits = timeUnitsManager.GetTimeUnits(scheduledWork.DayId, scheduledWork.Interval);

                // Update theirs number of workers
                foreach (var timeUnit in timeUnits)
                    timeUnit.AssignedWorkers++;

                // We update scheduled state of the work
                scheduledWork.Scheduled = true;

                // Finally we update persons scheduleOld
                var scheduledPerson = scheduledWork.ScheduledPerson;
                var personsdayToHours = scheduledPerson.NumberOfWorkedUnitsPerDays;
                var dayId = scheduledWork.DayId;

                if (!personsdayToHours.ContainsKey(dayId))
                    personsdayToHours.Add(dayId, 0);

                var intervalCount = scheduledWork.Interval.Count;
                scheduledPerson.TotalNumberOfWorkedUnits += intervalCount;
                personsdayToHours[dayId] += intervalCount;
            }

            return CreateResultingSchedule(timeUnitsManager);
        }

        private static ResultingScheduleOld CreateResultingSchedule(TimeUnitsManager timeUnits)
        {
            var dictionary = new Dictionary<PersonOld, ScheduleOld>();

            foreach (var schedulableWork in timeUnits.SchedulableWork.Where(work => work.Scheduled))
            {
                var scheduledPerson = schedulableWork.ScheduledPerson;
                var person = scheduledPerson.PersonOld;

                if (!dictionary.ContainsKey(person))
                {
                    dictionary.Add(person, new ScheduleOld(new Dictionary<int, Intervals<Interval>>()));
                }

                var dailySchedules = dictionary[person].DailySchedules;
                var dayId = schedulableWork.DayId;

                if (!dailySchedules.ContainsKey(dayId))
                {
                    dailySchedules.Add(dayId, new Intervals<Interval>(new List<Interval>()));
                }

                dailySchedules[dayId].IntervalsList.Add(schedulableWork.Interval);
            }

            return new ResultingScheduleOld(dictionary);
        }
    }
}