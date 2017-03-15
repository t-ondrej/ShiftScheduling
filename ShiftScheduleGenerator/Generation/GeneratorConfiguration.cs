namespace ShiftScheduleGenerator.Generation
{
    internal class GeneratorConfiguration
    {
        public int ScheduleDaysCount { get; set; }

        public double DayAssignmentDensity { get; set; }

        public int WorkingTimePerMonthMin { get; set; }

        public int WorkingTimePerMonthMax { get; set; }

        public int WorkingTimePerDay { get; set; }

        public int NumberOfShiftWeightValues { get; set; }

        public int EmployeeCount { get; set; }

        public int NumberOfSets { get; set; }

        public double DifficultyToFulfilRequirements { get; set; }
    }
}