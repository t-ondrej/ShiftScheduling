using System.Collections.Generic;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal class TimeUnit
    {
        public int DayId { get; }

        public int UnitOfDay { get; }

        public int RequiredWorkers { get; }

        public int AssignedWorkers { get; set; }

        public IDictionary<int, SchedulableWork> IdToPotentionalWork { get; }

        public int WorkersToSpare => IdToPotentionalWork.Count - AssignedWorkers - RequiredWorkers;

        public bool Fulfilled => AssignedWorkers >= RequiredWorkers;

        public TimeUnit(int dayId, int unitOfDay, int requiredWorkers)
        {
            DayId = dayId;
            UnitOfDay = unitOfDay;
            RequiredWorkers = requiredWorkers;
            IdToPotentionalWork = new Dictionary<int, SchedulableWork>();
        }
    }
}