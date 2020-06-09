// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Any item which can be contained by a parent item.
    /// </summary>
    /// <typeparam name="T">The type class of the potential parent.</typeparam>
    public interface IParentItem<T>
    {
        #region Properties

        /// <summary>
        /// Gets the parent item relative to this item.
        /// </summary>
        /// <returns>The parent.</returns>
        T GetParentItem();

        /// <summary>
        /// Sets the parent legend item for this item.
        /// </summary>
        /// <param name="value">The parent.</param>
        void SetParentItem(T value);

        #endregion
    }
}