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
    /// This is a low-level place holder for a linestring with only two points.
    /// This does not inherit geometry (Use ILineString for those features).
    /// The Idea is that this provides just enough information to communicate
    /// the definition of a LineSegment.
    /// </summary>
    public interface ILineSegmentBase
    {
        /// <summary>
        /// The first of two coordinates that defines the segment
        /// </summary>
        Coordinate P1
        {
            get;
            set;
        }

        /// <summary>
        /// The second of two endpoints that defines the segment
        /// </summary>
        Coordinate P0
        {
            get;
            set;
        }
    }
}