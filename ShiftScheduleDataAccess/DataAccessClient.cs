using System.IO;
using ShiftScheduleDataAccess.FileDao;

namespace ShiftScheduleDataAccess
{
    public class DataAccessClient
    {
        public string WorkingFolder { get; }

        public string OutputFolder { get; }

        public DataAccessClient(string workingFolder)
        {
            WorkingFolder = workingFolder;
            OutputFolder = Path.Combine(WorkingFolder, FolderConstants.OutputFolderName);
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
            return new FileResultingScheduleDao(OutputFolder);
        }
    }
}