using System;
using System.Diagnostics;
using System.IO;

namespace ShiftScheduleUtilities
{
    public static class PathUtilities
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
    }
}