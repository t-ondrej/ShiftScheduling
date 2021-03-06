﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class TimeUnitsManager
    {
        public AlgorithmInput AlgorithmInput { get; }

        public List<TimeUnit> AllTimeUnits { get; }

        public List<ScheduledPerson> ScheduledPersons { get; }

        private readonly IntervalsGenerator _intervalsGenerator;

        private readonly IDictionary<int, int> _dayIdToUnitsCount;

        public TimeUnitsManager(AlgorithmInput algorithmInput)
        {
            AlgorithmInput = algorithmInput;
            AllTimeUnits = new List<TimeUnit>();
            ScheduledPersons = new List<ScheduledPerson>();
            var maxNumberOfHours = MaxNumberOfHours();
            _intervalsGenerator = new IntervalsGenerator(maxNumberOfHours, AlgorithmInput.AlgorithmConfiguration);
            _dayIdToUnitsCount = new Dictionary<int, int>();
            CreateTimeUnits();
            CreateScheduledPersons();
            CreateSchedulesForPersons();
        }

        public void AssignScheduleToTimeUnits(ScheduleForDay scheduleForDay)
        {
            var dayId = scheduleForDay.DayId;
            var start = scheduleForDay.Intervals.First().Start;
            var end = scheduleForDay.Intervals.Last().End;

            // Assign the scheduleForDay to the all timeUnits it covers
            for (var unit = start; unit <= end; unit++)
            {
                var timeUnit = AllTimeUnits.Find(t => t.DayId == dayId && t.UnitOfDay == unit);
                timeUnit.AssignSchedule(scheduleForDay);
            }
        }

        private int MaxNumberOfHours()
        {
            return AlgorithmInput.Requirements.DaysToRequirements.Values
                .Select(requirement => requirement.HourToWorkers.Count)
                .Max();
        }

        private void CreateTimeUnits()
        {
            AlgorithmInput.Requirements.DaysToRequirements.ForEach(pair =>
            {
                var dayId = pair.Key;
                var hoursToWorkers = pair.Value.HourToWorkers;
                _dayIdToUnitsCount.Add(dayId, hoursToWorkers.Count);

                pair.Value.HourToWorkers.ForEach((workers, unitId) =>
                {
                    var acuteness = ComputeAcuteness(dayId, unitId);
                    var timeUnit = new TimeUnit(dayId, unitId, workers, acuteness);
                    AllTimeUnits.Add(timeUnit);
                });
            });
        }

        private void CreateScheduledPersons()
        {
            AlgorithmInput.Persons.Select(person =>
            {
                var shiftWeights = new Dictionary<int, double>();

                foreach (var personDailyAvailability in person.DailyAvailabilities)
                {
                    var dayId = personDailyAvailability.Key;

                    if (!_dayIdToUnitsCount.ContainsKey(dayId))
                    {
                        Console.WriteLine("Someone has scheduled a work to a day without requirements");
                    }
                }

                return new ScheduledPerson(person, person.MaxWork, shiftWeights);
            }).ForEach(person => ScheduledPersons.Add(person));

            AddShiftWeights();
        }

        private void AddShiftWeights()
        {
            foreach (var scheduledPerson in ScheduledPersons)
            {
                var shiftWeights = scheduledPerson.ShiftWeights;

                scheduledPerson.Person.DailyAvailabilities
                    .ForEach(pair => shiftWeights.Add(pair.Key, pair.Value.ShiftWeight));

                var shiftedDays = shiftWeights.Keys;

                _dayIdToUnitsCount.Keys.Where(dayId => !shiftedDays.Contains(dayId))
                    .ForEach(dayId => shiftWeights.Add(dayId, 1));
            }
        }

        private void CreateSchedulesForPersons()
        {
            ScheduledPersons.ForEach(person =>
            {
                AlgorithmInput.Requirements.DaysToRequirements.Keys.ForEach(dayId =>
                {
                    var shiftWeight = person.ShiftWeights[dayId];
                    var lengthOfDay = _dayIdToUnitsCount[dayId];

                    var personDailyAvailability = person.Person.DailyAvailabilities[dayId];

                    var personsWorkingDayStart = personDailyAvailability.Availability.Start - personDailyAvailability.LeftTolerance;
                    var personsWorkingDayEnd = personDailyAvailability.Availability.End + personDailyAvailability.RightTolerance;

                    var scheduleForDays = _intervalsGenerator.GetIntervalsWithinBoundaries(personsWorkingDayStart, personsWorkingDayEnd).Select
                    (
                        intervals => new ScheduleForDay(person, dayId, shiftWeight, intervals)
                    );

                    var schedulesForDay = new SchedulesForDay(person, dayId, scheduleForDays.ToList());
                    person.AssignableSchedulesForDays.Add(dayId, schedulesForDay);
                    Debug.WriteLine($"Adding day = {dayId} to person {person.Person.Id}");
                });
            });
        }

        private double ComputeAcuteness(int dayId, int unitId)
        {
            var shiftWeightsSum = AlgorithmInput.Persons
                .Where(person => person.DailyAvailabilities[dayId].Availability.Start - person.DailyAvailabilities[dayId].LeftTolerance <= unitId
                        && person.DailyAvailabilities[dayId].Availability.End + person.DailyAvailabilities[dayId].RightTolerance >= unitId)
                .Sum(person => person.DailyAvailabilities[dayId].ShiftWeight);

            var workersRequirement = AlgorithmInput.Requirements.DaysToRequirements[dayId].HourToWorkers[unitId];

            return workersRequirement > 0 
                ? shiftWeightsSum / workersRequirement 
                : -1;
        }

    }
}