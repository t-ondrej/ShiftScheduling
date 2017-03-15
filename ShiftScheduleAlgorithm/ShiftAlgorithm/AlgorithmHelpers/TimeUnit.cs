using System;
using System.Collections.Generic;

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
        /// Gets the total sum of the possible amount of work that we can currently achieve. 
        /// The value might change in time when we add / remove schedulable work that is possible for this time unit.
        /// </summary>
        public double SumOfPossibleWorkAmount { get; private set; }

        /// <summary>
        /// Gets the total sum of the current amount of the work that we have already assigned.
        /// </summary>
        public double SumOfCurrentWorkAmount { get; private set; }

        /// <summary>
        /// Gets if we have already assigned enough workers to the unit.
        /// </summary>
        public bool Fulfilled => SumOfCurrentWorkAmount >= RequiredWorkAmount;

        /// <summary>
        /// Gets if we can theoretically assign workers so that they can fulfil the unit.
        /// This may change because of disgarding some schedulable work, when the work cannot be done,
        /// because the person has reached it's max daily / monthly limit.
        /// </summary>
        public bool Fulfillable => SumOfPossibleWorkAmount >= RequiredWorkAmount;

        /// <summary>
        /// Gets the total amount of work that would be done over the required limit when we assign 
        /// all the workers to this time unit. It can be negative, in that case, the time unit is not fulfillable. 
        /// </summary>
        public double CurrentWorkToSpare => SumOfPossibleWorkAmount - RequiredWorkAmount;

        /// <summary>
        /// The dictionary from person id to schedulable work includes this time unit.
        /// </summary>
        private readonly IDictionary<int, SchedulableWork> _personIdToPotentionalWork;

        public IEnumerable<SchedulableWork> AllScSchedulableWork => _personIdToPotentionalWork.Values;

        public TimeUnit(int dayId, int unitOfDay, double requiredWorkAmount)
        {
            DayId = dayId;
            UnitOfDay = unitOfDay;
            RequiredWorkAmount = requiredWorkAmount;
            _personIdToPotentionalWork = new Dictionary<int, SchedulableWork>();
        }

        public void RegisterSchedulableWork(int personId, SchedulableWork schedulableWork)
        {
            _personIdToPotentionalWork.Add(personId, schedulableWork);
            SumOfPossibleWorkAmount += schedulableWork.DailyAvailability.ShiftWeight;
        }

        public void AssignedSchedulableWork(SchedulableWork schedulableWork)
        {
            schedulableWork.StateOfWork = SchedulableWork.State.Assigned;
            SumOfCurrentWorkAmount += schedulableWork.DailyAvailability.ShiftWeight;
        }

        public void CancelSchedulableWork(SchedulableWork schedulableWork)
        {
            if (schedulableWork.StateOfWork == SchedulableWork.State.Assigned)
                throw new InvalidOperationException("Cannot cancel assigned work.");

            schedulableWork.StateOfWork = SchedulableWork.State.Canceled;
            SumOfPossibleWorkAmount -= schedulableWork.DailyAvailability.ShiftWeight;
        }
    }
}