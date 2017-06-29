// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
    /// IPointSymbolizer
    /// </summary>
    public interface IPointSymbolizer : IFeatureSymbolizer
    {
        #region Methods

        /// <summary>
        /// Draws the symbol to the specified graphics object.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize"></param>
        void Draw(Graphics g, double scaleSize);

        /// <summary>
        /// Multiplies all the linear measurements, like width, height, and offset values by the specified value.
        /// </summary>
        /// <param name="value">The double precision value to multiply all of the values against.</param>
        void Scale(double value);

        /// <summary>
        /// Gets the maximum size for all of the layers, modified by the offsets.  This, in essence, represents the bounds
        /// for the entire symbol.
        /// </summary>
        Size2D GetSize();

        /// <summary>
        /// This assumes that you wish to simply scale the various sizes.
        /// It will adjust all of the sizes so that the maximum size is
        /// the same as the specified size.
        /// </summary>
        /// <param name="value">The Size2D of the new maximum size</param>
        void SetSize(Size2D value);

        /// <summary>
        /// Gets the color of the top-most symbol
        /// </summary>
        /// <returns>The color of the top-most symbol</returns>
        Color GetFillColor();

        /// <summary>
        /// Sets the color of the top-most layer symbol
        /// </summary>
        /// <param name="color">The color to assign to the top-most layer.</param>
        void SetFillColor(Color color);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of symbols.
        /// </summary>
        IList<ISymbol> Symbols
        {
            get;
            set;
        }

        #endregion
    }
}