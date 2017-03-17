using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal interface IRemeaningPeopleChooser
    {
        void AssignScheduleToRemainingPeople(TimeUnitsManager timeUnitsManager);
    }
}
