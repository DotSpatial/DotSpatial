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

using System.Collections;
using System.IO;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A list of edge intersections along an Edge.
    /// </summary>
    public class EdgeIntersectionList
    {
        // a list of EdgeIntersections
        private readonly Edge _edge;  // the parent edge
        private readonly IDictionary _nodeMap = new SortedList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        public EdgeIntersectionList(Edge edge)
        {
            _edge = edge;
        }

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get
            {
                return _nodeMap.Count;
            }
        }

        /// <summary>
        /// Adds an intersection into the list, if it isn't already there.
        /// The input segmentIndex and dist are expected to be normalized.
        /// </summary>
        /// <param name="intPt"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="dist"></param>
        public virtual void Add(Coordinate intPt, int segmentIndex, double dist)
        {
            EdgeIntersection eiNew = new EdgeIntersection(intPt, segmentIndex, dist);
            EdgeIntersection ei = (EdgeIntersection)_nodeMap[eiNew];
            if (ei != null)
                return;
            _nodeMap.Add(eiNew, eiNew);
            return;
        }

        /// <summary>
        /// Returns an iterator of EdgeIntersections.
        /// </summary>
        public virtual IEnumerator GetEnumerator()
        {
            return _nodeMap.Values.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool IsIntersection(Coordinate pt)
        {
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeIntersection ei = (EdgeIntersection)it.Current;
                if (ei.Coordinate.Equals(pt))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Adds entries for the first and last points of the edge to the list.
        /// </summary>
        public virtual void AddEndpoints()
        {
            int maxSegIndex = _edge.Points.Count - 1;
            Add(_edge.Points[0], 0, 0.0);
            Add(_edge.Points[maxSegIndex], maxSegIndex, 0.0);
        }

        /// <summary>
        /// Creates new edges for all the edges that the intersections in this
        /// list split the parent edge into.
        /// Adds the edges to the input list (this is so a single list
        /// can be used to accumulate all split edges for a Geometry).
        /// </summary>
        /// <param name="edgeList"></param>
        public virtual void AddSplitEdges(IList edgeList)
        {
            // ensure that the list has entries for the first and last point of the edge
            AddEndpoints();

            IEnumerator it = GetEnumerator();
            it.MoveNext();
            // there should always be at least two entries in the list
            EdgeIntersection eiPrev = (EdgeIntersection)it.Current;
            while (it.MoveNext())
            {
                EdgeIntersection ei = (EdgeIntersection)it.Current;
                Edge newEdge = CreateSplitEdge(eiPrev, ei);
                edgeList.Add(newEdge);

                eiPrev = ei;
            }
        }

        /// <summary>
        /// Create a new "split edge" with the section of points between
        /// (and including) the two intersections.
        /// The label for the new edge is the same as the label for the parent edge.
        /// </summary>
        /// <param name="ei0"></param>
        /// <param name="ei1"></param>
        public virtual Edge CreateSplitEdge(EdgeIntersection ei0, EdgeIntersection ei1)
        {
            int npts = ei1.SegmentIndex - ei0.SegmentIndex + 2;
            Coordinate lastSegStartPt = _edge.Points[ei1.SegmentIndex];
            // if the last intersection point is not equal to the its segment start pt,
            // add it to the points list as well.
            // (This check is needed because the distance metric is not totally reliable!)
            // The check for point equality is 2D only - Z values are ignored
            bool useIntPt1 = ei1.Distance > 0.0 || !ei1.Coordinate.Equals2D(lastSegStartPt);
            if (!useIntPt1)
                npts--;

            Coordinate[] pts = new Coordinate[npts];
            int ipt = 0;
            pts[ipt++] = new Coordinate(ei0.Coordinate);
            for (int i = ei0.SegmentIndex + 1; i <= ei1.SegmentIndex; i++)
                pts[ipt++] = _edge.Points[i];

            if (useIntPt1) pts[ipt] = ei1.Coordinate;
            return new Edge(pts, new Label(_edge.Label));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            outstream.WriteLine("Intersections:");
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeIntersection ei = (EdgeIntersection)it.Current;
                ei.Write(outstream);
            }
        }
    }
}