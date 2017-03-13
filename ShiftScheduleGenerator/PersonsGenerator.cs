using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleGenerator
{
    internal class PersonsGenerator
    {
        private static readonly Random Random = new Random();

        private const double DayAssignmentProbability = 0.5;

        private const double ShiftWeightUnit = 0.25;

        public GeneratorConfiguration Configuration { get; }

        public PersonsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Person> GeneratePersons()
        {
            var persons = new List<Person>();

            for (var i = 0; i < Configuration.EmployeeCount; i++)
            {
                var person = CreatePerson(i);
                persons.Add(person);
            }

            return persons;
        }

        private Person CreatePerson(int id)
        {
            var dailyAvailabilities = GenerateDailyAvailabilities();
            var maxWork = Random.Next(1, Configuration.WorkingTimePerMonthMax + 1);

            return new Person(id, maxWork, dailyAvailabilities);
        }

        private IDictionary<int, Person.DailyAvailability> GenerateDailyAvailabilities()
        {
            var dailyAvailabilities = new Dictionary<int, Person.DailyAvailability>();
            var days = GenerateDays();

            foreach (var day in days)
            {
                var dailyAvailability = GenerateDailyAvailability();
                dailyAvailabilities.Add(day, dailyAvailability);
            }

            return dailyAvailabilities;
        }

        private Person.DailyAvailability GenerateDailyAvailability()
        {
            var availability = GenerateInterval();
            var leftTolerance = Random.Next(0, availability.Start + 1);
            var rightTolerance = Random.Next(0, Configuration.WorkingTimeLength - availability.End + 1);
            var shiftWeight = GenerateShiftWeight();

            return new Person.DailyAvailability(availability, leftTolerance, rightTolerance, shiftWeight);
        }

        private double GenerateShiftWeight()
        {
            var unitCount = (int) (Configuration.ShiftWeightMax / ShiftWeightUnit);

            return Random.Next(0, unitCount + 1) * ShiftWeightUnit;
        }

        private Interval GenerateInterval()
        {
            var start = Random.Next(0, Configuration.WorkingTimePerMonthMax);
            var end = Random.Next(start, Configuration.WorkingTimePerMonthMax);

            return new Interval(start, end); 
        }

        private IEnumerable<int> GenerateDays()
        {
            var days = new List<int>();

            for (var i = 0; i < Configuration.ScheduleDaysCount; i++)
            {
                if (Random.NextDouble() < DayAssignmentProbability)
                {
                    days.Add(i);
                }
            }

            return days;
        }
    }
}