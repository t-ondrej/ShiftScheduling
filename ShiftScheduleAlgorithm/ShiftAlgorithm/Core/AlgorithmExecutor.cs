using System;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal static class AlgorithmExecutor
    {
        public static ResultingSchedule ExecuteAlgorithm(AlgorithmInput algorithmInput)
        {
            if (algorithmInput == null)
                throw new ArgumentNullException(nameof(algorithmInput));

            var strategy = algorithmInput.AlgorithmConfiguration.AlgorithmStrategy;
            var algorithm = Activator.CreateInstance(strategy.GetAlgorithmType(), algorithmInput) as Algorithm;

            if (algorithm == null)
                throw new NotImplementedException();

            return algorithm.CreateScheduleForPeople();
        }
    }
}