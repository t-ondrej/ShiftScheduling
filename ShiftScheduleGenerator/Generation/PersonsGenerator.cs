using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleGenerator.Generation
{
    internal class PersonsGenerator
    {
        private static readonly Random Random = new Random();

        public GeneratorConfiguration Configuration { get; }

        public IEnumerable<int> WorkingDays { get;  } 

        public PersonsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
            WorkingDays = GenerateWorkingDays();
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
            var maxWork = Random.Next(Configuration.WorkingTimePerMonthMin, Configuration.WorkingTimePerMonthMax + 1);

            return new Person(id, maxWork, dailyAvailabilities);
        }

        private IDictionary<int, Person.DailyAvailability> GenerateDailyAvailabilities()
        {
            var dailyAvailabilities = new Dictionary<int, Person.DailyAvailability>();

            foreach (var day in WorkingDays)
            {
                var dailyAvailability = GenerateDailyAvailability();
                dailyAvailabilities.Add(day, dailyAvailability);
            }

            return dailyAvailabilities;
        }

        private Person.DailyAvailability GenerateDailyAvailability()
        {
            var availability = GenerateInterval();

            var leftTolerance = Random.NextDouble() < Configuration.ToleranceAssignmentProbability ? 
                                    Random.Next(0, availability.Start + 1) : 0;
            var rightTolerance = Random.NextDouble() < Configuration.ToleranceAssignmentProbability ? 
                                    Random.Next(0, Configuration.WorkingTimePerDay - availability.End) : 0;

            var shiftWeight = GenerateShiftWeight();

            return new Person.DailyAvailability(availability, leftTolerance, rightTolerance, shiftWeight);
        }

        private double GenerateShiftWeight()
        {
            var unitCount = Configuration.NumberOfShiftWeightValues;
            return (double) Random.Next(0, unitCount + 1) / unitCount;
        }

        private Interval GenerateInterval()
        {
            var start = Random.Next(0, Configuration.WorkingTimePerDay);
            var end = Random.Next(start, Configuration.WorkingTimePerDay);

            return new Interval(start, end);
        }

        private IEnumerable<int> GenerateWorkingDays()
        {
            var days = new List<int>();

            for (var i = 0; i < Configuration.ScheduleDaysCount; i++)
            {
                if (Random.NextDouble() < Configuration.DayAssignmentDensity)
                {
                    days.Add(i);
                }
            }

            return days;
        }
    }
}