using System.Collections.Generic;

namespace ShiftScheduleDataAccess.OldEntities
{
    public class ResultingScheduleOld
    {
        public IDictionary<PersonOld, ScheduleOld> SchedulesForPeople { get; }

        public ResultingScheduleOld(IDictionary<PersonOld, ScheduleOld> schedulesForPeople)
        {
            SchedulesForPeople = schedulesForPeople;
        }
    }
}