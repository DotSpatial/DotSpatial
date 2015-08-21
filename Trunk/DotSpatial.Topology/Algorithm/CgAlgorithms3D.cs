using System;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Mathematics;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Basic computational geometry algorithms for geometry and coordinates defined in 3-dimensional Cartesian space.
    /// @author mdavis
    /// </summary>
    public class CGAlgorithms3D
    {
        #region Methods

        /// <summary>
        /// Computes the distance between two points.
        /// </summary>
        /// <param name="p0">the first point</param>
        /// <param name="p1">the second point</param>
        /// <returns>the distance between the the two points</returns>
        public static double Distance(Coordinate p0, Coordinate p1)
        {
            // default to 2D distance if either Z is not set
            if (double.IsNaN(p0.Z) || double.IsNaN(p1.Z)) return p0.Distance(p1);

            double dx = p0.X - p1.X;
            double dy = p0.Y - p1.Y;
            double dz = p0.Z - p1.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Computes the distance between a point and a 3D segments.
        /// </summary>
        /// <param name="p">the point</param>
        /// <param name="a">the start point of the segment</param>
        /// <param name="b">the end point of the segment</param>
        /// <returns>the distance between the segment and the point</returns>
        public static double DistancePointSegment(Coordinate p, Coordinate a, Coordinate b)
        {
            // if start = end, then just compute distance to one of the endpoints
            if (a.Equals3D(b)) return Distance(p, a);

            // otherwise use comp.graphics.algorithms Frequently Asked Questions method
            /*
             * (1) r = AC dot AB 
             *         --------- 
             *         ||AB||^2 
             *         
             * r has the following meaning: 
             *   r=0 P = A 
             *   r=1 P = B 
             *   r<0 P is on the backward extension of AB 
             *   r>1 P is on the forward extension of AB 
             *   0<r<1 P is interior to AB
             */

            double len2 = (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y) + (b.Z - a.Z) * (b.Z - a.Z);
            if (double.IsNaN(len2)) throw new ArgumentException("Ordinates must not be NaN");
            double r = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y) + (p.Z - a.Z) * (b.Z - a.Z)) / len2;

            if (r <= 0.0) return Distance(p, a);
            if (r >= 1.0) return Distance(p, b);

            // compute closest point q on line segment
            double qx = a.X + r * (b.X - a.X);
            double qy = a.Y + r * (b.Y - a.Y);
            double qz = a.Z + r * (b.Z - a.Z);
            // result is distance from p to q
            double dx = p.X - qx;
            double dy = p.Y - qy;
            double dz = p.Z - qz;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Computes the distance between two 3D segments.
        /// </summary>
        /// <param name="a">the start point of the first segment</param>
        /// <param name="b">the end point of the first segment</param>
        /// <param name="c">the start point of the second segment</param>
        /// <param name="d">the end point of the second segment</param>
        /// <returns>the distance between the segments</returns>
        public static double DistanceSegmentSegment(Coordinate a, Coordinate b, Coordinate c, Coordinate d)
        {
            // This calculation is susceptible to roundoff errors when passed large ordinate values.
            // It may be possible to improve this by using {@link DD} arithmetic.
            if (a.Equals3D(b)) return DistancePointSegment(a, c, d);
            if (c.Equals3D(b)) return DistancePointSegment(c, a, b);

            /**
             * Algorithm derived from http://softsurfer.com/Archive/algorithm_0106/algorithm_0106.htm
             */
            double a1 = Vector3D.Dot(a, b, a, b);
            double b1 = Vector3D.Dot(a, b, c, d);
            double c1 = Vector3D.Dot(c, d, c, d);
            double d1 = Vector3D.Dot(a, b, c, a);
            double e1 = Vector3D.Dot(c, d, c, a);

            double denom = a1 * c1 - b1 * b1;
            if (double.IsNaN(denom)) throw new ArgumentException("Ordinates must not be NaN");

            double s;
            double t;
            if (denom <= 0.0)
            {
                // The lines are parallel. 
                // In this case solve for the parameters s and t by assuming s is 0.
                s = 0;
                // choose largest denominator for optimal numeric conditioning
                t = (b1 > c1) ? d1 / b1 : e1 / c1;
            }
            else
            {
                s = (b1 * e1 - c1 * d1) / denom;
                t = (a1 * e1 - b1 * d1) / denom;
            }
            if (s < 0) return DistancePointSegment(a, c, d);
            if (s > 1) return DistancePointSegment(b, c, d);
            if (t < 0) return DistancePointSegment(c, a, b);
            if (t > 1) return DistancePointSegment(d, a, b);

            /**
             * The closest points are in interiors of segments,
             * so compute them directly
             */
            double x1 = a.X + s * (b.X - a.X);
            double y1 = a.Y + s * (b.Y - a.Y);
            double z1 = a.Z + s * (b.Z - a.Z);

            double x2 = c.X + t * (d.X - c.X);
            double y2 = c.Y + t * (d.Y - c.Y);
            double z2 = c.Z + t * (d.Z - c.Z);

            // length (p1-p2)
            return Distance(new Coordinate(x1, y1, z1), new Coordinate(x2, y2, z2));
        }

        #endregion
    }
}