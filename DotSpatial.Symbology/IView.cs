// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A smaller interface that just gives the user enough control to draw on a map surface
    /// without giving them the ability to change the base control.
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface IView : IRenderable
    {
        #region Methods

        /// <summary>
        /// Causes the view to shift in geographic coordinates by the specified dX and dY.
        /// The geographic width and height of the Extents will remain the same, as
        /// will the aspect ratio.
        /// </summary>
        /// <param name="dX">Double specifying the pixel change in X position</param>
        /// <param name="dY">Double specifying the pixel change in Y position</param>
        void Pan(int dX, int dY);

        /// <summary>
        /// Expands or shrinks the envelope so that it's new size is the specified
        /// percent of the previous size.  The aspect ratio will be preserved.
        /// </summary>
        /// <param name="percent">The double percentage of the previous size</param>
        /// <example>
        /// Zoom in to a geographic extents half as large: Zoom(50);
        /// Zoom out to a geographic extents twice as large: Zoom(200);
        /// </example>
        void Zoom(double percent);

        #region PixelToProj

        /// <summary>
        /// Transforms a valid location on the screen (relative to the upper left corner of
        /// the map) into world coordinates (like longitude, latitude)
        /// </summary>
        /// <param name="pixelX">The integer pixel location relative to the left of the control </param>
        /// <param name="pixelY">The integer pixel location relative to the top of the control</param>
        /// <param name="x">The double horizontal or longitude coordinate</param>
        /// <param name="y">The double vertical or latitude coordinate</param>
        void PixelToProj(int pixelX, int pixelY, out double x, out double y);

        /// <summary>
        /// Transforms a valid location on the screen (relative to the upper left corner of
        /// the map) into world coordinates (like longitude, latitude)
        /// </summary>
        /// <param name="pixelPosition">A Point showing the PixelPosition</param>
        /// <returns>A Valid ICoordiante showing the location of the point in geographic coordinates</returns>
        Coordinate PixelToProj(Point pixelPosition);

        /// <summary>
        /// Transforms a valid location on the screen (relative to the upper left corner of
        /// the map) into world coordinates (like longitude, latitude)
        /// </summary>
        /// <param name="window">A Rectangle in pixel coordinates to find the geographic equivalent of</param>
        /// <returns>A Valid IEnvelope that shows the location on the screen</returns>
        Extent PixelToProj(Rectangle window);

        #endregion

        #region ProjToPixel

        /// <summary>
        /// Transforms the specified world location (like latitude, longitude) into a
        /// valid screen location (measured in pixels from the upper left corner of the map)
        /// </summary>
        /// <param name="x">The double horizontal or longitude coordinate</param>
        /// <param name="y">The double vertical or latitude coordinate</param>
        /// <param name="pixelX">The integer pixel location relative to the left of the control </param>
        /// <param name="pixelY">The integer pixel location relative to the top of the control</param>
        void ProjToPixel(double x, double y, out int pixelX, out int pixelY);

        /// <summary>
        /// Transforms the specified world location (like latitude, longitude) into a
        /// valid screen location (measured in pixels from the upper left corner of the map)
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        Point ProjToPixel(Coordinate coord);

        /// <summary>
        /// Transforms the specified world envelope (like latitude, longitude) into a
        /// valid screen rectangle (measured in pixels from the upper left corner of the map)
        /// </summary>
        /// <param name="env">The envelope in geographic coordinates</param>
        /// <returns>A Rectangle relative to the upper left corner of the control</returns>
        Rectangle ProjToPixel(Extent env);

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets a graphics surface that is essentially the "white box" area of a map to draw on.
        /// By default the GraphicsUnit should be set to world and the PageScale and PageOffset
        /// should be set to the appropriate value for world coordinates.
        /// </summary>
        Graphics Graphics
        {
            get;
        }

        /// <summary>
        /// Gets the IEnvelope representing the world coordinate extents of the map
        /// </summary>
        IEnvelope Extents
        {
            get;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the visible region being displayed on the map changes
        /// </summary>
        event EventHandler ExtentsChanged;

        #endregion
    }
}