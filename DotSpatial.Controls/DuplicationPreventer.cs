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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/18/2009 2:09:24 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Controls
{
    /// <summary>
    /// DuplicationPreventer
    /// </summary>
    public static class DuplicationPreventer
    {
        const int X = 0;
        const int Y = 1;

        #region Methods

        /// <summary>
        /// Cycles through the PointF points, where necessary and removes duplicate points
        /// that are found at the integer level.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Point> Clean(List<PointF> points)
        {
            List<Point> result = new List<Point>();
            Point previous = Point.Empty;
            bool isFirst = true;
            foreach (PointF point in points)
            {
                Point pt = new Point();
                pt.X = Convert.ToInt32(point.X);
                pt.Y = Convert.ToInt32(point.Y);
                if (isFirst || pt.X != previous.X || pt.Y != previous.Y)
                {
                    isFirst = false;
                    previous = pt;
                    result.Add(pt);
                }
            }
            return result;
        }

        /// <summary>
        /// Cleans the list of points by removing duplicates
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Point> Clean(List<double[]> points)
        {
            List<Point> result = new List<Point>();
            Point previous = Point.Empty;
            bool isFirst = true;
            foreach (double[] point in points)
            {
                if (double.IsNaN(point[X]) || double.IsNaN(point[Y])) continue;
                Point pt = new Point();
                pt.X = Convert.ToInt32(point[X]);
                pt.Y = Convert.ToInt32(point[Y]);
                if (isFirst || pt.X != previous.X || pt.Y != previous.Y)
                {
                    isFirst = false;
                    previous = pt;
                    result.Add(pt);
                }
            }
            return result;
        }

        #endregion
    }
}