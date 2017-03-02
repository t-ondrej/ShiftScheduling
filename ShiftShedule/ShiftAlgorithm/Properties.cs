namespace ShiftShedule.ShiftAlgorithm
{
    internal class Properties
    {
        public int MaxDailyWork { get; set; } = 12;

        public int WorkerPauseLength { get; set; } = 30;

        public int MaxConsecutiveWorkHours { get; set; } = 4;
    }
}
