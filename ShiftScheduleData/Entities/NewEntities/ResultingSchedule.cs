using System.Collections.Generic;
using ShiftScheduleData.Entities.NewEntities.Helpers;

namespace ShiftScheduleData.Entities.NewEntities
{
    public class ResultingSchedule
    {
        public ResultingSchedule(IDictionary<int, DailySchedule> dailySchedules)
        {
            DailySchedules = dailySchedules;
        }

        public IDictionary<int, DailySchedule> DailySchedules { get; }

        public class DailySchedule
        {
            public IDictionary<Person, IEnumerable<ShiftInterval>> DailySchedules { get; }

            public DailySchedule(IDictionary<Person, IEnumerable<ShiftInterval>> dailySchedules)
            {
                DailySchedules = dailySchedules;
            }
        }

        public class ShiftInterval : Interval
        {
            public enum IntervalType
            {
                Pause,
                Work
            }

            public IntervalType Type { get; }

            public ShiftInterval(int start, int end, IntervalType type) : base(start, end)
            {
                Type = type;
            }
        }
    }
}