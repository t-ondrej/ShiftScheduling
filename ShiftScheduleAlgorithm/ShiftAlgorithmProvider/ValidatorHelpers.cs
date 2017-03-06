using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    public enum ReportSeriousness
    {
        Error,
        Warning
    }

    public abstract class Report
    {
        public abstract ReportSeriousness Seriousness { get; }

        public void PrintReportMessage(TextWriter textWriter)
        {
            textWriter.WriteLine(GetReportMessage());
        }

        public abstract string GetReportMessage();
    }

    public class AlgorithmReport
    {
        public List<MaxMonthlyWorkNotMet> MaxMonthlyWork { get; }
        public List<MaxDailyWorkNotMet> MaxDailyWork { get; }
        public List<WorkerPauseLengthNotMet> WorkerPauseLenth { get; }
        public List<MaxConsecutiveWorkHoursNotMet> MaxConsecutiveWorkHours { get; }
        public List<PersonScheduleRequirementsNotMet> PersonScheduleRequirements { get; }
        public List<RequirementsAreNotMet> Requirements { get; }
        public List<OverlappingIntervals> OverlappingIntervals { get; }
        public List<ConsecutiveIntervals> ConsecutiveIntervals { get; }

        public AlgorithmReport()
        {
            MaxMonthlyWork = new List<MaxMonthlyWorkNotMet>();
            MaxDailyWork = new List<MaxDailyWorkNotMet>();
            WorkerPauseLenth = new List<WorkerPauseLengthNotMet>();
            MaxConsecutiveWorkHours = new List<MaxConsecutiveWorkHoursNotMet>();
            PersonScheduleRequirements = new List<PersonScheduleRequirementsNotMet>();
            Requirements = new List<RequirementsAreNotMet>();
            OverlappingIntervals = new List<OverlappingIntervals>();
            ConsecutiveIntervals = new List<ConsecutiveIntervals>();
        }
    }
    
    public class MaxMonthlyWorkNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Person Person { get; }

        public MaxMonthlyWorkNotMet(Person person)
        {
            Seriousness = ReportSeriousness.Error;
            Person = person;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than he can without in a month";
        }
    }

    public class MaxDailyWorkNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public MaxDailyWorkNotMet(Person person, int day)
        {
            Seriousness = ReportSeriousness.Error;
            Person = person;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than he can without a pause on day {Day}";
        }
    }

    public class WorkerPauseLengthNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public WorkerPauseLengthNotMet(Person person, int day)
        {
            Seriousness = ReportSeriousness.Error;
            Person = person;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than he can without a pause on day {Day}";
        }
    }

    public class MaxConsecutiveWorkHoursNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public MaxConsecutiveWorkHoursNotMet(Person person, int day)
        {
            Seriousness = ReportSeriousness.Error;
            Person = person;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than maximum consecutive working " +
                   $"hours on day {Day}";
        }
    }

    public class PersonScheduleRequirementsNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Person Person { get; }
        public int Day { get; }
        public Interval Interval { get; }

        public PersonScheduleRequirementsNotMet(Person person, int day, Interval interval)
        {
            Seriousness = ReportSeriousness.Error;
            Person = person;
            Day = day;
            Interval = interval;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works out of his AlgorithmOutput on day {Day} " +
                   $"in interval ({Interval.Start}, {Interval.End})";
        }
    }

    public class RequirementsAreNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public int Day { get; }
        public Interval Interval { get; }

        public RequirementsAreNotMet(int day, Interval interval)
        {
            Seriousness = ReportSeriousness.Error;
            Day = day;
            Interval = interval;
        }

        public override string GetReportMessage()
        {
            return $"Insufficient workers for day {Day} at Interval ({Interval.Start} - {Interval.End})";
        }
    }

    public class OverlappingIntervals : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Intervals Intervals { get; }
        public int Day { get; }

        public OverlappingIntervals(Intervals intervals, int day)
        {
            Seriousness = ReportSeriousness.Error;
            Intervals = intervals;
            Day = day;
        }

        public override string GetReportMessage()
        {
            var message = Intervals.Aggregate("Intervals ",
                (current, interval) => current + $"({interval.Start}, {interval.End}) ");

            return $"{message}overlap on day {Day}";
        }
    }

    /**
        For now won't be used, TODO: Rethink validation of inputs for intervals (2-3) (4-5) and such
         */
    public class ConsecutiveIntervals : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Interval First { get; }
        public Interval Second { get; }
        
        public ConsecutiveIntervals(Interval first, Interval second)
        {
            Seriousness = ReportSeriousness.Warning;
            First = first;
            Second = second;
        }

        public override string GetReportMessage()
        {
            return $"Intervals ({First.Start}, {First.End}) and ({Second.Start}, {Second.End}) " +
                   $"can be concatenated into a single interval";
        }
    }
}