// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/7/2008 10:21:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    public interface IMapView
    {
        #region Methods

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        Coordinate PixelToProj(Point position);

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An Envelope interface</returns>
        Extent PixelToProj(Rectangle rect);

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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        Extent ViewExtents { get; set; }

        #endregion
    }
}