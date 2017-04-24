using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;

namespace ShiftScheduleGenerator.Generation
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
            switch (Configuration.DifficultyToFulfilRequirements.DifficutyPossilibity)
            {
                case Difficulty.Possilibity.Possible:
                    return GeneratePossibleRequirements(persons);
                case Difficulty.Possilibity.Maybe:
                    return GenerateRandomRequirements(persons);
                case Difficulty.Possilibity.Impossible:
                    return GenerateImpossibleRequirements(persons);
                default:
                    return null;
            }
        }

        private Requirements GeneratePossibleRequirements(List<Person> persons)
        {
            // Array: Days x Hours
            var timeSpanRequirements = new double[Configuration.ScheduleDaysCount, Configuration.WorkingTimePerDay];

            // Person by person
            persons.ForEach(person =>
            {
                var dailyAvailabilities = new Dictionary<int, Person.DailyAvailability>(person.DailyAvailabilities);

                // Generate requirements that person should works only some % of his MaxWork 
                var workingTime = (int) (person.MaxWork * Configuration.DifficultyToFulfilRequirements.Value);
                var daysList = dailyAvailabilities.Keys.ToList();

                // Iterate day by day from person's availabilities
                while (daysList.Count > 0)
                {
                    // Take random day
                    var randomIdx = Random.Next(daysList.Count);
                    var pair = dailyAvailabilities.ElementAt(randomIdx);

                    var day = pair.Key;
                    var dailyAvailability = pair.Value;

                    // If there is some workingTime left
                    if (workingTime > 0)
                    {
                        // Randomly take left/right tolerance or neither
                        if (dailyAvailability.LeftTolerance > 0 &&
                            Random.NextDouble() < Configuration.ToleranceUseProbability)
                        {
                            // Random tolerance length
                            var increment = Math.Min(workingTime, Random.Next(1, dailyAvailability.LeftTolerance + 1));
                            var toleranceStart = dailyAvailability.Availability.Start - increment;

                            for (var hour = toleranceStart; hour < dailyAvailability.Availability.Start; hour++)
                            {
                                timeSpanRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                        }
                      

                        // Hour by hour in the random day
                        foreach (var hour in dailyAvailability.Availability)
                        {
                            if (workingTime >= 1)
                            {
                                timeSpanRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                            else
                                break;
                        }


                        if (dailyAvailability.RightTolerance > 0 &&
                            Random.NextDouble() < Configuration.ToleranceUseProbability)
                        {
                            var increment = Math.Min(workingTime, Random.Next(1, dailyAvailability.RightTolerance + 1));
                            var toleranceEnd = dailyAvailability.Availability.End + increment;

                            for (var hour = dailyAvailability.Availability.End + 1; hour <= toleranceEnd; hour++)
                            {
                                timeSpanRequirements[day, hour] += dailyAvailability.ShiftWeight;
                                workingTime--;
                            }
                        }
                    }

                    // Remove the day from list and copied daily availabilities
                    daysList.RemoveAt(randomIdx);
                    dailyAvailabilities.Remove(day);
                }
            }); 

            return new Requirements(ArrayToRequirements(timeSpanRequirements));
        }

        private Requirements GenerateRandomRequirements(List<Person> persons)
        {
            var requirements = GenerateMaxRequirements(persons);
            MakeRequirementsRandom(requirements);

            return requirements;
        }

        private Requirements GenerateImpossibleRequirements(List<Person> persons)
        {
            var requirements = GenerateMaxRequirements(persons);

            // Save the maximum number of workers required in a random day and random hour
            var randomDay = requirements.DaysToRequirements.Keys.ElementAt(Random.Next(1, requirements.DaysToRequirements.Count));
            var randomHour = Random.Next(0, requirements.DaysToRequirements[randomDay].HourToWorkers.Count);
            var maxWorkerCountRequirement = requirements.DaysToRequirements[randomDay].HourToWorkers[randomHour];

            MakeRequirementsRandom(requirements);

            requirements.DaysToRequirements[randomDay].HourToWorkers[randomHour] = maxWorkerCountRequirement + 1;

            return requirements;
        }


        private Requirements GenerateMaxRequirements(List<Person> persons)
        {
            var timeSpanRequirements = new double[Configuration.ScheduleDaysCount, Configuration.WorkingTimePerDay];

            foreach (var person in persons)
            {
                foreach (var day in person.DailyAvailabilities.Keys)
                {
                    foreach (var hour in person.DailyAvailabilities[day].Availability)
                    {
                        timeSpanRequirements[day, hour] += person.DailyAvailabilities[day].ShiftWeight;                     
                    }

                    var start = person.DailyAvailabilities[day].Availability.Start;
                    var end = person.DailyAvailabilities[day].Availability.End;

                    for (var i = start - 1; i >= start - person.DailyAvailabilities[day].LeftTolerance; i--)
                        timeSpanRequirements[day, i] += person.DailyAvailabilities[day].ShiftWeight;

                    for (var i = end + 1; i <= end + person.DailyAvailabilities[day].RightTolerance; i++)
                        timeSpanRequirements[day, i] += person.DailyAvailabilities[day].ShiftWeight;
                }
            }

            return new Requirements(ArrayToRequirements(timeSpanRequirements));
        }

        private void MakeRequirementsRandom(Requirements requirements)
        {
            requirements.DaysToRequirements.ToList().ForEach(daysToReq =>
            {
                daysToReq.Value.HourToWorkers.ForEach(hourToWorkers =>
                {
                    hourToWorkers -= Random.Next(0, (int)hourToWorkers + 1);
                });
            });
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

                var dailyRequirement = Enumerable.Repeat(0.0, array.GetLength(1)).ToList();

                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] > 0)
                    {
                        dailyRequirement[j] = array[i, j];
                    }
                }

                requirement.Add(i, new Requirements.DailyRequirement(dailyRequirement));
            }

            return requirement;
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