// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// ReferenceTypes
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// The coordinates are drawn in screen coordinates on the layer, and stay fixed as the map
        /// zooms and pans
        /// </summary>
        Screen,

        /// <summary>
        /// The drawing layer is geographically referenced and will move with the other spatially
        /// referenced map content.
        /// </summary>
        Geographic
    }
}