// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutRectangle
// Description:  The DotSpatial LayoutRectangle element, holds draws text for the layout
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
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// Brian Marchionni  | 3/15/2011  | With the addition of the polygon symbolizer on the base class the
//                   |            | outline and background were removed leaving the class empty
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A control that draws a standard colored rectangle to the print layout
    /// </summary>
    public class LayoutRectangle : LayoutElement
    {
        #region ------------------- public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutRectangle()
        {
            Name = "Rectangle";
            Background = new PolygonSymbolizer(Color.Transparent, Color.Black, 2.0);
            ResizeStyle = ResizeStyle.HandledInternally;
        }

        /// <summary>
        /// Doesn't need to do anything now because the drawing code is in the background property of the base class
        /// </summary>
        /// <param name="g"></param>
        /// <param name="printing"></param>
        public override void Draw(Graphics g, bool printing)
        {
        }

        #endregion
    }
}