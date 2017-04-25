using System;
using System.Diagnostics;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    internal class RandomRemainingPeopleChooser : IRemainingPeopleChooser
    {
        // #hovnokod
        public void AssignScheduleToRemainingPeople(TimeUnitsManager timeUnitsManager)
        {
            var algorithmConfiguration = timeUnitsManager.AlgorithmInput.AlgorithmConfiguration;
            var consecutiveWork = algorithmConfiguration.MaxConsecutiveWorkHours;

            foreach (var scheduledPerson in timeUnitsManager.ScheduledPersons)
            {
                if (scheduledPerson.CurrentWorkLeft <= 0) continue;

                var dailyAvailibilities = scheduledPerson.Person.DailyAvailabilities;

                foreach (var dayToSchedule in scheduledPerson.AssignedDays)
                {
                    if (!dailyAvailibilities.ContainsKey(dayToSchedule.Key)) continue;

                    var assignedIntervals = dayToSchedule.Value.Intervals;

                    if (assignedIntervals.Count() >= algorithmConfiguration.MaxDailyWork) continue;

                    var availibleInterval = dailyAvailibilities[dayToSchedule.Key].Availability;

                    var sortedIntervals = new Intervals<ShiftInterval>(assignedIntervals.IntervalsList);

                    sortedIntervals.SortByStart();

                    // Prolongation of the first work interval for this day

                    ShiftInterval firstInterval = sortedIntervals.IntervalsList[0];

                    if (firstInterval.Count >= consecutiveWork) continue;

                    int maxProlongation = Math.Max(consecutiveWork - firstInterval.Count,
                        algorithmConfiguration.MaxDailyWork - assignedIntervals.Count());

                    int bestNewStart = Math.Min(availibleInterval.Start, firstInterval.Start);

                    int newStart = Math.Max(bestNewStart, firstInterval.Start - maxProlongation);

                    var newShiftInterval = new ShiftInterval(newStart, firstInterval.End, firstInterval.Type);

                    Debug.WriteLine($"Prolonging the beginning interval {firstInterval.ToString()} to {newShiftInterval.ToString()}");

                    assignedIntervals.IntervalsList.Remove(firstInterval);
                    assignedIntervals.IntervalsList.Add(newShiftInterval);

                    // Prolongation of the last work interval for this day

                    ShiftInterval lastInterval = sortedIntervals.IntervalsList[assignedIntervals.Count() - 1];

                    if (assignedIntervals.Count() >= algorithmConfiguration.MaxDailyWork) continue;

                    if (lastInterval.Count >= consecutiveWork) continue;

                    // best new end of last interval without the consideration of 
                    // algorithm configuration
                    var bestNewEnd = Math.Max(availibleInterval.End, lastInterval.End);

                    // taking algorithm configuration into account
                    // max consecutive daily work and max daily work cannot be violated

                    maxProlongation = Math.Max(consecutiveWork - lastInterval.Count,
                        algorithmConfiguration.MaxDailyWork - assignedIntervals.Count());

                    var newEnd = Math.Min(bestNewEnd, lastInterval.End + maxProlongation);

                    newShiftInterval = new ShiftInterval(lastInterval.Start, newEnd, lastInterval.Type);

                    Debug.WriteLine($"Prolonging the ending interval {lastInterval.ToString()} to {newShiftInterval.ToString()}");

                    assignedIntervals.IntervalsList.Remove(lastInterval);
                    assignedIntervals.IntervalsList.Add(newShiftInterval);
                }
            }
        }
    }
}