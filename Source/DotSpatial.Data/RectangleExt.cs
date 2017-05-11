// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/7/2009 5:56:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// Contains various extensions for Rectangle.
    /// </summary>
    public static class RectangleExt
    {
        #region Methods

        /// <summary>
        /// Calculates the intersection by casting the floating point values to integer values.
        /// </summary>
        /// <param name="self">This rectangle</param>
        /// <param name="other">The floating point rectangle to calculate against</param>
        /// <returns>True, if this intersects with the other rectangle.</returns>
        public static bool IntersectsWith(this Rectangle self, RectangleF other)
        {
            var temp = new Rectangle((int)other.X, (int)other.Y, (int)other.Width, (int)other.Height);
            return self.IntersectsWith(temp);
        }

        /// <summary>
        /// Tests the location of the point. If the point is outside of the current rectangle, then the bounds
        /// of the rectangle are adjusted to include the new point.
        /// </summary>
        /// <param name="self">this</param>
        /// <param name="newPoint">The point that gets included.</param>
        public static void ExpandToInclude(this Rectangle self, Point newPoint)
        {
            int xMin = Math.Min(self.X, newPoint.X);
            int yMin = Math.Min(self.Y, newPoint.Y);
            int xMax = Math.Max(self.Right, newPoint.X);
            int yMax = Math.Max(self.Bottom, newPoint.Y);
            self.X = xMin;
            self.Y = yMin;
            self.Width = xMax - xMin;
            self.Height = yMax - yMin;
        }

        /// <summary>
        /// Expands the rectangle by the specified integer distance in all directions.
        /// </summary>
        /// <param name="self">The rectangle to expand</param>
        /// <param name="distance">The distance </param>
        /// <returns>The expanded rectangle.</returns>
        public static Rectangle ExpandBy(this Rectangle self, int distance)
        {
            return new Rectangle(self.X - distance, self.Y - distance, self.Width + (2 * distance), self.Height + (2 * distance));
        }

        /// <summary>
        /// Expands the rectangle by the specified integer distances.
        /// </summary>
        /// <param name="self">this</param>
        /// <param name="dx">Distance to add in x direction.</param>
        /// <param name="dy">Distance to add in y direction.</param>
        /// <returns>The expanded rectangle.</returns>
        public static Rectangle ExpandBy(this Rectangle self, int dx, int dy)
        {
            return new Rectangle(self.X - dx, self.Y - dy, self.Width + (2 * dx), self.Height + (2 * dy));
        }

        #endregion
    }
}