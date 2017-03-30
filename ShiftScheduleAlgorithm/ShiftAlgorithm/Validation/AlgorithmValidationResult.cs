using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Validation
{
    public class AlgorithmValidationResult
    {
        private readonly IDictionary<Type, IList<Report>> _reportsDictionary;

        public AlgorithmValidationResult()
        {
            _reportsDictionary = new Dictionary<Type, IList<Report>>();
        }

        public IEnumerable<T> GetReports<T>() where T : Report
        {
            var type = typeof(T);

            return !_reportsDictionary.ContainsKey(type) ? null : _reportsDictionary[type].Cast<T>();
        }

        public void AddReport(Report report)
        {
            var type = report.GetType();

            if (!_reportsDictionary.ContainsKey(type))
                _reportsDictionary.Add(type, new List<Report>());

            _reportsDictionary[type].Add(report);
        }

        //public string GetMessage<T>() where T : Report
        //{
        //    var reports = GetReports<T>();

        //    var sb = new StringBuilder();
        //    sb.Append("----------------------------------\n\n");
        //    foreach (var report in reports)
        //    {
        //        sb.Append(typeof(T) + "\n\n");

        //    }
        //    return null;
        //}
        public string GetMessage()
        {
            var sb = new StringBuilder();
            sb.Append("----------------------------------\n\n");

            foreach (var typeToReports in _reportsDictionary)
            {
                if (typeToReports.Value.Count < 1)
                {
                    continue;
                }

                sb.Append($"{typeToReports.Key.Name} :\n\n" +
                          $"Seriousness: {typeToReports.Value[0].ReportSeriousness}\n");

                typeToReports.Value.Aggregate(sb,
                    (current, report) => sb.Append($"\n{report.GetReportMessage()}"));
                
                sb.Append("\n\n----------------------------------\n\n");
            }

            return sb.ToString();
        }
    }
}