using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.DataAccess.FileDao
{
    internal static class ScheduleParser
    {
        public static Schedule Get(TextReader textReader)
        {
            var dictonary = new Dictionary<int, Intervals>();

            string line;

            while ((line = textReader.ReadLine()) != null && line != "")
            {
                var splited = line.Split(' ');
                var dayId = int.Parse(splited[0]);
                var intervals = splited[1].Split(',').Select(Utilities.ParseInterval).ToList();
                dictonary.Add(dayId, new Intervals(intervals));
            }

            return new Schedule(dictonary);
        }

        public static void Put(TextWriter textWriter, Schedule schedule)
        {
            foreach (var dailySchedule in schedule.DailySchedules)
            {
                var dayId = dailySchedule.Key;
                var intervals = dailySchedule.Value.IntervalsList;
                var intervalStrings = intervals.Select(Utilities.IntervalToString);
                textWriter.WriteLine($"{dayId} {string.Join(",", intervalStrings)}");
            }
        }
    }
}