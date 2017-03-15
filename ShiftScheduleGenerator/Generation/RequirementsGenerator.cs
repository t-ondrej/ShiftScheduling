using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleLibrary.Entities;

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

        public Requirements Generate(List<Person> persons)
        {
            switch (Configuration.DifficultyToFulfilRequirements.DifficutyPossilibity)
            {
                case Difficulty.Possilibity.Possible:
                    return GenerateRequirements(persons);
                case Difficulty.Possilibity.Maybe:
                    return GenerateRandomRequirements();
                case Difficulty.Possilibity.Impossible:
                    return GenerateImpossibleRequirements(persons);
                default:
                    return null;
            }
        }

        private Requirements GenerateRequirements(List<Person> persons)
        {
            // Array: Days x Hours
            var timeSpanRequirements = new double[Configuration.ScheduleDaysCount, Configuration.WorkingTimePerDay];

            // Person by person
            foreach (var person in persons)
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
                    var element = dailyAvailabilities.ElementAt(randomIdx);

                    var dailyAvailability = element.Value;
                    var day = element.Key;

                    // If there is some workingTime left
                    if (workingTime > 0)
                    {
                        // Randomly take left/right tolerance or neither
                        if (dailyAvailability.LeftTolerance > 0 && Random.NextDouble() < Configuration.ToleranceUseProbability)
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

                        if (dailyAvailability.RightTolerance > 0 && Random.NextDouble() < Configuration.ToleranceUseProbability)
                        {
                            var increment = Math.Min(workingTime, Random.Next(1, dailyAvailability.RightTolerance + 1));
                            var toleranceEnd = dailyAvailability.Availability.End + increment;

                            for (var hour = dailyAvailability.Availability.End + 1; hour <= toleranceEnd; hour++)
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

                    }
                    
                    // Remove the day from list and copied daily availabilities
                    daysList.RemoveAt(randomIdx);
                    dailyAvailabilities.Remove(day);
                }
            }

            return new Requirements(ArrayToRequirements(timeSpanRequirements));
        }

        private Requirements GenerateRandomRequirements()
        {
            var persons = new PersonsGenerator(Configuration).GeneratePersons(Configuration.EmployeeCount * (2/3));

            return GenerateRequirements(persons);
        }

        private Requirements GenerateImpossibleRequirements(List<Person> persons)
        {
            var requirements = GenerateMaxRequirements(persons);

            var randomDay = Random.Next(0, requirements.DaysToRequirements.Keys.ToList().Count);
            var randomHour = Random.Next(0, requirements.DaysToRequirements[randomDay].HourToWorkers.Keys.ToList().Count);

            requirements.DaysToRequirements[randomDay].HourToWorkers[randomHour] += 1;

            foreach (var day in requirements.DaysToRequirements.Keys)
            {
                foreach (var hour in requirements.DaysToRequirements[day].HourToWorkers.Keys)
                {
                    if (day == randomDay && hour == randomHour)
                        continue;

                    requirements.DaysToRequirements[day].HourToWorkers[hour] -= 
                        Random.Next(0, (int)requirements.DaysToRequirements[day].HourToWorkers[hour]);
                }
            }

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

                        for (var i = hour - 1; i >= hour - person.DailyAvailabilities[day].LeftTolerance; i--)
                            timeSpanRequirements[day, i] += person.DailyAvailabilities[day].ShiftWeight;

                        for (var i = hour + 1; i <= hour + person.DailyAvailabilities[day].RightTolerance; i++)
                            timeSpanRequirements[day, i] += person.DailyAvailabilities[day].ShiftWeight;

                    }
                }
            }

            return new Requirements(ArrayToRequirements(timeSpanRequirements));
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
