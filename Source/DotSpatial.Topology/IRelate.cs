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
namespace DotSpatial.Topology
{
    /// <summary>
    /// Supports the specific methods associated with relationships, usually
    /// returning a boolean style test if the relationship is true.
    /// </summary>
    public interface IRelate
    {
        /// Contains
        /// <summary>
        /// Returns true if other.within(this) returns true.
        /// </summary>
        /// <param name="geom">The Geometry with which to compare this Geometry.</param>
        /// <returns>true if this Geometry contains other.</returns>
        bool Contains(IGeometry geom);

        /// CoveredBy
        /// <summary>
        /// Returns <c>true</c> if this geometry is covered by the specified geometry.
        /// <para>
        /// The <c>CoveredBy</c> predicate has the following equivalent definitions:
        ///     - Every point of this geometry is a point of the other geometry.
        ///     - The DE-9IM Intersection Matrix for the two geometries is <c>T*F**F***</c> or <c>*TF**F***</c> or <c>**FT*F***</c> or <c>**F*TF***</c>.
        ///     - <c>g.Covers(this)</c> (<c>CoveredBy</c> is the inverse of <c>Covers</c>).
        /// </para>
        /// Notice the difference between <c>CoveredBy</c> and <c>Within</c>: <c>CoveredBy</c> is a more inclusive relation.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c></param>.
        /// <returns><c>true</c> if this <c>Geometry</c> is covered by <paramref name="geom" />.</returns>
        bool CoveredBy(IGeometry geom);

        /// Covers
        /// <summary>
        /// Returns <c>true</c> if this geometry covers the specified geometry.
        /// <para>
        /// The <c>Covers</c> predicate has the following equivalent definitions:
        ///     - Every point of the other geometry is a point of this geometry.
        ///     - The DE-9IM Intersection Matrix for the two geometries is <c>T*****FF*</c> or <c>*T****FF*</c> or <c>***T**FF*</c> or <c>****T*FF*</c>.
        ///     - <c>g.CoveredBy(this)</c> (<c>Covers</c> is the inverse of <c>CoveredBy</c>).
        /// </para>
        /// Notice the difference between <c>Covers</c> and <c>Contains</c>: <c>Covers</c> is a more inclusive relation.
        /// In particular, unlike <c>Contains</c> it does not distinguish between
        /// points in the boundary and in the interior of geometries.
        /// </summary>
        /// <remarks>
        /// For most situations, <c>Covers</c> should be used in preference to <c>Contains</c>.
        /// As an added benefit, <c>Covers</c> is more amenable to optimization, and hence should be more performant.
        /// </remarks>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c></param>
        /// <returns><c>true</c> if this <c>Geometry</c> covers <paramref name="geom" /></returns>
        bool Covers(IGeometry geom);

        /// Crosses
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is
        ///  T*T****** (for a point and a curve, a point and an area or a line
        /// and an area) 0******** (for two curves).
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>s cross.
        /// For this function to return <c>true</c>, the <c>Geometry</c>
        /// s must be a point and a curve; a point and a surface; two curves; or a
        /// curve and a surface.
        /// </returns>
        bool Crosses(IGeometry geom);

        /// Disjoint
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is  DE-9IM: FF*FF****.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns><c>true</c> if the two <c>Geometry</c>s are disjoint.</returns>
        bool Disjoint(IGeometry geom);

        /// Intersects
        /// <summary>
        /// Returns <c>true</c> if <c>disjoint</c> returns false.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns><c>true</c> if the two <c>Geometry</c>s intersect.</returns>
        bool Intersects(IGeometry geom);

        /// Overlaps
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is
        ///  T*T***T** (for two points or two surfaces)
        ///  1*T***T** (for two curves).
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>s overlap.
        /// For this function to return <c>true</c>, the <c>Geometry</c>
        /// s must be two points, two curves or two surfaces.
        /// </returns>
        bool Overlaps(IGeometry geom);

        /// Relate
        /// <summary>
        /// Returns <c>true</c> if the elements in the DE-9IM intersection
        /// matrix for the two <c>Geometry</c>s match the elements in <c>intersectionPattern</c>
        ///, which may be:
        ///  0
        ///  1
        ///  2
        ///  T ( = 0, 1 or 2)
        ///  F ( = -1)
        ///  * ( = -1, 0, 1 or 2)
        /// For more information on the DE-9IM, see the OpenGIS Simple Features
        /// Specification.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <param name="intersectionPattern">The pattern against which to check the intersection matrix for the two <c>Geometry</c>s.</param>
        /// <returns><c>true</c> if the DE-9IM intersection matrix for the two <c>Geometry</c>s match <c>intersectionPattern</c>.</returns>
        bool Relate(IGeometry geom, string intersectionPattern);

        /// <summary>
        /// Returns the DE-9IM intersection matrix for the two <c>Geometry</c>s.
        /// </summary>
        /// <param name="g">The <c>Geometry</c> with which to compare this <c>Geometry</c></param>
        /// <returns>
        /// A matrix describing the intersections of the interiors,
        /// boundaries and exteriors of the two <c>Geometry</c>s.
        /// </returns>
        IIntersectionMatrix Relate(IGeometry g);

        /// Touches
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is FT*******, F**T***** or F***T****.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>s touch;
        /// Returns false if both <c>Geometry</c>s are points.
        /// </returns>
        bool Touches(IGeometry geom);

        /// Within
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is T*F**F***.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns><c>true</c> if this <c>Geometry</c> is within <c>other</c>.</returns>
        bool Within(IGeometry geom);
    }
}