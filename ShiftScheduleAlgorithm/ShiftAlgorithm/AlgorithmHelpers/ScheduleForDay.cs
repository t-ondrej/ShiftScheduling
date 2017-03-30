using System.Linq;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class ScheduleForDay
    {
        public ScheduledPerson ScheduledPerson { get; }

        public int DayId { get; }

        public double ShiftWeight { get; }

        public Intervals<ShiftInterval> Intervals { get; }

        public ScheduleForDay(ScheduledPerson scheduledPerson, int dayId, double shiftWeight,
            Intervals<ShiftInterval> intervals)
        {
            ScheduledPerson = scheduledPerson;
            DayId = dayId;
            Intervals = intervals;
            ShiftWeight = shiftWeight;
        }

        public double GetShiftWeightForUnit(int unitOfDay)
        {
            var work = ShiftInterval.IntervalType.Work;
            return Intervals.First(interval => interval.Contains(unitOfDay)).Type == work
                ? ShiftWeight
                : 0;
        }

        public void Assign()
        {
            ScheduledPerson.Assign(this);
        }

        public int GetTotalWork()
        {
            return Intervals.GetLength();
        }
    }
}