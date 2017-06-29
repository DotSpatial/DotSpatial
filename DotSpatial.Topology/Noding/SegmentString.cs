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
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Represents a list of contiguous line segments, and supports noding the segments.
    /// The line segments are represented by an array of <see cref="Coordinate" />s.
    /// Intended to optimize the noding of contiguous segments by
    /// reducing the number of allocated objects.
    /// <see cref="SegmentString" />s can carry a context object, which is useful
    /// for preserving topological or parentage information.
    /// All noded substrings are initialized with the same context object.
    /// </summary>
    public class SegmentString
    {
        #region Private Variables

        private readonly SegmentNodeList _nodeList;
        private readonly IList<Coordinate> _pts;
        private object _data;

        #endregion

        #region Static

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        /// <returns></returns>
        public static IList GetNodedSubstrings(IList segStrings)
        {
            IList resultEdgelist = new ArrayList();
            GetNodedSubstrings(segStrings, resultEdgelist);
            return resultEdgelist;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="resultEdgelist"></param>
        public static void GetNodedSubstrings(IList segStrings, IList resultEdgelist)
        {
            foreach (object obj in segStrings)
            {
                SegmentString ss = (SegmentString)obj;
                ss.NodeList.AddSplitEdges(resultEdgelist);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new segment string from a list of vertices.
        /// </summary>
        /// <param name="pts">The vertices of the segment string.</param>
        /// <param name="data">The user-defined data of this segment string (may be null).</param>
        public SegmentString(IList<Coordinate> pts, Object data)
        {
            _nodeList = new SegmentNodeList(this);

            _pts = pts;
            _data = data;
        }

        /// <summary>
        /// Gets/Sets the user-defined data for this segment string.
        /// </summary>
        public object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public SegmentNodeList NodeList
        {
            get
            {
                return _nodeList;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Count
        {
            get
            {
                return _pts.Count;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IList<Coordinate> Coordinates
        {
            get
            {
                return _pts;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return _pts[0].Equals(_pts[_pts.Count - 1]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Coordinate GetCoordinate(int i)
        {
            return _pts[i];
        }

        /// <summary>
        ///  Gets the octant of the segment starting at vertex <c>index</c>.
        /// </summary>
        /// <param name="index">
        /// The index of the vertex starting the segment.
        /// Must not be the last index in the vertex list
        /// </param>
        /// <returns>The octant of the segment at the vertex</returns>
        public OctantDirection GetSegmentOctant(int index)
        {
            if (index == _pts.Count - 1)
                return OctantDirection.Null;
            return Octant.GetOctant(GetCoordinate(index), GetCoordinate(index + 1));
        }

        /// <summary>
        /// Adds EdgeIntersections for one or both
        /// intersections found for a segment of an edge to the edge intersection list.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        public void AddIntersections(LineIntersector li, int segmentIndex)
        {
            for (int i = 0; i < li.IntersectionNum; i++)
                AddIntersection(li, segmentIndex, i);
        }

        /// <summary>
        /// Add an <see cref="SegmentNode" /> for intersection intIndex.
        /// An intersection that falls exactly on a vertex
        /// of the <see cref="SegmentString" /> is normalized
        /// to use the higher of the two possible segmentIndexes.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="intIndex"></param>
        public void AddIntersection(LineIntersector li, int segmentIndex, int intIndex)
        {
            Coordinate intPt = new Coordinate(li.GetIntersection(intIndex));
            AddIntersection(intPt, segmentIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="intPt"></param>
        /// <param name="segmentIndex"></param>
        public void AddIntersection(Coordinate intPt, int segmentIndex)
        {
            int normalizedSegmentIndex = segmentIndex;
            // normalize the intersection point location
            int nextSegIndex = normalizedSegmentIndex + 1;
            if (nextSegIndex < _pts.Count)
            {
                Coordinate nextPt = _pts[nextSegIndex];

                // Normalize segment index if intPt falls on vertex
                // The check for point equality is 2D only - Z values are ignored
                if (intPt.Equals2D(nextPt))
                    normalizedSegmentIndex = nextSegIndex;
            }

            // Add the intersection point to edge intersection list.
            _nodeList.Add(intPt, normalizedSegmentIndex);
        }
    }
}