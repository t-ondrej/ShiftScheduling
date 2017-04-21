using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    class ProductiveScheduleChooser : IScheduleChooser
    {
        public ScheduleForDay FindScheduleToCoverUnit(TimeUnitsManager timeUnitsManager, TimeUnit timeUnit)
        {
            var dayId = timeUnit.DayId;
            ScheduleForDay schedule;

            // Take a person who has any assignableSchedules for the day. Take into account person's CurrentWorkLeft and ShiftWeight
            var person = timeUnitsManager.ScheduledPersons
                .Where(p => p.AssignableSchedulesForDays.ContainsKey(dayId)
                        && p.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(timeUnit.UnitOfDay).Count() > 0)
                .OrderBy(p => -(p.CurrentWorkLeft * p.CurrentWorkLeft) * p.ShiftWeights[timeUnit.DayId])
                .FirstOrDefault();

            // Take a schedule which contains the timeUnit and is the shortest
            schedule = person.AssignableSchedulesForDays[dayId].GetSchedulesThatCoverTimeUnit(timeUnit.UnitOfDay)
                .OrderBy(s => s.GetTotalWork())
                .FirstOrDefault();

            return schedule;
        }
    }
}
