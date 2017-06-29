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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 3:52:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The draw interface is used to add custom drawing to the DotSpatial view.
    /// </summary>
    public interface IDraw
    {
        #region Methods

        /// <summary>
        /// Clears all custom drawing on the specified drawing layer.
        /// </summary>
        /// <param name="drawHandle">Handle of the drawing layer to clear.</param>
        /// <remarks>Clearing a single drawing (using the drawing handle) is faster than clearing all drawings.</remarks>
        void ClearDrawing(int drawHandle);

        /// <summary>
        /// Clears all custom drawings on all drawing layers.
        /// </summary>
        /// <remarks>Clearing a single drawing (using the drawing handle) is faster than clearing all drawings.</remarks>
        void ClearDrawings();

        /// <summary>
        /// Draws a circle on the current drawing layer.
        /// </summary>
        /// <param name="x">X coordinate of the center of the circle.</param>
        /// <param name="y">Y coordinate of the center of the circle.</param>
        /// <param name="pixelRadius">Radius of the circle in pixels.</param>
        /// <param name="color">Color to use when drawing the circle.</param>
        /// <param name="fillCircle">Specifies whether or not the circle is drawn filled.</param>
        void DrawCircle(double x, double y, double pixelRadius, Color color, bool fillCircle);

        /// <summary>
        /// Draws a line on the current drawing layer.
        /// </summary>
        /// <param name="x1">First x coordinate.</param>
        /// <param name="y1">First y coordinate.</param>
        /// <param name="x2">Second x coordinate.</param>
        /// <param name="y2">Second y coordinate.</param>
        /// <param name="pixelWidth">Width of the line in pixels</param>
        /// <param name="color">Color to draw the line with.</param>
        void DrawLine(double x1, double y1, double x2, double y2, int pixelWidth, Color color);

        /// <summary>
        /// Draws a point on the current drawing layer.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="pixelSize">Size of the point in pixels.</param>
        /// <param name="color">Color to draw the point with.</param>
        void DrawPoint(double x, double y, int pixelSize, Color color);

        /// <summary>
        /// Draws a polygon on the current drawing layer.
        /// </summary>
        /// <param name="x">Array of x points for the polygon.</param>
        /// <param name="y">Array of y points for the polygon.</param>
        /// <param name="color">Color to draw the polygon with.</param>
        /// <param name="fillPolygon">Specifies whether or not to fill the polygon.</param>
        /// <remarks>The points in a polygon should be defined in a clockwise order and have no
        /// crossing lines if they are to be filled.  The first and last point should be the same.</remarks>
        void DrawPolygon(double[] x, double[] y, Color color, bool fillPolygon);

        /// <summary>
        /// Creates a new drawing layer.
        /// </summary>
        /// <param name="projection">Specifies whether to use screen coordinates or projected map coordinates.</param>
        /// <returns>Returns a drawing handle.  You should save this handle if you wish to clear this drawing later.</returns>
        /// <remarks>The concept of drawing layers is only partially implemented in this version of the MapWinGIS, which is used
        /// by the DotSpatial.  There is only one active drawing layer, which is the most recently created one.  There is no
        /// way to access any other drawing layers other than the current one.</remarks>
        int NewDrawing(ReferenceType projection);

        #endregion

        #region Properties

        /// <summary>
        /// Specifies whether or not to use double buffering.  Double buffering makes the drawing of the
        /// custom drawings smoother (not flickering). It is recommended that you use double buffering.
        /// </summary>
        bool DoubleBuffer { get; set; }

        #endregion
    }
}