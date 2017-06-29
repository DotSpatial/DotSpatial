// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/4/2009 11:37:04 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// System.Font is notoriously difficult when serialization comes up.  This replaces that
    /// with a more serializable version.
    /// </summary>
    [Serializable]
    public class TextFont : IDisposable
    {
        #region Private Variables

        private StringAlignment _alignment;
        private Brush _brush;
        private bool _brushValid;
        private Color _color;
        private string _familyName;
        private StringFormatFlags _flags;

        private Font _font;

        private bool _fontValid;
        private StringFormat _format;
        private bool _formatValid;
        private StringAlignment _lineAlignment;
        private float _size;
        private FontStyle _style;
        private StringTrimming _trimming;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default, black, 8pt sans serif font with a normal style.
        /// </summary>
        public TextFont()
        {
            Configure();
        }

        /// <summary>
        /// Creates a sans serif, black, normal font of the specified size.
        /// </summary>
        /// <param name="size">The size to use.</param>
        public TextFont(float size)
        {
            Configure();
            _size = size;
        }

        /// <summary>
        /// Creates a new instance of TextFont
        /// </summary>
        public TextFont(Font font, Color color)
        {
            Configure();
            _familyName = font.FontFamily.Name;
            _size = font.Size;
            _style = font.Style;
            _color = color;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="family"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public TextFont(FontFamily family, float size, FontStyle style, Color color)
        {
            Configure();
            _familyName = family.Name;
            _size = size;
            _style = style;
            _color = color;
        }

        private void Configure()
        {
            // brush
            _color = Color.Black;

            // font
            _familyName = FontFamily.GenericSansSerif.Name;
            _size = 8;
            _style = FontStyle.Regular;

            // format
            StringFormat temp = new StringFormat();
            _alignment = temp.Alignment;
            _flags = temp.FormatFlags;
            _lineAlignment = temp.LineAlignment;
            _trimming = temp.Trimming;
            temp.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Assigns the parameters from the specifed StringFormat class to the members of
        /// this TextFont.
        /// </summary>
        /// <param name="format">The StringFormat to apply to this object</param>
        public void SetFormat(StringFormat format)
        {
            _alignment = format.Alignment;
            _flags = format.FormatFlags;
            _lineAlignment = format.LineAlignment;
            _trimming = format.Trimming;
        }

        /// <summary>
        /// Sets the font on this Textfont to the specified value.
        /// </summary>
        /// <param name="font">The font to use.</param>
        public void SetFont(Font font)
        {
            _familyName = font.Name;
            _size = font.Size;
            _style = font.Style;
            _font = font;
            _fontValid = true;
        }

        /// <summary>
        /// This returns the actual internal font.  Be careful not to dispose this.
        /// </summary>
        /// <returns>A System.Font</returns>
        public Font GetFont()
        {
            Setup(); // in case the font is not yet valid
            return _font;
        }

        /// <summary>
        /// Draws the specified text to the specified graphics object in the specified location,
        /// but using all of the parameters specified by this TextFont object.
        /// </summary>
        /// <param name="g">The Graphics surface to draw to</param>
        /// <param name="text">The string text to draw</param>
        /// <param name="x">The x coordinate of the top left position</param>
        /// <param name="y">The y coordinate of the top left position</param>
        public void Draw(Graphics g, string text, float x, float y)
        {
            Setup();
            OnDraw(g, text, x, y);
        }

        /// <summary>
        /// Draws the specified text to the specified graphics object in the specified location,
        /// but using all of the parameters specified by this TextFont object.
        /// </summary>
        /// <param name="g">The Graphics surface to draw to</param>
        /// <param name="text">The string text to draw</param>
        /// <param name="location">The PointF describing the location to draw</param>
        public void Draw(Graphics g, string text, PointF location)
        {
            Setup();
            OnDraw(g, text, location.X, location.Y);
        }

        /// <summary>
        /// Handles drawing for point location drawing
        /// </summary>
        /// <param name="g">The Graphics surface to draw to.</param>
        /// <param name="text">The string to draw</param>
        /// <param name="x">The x floating point value</param>
        /// <param name="y">The y floating point value</param>
        protected virtual void OnDraw(Graphics g, string text, float x, float y)
        {
            g.DrawString(text, _font, _brush, x, y, _format);
        }

        /// <summary>
        /// Handles drawing for drawing that falls within a rectangleF structure.
        /// </summary>
        /// <param name="g">The Graphics surface to draw to</param>
        /// <param name="text">The string to draw</param>
        /// <param name="box">The RectangleF structure</param>
        protected virtual void OnDraw(Graphics g, string text, RectangleF box)
        {
            g.DrawString(text, _font, _brush, box, _format);
        }

        private void Setup()
        {
            if (!_brushValid || _brush == null)
            {
                if (_brush != null) _brush.Dispose();
                _brush = new SolidBrush(_color);
            }
            if (!_fontValid || _font == null)
            {
                if (_font != null) _font.Dispose();
                _font = new Font(_familyName, _size, _style);
            }
            if (!_formatValid || _format == null)
            {
                if (_format != null) _format.Dispose();
                _format = new StringFormat();
                _format.Alignment = _alignment;
                _format.FormatFlags = _flags;
                _format.LineAlignment = _lineAlignment;
                _format.Trimming = _trimming;
            }
        }

        /// <summary>
        /// Draws the specified text to the specified graphics object within the specified box.
        /// </summary>
        /// <param name="g">The graphics surface to draw to.</param>
        /// <param name="text">The text to draw</param>
        /// <param name="box">The rectangular box to draw within</param>
        public void Draw(Graphics g, string text, RectangleF box)
        {
            Setup();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the alignment information on the vertical plane
        /// </summary>
        [Serialize("Alignment")]
        public StringAlignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                _formatValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the System.Color to use for the font color.
        /// </summary>
        [Serialize("Color")]
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _brushValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the string family name for this font.
        /// </summary>
        [Serialize("FamilyName")]
        public string FamilyName
        {
            get { return _familyName; }
            set
            {
                _familyName = value;
                _fontValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the string format flags
        /// </summary>
        [Serialize("FormatFlags")]
        public StringFormatFlags FormatFlags
        {
            get { return _flags; }
            set
            {
                _flags = value;
                _formatValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the line alignment on the horizontal plane
        /// </summary>
        [Serialize("LineAlignment")]
        public StringAlignment LineAlignment
        {
            get { return _lineAlignment; }
            set
            {
                _lineAlignment = value;
                _formatValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the floating point value controling the size.
        /// </summary>
        [Serialize("Size")]
        public float Size
        {
            get { return _size; }
            set
            {
                _size = value;
                _fontValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the style
        /// </summary>
        [Serialize("Style")]
        public FontStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                _fontValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the StringTrimming options
        /// </summary>
        [Serialize("Trimming")]
        public StringTrimming Trimming
        {
            get { return _trimming; }
            set
            {
                _trimming = value;
                _formatValid = false;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of the font, brush and format that are stored internally
        /// </summary>
        public void Dispose()
        {
            if (_font != null) _font.Dispose();
            if (_brush != null) _brush.Dispose();
            if (_format != null) _format.Dispose();
        }

        #endregion
    }
}