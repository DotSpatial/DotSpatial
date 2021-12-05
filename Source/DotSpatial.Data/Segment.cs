// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.NTSExtension;
using NetTopologySuite.Geometries;
using NetTopologySuite.Mathematics;

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents segment between 2 vertices.
    /// </summary>
    public class Segment
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="x1">The x value of the start point.</param>
        /// <param name="y1">The y value of the start point.</param>
        /// <param name="x2">The x value of the end point.</param>
        /// <param name="y2">The y value of the end point.</param>
        public Segment(double x1, double y1, double x2, double y2)
        {
            P1 = new Vertex(x1, y1);
            P2 = new Vertex(x2, y2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="p1">The start point.</param>
        /// <param name="p2">The end point.</param>
        public Segment(Vertex p1, Vertex p2)
        {
            P1 = p1;
            P2 = p2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the precision for calculating equality, but this is just a re-direction to Vertex.Epsilon.
        /// </summary>
        public static double Epsilon
        {
            get { return Vertex.Epsilon; }
            set { Vertex.Epsilon = value; }
        }

        /// <summary>
        /// Gets or sets the start point of the segment.
        /// </summary>
        public Vertex P1 { get; set; }

        /// <summary>
        /// Gets or sets the end point of the segment.
        /// </summary>
        public Vertex P2 { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the intersection count to detect if there is an intersection.
        /// </summary>
        /// <param name="other">Segment to check against.</param>
        /// <returns>True if there was an intersection.</returns>
        public bool Intersects(Segment other)
        {
            return IntersectionCount(other) > 0;
        }

        /// <summary>
        /// Calculates the shortest distance to this line segment from the specified MapWinGIS.Point.
        /// </summary>
        /// <param name="point">A MapWinGIS.Point specifing the location to find the distance to the line.</param>
        /// <returns>A double value that is the shortest distance from the given Point to this line segment.</returns>
        public double DistanceTo(Coordinate point)
        {
            Vertex p = new Vertex(point.X, point.Y);
            Vertex pt = ClosestPointTo(p);
            var dist = new Vector2D(new Coordinate(pt.X, pt.Y), point);
            return dist.Length();
        }

        /// <summary>
        /// Returns a vertex representing the closest point on this line segment from a given vertex.
        /// </summary>
        /// <param name="point">The point we want to be close to.</param>
        /// <returns>The point on this segment that is closest to the given point.</returns>
        public Vertex ClosestPointTo(Vertex point)
        {
            EndPointInteraction endPointFlag;
            return ClosestPointTo(point, false, out endPointFlag);
        }

        /// <summary>
        /// Returns a vertex representing the closest point on this line segment from a given vertex.
        /// </summary>
        /// <param name="point">The point we want to be close to.</param>
        /// <param name="isInfiniteLine">If true treat the line as infinitly long.</param>
        /// <param name="endPointFlag">Outputs 0 if the vertex is on the line segment, 1 if beyond P0, 2 if beyong P1 and -1 if P1=P2.</param>
        /// <returns>The point on this segment or infinite line that is closest to the given point.</returns>
        public Vertex ClosestPointTo(Vertex point, bool isInfiniteLine, out EndPointInteraction endPointFlag)
        {
            // If the points defining this segment are the same, we treat the segment as a point
            // special handling to avoid 0 in denominator later
            if (P2.X == P1.X && P2.Y == P1.Y)
            {
                endPointFlag = EndPointInteraction.P1EqualsP2;
                return P1;
            }

            // http://softsurfer.com/Archive/algorithm_0102/algorithm_0102.htm
            var v = ToVector2D(); // vector from p1 to p2 in the segment
            Vector2D w = new Vector2D(new Coordinate(P1.X, P1.Y), new Coordinate(point.X, point.Y));

            double c1 = w.Dot(v); // the dot product represents the projection onto the line

            if (c1 < 0)
            {
                endPointFlag = EndPointInteraction.PastP1;
                if (!isInfiniteLine) // The closest point on the segment to Point is p1
                    return P1;
            }

            double c2 = v.Dot(v);

            if (c2 <= c1)
            {
                endPointFlag = EndPointInteraction.PastP2;
                if (!isInfiniteLine) // The closest point on the segment to Point is p2
                    return P2;
            }

            // The closest point on the segment is perpendicular to the point,
            // but somewhere on the segment between P1 and P2
            endPointFlag = EndPointInteraction.OnLine;
            double b = c1 / c2;
            v *= b;
            Vertex pb = new Vertex(P1.X + v.X, P1.Y + v.Y);
            return pb;
        }

        /// <summary>
        /// Casts this to a vector.
        /// </summary>
        /// <returns>This as vector.</returns>
        public Vector3D ToVector3D()
        {
            double x = P2.X - P1.X;
            double y = P2.Y - P1.Y;
            return new Vector3D(x, y, 0);
        }

        /// <summary>
        /// Casts this to a vector.
        /// </summary>
        /// <returns>This as vector.</returns>
        public Vector2D ToVector2D()
        {
            double x = P2.X - P1.X;
            double y = P2.Y - P1.Y;
            return new Vector2D(x, y);
        }

        /// <summary>
        /// Determines the shortest distance between two segments.
        /// </summary>
        /// <param name="lineSegment">Segment, The line segment to test against this segment.</param>
        /// <returns>Double, the shortest distance between two segments.</returns>
        public double DistanceTo(Segment lineSegment)
        {
            // http://www.geometryalgorithms.com/Archive/algorithm_0106/algorithm_0106.htm
            const double SmallNum = 0.00000001;
            Vector2D u = ToVector2D(); // Segment 1
            Vector2D v = lineSegment.ToVector2D(); // Segment 2
            Vector2D w = ToVector2D();
            double a = u.Dot(u);  // length of segment 1
            double b = u.Dot(v);  // length of segment 2 projected onto line 1
            double c = v.Dot(v);  // length of segment 2
            double d = u.Dot(w);
            double e = v.Dot(w);
            double dist = (a * c) - (b * b);
            double sc, sN, sD = dist;
            double tc, tN, tD = dist;

            // compute the line parameters of the two closest points
            if (dist < SmallNum)
            {
                // the lines are almost parallel force using point P0 on segment 1
                // to prevent possible division by 0 later
                sN = 0.0;
                sD = 1.0;
                tN = e;
                tD = c;
            }
            else
            {
                // get the closest points on the infinite lines
                sN = (b * e) - (c * d);
                tN = (a * e) - (b * d);
                if (sN < 0.0)
                {
                    // sc < 0 => the s=0 edge is visible
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    // sc > 1 => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0)
            {
                // tc < 0 => the t=0 edge is visible
                tN = 0.0;

                // recompute sc for this edge
                if (-d < 0.0)
                {
                    sN = 0.0;
                }
                else if (-d > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {
                // tc > 1 => the t = 1 edge is visible
                // recompute sc for this edge
                if ((-d + b) < 0.0)
                {
                    sN = 0;
                }
                else if ((-d + b) > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = -d + b;
                    sD = a;
                }
            }

            // finally do the division to get sc and tc
            if (Math.Abs(sN) < SmallNum)
            {
                sc = 0.0;
            }
            else
            {
                sc = sN / sD;
            }

            if (Math.Abs(tN) < SmallNum)
            {
                tc = 0.0;
            }
            else
            {
                tc = tN / tD;
            }

            // get the difference of the two closest points
            Vector2D dU = u * sc;
            Vector2D dV = v * tc;
            Vector2D dP = w + dU - dV;

            // S1(sc) - S2(tc)
            return dP.Length();
        }

        /// <summary>
        /// Returns 0 if no intersections occur, 1 if an intersection point is found,
        /// and 2 if the segments are colinear and overlap.
        /// </summary>
        /// <param name="other">The segment to check against.</param>
        /// <returns>0 = no intersection, 1 = intersection point found, 2 = segments are collinear or overlap.</returns>
        public int IntersectionCount(Segment other)
        {
            double x1 = P1.X;
            double y1 = P1.Y;
            double x2 = P2.X;
            double y2 = P2.Y;
            double x3 = other.P1.X;
            double y3 = other.P1.Y;
            double x4 = other.P2.X;
            double y4 = other.P2.Y;
            double denom = ((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1));

            // The case of two degenerate segements
            if ((x1 == x2) && (y1 == y2) && (x3 == x4) && (y3 == y4))
            {
                if ((x1 != x3) || (y1 != y3))
                    return 0;
            }

            // if denom is 0, then the two lines are parallel
            double na = ((x4 - x3) * (y1 - y3)) - ((y4 - y3) * (x1 - x3));
            double nb = ((x2 - x1) * (y1 - y3)) - ((y2 - y1) * (x1 - x3));

            // if denom is 0 AND na and nb are 0, then the lines are coincident and DO intersect
            if (Math.Abs(denom) < Epsilon && Math.Abs(na) < Epsilon && Math.Abs(nb) < Epsilon) return 2;

            // If denom is 0, but na or nb are not 0, then the lines are parallel and not coincident
            if (denom == 0) return 0;
            double ua = na / denom;
            double ub = nb / denom;
            if (ua < 0 || ua > 1) return 0; // not intersecting with segment a
            if (ub < 0 || ub > 1) return 0; // not intersecting with segment b

            // If we get here, then one intersection exists and it is found on both line segments
            return 1;
        }

        /// <summary>
        /// Tests to see if the specified segment contains the point within Epsilon tollerance.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>True if the point intersects with the segment.</returns>
        public bool IntersectsVertex(Vertex point)
        {
            double x1 = P1.X;
            double y1 = P1.Y;
            double x2 = P2.X;
            double y2 = P2.Y;
            double pX = point.X;
            double pY = point.Y;

            // Collinear
            if (Math.Abs(((x2 - x1) * (pY - y1)) - ((pX - x1) * (y2 - y1))) > Epsilon) return false;

            // In the x is in bounds and it is colinear, it is on the segment
            if (x1 < x2)
            {
                if (x1 <= pX && pX <= x2) return true;
            }
            else
            {
                if (x2 <= pX && pX <= x1) return true;
            }

            return false;
        }

        #endregion
    }
}