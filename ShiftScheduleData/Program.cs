using System;
using System.Linq;
using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Entities.Helpers;
using static ShiftScheduleUtilities.PathUtilities;

namespace ShiftScheduleData
{
    internal class Program
    {
        private const string ReadSetName = "TestData";
        private const string PutSetName = "TestData/Output";

        private static void Main()
        {
            Console.WriteLine("PersonOld dao test:\n");
            PersonDaoTest();
            Console.WriteLine("MonthlyRequirements dao test:\n");
            RequirementsDaoTest();
            Console.WriteLine("\nResulting schedule dao test:\n");
            ResultingScheduleDaoTest();
        }

        private static void RequirementsDaoTest()
        {
            var requirementsDaoRead = new FileRequirementsDao(GetPathFromRelativeProjectPath(ReadSetName));
            var requirementsDaoPut = new FileRequirementsDao(GetPathFromRelativeProjectPath(PutSetName));
            var requirements = requirementsDaoRead.GetRequirements();

            EntitiesPrinter.PrintRequirements(requirements, Console.Out);
            requirementsDaoPut.SaveRequirements(requirements);
        }

        private static void PersonDaoTest()
        {
            var personDaoRead = new FilePersonDao(GetPathFromRelativeProjectPath(ReadSetName));
            var personDaoPut = new FilePersonDao(GetPathFromRelativeProjectPath(PutSetName));
            var persons = personDaoRead.GetAllPersons().ToList();

            foreach (var person in persons)
            {
                EntitiesPrinter.PrintPerson(person, Console.Out);
                Console.WriteLine();
            }

            foreach (var person in persons)
            {
                personDaoPut.SavePerson(person);
            }
        }

        private static void ResultingScheduleDaoTest()
        {
            var personDaoRead = new FilePersonDao(GetPathFromRelativeProjectPath(ReadSetName));
            var persons = personDaoRead.GetAllPersons().ToList();
            var dictionary = persons.ToDictionary(p => p, p => p.Schedule);
            var resultingSchedule = new ResultingScheduleOld(dictionary);
            var resultingScheduleDao = new FileResultingScheduleDao(GetPathFromRelativeProjectPath(ReadSetName));
            resultingScheduleDao.SaveResultingSchedule(resultingSchedule);

            var reloadedSchedule = resultingScheduleDao.GetResultingSchedule(persons);
            var resultingSchedulePut = new FileResultingScheduleDao(GetPathFromRelativeProjectPath(PutSetName));
            resultingSchedulePut.SaveResultingSchedule(reloadedSchedule);

            EntitiesPrinter.PrintResultingschedule(resultingSchedule, Console.Out);
        }
    }
}