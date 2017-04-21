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

            var workAmountToBeAdded = scheduleForDay.GetTotalWork();

            if (workAmountToBeAdded + CurrentWorkForMonth > TotalWorkForMonth)
                throw new InvalidOperationException();

            if (AssignedDays.ContainsKey(dayId))           
                AssignedDays[dayId] = scheduleForDay;
            else
                AssignedDays.Add(dayId, scheduleForDay);

        //  AssignableSchedulesForDays.Remove(dayId);
            CurrentWorkForMonth += workAmountToBeAdded;
            ResolveUnassignableSchedules();
        }

        private void ResolveUnassignableSchedules()
        {
            foreach (var schedulesForDay in AssignableSchedulesForDays.Values)
            {
                if (AssignedDays.ContainsKey(schedulesForDay.DayId))
                {
                    var assignedSchedule = AssignedDays[schedulesForDay.DayId];
                    schedulesForDay.Schedules.RemoveAll(schedule => schedule.GetTotalWork() <= assignedSchedule.GetTotalWork() 
                                || (!schedule.Intervals.ContainsSubInterval(assignedSchedule.Intervals.First())
                                        && !schedule.Intervals.ContainsSubInterval(assignedSchedule.Intervals.Last())));
                }

                schedulesForDay.Schedules.RemoveAll(schedule => schedule.GetTotalWork() > CurrentWorkLeft);
            }
            
            AssignableSchedulesForDays.RemoveAll(schedule => schedule.Value.Schedules.Count == 0);
        }
    }
}