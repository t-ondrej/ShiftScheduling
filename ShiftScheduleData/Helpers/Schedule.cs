using System.Collections.Generic;

namespace ShiftScheduleData.Helpers
{
    public class Schedule
    {
        public IDictionary<int, Intervals> DailySchedules { get; }

        public Schedule(IDictionary<int, Intervals> dailySchedules)
        {
            DailySchedules = dailySchedules;
        }
    }
}