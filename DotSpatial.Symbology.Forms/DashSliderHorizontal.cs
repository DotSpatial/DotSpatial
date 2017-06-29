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
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DashSliderHorizontal
        /// </summary>
        public DashSliderHorizontal()
            : base(Orientation.Horizontal)
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
                    result.X = Position.X - Image.Width / 2;
                    result.Y = 0;
                    result.Width = Image.Width;
                    result.Height = Image.Height;
                }
                else
                {
                    result.X = Position.X - Size.Width / 2;
                    result.Y = 0;
                    result.Width = Size.Width;
                    result.Height = Size.Height * 3 / 2;
                }
                return result;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Draws the dash slider
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        public override void Draw(Graphics g, Rectangle clipRectangle)
        {
            DrawHorizontal(g, clipRectangle);
        }

        #endregion

        #region Private Functions

        private void DrawHorizontal(Graphics g, Rectangle clipRectangle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Image != null)
            {
                g.DrawImage(Image, new PointF(Position.X - Image.Width / 2, 0));
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