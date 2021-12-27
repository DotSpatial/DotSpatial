// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PatternTypes.
    /// </summary>
    public enum PatternType
    {
        /// <summary>
        /// A pattern that gradually changes from one color to another
        /// </summary>
        Gradient,

        /// <summary>
        /// A pattern comprised of evenly spaced lines
        /// </summary>
        Line,

        /// <summary>
        /// A pattern comprised of point symbolizers
        /// </summary>
        Marker,

        /// <summary>
        /// A pattern comprised of a tiled texture
        /// </summary>
        Picture,

        /// <summary>
        /// A pattern comprised strictly of a fill color.
        /// </summary>
        Simple
    }
}