using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm
{
    internal class TestAlgorithm : Algorithm
    {
        private static readonly Comparer<TimeUnit> UnitsComparer;

        static TestAlgorithm()
        {
            UnitsComparer = Comparer<TimeUnit>.Create((t1, t2) => t1.CurrentWorkToSpare.CompareTo(t2.CurrentWorkToSpare));
        }

        private TimeUnitsManager _timeUnitsManager;

        public TestAlgorithm(AlgorithmInput algorithmInput) : base(algorithmInput)
        {
        }

        public override ResultingSchedule CreateScheduleForPeople()
        {
            _timeUnitsManager = new TimeUnitsManager(AlgorithmInput);

            AssignmentAlgorithm();

            return CreateResultingSchedule();
        }

        // One idea is to consider pauses and run the algorithm so many times until it doesn't stop
        // The other idea about pauses is, that for particular unit, start the work such that the pause isn't 
        // Gonna be a problem. The problem might be, when we want to extend it actually.

        private void AssignmentAlgorithm()
        {
            while (true)
            {
                // We have the enemerable of all time units that are not fulfiled and have worked to spare
                var allTimeUnits = _timeUnitsManager.AllTimeUnits.Where(u => !u.Fulfilled && u.Fulfillable).ToList();

                // If there are no unfulfilled time units, we are done
                if (!allTimeUnits.Any())
                    break;

                // We pick the elements where there are fewest workest to spare
                // TODO: Consider a more clever way to find the most critical spots
                var fewestUnits = allTimeUnits.MinBy(unit => unit, UnitsComparer);

                // By some cool way we pick the unit that we want to fulfil
                // TODO: Consider a better way
                var fewestUnit = fewestUnits.First();

                // By some pick some random scheduled work that can cover this unit with
                // We should have some, because otherwise the unit would be unfulfillable
                // TODO: Consider a better way
                var scheduledWork = fewestUnit.AllScSchedulableWork.First(
                    work => work.StateOfWork == SchedulableWork.State.Assignable);

                // We obtain all the time units that covers this work.
                // The other option would be to take just part of the work, even if we don't have to
                // because of the max works per day / month
                var dayId = scheduledWork.DayId;
                var availabilityInterval = scheduledWork.DailyAvailability.Availability;
                var timeUnits = _timeUnitsManager.GetTimeUnits(dayId, availabilityInterval);

                // Assign the work to the particular time units
                // TODO: Decide, whether it does or doesn't make sense to assign the whole interval
                // TODO: Pause spliting
                // TODO: Max amount of work check
                foreach (var timeUnit in timeUnits)
                {
                    timeUnit.AssignedSchedulableWork(scheduledWork);
                }

                // Finally we update persons schedule
                var numberOfHours = scheduledWork.DailyAvailability.Availability.Count;
                scheduledWork.ScheduledPerson.AddWorkForDay(dayId, numberOfHours);
            }
        }

        private static ResultingSchedule CreateResultingSchedule()
        {
            return null;
        }
    }
}