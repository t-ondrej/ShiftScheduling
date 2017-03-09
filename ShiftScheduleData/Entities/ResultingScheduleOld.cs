using System.Collections.Generic;

namespace ShiftScheduleData.Entities
{
    public class ResultingScheduleOld
    {
        public IDictionary<PersonOld, Schedule> SchedulesForPeople { get; }

        public ResultingScheduleOld(IDictionary<PersonOld, Schedule> schedulesForPeople)
        {
            SchedulesForPeople = schedulesForPeople;
        }
    }
}