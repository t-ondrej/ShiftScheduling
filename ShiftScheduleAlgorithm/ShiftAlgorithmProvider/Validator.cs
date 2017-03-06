using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    internal class Validator
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public ResultingSchedule AlgorithmOutput { get; }

        private readonly AlgorithmReport _resultAlgorithmReport;

        public Validator(ShiftAlgorithm.Input algorithmInput, ResultingSchedule algorithmOutput)
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
                var person = personSchedule.Key;
                var schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    if (scheduleDailySchedule.Value.GetLengthInTime() >
                        AlgorithmInput.AlgorithmConfiguration.MaxDailyWork)
                    {
                        _resultAlgorithmReport.MaxDailyWork.Add(
                            new MaxDailyWorkNotMet(person, scheduleDailySchedule.Key));
                    }
                }
            }
        }

        private void CheckWorkerPauseLengthNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckMaxConsecutiveWorkHoursNotMet()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                var person = personSchedule.Key;
                var schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    var foundConsecutiveInterval = false;
                    var firstIntervalStart = -1;
                    var previousIntervalEnd = -1;

                    foreach (var interval in scheduleDailySchedule.Value)
                    {
                        if (interval.Count > AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours)
                        {
                            _resultAlgorithmReport.MaxConsecutiveWorkHours.Add(
                                new MaxConsecutiveWorkHoursNotMet(person, scheduleDailySchedule.Key));
                            break;
                        }

                        if (!foundConsecutiveInterval)
                        {
                            firstIntervalStart = interval.Start;
                            previousIntervalEnd = interval.End;
                        }

                        if (interval.Start == previousIntervalEnd + 1)
                        {
                            foundConsecutiveInterval = true;
                            previousIntervalEnd = interval.End;

                            if (previousIntervalEnd - firstIntervalStart + 1 <=
                                AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours) continue;

                            _resultAlgorithmReport.MaxConsecutiveWorkHours.Add(
                                new MaxConsecutiveWorkHoursNotMet(person, scheduleDailySchedule.Key));

                            break;
                        }
                        foundConsecutiveInterval = false;
                    }
                }
            }
        }

        private void CheckPersonScheduleRequirementsNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckRequirementsAreNotMet()
        {
            throw new NotImplementedException();
        }

        // TODO: sort intervals by time
        private void CheckOverlappingIntervals()
        {
            foreach (var personSchedule in AlgorithmOutput.SchedulesForPeople)
            {
                var person = personSchedule.Key;
                var schedule = personSchedule.Value;

                foreach (var scheduleDailySchedule in schedule.DailySchedules)
                {
                    var previousInterval = new Interval(-1, -1);
                    var overlappingIntervals = new HashSet<Interval>();

                    foreach (var interval in scheduleDailySchedule.Value)
                    {
                        if (interval.Start <= previousInterval.End)
                        {
                            overlappingIntervals.Add(previousInterval);
                            overlappingIntervals.Add(interval);
                        }
                        else
                        {
                            _resultAlgorithmReport.OverlappingIntervals.Add(
                                new OverlappingIntervals(new Intervals(overlappingIntervals.ToList()),
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