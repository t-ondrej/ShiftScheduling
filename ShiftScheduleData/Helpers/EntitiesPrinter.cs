using System.IO;
using System.Linq;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.Helpers
{
    public static class EntitiesPrinter
    {
        public static void PrintPerson(Person person, TextWriter textWriter)
        {
            textWriter.WriteLine($"id={person.Id} maxHours={person.MaxHoursPerMonth}");

            foreach (var dailySchedule in person.MonthlySchedule.DailySchedules)
            {
                var intervals = dailySchedule.Value.IntervalsList.Select(i => i.ToString());
                textWriter.WriteLine($"day={dailySchedule.Key} hours=[{string.Join(",", intervals)}]");
            }

            textWriter.WriteLine();
        }

        public static void PrintRequirements(Requirements requirements, TextWriter textWriter)
        {
            foreach (var dailyRequirement in requirements.DayToRequirement)
            {
                var requirementStrings = dailyRequirement.Value.HourToWorkers.Select(i => $"[h={i.Key},n={i.Value}]");
                var requirementString = string.Join(" ", requirementStrings);
                textWriter.WriteLine($"day={dailyRequirement.Key} {requirementString}");
            }

            textWriter.WriteLine();
        }

        public static void PrintResultingschedule(ResultingSchedule resultingSchedule, TextWriter textWriter)
        {
            foreach (var scheduleForPerson in resultingSchedule.SchedulesForPeople)
            {
                textWriter.WriteLine($"person={scheduleForPerson.Key.Id}");
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