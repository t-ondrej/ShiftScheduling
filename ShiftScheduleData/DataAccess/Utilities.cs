using System.Diagnostics;
using System.IO;

namespace ShiftScheduleData.DataAccess
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
    }
}