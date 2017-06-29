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

/*using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;*/

// CGX
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
// CGX END

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

        //CGX
        private int _iAngle = 0;
        private double _dCurvedTextRadius = 0;
        private double _dCurvedStartAngle = 90;
        private bool _bAutoSize = true;
        // CGX END

        #region ------------------ Public Properties

        /// <summary>
        /// Gets or sets the text thats drawn in the graphics object
        /// </summary>
        [Browsable(true), Category("Symbol")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
        public string Text
        {
            get { return _text; }
            //set { _text = value; base.UpdateThumbnail(); base.OnInvalidate(); }
            // CGX
            set { _text = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
            // Fin CGX
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
            //set { _font = value; base.UpdateThumbnail(); base.OnInvalidate(); }
            // CGx
            set { _font = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
            // Fin CGX
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

        // CGX

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public int Angle
        {
            get { return _iAngle; }
            // CGX
            set { _iAngle = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
            // Fin CGX
        }

        // CGX

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Curved Text")]
        public double Radius
        {
            get { return _dCurvedTextRadius; }
            set { _dCurvedTextRadius = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Curved Text")]
        public double StartAngle
        {
            get { return _dCurvedStartAngle; }
            set { _dCurvedStartAngle = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public bool Autosize
        {
            get { return _bAutoSize; }
            set { _bAutoSize = value; this.UpdateSize(); base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        // Fin CGX

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
            //CGX
            if (!Visible) return;
            g.TextRenderingHint = _textHint;
            Brush colorBrush = new SolidBrush(_color);

            // Récupération de l'alignement
            StringFormat sf = GetAlignement(_contentAlignment);
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;

            string sReplacedText = _text.Replace("\\n", "\n");
            sReplacedText = sReplacedText.Replace("\\r", "\r");
            if (sReplacedText.Length > 0)
            {
                if (sReplacedText.LastIndexOf('\n') == sReplacedText.Length - 1)
                    sReplacedText = sReplacedText.Substring(0, sReplacedText.Length - 1);
                if (sReplacedText.LastIndexOf('\r') == sReplacedText.Length - 1)
                    sReplacedText = sReplacedText.Substring(0, sReplacedText.Length - 1);
            }

            SizeF sTextSize = g.MeasureString(sReplacedText, _font);


            RectangleF retText = new RectangleF(0, 0, 0, 0);
            switch (_contentAlignment)
            {
                case ContentAlignment.TopLeft: retText = new RectangleF(Rectangle.X, Rectangle.Y, sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.TopCenter: retText = new RectangleF(Rectangle.X + (Rectangle.Width / 2) - (sTextSize.Width / 2), Rectangle.Y, sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.TopRight: retText = new RectangleF(Rectangle.X + Rectangle.Width - sTextSize.Width, Rectangle.Y, sTextSize.Width, sTextSize.Height); break;

                case ContentAlignment.MiddleLeft: retText = new RectangleF(Rectangle.X, Rectangle.Y + (Rectangle.Height / 2) - (sTextSize.Height / 2), sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.MiddleCenter: retText = new RectangleF(Rectangle.X + (Rectangle.Width / 2) - (sTextSize.Width / 2), Rectangle.Y + (Rectangle.Height / 2) - (sTextSize.Height / 2), sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.MiddleRight: retText = new RectangleF(Rectangle.X + Rectangle.Width - sTextSize.Width, Rectangle.Y + (Rectangle.Height / 2) - (sTextSize.Height / 2), sTextSize.Width, sTextSize.Height); break;

                case ContentAlignment.BottomLeft: retText = new RectangleF(Rectangle.X, Rectangle.Y + Rectangle.Height - (sTextSize.Height), sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.BottomCenter: retText = new RectangleF(Rectangle.X + (Rectangle.Width / 2) - (sTextSize.Width / 2), Rectangle.Y + Rectangle.Height - (sTextSize.Height), sTextSize.Width, sTextSize.Height); break;
                case ContentAlignment.BottomRight: retText = new RectangleF(Rectangle.X + Rectangle.Width - sTextSize.Width, Rectangle.Y + Rectangle.Height - (sTextSize.Height), sTextSize.Width, sTextSize.Height); break;
            }
            //g.DrawRectangle(new Pen(Color.Red, 2), retText.Location.X, retText.Location.Y, retText.Width, retText.Height);
            //PointF pTextPos = GetTextPosition(g.MeasureString(_text, _font), Rectangle/*, _contentAlignment*/);

            Matrix gpTrans = new Matrix();

            gpTrans.RotateAt(_iAngle, new PointF(retText.X + retText.Width / 2, retText.Y + retText.Height / 2));



            if (_dCurvedTextRadius == 0)
            {
                GraphicsPath GP = new GraphicsPath();
                GP.AddString(sReplacedText, _font.FontFamily, (int)_font.Style, _font.SizeInPoints * 96F / 72F, retText, sf);
                GP.Transform(gpTrans);
                Region gClipped = g.Clip;
                g.Clip = new Region(Rectangle);
                g.FillPath(new SolidBrush(_color), GP);
                g.Clip = gClipped;
                GP.Dispose();
            }
            else
            {
                float fAngle = (float)((Math.PI * (360 - _dCurvedStartAngle + 90) / 180.0));
                PointF pTextPos = new PointF(LocationF.X + Rectangle.Width / 2, Location.Y + Rectangle.Height / 2);
                //RectangleF testRect = GetCurvedTextBound(_text, new PointF(pTextPos.X, (float)(pTextPos.Y + _dCurvedTextRadius + (sSizeText.Height / 2))), (float)_dCurvedTextRadius, fAngle);
                RectangleF testRect = GetCurvedTextBound(_text, pTextPos, (float)_dCurvedTextRadius, fAngle);

                //g.DrawRectangle(new Pen(Color.Green, 2), testRect.X, testRect.Y, testRect.Width, testRect.Height);

                //pTextPos = new PointF(LocationF.X, Location.Y);
                pTextPos = new PointF(LocationF.X + testRect.Width / 2 + (LocationF.X - testRect.X), Location.Y + testRect.Height / 2 + (LocationF.Y - testRect.Y));
                //pTextPos = new PointF(pTextPos.X, (float)(pTextPos.Y + _dCurvedTextRadius + fOffset));
                float fOffset = (_dCurvedTextRadius < 0) ? (float)sTextSize.Height / 1F : (float)-sTextSize.Height / 1F;
                Region gClipped = g.Clip;
                g.Clip = new Region(Rectangle);
                DrawCurvedText(g, _text, pTextPos, (float)_dCurvedTextRadius, fAngle);
                g.Clip = gClipped;

                //g.DrawRectangle(new Pen(Color.Red, 2), LocationF.X, LocationF.Y, 10, 10);
                //g.DrawRectangle(new Pen(Color.Blue, 2), pTextPos.X, pTextPos.Y, 10, 10);

            }

            //gpTrans.Dispose();
            sf.Dispose();
            colorBrush.Dispose();
            //CGXs
        }

        //CGX

        /// <summary>
        /// 
        /// </summary>
        private SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateSize()
        {
            if (_bAutoSize)
            {
                string Text = _text;

                if (_dCurvedTextRadius == 0)
                {
                    Size size = new System.Drawing.Size();
                    size = MeasureString(Text, Font).ToSize();

                    //Size = size;
                    float fOffsetX = Rectangle.X;
                    float fOffsetY = Rectangle.Y;
                    //Rectangle = new RectangleF(fOffsetX, fOffsetY, size.Width, size.Height);
                    Rectangle = new RectangleF(fOffsetX, fOffsetY, size.Width, size.Height);

                    Matrix gpTrans = new Matrix();
                    gpTrans.RotateAt(_iAngle, new PointF(Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + (Rectangle.Height / 2)));

                    GraphicsPath GP_Rect = new GraphicsPath();
                    GP_Rect.AddRectangle(Rectangle);
                    GP_Rect.Transform(gpTrans);
                    RectangleF newRect = GP_Rect.GetBounds();
                    GP_Rect.Dispose();
                    //newRect.Location = new PointF(Rectangle.X + fOffsetX, Rectangle.Y + fOffsetY);
                    Rectangle = newRect;
                    Size = new SizeF(newRect.Width, newRect.Height);
                }
                else
                {
                    PointF pLoc = LocationF;

                    SizeF sSizeText = TextRenderer.MeasureText(_text, _font);
                    PointF pTextPos = new PointF(LocationF.X + (sSizeText.Width / 2), Location.Y + (sSizeText.Height / 2));
                    //PointF pTextPos = GetTextPosition(sSizeText, Rectangle);

                    float fAngle = (float)((Math.PI * (360 - _dCurvedStartAngle + 90) / 180.0));
                    Rectangle = GetCurvedTextBound(_text, new PointF(pTextPos.X, (float)(pTextPos.Y + _dCurvedTextRadius + (sSizeText.Height / 2))), (float)_dCurvedTextRadius, fAngle);


                    //float fOffsetX = (float)(pTextPos.X - (size.Width / 2));
                    //float fOffsetY = (float)(pTextPos.Y - (size.Height / 2));
                    //Rectangle = new RectangleF(fOffsetX, fOffsetY, (float)size.Width, (float)size.Height);
                    Size = new SizeF(Rectangle.Width, Rectangle.Height);

                    LocationF = pLoc;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private StringFormat GetAlignement(ContentAlignment CA)
        {
            StringFormat sf = StringFormat.GenericDefault;

            try
            {
                switch (CA)
                {
                    case ContentAlignment.TopLeft: sf.Alignment = StringAlignment.Near; break;
                    case ContentAlignment.TopCenter: sf.Alignment = StringAlignment.Center; break;
                    case ContentAlignment.TopRight: sf.Alignment = StringAlignment.Far; break;
                    case ContentAlignment.MiddleLeft: sf.Alignment = StringAlignment.Near; break;
                    case ContentAlignment.MiddleCenter: sf.Alignment = StringAlignment.Center; break;
                    case ContentAlignment.MiddleRight: sf.Alignment = StringAlignment.Far; break;
                    case ContentAlignment.BottomLeft: sf.Alignment = StringAlignment.Near; break;
                    case ContentAlignment.BottomCenter: sf.Alignment = StringAlignment.Center; break;
                    case ContentAlignment.BottomRight: sf.Alignment = StringAlignment.Far; break;
                }
            }
            catch (Exception ex)
            { }

            return sf;
        }

        /// <summary>
        /// 
        /// </summary>
        private PointF GetTextPosition(SizeF sTextSize, RectangleF r/*, ContentAlignment CA*/)
        {
            PointF pTextPos = new PointF(r.X, r.Y);

            try
            {
                pTextPos = new PointF(r.X + (r.Width / 2), r.Y + (r.Height / 2));
            }
            catch (Exception ex)
            { }

            return pTextPos;
        }

        //Fin CGX

        #endregion

        // CGX
        #region Curved Text

        /// <summary>
        /// 
        /// </summary>
        private RectangleF GetCurvedTextBound(string text, PointF centre, float distanceFromCentreToBaseOfText, float radiansToTextCentre)
        {
            RectangleF retRect = new RectangleF();

            try
            {
                using (var image = new Bitmap(1, 1))
                {
                    using (var graphics = Graphics.FromImage(image))
                    {
                        List<RectangleF> lRect = new List<RectangleF>();

                        // Circumference for use later
                        float circleCircumference = (float)(Math.PI * 2 * distanceFromCentreToBaseOfText);

                        // Get the width of each character
                        List<float> characterWidths = GetCharacterWidths(graphics, text, _font);

                        // The overall height of the string
                        float characterHeight = graphics.MeasureString(text, _font).Height;

                        float textLength = 0.0F;
                        foreach (float f in characterWidths)
                            textLength += f;

                        // The string length above is the arc length we'll use for rendering the string. Work out the starting angle required to 
                        // centre the text across the radiansToTextCentre.
                        float fractionOfCircumference = textLength / circleCircumference;

                        float currentCharacterRadians = radiansToTextCentre - (float)(Math.PI * fractionOfCircumference);

                        for (int characterIndex = 0; characterIndex < text.Length; characterIndex++)
                        {
                            char @char = text[characterIndex];

                            // Polar to cartesian
                            float x = (float)(distanceFromCentreToBaseOfText * Math.Sin(currentCharacterRadians));
                            float y = -(float)(distanceFromCentreToBaseOfText * Math.Cos(currentCharacterRadians));

                            using (GraphicsPath characterPath = new GraphicsPath())
                            {
                                characterPath.AddString(@char.ToString(), _font.FontFamily, (int)_font.Style, _font.Size * graphics.DpiX / 72F, PointF.Empty, StringFormat.GenericTypographic);

                                var pathBounds = characterPath.GetBounds();

                                // Transformation matrix to move the character to the correct location. 
                                // Note that all actions on the Matrix class are prepended, so we apply them in reverse.
                                var transform = new Matrix();

                                // Translate to the final position
                                transform.Translate(centre.X + x, centre.Y + y);

                                // Rotate the character
                                var rotationAngleDegrees = currentCharacterRadians * 180F / (float)Math.PI;// -180F;
                                transform.Rotate(rotationAngleDegrees);

                                // Translate the character so the centre of its base is over the origin
                                transform.Translate(-pathBounds.Width / 2F, -characterHeight);

                                characterPath.Transform(transform);

                                RectangleF rBounds = characterPath.GetBounds();
                                if ((rBounds.Width != 0) && (rBounds.Height != 0))
                                    lRect.Add(rBounds);
                            }

                            if (characterIndex != text.Length - 1)
                            {
                                // Move "currentCharacterRadians" on to the next character
                                var distanceToNextChar = (characterWidths[characterIndex] + characterWidths[characterIndex + 1]) / 2F;
                                float charFractionOfCircumference = distanceToNextChar / circleCircumference;
                                currentCharacterRadians += charFractionOfCircumference * (float)(2F * Math.PI);
                            }
                        }

                        // fusion des bounds
                        foreach (RectangleF rect in lRect)
                        {
                            if ((retRect.Width == 0) || (retRect.Height == 0))
                                retRect = rect;
                            else
                                retRect = RectangleF.Union(retRect, rect);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

            return retRect;
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawCurvedText(Graphics graphics, string text, PointF centre, float distanceFromCentreToBaseOfText, float radiansToTextCentre)
        {
            try
            {
                // Circumference for use later
                float circleCircumference = (float)(Math.PI * 2 * distanceFromCentreToBaseOfText);

                // Get the width of each character
                List<float> characterWidths = GetCharacterWidths(graphics, text, _font);

                // The overall height of the string
                float characterHeight = graphics.MeasureString(text, _font).Height;

                float textLength = 0.0F;
                foreach (float f in characterWidths)
                    textLength += f;

                // The string length above is the arc length we'll use for rendering the string. Work out the starting angle required to 
                // centre the text across the radiansToTextCentre.
                float fractionOfCircumference = textLength / circleCircumference;

                float currentCharacterRadians = radiansToTextCentre - (float)(Math.PI * fractionOfCircumference);

                for (int characterIndex = 0; characterIndex < text.Length; characterIndex++)
                {
                    char @char = text[characterIndex];

                    // Polar to cartesian
                    float x = (float)(distanceFromCentreToBaseOfText * Math.Sin(currentCharacterRadians));
                    float y = -(float)(distanceFromCentreToBaseOfText * Math.Cos(currentCharacterRadians));

                    using (GraphicsPath characterPath = new GraphicsPath())
                    {
                        characterPath.AddString(@char.ToString(), _font.FontFamily, (int)_font.Style, _font.Size * 96F / 72F, PointF.Empty, StringFormat.GenericTypographic);

                        var pathBounds = characterPath.GetBounds();

                        // Transformation matrix to move the character to the correct location. 
                        // Note that all actions on the Matrix class are prepended, so we apply them in reverse.
                        var transform = new Matrix();

                        // Translate to the final position
                        transform.Translate(centre.X + x, centre.Y + y);

                        // Rotate the character
                        var rotationAngleDegrees = currentCharacterRadians * 180F / (float)Math.PI;// -180F;
                        transform.Rotate(rotationAngleDegrees);

                        // Translate the character so the centre of its base is over the origin
                        transform.Translate(-pathBounds.Width / 2F, -characterHeight);

                        characterPath.Transform(transform);

                        // Draw the character
                        graphics.FillPath(new SolidBrush(Color.Black), characterPath);
                    }

                    if (characterIndex != text.Length - 1)
                    {
                        // Move "currentCharacterRadians" on to the next character
                        var distanceToNextChar = (characterWidths[characterIndex] + characterWidths[characterIndex + 1]) / 2F;
                        float charFractionOfCircumference = distanceToNextChar / circleCircumference;
                        currentCharacterRadians += charFractionOfCircumference * (float)(2F * Math.PI);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<float> GetCharacterWidths(Graphics graphics, string text, Font font)
        {
            List<float> ret = new List<float>();

            try
            {
                //float fSpaceWidth = TextRenderer.MeasureText(" ", font).Width;
                float fSpaceWidth = graphics.MeasureString(" ", font, PointF.Empty, StringFormat.GenericDefault).Width;
                //ret = text.Select(c => c == ' ' ? spaceLength : graphics.MeasureString(c.ToString(), font, PointF.Empty, StringFormat.GenericTypographic).Width);
                foreach (char c in text.ToCharArray())
                {
                    if (c != ' ')
                        ret.Add(graphics.MeasureString(c.ToString(), font, PointF.Empty, StringFormat.GenericTypographic).Width);
                    else
                        ret.Add(fSpaceWidth);
                }
            }
            catch (Exception ex)
            { }

            return ret;
        }

        #endregion
    }
}