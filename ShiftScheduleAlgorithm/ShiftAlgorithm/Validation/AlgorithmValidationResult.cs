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

        public string GetMessage()
        {
            var sb = new StringBuilder();
            sb.Append("----------------------------------\n\n");

            foreach (var typeToReports in _reportsDictionary)
            {
                sb.Append($"{typeToReports.Key.Name} :\n");

                typeToReports.Value.Aggregate(sb,
                    (current, report) => sb.Append($"\n{report.GetReportMessage()}"));
                
                sb.Append("\n\n----------------------------------\n\n");
            }

            return sb.ToString();
        }
    }
}