// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Controls whether only intersecting shapes should be used or whether all shapes should be used.
    /// </summary>
    public enum ShapeRelateType
    {
        /// <summary>
        /// All shapes will be used
        /// </summary>
        All,

        /// <summary>
        /// Only intersecting shapes will be used
        /// </summary>
        Intersecting
    }
}