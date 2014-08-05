// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/17/2009 3:33:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Southerland-Hodgman clipper
    /// </summary>
    public class SoutherlandHodgman
    {
        const int BOUND_RIGHT = 0;
        const int BOUND_TOP = 1;
        const int BOUND_LEFT = 2;
        const int BOUND_BOTTOM = 3;

        /// <summary>
        /// Create SoutherlandHodgman polygon clipper with clipping rectangle
        /// </summary>
        /// <param name="clipRect"></param>
        public SoutherlandHodgman(Rectangle clipRect)
        {
            ClippingRectangle = clipRect;
        }

        /// <summary>
        /// Create southerlandHodgman polygon clipper with default clipping rectangle
        /// </summary>
        public SoutherlandHodgman()
        {
            ClippingRectangle = new Rectangle(-32000, -32000, 64000, 64000);
        }

        /// <summary>
        /// Get or set the clipping rectangle used in subsequent Clip calls.
        /// </summary>
        public Rectangle ClippingRectangle { get; set; }

        #region Public methods

        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This specific overload works with arrays of doubles instead of PointF structures.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="vertexValues">The list of arrays of doubles where the X index is 0 and the Y index is 1.</param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds</returns>
        public List<Vertex> Clip(List<Vertex> vertexValues)
        {
            var result = vertexValues;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
            }

            return result;
        }

        #endregion

        #region Private methods

        private bool IsInside(Vertex point, int direction)
        {
            switch (direction)
            {
                case BOUND_RIGHT:
                    if (point.X <= ClippingRectangle.Right) return true;
                    return false;
                case BOUND_LEFT:
                    if (point.X >= ClippingRectangle.Left) return true;
                    return false;
                case BOUND_TOP:
                    if (point.Y >= ClippingRectangle.Top) return true;
                    return false;
                case BOUND_BOTTOM:
                    if (point.Y <= ClippingRectangle.Bottom) return true;
                    return false;
            }
            return false;
        }

        private Vertex BoundIntersection(Vertex start, Vertex end, int direction)
        {
            double x, y;
            switch (direction)
            {
                case BOUND_RIGHT:
                    x = ClippingRectangle.Right;
                    y = start.Y + (end.Y - start.Y) * (ClippingRectangle.Right - start.X) / (end.X - start.X);
                    break;
                case BOUND_LEFT:
                    x = ClippingRectangle.Left;
                    y = start.Y + (end.Y - start.Y) * (ClippingRectangle.Left - start.X) / (end.X - start.X);
                    break;
                case BOUND_TOP:
                    y = ClippingRectangle.Top;
                    x = start.X + (end.X - start.X) * (ClippingRectangle.Top - start.Y) / (end.Y - start.Y);
                    break;
                case BOUND_BOTTOM:
                    y = ClippingRectangle.Bottom;
                    x = start.X + (end.X - start.X) * (ClippingRectangle.Bottom - start.Y) / (end.Y - start.Y);
                    break;
                default:
                    x = y = 0;
                    break;
            }
            return new Vertex(x, y);
        }

        private List<Vertex> ClipDirection(IEnumerable<Vertex> points, int direction)
        {
            bool previousInside = true;
            var result = new List<Vertex>();
            var previous = new Vertex();
            bool isFirst = true;
            foreach (var point in points)
            {
                bool inside = IsInside(point, direction);
                if (previousInside && inside)
                {
                    // both points are inside, so simply add the current point
                    result.Add(point);
                    previous = point;
                }
                if (previousInside && inside == false)
                {
                    if (isFirst == false)
                    {
                        // crossing the boundary going out, so insert the intersection instead
                        result.Add(BoundIntersection(previous, point, direction));
                    }
                    previous = point;
                }
                if (previousInside == false && inside)
                {
                    // crossing the boundary going in, so insert the intersection AND the new point
                    result.Add(BoundIntersection(previous, point, direction));
                    result.Add(point);
                    previous = point;
                }
                if (previousInside == false && inside == false)
                {
                    previous = point;
                }
                isFirst = false;
                previousInside = inside;
            }
            // be sure to close the polygon if it is not closed
            if (result.Count > 0)
            {
                if (result[0].X != result[result.Count - 1].X ||
                    result[0].Y != result[result.Count - 1].Y)
                {
                    result.Add(result[0]);
                }
            }
            return result;
        }

        #endregion
    }
}