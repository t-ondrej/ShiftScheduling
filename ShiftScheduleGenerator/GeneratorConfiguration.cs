namespace ShiftScheduleGenerator
{
    internal class GeneratorConfiguration
    {
        public int ScheduleDaysCount { get; }

        public int WorkingTimeLength { get; }

        public int WorkingTimePerMonthMax { get; }

        public int EmployeeCount { get; }

        public string FolderName { get; }

        public int NumberOfSets { get; }

        public GeneratorConfiguration(int scheduleDaysCount, int workingTimeLength, int workingTimePerMonthMax,
            int employeeCount, string folderName, int numberOfSets)
        {
            ScheduleDaysCount = scheduleDaysCount;
            WorkingTimeLength = workingTimeLength;
            WorkingTimePerMonthMax = workingTimePerMonthMax;
            EmployeeCount = employeeCount;
            FolderName = folderName;
            NumberOfSets = numberOfSets;
        }
    }
}