using System;
using ShiftScheduleData.DataAccess;
using ShiftScheduleData.DataAccess.FileDao;

namespace ShiftScheduleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var personDao = new FilePersonDao(Utilities.GetPathFromRelativeProjectPath("Data/Set1"));

            foreach (var person in personDao.GetAllPersons())
            {
                Console.WriteLine(person.Id);
            }
        }
    }
}
