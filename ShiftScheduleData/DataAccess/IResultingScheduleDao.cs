using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IResultingScheduleDao
    {
        ResultingSchedule GetResultingSchedule();

        void SaveResultingSchedule(ResultingSchedule resultingSchedule);
    }
}
