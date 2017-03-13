using System;
using System.Collections.Generic;

namespace ShiftScheduleUtilities
{
    public static class LinqExtensions
    {
        #region MinBy

        // http://stackoverflow.com/questions/914109/how-to-use-linq-to-select-object-with-minimum-or-maximum-property-value

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = enumerable.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");

                var min = sourceIterator.Current;
                var minKey = selector(min);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }

                return min;
            }
        }

        #endregion

        #region Iterating

        public static void Iterate<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            enumerable.Iterate((arg, index) => action(arg));
        }

        public static void Iterate<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var enumerator = enumerable.GetEnumerator())
            {
                var index = 0;

                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    action(current, index);
                    index++;
                }
            }
        }

        #endregion

        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this IEnumerable<T> enumerable,
            Func<T, int, TKey> keyFunc, Func<T, int, TValue> valueFunc)
        {
            var result = new Dictionary<TKey, TValue>();

            enumerable.Iterate((element, index) =>
            {
                var key = keyFunc(element, index);
                var value = valueFunc(element, index);
                result.Add(key, value);
            });

            return result;
        }
    }
}