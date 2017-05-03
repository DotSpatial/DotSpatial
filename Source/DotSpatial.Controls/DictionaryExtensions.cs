using System;
using System.Collections.Generic;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Extension methods for dictionarys.
    /// </summary>
    internal static class DictionaryExtensions
    {
        #region Methods

        /// <summary>
        /// Tries to get the value of the given key. If it doesn't exist, it gets added.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dic">this</param>
        /// <param name="key">They key whose value should be returned.</param>
        /// <param name="valueFactory">Creates the value that gets added to the dict.</param>
        /// <returns>The value belonging to the key.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }

            return value;
        }

        #endregion
    }
}