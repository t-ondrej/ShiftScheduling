using System.IO;
using System.Linq;

namespace ShiftScheduleData.Entities.Helpers
{
    public static class EntitiesPrinter
    {
        public static void PrintPerson(PersonOld personOld, TextWriter textWriter)
        {
            textWriter.WriteLine($"id={personOld.Id} maxHours={personOld.MaxHoursPerMonth}");

            foreach (var dailySchedule in personOld.Schedule.DailySchedules)
            {
                var intervals = dailySchedule.Value.IntervalsList.Select(i => i.ToString());
                textWriter.WriteLine($"day={dailySchedule.Key} hours=[{string.Join(",", intervals)}]");
            }

            textWriter.WriteLine();
        }

        public static void PrintRequirements(MonthlyRequirements monthlyRequirements, TextWriter textWriter)
        {
            foreach (var dailyRequirement in monthlyRequirements.DaysToRequirements)
            {
                var requirementStrings = dailyRequirement.Value.HourToWorkers.Select(i => $"[h={i.Key},n={i.Value}]");
                var requirementString = string.Join(" ", requirementStrings);
                textWriter.WriteLine($"day={dailyRequirement.Key} {requirementString}");
            }

            textWriter.WriteLine();
        }

        public static void PrintResultingschedule(ResultingScheduleOld resultingScheduleOld, TextWriter textWriter)
        {
            foreach (var scheduleForPerson in resultingScheduleOld.SchedulesForPeople)
            {
                textWriter.WriteLine($"personOld={scheduleForPerson.Key.Id}");
                PrintSchedule(scheduleForPerson.Value, textWriter);
            }
        }

        public static void PrintSchedule(Schedule schedule, TextWriter textWriter)
        {
            foreach (var dailySchedule in schedule.DailySchedules)
            {
                var intervals = dailySchedule.Value.IntervalsList.Select(i => i.ToString());
                textWriter.WriteLine($"day={dailySchedule.Key} hours=[{string.Join(",", intervals)}]");
            }

            textWriter.WriteLine();
        }
    }
}