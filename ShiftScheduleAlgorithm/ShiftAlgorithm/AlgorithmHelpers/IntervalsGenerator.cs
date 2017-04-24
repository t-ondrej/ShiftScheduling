using System.Collections.Generic;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleLibrary.Utilities;
using ShiftScheduleUtilities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers
{
    public class IntervalsGenerator
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
            for (var i = 0; i < MaxNumberExclusively; i++)
            {
                for (var j = i; j < MaxNumberExclusively; j++)
                {
                    var intervals = CreateIntervals(i, j);

                    // This has been already added to the dictionary
                    if (intervals == null)
                        continue;

                    var length = j - i + 1;

                    // We include pauses to do daily work, so it's just the total length of the intervals
                    if (length > AlgorithmConfiguration.MaxDailyWork)
                        continue;

                    if (!_intervalLengthToIntervals.ContainsKey(length))
                        _intervalLengthToIntervals.Add(length, new List<Intervals<ShiftInterval>>());

                    _intervalLengthToIntervals[length].Add(new Intervals<ShiftInterval>(intervals));
                }
            }
        }

        private List<ShiftInterval> CreateIntervals(int start, int end)
        {
            var result = new List<ShiftInterval>();
            var workLength = AlgorithmConfiguration.MaxConsecutiveWorkHours;
            var pauseLength = AlgorithmConfiguration.WorkerPauseLength;

            var current = start;

            while (true)
            {
                var nextPoint = current + workLength - 1;

                if (nextPoint >= end)
                {
                    result.Add(new ShiftInterval(current, end, ShiftInterval.IntervalType.Work));
                    break;
                }

                result.Add(new ShiftInterval(current, nextPoint, ShiftInterval.IntervalType.Work));

                var nextEnd = nextPoint + pauseLength;

                // If we don't have any time units left after taking the pause, we shouldn't 
                // continue
                if (nextEnd >= end)
                    return null;

                result.Add(new ShiftInterval(nextPoint + 1, nextEnd, ShiftInterval.IntervalType.Pause));
                current = nextEnd + 1;
            }

            return result;
        }

        public IEnumerable<Intervals<ShiftInterval>> GetIntervalsWithLengthAtMost(int lengthOfDay)
        {
            return _intervalLengthToIntervals
                .Where(pair => pair.Key <= lengthOfDay)
                .SelectMany(length => _intervalLengthToIntervals[length.Key]);
        }

        public IEnumerable<Intervals<ShiftInterval>> GetIntervalsWithLength(int i)
        {
            return _intervalLengthToIntervals.ContainsKey(i)
                ? _intervalLengthToIntervals[i]
                : new List<Intervals<ShiftInterval>>();
        }

        public IEnumerable<Intervals<ShiftInterval>> GetIntervalsWithinBoundaries(int start, int end)
        {
            return _intervalLengthToIntervals.Values
                       .SelectMany(value => value.Where(intervals => intervals.IntervalsList.First().Start >= start
                                                           && intervals.IntervalsList.Last().End <= end)
                                                 .Select(intervals => intervals));
        }
    }
}