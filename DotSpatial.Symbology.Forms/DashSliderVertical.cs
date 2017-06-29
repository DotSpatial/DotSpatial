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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/1/2009 2:18:39 PM
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
    /// DashSliderVertical
    /// </summary>
    public class DashSliderVertical : DashSlider
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DashSliderVertical
        /// </summary>
        public DashSliderVertical()
            : base(Orientation.Vertical)
        {
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounding rectangle for this slider.
        /// </summary>
        public override RectangleF Bounds
        {
            get
            {
                RectangleF result = new RectangleF();
                if (Image != null)
                {
                    result.X = 0;
                    result.Y = Position.Y - Image.Height / 2;
                    result.Width = Image.Width;
                    result.Height = Image.Height;
                }
                else
                {
                    result.X = 0;
                    result.Y = Position.Y - Size.Height / 2;
                    result.Width = Size.Width;
                    result.Height = (Size.Height * 3) / 2;
                }
                return result;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Teh Publick method allowing this dash slider to be moved
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="clipRectangle">The clip rectangle defining where drawing should take place</param>
        public override void Draw(Graphics g, Rectangle clipRectangle)
        {
            DrawVertical(g, clipRectangle);
        }

        #endregion

        #region Private Functions

        private void DrawVertical(Graphics g, Rectangle clipRectangle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Image != null)
            {
                g.DrawImage(Image, 0, Position.Y - Image.Height / 2);
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