// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for the map frame.
    /// </summary>
    public interface IMapFrame : IFrame, IMapGroup, IProj
    {
        #region Events

        /// <summary>
        /// Occurs when the buffer content has been altered and any containing maps should quick-draw
        /// from the buffer, followed by the tool drawing.
        /// </summary>
        event EventHandler<ClipArgs> BufferChanged;

        /// <summary>
        /// Occurs after every one of the zones, chunks and stages has finished rendering to a stencil.
        /// </summary>
        event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs after changes have been made to the back buffer that affect the viewing area of the screen,
        /// thereby requiring an invalidation.
        /// </summary>
        event EventHandler ScreenUpdated;

        /// <summary>
        /// Occurs when View changed
        /// </summary>
        event EventHandler<ViewChangedEventArgs> ViewChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the buffered image. Mess with this at your own risk.
        /// </summary>
        Image BufferImage { get; set; }

        /// <summary>
        /// Gets a rectangle indicating the size of the map frame image
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        /// Gets or sets the integer that specifies the chunk that is actively being drawn
        /// </summary>
        int CurrentChunk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this map frame should define its buffer
        /// region to be the same size as the client, or three times larger.
        /// </summary>
        bool ExtendBuffer { get; set; }

        /// <summary>
        /// Gets the coefficient used for ExtendBuffer. This coefficient should not be modified.
        /// </summary>
        int ExtendBufferCoeff { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this map frame is currently in the process of redrawing the
        /// stencils after a pan operation. Drawing should not take place if this is true.
        /// </summary>
        bool IsPanning { get; set; }

        /// <summary>
        /// Gets or sets the layers
        /// </summary>
        new IMapLayerCollection Layers { get; set; }

        /// <summary>
        /// Gets or sets the parent control for this map frame.
        /// </summary>
        Control Parent { get; set; }

        /// <summary>
        /// Gets or sets the PromptMode that determines how to warn users when attempting to add a layer without
        /// a projection to a map that has a projection.
        /// </summary>
        ActionMode ProjectionModeDefine { get; set; }

        /// <summary>
        /// Gets or sets the PromptMode that determines how to warn users when attempting to add a layer with
        /// a coordinate system that is different from the current projection.
        /// </summary>
        ActionMode ProjectionModeReproject { get; set; }

        /// <summary>
        /// gets or sets the rectangle in pixel coordinates that will be drawn to the entire screen.
        /// </summary>
        Rectangle View { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Unlike PixelToProj, which works relative to the client control,
        /// BufferToProj takes a pixel coordinate on the buffer and
        /// converts it to geographic coordinates.
        /// </summary>
        /// <param name="position">A Point describing the pixel position on the back buffer</param>
        /// <returns>An ICoordinate describing the geographic position</returns>
        Coordinate BufferToProj(Point position);

        /// <summary>
        /// This projects a rectangle relative to the buffer into and Envelope in geographic coordinates.
        /// </summary>
        /// <param name="rect">A Rectangle</param>
        /// <returns>An Envelope interface</returns>
        Extent BufferToProj(Rectangle rect);

        /// <summary>
        /// Using the standard independent paint method would potentially cause for dis-synchrony between
        /// the parents state and the state of this control. This way, the drawing is all done at the same time.
        /// </summary>
        /// <param name="e">The event args.</param>
        void Draw(PaintEventArgs e);

        /// <summary>
        /// Uses the current buffer and envelope to force each of the contained layers
        /// to re-draw their content. This is useful after a zoom or size change.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Instructs the map frame to draw content from the specified regions to the buffer..
        /// </summary>
        /// <param name="regions">The regions to initialize.</param>
        void Initialize(List<Extent> regions);

        /// <summary>
        /// This will cause an invalidation for each layer. The actual rectangle to re-draw is not specified
        /// here, but rather this simply indicates that some re-calculation is necessary.
        /// </summary>
        void InvalidateLayers();

        /// <summary>
        /// Pans the image for this map frame. Instead of drawing entirely new content, from all 5 zones,
        /// just the slivers of newly revealed area need to be re-drawn.
        /// </summary>
        /// <param name="shift">A Point showing the amount to shift in pixels</param>
        void Pan(Point shift);

        /// <summary>
        /// When the control is being resized, the view needs to change in order to preserve the aspect ratio,
        /// even though we want to use the exact same extents.
        /// </summary>
        void ParentResize();

        /// <summary>
        /// Obtains a rectangle relative to the background image by comparing
        /// the current View rectangle with the parent control's size.
        /// </summary>
        /// <param name="clip">Rectangle used for clipping.</param>
        /// <returns>The resulting rectangle.</returns>
        Rectangle ParentToView(Rectangle clip);

		// CGX
        /// <summary>
        /// 
        /// </summary>
        void ZoomToLayerEnvelope(Envelope layerEnvelope);
        // CGX END

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object. This is useful
        /// for doing vector drawing on much larger pages. The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">Graphics device to print to</param>
        /// <param name="targetRectangle">The target rectangle in the graphics units of the device</param>
        void Print(Graphics device, Rectangle targetRectangle);

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object. This is useful
        /// for doing vector drawing on much larger pages. The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">Graphics object used for drawing.</param>
        /// <param name="targetRectangle">Rectangle to draw the content to.</param>
        /// <param name="targetEnvelope">the extents to draw to the target rectangle</param>
        void Print(Graphics device, Rectangle targetRectangle, Extent targetEnvelope);

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        Point ProjToBuffer(Coordinate location);

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="ext">The geographic Envelope</param>
        /// <returns>A Rectangle</returns>
        Rectangle ProjToBuffer(Extent ext);

        /// <summary>
        /// Re-creates the buffer based on the size of the control without changing
        /// the geographic extents. This is used after a resize operation.
        /// </summary>
        void ResetBuffer();

        /// <summary>
        /// This is not called during a resize, but rather after panning or zooming where the
        /// view is used as a guide to update the extents. This will also call ResetBuffer.
        /// </summary>
        void ResetExtents();

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        void ZoomIn();

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        void ZoomOut();

        /// <summary>
        /// Zooms to the next extent of the map frame
        /// </summary>
        void ZoomToNext();

        /// <summary>
        /// Zooms to the previous extent of the map frame
        /// </summary>
        void ZoomToPrevious();

		//CGX
        double ComputeScaleFromExtent();

        /// <summary>
        /// 
        /// </summary>
        void ComputeExtentFromScale(double dScale);

        /// <summary>
        /// 
        /// </summary>
        void ComputeExtentFromScale(double dScale, Point mousePosition);

        /// <summary>
        /// Gets or sets the map reference scale.
        /// </summary>
        double ReferenceScale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the map current scale.
        /// </summary>
        double CurrentScale
        {
            get;
            set;
        }
        /// <summary>
        /// gets or sets the map background color.
        /// </summary>
        Color BackgroundColor
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the map bookmarks.
        /// </summary>
        List<CBookmarks> Bookmarks
        {
            get;
            set;
        }
        //Fin CGX
        #endregion
    }
}