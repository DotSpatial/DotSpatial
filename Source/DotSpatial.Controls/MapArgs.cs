// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;
using GeoAPI.Geometries;
using System;
using System.Drawing;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The map arguments.
    /// </summary>
    public class MapArgs : EventArgs, IProj
    {

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapArgs"/> class.
        /// </summary>
        /// <param name="bufferRectangle">The buffer rectangle.</param>
        /// <param name="bufferEnvelope">The buffer envelope.</param>
        public MapArgs(Rectangle bufferRectangle, Extent bufferEnvelope, int factor = 1)
        {
            ImageRectangle = bufferRectangle;
            GeographicExtents = bufferEnvelope;
            Factor = factor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapArgs"/> class, where the device is also specified, overriding the default buffering behavior.
        /// </summary>
        /// <param name="bufferRectangle">The buffer rectangle.</param>
        /// <param name="bufferEnvelope">The buffer envelope.</param>
        /// <param name="g">The graphics object used for drawing.</param>
        public MapArgs(Rectangle bufferRectangle, Extent bufferEnvelope, Graphics g, int factor = 1)
        {
            ImageRectangle = bufferRectangle;
            GeographicExtents = bufferEnvelope;
            Device = g;
            Factor = factor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an optional parameter that specifies a device to use instead of the normal buffers.
        /// </summary>
        public Graphics Device { get; }

        /// <summary>
        /// Gets the Dx
        /// </summary>
        public double Dx => GeographicExtents.Width != 0.0 ? ImageRectangle.Width / GeographicExtents.Width : 0.0;

        /// <summary>
        /// Gets the Dy
        /// </summary>
        public double Dy => GeographicExtents.Height != 0.0 ? ImageRectangle.Height / GeographicExtents.Height : 0.0;

        /// <summary>
        /// Gets the geographic bounds of the content of the buffer.
        /// </summary>
        public Extent GeographicExtents { get; }

        /// <summary>
        /// Gets the rectangle dimensions of what the buffer should be in pixels
        /// </summary>
        public Rectangle ImageRectangle { get; }

        /// <summary>
        /// Gets the maximum Y value
        /// </summary>
        public double MaxY => GeographicExtents.MaxY;

        /// <summary>
        /// Gets the minimum X value
        /// </summary>
        public double MinX => GeographicExtents.MinX;

        // CGX
        public int Factor { get; set; }
        // CGX END

        #endregion
    }
}