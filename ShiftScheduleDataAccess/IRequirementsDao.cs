using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess
{
    public interface IRequirementsDao
    {
        Requirements GetRequirements();

        void SaveRequirements(Requirements requirements);
    }
}
