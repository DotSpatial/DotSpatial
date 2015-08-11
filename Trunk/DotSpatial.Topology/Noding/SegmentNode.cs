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
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Represents an intersection point between two <see cref="ISegmentString" />s.
    /// </summary>
    public class SegmentNode : IComparable
    {
        #region Fields

        /// <summary>
        /// the index of the containing line segment in the parent edge
        /// </summary>
        public readonly int SegmentIndex;

        private readonly OctantDirection _segmentOctant;
        private readonly INodableSegmentString _segString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentNode"/> class.
        /// </summary>
        /// <param name="segString"></param>
        /// <param name="coord"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="segmentOctant"></param>
        public SegmentNode(INodableSegmentString segString, Coordinate coord, int segmentIndex, OctantDirection segmentOctant)
        {
            Coordinate = null;
            _segString = segString;
            Coordinate = new Coordinate(coord.X, coord.Y, coord.Z);
            SegmentIndex = segmentIndex;
            _segmentOctant = segmentOctant;
            IsInterior = !coord.Equals2D(segString.Coordinates[segmentIndex]);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="Coordinate"/> giving the location of this node.
        /// </summary>
        public Coordinate Coordinate { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInterior { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// -1 this SegmentNode is located before the argument location;<br/>
        ///  0 this SegmentNode is at the argument location;<br/>
        ///  1 this SegmentNode is located after the argument location.   
        /// </returns>
        public int CompareTo(object obj)
        {
            var other = (SegmentNode)obj;
            if (SegmentIndex < other.SegmentIndex) return -1;
            if (SegmentIndex > other.SegmentIndex) return 1;
            if (Coordinate.Equals2D(other.Coordinate)) return 0;
            return SegmentPointComparator.Compare(_segmentOctant, Coordinate, other.Coordinate);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxSegmentIndex"></param>
        /// <returns></returns>
        public bool IsEndPoint(int maxSegmentIndex)
        {
            if (SegmentIndex == 0 && !IsInterior) return true;
            return SegmentIndex == maxSegmentIndex;
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

        #endregion
    }
}