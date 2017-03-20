using System.Collections.Generic;
using System.Linq;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class SchedulesForDay
    {
        public ScheduledPerson ScheduledPerson { get; }

        public int DayId { get; }

        public List<ScheduleForDay> Schedules { get; }

        public SchedulesForDay(ScheduledPerson scheduledPerson, int dayId, List<ScheduleForDay> schedules)
        {
            ScheduledPerson = scheduledPerson;
            DayId = dayId;
            Schedules = schedules;
        }

        public IEnumerable<ScheduleForDay> GetSchedulesThatCoverTimeUnit(int timeUnit)
        {
            return Schedules.Where(schedule => schedule.Intervals.Any(interval =>
                interval.Contains(timeUnit) && interval.Type == ShiftInterval.IntervalType.Work));
        }
    }
}