using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using GeoFramework.Projections;

namespace GeoFramework
{
    /// <summary>
    /// Provides additional functionality for the RectangleF structure.
    /// </summary>
    public static class RectangleFHelper
    {

        /// <summary>
        /// Returns the point at the center of the specified rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF Center(RectangleF rectangle)
        {
            return new PointF((rectangle.Width * .5f) + rectangle.X, (rectangle.Height * .5f) + rectangle.Y);
        }

        /// <summary>
        /// Increases the height or broadens the width of a rectangle to match the specified aspect ratio.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        public static RectangleF ToAspectRatio(RectangleF rectangle, float aspectRatio)
        {
            float projectedAspect = ((float)rectangle.Width / (float)rectangle.Height);

            if (aspectRatio > projectedAspect)
            {
#if !PocketPC
                rectangle.Inflate((aspectRatio * rectangle.Height - rectangle.Width) * .5f, 0);
#else
                return RectangleFHelper.Inflate(rectangle, (aspectRatio * rectangle.Height - rectangle.Width) * .5f, 0);
#endif
            }
            else if (aspectRatio < projectedAspect)
            {
#if !PocketPC
                rectangle.Inflate(0, (rectangle.Width / aspectRatio - rectangle.Height) * .5f);
#else
                return RectangleFHelper.Inflate(rectangle, 0, (rectangle.Width / aspectRatio - rectangle.Height) * .5f);
#endif
            }
            return rectangle;
        }

        /// <summary>
        /// Shortens the height or narrows the width of a rectangle to match the specified aspect ratio.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        public static RectangleF ToAspectRatioB(RectangleF rectangle, float aspectRatio)
        {
            float projectedAspect = ((float)rectangle.Width / (float)rectangle.Height);

            if (aspectRatio > projectedAspect)
            {
#if !PocketPC
                rectangle.Inflate(0, (rectangle.Width / aspectRatio - rectangle.Height) * .5f);
#else
                return RectangleFHelper.Inflate(rectangle, 0, (rectangle.Width / aspectRatio - rectangle.Height) * .5f);
#endif
            }
            else if (aspectRatio < projectedAspect)
            {
#if !PocketPC
                rectangle.Inflate((aspectRatio * rectangle.Height - rectangle.Width) * .5f, 0);
#else
                return RectangleFHelper.Inflate(rectangle, (aspectRatio * rectangle.Height - rectangle.Width) * .5f, 0);
#endif
            }
            return rectangle;
        }

        /// <summary>
        /// Returns the corners of a rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF[] Corners(RectangleF rectangle)
        {
            return new PointF[4]
            { 
                new PointF(rectangle.Left, rectangle.Top), 
                new PointF(rectangle.Right, rectangle.Top), 
                new PointF(rectangle.Right, rectangle.Bottom),
                new PointF(rectangle.Left, rectangle.Bottom) 
            };
        }

        /// <summary>
        /// Calculates the bounding rectangle for the supplied points.
        /// </summary>
        /// <returns></returns>
        public static RectangleF ComputeBoundingBox(PointF[] projectedPoints)
        {
            // ffs
            if (projectedPoints.Length == 0) return RectangleF.Empty;

            // Now figure out which points represent the maximum bounds, starting
            // with the first point
            float projectedLeft = projectedPoints[0].X;
            float projectedRight = projectedPoints[0].X;
            float projectedTop = projectedPoints[0].Y;
            float projectedBottom = projectedPoints[0].Y;

            // Now consider all other points
            int Limit = projectedPoints.Length;
            for (int index = 1; index < Limit; index++)
            {
                // Get the current projected point
                PointF projectedPoint = projectedPoints[index];

                // Now see if it exceeds the current bounds
                if (projectedPoint.X < projectedLeft)
                    projectedLeft = projectedPoint.X;
                if (projectedPoint.X > projectedRight)
                    projectedRight = projectedPoint.X;
                if (projectedPoint.Y < projectedTop)
                    projectedTop = projectedPoint.Y;
                if (projectedPoint.Y > projectedBottom)
                    projectedBottom = projectedPoint.Y;
            }

            // finally, return a rectangle with these bounds
#if PocketPC
            return RectangleFHelper.FromLTRB(projectedLeft, projectedTop, projectedRight, projectedBottom);
#else
            return RectangleF.FromLTRB(projectedLeft, projectedTop, projectedRight, projectedBottom);
#endif
        }

        /// <summary>
        /// Returns the length of the hypotenuse of the specified rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static float Hypotenuse(RectangleF rectangle)
        {
            return (float)Math.Sqrt(Math.Pow(rectangle.Width, 2) + Math.Pow(rectangle.Height, 2));
        }

