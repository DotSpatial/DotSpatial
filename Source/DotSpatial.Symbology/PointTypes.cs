// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Allows the selection of several pre-defined shapes that have built
    /// in drawing code.
    /// </summary>
    public enum PointShape
    {
        /// <summary>
        /// Like a rectangle, but oriented with the points vertically
        /// </summary>
        Diamond,

        /// <summary>
        /// An rounded elipse. The Size parameter determines the size of the ellipse before rotation.
        /// </summary>
        Ellipse,

        /// <summary>
        /// A hexagon drawn to fit the size specified. Only the smaller dimension is used.
        /// </summary>
        Hexagon,

        /// <summary>
        /// A rectangle fit to the Size before any rotation occurs.
        /// </summary>
        Rectangle,

        /// <summary>
        /// A pentagon drawn to fit the size specified. Only the smaller dimension is used.
        /// </summary>
        Pentagon,

        /// <summary>
        /// A star drawn to fit the size. Only the smaller size dimension is used.
        /// </summary>
        Star,

        /// <summary>
        /// Triangle with the point facing upwards initially.
        /// </summary>
        Triangle,

        /// <summary>
        /// The default value.
        /// </summary>
        Undefined,
    }
}