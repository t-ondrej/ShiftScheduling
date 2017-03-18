using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    internal class RandomScheduleChooser : IScheduleChooser
    {
        public ScheduleForDay FindScheduleToCoverUnit(TimeUnitsManager timeUnitsManager, TimeUnit timeUnit)
        {
            var dayId = timeUnit.DayId;
            var unitId = timeUnit.UnitOfDay;

            return
            (
                from scheduledPerson in timeUnitsManager.ScheduledPersons
                select scheduledPerson.AssignableSchedulesForDays
                into assignableSchedules
                where assignableSchedules.ContainsKey(dayId)
                select assignableSchedules[dayId].GetSchedulesThatCoverTimeUnit(unitId).ToList()
                into possibleUnits
                where possibleUnits.Any()
                select possibleUnits.MinBy(schedule => -schedule.Intervals.GetLengthInTime()).First()
            ).FirstOrDefault();
        }
    }
}