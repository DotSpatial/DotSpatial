// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A basic map interface.
    /// </summary>
    public interface IBasicMap : ISelectable
    {
        #region Properties

        /// <summary>
        /// Gets the bounding rectangle representing this map in screen coordinates
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Gets an image that has been buffered
        /// </summary>
        Image BufferedImage { get; }

        /// <summary>
        /// Gets the client rectangle of the map control
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        Extent Extent { get; }

        /// <summary>
        /// Gets or sets the current tool mode. This rapidly enables or disables specific tools to give
        /// a combination of functionality. Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets which tool or combination of tools are enabled on the map.")]
        FunctionMode FunctionMode { get; set; }

        /// <summary>
        /// Gets the height of the control
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a map-function is currently interacting with the map.
        /// If this is true, then any tool-tip like popups or other mechanisms that require lots of re-drawing
        /// should suspend themselves to prevent conflict. Setting this actually increments an internal integer,
        /// so when that integer is 0, the map is "Not" busy, but multiple busy processes can work independently.
        /// </summary>
        bool IsBusy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Map is Zoomed out to full extent or not.
        /// Added 1/3/2013 by Eric Hullinger
        /// </summary>
        bool IsZoomedToMaxExtent { get; set; }

        /// <summary>
        /// Gets the screen coordinates of the
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Gets or sets the legend, if any, associated with this map control.
        /// </summary>
        ILegend Legend { get; set; }

        /// <summary>
        /// Gets the MapFrame.
        /// </summary>
        IFrame MapFrame { get; }

        /// <summary>
        /// Gets the screen coordinates of the top of this control.
        /// </summary>
        int Top { get; }

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        Extent ViewExtents { get; set; }

        /// <summary>
        /// Gets the width of the control.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets or sets a value indicating whether it is allowed to zoom out farther than the extent of the map. This is useful if we have only layers with small extents and want to look at them from farther out.
        /// </summary>
        bool ZoomOutFartherThanMaxExtent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new layer to the map using an open file dialog.
        /// </summary>
        /// <returns>The added layer.</returns>
        ILayer AddLayer();

        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        void ClearLayers();

        /// <summary>
        /// Returns a functional list of the ILayer members. This list will be separate from the actual list stored,
        /// but contains a shallow copy of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns>A list of layers.</returns>
        List<ILayer> GetLayers();

        /// <summary>
        /// Gets the MaxExtents of current Map.
        /// </summary>
        /// <param name="expand">Indicates whether the extent should be expanded by 10% to satisfy issue 84 (Expand target envelope by 10%).</param>
        /// <returns>The maximum extent of the map.</returns>
        // Added by Eric Hullinger 12/28/2012 for use in preventing zooming out too far.
        Extent GetMaxExtent(bool expand = false);

        /// <summary>
        /// Invalidates the entire Map control, forcing it to redraw itself from the back buffer stencils.
        /// This is good for drawing on top of the map, or when a layer is visible or not. If you need
        /// to change the colorscheme as well.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Invalidates the specified clipRectangle so that only that small region needs to redraw itself.
        /// </summary>
        /// <param name="clipRectangle">Clip rectangle that gets invalidated.</param>
        void Invalidate(Rectangle clipRectangle);

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate.
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        Coordinate PixelToProj(Point position);

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An Envelope interface</returns>
        Extent PixelToProj(Rectangle rect);

        /// <summary>
        /// Converts a point from screen coordinates to client coordinates.
        /// </summary>
        /// <param name="position">The Point representing the screen position</param>
        /// <returns>The Point</returns>
        Point PointToClient(Point position);

        /// <summary>
        /// Converts a point from client coordinates to screen coordinates.
        /// </summary>
        /// <param name="position">The client location.</param>
        /// <returns>A Point in screen coordinates</returns>
        Point PointToScreen(Point position);

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        Point ProjToPixel(Coordinate location);

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic Envelope</param>
        /// <returns>A Rectangle</returns>
        Rectangle ProjToPixel(Extent env);

        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle">The clip rectangle that gets updated.</param>
        void RefreshMap(Rectangle clipRectangle);

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        void ZoomToMaxExtent();

        #endregion
    }
}