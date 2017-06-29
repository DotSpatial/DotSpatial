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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/7/2009 5:58:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// GraphicsPathEM
    /// </summary>
    public static class GraphicsPathExt
    {
        #region Methods

        /// <summary>
        /// Adds a round rectangle to the graphics path where the integer radius specified determines how rounded the rectangle should become.
        /// This can be thought of rounded arcs connected by straight lines.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        public static void AddRoundedRectangle(this GraphicsPath self, Rectangle bounds, int radius)
        {
            if (radius * 2 > bounds.Width)
            {
                self.AddEllipse(bounds);
                return;
            }
            if (radius <= 0)
            {
                self.AddRectangle(bounds);
                return;
            }
            int w = radius * 2;
            Rectangle br = new Rectangle(bounds.Right - w, bounds.Bottom - w, w, w);
            Rectangle bl = new Rectangle(bounds.Left, bounds.Bottom - w, w, w);
            Rectangle tl = new Rectangle(bounds.Left, bounds.Top, w, w);
            Rectangle tr = new Rectangle(bounds.Right - w, bounds.Top, w, w);
            self.AddArc(br, 0, 90F);
            self.AddLine(new Point(bounds.Right - radius, bounds.Bottom), new Point(bounds.Left + radius, bounds.Bottom));
            self.AddArc(bl, 90F, 90F);
            self.AddLine(new Point(bounds.Left, bounds.Bottom - radius), new Point(bounds.Left, bounds.Top + radius));
            self.AddArc(tl, 180F, 90F);
            self.AddLine(new Point(bounds.Left + radius, bounds.Top), new Point(bounds.Right - radius, bounds.Top));
            self.AddArc(tr, 270F, 90F);
            self.AddLine(new Point(bounds.Right, bounds.Top + radius), new Point(bounds.Right, bounds.Bottom - radius));
            self.CloseFigure();
        }

        /// <summary>
        /// Adds the unclosed set of lines that are the top and left of the shape
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        public static void AddRoundedRectangleTopLeft(this GraphicsPath self, Rectangle bounds, int radius)
        {
            if (radius * 2 > bounds.Width)
            {
                self.AddArc(bounds, 180F, 90F);
                return;
            }
            int w = radius * 2;
            Rectangle tl = new Rectangle(bounds.Left, bounds.Top, w, w);
            self.AddLine(new Point(bounds.Left, bounds.Bottom - radius), new Point(bounds.Left, bounds.Top + radius));
            self.AddArc(tl, 180F, 90F);
            self.AddLine(new Point(bounds.Left + radius, bounds.Top), new Point(bounds.Right - radius, bounds.Top));
        }

        /// <summary>
        /// Adds the unclosed set of lines that are the bottom and right of the shape
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        public static void AddRoundedRectangleBottomRight(this GraphicsPath self, Rectangle bounds, int radius)
        {
            if (radius * 2 > bounds.Width)
            {
                self.AddArc(bounds, 0F, 90F);
            }
            int w = radius * 2;
            Rectangle br = new Rectangle(bounds.Right - w, bounds.Bottom - w, w, w);
            self.AddLine(new Point(bounds.Right, bounds.Top + radius), new Point(bounds.Right, bounds.Bottom - radius));
            self.AddArc(br, 0, 90F);
            self.AddLine(new Point(bounds.Right - radius, bounds.Bottom), new Point(bounds.Left + radius, bounds.Bottom));
        }

        /// <summary>
        /// Tests each of the points in the graphics path, and calculates a RectangleF that completely contains
        /// all of the points in the graphics path.
        /// </summary>
        /// <param name="self">The grpahics path to test.</param>
        /// <returns>A RectangleF</returns>
        public static RectangleF GetBounds(this GraphicsPath self)
        {
            PointF[] points = self.PathPoints;
            if (points.Length == 0) return RectangleF.Empty;
            float xMin = float.PositiveInfinity;
            float xMax = float.NegativeInfinity;
            float yMin = float.PositiveInfinity;
            float yMax = float.NegativeInfinity;
            foreach (PointF point in points)
            {
                if (xMin > point.X) xMin = point.X;
                if (xMax < point.X) xMax = point.X;
                if (yMin > point.Y) yMin = point.Y;
                if (yMax < point.Y) yMax = point.Y;
            }
            float dx = xMax - xMin;
            float dy = yMax - yMin;
            return new RectangleF(xMin, xMax, dx, dy);
        }

        #endregion
    }
}