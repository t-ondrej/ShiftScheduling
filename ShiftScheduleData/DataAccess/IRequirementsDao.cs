using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IRequirementsDao
    {
        Requirements GetRequirements();

        void SaveRequirements(Requirements requirements);
    }
}
