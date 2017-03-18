using System.Diagnostics;
using System.Linq;
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
            Debug.WriteLine("Person dao test:\n");
            PersonDaoTest();
            Debug.WriteLine("Requirements dao test:\n");
            RequirementsDaoTest();
            Debug.WriteLine("Resulting schedule dao test:\n");
            ResultingScheduleDaoTest();
            Debug.WriteLine("Requirements fulfilling stats dao test:\n");
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
            var resultingSchedule = ClientRead.ResultingScheduleDao.GetResultingSchedules().First();
            ClientSave.ResultingScheduleDao.SaveResultingSchedule(resultingSchedule);
        }

        private static void RequirementsFulfillingStatsDaoTest()
        {
            var requirementsFulfillingStats = ClientRead.RequirementsFulfillingStatsDao.GetRequirements();
            ClientSave.RequirementsFulfillingStatsDao.SaveRequirements(requirementsFulfillingStats);
        }
    }
}