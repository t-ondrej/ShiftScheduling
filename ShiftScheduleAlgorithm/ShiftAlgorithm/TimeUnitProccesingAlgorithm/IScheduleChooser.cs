using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal interface IScheduleChooser
    {
        ScheduleForDay FindScheduleToCoverUnit(TimeUnitsManager timeUnitsManager, TimeUnit timeUnit);
    }
}
