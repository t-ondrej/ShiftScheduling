using System;
using System.Collections.Generic;
using System.Linq;
using ShiftScheduleDataAccess.OldEntities;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;
using static ShiftScheduleLibrary.Entities.ResultingSchedule;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    // TODO: Unit testing
    internal class Validator
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public ResultingSchedule AlgorithmOutput { get; }

        private readonly AlgorithmReport _resultAlgorithmReport;

        private readonly IDictionary<int, Person> _idToPersons;


        public Validator(ShiftAlgorithm.Input algorithmInput, ResultingSchedule algorithmOutput)
        {
            AlgorithmInput = algorithmInput;
            AlgorithmOutput = algorithmOutput;
            _resultAlgorithmReport = new AlgorithmReport();
            _idToPersons = algorithmInput.Persons.ToDictionary(p => p.Id, p => p);
        }

        public AlgorithmReport Validate()
        {
            CheckMaxMonthlyWorkNotMet();
            CheckMaxDailyWorkNotMet();
            CheckWorkerPauseLengthNotMet();
            CheckMaxConsecutiveWorkHoursNotMet();
            CheckRequirementsAreNotMet();
            CheckOverlappingIntervals();

            return _resultAlgorithmReport;
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
                    _resultAlgorithmReport.AddReport(new MaxDailyWorkNotMet(person, day));
            });
        }

        private void CheckMaxMonthlyWorkNotMet()
        {
            var personToMonthlyTime = new Dictionary<Person, int>();

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

            foreach (var personToTime in personToMonthlyTime)
            {
                if (personToTime.Value > personToTime.Key.MaxWork)
                {
                    _resultAlgorithmReport.AddReport(new MaxMonthlyWorkNotMet(personToTime.Key));
                }
            }
        }

        private void CheckWorkerPauseLengthNotMet()
        {
            throw new NotImplementedException();
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
                        _resultAlgorithmReport.AddReport(new MaxConsecutiveWorkHoursNotMet(person, day));
                    }
                }
            });
        }

        private void CheckRequirementsAreNotMet()
        {

            //var tempRequirements = new Requirements(AlgorithmInput.MonthlyRequirementsOld);

            //IterateAlgorithmOutput((person, schedule, day) =>
            //{
            //    foreach (var shiftInterval in schedule)
            //    {
            //        foreach (int hour in shiftInterval)
            //        {
            //            tempRequirements.DailySchedules[day].   
            //        }
            //    }
            //});
            //// var tempRequirements = new MonthlyRequirementsOld(AlgorithmInput.MonthlyRequirementsOld.DaysToRequirements);

            //foreach (var personAndMonthlySchedule in AlgorithmOutput.SchedulesForPeople)
            //{
            //    // which personOld it is is irrelevant for this
            //    ScheduleOld personScheduleOld = personAndMonthlySchedule.Value;

            //    foreach (var dayAndDailySchedule in personScheduleOld.DailySchedules)
            //    {
            //        //Intervals intervalsOld = dayAndDailySchedule.Value;

            //foreach (var interval in intervalsOld)
            //{
            //    for (var i = interval.Start; i <= interval.End; i++)
            //    {
            //        // substracts a worker from monthlyRequirementsOld for each hour in the interval
            //        tempRequirements.DaysToRequirements[i].HourToWorkers[i]--;
            //    }
            //}
            //  }
            //}

            // check whether there's enough workers for each hour
            // if there is enough workers, more or equal workers were substracted from hourly monthlyRequirementsOld 
            //foreach (var dayAndRequirement in tempRequirements.DaysToRequirements)
            //{
            //    var day = dayAndRequirement.Key;
            //    var dailyRequirement = dayAndRequirement.Value;

            //    foreach (var hourAndWorkers in dailyRequirement.HourToWorkers)
            //    {
            //        var hour = hourAndWorkers.Value;
            //        var workersCount = hourAndWorkers.Key;

            //        if (workersCount > 0)
            //        {
            //            _resultAlgorithmReport.Requirements.Add(new RequirementsAreNotMet(day, hour));
            //        }
            //    }
            //}
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

                            _resultAlgorithmReport.AddReport(new OverlappingIntervals(reportIntervals, day));

                            overlappingIntervals = new HashSet<ShiftInterval>();
                        }
                        previousInterval = interval;
                    }
                }
            );
        }
    }
}