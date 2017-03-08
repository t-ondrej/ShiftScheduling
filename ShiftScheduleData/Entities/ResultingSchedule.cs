using System.Collections.Generic;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.Entities
{
    public class ResultingSchedule
    {
        public IDictionary<Person, MonthlySchedule> SchedulesForPeople { get; }

        public ResultingSchedule(IDictionary<Person, MonthlySchedule> schedulesForPeople)
        {
            SchedulesForPeople = schedulesForPeople;
        }
    }
}
