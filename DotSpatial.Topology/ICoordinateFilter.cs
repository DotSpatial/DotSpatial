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
// ********************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// <c>Geometry</c> classes support the concept of applying a
    /// coordinate filter to every coordinate in the <c>Geometry</c>. A
    /// coordinate filter can either record information about each coordinate or
    /// change the coordinate in some way. Coordinate filters implement the
    /// interface <c>ICoordinateFilter</c>.
    /// <c>ICoordinateFilter</c> is an example of the Gang-of-Four Visitor pattern.
    /// Coordinate filters can be
    /// used to implement such things as coordinate transformations, centroid and
    /// envelope computation, and many other functions.
    /// </summary>
    public interface ICoordinateFilter
    {
        /// <summary>
        /// Performs an operation with or on <c>coord</c>.
        /// </summary>
        /// <param name="coord"><c>Coordinate</c> to which the filter is applied.</param>
        void Filter(Coordinate coord);
    }
}