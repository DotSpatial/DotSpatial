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
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    ///
    /// </summary>
    public abstract class EdgeRing
    {
        private readonly IList _edges = new ArrayList();  // the DirectedEdges making up this EdgeRing
        private readonly ArrayList _holes = new ArrayList(); // a list of EdgeRings which are holes in this EdgeRing
        private readonly IGeometryFactory _innerGeometryFactory;
        private readonly Label _label = new Label(LocationType.Null); // label stores the locations of each point on the face surrounded by this ring
        private readonly IList _pts = new ArrayList();
        private bool _isHole;
        private int _maxNodeDegree = -1;
        private ILinearRing _ring;  // the ring created for this EdgeRing
        private EdgeRing _shell;   // if non-null, the ring is a hole and this EdgeRing is its containing shell
        private DirectedEdge _startDe;

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="geometryFactory"></param>
        protected EdgeRing(DirectedEdge start, IGeometryFactory geometryFactory)
        {
            _innerGeometryFactory = geometryFactory;
            ComputePoints(start);
            ComputeRing();
        }

        /// <summary>
        /// Gets the inner geometry factory
        /// </summary>
        protected IGeometryFactory InnerGeometryFactory
        {
            get { return _innerGeometryFactory; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsIsolated
        {
            get
            {
                return (_label.GeometryCount == 1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsHole
        {
            get
            {
                return _isHole;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual ILinearRing LinearRing
        {
            get
            {
                return _ring;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Label Label
        {
            get
            {
                return _label;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsShell
        {
            get
            {
                return _shell == null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual EdgeRing Shell
        {
            get
            {
                return _shell;
            }
            set
            {
                _shell = value;
                if (value != null)
                    _shell.AddHole(this);
            }
        }

        /// <summary>
        /// Returns the list of DirectedEdges that make up this EdgeRing.
        /// </summary>
        public virtual IList Edges
        {
            get
            {
                return _edges;
            }
        }

        /// <summary>
        /// The directed edge which starts the list of edges for this EdgeRing.
        /// </summary>
        protected DirectedEdge StartDe
        {
            get { return _startDe; }
            set { _startDe = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int MaxNodeDegree
        {
            get
            {
                if (_maxNodeDegree < 0)
                    ComputeMaxNodeDegree();
                return _maxNodeDegree;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Coordinate GetCoordinate(int i)
        {
            return (Coordinate)_pts[i];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        public virtual void AddHole(EdgeRing ring)
        {
            _holes.Add(ring);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometryFactory"></param>
        /// <returns></returns>
        public virtual IPolygon ToPolygon(IGeometryFactory geometryFactory)
        {
            ILinearRing[] holeLr = new LinearRing[_holes.Count];
            for (int i = 0; i < _holes.Count; i++)
                holeLr[i] = ((EdgeRing)_holes[i]).LinearRing;
            IPolygon poly = geometryFactory.CreatePolygon(LinearRing, holeLr);
            return poly;
        }

        /// <summary>
        /// Compute a LinearRing from the point list previously collected.
        /// Test if the ring is a hole (i.e. if it is CCW) and set the hole flag
        /// accordingly.
        /// </summary>
        public void ComputeRing()
        {
            if (_ring != null)
                return;   // don't compute more than once
            Coordinate[] coord = new Coordinate[_pts.Count];
            for (int i = 0; i < _pts.Count; i++)
                coord[i] = (Coordinate)_pts[i];
            _ring = InnerGeometryFactory.CreateLinearRing(coord);
            _isHole = CgAlgorithms.IsCounterClockwise(_ring.Coordinates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public abstract DirectedEdge GetNext(DirectedEdge de);

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        /// <param name="er"></param>
        public abstract void SetEdgeRing(DirectedEdge de, EdgeRing er);

        /// <summary>
        /// Collect all the points from the DirectedEdges of this ring into a contiguous list.
        /// </summary>
        /// <param name="start"></param>
        protected void ComputePoints(DirectedEdge start)
        {
            StartDe = start;
            DirectedEdge de = start;
            bool isFirstEdge = true;
            do
            {
                Assert.IsTrue(de != null, "found null Directed Edge");
                if (de == null) continue;
                if (de.EdgeRing == this)
                    throw new TopologyException("Directed Edge visited twice during ring-building at " + de.Coordinate);

                _edges.Add(de);
                Label label = de.Label;

                Assert.IsTrue(label.IsArea());
                MergeLabel(label);
                AddPoints(de.Edge, de.IsForward, isFirstEdge);
                isFirstEdge = false;
                SetEdgeRing(de, this);
                de = GetNext(de);
            }
            while (de != StartDe);
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeMaxNodeDegree()
        {
            _maxNodeDegree = 0;
            DirectedEdge de = StartDe;
            do
            {
                Node node = de.Node;
                int degree = ((DirectedEdgeStar)node.Edges).GetOutgoingDegree(this);
                if (degree > _maxNodeDegree)
                    _maxNodeDegree = degree;
                de = GetNext(de);
            }
            while (de != StartDe);
            _maxNodeDegree *= 2;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SetInResult()
        {
            DirectedEdge de = StartDe;
            do
            {
                de.Edge.IsInResult = true;
                de = de.Next;
            }
            while (de != StartDe);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="deLabel"></param>
        protected virtual void MergeLabel(Label deLabel)
        {
            MergeLabel(deLabel, 0);
            MergeLabel(deLabel, 1);
        }

        /// <summary>
        /// Merge the RHS label from a DirectedEdge into the label for this EdgeRing.
        /// The DirectedEdge label may be null.  This is acceptable - it results
        /// from a node which is NOT an intersection node between the Geometries
        /// (e.g. the end node of a LinearRing).  In this case the DirectedEdge label
        /// does not contribute any information to the overall labelling, and is simply skipped.
        /// </summary>
        /// <param name="deLabel"></param>
        /// <param name="geomIndex"></param>
        protected virtual void MergeLabel(Label deLabel, int geomIndex)
        {
            LocationType loc = deLabel.GetLocation(geomIndex, PositionType.Right);
            // no information to be had from this label
            if (loc == LocationType.Null)
                return;
            // if there is no current RHS value, set it
            if (_label.GetLocation(geomIndex) == LocationType.Null)
            {
                _label.SetLocation(geomIndex, loc);
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="isForward"></param>
        /// <param name="isFirstEdge"></param>
        protected virtual void AddPoints(Edge edge, bool isForward, bool isFirstEdge)
        {
            IList<Coordinate> edgePts = edge.Coordinates;
            if (isForward)
            {
                int startIndex = 1;
                if (isFirstEdge)
                    startIndex = 0;
                for (int i = startIndex; i < edgePts.Count; i++)
                    _pts.Add(edgePts[i]);
            }
            else
            {
                // is backward
                int startIndex = edgePts.Count - 2;
                if (isFirstEdge)
                    startIndex = edgePts.Count - 1;
                for (int i = startIndex; i >= 0; i--)
                    _pts.Add(edgePts[i]);
            }
        }

        /// <summary>
        /// This method will cause the ring to be computed.
        /// It will also check any holes, if they have been assigned.
        /// </summary>
        /// <param name="p"></param>
        public virtual bool ContainsPoint(Coordinate p)
        {
            ILinearRing shell = LinearRing;
            IEnvelope env = shell.EnvelopeInternal;
            if (!env.Contains(p))
                return false;
            if (!CgAlgorithms.IsPointInRing(p, shell.Coordinates))
                return false;
            for (IEnumerator i = _holes.GetEnumerator(); i.MoveNext(); )
            {
                EdgeRing hole = (EdgeRing)i.Current;
                if (hole.ContainsPoint(p))
                    return false;
            }
            return true;
        }
    }
}