// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// PointTypes.
    /// </summary>
    public enum LegacyPointType
    {
        /// <summary>
        /// Circular points
        /// </summary>
        Circle = 1,

        /// <summary>
        /// Diamond
        /// </summary>
        Diamond = 2,

        /// <summary>
        /// FontChar
        /// </summary>
        FontChar = 9,

        /// <summary>
        /// ImageList
        /// </summary>
        ImageList = 8,

        /// <summary>
        /// Square
        /// </summary>
        Square = 0,

        /// <summary>
        /// Triangle pointed down
        /// </summary>
        TriangleDown = 4,

        /// <summary>
        /// Triangle pointed left
        /// </summary>
        TriangleLeft = 5,

        /// <summary>
        /// Triangle pointed right
        /// </summary>
        TriangleRight = 6,

        /// <summary>
        /// Triangle pointed up
        /// </summary>
        TriangleUp = 3,

        /// <summary>
        /// User defined
        /// </summary>
        UserDefined = 7,
    }
}