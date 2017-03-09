using System.Collections.Generic;
using ShiftScheduleData.Entities.Helpers;

namespace ShiftScheduleData.Entities
{
    public class Schedule
    {
        public IDictionary<int, IntervalsOld> DailySchedules { get; }

        public Schedule(IDictionary<int, IntervalsOld> dailySchedules)
        {
            DailySchedules = dailySchedules;
        }
    }
}
