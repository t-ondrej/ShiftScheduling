using System;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal static class AlgorithmProvider
    {
        public static AlgorithmStrategy ParseStrategy(string s)
        {
            if (s == null)
                return null;

            var split = s.Split(' ');

            if (split.Length == 0)
                return null;

            switch (split[0])
            {
                case "TimeUnitAlgorithm":
                    return ParseTimeUnitStrategy(split.Skip(1).ToArray());
                default:
                    throw new Exception("Unknown algorithm");
            }
        }

        private static AlgorithmStrategy ParseTimeUnitStrategy(string[] array)
        {
            if (array.Length != 3)
                return null;

            ITimeUnitChooser timeUnitChooser;

            switch (array[0])
            {
                case "RandomTimeUnitChooser":
                    timeUnitChooser = new RandomTimeUnitChooser();
                    break;
                default:
                    throw new Exception("Unknown time unit chooser");
            }

            IScheduleChooser scheduleChooser;

            switch (array[1])
            {
                case "RandomScheduleChooser":
                    scheduleChooser = new RandomScheduleChooser();
                    break;
                default:
                    throw new Exception("Unknown schedule chooser.");
            }

            IRemainingPeopleChooser remainingPeopleChooser;

            switch (array[2])
            {
                case "RandomRemainingPeopleChooser":
                    remainingPeopleChooser = new RandomRemainingPeopleChooser();
                    break;
                default:
                    throw new Exception("Unknown random remaining people chooser.");
            }
            
            return new TimeUnitStrategy(timeUnitChooser, scheduleChooser, remainingPeopleChooser);
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