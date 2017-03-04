using System.IO;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal static class HelpersPrinter
    {
        public static void PrintScheduledPerson(ScheduledPerson scheduledPerson, TextWriter textWriter)
        {
            EntitiesPrinter.PrintPerson(scheduledPerson.Person, textWriter);
        }

        public static void PrintScheduledWork(SchedulableWork schedulableWork, TextWriter textWriter)
        {
            textWriter.WriteLine($"person={schedulableWork.ScheduledPerson.Person.Id} " +
                                 $"day={schedulableWork.DayId} " +
                                 $"interval={schedulableWork.Interval.Start}-{schedulableWork.Interval.End}");
        }

        public static void PrintTimeUnit(TimeUnit timeUnit, TextWriter textWriter)
        {
            textWriter.WriteLine($"day={timeUnit.DayId} " +
                                 $"unitOfDay={timeUnit.UnitOfDay} " +
                                 $"possibleWorkers={timeUnit.IdToPotentionalWork.Count} " +
                                 $"requiredWorkers={timeUnit.RequiredWorkers}");
        }
    }
}