using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleDataAccess.OldEntities;
using ShiftScheduleLibrary.Utilities;

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

            if (!_reportsDictionary.ContainsKey(type))
                return null;

            return _reportsDictionary[type].Cast<T>();
        }

        public void AddReport(Report report)
        {
            var type = report.GetType();

            if (!_reportsDictionary.ContainsKey(type))
                _reportsDictionary.Add(type, new List<Report>());

            _reportsDictionary[type].Add(report);
        }

        //public List<MaxMonthlyWorkNotMet> MaxMonthlyWork { get; }
        //public List<MaxDailyWorkNotMet> MaxDailyWork { get; }
        //public List<WorkerPauseLengthNotMet> WorkerPauseLenth { get; }
        //public List<MaxConsecutiveWorkHoursNotMet> MaxConsecutiveWorkHours { get; }
        //public List<PersonScheduleRequirementsNotMet> PersonScheduleRequirements { get; }
        //public List<RequirementsAreNotMet> Requirements { get; }
        //public List<OverlappingIntervals> OverlappingIntervals { get; }
        //public List<ConsecutiveIntervals> ConsecutiveIntervals { get; }

        //public AlgorithmReport()
        //{
        //    MaxMonthlyWork = new List<MaxMonthlyWorkNotMet>();
        //    MaxDailyWork = new List<MaxDailyWorkNotMet>();
        //    WorkerPauseLenth = new List<WorkerPauseLengthNotMet>();
        //    MaxConsecutiveWorkHours = new List<MaxConsecutiveWorkHoursNotMet>();
        //    PersonScheduleRequirements = new List<PersonScheduleRequirementsNotMet>();
        //    Requirements = new List<RequirementsAreNotMet>();
        //    OverlappingIntervals = new List<OverlappingIntervals>();
        //    ConsecutiveIntervals = new List<ConsecutiveIntervals>();
        //}
    }

    public class MaxMonthlyWorkNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public PersonOld PersonOld { get; }

        public MaxMonthlyWorkNotMet(PersonOld personOld)
        {
            Seriousness = ReportSeriousness.Error;
            PersonOld = personOld;
        }

        public override string GetReportMessage()
        {
            return $"PersonOld {PersonOld.Id} works more than he can without in a month";
        }
    }

    public class MaxDailyWorkNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public PersonOld PersonOld { get; }
        public int Day { get; }

        public MaxDailyWorkNotMet(PersonOld personOld, int day)
        {
            Seriousness = ReportSeriousness.Error;
            PersonOld = personOld;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"PersonOld {PersonOld.Id} works more than he can without a pause on day {Day}";
        }
    }

    public class WorkerPauseLengthNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public PersonOld PersonOld { get; }
        public int Day { get; }

        public WorkerPauseLengthNotMet(PersonOld personOld, int day)
        {
            Seriousness = ReportSeriousness.Error;
            PersonOld = personOld;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"PersonOld {PersonOld.Id} works more than he can without a pause on day {Day}";
        }
    }

    public class MaxConsecutiveWorkHoursNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public PersonOld PersonOld { get; }
        public int Day { get; }

        public MaxConsecutiveWorkHoursNotMet(PersonOld personOld, int day)
        {
            Seriousness = ReportSeriousness.Error;
            PersonOld = personOld;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"PersonOld {PersonOld.Id} works more than maximum consecutive working " +
                   $"hours on day {Day}";
        }
    }

    public class PersonScheduleRequirementsNotMet : Report
    {
        public override ReportSeriousness Seriousness { get; }

        public PersonOld PersonOld { get; }
        public int Day { get; }
        public Interval Interval { get; }

        public PersonScheduleRequirementsNotMet(PersonOld personOld, int day, Interval interval)
        {
            Seriousness = ReportSeriousness.Error;
            PersonOld = personOld;
            Day = day;
            Interval = interval;
        }

        public override string GetReportMessage()
        {
            return $"PersonOld {PersonOld.Id} works out of his AlgorithmOutput on day {Day} " +
                   $"in hour ({Interval.Start}, {Interval.End})";
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

        public Intervals<Interval> IntervalsOld { get; }
        public int Day { get; }

        public OverlappingIntervals(Intervals<Interval> intervalsOld, int day)
        {
            Seriousness = ReportSeriousness.Error;
            IntervalsOld = intervalsOld;
            Day = day;
        }

        public override string GetReportMessage()
        {
            var message = IntervalsOld.Aggregate("IntervalsOld ",
                (current, interval) => current + $"({interval.Start}, {interval.End}) ");

            return $"{message}overlap on day {Day}";
        }
    }

    /**
        For now won't be used, TODO: Rethink validation of inputs for intervalsOld (2-3) (4-5) and such
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
            return $"IntervalsOld ({First.Start}, {First.End}) and ({Second.Start}, {Second.End}) " +
                   $"can be concatenated into a single hour";
        }
    }
}