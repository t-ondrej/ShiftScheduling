using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal abstract class Algorithm
    {
        public AlgorithmInput AlgorithmInput { get; }

        protected Algorithm(AlgorithmInput algorithmInput)
        {
            AlgorithmInput = algorithmInput;
        }

        public abstract ResultingSchedule CreateScheduleForPeople();
    }
}
