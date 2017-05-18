// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// In cases where we want to allow some basic cycling or checking of values, but we don't want the user to change
    /// the values directly, we can wrap the values in this read-only list, which restricts what the user can do.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        #region Fields

        private readonly IList<T> _internalList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyList{T}"/> class.
        /// </summary>
        /// <param name="sourceList">The list used in this read only list.</param>
        public ReadOnlyList(IList<T> sourceList)
        {
            _internalList = sourceList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer count of items in this list.
        /// </summary>
        public int Count => _internalList.Count;

        /// <summary>
        /// Gets a value indicating whether this is read only. Returns true because this is a read-only list.
        /// </summary>
        public bool IsReadOnly => true;

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the item at the specified index. Ideally, this ReadOnlyList is used with
        /// value types, or else this gives the user considerable power over the core content.
        /// </summary>
        /// <param name="index">The item to obtain from this list</param>
        /// <returns>The item at the specified index.</returns>
        public T this[int index] => _internalList[index];

        #endregion

        #region Methods

        /// <summary>
        /// Tests to see if the specified item is contained in the list. This returns true if the item is contained in the list.
        /// </summary>
        /// <param name="item">The item to test for.</param>
        /// <returns>Boolean, true if the item is found in the list</returns>
        public bool Contains(T item)
        {
            return _internalList.Contains(item);
        }

        /// <summary>
        /// Copies the specified members into the array, starting at the specified index.
        /// </summary>
        /// <param name="array">The array to copy values to.</param>
        /// <param name="arrayIndex">The array index where the 0 member of this list should be copied to.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Obtains an enumerator for cycling through the values in this list.
        /// </summary>
        /// <returns>An enumerator for the items in this list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        /// <summary>
        /// Obtains the index of the specified item
        /// </summary>
        /// <param name="item">The item to find the index of</param>
        /// <returns>An integer representing the index of the specified item</returns>
        public int IndexOf(T item)
        {
            return _internalList.IndexOf(item);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion
    }
}