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
using System.IO;
using System.Text;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph.Index;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    ///
    /// </summary>
    public class Edge : GraphComponent
    {
        private readonly Depth _depth = new Depth();
        private readonly EdgeIntersectionList _eiList;
        private int _depthDelta;   // the change in area depth from the R to Curve side of this edge
        private Envelope _env;
        private bool _isIsolated = true;
        private MonotoneChainEdge _mce;
        private string _name;
        private IList<Coordinate> _pts;

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="label"></param>
        public Edge(IList<Coordinate> pts, Label label)
        {
            _eiList = new EdgeIntersectionList(this);

            _pts = pts;
            base.Label = label;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        public Edge(IList<Coordinate> pts) : this(pts, null) { }

        /// <summary>
        ///
        /// </summary>
        public virtual IList<Coordinate> Points
        {
            get
            {
                return _pts;
            }
            set
            {
                _pts = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int NumPoints
        {
            get
            {
                return _pts.Count;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList<Coordinate> Coordinates
        {
            get
            {
                return _pts;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override Coordinate Coordinate
        {
            get
            {
                return Points.Count > 0 ? Points[0] : Coordinate.Empty;
            }
            set
            {
                if (Points.Count > 0)
                {
                    Points[0] = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Envelope Envelope
        {
            get
            {
                // compute envelope lazily
                if (_env == null)
                {
                    _env = new Envelope();
                    for (int i = 0; i < Points.Count; i++)
                        _env.ExpandToInclude(Points[i]);
                }
                return _env;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Depth Depth
        {
            get
            {
                return _depth;
            }
        }

        /// <summary>
        /// The depthDelta is the change in depth as an edge is crossed from R to L.
        /// </summary>
        /// <returns>The change in depth as the edge is crossed from R to L.</returns>
        public virtual int DepthDelta
        {
            get
            {
                return _depthDelta;
            }
            set
            {
                _depthDelta = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int MaximumSegmentIndex
        {
            get
            {
                return Points.Count - 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual EdgeIntersectionList EdgeIntersectionList
        {
            get
            {
                return _eiList;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual MonotoneChainEdge MonotoneChainEdge
        {
            get
            {
                if (_mce == null)
                    _mce = new MonotoneChainEdge(this);
                return _mce;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsClosed
        {
            get
            {
                return Points[0].Equals(Points[Points.Count - 1]);
            }
        }

        /// <summary>
        /// An Edge is collapsed if it is an Area edge and it consists of
        /// two segments which are equal and opposite (eg a zero-width V).
        /// </summary>
        public virtual bool IsCollapsed
        {
            get
            {
                if (!Label.IsArea())
                    return false;
                if (Points.Count != 3)
                    return false;
                if (Points[0].Equals(Points[2]))
                    return true;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Edge CollapsedEdge
        {
            get
            {
                Coordinate[] newPts = new Coordinate[2];
                newPts[0] = Points[0];
                newPts[1] = Points[1];
                Edge newe = new Edge(newPts, Label.ToLineLabel(Label));
                return newe;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool Isolated
        {
            get
            {
                return _isIsolated;
            }
            set
            {
                _isIsolated = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsIsolated
        {
            get
            {
                return _isIsolated;
            }
        }

        /// <summary>
        /// Updates an IM from the label for an edge.
        /// Handles edges from both L and A geometries.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="im"></param>
        public static void UpdateIm(Label label, IntersectionMatrix im)
        {
            im.SetAtLeastIfValid(label.GetLocation(0, PositionType.On), label.GetLocation(1, PositionType.On), DimensionType.Curve);
            if (!label.IsArea()) return;
            im.SetAtLeastIfValid(label.GetLocation(0, PositionType.Left), label.GetLocation(1, PositionType.Left), DimensionType.Surface);
            im.SetAtLeastIfValid(label.GetLocation(0, PositionType.Right), label.GetLocation(1, PositionType.Right), DimensionType.Surface);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Coordinate GetCoordinate(int i)
        {
            return Points[i];
        }

        /// <summary>
        /// Adds EdgeIntersections for one or both
        /// intersections found for a segment of an edge to the edge intersection list.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="geomIndex"></param>
        public virtual void AddIntersections(LineIntersector li, int segmentIndex, int geomIndex)
        {
            for (int i = 0; i < li.IntersectionNum; i++)
                AddIntersection(li, segmentIndex, geomIndex, i);
        }

        /// <summary>
        /// Add an EdgeIntersection for intersection intIndex.
        /// An intersection that falls exactly on a vertex of the edge is normalized
        /// to use the higher of the two possible segmentIndexes.
        /// </summary>
        /// <param name="li"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="geomIndex"></param>
        /// <param name="intIndex"></param>
        public virtual void AddIntersection(LineIntersector li, int segmentIndex, int geomIndex, int intIndex)
        {
            Coordinate intPt = new Coordinate(li.GetIntersection(intIndex));
            int normalizedSegmentIndex = segmentIndex;
            double dist = li.GetEdgeDistance(geomIndex, intIndex);

            // normalize the intersection point location
            int nextSegIndex = normalizedSegmentIndex + 1;
            if (nextSegIndex < Points.Count)
            {
                Coordinate nextPt = Points[nextSegIndex];

                // Normalize segment index if intPt falls on vertex
                // The check for point equality is 2D only - Z values are ignored
                if (intPt.Equals2D(nextPt))
                {
                    normalizedSegmentIndex = nextSegIndex;
                    dist = 0.0;
                }
                // Add the intersection point to edge intersection list.
                EdgeIntersectionList.Add(intPt, normalizedSegmentIndex, dist);
            }
        }

        /// <summary>
        /// Update the IM with the contribution for this component.
        /// A component only contributes if it has a labelling for both parent geometries.
        /// </summary>
        /// <param name="im"></param>
        public override void ComputeIm(IntersectionMatrix im)
        {
            UpdateIm(Label, im);
        }

        /// <summary>
        /// Equals is defined to be:
        /// e1 equals e2
        /// iff
        /// the coordinates of e1 are the same or the reverse of the coordinates in e2.
        /// </summary>
        /// <param name="o"></param>
        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            if (!(o is Edge))
                return false;
            return Equals(o as Edge);
        }

        /// <summary>
        /// Equals is defined to be:
        /// e1 equals e2
        /// iff
        /// the coordinates of e1 are the same or the reverse of the coordinates in e2.
        /// </summary>
        /// <param name="e"></param>
        protected virtual bool Equals(Edge e)
        {
            if (Points.Count != e.Points.Count)
                return false;

            bool isEqualForward = true;
            bool isEqualReverse = true;
            int iRev = Points.Count;
            for (int i = 0; i < Points.Count; i++)
            {
                if (!Points[i].Equals2D(e.Points[i]))
                    isEqualForward = false;
                if (!Points[i].Equals2D(e.Points[--iRev]))
                    isEqualReverse = false;
                if (!isEqualForward && !isEqualReverse)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(Edge obj1, Edge obj2)
        {
            return Equals(obj1, obj2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(Edge obj1, Edge obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <returns>
        /// <c>true</c> if the coordinate sequences of the Edges are identical.
        /// </returns>
        /// <param name="e"></param>
        public virtual bool IsPointwiseEqual(Edge e)
        {
            if (Points.Count != e.Points.Count)
                return false;
            for (int i = 0; i < Points.Count; i++)
                if (!Points[i].Equals2D(e.Points[i]))
                    return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            outstream.Write("edge " + _name + ": ");
            outstream.Write("LINESTRING (");
            for (int i = 0; i < Points.Count; i++)
            {
                if (i > 0) outstream.Write(",");
                outstream.Write(Points[i].X + " " + Points[i].Y);
            }
            outstream.Write(")  " + Label + " " + _depthDelta);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void WriteReverse(StreamWriter outstream)
        {
            outstream.Write("edge " + _name + ": ");
            for (int i = Points.Count - 1; i >= 0; i--)
                outstream.Write(Points[i] + " ");
            outstream.WriteLine(String.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("edge " + _name + ": ");
            sb.Append("LINESTRING (");
            for (int i = 0; i < Points.Count; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(Points[i].X + " " + Points[i].Y);
            }
            sb.Append(")  " + Label + " " + _depthDelta);
            return sb.ToString();
        }
    }
}