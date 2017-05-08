// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 4:53:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LineDecoration.
    /// </summary>
    public interface ILineDecoration : IDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether all of the symbols should be flipped.
        /// </summary>
        bool FlipAll { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the first symbol in relation
        /// to the direction of the line should be flipped.
        /// </summary>
        bool FlipFirst { get; set; }

        /// <summary>
        /// Gets or sets the number of symbols that should be drawn on each line. (not each segment).
        /// </summary>
        int NumSymbols { get; set; }

        /// <summary>
        /// Gets or sets the offset distance measured to the left of the line in pixels.
        /// </summary>
        double Offset { get; set; }

        /// <summary>
        /// Gets or sets the percentual position between line start and end at which the single decoration gets drawn.
        /// </summary>
        int PercentualPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the symbol should be rotated according to the direction of the line.
        /// Arrows at the ends, for instance, will point along the direction of the line, regardless of the direction of the line.
        /// </summary>
        bool RotateWithLine { get; set; }

        /// <summary>
        /// Gets or sets the decorative symbol.
        /// </summary>
        IPointSymbolizer Symbol { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Given the points on this line decoration, this will cycle through and handle
        /// the drawing as dictated by this decoration.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="path">The path that gets drawn.</param>
        /// <param name="scaleWidth">The double scale width for controling markers</param>
        void Draw(Graphics g, GraphicsPath path, double scaleWidth);

        /// <summary>
        /// Gets the size that is needed to draw this decoration with max. 2 symbols.
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        Size GetLegendSymbolSize();

        #endregion
    }
}