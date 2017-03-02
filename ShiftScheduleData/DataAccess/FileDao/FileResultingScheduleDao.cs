using System;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FileResultingScheduleDao : FileClient, IResultingScheduleDao
    {
        public FileResultingScheduleDao(string folderPath) : base(folderPath)
        {
        }

        public ResultingSchedule GetResultingSchedule()
        {
            throw new NotImplementedException();
        }

        public void SaveResultingSchedule(ResultingSchedule resultingSchedule)
        {
            throw new NotImplementedException();
        }
    }
}
