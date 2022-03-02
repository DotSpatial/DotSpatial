// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Gets or sets the selection method to use.
    /// <list type="bullet">
    /// <item>Inclusion</item>
    /// <item>Intersection</item>
    /// </list>
    /// Inclusion means that the entire shape must be within the selection bounds in order to select
    /// the shape.  Intersection means that only a portion of the shape must be within the selection
    /// bounds in order for the shape to be selected.
    /// </summary>
    public enum SelectMode
    {
        /// <summary>
        /// The entire contents of the potentially selected item must fall withing the specified extents
        /// </summary>
        Inclusion,

        /// <summary>
        /// The item will be selected if any of the contents of the potentially selected item can be found
        /// in the specified extents.
        /// </summary>
        Intersection,
    }
}