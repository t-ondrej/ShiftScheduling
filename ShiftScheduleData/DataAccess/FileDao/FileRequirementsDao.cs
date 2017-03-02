using System;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FileRequirementsDao : FileClient, IRequirementsDao
    {
        public FileRequirementsDao(string folderPath) : base(folderPath)
        {
        }

        public Requirements GetRequirements()
        {
            throw new NotImplementedException();
        }

        public void SaveRequirements(Requirements requirements)
        {
            throw new NotImplementedException();
        }
    }
}
