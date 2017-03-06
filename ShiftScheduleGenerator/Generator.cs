using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Helpers;
using ShiftScheduleData.Entities;
using static ShiftScheduleGenerator.GeneratorConfiguration;

namespace ShiftScheduleGenerator
{
    internal class Generator
    {
        internal static void GenerateData()
        {
            var fileRequirementsDao = new FileRequirementsDao($"..\\..\\{FolderName}");
            var filePersonDao = new FilePersonDao($"..\\..\\{FolderName}");

            var scheduleGenerator = new ScheduleGenerator();
            var requirementsGenerator = new RequirementsGenerator();

            var persons = scheduleGenerator.GeneratePersons();
            var requirements = requirementsGenerator.GenerateRequirements(persons);

            foreach (var person in persons)
            {
                filePersonDao.SavePerson(person);
            }

            fileRequirementsDao.SaveRequirements(requirements);
        } 
    }
}
