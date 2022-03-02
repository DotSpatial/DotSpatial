// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// This class handles some extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class IListExtensions
    {
        #region Methods

        /// <summary>
        /// Decreases the index of the specified item by one.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="self">This list.</param>
        /// <param name="item">the item of type T to decrease the index of.</param>
        /// <returns>True, if the index was decreased.</returns>
        public static bool DecreaseIndex<T>(this IList<T> self, T item)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            int index = self.IndexOf(item);
            if (index == -1) return false;
            if (index == 0) return false;

            self.RemoveAt(index);
            self.Insert(index - 1, item);
            return true;
        }

        /// <summary>
        /// This extension method helps by simply increasing the index value of the specified item
        /// by one.
        /// </summary>
        /// <typeparam name="T">The generic type of this list.</typeparam>
        /// <param name="self">This list.</param>
        /// <param name="item">The item to increase the index of.</param>
        /// <returns>True, if the index was increased.</returns>
        public static bool IncreaseIndex<T>(this IList<T> self, T item)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            int index = self.IndexOf(item);
            if (index == -1) return false;
            if (index == self.Count - 1) return false;

            self.RemoveAt(index);
            self.Insert(index + 1, item);
            return true;
        }

        #endregion
    }
}