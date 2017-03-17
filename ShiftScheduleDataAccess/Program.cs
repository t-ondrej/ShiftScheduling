using System;
using ShiftScheduleDataAccess.Dao;
using ShiftScheduleUtilities;

namespace ShiftScheduleDataAccess
{
    internal class Program
    {
        private static readonly DataAccessClient ClientRead = new DataAccessClient("../../Test/Read");
        private static readonly DataAccessClient ClientSave = new DataAccessClient("../../Test/Save");

        private static void Main()
        {
            ClientSave.InitializeWorkingFolder();
            Console.WriteLine("Person dao test:\n");
            PersonDaoTest();
            Console.WriteLine("Requirements dao test:\n");
            RequirementsDaoTest();
            Console.WriteLine("Resulting schedule dao test:\n");
            ResultingScheduleDaoTest();
            Console.WriteLine("Requirements fulfilling stats dao test:\n");
            RequirementsFulfillingStatsDaoTest();
        }

        private static void PersonDaoTest()
        {
            var persons = ClientRead.PersonDao.GetAllPersons();
            persons.ForEach(person => ClientSave.PersonDao.SavePerson(person));
        }

        private static void RequirementsDaoTest()
        {
            var requirements = ClientRead.RequirementsDao.GetRequirements();
            ClientSave.RequirementsDao.SaveRequirements(requirements);
        }

        private static void ResultingScheduleDaoTest()
        {
            var resultingSchedule = ClientRead.ResultingScheduleDao.GetResultingSchedule();
            ClientSave.ResultingScheduleDao.SaveResultingSchedule(resultingSchedule);
        }

        private static void RequirementsFulfillingStatsDaoTest()
        {
            var requirementsFulfillingStats = ClientRead.RequirementsFulfillingStatsDao.GetRequirements();
            ClientSave.RequirementsFulfillingStatsDao.SaveRequirements(requirementsFulfillingStats);
        }
    }
}