        /// <summary>
        /// Centers the rectangle on a specific point.
        /// </summary>
        /// <param name="rectangle">The rectangle to translate</param>
        /// <param name="point">The point on which to center the reactangle</param>
        /// <returns>The new rectangle centered on the specified point</returns>
        public static RectangleF CenterOn(RectangleF rectangle, PointF point)
        {
            PointF center = RectangleFHelper.Center(rectangle);
#if !PocketPC
            rectangle.Offset(point.X - center.X, point.Y - center.Y);
            return rectangle;
#else
            return RectangleFHelper.Offset(rectangle, point.X - center.X, point.Y - center.Y);
#endif
        }

        /// <summary>
        /// Returns whether any one of a rectangle's sides is a NaN (not a number).
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static bool IsNaN(RectangleF rectangle)
        {
            return double.IsNaN(rectangle.X * rectangle.Y * rectangle.Right * rectangle.Bottom);
        }

        /// <summary>
        /// Rotates a rectangle around its center
        /// </summary>
        /// <param name="rectangle">The rectangle to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The minimum bounding rectangle (MBR) of the rotated rectangle</returns>
        public static RectangleF Rotate(RectangleF rectangle, Angle angle)
        {
            return RectangleFHelper.RotateAt(rectangle, (float)angle.DecimalDegrees, RectangleFHelper.Center(rectangle));
        }

        /// <summary>
        /// Rotates a rectangle around its center
        /// </summary>
        /// <param name="rectangle">The rectangle to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The minimum bounding rectangle (MBR) of the rotated rectangle</returns>
        public static RectangleF Rotate(RectangleF rectangle, float angle)
        {
            return RectangleFHelper.RotateAt(rectangle, angle, RectangleFHelper.Center(rectangle));
        }

        /// <summary>
        /// Rotates a rectangle around a coordinate
        /// </summary>
        /// <param name="rectangle">The rectangle to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The minimum bounding rectangle (MBR) of the rotated rectangle</returns>
        public static RectangleF RotateAt(RectangleF rectangle, Angle angle, PointF center)
        {
            return RectangleFHelper.RotateAt(rectangle, (float)angle.DecimalDegrees, center);
        }

        /// <summary>
        /// Rotates a rectangle around a coordinate
        /// </summary>
        /// <param name="rectangle">The rectangle to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The minimum bounding rectangle (MBR) of the rotated rectangle</returns>
        public static RectangleF RotateAt(RectangleF rectangle, float angle, PointF center)
        {
            // The graphics transform method only accepts arrays :P
            PointF[] points = new PointF[4] 
            { 
                new PointF(rectangle.Left, rectangle.Top), 
                new PointF(rectangle.Right, rectangle.Top), 
                new PointF(rectangle.Right, rectangle.Bottom),
                new PointF(rectangle.Left, rectangle.Bottom) 
            };

            Matrix rotationMatrix = new Matrix();
            rotationMatrix.RotateAt((float)angle, center);
            rotationMatrix.TransformPoints(points);
            rotationMatrix.Dispose();

            // Return the result
            return RectangleFHelper.ComputeBoundingBox(points);
        }

        /// <summary>
        /// Rotates a coordinate around a coordinate
        /// </summary>
        /// <param name="point">The point to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The rectangle resulting from the rotation of the upperleft and lower rightt corners of the input rectangle</returns>
        public static PointF RotatePointF(PointF point, Angle angle, PointF center)
        {
            return RectangleFHelper.RotatePointF(point, (float)angle.DecimalDegrees, center);
        }

        /// <summary>
        /// Rotates a coordinate around a coordinate
        /// </summary>
        /// <param name="point">The point to apply the rotation</param>
        /// <param name="angle">The clockwise angle of the rotation</param>
        /// <returns>The rectangle resulting from the rotation of the upperleft and lower rightt corners of the input rectangle</returns>
        public static PointF RotatePointF(PointF point, float angle, PointF center)
        {
            // The graphics transform method only accepts arrays :P
            PointF[] points = new PointF[1] 
            { 
                point, 
            };

            Matrix rotationMatrix = new Matrix();
            rotationMatrix.RotateAt((float)angle, center);
            rotationMatrix.TransformPoints(points);
            rotationMatrix.Dispose();

            // Return the result
            return (points[0]);
        }

        public static bool IsRectangle(PointF[] points)
        {
            if (points.Length != 4) return false;

            // The rectangle could be rotated, we need to check 2 orientations
            return (
                (
                    points[0].X == points[3].X &&
                    points[0].Y == points[1].Y &&
                    points[0].X != points[2].X &&
                    points[0].Y != points[2].Y
                ) ||
                (
                    points[0].X == points[1].X &&
                    points[0].Y == points[3].Y &&
                    points[0].X != points[2].X &&
                    points[0].Y != points[2].Y
                )
            );
        }

#if PocketPC

