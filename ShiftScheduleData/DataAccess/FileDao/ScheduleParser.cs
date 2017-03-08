using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleData.DataAccess.FileDao
{
    internal static class ScheduleParser
    {
        public static MonthlySchedule Get(TextReader textReader)
        {
            var dictonary = new Dictionary<int, Intervals>();

            string line;

            while ((line = textReader.ReadLine()) != null && line != "")
            {
                var splited = line.Split(' ');
                var dayId = int.Parse(splited[0]);
                var intervals = splited[1].Split(',').Select(Interval.FromString).ToList();
                dictonary.Add(dayId, new Intervals(intervals));
            }

            return new MonthlySchedule(dictonary);
        }

        public static void Put(TextWriter textWriter, MonthlySchedule monthlySchedule)
        {
            foreach (var dailySchedule in monthlySchedule.DailySchedules)
            {
                var dayId = dailySchedule.Key;
                var intervals = dailySchedule.Value.IntervalsList;
                var intervalStrings = intervals.Select(i => i.ToString());
                textWriter.WriteLine($"{dayId} {string.Join(",", intervalStrings)}");
            }
        }
    }
}