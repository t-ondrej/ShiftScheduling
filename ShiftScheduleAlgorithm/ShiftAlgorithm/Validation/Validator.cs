using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Validation.Reports;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation
{
    // TODO: Unit testing
    public class Validator
    {
        public AlgorithmInput AlgorithmInput { get; }

        public ResultingSchedule AlgorithmOutput { get; }

        private readonly AlgorithmValidationResult _algorithmValidationResult;

        private readonly IDictionary<int, Person> _idToPersons;

        public Validator(AlgorithmInput algorithmInput, ResultingSchedule algorithmOutput)
        {
            AlgorithmInput = algorithmInput;
            AlgorithmOutput = algorithmOutput;
            _algorithmValidationResult = new AlgorithmValidationResult();
            _idToPersons = algorithmInput.Persons.ToDictionary(p => p.Id, p => p);
        }

        public AlgorithmValidationResult Validate()
        {
            CheckMaxMonthlyWorkNotMet();
            CheckMaxDailyWorkNotMet();
            CheckMaxConsecutiveWorkHoursNotMet();
            CheckRequirementsAreNotMet();
            CheckOverlappingIntervals();
            CheckPauseScheduling();

            return _algorithmValidationResult;
        }

        public void IterateAlgorithmOutput(Action<Person, Intervals<ShiftInterval>, int> action)
        {
            foreach (var dayToSchedule in AlgorithmOutput.DailySchedules)
            {
                var day = dayToSchedule.Key;
                foreach (var personSchedule in dayToSchedule.Value.PersonIdToDailySchedule)
                {
                    Person person = _idToPersons[personSchedule.Key];
                    var scheduleInIntervals = personSchedule.Value;
                    action(person, scheduleInIntervals, day);
                }
            }
        }

        public void IterateAlgorithmOutput(Action<Person, Intervals<ShiftInterval>> action)
        {
            IterateAlgorithmOutput((person, intervals, day) => action(person, intervals));
        }

        private void CheckMaxDailyWorkNotMet()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var dailyWorkTime = schedule.GetLength();
                
                if (dailyWorkTime > AlgorithmInput.AlgorithmConfiguration.MaxDailyWork)
                    _algorithmValidationResult.AddReport(new MaxDailyWorkNotMet(person, day));
            });
        }

        private void CheckMaxMonthlyWorkNotMet()
        {
            var personToMonthlyTime = new Dictionary<Person, int>();

            // map persons montly work he got assigned
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var dailyWorkTime = schedule.GetLength();

                if (!personToMonthlyTime.ContainsKey(person))
                {
                    personToMonthlyTime.Add(person, dailyWorkTime);
                }
                else
                {
                    personToMonthlyTime[person] += dailyWorkTime;
                }
            });

            // check if the amount of work is corresponding to his demands
            // must be equal, not more, not less
            foreach (var personToTime in personToMonthlyTime)
            {
                if (personToTime.Value != personToTime.Key.MaxWork)
                {
                    _algorithmValidationResult.AddReport(new MaxMonthlyWorkNotMet(personToTime.Key));
                }
            }
        }

        private void CheckMaxConsecutiveWorkHoursNotMet()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var tempIntervals  = Intervals<ShiftInterval>.MergeAndSort(schedule);

                foreach (var shiftInterval in tempIntervals)
                {
                    if (shiftInterval.Type == ShiftInterval.IntervalType.Pause) continue;

                    if (shiftInterval.Count > AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours)
                    {
                        _algorithmValidationResult.AddReport(new MaxConsecutiveWorkHoursNotMet(person, day));
                    }
                }
            });
        }

        private void CheckRequirementsAreNotMet()
        {
            var tempRequirements = new Requirements(
                new Dictionary<int, Requirements.DailyRequirement>(
                    AlgorithmInput.Requirements.DaysToRequirements));

            IterateAlgorithmOutput((person, schedule, day) =>
            {
                foreach (var shiftInterval in schedule)
                {
                    if (shiftInterval.Type == ShiftInterval.IntervalType.Pause)
                    {
                        continue;
                    }
                    foreach (var hour in shiftInterval)
                    {
                        var hourToWorkers = tempRequirements.DaysToRequirements[day].HourToWorkers;
                        var shiftWeight = person.DailyAvailabilities[day].ShiftWeight;

                        // TODO: validate whether there's longer output than input
                        // substracts done work from each assigned requirement
                        hourToWorkers[hour] -= shiftWeight;
                    }
                }
            });

            // if there is enough workers, more or equal worker shift weight were 
            // substracted from hourly monthlyRequirements
            foreach (var tempRequirement in tempRequirements.DaysToRequirements)
            {
                int day = tempRequirement.Key;
                IList<double> hourToWorkers = tempRequirement.Value.HourToWorkers;

                hourToWorkers.ForEach((workers, hour) =>
                {
                    if (workers > 0)
                    {
                        _algorithmValidationResult.AddReport(new RequirementsAreNotMet(day, hour));
                    }
                });
            }
        }

        private void CheckOverlappingIntervals()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var tempIntervals = new Intervals<ShiftInterval>(new List<ShiftInterval>(schedule.IntervalsList));
                tempIntervals.SortByStart();

                var previousInterval = new ShiftInterval(-1, -1, 0);
                var overlappingIntervals = new HashSet<ShiftInterval>();

                foreach (var interval in tempIntervals)
                {
                    if (interval.Overlaps(previousInterval))
                    {
                        overlappingIntervals.Add(previousInterval);
                        overlappingIntervals.Add(interval);
                    } 
                    else
                    {
                        if (overlappingIntervals.Count > 0)
                        {
                            var reportIntervals = new Intervals<ShiftInterval>(overlappingIntervals.ToList());

                            _algorithmValidationResult.AddReport(new OverlappingIntervals(reportIntervals, day));

                            overlappingIntervals = new HashSet<ShiftInterval>();
                        }
                    }
                    previousInterval = interval;
                }
            });
        }

        private void CheckPauseScheduling()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var intervals = new Intervals<ShiftInterval>(schedule.IntervalsList);
                intervals.SortByStart();

                switch (intervals.IntervalsList.Count)
                {
                    case 0:
                        // do nothing
                        break;
                    case 1:
                        if (IsPause(intervals.IntervalsList[0]))
                        {
                            _algorithmValidationResult.AddReport(new ImproperPauseScheduling(person, day));
                        }
                        break;
                    case 2:
                        if (IsPause(intervals.IntervalsList[0]) || IsPause(intervals.IntervalsList[1]))
                        {
                            _algorithmValidationResult.AddReport(new ImproperPauseScheduling(person, day));
                        }
                        break;
                    case 3:
                        ShiftInterval middleInterval = intervals.IntervalsList[1];

                        if (IsPause(intervals.IntervalsList[0]) || IsPause(intervals.IntervalsList[2]))
                        {
                            _algorithmValidationResult.AddReport(new ImproperPauseScheduling(person, day));
                        }

                        if (IsWork(middleInterval))
                        {
                            if (intervals.GetLength() >
                                AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours)
                            {
                                _algorithmValidationResult.AddReport(new ImproperPauseScheduling(person, day));
                            }
                        }

                        if (IsPause(middleInterval))
                        {
                            if (intervals.GetLength() <
                                AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours)
                            {
                                _algorithmValidationResult.AddReport(new UnnecessaryPauseScheduling(person, day));
                            }

                            if (middleInterval.Count != AlgorithmInput.AlgorithmConfiguration.WorkerPauseLength)
                            {
                                _algorithmValidationResult.AddReport(new WorkerPauseLengthNotMet(person, day));
                            }
                        }
                        break;
                }
            });
        }

        private static bool IsPause(ShiftInterval interval)
        {
            return interval.Type == ShiftInterval.IntervalType.Pause;
        }

        private static bool IsWork(ShiftInterval interval)
        {
            return interval.Type == ShiftInterval.IntervalType.Work;
        }
    }
}