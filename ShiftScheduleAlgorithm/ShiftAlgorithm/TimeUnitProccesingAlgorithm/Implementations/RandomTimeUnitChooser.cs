using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    internal class RandomTimeUnitChooser : ITimeUnitChooser
    {
        public TimeUnit FindTimeUnitToBeProccessed(TimeUnitsManager timeUnitsManager)
        {
            return timeUnitsManager.AllTimeUnits.Find(unit => !unit.Fulfilled && unit.Fulfillable);
        }
    }
}