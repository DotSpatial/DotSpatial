// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 1:08:56 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for PointSymbolizer.
    /// </summary>
    public interface IPointSymbolizer : IFeatureSymbolizer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of symbols.
        /// </summary>
        IList<ISymbol> Symbols { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the point symbol to the specified graphics object by cycling through each of the layers and
        /// drawing the content. This assumes that the graphics object has been translated to the specified point.
        /// </summary>
        /// <param name="g">Graphics object that is used for drawing.</param>
        /// <param name="scaleSize">Scale size represents the constant to multiply to the geographic measures in order to turn them into pixel coordinates </param>
        void Draw(Graphics g, double scaleSize);

        /// <summary>
        /// Gets the color of the top-most symbol
        /// </summary>
        /// <returns>The color of the top-most symbol</returns>
        Color GetFillColor();

        /// <summary>
        /// Gets the maximum size for all of the layers, modified by the offsets. This, in essence, represents the bounds
        /// for the entire symbol.
        /// </summary>
        /// <returns>The maximum size.</returns>
        Size2D GetSize();

        /// <summary>
        /// Multiplies all the linear measurements, like width, height, and offset values by the specified value.
        /// </summary>
        /// <param name="value">The double precision value to multiply all of the values against.</param>
        void Scale(double value);

        /// <summary>
        /// Sets the color of the top-most layer symbol
        /// </summary>
        /// <param name="color">The color to assign to the top-most layer.</param>
        void SetFillColor(Color color);

        /// <summary>
        /// This assumes that you wish to simply scale the various sizes.
        /// It will adjust all of the sizes so that the maximum size is
        /// the same as the specified size.
        /// </summary>
        /// <param name="value">The Size2D of the new maximum size</param>
        void SetSize(Size2D value);

        #endregion
    }
}