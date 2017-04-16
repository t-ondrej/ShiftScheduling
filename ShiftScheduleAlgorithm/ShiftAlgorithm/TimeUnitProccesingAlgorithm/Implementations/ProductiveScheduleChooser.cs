using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleUtilities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    class ProductiveScheduleChooser : IScheduleChooser
    {
        public ScheduleForDay FindScheduleToCoverUnit(TimeUnitsManager timeUnitsManager, TimeUnit timeUnit)
        {
            var dayId = timeUnit.DayId;
            ScheduleForDay schedule;

            // Take a person who has any assignableSchedules for the day. Take into account person's CurrentWorkLeft and ShiftWeight
            var personsWithTimeUnit = timeUnitsManager.ScheduledPersons
                .Where(scheduledPerson => scheduledPerson.AssignableSchedulesForDays.ContainsKey(dayId))
                .MinBy(scheduledPerson => -(scheduledPerson.CurrentWorkLeft * scheduledPerson.CurrentWorkLeft) * scheduledPerson.ShiftWeights[timeUnit.DayId])
                .First();

            // If the person has already been assigned with a schedule
            if (personsWithTimeUnit.AssignedDays.ContainsKey(dayId))
            {
                // Take a schedule which contains the assigned one, the timeUnit and is the shortest
                schedule = personsWithTimeUnit.AssignableSchedulesForDays[dayId].Schedules
                    .Where(s => s.Intervals.ContainsSubInterval(personsWithTimeUnit.AssignedDays[dayId].Intervals.First())
                                    && s.Intervals.ContainsSubInterval(personsWithTimeUnit.AssignedDays[dayId].Intervals.Last())
                                        && s.Intervals.ContainsSubInterval(new Interval(timeUnit.UnitOfDay, timeUnit.UnitOfDay)))
                    .MinBy(s => s.GetTotalWork())
                    .First();
            }
            else
            {
                // Take a schedule which contains the timeUnit and is the shortest
                schedule = personsWithTimeUnit.AssignableSchedulesForDays[dayId].Schedules
                    .Where(s => s.GetTotalWork() == 1 
                                    && s.Intervals.IntervalsList.First().Contains(timeUnit.UnitOfDay))
                    .First();
            }

            return schedule;
        }
    }
}
