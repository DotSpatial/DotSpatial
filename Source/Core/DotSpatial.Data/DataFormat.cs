// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// The data format.
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// Lines, Points and Polygons make up standard static vector formats.
        /// These are drawn dynamically based on the symbolizer.
        /// </summary>
        Vector,

        /// <summary>
        /// Rasters are grids of integers, doubles, floats, or other numeric value types.
        /// These can be symbolized and represented as images, but not drawn directly.
        /// </summary>
        Raster,

        /// <summary>
        /// Images specifically have pixels coordinates that store a color.
        /// These are drawn directly.
        /// </summary>
        Image,

        /// <summary>
        /// This represents an extended format that does not have a formal definition in DotSpatial.
        /// </summary>
        Custom
    }
}