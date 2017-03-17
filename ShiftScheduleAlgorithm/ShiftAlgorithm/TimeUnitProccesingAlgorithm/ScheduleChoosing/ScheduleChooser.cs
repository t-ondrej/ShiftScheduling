using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.ScheduleChoosing
{
    internal abstract class ScheduleChooser
    {
        public TimeUnitsManager TimeUnitsManager { get; }

        protected ScheduleChooser(TimeUnitsManager timeUnitsManager)
        {
            TimeUnitsManager = timeUnitsManager;
        }

        public abstract ScheduleForDay FindScheduleToCoverUnit(TimeUnit timeUnit);
    }
}
