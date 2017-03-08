using System;
using System.Collections.Generic;
using ShiftScheduleData.DataAccess.FileDao;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;
using static ShiftScheduleGenerator.GeneratorConfiguration;

namespace ShiftScheduleGenerator
{
    internal class ScheduleGenerator
    {
        private static readonly Random Random = new Random();

        internal List<Person> GeneratePersons()
        {
            var persons = new List<Person>();

            for (var i = 0; i < EmployeeCount; i++)
            {
                var person = CreatePerson(i);
                persons.Add(person);
            }

            return persons;
        }

        private Person CreatePerson(int id)
        {
            var dailySchedules = GenerateDailySchedules();
            var maxHoursPerMonth = WorkingTimePerMonthMax;

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

            var spaceForIntervalMin = IntervalLengthMin + IntervalsDistanceMin;
            var end = WorkingTimeLength - (spaceForIntervalMin * (intervalCount - 1));

            start = Random.Next(start, end);
            end = Random.Next(start, end);

            var interval = new Interval(start, end);

            intervals.Add(interval);
            intervals.AddRange(GenerateIntervals(intervalCount - 1, end + IntervalsDistanceMin + 1));

            return intervals;
        }

        private IEnumerable<int> GenerateDays()
        {
            var days = new List<int>();

            for (var i = 0; i < ScheduleDaysCount; i++)
            {
                if (Random.NextDouble() < 0.5)
                {
                    days.Add(i);
                }
            }

            return days;
        }
    }
}
