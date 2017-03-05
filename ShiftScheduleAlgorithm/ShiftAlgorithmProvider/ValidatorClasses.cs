using System;
using System.Collections;
using System.Collections.Generic;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    public interface IErrorReport
    {
        void PrintErrorMessage();
    }

    public class AlgorithmErrorReport
    {
        private List<MaxDailyWorkPropertyNotMet> MaxDailyWork { get; }
        private List<WorkerPauseLengthPropertyNotMet> WorkerPauseLenth { get; }
        private List<MaxConsecutiveWorkHoursPropertyNotMet> MaxConsecutiveWorkHours { get; }
        private List<PersonScheduleRequirementsNotMet> PersonScheduleRequirements { get; }
        private List<RequirementsAreNotMet> Requirements { get; }

        public AlgorithmErrorReport()
        {
            MaxDailyWork = new List<MaxDailyWorkPropertyNotMet>();
            WorkerPauseLenth = new List<WorkerPauseLengthPropertyNotMet>();
            MaxConsecutiveWorkHours = new List<MaxConsecutiveWorkHoursPropertyNotMet>();
            PersonScheduleRequirements = new List<PersonScheduleRequirementsNotMet>();
            Requirements = new List<RequirementsAreNotMet>();
        }
    }

    public class MaxDailyWorkPropertyNotMet : IErrorReport
    {
        public Person Person { get; }
        public int Day { get; }

        public MaxDailyWorkPropertyNotMet(Person person, int day)
        {
            Person = person;
            Day = day;
        }

        public void PrintErrorMessage()
        {
            Console.Out.WriteLine($"Person {Person.Id} works more than he can without " +
                                  $"a pause on day {Day}");
        }
    }

    public class WorkerPauseLengthPropertyNotMet : IErrorReport
    {
        public Person Person { get; }
        public int Day { get; }

        public WorkerPauseLengthPropertyNotMet(Person person, int day)
        {
            Person = person;
            Day = day;
        }

        public void PrintErrorMessage()
        {
            Console.Out.WriteLine($"Person {Person.Id} works more than he can without " +
                                  $"a pause on day {Day}");
        }
    }

    public class MaxConsecutiveWorkHoursPropertyNotMet : IErrorReport
    {
        public Person Person { get; }
        public int Day { get; }

        public MaxConsecutiveWorkHoursPropertyNotMet(Person person, int day)
        {
            Person = person;
            Day = day;
        }

        public void PrintErrorMessage()
        {
            Console.Out.WriteLine($"Person {Person.Id} works more than maximum consecutive working " +
                                  $"hours on day {Day}");
        }
    }

    public class PersonScheduleRequirementsNotMet : IErrorReport
    {
        public Person Person { get; }
        public int Day { get; }
        public Interval Interval { get; }

        public PersonScheduleRequirementsNotMet(Person person, int day, Interval interval)
        {
            Person = person;
            Day = day;
            Interval = interval;
        }

        public void PrintErrorMessage()
        {
            Console.Out.WriteLine($"Person {Person.Id} works out of his Schedule on day {Day} " +
                                  $"in interval ({Interval.Start}, {Interval.End})");
        }
    }

    public class RequirementsAreNotMet : IErrorReport
    {
        public int Day { get; }
        public Interval Interval { get; }

        public RequirementsAreNotMet(int day, Interval interval)
        {
            Day = day;
            Interval = interval;
        }

        public void PrintErrorMessage()
        {
            Console.Out.WriteLine($"Insufficient workers for day {Day} at Interval " +
                                  $"({Interval.Start} - {Interval.End})");
        }
    }
}