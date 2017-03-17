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
    internal class Validator
    {
        public AlgorithmInput AlgorithmInput { get; }

        public ResultingSchedule AlgorithmOutput { get; }

        private readonly AlgorithmValidationResult _resultAlgorithmValidationResult;

        private readonly IDictionary<int, Person> _idToPersons;

        public Validator(AlgorithmInput algorithmInput, ResultingSchedule algorithmOutput)
        {
            AlgorithmInput = algorithmInput;
            AlgorithmOutput = algorithmOutput;
            _resultAlgorithmValidationResult = new AlgorithmValidationResult();
            _idToPersons = algorithmInput.Persons.ToDictionary(p => p.Id, p => p);
        }

        public AlgorithmValidationResult Validate()
        {
            CheckMaxMonthlyWorkNotMet();
            CheckMaxDailyWorkNotMet();
            CheckWorkerPauseLengthNotMet();
            CheckMaxConsecutiveWorkHoursNotMet();
            CheckRequirementsAreNotMet();
            CheckOverlappingIntervals();

            return _resultAlgorithmValidationResult;
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
                var dailyWorkTime = schedule.GetLengthInTime();

                if (dailyWorkTime > person.MaxWork)
                    _resultAlgorithmValidationResult.AddReport(new MaxDailyWorkNotMet(person, day));
            });
        }

        private void CheckMaxMonthlyWorkNotMet()
        {
            var personToMonthlyTime = new Dictionary<Person, int>();

            // map persons montly work he got assigned
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var dailyWorkTime = schedule.GetLengthInTime();

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
                    _resultAlgorithmValidationResult.AddReport(new MaxMonthlyWorkNotMet(personToTime.Key));
                }
            }
        }

        private void CheckMaxConsecutiveWorkHoursNotMet()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var tempIntervals = new Intervals<ShiftInterval>(schedule.IntervalsList);
                Intervals<ShiftInterval>.MergeAndSort(tempIntervals);

                foreach (var shiftInterval in schedule.IntervalsList)
                {
                    if (shiftInterval.Type == ShiftInterval.IntervalType.Pause) continue;

                    if (shiftInterval.Count > AlgorithmInput.AlgorithmConfiguration.MaxConsecutiveWorkHours)
                    {
                        _resultAlgorithmValidationResult.AddReport(new MaxConsecutiveWorkHoursNotMet(person, day));
                    }
                }
            });
        }

        private void CheckRequirementsAreNotMet()
        {
            var tempRequirements = new Requirements(AlgorithmInput.Requirements.DaysToRequirements);

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

                        // substracts done work from each assigned requirement
                        hourToWorkers[hour] -= shiftWeight;
                    }
                }
            });

            // if there is enough workers, more or equal worker shift weight were 
            // substracted from hourly monthlyRequirementsOld 
            foreach (var tempRequirement in tempRequirements.DaysToRequirements)
            {
                int day = tempRequirement.Key;
                IList<double> hourToWorkers = tempRequirement.Value.HourToWorkers;

                hourToWorkers.ForEach((workers, hour) =>
                {
                    if (workers > 0)
                    {
                        _resultAlgorithmValidationResult.AddReport(new RequirementsAreNotMet(day, hour));
                    }
                });
            }
        }

        private void CheckOverlappingIntervals()
        {
            IterateAlgorithmOutput((person, schedule, day) =>
            {
                var tempIntervals = new Intervals<ShiftInterval>(schedule.IntervalsList);
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
                        var reportIntervals = new Intervals<ShiftInterval>(tempIntervals.ToList());

                        _resultAlgorithmValidationResult.AddReport(new OverlappingIntervals(reportIntervals, day));

                        overlappingIntervals = new HashSet<ShiftInterval>();
                    }
                    previousInterval = interval;
                }
            });
        }

        private void CheckWorkerPauseLengthNotMet()
        {
            throw new NotImplementedException();
        }
    }
}