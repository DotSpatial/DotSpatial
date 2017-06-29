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

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    ///
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        ///  An indicator that a Location is <c>on</c> a GraphComponent (0)
        /// </summary>
        On = 0,

        /// <summary>
        /// An indicator that a Location is to the <c>left</c> of a GraphComponent (1)
        /// </summary>
        Left = 1,

        /// <summary>
        /// An indicator that a Location is to the <c>right</c> of a GraphComponent (2)
        /// </summary>
        Right = 2,

        /// <summary>
        /// An indicator that a Location is <c>is parallel to x-axis</c> of a GraphComponent (-1)
        /// /// </summary>
        Parallel = -1,
    }

    /// <summary>
    /// A Position indicates the position of a Location relative to a graph component
    /// (Node, Edge, or Area).
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Returns Positions.Left if the position is Positions.Right,
        /// Positions.Right if the position is Left, or the position
        /// otherwise.
        /// </summary>
        /// <param name="position"></param>
        public static PositionType Opposite(PositionType position)
        {
            if (position == PositionType.Left)
                return PositionType.Right;
            if (position == PositionType.Right)
                return PositionType.Left;
            return position;
        }
    }
}