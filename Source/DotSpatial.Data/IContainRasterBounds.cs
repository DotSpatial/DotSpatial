// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An interface for specifying something has a Bounds property on it that is a raster bounds.
    /// </summary>
    public interface IContainRasterBounds
    {
        /// <summary>
        /// Gets or sets the image bounds being used to define the georeferencing of the image.
        /// </summary>
        IRasterBounds Bounds { get; set; }
    }
}