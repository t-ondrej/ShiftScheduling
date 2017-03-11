using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess
{
    public interface IResultingScheduleDao
    {
        ResultingSchedule GetResultingSchedule();

        void SaveResultingSchedule(ResultingSchedule resultingScheduleOld);
    }
}
