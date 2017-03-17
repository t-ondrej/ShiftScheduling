using System.Collections.Generic;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleLibrary.Entities
{
    public class ResultingSchedule
    {
        public IDictionary<int, DailySchedule> DailySchedules { get; }

        public string Specification { get; set; }

        public ResultingSchedule(IDictionary<int, DailySchedule> dailySchedules, string specification)
        {
            DailySchedules = dailySchedules;
            Specification = specification;
        }

        public ResultingSchedule(IDictionary<int, DailySchedule> dailySchedules) : this(dailySchedules, null)
        {
        }

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