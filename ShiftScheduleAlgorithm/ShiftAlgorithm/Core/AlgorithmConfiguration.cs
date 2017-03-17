namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    public class AlgorithmConfiguration
    {
        public AlgorithmStrategy AlgorithmStrategy { get; set; }

        public int MaxDailyWork { get; set; }

        public int WorkerPauseLength { get; set; }

        public int MaxConsecutiveWorkHours { get; set; }
    }
}