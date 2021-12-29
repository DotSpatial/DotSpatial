// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for ReadOnlyList{T}.
    /// </summary>
    /// <typeparam name="T">Type of contained items.</typeparam>
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        #region Properties

        /// <summary>
        /// Gets the integer count of items in this list.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether this is readonly. This returns true because this is a read-only list.
        /// </summary>
        bool IsReadOnly { get; }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the item at the specified index. Ideally, this ReadOnlyList is used with
        /// value types, or else this gives the user considerable power over the core content.
        /// </summary>
        /// <param name="index">The item to obtain from this list.</param>
        /// <returns>The item at the specified index.</returns>
        T this[int index] { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Tests to see if the specified item is contained in the list. This returns true if the item is contained in the list.
        /// </summary>
        /// <param name="item">The item to test for.</param>
        /// <returns>Boolean, true if the item is found in the list.</returns>
        bool Contains(T item);

        /// <summary>
        /// Copies the specified members into the array, starting at the specified index.
        /// </summary>
        /// <param name="array">The array to copy values to.</param>
        /// <param name="arrayIndex">The array index where the 0 member of this list should be copied to.</param>
        void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Obtains the index of the specified item.
        /// </summary>
        /// <param name="item">The item to find the index of.</param>
        /// <returns>An integer representing the index of the specified item.</returns>
        int IndexOf(T item);

        #endregion
    }
}