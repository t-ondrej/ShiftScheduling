using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal static class AlgorithmFactory
    {
        private static readonly IDictionary<string, Type> ProvidersDictionary = new Dictionary<string, Type>
        {
            {"TimeUnitAlgorithm", typeof(TimeUnitStrategyProvider)}
        };

        public static AlgorithmStrategy ParseStrategy(string s)
        {
            if (s == null)
                return null;

            var split = s.Split(' ');

            if (split.Length == 0)
                return null;

            var className = split[0];

            if (!ProvidersDictionary.ContainsKey(className))
                return null;

            var otherArgs = split.Skip(1).ToArray();
            var type = ProvidersDictionary[className];
            var algorithmProvider = Activator.CreateInstance(type) as IAlgorithmStrategyProvider;

            return algorithmProvider?.GetAlgorithm(otherArgs);
        }

        public static ResultingSchedule ExecuteAlgorithm(AlgorithmInput algorithmInput)
        {
            if (algorithmInput == null)
                throw new ArgumentNullException(nameof(algorithmInput));

            var strategy = algorithmInput.AlgorithmConfiguration.AlgorithmStrategy;
            Algorithm algorithm = null;

            if (strategy is TimeUnitStrategy timeUnitStrategy)
            {
                algorithm = new TimeUnitAlgorithm(algorithmInput, timeUnitStrategy);
            }
            // Here will come the other strategies if we code any

            if (algorithm == null)
                throw new NotImplementedException();

            return algorithm.CreateScheduleForPeople();
        }
    }
}