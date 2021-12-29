// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The map raster layer is used for drawing rasters.
    /// </summary>
    public interface IMapRasterLayer : IRasterLayer, IMapLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        Image BackBuffer { get; set; }

        /// <summary>
        /// Gets or sets the current buffer.
        /// </summary>
        Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image layer is initialized.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new bool IsInitialized { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing. Specifying transparent
        /// will replace content with transparent pixels.</param>
        void Clear(List<Rectangle> rectangles, Color color);

        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        void FinishDrawing();

        /// <summary>
        /// Copies any current content to the back buffer so that drawing should occur on the
        /// back buffer (instead of the fore-buffer). Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer
        /// where drawing will be taking place.</param>
        void StartDrawing(bool preserve);

        #endregion
    }
}