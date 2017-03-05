using System;
using System.Collections;
using System.Collections.Generic;

namespace ShiftScheduleData.Helpers
{
    public class Interval : IEnumerable<int>
    {
        public int Start { get; }

        public int End { get; }

        public int Count => End - Start + 1;

        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Overlaps(Interval interval)
        {
            return AreOverlapping(this, interval);
        }

        public bool Contains(int i)
        {
            return Start <= i && i <= End;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new IntervalEnumerator(Start, End);
        }

        private sealed class IntervalEnumerator : IEnumerator<int>
        {
            public int Current { get; private set; }

            private readonly int _start;

            private readonly int _end;

            public IntervalEnumerator(int start, int end)
            {
                Current = start - 1;
                _start = start;
                _end = end;
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                Current++;
                return Current <= _end;
            }

            public void Reset()
            {
                Current = _start - 1;
            }

            public void Dispose()
            {
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

        public override string ToString()
        {
            return $"{Start}-{End}";
        }

        public static Interval FromString(string s)
        {
            var values = s.Split('-');
            var start = int.Parse(values[0]);
            var end = int.Parse(values[1]);
            return new Interval(start, end);
        }
    }
}