using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IRequirementsDao
    {
        MonthlyRequirements GetRequirements();

        void SaveRequirements(MonthlyRequirements monthlyRequirements);
    }
}
