using System;
using System.Collections.Generic;
using System.Linq;

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
            var message = "----------------------------------\n\n";

            foreach (var typeToReports in _reportsDictionary)
            {
                message = string.Concat(message, $"{typeToReports.Key.Name} :");

                message = typeToReports.Value.Aggregate(message,
                    (current, report) => string.Concat(current, $"\n{report.GetReportMessage()}"));

                message = string.Concat(message, "\n\n----------------------------------\n\n");
            }

            return message;
        }
    }
}