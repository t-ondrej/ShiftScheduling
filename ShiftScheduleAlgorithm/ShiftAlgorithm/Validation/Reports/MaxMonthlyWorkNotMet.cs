﻿using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports
{
    public class MaxMonthlyWorkNotMet : Report
    {
        public override Seriousness ReportSeriousness { get; }

        public Person Person { get; }

        public MaxMonthlyWorkNotMet(Person person)
        {
            ReportSeriousness = Seriousness.Error;
            Person = person;
        }

        public override string GetReportMessage()
        {
            return $"Person {Person.Id} doesn't work the required amount during this month";
        }
    }
}