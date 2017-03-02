using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.DataAccess.FileDao
{
    internal static class ScheduleParser
    {
        public static Schedule Get(StreamReader streamReader)
        {
            var dictonary = new Dictionary<int, Intervals>();

            string line;
            var i = 0;

            while ((line = streamReader.ReadLine()) != null)
            {
                if (line != "")
                {
                    var intervals = line.Split(' ').Select(ParseInterval).ToList();
                    dictonary.Add(i, new Intervals(intervals));
                }

                i++;
            }

            return new Schedule(dictonary);
        }

        private static Interval ParseInterval(string s)
        {
            var values = s.Split('-');
            var start = int.Parse(values[0]);
            var end = int.Parse(values[1]);
            return new Interval(start, end);
        }

        public static void Put(StreamWriter streamWriter, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}