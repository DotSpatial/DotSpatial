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
    /// LineDecoration
    /// </summary>
    public interface ILineDecoration : IDescriptor
    {
        #region Methods

        /// <summary>
        /// Given the points on this line decoration, this will cycle through and handle
        /// the drawing as dictated by this decoration.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="scaleWidth">The double scale width for controling markers</param>
        void Draw(Graphics g, GraphicsPath path, double scaleWidth);

        /// <summary>
        /// Gets the size that is needed to draw this decoration with max. 2 symbols.
        /// </summary>
													  
        Size GetLegendSymbolSize();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the decorative symbol
        /// </summary>
        IPointSymbolizer Symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, flips the first symbol in relation
        /// to the direction of the line.
        /// </summary>
        bool FlipFirst
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, reverse one symbols on 2
        /// </summary>
								   

					 
																									 
					  
									

					 
																						
					  
        bool Flip1on2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, reverses all of the symbols
        /// </summary>
        bool FlipAll
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, will cause the symbol to
        /// be rotated according to the direction of the line.  Arrows
        /// at the ends, for instance, will point along the direction of
        /// the line, regardless of the direction of the line.
        /// </summary>
        bool RotateWithLine
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a boolean that, if true, will cause the symbol to 
        /// be spaced according to the spacing value.
        /// </summary>
        bool UseSpacing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of symbols that should be drawn on each
        /// line.  (not each segment).
        /// </summary>
        int NumSymbols
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the spacing between each line decoration.
        /// </summary>
        float Spacing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit used by the spacing (mm or inch).
        /// </summary>
        string SpacingUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the percentual position between line start and end at which the single decoration gets drawn.
        /// </summary>
        int PercentualPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the offset distance measured to the left of the line in pixels.
													 
        /// </summary>
        double Offset
        {
            get;
            set;
        }

        #endregion
    }
}