using ShiftScheduleData.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm
{
    internal class Validator
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public ResultingSchedule Schedule { get; }

        public Validator(ShiftAlgorithm.Input algorithmInput, ResultingSchedule schedule)
        {
            AlgorithmInput = algorithmInput;
            Schedule = schedule;
        }

        public bool Validate()
        {
            return true;
        }
    }
}