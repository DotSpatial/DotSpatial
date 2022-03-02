// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Add, remove and clear methods don't work on all the categorical sub-filters, but rather only on the
    /// most immediate.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Categories
        /// </summary>
        Category,

        /// <summary>
        /// Chunks
        /// </summary>
        Chunk,

        /// <summary>
        /// Selected or unselected
        /// </summary>
        Selection,

        /// <summary>
        /// Visible or not
        /// </summary>
        Visible,
    }
}