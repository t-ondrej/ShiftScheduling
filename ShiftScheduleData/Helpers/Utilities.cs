using System;
using System.Diagnostics;
using System.IO;

namespace ShiftScheduleData.Helpers
{
    public static class Utilities
    {
        public static string GetPathFromRelativeProjectPath(string relativePath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parent = Directory.GetParent(currentDirectory).Parent?.FullName;
            Debug.Assert(parent != null, "parent != null");
            return Path.Combine(parent, relativePath);
        }

        public static string GetPathFromRelativeProjectPath(string projectName, string relativePath)
        {
            var baseDirectoy = AppDomain.CurrentDomain.BaseDirectory;
            var solutionPath = Path.GetFullPath(Path.Combine(baseDirectoy, "..\\..\\..\\"));
            var projectDirectory = Path.Combine(solutionPath, projectName);
            return Path.Combine(projectDirectory, relativePath);
        }

        public static Interval ParseInterval(string s)
        {
            var values = s.Split('-');
            var start = int.Parse(values[0]);
            var end = int.Parse(values[1]);
            return new Interval(start, end);
        }

        public static string IntervalToString(Interval interval)
        {
            return $"{interval.Start}-{interval.End}";
        }
    }
}