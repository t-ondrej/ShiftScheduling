using System;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal static class AlgorithmProvider
    {
        public enum Strategy
        {
            TimeUnitStrategy
        }

        public static ResultingSchedule ExecuteAlgorithm(AlgorithmInput algorithmInput, Strategy strategy)
        {
            Algorithm algorithm;

            switch (strategy)
            {
                case Strategy.TimeUnitStrategy:
                    algorithm = new TimeUnitAlgorithm(algorithmInput, null, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return algorithm.CreateScheduleForPeople();
        }
    }
}