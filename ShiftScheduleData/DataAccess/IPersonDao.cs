using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IPersonDao
    {
        IEnumerable<PersonOld> GetAllPersons();

        void SavePerson(PersonOld personOld);
    }
}
