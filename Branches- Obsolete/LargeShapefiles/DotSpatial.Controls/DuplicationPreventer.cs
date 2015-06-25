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
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Contains methods to remove duplicates
    /// </summary>
    public static class DuplicationPreventer
    {
        #region Methods

        /// <summary>
        /// Cycles through the PointF points, where necessary and removes duplicate points
        /// that are found at the integer level.
        /// </summary>
        public static IEnumerable<Point> Clean(IEnumerable<PointF> points)
        {
            return Clean(points,
               v => new Point(Convert.ToInt32(v.X), Convert.ToInt32(v.Y)),
               v => float.IsNaN(v.X) || float.IsNaN(v.Y));
        }

        /// <summary>
        /// Cleans the enumerable of points by removing duplicates
        /// </summary>
        public static IEnumerable<Point> Clean(IEnumerable<double[]> points)
        {
            return Clean(points,
               v => new Point(Convert.ToInt32(v[0]), Convert.ToInt32(v[1])),
               v => double.IsNaN(v[0]) || double.IsNaN(v[1]));
        }

        /// <summary>
        /// Cleans the enumerable of vertex by removing duplicates
        /// </summary>
        public static IEnumerable<Point> Clean(IEnumerable<Vertex> points)
        {
            return Clean(points,
                v => new Point(Convert.ToInt32(v.X), Convert.ToInt32(v.Y)),
                v => double.IsNaN(v.X) || double.IsNaN(v.Y));
        }

        private static IEnumerable<Point> Clean<T>(IEnumerable<T> points, Func<T, Point> getXY, Func<T, bool> isNan)
        {
            var previous = Point.Empty;
            var isFirst = true;
            foreach (var point in points)
            {
                if (isNan(point)) continue;

                var pt = getXY(point);
                if (isFirst || pt.X != previous.X || pt.Y != previous.Y)
                {
                    isFirst = false;
                    previous = pt;
                    yield return pt;
                }
            }
        }

        #endregion
    }
}