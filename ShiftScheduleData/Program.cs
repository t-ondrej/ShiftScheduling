using System;
using System.Linq;
using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;
// ReSharper disable PossibleMultipleEnumeration

namespace ShiftScheduleData
{
    class Program
    {
        private const string ReadSetName = "Data/Set1";
        private const string PutSetName = "Data/Set1/Output";

        private static void Main()
        {
            Console.WriteLine("Person dao test:\n");
            PersonDaoTest();
            Console.WriteLine("Requirements dao test:\n");
            RequirementsDaoTest();
            Console.WriteLine("\nResulting schedule dao test:\n");
            ResultingScheduleDaoTest();
        }

        private static void RequirementsDaoTest()
        {
            var requirementsDaoRead = new FileRequirementsDao(Utilities.GetPathFromRelativeProjectPath(ReadSetName));
            var requirementsDaoPut = new FileRequirementsDao(Utilities.GetPathFromRelativeProjectPath(PutSetName));
            var requirements = requirementsDaoRead.GetRequirements();

            Printers.PrintRequirements(requirements, Console.Out);
            requirementsDaoPut.SaveRequirements(requirements);
        }

        private static void PersonDaoTest()
        {
            var personDaoRead = new FilePersonDao(Utilities.GetPathFromRelativeProjectPath(ReadSetName));
            var personDaoPut = new FilePersonDao(Utilities.GetPathFromRelativeProjectPath(PutSetName));
            var persons = personDaoRead.GetAllPersons();

            foreach (var person in persons)
            {
                Printers.PrintPerson(person, Console.Out);
                Console.WriteLine();
            }

            foreach (var person in persons)
            {
                personDaoPut.SavePerson(person);
            }
        }

        private static void ResultingScheduleDaoTest()
        {
            var personDaoRead = new FilePersonDao(Utilities.GetPathFromRelativeProjectPath(ReadSetName));
            var persons = personDaoRead.GetAllPersons();
            var dictionary = persons.ToDictionary(p => p, p => p.MonthlySchedule);
            var resultingSchedule = new ResultingSchedule(dictionary);
            var resultingScheduleDao = new FileResultingScheduleDao(Utilities.GetPathFromRelativeProjectPath(ReadSetName));
            resultingScheduleDao.SaveResultingSchedule(resultingSchedule);

            var reloadedSchedule = resultingScheduleDao.GetResultingSchedule(persons);
            var resultingSchedulePut = new FileResultingScheduleDao(Utilities.GetPathFromRelativeProjectPath(PutSetName));
            resultingSchedulePut.SaveResultingSchedule(reloadedSchedule);

            Printers.PrintResultingschedule(resultingSchedule, Console.Out);
        }
    }
}