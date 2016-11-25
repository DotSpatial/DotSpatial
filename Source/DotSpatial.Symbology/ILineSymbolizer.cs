// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 2:20:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public interface ILineSymbolizer : IFeatureSymbolizer
    {
        #region Methods

        /// <summary>
        /// Draws the line that is needed to show this lines legend symbol.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="target">The rectangle that is used to calculate the lines position and size.</param>
        void DrawLegendSymbol(Graphics g, Rectangle target);

        /// <summary>
        /// Sequentially draws all of the strokes using the specified graphics path.
        /// </summary>
        /// <param name="g">The graphics device to draw to</param>
        /// <param name="gp">The graphics path that describes the pathway to draw</param>
        /// <param name="scaleWidth">The double scale width that when multiplied by the width gives a measure in pixels</param>
        void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth);

        /// <summary>
        /// Gets the color of the top-most stroke.
        /// </summary>
        Color GetFillColor();

        /// <summary>
        /// Sets the fill color fo the top-most stroke, and forces the top-most stroke
        /// to be a type of stroke that can accept a fill color if necessary.
        /// </summary>
        /// <param name="fillColor"></param>
        void SetFillColor(Color fillColor);

        /// <summary>
        /// This gets the largest width of all the strokes.
        /// Setting this will change the width of all the strokes to the specified width, and is not recommended
        /// if you are using thin lines drawn over thicker lines.
        /// </summary>
        double GetWidth();

        /// <summary>
        /// This forces the width to exist across all the strokes in this symbolizer.
        /// </summary>
        void SetWidth(double width);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of strokes that will be combined to make up a single drawing pass for this line.
        /// </summary>
        IList<IStroke> Strokes { get; set; }

        #endregion
    }
}