// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// ItemType.
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// The specified element is a folder
        /// </summary>
        Folder,

        /// <summary>
        /// The specified element is an image
        /// </summary>
        Image,

        /// <summary>
        /// The specified element is a vector line file format
        /// </summary>
        Line,

        /// <summary>
        /// The specified element is a vector line point format
        /// </summary>
        Point,

        /// <summary>
        /// The specified element is a vector polygon format
        /// </summary>
        Polygon,

        /// <summary>
        /// The specified element is a raster format
        /// </summary>
        Raster,

        /// <summary>
        /// The specified element is a custom format, so the custom icon is used
        /// </summary>
        Custom
    }
}