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

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// An EdgeIntersection represents a point on an
    /// edge which intersects with another edge.
    /// The intersection may either be a single point, or a line segment
    /// (in which case this point is the start of the line segment)
    /// The label attached to this intersection point applies to
    /// the edge from this point forwards, until the next
    /// intersection or the end of the edge.
    /// The intersection point must be precise.
    /// </summary>
    public class EdgeIntersection : IComparable
    {
        private Coordinate _coordinate;
        private double _dist;

        private int _segmentIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="dist"></param>
        public EdgeIntersection(Coordinate coord, int segmentIndex, double dist)
        {
            _coordinate = new Coordinate(coord);
            _segmentIndex = segmentIndex;
            _dist = dist;
        }

        /// <summary>
        /// The point of intersection.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
            set
            {
                _coordinate = value;
            }
        }

        /// <summary>
        /// The index of the containing line segment in the parent edge.
        /// </summary>
        public virtual int SegmentIndex
        {
            get
            {
                return _segmentIndex;
            }
            set
            {
                _segmentIndex = value;
            }
        }

        /// <summary>
        /// The edge distance of this point along the containing line segment.
        /// </summary>
        public virtual double Distance
        {
            get
            {
                return _dist;
            }
            set
            {
                _dist = value;
            }
        }

        #region IComparable Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            EdgeIntersection other = (EdgeIntersection)obj;
            return Compare(other.SegmentIndex, other.Distance);
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="segmentIndex"></param>
        /// <param name="dist"></param>
        /// <returns>
        /// -1 this EdgeIntersection is located before the argument location,
        /// 0 this EdgeIntersection is at the argument location,
        /// 1 this EdgeIntersection is located after the argument location.
        /// </returns>
        public virtual int Compare(int segmentIndex, double dist)
        {
            if (SegmentIndex < segmentIndex)
                return -1;
            if (SegmentIndex > segmentIndex)
                return 1;
            if (Distance < dist)
                return -1;
            return Distance > dist ? 1 : 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxSegmentIndex"></param>
        /// <returns></returns>
        public virtual bool IsEndPoint(int maxSegmentIndex)
        {
            if (SegmentIndex == 0 && Distance == 0.0)
                return true;
            if (SegmentIndex == maxSegmentIndex)
                return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            outstream.Write(Coordinate);
            outstream.Write(" seg # = " + SegmentIndex);
            outstream.WriteLine(" dist = " + Distance);
        }
    }
}