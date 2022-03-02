// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The HashSet is a dictionary that provides the optimized.
    /// </summary>
    /// <typeparam name="T">The generic type used in the HashSet.</typeparam>
    public class HashSet<T> : ICollection<T>
    {
        #region Fields

        private readonly Dictionary<T, T> _h = new Dictionary<T, T>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer count of members in this set.
        /// </summary>
        public int Count => _h.Count;

        /// <summary>
        /// Gets a value indicating whether these sets can be modified.
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified object to the hash set collection.
        /// </summary>
        /// <param name="item">The object to add.</param>
        public void Add(T item)
        {
            _h.Add(item, item);
        }

        /// <summary>
        /// Clears the entire set.
        /// </summary>
        public void Clear()
        {
            _h.Clear();
        }

        /// <summary>
        /// Gets a boolean indicating if the specified item is in the set.
        /// </summary>
        /// <param name="item">Item that gets checked.</param>
        /// <returns>True, if the item is contained.</returns>
        public bool Contains(T item)
        {
            return _h.ContainsKey(item);
        }

        /// <summary>
        /// Copies the members of this hash set to the specified array, starting at the specified index.
        /// </summary>
        /// <param name="array">Array, the itmes get copied to.</param>
        /// <param name="index">Index where copying starts.</param>
        public void CopyTo(T[] array, int index)
        {
            _h.Keys.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the enumerator of the items in the set.
        /// </summary>
        /// <returns>The enumerator of the items in the set.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _h.Keys.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator of the items in the set.
        /// </summary>
        /// <returns>The enumerator of the items in the set.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _h.Keys.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">Item that gets removed.</param>
        /// <returns>True if the element is successfully found and removed; otherwise, false.</returns>
        public bool Remove(T item)
        {
            return _h.Remove(item);
        }

        #endregion
    }
}