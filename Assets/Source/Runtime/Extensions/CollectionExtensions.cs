using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace Sample.Extensions
{
    public static class CollectionExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var value) && value == null ? defaultValue : value;
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> source) where TKey : notnull
        {
            return source.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value) where TKey : notnull
        {
            if (source.ContainsKey(key))
            {
                return;
            }

            source.Add(key, value);
        }

        public static void AddOrSet<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value) where TKey : notnull
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
                return;
            }

            source.Add(key, value);
        }

        public static void RemoveIfExists<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key) where TKey : notnull
        {
            if (!source.ContainsKey(key))
            {
                return;
            }

            source.Remove(key);
        }

        public static T Random<T>(this IEnumerable<T> source)
        {
            return source.Shuffle().FirstOrDefault();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();

            var n = array.Length;

            while (n > 1)
            {
                var k = UnityEngine.Random.Range(0, n--);
                (array[n], array[k]) = (array[k], array[n]);
            }

            return array;
        }

        public static List<T> Resize<T>(this List<T> list, int size, T element = default)
        {
            if (size < list.Count)
            {
                list.RemoveRange(size, list.Count - size);
                return list;
            }

            if (size > list.Capacity)
            {
                list.Capacity = size;
            }

            for (var i = 0; i < size - list.Count; i++)
            {
                list.Add(element);
            }

            return list;
        }

        public static void Shrink<T>(this List<T> list, int size)
        {
            if (size >= list.Count)
            {
                return;
            }

            list.RemoveRange(size, list.Count - size);
        }

        public static bool IndexInBounds(this ICollection collection, int index)
        {
            return index >= 0 && index < collection.Count;
        }

        public static bool IndexInBounds<T>(this NativeArray<T> array, int index) where T : struct
        {
            return index >= 0 && index < array.Length;
        }

        public static T ElementAtOrDefault<T>(this T[] array, int index)
        {
            return array.IndexInBounds(index) ? array[index] : default;
        }
    }
}