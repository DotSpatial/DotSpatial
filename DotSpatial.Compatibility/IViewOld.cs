// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:00:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Topology;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The View interface is used to manipulate or work with the main DotSpatial display.
    /// </summary>
    public interface IViewOld
    {
        /// <summary>
        /// Determines whether the preview map panel is visible.
        /// </summary>
        bool PreviewVisible { get; set; }

        /// <summary>
        /// Determines whether the legend panel is visible.
        /// </summary>
        bool LegendVisible { get; set; }

        /// <summary>
        /// Gets or sets the background color of the map.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the current cursor mode.  The cursor mode can be any of the following:
        /// <list type="bullet">
        /// <item>None</item>
        /// <item>Pan</item>
        /// <item>Selection</item>
        /// <item>ZoomIn</item>
        /// <item>ZoomOut</item>
        /// </list>
        /// </summary>
        CursorMode CursorMode { get; set; }

        /// <summary>
        /// Returns a <c>DotSpatial.Interfaces.Draw</c> interface used to add custom drawing to the map.
        /// </summary>
        IDraw Draw { get; }

        /// <summary>
        /// Gets or sets the amount to pad around the extents when calling <c>ZoomToMaxExtents</c>,
        /// <c>ZoomToLayer</c>, and <c>ZoomToShape</c>.
        /// </summary>
        double ExtentPad { get; set; }

        /// <summary>
        /// Gets or sets the map's current extents.
        /// </summary>
        IEnvelope Extents { get; set; }

        /// <summary>
        /// Gets or sets the cursor to use on the map.  The enumeration can be any of the following:
        /// <list type="bullet">
        /// <item>crsrAppStarting</item>
        /// <item>crsrArrow</item>
        /// <item>crsrCross</item>
        /// <item>crsrHelp</item>
        /// <item>crsrIBeam</item>
        /// <item>crsrMapDefault</item>
        /// <item>crsrNo</item>
        /// <item>crsrSizeAll</item>
        /// <item>crsrSizeNESW</item>
        /// <item>crsrSizeNS</item>
        /// <item>crsrSizeNWSE</item>
        /// <item>crsrSizeWE</item>
        /// <item>crsrUpArrow</item>
        /// <item>crsrUserDefined</item>
        /// <item>crsrWait</item>
        /// </list>
        /// </summary>
        MapCursorMode MapCursor { get; set; }

        /// <summary>
        /// Indicates that the map should handle file drag-drop events (as opposed to firing a message indicating file(s) were dropped).
        /// </summary>
        bool HandleFileDrop { get; set; }

        /// <summary>
        /// Gets or sets the <c>MapState</c> string which describes in a single string the entire
        /// map state, including layers and coloring schemes.
        /// </summary>
        string MapState { get; set; }

        /// <summary>
        /// Returns a <c>SelectInfo</c> object containing information about all shapes that are currently selected.
        /// </summary>
        ISelectInfo SelectedShapes { get; }

        /// <summary>
        /// Determines whether labels are loaded from and saved to a project-specific label file or from a shapefile-specific label file. Using a project-level label file will create a new subdirectory with the project's name.
        /// </summary>
        bool LabelsUseProjectLevel { get; set; }

        /// <summary>
        /// Gets or sets whether selection should be persistent.  If selection is persistent, previously selected
        /// shapes are not cleared before selecting the new shapes.  When selection is persistent you must select
        /// nothing to clear the selection.  The default value for this property is false.  When selection is not
        /// persistent, all selected shapes are cleared between selection routines unless the user is holding down
        /// a control or shift key.
        /// </summary>
        bool SelectionPersistence { get; set; }

        /// <summary>
        /// The tolerance, in projected map units, to use for selection.
        /// </summary>
        double SelectionTolerance { get; set; }

        /// <summary>
        /// Gets or sets the selection method to use.
        /// <list type="bullet">
        /// <item>Inclusion</item>
        /// <item>Intersection</item>
        /// </list>
        /// Inclusion means that the entire shape must be within the selection bounds in order to select
        /// the shape.  Intersection means that only a portion of the shape must be within the selection
        /// bounds in order for the shape to be selected.
        /// </summary>
        SelectMode SelectMethod { get; set; }

        /// <summary>
        /// Gets or sets the color used to indicate a selected shape.
        /// </summary>
        Color SelectColor { get; set; }

        /// <summary>
        /// Gets or sets a tag for the map.  The tag is a string variable that can be used by a developer
        /// to store any information they desire.
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        /// Gets or sets the handle of the cursor to use when the <c>CursorMode</c> is <c>cmUserDefined</c>.
        /// </summary>
        int UserCursorHandle { get; set; }

        /// <summary>
        /// Gets or sets the default zoom percentage to use when interacting with the map using a mouse.
        /// </summary>
        double ZoomPercent { get; set; }

        /// <summary>
        /// Clears all selected shapes from the DotSpatial, reverting them to their original colors.
        /// </summary>
        void ClearSelectedShapes();

        /// <summary>
        /// Queries all of the active shapefile layers for any within the specified tolerance of the given point.
        /// </summary>
        /// <param name="projX">The x coordinate, in projected map units, of the point to query.</param>
        /// <param name="projY">The y coordinate, in projected map units, of the point to query.</param>
        /// <param name="tolerance">Tolerance is the distance, in projected map units, around the point to include in the query.</param>
        /// <returns>Returns an <c>IdentifiedLayers</c> object containing query results.</returns>
        IIdentifiedLayers Identify(double projX, double projY, double tolerance);

        /// <summary>
        /// Prevents the legend from showing any changes made to it until the <c>UnlockLegend</c> function is called.
        /// The legend maintains a count of the number of locks it has, only redrawing if there are no locks.
        /// </summary>
        void LockLegend();

        /// <summary>
        /// Unlocking the Legend allows it to redraw and update the view to reflect any changes that were made.
        /// The legend maintains a count of the number of locks it has, only redrawing if there are no locks.
        /// There can never be a negative number of locks.
        /// </summary>
        void UnlockLegend();

        /// <summary>
        /// Prevents the map from updating due to any changes made to the layers that are loaded until the <c>UnlockMap</c> function is called.
        /// The map maintains a count of the number of locks it has, only redrawing if there are no locks.
        /// </summary>
        void LockMap();

        /// <summary>
        /// Allows the map to redraw entirely when changes are made.
        /// The map maintains a count of the number of locks it has, only redrawing if there are no locks.
        /// There can never be a negative number of locks.
        /// </summary>
        void UnlockMap();

        /// <summary>
        /// Converts a point in pixel coordinates to a point in projected map units.
        /// </summary>
        /// <param name="pixelX">X coordinate of the original point in pixels.</param>
        /// <param name="pixelY">Y coordinate of the original point in pixels.</param>
        /// <param name="projX">ByRef.  X coordinate of the projected point.</param>
        /// <param name="projY">ByRef.  Y coordinate of the projected point.</param>
        void PixelToProj(double pixelX, double pixelY, ref double projX, ref double projY);

        /// <summary>
        /// Converts a point in projected map units to a point in screen coordinates.
        /// </summary>
        /// <param name="projX">X coordinate of the projected map point.</param>
        /// <param name="projY">Y coordinate of the projected map point.</param>
        /// <param name="pixelX">ByRef. X coordinate of the screen point in pixels.</param>
        /// <param name="pixelY">ByRef. Y coordinate of the screen point in pixels.</param>
        void ProjToPixel(double projX, double projY, ref double pixelX, ref double pixelY);

        /// <summary>
        /// Forces the map to redraw.  This function has no effect if the map is locked.
        /// </summary>
        void Redraw();

        /// <summary>
        /// Shows a tooltip under the cursor on the map.
        /// </summary>
        /// <param name="text">The text to display in the tooltip.</param>
        /// <param name="milliseconds">Number of milliseconds before the tooltip automatically disappears.</param>
        void ShowToolTip(string text, int milliseconds);

        /// <summary>
        /// Zooms the view to the maximum extents of all the loaded and visible layers.
        /// </summary>
        void ZoomToMaxExtents();

        /// <summary>
        /// Zooms the display in by the given factor.
        /// </summary>
        /// <param name="percent">The percentage to zoom by.</param>
        void ZoomIn(double percent);

        /// <summary>
        /// Zooms the display out by the given factor.
        /// </summary>
        /// <param name="percent">The percentage to zoom by.</param>
        void ZoomOut(double percent);

        /// <summary>
        /// Zooms to the previous extent.
        /// </summary>
        void ZoomToPrev();

        /// <summary>
        /// Takes a snapshot of the currently visible layers at the extents specified.
        /// </summary>
        /// <param name="bounds">The area to take the snapshot of.</param>
        Image Snapshot(IEnvelope bounds);

        /// <summary>
        /// Triggers reloading of field values for the given layer. No action will occur if the layer handle is invalid, not a shapefile, or has no labels.
        /// </summary>
        /// <param name="layerHandle"></param>
        void LabelsRelabel(int layerHandle);

        /// <summary>
        /// Displays the label editor form for the specified layer. No action will occur if the layer handle is invalid or not a shapefile.
        /// </summary>
        /// <param name="layerHandle"></param>
        void LabelsEdit(int layerHandle);

        /// <summary>
        /// Selects shapes in the mapwindow from the specified point.  The tolerance used is the global
        /// tolerance set through the <c>View.SelectionTolerance</c>.  This function uses the same selection
        /// routines that are called when a user selects with a mouse.
        /// </summary>
        /// <param name="screenX">The x coordinate in pixels of the location to select.</param>
        /// <param name="screenY">The y coordinate in pixels of the location to select.</param>
        /// <param name="clearOldSelection">Specifies whether to clear all previously selected shapes or not.</param>
        ISelectInfo Select(int screenX, int screenY, bool clearOldSelection);

        /// <summary>
        /// Selects shapes in the mapwindow from the specified rectangle.  This function uses the
        /// same selection routines that are called when a user selects with a mouse.
        /// </summary>
        /// <param name="screenBounds">The rectangle to select shapes with.  This rectangle must be in screen (pixel) coordinates.</param>
        /// <param name="clearOldSelection">Specifies whether to clear all previously selected shapes or not.</param>
        ISelectInfo Select(Rectangle screenBounds, bool clearOldSelection);
    }
}