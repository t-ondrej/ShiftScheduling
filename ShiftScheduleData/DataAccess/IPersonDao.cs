using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess
{
    public interface IPersonDao
    {
        IEnumerable<Person> GetAllPersons();

        void SavePerson(Person person);
    }
}
