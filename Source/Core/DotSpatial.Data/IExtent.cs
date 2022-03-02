// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// A very simple, 2D extent specification.
    /// </summary>
    public interface IExtent
    {
        /// <summary>
        /// Gets or sets the minimum in the X dimension, usually left or minimum longitude.
        /// </summary>
        double MinX { get; set; }

        /// <summary>
        /// Gets or sets the maximum in the x dimension, usually right or maximum longitude.
        /// </summary>
        double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the minimum in the y dimension, usually the bottom or minimum latitude.
        /// </summary>
        double MinY { get; set; }

        /// <summary>
        /// Gets or sets the maximum in the y dimension, usually the top or maximum latitude.
        /// </summary>
        double MaxY { get; set; }

        /// <summary>
        /// Gets a value indicating whether the Min and Max M range should be used.
        /// </summary>
        bool HasM { get; }

        /// <summary>
        /// Gets a value indicating whether the Z value should be used.
        /// </summary>
        bool HasZ { get; }
    }
}