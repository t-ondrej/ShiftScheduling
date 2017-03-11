using System.IO;
using ShiftScheduleDataAccess.FileDao;

namespace ShiftScheduleDataAccess
{
    public class DataAccessClient
    {
        public string WorkingFolder { get; }

        public DataAccessClient(string workingFolder)
        {
            WorkingFolder = workingFolder;
        }

        public IPersonDao GetPersonDao()
        {
            return new FilePersonDao(WorkingFolder);
        }

        public IRequirementsDao GetRequirementsDao()
        {
            return new FileRequirementsDao(WorkingFolder);
        }

        public IRequirementsFulfillingStatsDao GetRequirementsFulfillingStatsDao()
        {
            return new FileRequirementsFulfillingStatsDao(WorkingFolder);
        }

        public IResultingScheduleDao GetResultingScheduleDao()
        {
            var resultPath = Path.Combine(WorkingFolder, FolderConstants.OutputFolderName);
            return new FileResultingScheduleDao(resultPath);
        }
    }
}