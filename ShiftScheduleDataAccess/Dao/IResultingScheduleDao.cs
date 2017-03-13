using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao
{
    public interface IResultingScheduleDao
    {
        ResultingSchedule GetResultingSchedule();

        void SaveResultingSchedule(ResultingSchedule resultingScheduleOld);
    }
}
