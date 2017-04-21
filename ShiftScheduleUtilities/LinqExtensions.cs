using System;
using System.Collections.Generic;
using System.Linq;

namespace ShiftScheduleUtilities
{
    public static class LinqExtensions
    {
        #region MinBy

        public static IEnumerable<TSource> MinBy<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> selector)
        {
            return enumerable.MinBy(selector, null);
        }

        public static IEnumerable<TSource> MinBy<TSource, TKey>(this IEnumerable<TSource> enumerable,
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

                var minKey = selector(sourceIterator.Current);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, minKey) < 0)
                         minKey = candidateProjected;
                }

                sourceIterator.Reset();

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, minKey) == 0)
                        yield return candidate;
                }
            }
        }

        #endregion

        #region Iterating

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            enumerable.ForEach((arg, index) => action(arg));
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
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

            enumerable.ForEach((element, index) =>
            {
                var key = keyFunc(element, index);
                var value = valueFunc(element, index);
                result.Add(key, value);
            });

            return result;
        }

        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            var keys = dictionary.Where(predicate).ToList();

            foreach (var key in keys)
            {
                dictionary.Remove(key);
            }
        }

        public static TValue GetIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return !dictionary.ContainsKey(key) ? default(TValue) : dictionary[key];
        }
    }
}