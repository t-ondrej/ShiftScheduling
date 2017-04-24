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

        public List<int> WorkingDays { get;  } 

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
            var shiftWeights = GenerateShiftWeights();

            WorkingDays.ForEach(day => {
                var dailyAvailability = GenerateDailyAvailability(shiftWeights[day]);
                dailyAvailabilities.Add(day, dailyAvailability);
            });

            return dailyAvailabilities;
        }

        private Person.DailyAvailability GenerateDailyAvailability(double shiftWeight)
        {
            var availability = GenerateInterval();

            var leftTolerance = Random.NextDouble() < Configuration.ToleranceAssignmentProbability 
                ? Random.Next(0, availability.Start + 1) 
                : 0;

            var rightTolerance = Random.NextDouble() < Configuration.ToleranceAssignmentProbability 
                ? Random.Next(0, Configuration.WorkingTimePerDay - availability.End) 
                : 0;

            return new Person.DailyAvailability(availability, leftTolerance, rightTolerance, shiftWeight);
        }

        private IDictionary<int, double> GenerateShiftWeights()
        {
            var shiftWeights = new Dictionary<int, double>();

            WorkingDays.ForEach(day => {
                var unitCount = Configuration.NumberOfShiftWeightValues;
                shiftWeights.Add(day, (double)Random.Next(1, unitCount + 1) / unitCount);
            });
        
            return shiftWeights;
        }

        private Interval GenerateInterval()
        {
            var start = Random.Next(0, Configuration.WorkingTimePerDay);
            var end = Random.Next(start, Configuration.WorkingTimePerDay);

            return new Interval(start, end);
        }

        private List<int> GenerateWorkingDays()
        {
            var days = new List<int>();

            for (var i = 0; i < Configuration.ScheduleDaysCount; i++)           
                if (Random.NextDouble() < Configuration.DayAssignmentDensity)               
                    days.Add(i);
                         
            return days;
        }
    }
}