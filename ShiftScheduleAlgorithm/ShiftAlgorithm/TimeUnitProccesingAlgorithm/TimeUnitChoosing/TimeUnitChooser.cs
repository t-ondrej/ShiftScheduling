using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.TimeUnitChoosing
{
    internal abstract class TimeUnitChooser
    {
        public TimeUnitsManager TimeUnitsManager { get; }

        protected TimeUnitChooser(TimeUnitsManager timeUnitsManager)
        {
            TimeUnitsManager = timeUnitsManager;
        }

        public abstract TimeUnit FindTimeUnitToBeProccessed();
    }
}
