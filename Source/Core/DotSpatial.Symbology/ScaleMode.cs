// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Specifies whether non-coordinate drawing properties like width or size
    /// use pixels or map coordinates. If pixels are used, a "back transform"
    /// to approximate pixel sizes.
    /// </summary>
    public enum ScaleMode
    {
        /// <summary>
        /// Uses the simplest symbology possible, but can draw quickly
        /// </summary>
        Simple = 0,

        /// <summary>
        /// Symbol sizing parameters are based in world coordinates and will get smaller when zooming out like a real object.
        /// </summary>
        Geographic = 1,

        /// <summary>
        /// The symbols approximately preserve their size as you zoom
        /// </summary>
        Symbolic = 2
    }
}