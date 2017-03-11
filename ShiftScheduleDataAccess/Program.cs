using System;
using ShiftScheduleDataAccess.OldEntities;
using ShiftScheduleUtilities;

namespace ShiftScheduleDataAccess
{
    internal class Program
    {
        private const string ReadSetName = "TestData";
        private const string PutSetName = "TestData/Output";

        private static void Main()
        {
            Console.WriteLine("PersonOld dao test:\n");
            PersonDaoTest();
            Console.WriteLine("MonthlyRequirementsOld dao test:\n");
            RequirementsDaoTest();
            Console.WriteLine("\nResulting scheduleOld dao test:\n");
            ResultingScheduleDaoTest();
        }

        private static void RequirementsDaoTest()
        {
            var requirementsDaoRead = new FileRequirementsDaoOld(PathUtilities.GetPathFromRelativeProjectPath(ReadSetName));
            var requirementsDaoPut = new FileRequirementsDaoOld(PathUtilities.GetPathFromRelativeProjectPath(PutSetName));
            var requirements = requirementsDaoRead.GetRequirements();

            EntitiesPrinter.PrintRequirements(requirements, Console.Out);
            requirementsDaoPut.SaveRequirements(requirements);
        }

        private static void PersonDaoTest()
        {
            var personDaoRead = new FilePersonDaoOld(PathUtilities.GetPathFromRelativeProjectPath(ReadSetName));
            var personDaoPut = new FilePersonDaoOld(PathUtilities.GetPathFromRelativeProjectPath(PutSetName));
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
            var personDaoRead = new FilePersonDaoOld(PathUtilities.GetPathFromRelativeProjectPath(ReadSetName));
            var persons = personDaoRead.GetAllPersons().ToList();
            var dictionary = persons.ToDictionary(p => p, p => p.ScheduleOld);
            var resultingSchedule = new ResultingScheduleOld(dictionary);
            var resultingScheduleDao = new FileResultingScheduleDaoOld(PathUtilities.GetPathFromRelativeProjectPath(ReadSetName));
            resultingScheduleDao.SaveResultingSchedule(resultingSchedule);

            var reloadedSchedule = resultingScheduleDao.GetResultingSchedule(persons);
            var resultingSchedulePut = new FileResultingScheduleDaoOld(PathUtilities.GetPathFromRelativeProjectPath(PutSetName));
            resultingSchedulePut.SaveResultingSchedule(reloadedSchedule);

            EntitiesPrinter.PrintResultingschedule(resultingSchedule, Console.Out);
        }
    }
}