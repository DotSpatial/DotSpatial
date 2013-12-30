// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
    /// RectangleEM
    /// </summary>
    public static class RectangleExt
    {
        #region Methods

        /// <summary>
        /// Calculates the intersection by casting the floating point values to integer values.
        /// </summary>
        /// <param name="self">This rectangle</param>
        /// <param name="other">The floating point rectangle to calculate against</param>
        /// <returns></returns>
        public static bool IntersectsWith(this Rectangle self, RectangleF other)
        {
            Rectangle temp = new Rectangle((int)other.X, (int)other.Y, (int)other.Width, (int)other.Height);
            return self.IntersectsWith(temp);
        }

        /// <summary>
        /// Tests the location of the point.  If the point is outside of the current rectangle, then the bounds
        /// of the rectangle are adjusted to include the new point.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="newPoint"></param>
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
        public static Rectangle ExpandBy(this Rectangle self, int distance)
        {
            return new Rectangle(self.X - distance, self.Y - distance, self.Width + 2 * distance, self.Height + 2 * distance);
        }

        #endregion

        #region Properties

        #endregion
    }
}