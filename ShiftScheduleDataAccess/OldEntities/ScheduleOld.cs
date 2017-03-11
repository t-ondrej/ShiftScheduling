using System.Collections.Generic;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleDataAccess.OldEntities
{
    public class ScheduleOld
    {
        public IDictionary<int, Intervals<Interval>> DailySchedules { get; }

        public ScheduleOld(IDictionary<int, Intervals<Interval>> dailySchedules)
        {
            DailySchedules = dailySchedules;
        }
    }
}
