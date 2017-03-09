using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IResultingScheduleDao
    {
        ResultingScheduleOld GetResultingSchedule(IEnumerable<PersonOld> persons );

        void SaveResultingSchedule(ResultingScheduleOld resultingScheduleOld);
    }
}
