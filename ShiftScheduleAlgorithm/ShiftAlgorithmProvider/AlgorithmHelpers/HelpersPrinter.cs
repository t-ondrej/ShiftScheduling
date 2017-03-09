﻿using System.IO;
using ShiftScheduleData.Entities.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal static class HelpersPrinter
    {
        public static void PrintScheduledPerson(ScheduledPerson scheduledPerson, TextWriter textWriter)
        {
            EntitiesPrinter.PrintPerson(scheduledPerson.PersonOld, textWriter);
        }

        public static void PrintScheduledWork(SchedulableWork schedulableWork, TextWriter textWriter)
        {
            textWriter.WriteLine($"personOld={schedulableWork.ScheduledPerson.PersonOld.Id} " +
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