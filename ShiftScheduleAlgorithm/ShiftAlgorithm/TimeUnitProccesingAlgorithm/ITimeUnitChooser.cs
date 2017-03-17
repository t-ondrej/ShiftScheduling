using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal interface ITimeUnitChooser
    {
        TimeUnit FindTimeUnitToBeProccessed(TimeUnitsManager timeUnitsManager);
    }
}
