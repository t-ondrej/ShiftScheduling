using System.IO;
using System.Linq;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.Helpers
{
    public static class Printers
    {
        public static void PrintPerson(Person person, TextWriter textWriter)
        {
            textWriter.WriteLine($"id={person.Id} maxHours={person.MaxHoursPerMonth}");

            foreach (var dailySchedule in person.MonthlySchedule.DailySchedules)
            {
                var intervals = dailySchedule.Value.IntervalsList.Select(Utilities.IntervalToString);
                textWriter.WriteLine($"day={dailySchedule.Key} hours=[{string.Join(",", intervals)}]");
            }
        }

        public static void PrintRequirements(Requirements requirements, TextWriter textWriter)
        {
            foreach (var dailyRequirement in requirements.DayToRequirement)
            {
                var requirementStrings = dailyRequirement.Value.HourToWorkers.Select(i => $"[h={i.Key},n={i.Value}]");
                var requirementString = string.Join(" ", requirementStrings);
                textWriter.WriteLine($"day={dailyRequirement.Key} {requirementString}");
            }
        }

        public static void PrintResultingschedule(ResultingSchedule resultingSchedule, TextWriter textWriter)
        {
            foreach (var scheduleForPerson in resultingSchedule.SchedulesForPeople)
            {
                textWriter.WriteLine($"person={scheduleForPerson.Key.Id}");

                foreach (var dailySchedule in scheduleForPerson.Value.DailySchedules)
                {
                    var intervals = dailySchedule.Value.IntervalsList.Select(Utilities.IntervalToString);
                    textWriter.WriteLine($"day={dailySchedule.Key} hours=[{string.Join(",", intervals)}]");
                }

                textWriter.WriteLine();
            }
        }
    }
}