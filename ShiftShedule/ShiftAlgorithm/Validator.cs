using System;
using ShiftScheduleData.Entities;

namespace ShiftSheduleAlgorithm.ShiftAlgorithm
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
            throw new NotImplementedException();
        }
    }
}