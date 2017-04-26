// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleand up the raw source from code project
// ********************************************************************************************************

using System;

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// Contains several helpful tools that allow the voronoi polygon creation to work.
    /// </summary>
    public abstract class MathTools
    {
        #region Methods

        /// <summary>
        /// Checks whether the given triangle is counter clockwise.
        /// </summary>
        /// <param name="p0X">First x value.</param>
        /// <param name="p0Y">First y value.</param>
        /// <param name="p1X">Second x value.</param>
        /// <param name="p1Y">Second y value.</param>
        /// <param name="p2X">Third x value.</param>
        /// <param name="p2Y">Third y value.</param>
        /// <param name="plusOneOnZeroDegrees">Indicates whether to add 0 to the result if the result is 0</param>
        /// <returns>+1 for counter-clockwise, -1 for clockwise, 0 for collinear</returns>
        public static int Ccw(double p0X, double p0Y, double p1X, double p1Y, double p2X, double p2Y, bool plusOneOnZeroDegrees)
        {
            double dx1 = p1X - p0X;
            double dy1 = p1Y - p0Y;
            double dx2 = p2X - p0X;
            double dy2 = p2Y - p0Y;
            if (dx1 * dy2 > dy1 * dx2) return +1;
            if (dx1 * dy2 < dy1 * dx2) return -1;
            if ((dx1 * dx2 < 0) || (dy1 * dy2 < 0)) return -1;
            if ((dx1 * dx1) + (dy1 * dy1) < (dx2 * dx2) + (dy2 * dy2) && plusOneOnZeroDegrees)
                return +1;
            return 0;
        }

        /// <summary>
        /// Calculates the euclidean distance.
        /// </summary>
        /// <param name="x1">First x value.</param>
        /// <param name="y1">First y value.</param>
        /// <param name="x2">Second x value.</param>
        /// <param name="y2">Second y value.</param>
        /// <returns>The distance between the two points.</returns>
        public static double Dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        #endregion
    }
}