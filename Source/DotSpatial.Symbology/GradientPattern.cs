// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
    /// Pattern that uses a gradient.
    /// </summary>
    public class GradientPattern : Pattern, IGradientPattern
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientPattern"/> class.
        /// </summary>
        public GradientPattern()
        {
            GradientType = GradientType.Linear;
            Colors = new Color[2];
            Colors[0] = SymbologyGlobal.RandomLightColor(1F);
            Colors[1] = Colors[0].Darker(.3F);
            Positions = new[] { 0F, 1F };
            Angle = -45;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientPattern"/> class using the specified colors.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        public GradientPattern(Color startColor, Color endColor)
            : this(startColor, endColor, -45)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientPattern"/> class using the specified colors and angle.
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        public GradientPattern(Color startColor, Color endColor, double angle)
            : this(startColor, endColor, angle, GradientType.Linear)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientPattern"/> class using the specified colors, angle and style.
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">Controls how the gradient is drawn</param>
        public GradientPattern(Color startColor, Color endColor, double angle, GradientType style)
        {
            Colors = new Color[2];
            Colors[0] = startColor;
            Colors[1] = endColor;
            Positions = new[] { 0F, 1F };
            Angle = angle;
            GradientType = style;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle for the gradient pattern.
        /// </summary>
        [Serialize("Angle")]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the colors (0 = start color, 1 = end color).
        /// </summary>
        [Serialize("Colors")]
        public Color[] Colors { get; set; }

        /// <summary>
        /// Gets or sets the gradient type.
        /// </summary>
        [Serialize("GradientTypes")]
        public GradientType GradientType { get; set; }

        /// <summary>
        /// Gets or sets the start positions (0 = start color, 1 = end color).
        /// </summary>
        [Serialize("Positions")]
        public float[] Positions { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the drawing code for linear gradient paths.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="gp">Path that gets drawn.</param>
        public override void FillPath(Graphics g, GraphicsPath gp)
        {
            RectangleF bounds = Bounds;
            if (bounds.IsEmpty) bounds = gp.GetBounds();
            if (bounds.Width == 0 || bounds.Height == 0) return;

            // also don't draw gradient for very small polygons
            if (bounds.Width < 0.01 || bounds.Height < 0.01) return;

            if (GradientType == GradientType.Linear)
            {
                using (LinearGradientBrush b = new LinearGradientBrush(bounds, Colors[0], Colors[Colors.Length - 1], (float)-Angle))
                {
                    ColorBlend cb = new ColorBlend
                    {
                        Positions = Positions,
                        Colors = Colors
                    };
                    b.InterpolationColors = cb;
                    g.FillPath(b, gp);
                }
            }
            else if (GradientType == GradientType.Circular)
            {
                PointF center = new PointF(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));
                float x = (float)(center.X - (Math.Sqrt(2) * bounds.Width / 2));
                float y = (float)(center.Y - (Math.Sqrt(2) * bounds.Height / 2));
                float w = (float)(bounds.Width * Math.Sqrt(2));
                float h = (float)(bounds.Height * Math.Sqrt(2));
                RectangleF circum = new RectangleF(x, y, w, h);

                GraphicsPath round = new GraphicsPath();
                round.AddEllipse(circum);
                using (PathGradientBrush pgb = new PathGradientBrush(round))
                {
                    ColorBlend cb = new ColorBlend
                    {
                        Colors = Colors,
                        Positions = Positions
                    };
                    pgb.InterpolationColors = cb;
                    g.FillPath(pgb, gp);
                }
            }
            else if (GradientType == GradientType.Rectangular)
            {
                double a = bounds.Width / 2;
                double b = bounds.Height / 2;
                double angle = Angle;
                if (angle < 0) angle = 360 + angle;
                angle = angle % 90;
                angle = 2 * (Math.PI * angle / 180);
                double x = a * Math.Cos(angle);
                double y = -b - (a * Math.Sin(angle));

                PointF center = new PointF(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));
                PointF[] points = new PointF[5];
                points[0] = new PointF((float)x + center.X, (float)y + center.Y);
                x = a + (b * Math.Sin(angle));
                y = b * Math.Cos(angle);
                points[1] = new PointF((float)x + center.X, (float)y + center.Y);
                x = -a * Math.Cos(angle);
                y = b + (a * Math.Sin(angle));
                points[2] = new PointF((float)x + center.X, (float)y + center.Y);
                x = -a - (b * Math.Sin(angle));
                y = -b * Math.Cos(angle);
                points[3] = new PointF((float)x + center.X, (float)y + center.Y);
                points[4] = points[0];

                GraphicsPath rect = new GraphicsPath();
                rect.AddPolygon(points);

                using (PathGradientBrush pgb = new PathGradientBrush(rect))
                {
                    ColorBlend cb = new ColorBlend
                    {
                        Colors = Colors,
                        Positions = Positions
                    };
                    pgb.InterpolationColors = cb;
                    g.FillPath(pgb, gp);
                }
            }
        }

        /// <summary>
        /// Gets a color that can be used to represent this pattern. In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        public override Color GetFillColor()
        {
            Color l = Colors[0];
            Color h = Colors[Colors.Length - 1];
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
            if (Colors == null || Colors.Length == 0) return;

            if (Colors.Length == 1)
            {
                Colors[0] = color;
                return;
            }

            Color l = Colors[0];
            Color h = Colors[Colors.Length - 1];
            int da = (h.A - l.A) / 2;
            int dr = (h.R - l.R) / 2;
            int dg = (h.G - l.G) / 2;
            int db = (h.B - l.B) / 2;
            Colors[0] = Color.FromArgb(ByteSize(color.A - da), ByteSize(color.R - dr), ByteSize(color.G - dg), ByteSize(color.B - db));
            Colors[Colors.Length - 1] = Color.FromArgb(ByteSize(color.A + da), ByteSize(color.R + dr), ByteSize(color.G + dg), ByteSize(color.B + db));
        }

        private static int ByteSize(int value)
        {
            if (value > 255) return 255;

            return value < 0 ? 0 : value;
        }

        #endregion
    }
}