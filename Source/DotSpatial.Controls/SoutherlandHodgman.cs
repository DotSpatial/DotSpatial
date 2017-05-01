// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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
    /// Southerland Hodgman polygon clipper.
    /// </summary>
    public class SoutherlandHodgman
    {
        #region Fields

        private const int BoundBottom = 3;
        private const int BoundLeft = 2;
        private const int BoundRight = 0;
        private const int BoundTop = 1;
        private const int X = 0;
        private const int Y = 1;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SoutherlandHodgman"/> class.
        /// </summary>
        /// <param name="clipRect">The clipping rectangle.</param>
        public SoutherlandHodgman(Rectangle clipRect)
        {
            ClippingRectangle = clipRect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoutherlandHodgman"/> class with a default clipping rectangle of -32000|64000 in both directions.
        /// </summary>
        public SoutherlandHodgman()
        {
            ClippingRectangle = new Rectangle(-32000, -32000, 64000, 64000);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the clipping rectangle used in subsequent Clip calls.
        /// </summary>
        public Rectangle ClippingRectangle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Southerland-Hodgman clip using the actual drawing coordinates.
        /// This hopefully will be much faster than NTS which seems unncessarilly slow to calculate.
        /// http://www.codeguru.com/cpp/misc/misc/graphics/article.php/c8965
        /// </summary>
        /// <param name="points">Points that get clipped.</param>
        /// <returns>A modified list of points that has been clipped to the drawing bounds.</returns>
        public List<PointF> Clip(List<PointF> points)
        {
            List<PointF> result = points;
            for (int direction = 0; direction < 4; direction++)
            {
                result = ClipDirection(result, direction);
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

        private PointF BoundIntersection(PointF start, PointF end, int direction)
        {
            PointF result = default(PointF);
            switch (direction)
            {
                case BoundRight:
                    result.X = ClippingRectangle.Right;
                    result.Y = start.Y + ((end.Y - start.Y) * (ClippingRectangle.Right - start.X) / (end.X - start.X));
                    break;
                case BoundLeft:
                    result.X = ClippingRectangle.Left;
                    result.Y = start.Y + ((end.Y - start.Y) * (ClippingRectangle.Left - start.X) / (end.X - start.X));
                    break;
                case BoundTop:
                    result.Y = ClippingRectangle.Top;
                    result.X = start.X + ((end.X - start.X) * (ClippingRectangle.Top - start.Y) / (end.Y - start.Y));
                    break;
                case BoundBottom:
                    result.Y = ClippingRectangle.Bottom;
                    result.X = start.X + ((end.X - start.X) * (ClippingRectangle.Bottom - start.Y) / (end.Y - start.Y));
                    break;
            }

            return result;
        }

        private double[] BoundIntersection(double[] start, double[] end, int direction)
        {
            double[] result = new double[2];
            switch (direction)
            {
                case BoundRight:
                    result[X] = ClippingRectangle.Right;
                    result[Y] = start[Y] + ((end[Y] - start[Y]) * (ClippingRectangle.Right - start[X]) / (end[X] - start[X]));
                    break;
                case BoundLeft:
                    result[X] = ClippingRectangle.Left;
                    result[Y] = start[Y] + ((end[Y] - start[Y]) * (ClippingRectangle.Left - start[X]) / (end[X] - start[X]));
                    break;
                case BoundTop:
                    result[Y] = ClippingRectangle.Top;
                    result[X] = start[X] + ((end[X] - start[X]) * (ClippingRectangle.Top - start[Y]) / (end[Y] - start[Y]));
                    break;
                case BoundBottom:
                    result[Y] = ClippingRectangle.Bottom;
                    result[X] = start[X] + ((end[X] - start[X]) * (ClippingRectangle.Bottom - start[Y]) / (end[Y] - start[Y]));
                    break;
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

                if (previousInside && !inside)
                {
                    if (!previous.IsEmpty)
                    {
                        // crossing the boundary going out, so insert the intersection instead
                        result.Add(BoundIntersection(previous, point, direction));
                    }

                    previous = point;
                }

                if (!previousInside && inside)
                {
                    // crossing the boundary going in, so insert the intersection AND the new point
                    result.Add(BoundIntersection(previous, point, direction));
                    result.Add(point);
                    previous = point;
                }

                if (!previousInside && !inside)
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

                if (previousInside && !inside)
                {
                    if (!isFirst)
                    {
                        // crossing the boundary going out, so insert the intersection instead
                        result.Add(BoundIntersection(previous, point, direction));
                    }

                    previous = point;
                }

                if (!previousInside && inside)
                {
                    // crossing the boundary going in, so insert the intersection AND the new point
                    result.Add(BoundIntersection(previous, point, direction));
                    result.Add(point);
                    previous = point;
                }

                if (!previousInside && !inside)
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

        private bool IsInside(PointF point, int direction)
        {
            switch (direction)
            {
                case BoundRight:
                    if (point.X <= ClippingRectangle.Right) return true;
                    return false;
                case BoundLeft:
                    if (point.X >= ClippingRectangle.Left) return true;
                    return false;
                case BoundTop:
                    if (point.Y >= ClippingRectangle.Top) return true;
                    return false;
                case BoundBottom:
                    if (point.Y <= ClippingRectangle.Bottom) return true;
                    return false;
            }

            return false;
        }

        private bool IsInside(double[] point, int direction)
        {
            switch (direction)
            {
                case BoundRight:
                    if (point[X] <= ClippingRectangle.Right) return true;
                    return false;
                case BoundLeft:
                    if (point[X] >= ClippingRectangle.Left) return true;
                    return false;
                case BoundTop:
                    if (point[Y] >= ClippingRectangle.Top) return true;
                    return false;
                case BoundBottom:
                    if (point[Y] <= ClippingRectangle.Bottom) return true;
                    return false;
            }

            return false;
        }

        #endregion
    }
}