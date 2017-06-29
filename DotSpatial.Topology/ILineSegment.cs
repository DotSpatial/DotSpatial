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
// ***************************************************************************************************

using System;

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
    public interface ILineSegment : IComparable, ILineSegmentBase
    {
        /// <summary>
        /// Computes the length of the line segment.
        /// </summary>
        /// <returns>The length of the line segment.</returns>
        double Length
        {
            get;
        }

        /// <summary>
        /// Tests whether the segment is horizontal.
        /// </summary>
        /// <returns><c>true</c> if the segment is horizontal.</returns>
        bool IsHorizontal
        {
            get;
        }

        /// <summary>
        /// Tests whether the segment is vertical.
        /// </summary>
        /// <returns><c>true</c> if the segment is vertical.</returns>
        bool IsVertical
        {
            get;
        }

        /// <returns>
        /// The angle this segment makes with the x-axis (in radians).
        /// </returns>
        double Angle
        {
            get;
        }

        /// <summary>
        /// Returns an ICoordinate for the point specified by index i.
        /// </summary>
        /// <param name="i">Integer point index.  0 returns the first point, 1 returns the second.</param>
        /// <returns>ICoordinate</returns>
        Coordinate GetCoordinate(int i);

        /// <summary>
        /// Sets the two coordinates to match the coordinates in the specified ILineSegment
        /// </summary>
        /// <param name="ls"></param>
        void SetCoordinates(ILineSegmentBase ls);

        /// <summary>
        /// Sets the two coordinates of this ILineString based on the ICoordinate
        /// values passed.
        /// </summary>
        /// <param name="p0">An ICoordinate that specifies the startpoint of the segment</param>
        /// <param name="p1">An ICoordinate that specifies the location of the endpoint of the segment</param>
        void SetCoordinates(Coordinate p0, Coordinate p1);

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
        int OrientationIndex(ILineSegmentBase seg);

        /// <summary>
        /// Reverses the direction of the line segment.
        /// </summary>
        void Reverse();

        /// <summary>
        /// Puts the line segment into a normalized form.
        /// This is useful for using line segments in maps and indexes when
        /// topological equality rather than exact equality is desired.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Computes the distance between this line segment and another one.
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        double Distance(ILineSegmentBase ls);

        /// <summary>
        /// Computes the distance between this line segment and a point.
        /// </summary>
        double Distance(Coordinate p);

        /// <summary>
        /// Computes the perpendicular distance between the (infinite) line defined
        /// by this line segment and a point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        double DistancePerpendicular(Coordinate p);

        /// <summary>
        /// Compute the projection factor for the projection of the point p
        /// onto this <c>LineSegment</c>. The projection factor is the constant k
        /// by which the vector for this segment must be multiplied to
        /// equal the vector for the projection of p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        double ProjectionFactor(Coordinate p);

        /// <summary>
        /// Compute the projection of a point onto the line determined
        /// by this line segment.
        /// Notice that the projected point
        /// may lie outside the line segment.  If this is the case,
        /// the projection factor will lie outside the range [0.0, 1.0].
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Coordinate Project(Coordinate p);

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
        ILineSegment Project(ILineSegmentBase seg);

        /// <summary>
        /// Computes the closest point on this line segment to another point.
        /// </summary>
        /// <param name="p">The point to find the closest point to.</param>
        /// <returns>
        /// A Coordinate which is the closest point on the line segment to the point p.
        /// </returns>
        Coordinate ClosestPoint(Coordinate p);

        /// <summary>
        /// Computes the closest points on a line segment.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// A pair of Coordinates which are the closest points on the line segments.
        /// </returns>
        Coordinate[] ClosestPoints(ILineSegmentBase line);

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
        Coordinate Intersection(ILineSegmentBase line);

        /// <summary>
        /// Performs an intersection of this line segment with the specified envelope
        /// </summary>
        /// <param name="inEnvelope">The envelope to compare against</param>
        /// <returns>An ILineSegment, or null if there is no intersection.</returns>
        ILineSegment Intersection(Envelope inEnvelope);

        /// <summary>
        /// Determines if any portion of this segment intersects the specified extent.
        /// </summary>
        /// <param name="inEnvelope">The</param>
        /// <returns>Boolean, true if this line segment intersects the specified envelope</returns>
        bool Intersects(Envelope inEnvelope);

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
        bool EqualsTopologically(ILineSegmentBase other);

        /// <summary>
        /// Returns Well Known Text for a LineString with just 2 points
        /// </summary>
        /// <returns>String: Well Known Text</returns>
        string ToString();
    }
}