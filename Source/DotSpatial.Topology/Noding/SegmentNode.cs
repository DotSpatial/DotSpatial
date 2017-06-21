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

using System;
using System.IO;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Represents an intersection point between two <see cref="SegmentString" />s.
    /// </summary>
    public class SegmentNode : IComparable
    {
        /// <summary>
        ///
        /// </summary>
        public readonly Coordinate Coordinate;   // the point of intersection

        /// <summary>
        ///
        /// </summary>
        public readonly int SegmentIndex;   // the index of the containing line segment in the parent edge

        private readonly bool _isInterior;
        private readonly OctantDirection _segmentOctant = OctantDirection.Null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentNode"/> class.
        /// </summary>
        /// <param name="segString"></param>
        /// <param name="coord"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="segmentOctant"></param>
        public SegmentNode(SegmentString segString, Coordinate coord, int segmentIndex, OctantDirection segmentOctant)
        {
            Coordinate = new Coordinate(coord);
            SegmentIndex = segmentIndex;
            _segmentOctant = segmentOctant;
            _isInterior = !coord.Equals2D(segString.GetCoordinate(segmentIndex));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsInterior
        {
            get
            {
                return _isInterior;
            }
        }

        #region IComparable Members

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// -1 this SegmentNode is located before the argument location, or
        ///  0 this SegmentNode is at the argument location, or
        ///  1 this SegmentNode is located after the argument location.
        /// </returns>
        public int CompareTo(object obj)
        {
            SegmentNode other = (SegmentNode)obj;
            if (SegmentIndex < other.SegmentIndex)
                return -1;
            if (SegmentIndex > other.SegmentIndex)
                return 1;
            if (Coordinate.Equals2D(other.Coordinate))
                return 0;
            return SegmentPointComparator.Compare(_segmentOctant, Coordinate, other.Coordinate);
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxSegmentIndex"></param>
        /// <returns></returns>
        public bool IsEndPoint(int maxSegmentIndex)
        {
            if (SegmentIndex == 0 && !_isInterior)
                return true;
            if (SegmentIndex == maxSegmentIndex)
                return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public void Write(StreamWriter outstream)
        {
            outstream.Write(Coordinate);
            outstream.Write(" seg # = " + SegmentIndex);
        }
    }
}