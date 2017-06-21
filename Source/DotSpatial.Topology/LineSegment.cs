// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// *********************************************************************************************************

using System;
using System.Text;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Represents a line segment defined by two <c>Coordinate</c>s.
    /// Provides methods to compute various geometric properties
    /// and relationships of line segments.
    /// This class is designed to be easily mutable (to the extent of
    /// having its contained points public).
    /// This supports a common pattern of reusing a single LineSegment
    /// object as a way of computing segment properties on the
    /// segments defined by arrays or lists of <c>Coordinate</c>s.
    /// </summary>
    [Serializable]
    public class LineSegment : ILineSegment
    {
        private Coordinate _p0, _p1;

        /// <summary>
        /// Creates an instance of LineSegment from two coordiantes
        /// </summary>
        /// <param name="p0">The first point of the segment</param>
        /// <param name="p1">The second point of the segment</param>
        public LineSegment(Coordinate p0, Coordinate p1)
        {
            _p0 = p0;
            _p1 = p1;
        }

        /// <summary>
        /// Creates a new instance of a LineSegment which implements
        /// ILineSegment and ILineSegmentBase from an ILineSegmentBase
        /// </summary>
        /// <param name="ls"></param>
        public LineSegment(ILineSegmentBase ls) : this(ls.P0, ls.P1) { }

        /// <summary>
        /// Creates a new instance of a LineSegment which implements
        /// ILineSegment and ILineSegmentBase
        /// </summary>
        public LineSegment() : this(new Coordinate(), new Coordinate()) { }

        #region ILineSegment Members

        /// <summary>
        /// returns the one of the ICoordinate that defines this linesegment
        /// </summary>
        public virtual Coordinate P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        /// <summary>
        /// returns the ICoordianteBase defining the second endpoint of the segment
        /// </summary>
        public virtual Coordinate P0
        {
            get { return _p0; }
            set { _p0 = value; }
        }

        /// <summary>
        /// Retrieves the i'th coordiante.  Since there are only two,
        /// i can be either 0 or 1.
        /// </summary>
        /// <param name="i">Integer, specifies the coordiante</param>
        /// <returns>A topologically complete ICoordinate</returns>
        public virtual Coordinate GetCoordinate(int i)
        {
            if (i == 0) return new Coordinate(_p0);
            return new Coordinate(_p1);
        }

        /// <summary>
        /// Defines a new LineSegment based on the previous line segment
        /// </summary>
        /// <param name="ls">The ILineSegmentBase</param>
        public virtual void SetCoordinates(ILineSegmentBase ls)
        {
            SetCoordinates(ls.P0, ls.P1);
        }

        /// <summary>
        /// Sets the new coordinates using the ICoordinate interfaces specified
        /// </summary>
        /// <param name="p0">The first endpoint</param>
        /// <param name="p1">The second endpoint</param>
        public virtual void SetCoordinates(Coordinate p0, Coordinate p1)
        {
            P0.X = p0.X;
            P0.Y = p0.Y;
            P1.X = p1.X;
            P1.Y = p1.Y;
        }

        /// <summary>
        /// Computes the length of the line segment.
        /// </summary>
        /// <returns>The length of the line segment.</returns>
        public virtual double Length
        {
            get
            {
                return new Coordinate(_p0).Distance(P1);
            }
        }

        /// <summary>
        /// Tests whether the segment is horizontal.
        /// </summary>
        /// <returns><c>true</c> if the segment is horizontal.</returns>
        public virtual bool IsHorizontal
        {
            get
            {
                return P0.Y == P1.Y;
            }
        }

        /// <summary>
        /// Tests whether the segment is vertical.
        /// </summary>
        /// <returns><c>true</c> if the segment is vertical.</returns>
        public virtual bool IsVertical
        {
            get
            {
                return P0.X == P1.X;
            }
        }

        /// <summary>
        /// Determines the orientation of a LineSegment relative to this segment.
        /// The concept of orientation is specified as follows:
        /// Given two line segments A and L,
        /// A is to the left of a segment L if A lies wholly in the
        /// closed half-plane lying to the left of L
        /// A is to the right of a segment L if A lies wholly in the
        /// closed half-plane lying to the right of L
        /// otherwise, A has indeterminate orientation relative to L. This
        /// happens if A is collinear with L or if A crosses the line determined by L.
        /// </summary>
        /// <param name="seg">The <c>LineSegment</c> to compare.</param>
        /// <returns>
        /// 1 if <c>seg</c> is to the left of this segment,
        /// -1 if <c>seg</c> is to the right of this segment,
        /// 0 if <c>seg</c> has indeterminate orientation relative to this segment.
        /// </returns>
        public virtual int OrientationIndex(ILineSegmentBase seg)
        {
            int orient0 = CgAlgorithms.OrientationIndex(P0, P1, seg.P0);
            int orient1 = CgAlgorithms.OrientationIndex(P0, P1, seg.P1);
            // this handles the case where the points are Curve or collinear
            if (orient0 >= 0 && orient1 >= 0)
                return Math.Max(orient0, orient1);
            // this handles the case where the points are R or collinear
            if (orient0 <= 0 && orient1 <= 0)
                return Math.Max(orient0, orient1);
            // points lie on opposite sides ==> indeterminate orientation
            return 0;
        }

        /// <summary>
        /// Reverses the direction of the line segment.
        /// </summary>
        public virtual void Reverse()
        {
            Coordinate temp = P0;
            P0 = P1;
            P1 = temp;
        }

        /// <summary>
        /// Puts the line segment into a normalized form.
        /// This is useful for using line segments in maps and indexes when
        /// topological equality rather than exact equality is desired.
        /// </summary>
        public virtual void Normalize()
        {
            if (new Coordinate(P1).CompareTo(P0) < 0)
                Reverse();
        }

        /// <returns>
        /// The angle this segment makes with the x-axis (in radians).
        /// </returns>
        public virtual double Angle
        {
            get
            {
                return Math.Atan2(P1.Y - P0.Y, P1.X - P0.X);
            }
        }

        /// <summary>
        /// Computes the distance between this line segment and another one.
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public virtual double Distance(ILineSegmentBase ls)
        {
            return CgAlgorithms.DistanceLineLine(P0, P1, new Coordinate(ls.P0), new Coordinate(ls.P1));
        }

        /// <summary>
        /// Computes the distance between this line segment and a point.
        /// </summary>
        public virtual double Distance(Coordinate p)
        {
            return CgAlgorithms.DistancePointLine(new Coordinate(p), P0, P1);
        }

        /// <summary>
        /// Computes the perpendicular distance between the (infinite) line defined
        /// by this line segment and a point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual double DistancePerpendicular(Coordinate p)
        {
            return CgAlgorithms.DistancePointLinePerpendicular(new Coordinate(p), P0, P1);
        }

        /// <summary>
        /// Compute the projection factor for the projection of the point p
        /// onto this <c>LineSegment</c>. The projection factor is the constant k
        /// by which the vector for this segment must be multiplied to
        /// equal the vector for the projection of p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual double ProjectionFactor(Coordinate p)
        {
            if (p.Equals(P0)) return 0.0;
            if (p.Equals(P1)) return 1.0;

            // Otherwise, use comp.graphics.algorithms Frequently Asked Questions method
            /*     	          AC dot AB
                        r = ------------
                              ||AB||^2
                        r has the following meaning:
                        r=0 Point = A
                        r=1 Point = B
                        r<0 Point is on the backward extension of AB
                        r>1 Point is on the forward extension of AB
                        0<r<1 Point is interior to AB
            */
            double dx = P1.X - P0.X;
            double dy = P1.Y - P0.Y;
            double len2 = dx * dx + dy * dy;
            double r = ((p.X - P0.X) * dx + (p.Y - P0.Y) * dy) / len2;
            return r;
        }

        /// <summary>
        /// Compute the projection of a point onto the line determined
        /// by this line segment.
        /// Notice that the projected point
        /// may lie outside the line segment.  If this is the case,
        /// the projection factor will lie outside the range [0.0, 1.0].
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual Coordinate Project(Coordinate p)
        {
            if (p.Equals(P0) || p.Equals(P1))
                return new Coordinate(p);

            double r = ProjectionFactor(p);
            Coordinate coord = new Coordinate();
            coord.X = P0.X + r * (P1.X - P0.X);
            coord.Y = P0.Y + r * (P1.Y - P0.Y);
            return coord;
        }

        /// <summary>
        /// Project a line segment onto this line segment and return the resulting
        /// line segment.  The returned line segment will be a subset of
        /// the target line line segment.  This subset may be null, if
        /// the segments are oriented in such a way that there is no projection.
        /// Notice that the returned line may have zero length (i.e. the same endpoints).
        /// This can happen for instance if the lines are perpendicular to one another.
        /// </summary>
        /// <param name="seg">The line segment to project.</param>
        /// <returns>The projected line segment, or <c>null</c> if there is no overlap.</returns>
        public virtual ILineSegment Project(ILineSegmentBase seg)
        {
            double pf0 = ProjectionFactor(seg.P0);
            double pf1 = ProjectionFactor(seg.P1);
            // check if segment projects at all
            if (pf0 >= 1.0 && pf1 >= 1.0) return null;
            if (pf0 <= 0.0 && pf1 <= 0.0) return null;

            Coordinate newp0 = Project(seg.P0);
            if (pf0 < 0.0) newp0 = P0;
            if (pf0 > 1.0) newp0 = P1;

            Coordinate newp1 = Project(seg.P1);
            if (pf1 < 0.0) newp1 = P0;
            if (pf1 > 1.0) newp1 = P1;

            return new LineSegment(newp0, newp1);
        }

        /// <summary>
        /// Computes the closest point on this line segment to another point.
        /// </summary>
        /// <param name="p">The point to find the closest point to.</param>
        /// <returns>
        /// A Coordinate which is the closest point on the line segment to the point p.
        /// </returns>
        public virtual Coordinate ClosestPoint(Coordinate p)
        {
            double factor = ProjectionFactor(p);
            if (factor > 0 && factor < 1)
                return Project(p);
            double dist0 = new Coordinate(_p0).Distance(p);
            double dist1 = new Coordinate(P1).Distance(p);
            if (dist0 < dist1)
                return new Coordinate(_p0);
            return new Coordinate(_p1);
        }

        /// <summary>
        /// Computes the closest points on a line segment.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// A pair of Coordinates which are the closest points on the line segments.
        /// </returns>
        public virtual Coordinate[] ClosestPoints(ILineSegmentBase line)
        {
            LineSegment myLine = new LineSegment(line);

            // test for intersection
            Coordinate intPt = Intersection(line);

            if (intPt != null)
                return new[] { intPt, intPt };

            /*
            *  if no intersection closest pair contains at least one endpoint.
            * Test each endpoint in turn.
            */
            Coordinate[] closestPt = new Coordinate[2];

            Coordinate close00 = new Coordinate(ClosestPoint(line.P0));
            double minDistance = close00.Distance(line.P0);
            closestPt[0] = close00;
            closestPt[1] = new Coordinate(line.P0);

            Coordinate close01 = new Coordinate(ClosestPoint(line.P1));
            double dist = close01.Distance(line.P1);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestPt[0] = close01;
                closestPt[1] = new Coordinate(line.P1);
            }

            Coordinate close10 = new Coordinate(myLine.ClosestPoint(P0));
            dist = close10.Distance(P0);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestPt[0] = new Coordinate(P0);
                closestPt[1] = close10;
            }

            Coordinate close11 = new Coordinate(myLine.ClosestPoint(P1));
            dist = close11.Distance(P1);
            if (dist < minDistance)
            {
                closestPt[0] = new Coordinate(P1);
                closestPt[1] = close11;
            }

            return closestPt;
        }

        /// <summary>
        /// Computes an intersection point between two segments, if there is one.
        /// There may be 0, 1 or many intersection points between two segments.
        /// If there are 0, null is returned. If there is 1 or more, a single one
        /// is returned (chosen at the discretion of the algorithm).  If
        /// more information is required about the details of the intersection,
        /// the {RobustLineIntersector} class should be used.
        /// </summary>
        /// <param name="line"></param>
        /// <returns> An intersection point, or <c>null</c> if there is none.</returns>
        public virtual Coordinate Intersection(ILineSegmentBase line)
        {
            LineIntersector li = new RobustLineIntersector();
            li.ComputeIntersection(P0, P1, new Coordinate(line.P0), new Coordinate(line.P1));
            if (li.HasIntersection)
                return li.GetIntersection(0);
            return null;
        }

        /// <summary>
        /// Performs an intersection of this line segment with the specified envelope
        /// </summary>
        /// <param name="inEnvelope">The envelope to compare against</param>
        /// <returns>An ILineSegment, or null if there is no intersection.</returns>
        public ILineSegment Intersection(Envelope inEnvelope)
        {
            return inEnvelope.Intersection(this);
        }

        /// <summary>
        /// Determines if any portion of this segment intersects the specified extent.
        /// </summary>
        /// <param name="inEnvelope">The</param>
        /// <returns>Boolean, true if this line segment intersects the specified envelope</returns>
        public bool Intersects(Envelope inEnvelope)
        {
            return inEnvelope.Intersects(this);
        }

        /// <summary>
        /// Compares this object with the specified object for order.
        /// Uses the standard lexicographic ordering for the points in the LineSegment.
        /// </summary>
        /// <param name="o">
        /// The <c>LineSegment</c> with which this <c>LineSegment</c>
        /// is being compared.
        /// </param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <c>LineSegment</c>
        /// is less than, equal to, or greater than the specified <c>LineSegment</c>.
        /// </returns>
        public virtual int CompareTo(object o)
        {
            ILineSegmentBase other = (ILineSegmentBase)o;

            int comp0 = new Coordinate(_p0).CompareTo(other.P0);
            if (comp0 != 0) return comp0;
            return new Coordinate(_p1).CompareTo(other.P1);
        }

        /// <summary>
        /// Returns <c>true</c> if <c>other</c> is
        /// topologically equal to this LineSegment (e.g. irrespective
        /// of orientation).
        /// </summary>
        /// <param name="other">
        /// A <c>LineSegment</c> with which to do the comparison.
        /// </param>
        /// <returns>
        /// <c>true</c> if <c>other</c> is a <c>LineSegment</c>
        /// with the same values for the x and y ordinates.
        /// </returns>
        public virtual bool EqualsTopologically(ILineSegmentBase other)
        {
            return
                (new Coordinate(_p0).Equals(other.P0) && new Coordinate(_p1).Equals(other.P1)) ||
                (new Coordinate(_p0).Equals(other.P1) && new Coordinate(_p1).Equals(other.P0));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("LINESTRING( ");
            sb.Append(P0.X).Append(" ");
            sb.Append(P0.Y).Append(", ");
            sb.Append(P1.X).Append(" ");
            sb.Append(P1.Y).Append(")");
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Returns <c>true</c> if <c>o</c> has the same values for its points.
        /// </summary>
        /// <param name="o">A <c>LineSegment</c> with which to do the comparison.</param>
        /// <returns>
        /// <c>true</c> if <c>o</c> is a <c>LineSegment</c>
        /// with the same values for the x and y ordinates.
        /// </returns>
        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            if (!(o is ILineSegmentBase))
                return false;
            ILineSegmentBase other = (ILineSegmentBase)o;
            return (_p0.X == other.P0.X && _p0.Y == other.P0.Y && _p1.X == other.P1.X && _p1.Y == other.P1.Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(LineSegment obj1, ILineSegmentBase obj2)
        {
            return Equals(obj1, obj2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(LineSegment obj1, ILineSegmentBase obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Return HashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}