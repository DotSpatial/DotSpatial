// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:26:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for Stroke.
    /// </summary>
    public interface IStroke : IDescriptor
    {
        #region Methods

        /// <summary>
        /// This is an optional expression that allows drawing to the specified GraphicsPath.
        /// Overriding this allows for unconventional behavior to be included, such as
        /// specifying marker decorations, rather than simply returning a pen. A pen
        /// is also returned publicly for convenience.
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="path">the GraphicsPath to draw</param>
        /// <param name="scaleWidth">This is 1 for symbolic drawing, but could be
        /// any number for geographic drawing.</param>
        void DrawPath(Graphics g, GraphicsPath path, double scaleWidth);

        /// <summary>
        /// Casts this stroke to the appropriate pen.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        Pen ToPen(double width);

        /// <summary>
        /// Gets a color to represent this line. If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns>The color.</returns>
        Color GetColor();

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        void SetColor(Color color);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the stroke style for this stroke
        /// </summary>
        StrokeStyle StrokeStyle { get; }

        #endregion
    }
}