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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 12:37:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// GradientPattern
    /// </summary>
    public class GradientPattern : Pattern, IGradientPattern
    {
        #region Private Variables

        private double _angle;
        private Color[] _colors;
        private GradientType _gradientType;
        private float[] _positions;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GradientPattern
        /// </summary>
        public GradientPattern()
        {
            _gradientType = GradientType.Linear;
            _colors = new Color[2];
            _colors[0] = SymbologyGlobal.RandomLightColor(1F);
            _colors[1] = _colors[0].Darker(.3F);
            _positions = new[] { 0F, 1F };
            _angle = -45;
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        public GradientPattern(Color startColor, Color endColor)
        {
            _gradientType = GradientType.Linear;
            _colors = new Color[2];
            _colors[0] = startColor;
            _colors[1] = endColor;
            _positions = new[] { 0F, 1F };
            _angle = -45;
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors and angle
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        public GradientPattern(Color startColor, Color endColor, double angle)
        {
            _gradientType = GradientType.Linear;
            _colors = new Color[2];
            _colors[0] = startColor;
            _colors[1] = endColor;
            _positions = new[] { 0F, 1F };
            _angle = angle;
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors and angle
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">Controls how the gradient is drawn</param>
        public GradientPattern(Color startColor, Color endColor, double angle, GradientType style)
        {
            _gradientType = style;
            _colors = new Color[2];
            _colors[0] = startColor;
            _colors[1] = endColor;
            _positions = new[] { 0F, 1F };
            _angle = angle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a color that can be used to represent this pattern.  In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        public override Color GetFillColor()
        {
            Color l = _colors[0];
            Color h = _colors[_colors.Length - 1];
            int a = (l.A + h.A) / 2;
            int r = (l.R + h.R) / 2;
            int g = (l.G + h.G) / 2;
            int b = (l.B + h.B) / 2;
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Sets the fill color, keeping the approximate gradiant RGB changes the same, but adjusting
        /// the mean color to the specifeid color.
        /// </summary>
        /// <param name="color">The mean color to apply.</param>
        public override void SetFillColor(Color color)
        {
            if (_colors == null || _colors.Length == 0) return;
            if (_colors.Length == 1)
            {
                _colors[0] = color;
                return;
            }
            Color l = _colors[0];
            Color h = _colors[_colors.Length - 1];
            int da = (h.A - l.A) / 2;
            int dr = (h.R - l.R) / 2;
            int dg = (h.G - l.G) / 2;
            int db = (h.B - l.B) / 2;
            _colors[0] = Color.FromArgb(ByteSize(color.A - da), ByteSize(color.R - dr), ByteSize(color.G - dg), ByteSize(color.B - db));
            _colors[_colors.Length - 1] = Color.FromArgb(ByteSize(color.A + da), ByteSize(color.R + dr), ByteSize(color.G + dg), ByteSize(color.B + db));
        }

        private static int ByteSize(int value)
        {
            if (value > 255) return 255;
            return value < 0 ? 0 : value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the gradient pattern.
        /// </summary>
        [Serialize("Angle")]
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        /// <summary>
        /// Gets or sets the end color
        /// </summary>
        [Serialize("Colors")]
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        /// <summary>
        /// Gets or sets the start color
        /// </summary>
        [Serialize("Positions")]
        public float[] Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        /// <summary>
        /// Gets or sets the gradient type
        /// </summary>
        [Serialize("GradientTypes")]
        public GradientType GradientType
        {
            get { return _gradientType; }
            set { _gradientType = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the drawing code for linear gradient paths.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="gp"></param>
        public override void FillPath(Graphics g, GraphicsPath gp)
        {
            RectangleF bounds = Bounds;
            if (bounds.IsEmpty) bounds = gp.GetBounds();
            if (bounds.Width == 0 || bounds.Height == 0) return;
            //also don't draw gradient for very small polygons
            if (bounds.Width < 0.01 || bounds.Height < 0.01) return;
            if (_gradientType == GradientType.Linear)
            {
                LinearGradientBrush b = new LinearGradientBrush(bounds, _colors[0], _colors[_colors.Length - 1], (float)-_angle);
                ColorBlend cb = new ColorBlend();
                cb.Positions = _positions;
                cb.Colors = _colors;
                b.InterpolationColors = cb;
                g.FillPath(b, gp);
                b.Dispose();
            }
            else if (_gradientType == GradientType.Circular)
            {
                GraphicsPath round = new GraphicsPath();
                PointF center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
                float x = (float)(center.X - Math.Sqrt(2) * bounds.Width / 2);
                float y = (float)(center.Y - Math.Sqrt(2) * bounds.Height / 2);
                float w = (float)(bounds.Width * Math.Sqrt(2));
                float h = (float)(bounds.Height * Math.Sqrt(2));
                RectangleF circum = new RectangleF(x, y, w, h);
                round.AddEllipse(circum);
                PathGradientBrush pgb = new PathGradientBrush(round);
                ColorBlend cb = new ColorBlend();
                cb.Colors = _colors;
                cb.Positions = _positions;
                pgb.InterpolationColors = cb;
                g.FillPath(pgb, gp);
                pgb.Dispose();
            }
            else if (_gradientType == GradientType.Rectangular)
            {
                GraphicsPath rect = new GraphicsPath();
                PointF[] points = new PointF[5];
                PointF center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
                double a = bounds.Width / 2;
                double b = bounds.Height / 2;
                double angle = _angle;
                if (angle < 0) angle = 360 + angle;
                angle = angle % 90;
                angle = 2 * (Math.PI * angle / 180);
                double x = a * Math.Cos(angle);
                double y = -b - a * Math.Sin(angle);
                points[0] = new PointF((float)x + center.X, (float)y + center.Y);
                x = a + b * Math.Sin(angle);
                y = b * Math.Cos(angle);
                points[1] = new PointF((float)x + center.X, (float)y + center.Y);
                x = -a * Math.Cos(angle);
                y = b + a * Math.Sin(angle);
                points[2] = new PointF((float)x + center.X, (float)y + center.Y);
                x = -a - b * Math.Sin(angle);
                y = -b * Math.Cos(angle);
                points[3] = new PointF((float)x + center.X, (float)y + center.Y);
                points[4] = points[0];
                rect.AddPolygon(points);
                PathGradientBrush pgb = new PathGradientBrush(rect);
                ColorBlend cb = new ColorBlend();
                cb.Colors = _colors;
                cb.Positions = _positions;
                pgb.InterpolationColors = cb;
                g.FillPath(pgb, gp);
                pgb.Dispose();
            }
        }

        #endregion
    }
}