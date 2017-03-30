using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Validation;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithmTest.ShiftAlgorithm.Validation
{
    [TestClass]
    public class ValidatorTest
    {
        private Validator _validator;

        [TestInitialize]
        public void Init()
        {
            AlgorithmInput algorithmInput = GetInput();
            ResultingSchedule algorithmOutput = GetOutput();

            _validator = new Validator(algorithmInput, algorithmOutput);
        }

        [TestMethod]
        public void TestValidate()
        {
            var validationMessage = _validator.Validate().GetMessage();

            Trace.WriteLine(validationMessage);

            Assert.AreNotEqual(validationMessage, null);
        }

        [TestMethod]
        public void TestRequirementsNotMet()
        {
            var requirement = _validator.Validate().GetReports<RequirementsAreNotMet>();

            Assert.AreEqual(requirement.Count(), 2);
        }

        [TestMethod]
        public void TestMaxMonthlyWorkNotMet()
        {
            var monthlyWork = _validator.Validate().GetReports<MaxMonthlyWorkNotMet>();

            Assert.AreEqual(monthlyWork.Count(), 1);
        }

        [TestMethod]
        public void TestMaxDailyWorkNotMet()
        {
            var dailyWork = _validator.Validate().GetReports<MaxDailyWorkNotMet>();

            Assert.AreEqual(dailyWork.Count(), 3);
        }

        [TestMethod]
        public void TestMaxConsecutiveWorkHoursNotMet()
        {
            var consecutiveHours = _validator.Validate().GetReports<MaxConsecutiveWorkHoursNotMet>();

            Assert.AreEqual(consecutiveHours.Count(), 1);
        }

        [TestMethod]
        public void TestOverlappingIntervals()
        {
            var overlappingIntervals = _validator.Validate().GetReports<OverlappingIntervals>();

            Assert.AreEqual(overlappingIntervals.Count(), 1);
        }

        private ResultingSchedule GetOutput()
        {
            // Shift Interval Lists

            // person0
            // improper pause scheduling 

            var interval1P0 = new ShiftInterval(0, 4, ShiftInterval.IntervalType.Work);

            var intervalList1P0 = new List<ShiftInterval> { interval1P0 };

            var interval2P0 = new ShiftInterval(0, 4, ShiftInterval.IntervalType.Pause);

            var intervalList2P0 = new List<ShiftInterval> { interval2P0 };

            // person1 
            // overlapping, consecutive intervals, unnecessary pause, requirements not met (hour 3 day 1)
            var interval1P1 = new ShiftInterval(0, 2, ShiftInterval.IntervalType.Work);
            var interval2P1 = new ShiftInterval(2, 2, ShiftInterval.IntervalType.Work);
            var interval3P1 = new ShiftInterval(3, 3, ShiftInterval.IntervalType.Work);

            var intervalList1P1 = new List<ShiftInterval> { interval1P1, interval2P1, interval3P1 };

            var interval4P1 = new ShiftInterval(0, 0, ShiftInterval.IntervalType.Work);
            var interval5P1 = new ShiftInterval(1, 1, ShiftInterval.IntervalType.Pause);
            var interval6P1 = new ShiftInterval(2, 2, ShiftInterval.IntervalType.Work);

            var intervalList2P1 = new List<ShiftInterval> { interval4P1, interval5P1, interval6P1 };

            // day to shift interval list

            var personIdToDailySchedule0 = new Dictionary<int, Intervals<ShiftInterval>>
            {
                [0] = new Intervals<ShiftInterval>(intervalList1P0),
                [1] = new Intervals<ShiftInterval>(intervalList1P1)
            };

            var personIdToDailySchedule1 = new Dictionary<int, Intervals<ShiftInterval>>
            {
                [0] = new Intervals<ShiftInterval>(intervalList2P0),
                [1] = new Intervals<ShiftInterval>(intervalList2P1)
            };

            var dayToDailySchedule = new Dictionary<int, ResultingSchedule.DailySchedule>
            {
                [0] = new ResultingSchedule.DailySchedule(personIdToDailySchedule0),
                [1] = new ResultingSchedule.DailySchedule(personIdToDailySchedule1)
            };

            var resultingSchedule = new ResultingSchedule(dayToDailySchedule);

            return resultingSchedule;
        }

        private AlgorithmInput GetInput()
        {
            // persons
            // creation

            var dayToDailyAvailibility1 = new Dictionary<int, Person.DailyAvailability>
            {
                [0] = new Person.DailyAvailability(new Interval(0, 4), 1, 1, 1),
                [1] = null
            };

            var person0 = new Person(0, 10, dayToDailyAvailibility1);

            var dayToDailyAvailibility2 = new Dictionary<int, Person.DailyAvailability>
            {
                [0] = new Person.DailyAvailability(new Interval(0, 2), 1, 1, 1),
                [1] = new Person.DailyAvailability(new Interval(0, 2), 1, 1, 1)
            };

            var person1 = new Person(1, 6, dayToDailyAvailibility2);

            // addition

            var persons = new List<Person> { person0, person1 };

            // requirements

            IList<double> hourToWorkers0 = new List<double> { 2, 2, 1, 0, 0, 0 };
            IList<double> hourToWorkers1 = new List<double> { 1, 1, 1, 1, 0, 0 };

            var dailyRequirement0 = new Requirements.DailyRequirement(hourToWorkers0);
            var dailyRequirement1 = new Requirements.DailyRequirement(hourToWorkers1);

            var dayToDailyRequirement = new Dictionary<int, Requirements.DailyRequirement>
            {
                [0] = dailyRequirement0,
                [1] = dailyRequirement1
            };

            var requirements = new Requirements(dayToDailyRequirement);

            // configuration

            var algorithmConfiguration = new AlgorithmConfiguration
            {
                WorkerPauseLength = 1,
                MaxConsecutiveWorkHours = 3,
                MaxDailyWork = 3
            };

            // requirements fulfilling stats 

            var requirementsFulfillingStats = new RequirementsFulfillingStats(null);

            // algorithm input

            var algorithmInput = new AlgorithmInput(persons, requirements, requirementsFulfillingStats,
                algorithmConfiguration);

            return algorithmInput;
        }
    }
}