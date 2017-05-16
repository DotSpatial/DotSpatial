// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DashSliderVertical
    /// </summary>
    public class DashSliderVertical : DashSlider
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashSliderVertical"/> class.
        /// </summary>
        public DashSliderVertical()
            : base(Orientation.Vertical)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounding rectangle for this slider.
        /// </summary>
        public override RectangleF Bounds
        {
            get
            {
                if (Image != null)
                {
                    return new RectangleF(0, Position.Y - (Image.Height / 2), Image.Width, Image.Height);
                }

                return new RectangleF(0, Position.Y - (Size.Height / 2), Size.Width, (Size.Height * 3) / 2);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Teh Publick method allowing this dash slider to be moved
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="clipRectangle">The clip rectangle defining where drawing should take place</param>
        public override void Draw(Graphics g, Rectangle clipRectangle)
        {
            DrawVertical(g, clipRectangle);
        }

        private void DrawVertical(Graphics g, Rectangle clipRectangle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Image != null)
            {
                g.DrawImage(Image, 0, Position.Y - (Image.Height / 2));
            }
            else
            {
                float y = Position.Y;
                float dy = Size.Height / 2;
                float dx = Size.Width;
                PointF[] trianglePoints = new PointF[4];
                trianglePoints[0] = new PointF(dx, y);
                trianglePoints[1] = new PointF(0, y - dy);
                trianglePoints[2] = new PointF(0, y + dy);
                trianglePoints[3] = new PointF(dx, y);
                LinearGradientBrush br = CreateGradientBrush(Color, new PointF(0, y - dy), new PointF(dx, y + dy));
                g.FillPolygon(br, trianglePoints);
                br.Dispose();
                g.DrawPolygon(Pens.Black, trianglePoints);
            }
        }

        #endregion
    }
}