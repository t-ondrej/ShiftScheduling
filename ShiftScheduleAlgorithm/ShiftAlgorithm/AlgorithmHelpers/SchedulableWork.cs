using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class SchedulableWork
    {
        public enum State
        {
            Assigned, Canceled, Assignable
        }

        public ScheduledPerson ScheduledPerson { get; }

        public int DayId { get; }

        public Person.DailyAvailability DailyAvailability { get; }

        public State StateOfWork { get; set; }
        
        public SchedulableWork(ScheduledPerson scheduledPerson, int dayId)
        {
            ScheduledPerson = scheduledPerson;
            DayId = dayId;
            DailyAvailability = scheduledPerson.Person.DailyAvailabilities[dayId];
            StateOfWork = State.Assignable;
        }
    }
}