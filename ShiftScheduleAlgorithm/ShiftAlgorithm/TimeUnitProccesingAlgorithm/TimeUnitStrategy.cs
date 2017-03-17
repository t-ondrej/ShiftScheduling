using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal class TimeUnitStrategy : AlgorithmStrategy
    {
        public ITimeUnitChooser TimeUnitChooser { get; }

        public IScheduleChooser ScheduleChooser { get; }

        public IRemeaningPeopleChooser RemeaningPeopleChooser { get; }

        public TimeUnitStrategy(ITimeUnitChooser timeUnitChooser, IScheduleChooser scheduleChooser,
            IRemeaningPeopleChooser remeaningPeopleChooser)
        {
            TimeUnitChooser = timeUnitChooser;
            ScheduleChooser = scheduleChooser;
            RemeaningPeopleChooser = remeaningPeopleChooser;
        }
    }
}