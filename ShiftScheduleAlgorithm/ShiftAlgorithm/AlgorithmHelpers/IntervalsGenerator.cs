using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    internal class IntervalsGenerator
    {
        public int MaxNumberExclusively { get; }

        public AlgorithmConfiguration AlgorithmConfiguration { get; }

        private readonly IDictionary<int, IList<Intervals<ShiftInterval>>> _intervalLengthToIntervals;

        public IntervalsGenerator(int maxNumberExclusively, AlgorithmConfiguration algorithmConfiguration)
        {
            MaxNumberExclusively = maxNumberExclusively;
            AlgorithmConfiguration = algorithmConfiguration;
            _intervalLengthToIntervals = new Dictionary<int, IList<Intervals<ShiftInterval>>>();
            GenerateIntervals();
        }

        private void GenerateIntervals()
        {
            for (int i = 0; i < MaxNumberExclusively; i++)
            {
                for (int j = i; j < MaxNumberExclusively; j++)
                {
                    var work = ShiftInterval.IntervalType.Work;
                    var inteval = new ShiftInterval(i, j, work);
                    var intervals = new List<ShiftInterval> {inteval};
                    var intervalsS = new Intervals<ShiftInterval>(intervals);
                    var length = j - i + 1;

                    if (!_intervalLengthToIntervals.ContainsKey(length))
                        _intervalLengthToIntervals.Add(length, new List<Intervals<ShiftInterval>>());

                    _intervalLengthToIntervals[length].Add(intervalsS);
                }
            }
        }

        public IEnumerable<Intervals<ShiftInterval>> GetIntervalsWithLengthAtMost(int lengthOfDay)
        {
            return _intervalLengthToIntervals
                .Where(pair => pair.Key <= lengthOfDay)
                .SelectMany(length => _intervalLengthToIntervals[length.Key]);
        }
    }
}