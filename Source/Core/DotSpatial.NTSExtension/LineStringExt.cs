// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Contains extension methodes for NetTopologySuite.Geomtetries.LineString.
    /// </summary>
    public static class LineStringExt
    {
        #region Methods

        /// <summary>
        /// Given the specified test point, this checks each segment, and will return the closest point on the specified segment.
        /// </summary>
        /// <param name="self">The LineString, whose point is returned.</param>
        /// <param name="testPoint">The point to test.</param>
        /// <returns>The closest point.</returns>
        public static Coordinate ClosestPoint(this LineString self, Coordinate testPoint)
        {
            Coordinate closest = self.GetCoordinateN(0);
            double dist = double.MaxValue;
            for (int i = 0; i < self.NumPoints - 1; i++)
            {
                LineSegment s = new(self.GetCoordinateN(i), self.GetCoordinateN(i + 1));
                Coordinate temp = s.ClosestPoint(testPoint);
                double tempDist = testPoint.Distance(temp);
                if (tempDist < dist)
                {
                    dist = tempDist;
                    closest = temp;
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets the value of the angle between the StartPoint and the EndPoint in Radian.
        /// </summary>
        /// <param name="self">The LineString, whose angle is returned.</param>
        /// <remarks>added by JLeiss.</remarks>
        /// <returns>The angle between start end endpoint in radian.</returns>
        public static double RadAngle(this LineString self)
        {
            double deltaX = self.EndPoint.X - self.StartPoint.X;
            double deltaY = self.EndPoint.Y - self.StartPoint.Y;
            double winkel = Math.Atan(deltaY / deltaX);

            if (deltaX < 0)
            {
                if (deltaY <= 0) winkel += Math.PI;
                if (deltaY > 0) winkel -= Math.PI;
            }

            return winkel;
        }

        #endregion
    }
}