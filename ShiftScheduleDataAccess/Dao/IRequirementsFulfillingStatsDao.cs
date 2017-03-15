using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao
{
    public interface IRequirementsFulfillingStatsDao
    {
        RequirementsFulfillingStats GetRequirements();

        void SaveRequirements(RequirementsFulfillingStats requirements);
    }
}
