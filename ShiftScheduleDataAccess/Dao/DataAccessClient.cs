using System.IO;
using ShiftScheduleDataAccess.Dao.FileDao;

namespace ShiftScheduleDataAccess.Dao
{
    public class DataAccessClient
    {
        public string WorkingFolder { get; }

        public string OutputFolder { get; }

        public IPersonDao PersonDao { get; }

        public IRequirementsDao RequirementsDao { get; }

        public IRequirementsFulfillingStatsDao RequirementsFulfillingStatsDao { get; }

        public IResultingScheduleDao ResultingScheduleDao { get; }

        public DataAccessClient(string workingFolder)
        {
            WorkingFolder = workingFolder;
            OutputFolder = Path.Combine(WorkingFolder, FolderConstants.OutputFolderName);
            PersonDao = new FilePersonDao(WorkingFolder);
            RequirementsDao = new FileRequirementsDao(WorkingFolder);
            RequirementsFulfillingStatsDao = new FileRequirementsFulfillingStatsDao(WorkingFolder);
            ResultingScheduleDao = new FileResultingScheduleDao(OutputFolder);
        }

        public void InitializeWorkingFolder()
        {
            Directory.CreateDirectory(WorkingFolder);
            Directory.CreateDirectory(OutputFolder);
        }
    }
}