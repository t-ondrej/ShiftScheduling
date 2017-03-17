using System.Collections.Generic;
using System.Diagnostics;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class TimeUnit
    {
        /// <summary>
        /// Gets the day of the this time unit (which makes a unique ID of this time unit together with UnitOfDay)
        /// </summary>
        public int DayId { get; }

        /// <summary>
        /// Gets the unit of the day of this time unit (which makes a unique ID of this time unit together with DayId).
        /// </summary>
        public int UnitOfDay { get; }

        /// <summary>
        /// Gets the total amount of work that is required for this time unit. It's given by the algorithm requirements.
        /// </summary>
        public double RequiredWorkAmount { get; }

        /// <summary>
        /// Gets the total sum of the current amount of the work that we have already assigned.
        /// </summary>
        public double SumOfCurrentWorkAmount { get; private set; }

        /// <summary>
        /// Gets if we have already assigned enough workers to the unit.
        /// </summary>
        public bool Fulfilled => SumOfCurrentWorkAmount >= RequiredWorkAmount;

        /// <summary>
        /// Gets or setsif we can theoretically assign workers so that they can fulfil the unit.
        /// </summary>
        public bool Fulfillable { get; set; }

        /// <summary>
        /// Gets the current schedules assigned to the unit.
        /// </summary>
        public IList<ScheduleForDay> CurrentSchedules { get; }

        /// <summary>
        /// Constructs a time unit.
        /// </summary>
        /// <param name="dayId">The id of the day</param>
        /// <param name="unitOfDay">The id of the part of the day</param>
        /// <param name="requiredWorkAmount">The amount of work from requirements</param>
        public TimeUnit(int dayId, int unitOfDay, double requiredWorkAmount)
        {
            DayId = dayId;
            UnitOfDay = unitOfDay;
            RequiredWorkAmount = requiredWorkAmount;
            CurrentSchedules = new List<ScheduleForDay>();
        }

        /// <summary>
        /// Assigns the given schedule to the time unit.
        /// </summary>
        /// <param name="scheduleForDay">The schedule. It's assumed that the schedule actually covers the unit.</param>
        public void AssignSchedule(ScheduleForDay scheduleForDay)
        {
            Debug.Assert(scheduleForDay.DayId == DayId);
            var workAmount = scheduleForDay.GetShiftWeightForUnit(UnitOfDay);
            SumOfCurrentWorkAmount += workAmount;
            scheduleForDay.ScheduledPerson.Assign(scheduleForDay);
        }
    }
}