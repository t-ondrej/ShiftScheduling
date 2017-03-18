using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal interface IRemainingPeopleChooser
    {
        void AssignScheduleToRemainingPeople(TimeUnitsManager timeUnitsManager);
    }
}
