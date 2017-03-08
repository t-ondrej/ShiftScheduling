using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleGenerator
{
    internal class RequirementsGenerator
    {
        private enum Difficulties
        {
            Easy, Medium, Unsolveable
        }

        private static readonly Random Random = new Random();

        public GeneratorConfiguration Configuration { get; }

        public RequirementsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public MonthlyRequirements GenerateRequirements(List<Person> persons)
        {
            var monthRequirements = new int[Configuration.ScheduleDaysCount, Configuration.WorkingTimeLength];
            var difficulty = GetRandomDifficulty();

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

                        if (sumHours + interval.Count <= Configuration.WorkingTimePerMonthMax)
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

            var requirements = new MonthlyRequirements(ArrayToRequirements(monthRequirements));

            if (difficulty != Difficulties.Medium)
            {
                ChangeRequirementsDifficulty(requirements, difficulty);
            }

            return requirements;
        }

        private static IDictionary<int, MonthlyRequirements.DailyRequirement> ArrayToRequirements(int[,] array)
        {
            var requirement = new Dictionary<int, MonthlyRequirements.DailyRequirement>();

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


        private void ChangeRequirementsDifficulty(MonthlyRequirements requirements, Difficulties difficulty)
        {
            var ChangeRequirementProbability = 0.35;

            var days = requirements.DaysToRequirements.Keys;
            foreach (var day in days)
            {
                var hours = requirements.DaysToRequirements[day].HourToWorkers.Keys;
                foreach (var hour in hours)
                {
                    if (Random.NextDouble() < ChangeRequirementProbability)
                        requirements.DaysToRequirements[day].HourToWorkers[hour] +=
                            difficulty == Difficulties.Unsolveable ? 1 : -1;
                }
            }
        }

        private static Difficulties GetRandomDifficulty()
        {
            var difficultyIndex = Random.Next(1, 4);

            if (difficultyIndex == 1)
                return Difficulties.Easy;

            return difficultyIndex == 2 ? Difficulties.Medium : Difficulties.Unsolveable;
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
