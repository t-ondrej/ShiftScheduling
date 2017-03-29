using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Validation;
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

        private static ResultingSchedule GetOutput()
        {
            // Shift Interval Lists

            // person1
            // improper pause scheduling 

            var interval1P1 = new ShiftInterval(0, 4, ShiftInterval.IntervalType.Work);

            var intervalList1P1 = new List<ShiftInterval> {interval1P1};

            var interval2P1 = new ShiftInterval(0, 4, ShiftInterval.IntervalType.Pause);

            var intervalList2P1 = new List<ShiftInterval> {interval2P1};

            // person2 
            // overlapping, consecutive intervals, unnecessary pause, requirements not met (hour 3 day 1)
            var interval1P2 = new ShiftInterval(0, 2, ShiftInterval.IntervalType.Work);
            var interval2P2 = new ShiftInterval(2, 2, ShiftInterval.IntervalType.Work);
            var interval3P2 = new ShiftInterval(3, 3, ShiftInterval.IntervalType.Work);

            var intervalList1P2 = new List<ShiftInterval> {interval1P2, interval2P2, interval3P2};

            var interval4P2 = new ShiftInterval(0, 0, ShiftInterval.IntervalType.Work);
            var interval5P2 = new ShiftInterval(1, 1, ShiftInterval.IntervalType.Pause);
            var interval6P2 = new ShiftInterval(2, 2, ShiftInterval.IntervalType.Work);

            var intervalList2P2 = new List<ShiftInterval> {interval4P2, interval5P2, interval6P2};

            // day to shift interval list

            var personIdToDailySchedule1 = new Dictionary<int, Intervals<ShiftInterval>>
            {
                [0] = new Intervals<ShiftInterval>(intervalList1P1),
                [1] = new Intervals<ShiftInterval>(intervalList1P2)
            };

            var personIdToDailySchedule2 = new Dictionary<int, Intervals<ShiftInterval>>
            {
                [0] = new Intervals<ShiftInterval>(intervalList2P1),
                [1] = new Intervals<ShiftInterval>(intervalList2P2)
            };

            var dayToDailySchedule = new Dictionary<int, ResultingSchedule.DailySchedule>
            {
                [0] = new ResultingSchedule.DailySchedule(personIdToDailySchedule1),
                [1] = new ResultingSchedule.DailySchedule(personIdToDailySchedule2)
            };

            var resultingSchedule = new ResultingSchedule(dayToDailySchedule);

            return resultingSchedule;
        }

        private static AlgorithmInput GetInput()
        {
            // persons
            // creation

            var dayToDailyAvailibility1 = new Dictionary<int, Person.DailyAvailability>
            {
                [0] = new Person.DailyAvailability(new Interval(0, 4), 1, 1, 1),
                [1] = null
            };

            var person0 = new Person(0, 5, dayToDailyAvailibility1);

            var dayToDailyAvailibility2 = new Dictionary<int, Person.DailyAvailability>
            {
                [0] = new Person.DailyAvailability(new Interval(0, 2), 1, 1, 1),
                [1] = new Person.DailyAvailability(new Interval(0, 2), 1, 1, 1)
            };

            var person1 = new Person(1, 6, dayToDailyAvailibility2);

            // addition

            var persons = new List<Person> {person0, person1};

            // requirements

            IList<double> hourToWorkers0 = new List<double> {2, 2, 1, 0, 0, 0};
            IList<double> hourToWorkers1 = new List<double> {1, 1, 1, 1, 0, 0};

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