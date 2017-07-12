using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public static class IDictionaryExtensions
    {
        /// <summary>
        ///     Merges two dictionaries into one.
        ///     If a key already exists in the destination dictionary it will not be added nor updated.
        /// </summary>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="destination">Dictionary that will contain all the items</param>
        /// <param name="source">Dictionary to copy from</param>
        /// <param name="manipulationFunc">Function to manipulate the data before copying</param>
        /// <returns>Merged dictionary</returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> destination, IDictionary<TKey, TValue> source, Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>> manipulationFunc)
        {
            if (source == null)
            {
                return destination;
            }

            source.ForEach(pair =>
            {
                if (destination.ContainsKey(pair.Key) == false)
                {
                    if (manipulationFunc != null)
                    {
                        pair = manipulationFunc(pair);
                    }

                    destination.Add(pair.Key, pair.Value);
                }
            });

            return destination;
        }
    }
}
