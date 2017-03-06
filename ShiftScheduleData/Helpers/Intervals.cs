using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShiftScheduleData.Helpers
{
    public class Intervals : IEnumerable<Interval>
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

        public int GetLengthInTime()
        {
            return IntervalsList.Sum(interval => interval.Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Interval> GetEnumerator()
        {
            return IntervalsList.GetEnumerator();
        }
    }
}