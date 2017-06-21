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
    /// An interface for supporting the functions specific to geometry overlay opperations
    /// </summary>
    public interface IOverlay
    {
        /// Difference
        /// <summary>
        /// Returns a <c>Geometry</c> representing the points making up this
        /// <c>Geometry</c> that do not make up <c>other</c>. This method
        /// returns the closure of the resultant <c>Geometry</c>.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compute the difference.</param>
        /// <returns>The point set difference of this <c>Geometry</c> with <c>other</c>.</returns>
        IGeometry Difference(IGeometry geom);

        /// Intersection
        /// <summary>
        /// Returns a <c>Geometry</c> representing the points shared by this
        /// <c>Geometry</c> and <c>other</c>.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compute the intersection.</param>
        /// <returns>The points common to the two <c>Geometry</c>s.</returns>
        IGeometry Intersection(IGeometry geom);

        /// SymmetricDifference
        /// <summary>
        /// Returns a set combining the points in this <c>Geometry</c> not in
        /// <c>other</c>, and the points in <c>other</c> not in this
        /// <c>Geometry</c>. This method returns the closure of the resultant
        /// <c>Geometry</c>.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compute the symmetric difference.</param>
        /// <returns>The point set symmetric difference of this <c>Geometry</c> with <c>other</c>.</returns>
        IGeometry SymmetricDifference(IGeometry geom);

        /// Union
        /// <summary>
        /// Returns a <c>Geometry</c> representing all the points in this <c>Geometry</c>
        /// and <c>other</c>.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compute the union.</param>
        /// <returns>A set combining the points of this <c>Geometry</c> and the points of <c>other</c>.</returns>
        IGeometry Union(IGeometry geom);
    }
}