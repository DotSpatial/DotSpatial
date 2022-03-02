// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents coordinate type.
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// X and Y coordinates only
        /// </summary>
        Regular,

        /// <summary>
        /// M values are available
        /// </summary>
        M,

        /// <summary>
        /// Z values are available
        /// </summary>
        Z,
    }
}