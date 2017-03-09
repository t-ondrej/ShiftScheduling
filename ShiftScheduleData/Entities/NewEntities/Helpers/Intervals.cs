using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShiftScheduleData.Entities.NewEntities.Helpers
{
    public class Intervals<T> : IEnumerable<T> where T : Interval
    {
        public List<T> IntervalsList { get; }

        public Intervals(List<T> intervals)
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

        public IEnumerator<T> GetEnumerator()
        {
            return IntervalsList.GetEnumerator();
        }     
    }
}