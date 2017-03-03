using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IResultingScheduleDao
    {
        ResultingSchedule GetResultingSchedule(IEnumerable<Person> persons );

        void SaveResultingSchedule(ResultingSchedule resultingSchedule);
    }
}
