using System;
using System.Collections.Generic;
using System.Diagnostics;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations;
using ShiftScheduleUtilities;
using static ShiftScheduleUtilities.SingletonFactory;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal class TimeUnitStrategyProvider : IAlgorithmStrategyProvider
    {
        private static readonly IDictionary<string, Type> ScheduleChoosers = new Dictionary<string, Type>
        {
            {"RandomScheduleChooser", typeof(RandomScheduleChooser)}
        };

        private static readonly IDictionary<string, Type> TimeUnitChoosers = new Dictionary<string, Type>
        {
            {"RandomTimeUnitChooser", typeof(RandomTimeUnitChooser)}
        };

        private static readonly IDictionary<string, Type> RemainingPeopleChoosers = new Dictionary<string, Type>
        {
            {"RandomRemainingPeopleChooser", typeof(RandomRemainingPeopleChooser)}
        };

        public AlgorithmStrategy GetAlgorithm(string[] classNames)
        {
            Debug.Assert(classNames != null);

            if (classNames.Length != 3)
                return null;

            var timeUnitChooserType = TimeUnitChoosers.GetIfExists(classNames[0]);
            var scheduleChooserType = ScheduleChoosers.GetIfExists(classNames[1]);
            var remaningPeopleChooserType = RemainingPeopleChoosers.GetIfExists(classNames[2]);

            if (scheduleChooserType == null || timeUnitChooserType == null || remaningPeopleChooserType == null)
                return null;

            var scheduleChooser = GetInstance(scheduleChooserType) as IScheduleChooser;
            var timeUnitChooser = GetInstance(timeUnitChooserType) as ITimeUnitChooser;
            var remainingPeopleChooser = GetInstance(remaningPeopleChooserType) as IRemainingPeopleChooser;

            if (scheduleChooser == null || timeUnitChooser == null || remainingPeopleChooser == null)
                return null;

            return new TimeUnitStrategy(timeUnitChooser, scheduleChooser, remainingPeopleChooser);
        }
    }
}