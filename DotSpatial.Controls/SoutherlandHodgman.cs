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

namespace DotSpatial.Controls
{
    /// <summary>
    /// DoutherlandHodgmanClipper
    /// </summary>
    public class SoutherlandHodgman
    {
        const int BOUND_RIGHT = 0;
        const int BOUND_TOP = 1;
        const int BOUND_LEFT = 2;
        const int BOUND_BOTTOM = 3;
        const int X = 0;
        const int Y = 1;
        Rectangle _drawingBounds = new Rectangle(-32000, -32000, 64000, 64000);

        /// <summary>
        /// Create SoutherlandHodgman polygon clipper with clipping rectangle
        /// </summary>
        /// <param name="clipRect"></param>
        public SoutherlandHodgman(Rectangle clipRect)
        {
            _drawingBounds = clipRect;
        }

        /// <summary>
        /// Create southerlandHodgman polygon clipper with default clipping rectangle
        /// </summary>
        public SoutherlandHodgman()
        { }

        /// <summary>
        /// Get or set the clipping rectangle used in subsequent Clip calls.
        /// </summary>
        public Rectangle ClippingRectangle
        {
            get { return _drawingBounds; }
            set { _drawingBounds = value; }
        }

        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="points"></param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds</returns>
        public List<PointF> Clip(List<PointF> points)
        {
            List<PointF> result = points;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
            }

            return result;
        }

        private List<PointF> ClipDirection(IEnumerable<PointF> points, int direction)
        {
            bool previousInside = true;
            List<PointF> result = new List<PointF>();
            PointF previous = PointF.Empty;
            foreach (PointF point in points)
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
                    if (previous.IsEmpty == false)
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

                previousInside = inside;
            }
            // be sure to close the polygon if it is not closed
            if (result.Count > 0)
            {
                if (result[0].X != result[result.Count - 1].X || result[0].Y != result[result.Count - 1].Y)
                {
                    result.Add(new PointF(result[0].X, result[0].Y));
                }
            }
            return result;
        }

        private bool IsInside(PointF point, int direction)
        {
            switch (direction)
            {
                case BOUND_RIGHT:
                    if (point.X <= _drawingBounds.Right) return true;
                    return false;
                case BOUND_LEFT:
                    if (point.X >= _drawingBounds.Left) return true;
                    return false;
                case BOUND_TOP:
                    if (point.Y >= _drawingBounds.Top) return true;
                    return false;
                case BOUND_BOTTOM:
                    if (point.Y <= _drawingBounds.Bottom) return true;
                    return false;
            }
            return false;
        }

        private PointF BoundIntersection(PointF start, PointF end, int direction)
        {
            PointF result = new PointF();
            switch (direction)
            {
                case BOUND_RIGHT:
                    result.X = _drawingBounds.Right;
                    result.Y = start.Y + (end.Y - start.Y) * (_drawingBounds.Right - start.X) / (end.X - start.X);
                    break;
                case BOUND_LEFT:
                    result.X = _drawingBounds.Left;
                    result.Y = start.Y + (end.Y - start.Y) * (_drawingBounds.Left - start.X) / (end.X - start.X);
                    break;
                case BOUND_TOP:
                    result.Y = _drawingBounds.Top;
                    result.X = start.X + (end.X - start.X) * (_drawingBounds.Top - start.Y) / (end.Y - start.Y);
                    break;
                case BOUND_BOTTOM:
                    result.Y = _drawingBounds.Bottom;
                    result.X = start.X + (end.X - start.X) * (_drawingBounds.Bottom - start.Y) / (end.Y - start.Y);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This specific overload works with arrays of doubles instead of PointF structures.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="vertexValues">The list of arrays of doubles where the X index is 0 and the Y index is 1.</param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds</returns>
        public List<double[]> Clip(List<double[]> vertexValues)
        {
            List<double[]> result = vertexValues;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
            }

            return result;
        }

        private List<double[]> ClipDirection(IEnumerable<double[]> points, int direction)
        {
            bool previousInside = true;
            List<double[]> result = new List<double[]>();
            double[] previous = new double[2];
            bool isFirst = true;
            foreach (double[] point in points)
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
                if (result[0][X] != result[result.Count - 1][X] || result[0][Y] != result[result.Count - 1][Y])
                {
                    result.Add(new[] { result[0][X], result[0][Y] });
                }
            }
            return result;
        }

        private bool IsInside(double[] point, int direction)
        {
            switch (direction)
            {
                case BOUND_RIGHT:
                    if (point[X] <= _drawingBounds.Right) return true;
                    return false;
                case BOUND_LEFT:
                    if (point[X] >= _drawingBounds.Left) return true;
                    return false;
                case BOUND_TOP:
                    if (point[Y] >= _drawingBounds.Top) return true;
                    return false;
                case BOUND_BOTTOM:
                    if (point[Y] <= _drawingBounds.Bottom) return true;
                    return false;
            }
            return false;
        }

        private double[] BoundIntersection(double[] start, double[] end, int direction)
        {
            double[] result = new double[2];
            switch (direction)
            {
                case BOUND_RIGHT:
                    result[X] = _drawingBounds.Right;
                    result[Y] = start[Y] + (end[Y] - start[Y]) * (_drawingBounds.Right - start[X]) / (end[X] - start[X]);
                    break;
                case BOUND_LEFT:
                    result[X] = _drawingBounds.Left;
                    result[Y] = start[Y] + (end[Y] - start[Y]) * (_drawingBounds.Left - start[X]) / (end[X] - start[X]);
                    break;
                case BOUND_TOP:
                    result[Y] = _drawingBounds.Top;
                    result[X] = start[X] + (end[X] - start[X]) * (_drawingBounds.Top - start[Y]) / (end[Y] - start[Y]);
                    break;
                case BOUND_BOTTOM:
                    result[Y] = _drawingBounds.Bottom;
                    result[X] = start[X] + (end[X] - start[X]) * (_drawingBounds.Bottom - start[Y]) / (end[Y] - start[Y]);
                    break;
            }
            return result;
        }
    }
}