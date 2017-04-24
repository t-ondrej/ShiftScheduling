using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;
using System.Linq;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class ScheduledPerson
    {
        public Person Person { get; }

        public IDictionary<int, SchedulesForDay> AssignableSchedulesForDays { get; }

        public IDictionary<int, ScheduleForDay> AssignedDays { get; }

        public IDictionary<int, double> ShiftWeights { get; }

        public int TotalWorkForMonth { get; }

        public int CurrentWorkForMonth { get; private set; }

        public int CurrentWorkLeft => TotalWorkForMonth - CurrentWorkForMonth;

        public ScheduledPerson(Person person, int totalWorkForMonth, IDictionary<int, double> shiftWeights)
        {
            Person = person;
            TotalWorkForMonth = totalWorkForMonth;
            ShiftWeights = shiftWeights;
            AssignableSchedulesForDays = new Dictionary<int, SchedulesForDay>();
            AssignedDays = new Dictionary<int, ScheduleForDay>();
        }

        public void Assign(ScheduleForDay scheduleForDay)
        {
            var dayId = scheduleForDay.DayId;

            //   if (AssignedDays.ContainsKey(dayId))
            //       throw new InvalidOperationException();

            // If the person already has assigned a schedule in that day, get the totalWork of the old schedule
            var workDiff = AssignedDays.ContainsKey(dayId) ? AssignedDays[dayId].GetTotalWork() : 0;

            // Add only the difference of TotalWork between the old and the new schedule
            var workAmountToBeAdded = scheduleForDay.GetTotalWork() - workDiff;

            if (workAmountToBeAdded + CurrentWorkForMonth > TotalWorkForMonth)
                throw new InvalidOperationException();

            if (AssignedDays.ContainsKey(dayId))           
                AssignedDays[dayId] = scheduleForDay;
            else
                AssignedDays.Add(dayId, scheduleForDay);

            CurrentWorkForMonth += workAmountToBeAdded;
            ResolveUnassignableSchedules();
        }

        private void ResolveUnassignableSchedules()
        {
            // Iterate through all the assignable schedules
            foreach (var schedulesForDay in AssignableSchedulesForDays.Values)
            {
                // If the person has already assigned a schedule in the day
                if (AssignedDays.ContainsKey(schedulesForDay.DayId))
                {
                    var assignedSchedule = AssignedDays[schedulesForDay.DayId];
                    // Keep schedules that are longer than the assigned schedule and contains the assigned schedule as a subschedule also
                    schedulesForDay.Schedules.RemoveAll(schedule => schedule.GetTotalWork() <= assignedSchedule.GetTotalWork() 
                                || (!schedule.Intervals.ContainsSubInterval(assignedSchedule.Intervals.First())
                                        && !schedule.Intervals.ContainsSubInterval(assignedSchedule.Intervals.Last())));
                }

                // Remove all schedules that aren't assignable to person because he would exceed his MaxWorkPerMonth
                schedulesForDay.Schedules.RemoveAll(schedule => schedule.GetTotalWork() > CurrentWorkLeft);
            }
            
            // Remove all elements that has no assignable schedules in that day
            AssignableSchedulesForDays.RemoveAll(schedule => schedule.Value.Schedules.Count == 0);
        }
    }
}