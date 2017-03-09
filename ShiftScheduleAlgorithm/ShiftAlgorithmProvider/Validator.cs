using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Entities.Helpers;
using ShiftScheduleData.Entities.NewEntities.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{   
    // TODO: Unit testing
    internal class Validator
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public ResultingScheduleOld AlgorithmOutput { get; }

        private readonly AlgorithmReport _resultAlgorithmReport;

        public Validator(ShiftAlgorithm.Input algorithmInput, ResultingScheduleOld algorithmOutput)
        {
            AlgorithmInput = algorithmInput;
            AlgorithmOutput = algorithmOutput;
            _resultAlgorithmReport = new AlgorithmReport();
        }

        public AlgorithmReport Validate()
        {
            CheckMaxMonthlyWorkNotMet();
            CheckMaxDailyWorkNotMet();
            CheckWorkerPauseLengthNotMet();
            CheckMaxConsecutiveWorkHoursNotMet();
            CheckPersonScheduleRequirementsNotMet();
            CheckRequirementsAreNotMet();
            CheckOverlappingIntervals();

            return _resultAlgorithmReport;
        }

        private void CheckMaxMonthlyWorkNotMet()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                var person = personSchedule.Key;
                var schedule = personSchedule.Value;

                int monthlySchedule = schedule.DailySchedules.Sum(
                    scheduleDailySchedule => scheduleDailySchedule.Value.GetLengthInTime());

                if (monthlySchedule > person.MaxHoursPerMonth)
                    _resultAlgorithmReport.MaxMonthlyWork.Add(new MaxMonthlyWorkNotMet(person));
            }
        }

        private void CheckMaxDailyWorkNotMet()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                PersonOld personOld = personSchedule.Key;
                Schedule schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    if (scheduleDailySchedule.Value.GetLengthInTime() >
                        AlgorithmInput.AlgorithmConfiguration.MaxDailyWork)
                    {
                        _resultAlgorithmReport.MaxDailyWork.Add(
                            new MaxDailyWorkNotMet(personOld, scheduleDailySchedule.Key));
                    }
                }
            }
        }

        // TODO: ask about how this system works
        private void CheckWorkerPauseLengthNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckMaxConsecutiveWorkHoursNotMet()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                PersonOld personOld = personSchedule.Key;
                Schedule schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    IntervalsOld sortedIntervalsOld = IntervalsOld.MergeAndSort(scheduleDailySchedule.Value);

                    if (
                        sortedIntervalsOld.Any(
                            interval => interval.Count > AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours))
                    {
                        _resultAlgorithmReport.MaxConsecutiveWorkHours.Add(
                            new MaxConsecutiveWorkHoursNotMet(personOld, scheduleDailySchedule.Key));
                    }
                }
            }
        }

        // TODO: make it more effective (merge compare?), this is a stupid version, and finish it
        private void CheckPersonScheduleRequirementsNotMet()
        {
            foreach (var personToMonthlySchedule in AlgorithmOutput.SchedulesForPeople)
            {
                PersonOld personOld = personToMonthlySchedule.Key;
                Schedule schedule = personToMonthlySchedule.Value;

                foreach (var personDailyScheduleOutput in schedule.DailySchedules)
                {
                    var day = personDailyScheduleOutput.Key;

                    IntervalsOld scheduledIntervalsOld = IntervalsOld.MergeAndSort(personDailyScheduleOutput.Value);
                    IntervalsOld personDailyScheduleRequirement = IntervalsOld.MergeAndSort(
                        personOld.Schedule.DailySchedules[day]);

                    foreach (Interval interval in scheduledIntervalsOld)
                    {
                        if (!personDailyScheduleRequirement.ContainsSubInterval(interval))
                        {
                            _resultAlgorithmReport.PersonScheduleRequirements.Add(
                                new PersonScheduleRequirementsNotMet(personOld, day, interval));
                        }
                    }
                }
            }
        }

        private void CheckRequirementsAreNotMet()
        {
            var tempRequirements = new MonthlyRequirements(AlgorithmInput.MonthlyRequirements.DaysToRequirements);

            foreach (var personAndMonthlySchedule in AlgorithmOutput.SchedulesForPeople)
            {
                // which personOld it is is irrelevant for this
                Schedule personSchedule = personAndMonthlySchedule.Value;

                foreach (var dayAndDailySchedule in personSchedule.DailySchedules)
                {
                    IntervalsOld intervalsOld = dayAndDailySchedule.Value;

                    foreach (var interval in intervalsOld)
                    {
                        for (var i = interval.Start; i <= interval.End; i++)
                        {
                            // substracts a worker from monthlyRequirements for each hour in the interval
                            tempRequirements.DaysToRequirements[i].HourToWorkers[i]--;
                        }
                    }
                }
            }

            // check whether there's enough workers for each hour
            // if there is enough workers, more or equal workers were substracted from hourly monthlyRequirements 
            foreach (var dayAndRequirement in tempRequirements.DaysToRequirements)
            {
                var day = dayAndRequirement.Key;
                var dailyRequirement = dayAndRequirement.Value;

                foreach (var hourAndWorkers in dailyRequirement.HourToWorkers)
                {
                    var hour = hourAndWorkers.Value;
                    var workersCount = hourAndWorkers.Key;

                    if (workersCount > 0)
                    {
                        _resultAlgorithmReport.Requirements.Add(new RequirementsAreNotMet(day, hour));
                    }
                }
            }
        }

        private void CheckOverlappingIntervals()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                PersonOld personOld = personSchedule.Key;
                Schedule schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    var sortedIntervals = new IntervalsOld(scheduleDailySchedule.Value.IntervalsList);
                    // no need for a merge
                    sortedIntervals.SortByStart();

                    var previousInterval = new Interval(-1, -1);
                    var overlappingIntervals = new HashSet<Interval>();

                    foreach (var interval in sortedIntervals)
                    {
                        if (interval.Overlaps(previousInterval))
                        {
                            overlappingIntervals.Add(previousInterval);
                            overlappingIntervals.Add(interval);
                        }
                        else
                        {
                            _resultAlgorithmReport.OverlappingIntervals.Add(
                                new OverlappingIntervals(new IntervalsOld(overlappingIntervals.ToList()),
                                    scheduleDailySchedule.Key));
                            overlappingIntervals = new HashSet<Interval>();
                        }
                        previousInterval = interval;
                    }
                }
            }
        }
    }
}