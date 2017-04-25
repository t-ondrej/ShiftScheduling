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
            ScheduleForDay schedule;
            var dayId = timeUnit.DayId;
            var unitOfDay = timeUnit.UnitOfDay;

            // Take a person who has any assignable schedules for that day and isn't already scheduled for the TimeUnit
            // Take into account person's CurrentWorkForMonth and ShiftWeight
            var person = timeUnitsManager.ScheduledPersons
                  .Where(p => !(p.AssignedDays.ContainsKey(dayId) && p.AssignedDays[dayId].Intervals.Any(interval => interval.Contains(unitOfDay))) 
                          && p.AssignableSchedulesForDays.ContainsKey(dayId)
                              && p.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(unitOfDay).Any())
                  .OrderBy(p => (p.CurrentWorkForMonth * p.CurrentWorkForMonth) * p.ShiftWeights[dayId])
                  .FirstOrDefault();

            if (person == null)
                return null;

            // Take a schedule which contains the TimeUnit and is the shortest
            schedule = person.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(unitOfDay)
                .OrderBy(s => s.GetTotalWork())
                .FirstOrDefault();

            return schedule;
        }
    }
}
