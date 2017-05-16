// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The available feature symbolizer types
    /// </summary>
    public enum SymbolizerType
    {
        /// <summary>
        /// The type is PointSymbolizer
        /// </summary>
        Point,

        /// <summary>
        /// The type is LineSymbolizer
        /// </summary>
        Line,

        /// <summary>
        /// The type is PolygonSymbolizer
        /// </summary>
        Polygon,

        /// <summary>
        /// The type is RasterSymbolizer
        /// </summary>
        Raster,

        /// <summary>
        /// The type of the symbolizer is unknown
        /// </summary>
        Unknown
    }
}