using System;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm
{
    internal class Program
    {
        private static void Main()
        {
            Interval i1 = new Interval(1, 5);
            string s1 = i1.ToString();

            Interval i2 = new ShiftInterval(1, 5, ShiftInterval.IntervalType.Work);
            string s2 = i2.ToString();

            ShiftInterval i3 = new ShiftInterval(2, 4, ShiftInterval.IntervalType.Pause);
            string s3 = i3.ToString();

            Console.WriteLine($"{s1}\n{s2}\n{s3}");
            Console.WriteLine($"{Interval.FromString(s1)}\n{Interval.FromString(s2)}\n{ShiftInterval.FromString(s3)}");
        }
    }
}