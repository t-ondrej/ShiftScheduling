using System.Collections.Generic;
using ShiftScheduleData.Entities.NewEntities.Helpers;

namespace ShiftScheduleData.Entities
{
    class ShiftSchedules
    {
        public class PersonsSchedule
        {
            public IDictionary<int, Interval> IntervalsForDays { get; }
        }

        public IDictionary<PersonOld, PersonsSchedule> SchedulesForPeople { get; }

        public ShiftSchedules(IDictionary<PersonOld, PersonsSchedule> schedulesForPeople)
        {
            SchedulesForPeople = schedulesForPeople;
        }
    }
}