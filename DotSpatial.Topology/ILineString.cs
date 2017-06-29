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
    /// This adds the basic functionality of a
    /// </summary>
    public interface ILineString : IGeometry, IBasicLineString
    {
        /// <summary>
        /// Gets a topologically complete IPoint for the first coordinate
        /// </summary>
        IPoint StartPoint { get; }

        /// <summary>
        /// Gets a topologically complete IPoint for the last coordinate
        /// </summary>
        IPoint EndPoint { get; }

        /// <summary>
        /// If the first coordinate is the same as the final coordinate, then the
        /// linestring is closed.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// If the coordinates listed for the linestring are both closed and simple then
        /// they qualify as a linear ring.
        /// </summary>
        bool IsRing { get; }

        /// <summary>
        /// Returns the value of the angle between the <see cref="StartPoint" />
        /// and the <see cref="EndPoint" />.
        /// </summary>
        double Angle { get; }

        /// <summary>
        /// Retrieves a topologically complete IPoint for the n'th coordinate in the
        /// 0 based index of point values.
        /// </summary>
        /// <param name="n">Integer index specifying the point to retrieve</param>
        /// <returns>IPoint to retrieve</returns>
        IPoint GetPointN(int n);

        /// <summary>
        /// Returns an ILineString that has its coordinates completely reversed
        /// </summary>
        /// <returns></returns>
        ILineString Reverse();

        /// <summary>
        /// Returns true if the given point is a vertex of this <c>LineString</c>.
        /// </summary>
        /// <param name="pt">The <c>Coordinate</c> to check.</param>
        /// <returns><c>true</c> if <c>pt</c> is one of this <c>LineString</c>'s vertices.</returns>
        bool IsCoordinate(Coordinate pt);
    }
}