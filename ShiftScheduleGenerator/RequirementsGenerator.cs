using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;
using static ShiftScheduleGenerator.GeneratorConfiguration;

namespace ShiftScheduleGenerator
{
    internal class RequirementsGenerator
    {
        private static readonly Random Random = new Random();

        internal Requirements GenerateRequirements(List<Person> persons)
        {
            var monthRequirements = new int[ScheduleDaysCount, WorkingTimeLength];

            foreach (var person in persons)
            {
                IDictionary<int, Intervals> schedule = new Dictionary<int, Intervals>(person.MonthlySchedule.DailySchedules);
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

                        if (sumHours + intervalLength <= WorkingTimePerMonthMax)
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

            return new Requirements(ArrayToRequirements(monthRequirements));
        }

        private IDictionary<int, Requirements.DailyRequirement> ArrayToRequirements(int[,] array)
        {
            IDictionary<int, Requirements.DailyRequirement> requirement = new Dictionary<int, Requirements.DailyRequirement>();

            for (var i = 0; i < array.GetLength(0); i++)
            {
                if (IsArrayOfZeros(array, i))
                {
                    continue;
                }

                IDictionary<int, int> dailyRequirement = new Dictionary<int, int>();

                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] > 0)
                    {
                        dailyRequirement.Add(j, array[i, j]);
                    }
                }

                requirement.Add(i, new Requirements.DailyRequirement(dailyRequirement));
            }

            return requirement;
        }

        private bool IsArrayOfZeros(int[,] array, int index)
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