        /// <summary>
        /// Compares the imput rectangles and caculates the portion of the new rctangle not included in the old.
        /// </summary>
        /// <param name="current"> The current rectangle</param>
        /// <param name="previous">The previous rectangle</param>
        /// <returns>An array of rectangles describing the difference between the input rectangles.</returns>
        /// <remarks>
        /// This funtion is a liner exclusive OR on 2 rectangles. It is catagorized by specifying the order of the 
        /// rectangles in a linear fashion so that the xor'd intersection is directional. A natural XOR intersection
        /// would include the portions of both rectangles not found the intersction of the two. A Linear XOR includes 
        /// only the portion of the current rectangle not found in the intersection of the two.
        /// </remarks>
        public static RectangleF[] GetRegionScans(RectangleF current, RectangleF previous)
        {
            // If the extents are equal, or one contains the other, or they're not intersecting there's nothing
            // to do. Return the current rectangle. 
            if (
                !current.Equals(previous) &&
                !Contains(current, previous) &&
                !Contains(previous, current) &&
                IntersectsWith(current, previous))
            {
                // Get the horizontal rectangle, uncovered from a north or south pan 
                RectangleF h = RectangleFHelper.FromLTRB(
                    current.Left, //current.Left < previous.Left ? current.Left : previous.Left,
                    current.Top < previous.Top ? current.Top : previous.Bottom,
                    current.Right, //current.Left < previous.Left ? previous.Right : current.Right,
                    current.Top < previous.Top ? previous.Top : current.Bottom
                    );

                // Get the vertical rectangle, uncovered from an east or west pan 
                RectangleF v = RectangleFHelper.FromLTRB(
                    current.Left < previous.Left ? current.Left : previous.Right,
                    current.Top < previous.Top ? previous.Top : current.Top,
                    current.Left < previous.Left ? previous.Left : current.Right,
                    current.Top < previous.Top ? current.Bottom : previous.Bottom
                    );

                // Retangles with no width or height are excluded
                if ((h.Height <= 0 || h.Width <= 0) && (v.Height <= 0 || v.Width <= 0))
                    return new RectangleF[] { current };

                // Retangles with no width or height are excluded
                if (h.Height <= 0 || h.Width <= 0)
                    return new RectangleF[] { v };
                if (v.Height <= 0 || v.Width <= 0)
                    return new RectangleF[] { h };

                return new RectangleF[] { h, v };
            }

            return new RectangleF[] { current };
        }

        public static RectangleF FromLTRB(float left, float top, float right, float bottom)
        {
            return new RectangleF(left, top, right - left, bottom - top);
        }

        public static RectangleF Intersect(RectangleF left, RectangleF right)
        {
            if (IntersectsWith(left, right))
            {
                float l = left.Left > right.Left ? left.Left : right.Left;
                float r = left.Right < right.Right ? left.Right : right.Right;
                float t = left.Top > right.Top ? left.Top : right.Top;
                float b = left.Bottom < right.Bottom ? left.Bottom : right.Bottom;

                return FromLTRB(l, t, r, b);
            }
            else
            {
                return RectangleF.Empty;
            }
        }

        public static RectangleF Union(RectangleF left, RectangleF right)
        {
            return FromLTRB(
                left.Left < right.Left ? left.Left : right.Left,
                left.Top < right.Top ? left.Top : right.Top,
                left.Right > right.Right ? left.Right : right.Right,
                left.Bottom > right.Bottom ? left.Bottom : right.Bottom);
        }

        public static RectangleF Offset(RectangleF rectangle, float x, float y)
        {
            rectangle.X += x;
            rectangle.Y += y;
            return rectangle;
        }

        public static RectangleF Inflate(RectangleF rectangle, float x, float y)
        {
            return RectangleFHelper.FromLTRB(rectangle.X - x, rectangle.Y - y, rectangle.Right + x, rectangle.Bottom + y);
        }

        public static bool Contains(RectangleF rectangle, PointF point)
        {
            return
                rectangle.Left <= point.X &&
                rectangle.Right >= point.X &&
                rectangle.Top <= point.Y &&
                rectangle.Bottom >= point.Y;
        }

        public static bool Contains(RectangleF left, RectangleF right)
        {
            return
                left.Left <= right.Left &&
                left.Right >= right.Right &&
                left.Top <= right.Top &&
                left.Bottom >= right.Bottom;
        }

        public static bool IntersectsWith(RectangleF left, RectangleF right)
        {
            return
                (
                    (left.Left <= right.Left && left.Right >= right.Left) ||
                    (left.Left <= right.Right && left.Right >= right.Right) ||
                    (left.Left <= right.Left && left.Right >= right.Right) ||
                    (left.Left >= right.Left && left.Right <= right.Right)
                ) && (
                    (left.Top <= right.Top && left.Bottom >= right.Top) ||
                    (left.Top <= right.Bottom && left.Bottom >= right.Bottom) ||
                    (left.Top <= right.Top && left.Bottom >= right.Bottom) ||
                    (left.Top >= right.Top && left.Bottom <= right.Bottom)
                );
        }
#endif
    }
}
