using System.IO;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation
{
    public abstract class Report
    {
        public enum Seriousness
        {
            Error,
            Warning
        }

        public abstract Seriousness ReportSeriousness { get; }

        public void PrintReportMessage(TextWriter textWriter)
        {
            textWriter.WriteLine(GetReportMessage());
        }

        public abstract string GetReportMessage();
    }
}