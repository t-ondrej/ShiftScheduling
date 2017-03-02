using System;
using System.Collections.Generic;

namespace ShiftScheduleData.Helpers
{
    public class Interval
    {
        public int Start { get; }

        public int End { get; }

        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Overlaps(Interval interval)
        {
            return AreOverlapping(this, interval);
        }

        public static bool AreOverlapping(Interval interval1, Interval interval2)
        {
            return Math.Min(interval1.End, interval2.End) >= Math.Max(interval1.Start, interval2.Start);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Interval) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Start * 397) ^ End;
            }
        }

        public sealed class StartComparator : IComparer<Interval>
        {
            public int Compare(Interval x, Interval y)
            {
                return x.Start.CompareTo(y.Start);
            }
        }

        public sealed class EndComparator : IComparer<Interval>
        {
            public int Compare(Interval x, Interval y)
            {
                return x.End.CompareTo(y.End);
            }
        }
    }
}