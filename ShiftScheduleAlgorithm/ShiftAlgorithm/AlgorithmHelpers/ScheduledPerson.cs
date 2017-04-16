using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

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

            if (AssignedDays.ContainsKey(dayId))
                throw new InvalidOperationException();

            var workAmountToBeAdded = scheduleForDay.GetTotalWork();

            if (workAmountToBeAdded + CurrentWorkForMonth > TotalWorkForMonth)
                throw new InvalidOperationException();

            AssignedDays.Add(scheduleForDay.DayId, scheduleForDay);
        //  AssignableSchedulesForDays.Remove(dayId);
            CurrentWorkForMonth += workAmountToBeAdded;
            ResolveUnassignableSchedules();
        }

        private void ResolveUnassignableSchedules()
        {
            foreach (var schedulesForDay in AssignableSchedulesForDays.Values)
            {
                schedulesForDay.Schedules.RemoveAll(schedule => schedule.GetTotalWork() > CurrentWorkLeft);

                if (schedulesForDay.Schedules.Count == 0)
                    AssignableSchedulesForDays.Remove(schedulesForDay.DayId);
            }
        }
    }
}