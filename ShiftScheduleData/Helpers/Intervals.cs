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

        /**
         * Assumes sorted and merged intervals for speed purposes
        */
        public bool ContainsSubInterval(Interval subInterval)
        {
            return IntervalsList.Any(subInterval.IsSubinterval);
        }

        public static Intervals MergeAndSort(Intervals intervals)
        {
            var resultIntervals = new Intervals(intervals.IntervalsList);
            var tempIntervals = new Intervals(intervals.IntervalsList);
            var previousInterval = new Interval(-1, -1);

            tempIntervals.SortByStart();
            
            foreach (var interval in tempIntervals)
            {
                if (previousInterval.End + 1 == interval.Start)
                {
                    resultIntervals.IntervalsList.Remove(previousInterval);
                    resultIntervals.IntervalsList.Remove(interval);

                    previousInterval = new Interval(previousInterval.Start, interval.End);
                    resultIntervals.IntervalsList.Add(previousInterval);
                }
                else
                {
                    previousInterval = interval;
                }
            }

            resultIntervals.SortByStart();
            return resultIntervals;
        }
    }
}