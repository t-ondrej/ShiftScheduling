using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider.AlgorithmHelpers
{
    internal class SchedulableWork
    {
        public ScheduledPerson ScheduledPerson { get; }

        public int DayId { get; }

        public Interval Interval { get; }

        public bool Scheduled { get; set; }

        public SchedulableWork(ScheduledPerson scheduledPerson, int dayId, Interval interval)
        {
            ScheduledPerson = scheduledPerson;
            DayId = dayId;
            Interval = interval;
        }
    }
}