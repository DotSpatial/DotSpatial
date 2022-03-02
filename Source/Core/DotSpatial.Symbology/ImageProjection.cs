// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ImageProjection.
    /// </summary>
    public class ImageProjection : IProj
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageProjection"/> class.
        /// </summary>
        /// <param name="inExtent">The geographic extent.</param>
        /// <param name="inDestRectangle">The image rectangle.</param>
        public ImageProjection(Extent inExtent, Rectangle inDestRectangle)
        {
            GeographicExtents = inExtent;
            ImageRectangle = inDestRectangle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the geographic extents where drawing will take place.
        /// </summary>
        public Extent GeographicExtents { get; }

        /// <summary>
        /// Gets the destination rectangle on the bitmap where drawing should occur.
        /// </summary>
        public Rectangle ImageRectangle { get; }

        #endregion
    }
}