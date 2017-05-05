// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/1/2009 1:01:02 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DashSliderHorizontal
    /// </summary>
    public class DashSliderHorizontal : DashSlider
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashSliderHorizontal"/> class.
        /// </summary>
        public DashSliderHorizontal()
            : base(Orientation.Horizontal)
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
                    return new RectangleF(Position.X - (Image.Width / 2), 0, Image.Width, Image.Height);
                }

                return new RectangleF(Position.X - (Size.Width / 2), 0, Size.Width, Size.Height * 3 / 2);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the dash slider
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        public override void Draw(Graphics g, Rectangle clipRectangle)
        {
            DrawHorizontal(g, clipRectangle);
        }

        private void DrawHorizontal(Graphics g, Rectangle clipRectangle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Image != null)
            {
                g.DrawImage(Image, new PointF(Position.X - (Image.Width / 2), 0));
            }
            else
            {
                float x = Position.X;
                float dx = Size.Width / 2;
                float dy = Size.Height;

                PointF[] trianglePoints = new PointF[4];
                trianglePoints[0] = new PointF(x, dy);
                trianglePoints[1] = new PointF(x - dx, 0);
                trianglePoints[2] = new PointF(x + dx, 0);
                trianglePoints[3] = new PointF(x, dy);
                LinearGradientBrush br = CreateGradientBrush(Color, new PointF(x - dx, 0), new PointF(x + dx, dy));
                g.FillPolygon(br, trianglePoints);
                br.Dispose();
                g.DrawPolygon(Pens.Black, trianglePoints);
            }
        }

        #endregion
    }
}