using System.Collections.Generic;

namespace ShiftScheduleData.Helpers
{
    public class MonthlySchedule
    {
        public IDictionary<int, Intervals> DailySchedules { get; }

        public MonthlySchedule(IDictionary<int, Intervals> dailySchedules)
        {
            DailySchedules = dailySchedules;
        }
    }
}