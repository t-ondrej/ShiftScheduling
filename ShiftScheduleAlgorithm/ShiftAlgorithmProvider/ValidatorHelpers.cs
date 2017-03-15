using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleDataAccess.OldEntities;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;
using static ShiftScheduleLibrary.Entities.ResultingSchedule;

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
        private readonly IDictionary<Type, IList<Report>> _reportsDictionary;

        public AlgorithmReport()
        {
            _reportsDictionary = new Dictionary<Type, IList<Report>>();
        }

        public IEnumerable<T> GetReports<T>() where T : Report
        {
            var type = typeof(T);

            return !_reportsDictionary.ContainsKey(type) ? null : _reportsDictionary[type].Cast<T>();
        }

        public void AddReport(Report report)
        {
            var type = report.GetType();

            if (!_reportsDictionary.ContainsKey(type))
                _reportsDictionary.Add(type, new List<Report>());

            _reportsDictionary[type].Add(report);
        }
    }


    //MaxMonthlyWorkNotMet, MaxDailyWorkNotMet, WorkerPauseLengthNotMet,
    //MaxConsecutiveWorkHoursNotMet, RequirementsAreNotMet

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

    public class RequirementsAreNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public int Day { get; }
        public int Hour { get; }

        public RequirementsAreNotMet(int day, int hour)
        {
            Seriousness = ReportSeriousness.Error;
            Day = day;
            Hour = hour;
        }

        public override string GetReportMessage()
        {
            return $"Insufficient workers for day {Day} at hour {Hour}";
        }
    }

    public class OverlappingIntervals : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public Intervals<ShiftInterval> Intervals { get; }
        public int Day { get; }

        public OverlappingIntervals(Intervals<ShiftInterval> intervals, int day)
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
    //public class ConsecutiveIntervals : Report
    //{
    //    public override ReportSeriousness Seriousness { get; }

    //    public Interval First { get; }
    //    public Interval Second { get; }

    //    public ConsecutiveIntervals(Interval first, Interval second)
    //    {
    //        Seriousness = ReportSeriousness.Warning;
    //        First = first;
    //        Second = second;
    //    }

    //    public override string GetReportMessage()
    //    {
    //        return $"Intervals ({First.Start}, {First.End}) and ({Second.Start}, {Second.End}) " +
    //               $"can be concatenated into a single hour";
    //    }
    //}
}