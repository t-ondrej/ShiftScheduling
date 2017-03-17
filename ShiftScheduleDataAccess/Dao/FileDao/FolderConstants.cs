namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal static class FolderConstants
    {
        public const string Extension = ".txt";
        public const string RequirementsFileName = "requirements" + Extension;
        public const string ResultingSchedulePreffix = "result_";
        public const string ResultingScheduleFilesPattern = ResultingSchedulePreffix + "*" + Extension;
        public const string RequirementsFulfillingStatsFileName = "statistics" + Extension;
        public const string OutputFolderName = "Output";
    }
}