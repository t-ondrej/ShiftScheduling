﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShiftScheduleLibrary.Utilities
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
            IntervalsList.Sort(Interval.ComparerByStart);
        }

        public void SortByEnd()
        {
            IntervalsList.Sort(Interval.ComparerByEnd);
        }

        public int GetLength()
        {
            return IntervalsList.Sum(interval => interval.Count);
        }

        /**
         * Assumes sorted and merged intervals for speed purposes
        */
        public bool ContainsSubInterval(Interval subInterval)
        {
            return IntervalsList.Any(subInterval.IsSubinterval);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return IntervalsList.GetEnumerator();
        }


        public static Intervals<Interval> MergeAndSort(Intervals<Interval> intervals)
        {
            var resultIntervals = new Intervals<Interval>(intervals.IntervalsList);
            var tempIntervals = new Intervals<Interval>(intervals.IntervalsList);
            var previousInterval = new Interval(-2, -2);

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

        // TODO: rework for generic use
        public static Intervals<ShiftInterval> MergeAndSort(Intervals<ShiftInterval> intervals)
        {
            var resultIntervals = new Intervals<ShiftInterval>(new List<ShiftInterval>(intervals.IntervalsList));
            var tempIntervals = new Intervals<ShiftInterval>(new List<ShiftInterval>(intervals.IntervalsList));
            var previousInterval = new ShiftInterval(-2, -2, ShiftInterval.IntervalType.Work);

            tempIntervals.SortByStart();

            foreach (var interval in tempIntervals)
            {
                if (previousInterval.End + 1 == interval.Start && previousInterval.Type == interval.Type)
                {
                    resultIntervals.IntervalsList.Remove(previousInterval);
                    resultIntervals.IntervalsList.Remove(interval);

                    previousInterval = new ShiftInterval(previousInterval.Start, interval.End, interval.Type);
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