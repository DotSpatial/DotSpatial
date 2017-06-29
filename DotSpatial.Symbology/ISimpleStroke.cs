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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:24:54 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ISimpleStroke
    /// </summary>
    public interface ISimpleStroke : IStroke
    {
        #region Properties

        /// <summary>
        /// Gets or sets the color for this drawing layer
        /// </summary>
        Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the DashStyle for this stroke.  (Custom is just solid for simple strokes)
        /// </summary>
        DashStyle DashStyle
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets the opacity of the color.  1 is fully opaque while 0 is fully transparent.
        /// </summary>
        float Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of this line.  In geographic ScaleMode,
        /// this width is the actual geographic width of the line.  In Symbolic scale mode
        /// this is the width of the line in pixels.
        /// </summary>
        double Width
        {
            get;
            set;
        }

        #endregion
    }
}