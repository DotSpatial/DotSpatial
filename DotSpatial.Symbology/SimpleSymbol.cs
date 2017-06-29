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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 3:01:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// SimpleSymbol
    /// </summary>
    public class SimpleSymbol : OutlinedSymbol, ISimpleSymbol
    {
        #region Private Variables

        private Color _color;
        private PointShape _pointShape;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SimpleSymbol
        /// </summary>
        public SimpleSymbol()
        {
            Configure();
        }

        /// <summary>
        /// Creates a point symbol with the specified color.
        /// </summary>
        /// <param name="color">The color of the symbol.</param>
        public SimpleSymbol(Color color)
        {
            Configure();
            _color = color;
        }

        /// <summary>
        /// Creates a point symbol with the specified color and shape.
        /// </summary>
        /// <param name="color">The color of the symbol.</param>
        /// <param name="shape">The shape of the symbol.</param>
        public SimpleSymbol(Color color, PointShape shape)
        {
            Configure();
            _color = color;
            _pointShape = shape;
        }

        /// <summary>
        /// Creates a SimpleSymbol with the specified color, shape and size.  The size is used for
        /// both the horizontal and vertical directions.
        /// </summary>
        /// <param name="color">The color of the symbol.</param>
        /// <param name="shape">The shape of the symbol.</param>
        /// <param name="size">The size of the symbol.</param>
        public SimpleSymbol(Color color, PointShape shape, double size)
        {
            Configure();
            _color = color;
            _pointShape = shape;
            Size = new Size2D(size, size);
        }

        private void Configure()
        {
            base.SymbolType = SymbolType.Simple;
            _color = SymbologyGlobal.RandomColor();
            _pointShape = PointShape.Rectangle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the font color of this symbol to represent the color of this symbol
        /// </summary>
        /// <returns>The color of this symbol as a font</returns>
        public override Color GetColor()
        {
            return _color;
        }

        /// <summary>
        /// Sets the fill color of this symbol to the specified color.
        /// </summary>
        /// <param name="color">The Color.</param>
        public override void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Handles the specific drawing for this symbol.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics surface to draw with.</param>
        /// <param name="scaleSize">A double controling the scaling of the symbol.</param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            if (scaleSize == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;
            SizeF size = new SizeF((float)(scaleSize * Size.Width), (float)(scaleSize * Size.Height));
            Brush fillBrush = new SolidBrush(Color);
            GraphicsPath gp = new GraphicsPath();
            switch (PointShape)
            {
                case PointShape.Diamond:
                    AddRegularPoly(gp, size, 4);
                    break;
                case PointShape.Ellipse:
                    AddEllipse(gp, size);
                    break;
                case PointShape.Hexagon:
                    AddRegularPoly(gp, size, 6);
                    break;
                case PointShape.Pentagon:
                    AddRegularPoly(gp, size, 5);
                    break;
                case PointShape.Rectangle:
                    gp.AddRectangle(new RectangleF(-size.Width / 2, -size.Height / 2, size.Width, size.Height));
                    break;
                case PointShape.Star:
                    AddStar(gp, size);
                    break;
                case PointShape.Triangle:
                    AddRegularPoly(gp, size, 3);
                    break;
            }
            g.FillPath(fillBrush, gp);
            OnDrawOutline(g, scaleSize, gp);
            gp.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Only provided because XML Serialization doesn't work for colors
        /// </summary>
        [XmlElement("Color"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Serialize("XmlColor"), ShallowCopy]
        public string XmlColor
        {
            get { return ColorTranslator.ToHtml(_color); }
            set { _color = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        [XmlIgnore]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the opacity as a floating point value ranging from 0 to 1, where
        /// 0 is fully transparent and 1 is fully opaque.  This actually adjusts the alpha of the color value.
        /// </summary>
        [Serialize("Opacity"), ShallowCopy]
        public float Opacity
        {
            get { return _color.A / 255F; }
            set
            {
                int alpha = (int)(value * 255);
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;
                if (alpha != _color.A)
                {
                    _color = Color.FromArgb(alpha, _color);
                }
            }
        }

        /// <summary>
        /// Gets or sets the PointTypes enumeration that describes how to draw the simple symbol.
        /// </summary>
        [Serialize("PointShapes")]
        public PointShape PointShape
        {
            get { return _pointShape; }
            set { _pointShape = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs during the randomizing process
        /// </summary>
        /// <param name="generator"></param>
        protected override void OnRandomize(Random generator)
        {
            _color = generator.NextColor();
            Opacity = generator.NextFloat();
            _pointShape = generator.NextEnum<PointShape>();

            base.OnRandomize(generator);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Draws a 5 pointed star with the points having twice the radius as the bends.
        /// </summary>
        /// <param name="gp">The GraphicsPath to add the start to</param>
        /// <param name="scaledSize">The SizeF size to fit the Start to</param>
        public static void AddStar(GraphicsPath gp, SizeF scaledSize)
        {
            PointF[] polyPoints = new PointF[11];
            GetStars(scaledSize, polyPoints);
            gp.AddPolygon(polyPoints);
        }

        private static void GetStars(SizeF scaledSize, PointF[] polyPoints)
        {
            for (int i = 0; i <= 10; i++)
            {
                double ang = i * Math.PI / 5;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                if (i % 2 == 0)
                {
                    x /= 2; // the shorter radius points of the star
                    y /= 2;
                }
                polyPoints[i] = new PointF(x, y);
            }
        }

        /// <summary>
        /// Draws a 5 pointed star with the points having twice the radius as the bends.
        /// </summary>
        /// <param name="g">The Graphics surface to draw on</param>
        /// <param name="scaledBorderPen">The Pen to draw the border with</param>
        /// <param name="fillBrush">The Brush to use to fill the Star</param>
        /// <param name="scaledSize">The SizeF size to fit the Start to</param>
        public static void DrawStar(Graphics g, Pen scaledBorderPen, Brush fillBrush, SizeF scaledSize)
        {
            PointF[] polyPoints = new PointF[11];
            GetStars(scaledSize, polyPoints);
            if (fillBrush != null)
            {
                g.FillPolygon(fillBrush, polyPoints, FillMode.Alternate);
            }
            if (scaledBorderPen != null)
            {
                g.DrawPolygon(scaledBorderPen, polyPoints);
            }
        }

        /// <summary>
        /// Draws an ellipse on the specified graphics surface.
        /// </summary>
        /// <param name="gp">The GraphicsPath to add this shape to</param>
        /// <param name="scaledSize">The size to fit the ellipse into (the ellipse will be centered at 0, 0)</param>
        public static void AddEllipse(GraphicsPath gp, SizeF scaledSize)
        {
            PointF upperLeft = new PointF(-scaledSize.Width / 2, -scaledSize.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, scaledSize);
            gp.AddEllipse(destRect);
        }

        /// <summary>
        /// Draws an ellipse on the specified graphics surface.
        /// </summary>
        /// <param name="g">The graphics surface to draw on</param>
        /// <param name="scaledBorderPen">The Pen to use for the border, or null if no border should be drawn</param>
        /// <param name="fillBrush">The Brush to use for the fill, or null if no fill should be drawn</param>
        /// <param name="scaledSize">The size to fit the ellipse into (the ellipse will be centered at 0, 0)</param>
        public static void DrawEllipse(Graphics g, Pen scaledBorderPen, Brush fillBrush, SizeF scaledSize)
        {
            PointF upperLeft = new PointF(-scaledSize.Width / 2, -scaledSize.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, scaledSize);
            if (fillBrush != null)
            {
                g.FillEllipse(fillBrush, destRect);
            }
            if (scaledBorderPen != null)
            {
                g.DrawEllipse(scaledBorderPen, destRect);
            }
        }

        /// <summary>
        /// Draws a regular polygon with equal sides.  The first point will be located all the way to the right on the X axis.
        /// </summary>
        /// <param name="gp">Specifies the GraphicsPath surface to draw on</param>
        /// <param name="scaledSize">Specifies the SizeF to fit the polygon into</param>
        /// <param name="numSides">Specifies the integer number of sides that the polygon should have</param>
        public static void AddRegularPoly(GraphicsPath gp, SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = new PointF[numSides + 1];

            // Instead of figuring out the points in cartesian, figure them out in angles and re-convert them.
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                polyPoints[i] = new PointF(x, y);
            }
            gp.AddPolygon(polyPoints);
        }

        /// <summary>
        /// Draws a regular polygon with equal sides.  The first point will be located all the way to the right on the X axis.
        /// </summary>
        /// <param name="g">Specifies the Graphics surface to draw on</param>
        /// <param name="scaledBorderPen">Specifies the Pen to use for the border</param>
        /// <param name="fillBrush">Specifies the Brush to use for to fill the shape</param>
        /// <param name="scaledSize">Specifies the SizeF to fit the polygon into</param>
        /// <param name="numSides">Specifies the integer number of sides that the polygon should have</param>
        public static void DrawRegularPoly(Graphics g, Pen scaledBorderPen, Brush fillBrush, SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = new PointF[numSides + 1];

            // Instead of figuring out the points in cartesian, figure them out in angles and re-convert them.
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                polyPoints[i] = new PointF(x, y);
            }
            if (fillBrush != null)
            {
                g.FillPolygon(fillBrush, polyPoints, FillMode.Alternate);
            }
            if (scaledBorderPen != null)
            {
                g.DrawPolygon(scaledBorderPen, polyPoints);
            }
        }

        #endregion
    }
}