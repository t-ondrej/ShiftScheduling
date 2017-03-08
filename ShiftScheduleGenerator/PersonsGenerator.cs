using System;
using System.Collections.Generic;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleGenerator
{
    internal class PersonsGenerator
    {
        private static readonly Random Random = new Random();

        private const double DayAssignmentProbability = 0.5;

        private const int IntervalsPerDayMax = 5;

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
            var dailySchedules = GenerateDailySchedules();
            var maxHoursPerMonth = Configuration.WorkingTimePerMonthMax;

            return new Person(id, dailySchedules, maxHoursPerMonth);
        }

        private MonthlySchedule GenerateDailySchedules()
        {
            var dailySchedules = new Dictionary<int, Intervals>();
            var days = GenerateDays();

            foreach (var day in days)
            {
                var intervalCount = Random.Next(1, IntervalsPerDayMax + 1);
                var intervals = GenerateIntervals(intervalCount, 0);

                dailySchedules.Add(day, new Intervals(intervals));
            }

            return new MonthlySchedule(dailySchedules);
        }

        private List<Interval> GenerateIntervals(int intervalCount, int start)
        {
            var intervals = new List<Interval>();

            if (intervalCount == 0)
                return intervals;

            var end = Configuration.WorkingTimeLength - 2 * (intervalCount - 1);

            start = Random.Next(start, end);
            end = Random.Next(start, end);

            var interval = new Interval(start, end);

            intervals.Add(interval);
            intervals.AddRange(GenerateIntervals(intervalCount - 1, end + 2));

            return intervals;
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