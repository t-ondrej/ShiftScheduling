using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal class TimeUnitStrategy : AlgorithmStrategy
    {
        public ITimeUnitChooser TimeUnitChooser { get; }

        public IScheduleChooser ScheduleChooser { get; }

        public IRemainingPeopleChooser RemainingPeopleChooser { get; }

        public TimeUnitStrategy(ITimeUnitChooser timeUnitChooser, IScheduleChooser scheduleChooser,
            IRemainingPeopleChooser remainingPeopleChooser)
        {
            TimeUnitChooser = timeUnitChooser;
            ScheduleChooser = scheduleChooser;
            RemainingPeopleChooser = remainingPeopleChooser;
        }
    }
}