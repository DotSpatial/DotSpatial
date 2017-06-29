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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 1:17:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPolygonSymbolizer
    /// </summary>
    public interface IPolygonSymbolizer : IFeatureSymbolizer
    {
        #region Methods

        /// <summary>
        /// Gets the color of the top-most pattern, if it is a simple pattern,
        /// or return Color.Empty otherwise
        /// </summary>
        Color GetFillColor();

        /// <summary>
        /// Sets the color, forcing a simple pattern if necessary
        /// </summary>
        /// <param name="color">Gets the color of the top-most pattern</param>
        void SetFillColor(Color color);

        /// <summary>
        /// This gets the largest width of all the strokes of the outlines of all the patterns.  Setting this will
        /// forceably adjust the width of all the strokes of the outlines of all the patterns.
        /// </summary>
        double GetOutlineWidth();

        /// <summary>
        /// Forces the specified width to be the width of every stroke outlining every pattern.
        /// </summary>
        /// <param name="width">The width to force as the outline width</param>
        void SetOutlineWidth(double width);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the method for drawing the lines that make up the borders of this polygon
        /// </summary>
        ILineSymbolizer OutlineSymbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets the list of patterns to use for filling polygons.
        /// </summary>
        IList<IPattern> Patterns
        {
            get;
            set;
        }

        #endregion
    }
}