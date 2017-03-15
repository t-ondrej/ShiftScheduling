using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao
{
    public interface IPersonDao
    {
        IEnumerable<Person> GetAllPersons();

        void SavePerson(Person personOld);
    }
}
