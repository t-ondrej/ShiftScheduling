using System;
using System.IO;
using ShiftScheduleAlgorithm.ShiftAlgorithmProvider;
using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Entities.Helpers;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm
{
    internal class Program
    {
        private static void Main()
        {
            const string name = "DataSets/Set1";
            var path = PathUtilities.GetPathFromRelativeProjectPath(name);
            var requirementsDao = new FileRequirementsDao(path);
            var personsDao = new FilePersonDao(path);
            var resultingScheduleDao = new FileResultingScheduleDao(Path.Combine(path, "Output"));

            var requirements = requirementsDao.GetRequirements();
            var persons = personsDao.GetAllPersons();
            var properties = new AlgorithmConfiguration();

            var input = new ShiftAlgorithm.Input(persons, requirements, properties);
            var resultingSchedule = ShiftAlgorithm.ExecuteAlgorithm(input, ShiftAlgorithm.Strategy.Test);
            EntitiesPrinter.PrintResultingschedule(resultingSchedule, Console.Out);
            resultingScheduleDao.SaveResultingSchedule(resultingSchedule);

            //Console.WriteLine("Schedulable work:");
            //foreach (var schedulableWork in timeUnits.SchedulableWork)
            //{
            //    HelpersPrinter.PrintScheduledWork(schedulableWork, Console.Out);
            //}

            //Console.WriteLine("\nSchedulable persons:");
            //foreach (var scheduledPerson in timeUnits.ScheduledPersons)
            //{
            //    HelpersPrinter.PrintScheduledPerson(scheduledPerson, Console.Out);
            //}

            //Console.WriteLine("Time units:");
            //var timeUnitsCopy = new List<TimeUnit>(timeUnits.AllTimeUnits);
            //timeUnitsCopy.Sort((t1, t2) =>
            //{
            //    if (t1.DayId != t2.DayId)
            //        return t1.DayId - t2.DayId;

            //    return t1.UnitOfDay - t2.UnitOfDay;
            //});
            //foreach (var timeUnit in timeUnitsCopy)
            //{
            //    HelpersPrinter.PrintTimeUnit(timeUnit, Console.Out);
            //}
        }
    }
}