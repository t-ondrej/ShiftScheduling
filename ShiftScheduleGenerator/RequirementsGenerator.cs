using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleGenerator
{
    internal class RequirementsGenerator
    {
        private static readonly Random Random = new Random();

        public GeneratorConfiguration Configuration { get; }

        public RequirementsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Requirements GenerateRequirements(List<Person> persons)
        {
            var monthRequirements = new int[Configuration.ScheduleDaysCount, Configuration.WorkingTimeLength];

            foreach (var person in persons)
            {
                var schedule = new Dictionary<int, Intervals>(person.MonthlySchedule.DailySchedules);
                var sumHours = 0;

                var daysList = schedule.Keys.ToList();

                while (daysList.Count > 0)
                {
                    var randomIdx = Random.Next(daysList.Count);
                    var randomSchedule = schedule.ElementAt(randomIdx);

                    var intervals = randomSchedule.Value.IntervalsList;
                    var randomDay = randomSchedule.Key;

                    foreach (var interval in intervals)
                    {
                        var intervalLength = interval.End - interval.Start;

                        if (sumHours + intervalLength <= Configuration.WorkingTimePerMonthMax)
                        {
                            for (var j = interval.Start; j <= interval.End; j++)
                            {
                                monthRequirements[randomDay, j]++;
                                sumHours++;
                            }
                        }
                    }

                    daysList.RemoveAt(randomIdx);
                    schedule.Remove(randomDay);
                }
            }

            return new MonthlyRequirements(ArrayToRequirements(monthRequirements));
        }

        private static IDictionary<int, Requirements.DailyRequirement> ArrayToRequirements(int[,] array)
        {
            var requirement = new Dictionary<int, Requirements.DailyRequirement>();

            for (var i = 0; i < array.GetLength(0); i++)
            {
                if (IsArrayOfZeros(array, i))
                {
                    continue;
                }

                var dailyRequirement = new Dictionary<int, int>();

                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] > 0)
                    {
                        dailyRequirement.Add(j, array[i, j]);
                    }
                }

                requirement.Add(i, new MonthlyRequirements.DailyRequirement(dailyRequirement));
            }

            return requirement;
        }

        private static bool IsArrayOfZeros(int[,] array, int index)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                if (array[index, i] > 0)
                {
                    return false;
                }
            }               

            return true;
        }
    }
}
