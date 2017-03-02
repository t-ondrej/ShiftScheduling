using System.Collections.Generic;

namespace ShiftScheduleData.Helpers
{
    public class Intervals
    {
        public List<Interval> IntervalsList { get; }

        public Intervals(List<Interval> intervals)
        {
            IntervalsList = intervals;
        }

        public void SortByStart()
        {
            IntervalsList.Sort(new Interval.StartComparator());
        }

        public void SortByEnd()
        {
            IntervalsList.Sort(new Interval.EndComparator());
        }
    }
}