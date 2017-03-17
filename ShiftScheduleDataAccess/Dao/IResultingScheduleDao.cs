using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao
{
    public interface IResultingScheduleDao
    {
        IEnumerable<ResultingSchedule> GetResultingSchedules();

        void SaveResultingSchedule(ResultingSchedule resultingSchedule);
    }
}
