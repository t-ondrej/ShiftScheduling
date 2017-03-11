using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess
{
    public interface IRequirementsFulfillingStatsDao
    {
        RequirementsFulfillingStats GetRequirements();

        void SaveRequirements(RequirementsFulfillingStats requirements);
    }
}
