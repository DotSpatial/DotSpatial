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
// The Initial Developer of this Original Code is Brian Marchionni. Created 9/06/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// HatchPattern
    /// </summary>
    public class HatchPattern : Pattern, IHatchPattern
    {
        #region Private Variables

        Color _backColor;
        Color _foreColor;
        HatchStyle _hatchStyle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HatchPattern
        /// </summary>
        public HatchPattern()
        {
            HatchStyle = HatchStyle.Vertical;
            _foreColor = Color.Black;
            _backColor = Color.Transparent;
        }

        /// <summary>
        /// Creates a new HatchPattern with the specified image
        /// </summary>
        /// <param name="style">The hatch style to use</param>
        ///<param name="foreColor">the forecolor to use</param>
        ///<param name="backColor">the background color to use</param>
        public HatchPattern(HatchStyle style, Color foreColor, Color backColor)
        {
            HatchStyle = style;
            _foreColor = foreColor;
            _backColor = backColor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs the drawing code to fill the specified path with the specified pattern
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="gp">The GraphicsPath to fill</param>
        public override void FillPath(Graphics g, GraphicsPath gp)
        {
            HatchBrush hb = new HatchBrush(_hatchStyle, _foreColor, _backColor);
            g.FillPath(hb, gp);
            hb.Dispose();
            base.FillPath(g, gp);
        }

        /// <summary>
        /// Gets the forecolor
        /// </summary>
        /// <returns></returns>
        public override Color GetFillColor()
        {
            return _foreColor;
        }

        /// <summary>
        /// Sets the foreColor to the specified color
        /// </summary>
        /// <param name="color"></param>
        public override void SetFillColor(Color color)
        {
            _foreColor = color;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the hatch style
        /// </summary>
        [Serialize("HatchStyle")]
        public HatchStyle HatchStyle
        {
            get { return _hatchStyle; }
            set { _hatchStyle = value; }
        }

        /// <summary>
        /// Gets or sets the fore color of the hatch pattern
        /// </summary>
        [Serialize("ForeColor")]
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        [Serialize("ForeColorOpacity")]
        public float ForeColorOpacity
        {
            get
            { return _foreColor.GetOpacity(); }
            set
            { _foreColor = _foreColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets the background color of the hatch pattern
        /// </summary>
        [Serialize("BackColor")]
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        [Serialize("BackColorOpacity")]
        public float BackColorOpacity
        {
            get
            { return _backColor.GetOpacity(); }
            set
            { _backColor = _backColor.ToTransparent(value); }
        }

        #endregion
    }
}