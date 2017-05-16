// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// LayerType
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// Raster Layer
        /// </summary>
        Grid,

        /// <summary>
        /// Image Layer
        /// </summary>
        Image,

        /// <summary>
        /// Not a valid layer format
        /// </summary>
        Invalid,

        /// <summary>
        /// Line FeatureSet Layer
        /// </summary>
        LineShapefile,

        /// <summary>
        /// Point FeatureSet Layer
        /// </summary>
        PointShapefile,

        /// <summary>
        /// Polygon FeatureSet Layer
        /// </summary>
        PolygonShapefile,
    }
}