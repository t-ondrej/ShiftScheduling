using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao
{
    public interface IRequirementsDao
    {
        Requirements GetRequirements();

        void SaveRequirements(Requirements requirements);
    }
}
