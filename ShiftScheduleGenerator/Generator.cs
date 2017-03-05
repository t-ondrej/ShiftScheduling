using ShiftScheduleData.Helpers;
using ShiftScheduleData.Entities;
using System;
using System.Collections.Generic;
using ShiftScheduleData.DataAccess.FileDao;
using static ShiftScheduleGenerator.GeneratorConfiguration;

namespace ShiftScheduleGenerator
{
    internal class Generator
    {
        
        private static readonly Random Random = new Random();

        internal void GenerateData(string folderName)
        {
            FilePersonDao dao = new FilePersonDao($"..\\..\\{folderName}");
            var persons = GeneratePersons();
            
            foreach (var person in persons)
            {
                dao.SavePerson(person);
            }
        }

        private List<Person> GeneratePersons()
        {
            var persons = new List<Person>();

            for (int i = 0; i < EmployeeCount; i++)
            {       
                Person person = CreatePerson(i);
                persons.Add(person);
            }

            return persons;
        }

        private Person CreatePerson(int id)
        {
            var dailySchedules = GenerateDailySchedules();
            var maxHoursPerMonth = Random.Next(10, WorkingTimePerMonthMax);

            return new Person(id, dailySchedules, maxHoursPerMonth);
        }

        private Schedule GenerateDailySchedules()
        {
            var dailySchedules = new Dictionary<int, Intervals>();
            var days = GenerateDays();

            foreach (int day in days)
            {
                var intervalCount = Random.Next(1, IntervalsPerDayMax + 1);
                var intervals = GenerateIntervals(intervalCount, WorkingTimeStart);

                dailySchedules.Add(day, new Intervals(intervals));
            }

            return new Schedule(dailySchedules);
        }

        private List<Interval> GenerateIntervals(int intervalCount, int start)
        {
            var intervals = new List<Interval>();

            if (intervalCount == 0)
                return intervals;

            int minSpaceForInterval = IntervalLengthMin + IntervalsDistanceMin;
            int end = WorkingTimeEnd - (minSpaceForInterval * (intervalCount - 1));

            start = Random.Next(start, end - 1);
            end = Random.Next(start + 1, end);

            Interval interval = new Interval(start, end);

            intervals.Add(interval);
            intervals.AddRange(GenerateIntervals(intervalCount - 1, end + IntervalsDistanceMin + 1));

            return intervals;
        }

        private List<int> GenerateDays()
        {
            var days = new List<int>();

            for (int i = 0; i < ScheduleDaysCount; i++)
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
