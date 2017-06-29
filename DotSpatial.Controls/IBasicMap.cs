// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/7/2008 10:09:46 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// IBasicMap
    /// </summary>
    public interface IBasicMap : IMapView, ISelectable
    {
        #region Methods

        /// <summary>
        /// Adds a new layer to the map using an open file dialog.
        /// </summary>
        ILayer AddLayer();

        /// <summary>
        /// Converts a point from client coordinates to screen coordinates.
        /// </summary>
        /// <param name="position">The client location.</param>
        /// <returns>A Point in screen coordinates</returns>
        Point PointToScreen(Point position);

        /// <summary>
        /// Converst a point from screen coordinates to client coordinates
        /// </summary>
        /// <param name="position">The Point representing the screen position</param>
        /// <returns>The Point</returns>
        Point PointToClient(Point position);

        /// <summary>
        /// Invalidates the entire Map control, forcing it to redraw itself from the back buffer stencils.
        /// This is good for drawing on top of the map, or when a layer is visible or not.  If you need
        /// to change the colorscheme as well
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Invalidates the specified clipRectangle so that only that small region needs
        /// to redraw itself.
        /// </summary>
        /// <param name="clipRectangle"></param>
        void Invalidate(Rectangle clipRectangle);

        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle"></param>
        void RefreshMap(Rectangle clipRectangle);

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        void ZoomToMaxExtent();

        /// <summary> 
        /// //  Added by Eric Hullinger 12/28/2012 for use in preventing zooming out too far.
        /// Gets the MaxExtents of current Map.
        /// </summary>
        Extent GetMaxExtent();


        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounding rectangle representing this map in screen coordinates
        /// </summary>
        Rectangle Bounds
        {
            get;
        }

        /// <summary>
        /// Gets an image that has been buffered
        /// </summary>
        Image BufferedImage
        {
            get;
        }

        /// <summary>
        /// Gets the client rectangle of the map control
        /// </summary>
        Rectangle ClientRectangle
        {
            get;
        }

        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets which tool or combination of tools are enabled on the map.")]
        FunctionMode FunctionMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        Extent Extent
        {
            get;
        }

        /// <summary>
        /// Gets the height of the control
        /// </summary>
        int Height
        {
            get;
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether a map-function is currently interacting with the map.
        /// If this is true, then any tool-tip like popups or other mechanisms that require lots of re-drawing
        /// should suspend themselves to prevent conflict.  Setting this actually increments an internal integer,
        /// so when that integer is 0, the map is "Not" busy, but multiple busy processes can work independently.
        /// </summary>
        bool IsBusy
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the Map is Zoomed out to full extent or not.
        /// Added 1/3/2013 by Eric Hullinger
        /// </summary>
        bool IsZoomedToMaxExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the screen coordinates of the
        /// </summary>
        int Left
        {
            get;
        }

        /// <summary>
        /// Gets the legend, if any, associated with this map control.
        /// </summary>
        ILegend Legend
        {
            get;
            set;
        }

        /// <summary>
        /// A MapFrame
        /// </summary>
        IFrame MapFrame
        {
            get;
        }

        /// <summary>
        /// Gets the screen coordinates of the top of this control
        /// </summary>
        int Top
        {
            get;
        }

        /// <summary>
        /// Gets the width of the control
        /// </summary>
        int Width
        {
            get;
        }

        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        void ClearLayers();

        /// <summary>
        /// returns a functional list of the ILayer members.  This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns></returns>
        List<ILayer> GetLayers();

        #endregion
    }
}