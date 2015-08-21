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
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Implementation;
using DotSpatial.Topology.IO;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Represents a list of contiguous line segments, and supports noding the segments.
    /// The line segments are represented by an array of <see cref="Coordinate" />s.
    /// Intended to optimize the noding of contiguous segments by
    /// reducing the number of allocated objects.
    /// <see cref="NodedSegmentString" />s can carry a context object, which is useful
    /// for preserving topological or parentage information.
    /// All noded substrings are initialized with the same context object.
    /// </summary>
    public class NodedSegmentString : INodableSegmentString
    {
        #region Constructors

        /// <summary>
        /// Creates a new segment string from a list of vertices.
        /// </summary>
        /// <param name="pts">The vertices of the segment string.</param>
        /// <param name="data">The user-defined data of this segment string (may be null).</param>
        public NodedSegmentString(IList<Coordinate> pts, object data)
        {
            NodeList = new SegmentNodeList(this);

            Coordinates = pts;
            Context = data;
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Count
        {
            get { return Coordinates.Count; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed
        {
            get{return Coordinates[0].Equals2D(Coordinates[Coordinates.Count - 1]);}
        }

        /// <summary>
        /// Gets/Sets the user-defined data for this segment string.
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IList<Coordinate> Coordinates { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public SegmentNodeList NodeList { get; private set; }

        #endregion

        #region Indexers

        public LineSegment this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, TopologyText.NodedSegmentString_WrongParameterSize);
                return new LineSegment(Coordinates[index], Coordinates[index + 1]);
            }
            set
            {
                throw new NotSupportedException(TopologyText.NodedSegmentString_UnsupportedSettingLinesegments);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an <see cref="SegmentNode" /> for intersection intIndex.
        /// An intersection that falls exactly on a vertex
        /// of the <see cref="NodedSegmentString" /> is normalized
        /// to use the higher of the two possible segmentIndexes.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="geomIndex"></param>
        /// <param name="intIndex"></param>
        public void AddIntersection(LineIntersector li, int segmentIndex, int geomIndex, int intIndex)
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
            if (nextSegIndex < Coordinates.Count)
            {
                Coordinate nextPt = Coordinates[nextSegIndex];

                // Normalize segment index if intPt falls on vertex
                // The check for point equality is 2D only - Z values are ignored
                if (intPt.Equals2D(nextPt))
                    normalizedSegmentIndex = nextSegIndex;
            }

            // Add the intersection point to edge intersection list.
            NodeList.Add(intPt, normalizedSegmentIndex);
        }

        /// <summary>
        /// Adds EdgeIntersections for one or both
        /// intersections found for a segment of an edge to the edge intersection list.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="geomIndex"></param>
        public void AddIntersections(LineIntersector li, int segmentIndex, int geomIndex)
        {
            for (int i = 0; i < li.IntersectionNum; i++)
                AddIntersection(li, segmentIndex, geomIndex, i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Coordinate GetCoordinate(int i)
        {
            return Coordinates[i];
        }

        /// <summary>
        /// Gets the <see cref="ISegmentString"/>s which result from splitting this string at node points.
        /// </summary>
        /// <param name="segStrings">A collection of NodedSegmentStrings</param>
        /// <returns>A collection of NodedSegmentStrings representing the substrings</returns>
        public static IList<ISegmentString> GetNodedSubstrings(IList<ISegmentString> segStrings)
        {
            IList<ISegmentString> resultEdgelist = new List<ISegmentString>();
            GetNodedSubstrings(segStrings, resultEdgelist);
            return resultEdgelist;
        }

        /// <summary>
        /// Adds the noded <see cref="ISegmentString"/>s which result from splitting this string at node points.
        /// </summary>
        /// <param name="segStrings">A collection of NodedSegmentStrings</param>
        /// <param name="resultEdgelist">A list which will collect the NodedSegmentStrings representing the substrings</param>
        public static void GetNodedSubstrings(IList<ISegmentString> segStrings, IList<ISegmentString> resultEdgelist)
        {
            foreach (NodedSegmentString ss in segStrings.Cast<NodedSegmentString>())
                ss.NodeList.AddSplitEdges(resultEdgelist);
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
            return index == Coordinates.Count - 1 ? OctantDirection.Null : SafeOctant(GetCoordinate(index), GetCoordinate(index + 1));
        }

        public override string ToString()
        {
            return WKTWriter.ToLineString(new CoordinateArraySequence(Coordinates));
        }

        private static OctantDirection SafeOctant(Coordinate p0, Coordinate p1)
        {
            return p0.Equals2D(p1) ? OctantDirection.Zero : Octant.GetOctant(p0, p1);
        }

        #endregion
    }
}