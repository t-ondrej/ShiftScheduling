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
            var workLeft = timeUnit.RequiredWorkAmount - timeUnit.SumOfCurrentWorkAmount;
            ScheduleForDay schedule;

            // Take a person who has any assignable schedules for that day and isn't already scheduled for the TimeUnit
            // Take into account person's CurrentWorkForMonth and ShiftWeight
            var person = timeUnitsManager.ScheduledPersons
                  .Where(p => !(p.AssignedDays.ContainsKey(dayId) && p.AssignedDays[dayId].Intervals.ContainsSubInterval(new Interval(timeUnit.UnitOfDay, timeUnit.UnitOfDay))) 
                          && p.AssignableSchedulesForDays.ContainsKey(dayId)
                              && p.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(timeUnit.UnitOfDay).Count() > 0)
                  .OrderBy(p => (p.CurrentWorkForMonth * p.CurrentWorkForMonth) * p.ShiftWeights[timeUnit.DayId])
                  .FirstOrDefault();

            // Take a schedule which contains the TimeUnit and is the shortest
            schedule = person.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(timeUnit.UnitOfDay)
                .OrderBy(s => s.GetTotalWork())
                .FirstOrDefault();

            return schedule;
        }
    }
}
