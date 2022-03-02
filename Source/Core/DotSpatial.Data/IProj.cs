// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// The Projection interface that acts as a useful tool for calculating pixel coordinates
    /// to geographic coordinates and vise-versa.
    /// </summary>
    public interface IProj
    {
        #region Properties

        /// <summary>
        /// Gets the Rectangle representation of the geographic extents in image coordinates.
        /// </summary>
        Rectangle ImageRectangle { get; }

        /// <summary>
        /// Gets the geographic extents used for projection.
        /// </summary>
        Extent GeographicExtents { get; }

        #endregion
    }
}