using System.Collections.Generic;
using ShiftScheduleLibrary.Utilities;
using ShiftScheduleUtilities;

namespace ShiftScheduleLibrary.Entities
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
            public IDictionary<int, Intervals<ShiftInterval>> PersonIdToDailySchedule { get; }

            public DailySchedule(IDictionary<int, Intervals<ShiftInterval>> personIdToDailySchedule)
            {
                PersonIdToDailySchedule = personIdToDailySchedule;
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

            public override string ToString()
            {
                return $"{base.ToString()}={Type}";
            }

            public new static ShiftInterval FromString(string s)
            {
                var splited = s.Split(' ');
                var interval = Interval.FromString(s);
                var type = EnumUtilities.ParseEnum<IntervalType>(splited[1]);
                return new ShiftInterval(interval.Start, interval.End, type);
            }
        }
    }
}