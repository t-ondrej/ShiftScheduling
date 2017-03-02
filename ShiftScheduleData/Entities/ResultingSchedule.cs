using System.Collections.Generic;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.Entities
{
    public class ResultingSchedule
    {
        public IDictionary<Person, Schedule> SchedulesForPeople { get; }

        public ResultingSchedule(IDictionary<Person, Schedule> schedulesForPeople)
        {
            SchedulesForPeople = schedulesForPeople;
        }
    }
}
