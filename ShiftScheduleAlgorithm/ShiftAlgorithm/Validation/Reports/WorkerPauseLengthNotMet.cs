﻿using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class WorkerPauseLengthNotMet : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Person Person { get; }
        public int Day { get; }

        public WorkerPauseLengthNotMet(Person person, int day)
        {
            ReportSeriousness = Seriousness.Error;
            Person = person;
            Day = day;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} works more than he can without a pause on day {Day}";
        }
    }
}