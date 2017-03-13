using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleGenerator
{
    internal class RequirementsGenerator
    {
        private enum Difficulties
        {
            Easy, Medium, Unsolveable
        }

        private static readonly Random Random = new Random();

        private const double ToleranceAssignmentProbability = 0.1;

        public GeneratorConfiguration Configuration { get; }

        public RequirementsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Requirements GenerateRequirements(List<Person> persons)
        {
            // Array: Days x Hours
            var monthRequirements = new double[Configuration.ScheduleDaysCount, Configuration.WorkingTimeLength];
            var difficulty = GetRandomDifficulty();

            // Person by person
            foreach (var person in persons)
            {
                var dailyAvailabilities = new Dictionary<int, Person.DailyAvailability>(person.DailyAvailabilities);
                var workingTime = person.MaxWork;

                var daysList = dailyAvailabilities.Keys.ToList();

                // Day by day from person's availabilities
                while (daysList.Count > 0)
                {
                    // Take random day
                    var randomIdx = Random.Next(daysList.Count);
                    var element = dailyAvailabilities.ElementAt(randomIdx);

                    var dailyAvailability = element.Value;
                    var day = element.Key;                    

                    // If person didn't exceed his MaxWorkHours
                    if (workingTime > 0)
                    {
                        
                        // Randomly take left/right tolerance or neither
                        if (dailyAvailability.LeftTolerance > 0 && Random.NextDouble() < ToleranceAssignmentProbability)
                        {
                            // Random tolerance length
                            var increment = Math.Min(workingTime, Random.Next(1, dailyAvailability.LeftTolerance + 1));
                            var toleranceStart = dailyAvailability.Availability.Start - increment;
                            for (var hour = toleranceStart; hour < dailyAvailability.Availability.Start; hour++)
                            {
                                monthRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                        }

                        if (dailyAvailability.RightTolerance > 0 && Random.NextDouble() < ToleranceAssignmentProbability)
                        {
                            var increment = Math.Min(workingTime, Random.Next(1, dailyAvailability.RightTolerance + 1));
                            var toleranceEnd = dailyAvailability.Availability.End + increment;
                            for (var hour = dailyAvailability.Availability.End + 1; hour <= toleranceEnd; hour++)
                            {
                                monthRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                        }

                        // Hour by hour in the random day
                        foreach (var hour in dailyAvailability.Availability)
                        {
                            if (workingTime >= 1)
                            {
                                monthRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                            else
                                break;
                        }

                    }
                    
                    // Remove the day from list and person's daily availabilities
                    daysList.RemoveAt(randomIdx);
                    dailyAvailabilities.Remove(day);
                }
            }

            var requirements = new Requirements(ArrayToRequirements(monthRequirements));

            if (difficulty != Difficulties.Medium)
            {
                ChangeRequirementsDifficulty(requirements, difficulty);
            }

            return requirements;
        }

        private static IDictionary<int, Requirements.DailyRequirement> ArrayToRequirements(double[,] array)
        {
            var requirement = new Dictionary<int, Requirements.DailyRequirement>();

            for (var i = 0; i < array.GetLength(0); i++)
            {
                if (IsArrayOfZeros(array, i))
                {
                    continue;
                }

                var dailyRequirement = new Dictionary<int, double>();

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


        private void ChangeRequirementsDifficulty(Requirements requirements, Difficulties difficulty)
        {
            var RequirementChangeProbability = 0.35;

            var days = requirements.DaysToRequirements.Keys;
            foreach (var day in days)
            {
                var hours = requirements.DaysToRequirements[day].HourToWorkers.Keys;
                foreach (var hour in hours)
                {
                    if (Random.NextDouble() < RequirementChangeProbability)
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

        private static bool IsArrayOfZeros(double[,] array, int index)
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
