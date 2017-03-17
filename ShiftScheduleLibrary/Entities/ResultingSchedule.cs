using System.Collections.Generic;
using ShiftScheduleLibrary.Utilities;

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
        }
}