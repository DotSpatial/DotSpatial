// ********************************************************************************************************
// Product Name: DotSpatial.Layout
// Description:  The DotSpatial LayoutText element, holds draws text for the layout
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

// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Controls a rectangle
    /// </summary>
    public class LayoutText : LayoutElement
    {
        private Color _color;
        private ContentAlignment _contentAlignment;
        private Font _font;
        private string _text;
        private TextRenderingHint _textHint = TextRenderingHint.SystemDefault;

        #region ------------------ Public Properties

        /// <summary>
        /// Gets or sets the text thats drawn in the graphics object
        /// </summary>
        [Browsable(true), Category("Symbol")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
        public string Text
        {
            get { return _text; }
            set { _text = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the content alignment
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public ContentAlignment ContentAlignment
        {
            get { return _contentAlignment; }
            set { _contentAlignment = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the font used to draw this text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public Font Font
        {
            get { return _font; }
            set { _font = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public Color Color
        {
            get { return _color; }
            set { _color = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public TextRenderingHint TextHint
        {
            get { return _textHint; }
            set { _textHint = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        #endregion

        #region ------------------- public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutText()
        {
            Name = "Text Box";
            _font = new Font("Arial", 10);
            _color = Color.Black;
            _text = "Text Box";
            _textHint = TextRenderingHint.AntiAliasGridFit;
            ResizeStyle = ResizeStyle.HandledInternally;
            _contentAlignment = ContentAlignment.TopLeft;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if printing to the file</param>
        public override void Draw(Graphics g, bool printing)
        {
            g.TextRenderingHint = _textHint;
            Brush colorBrush = new SolidBrush(_color);
            StringFormat sf = StringFormat.GenericDefault;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            switch (_contentAlignment)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }
            // Letters were getting truncated or else bumped to the
            // next line when printing that were not being bumped while
            // in the view.  the added letter, here hopefully will prevent
            // the discrepency.
            SizeF f = g.MeasureString("0", _font);
            RectangleF r = Rectangle;
            r.Width += f.Width;
            g.DrawString(_text, _font, colorBrush, r, sf);

            sf.Dispose();
            colorBrush.Dispose();
        }

        #endregion
    }
}