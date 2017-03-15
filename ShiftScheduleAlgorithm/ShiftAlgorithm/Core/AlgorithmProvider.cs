using System;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal static class AlgorithmProvider
    {
        public enum Strategy
        {
            Test
        }

        public static ResultingSchedule ExecuteAlgorithm(AlgorithmInput algorithmInput, Strategy strategy)
        {
            Algorithm algorithm;

            switch (strategy)
            {
                case Strategy.Test:
                    algorithm = new TestAlgorithm(algorithmInput);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return algorithm.CreateScheduleForPeople();
        }
    }
}