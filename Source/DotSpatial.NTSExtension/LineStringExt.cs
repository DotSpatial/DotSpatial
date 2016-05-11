using System;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    public static class LineStringExt
    {

        /// <summary>
        /// Gets the value of the angle between the StartPoint and the EndPoint in Radian.
        /// </summary>
        /// <remarks>added by JLeiss</remarks>
        public static double RadAngle(this ILineString self)
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

        /// <summary>
        /// Given the specified test point, this checks each segment, and will
        /// return the closest point on the specified segment.
        /// </summary>
        /// <param name="testPoint">The point to test.</param>
        public static Coordinate ClosestPoint(this ILineString self, Coordinate testPoint)
        {
            Coordinate closest = self.GetCoordinateN(0);
            double dist = double.MaxValue;
            for (int i = 0; i < self.NumPoints - 1; i++)
            {
                LineSegment s = new LineSegment(self.GetCoordinateN(i), self.GetCoordinateN(i + 1));
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

    }
}
