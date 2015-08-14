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

using System.Collections.Generic;
using System.IO;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A DirectedEdgeStar is an ordered list of outgoing DirectedEdges around a node.
    /// It supports labelling the edges as well as linking the edges to form both
    /// MaximalEdgeRings and MinimalEdgeRings.
    /// </summary>
    public class DirectedEdgeStar : EdgeEndStar
    {
        #region Constant Fields

        private const int LinkingToOutgoing = 2;
        private const int ScanningForIncoming = 1;

        #endregion

        #region Fields

        private Label _label;

        /// <summary>
        /// A list of all outgoing edges in the result, in CCW order.
        /// </summary>
        private IList<DirectedEdge> _resultAreaEdgeList;

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        public virtual void ComputeDepths(DirectedEdge de)
        {
            int edgeIndex = FindIndex(de);
            int startDepth = de.GetDepth(PositionType.Left);
            int targetLastDepth = de.GetDepth(PositionType.Right);
            // compute the depths from this edge up to the end of the edge array
            int nextDepth = ComputeDepths(edgeIndex + 1, EdgeList.Count, startDepth);
            // compute the depths for the initial part of the array
            int lastDepth = ComputeDepths(0, edgeIndex, nextDepth);
            if (lastDepth != targetLastDepth)
                throw new TopologyException("depth mismatch at " + de.Coordinate);
        }

        /// <summary>
        /// Compute the DirectedEdge depths for a subsequence of the edge array.
        /// </summary>
        /// <returns>The last depth assigned (from the R side of the last edge visited).</returns>
        private int ComputeDepths(int startIndex, int endIndex, int startDepth)
        {
            int currDepth = startDepth;
            for (int i = startIndex; i < endIndex; i++)
            {
                DirectedEdge nextDe = (DirectedEdge)EdgeList[i];            
                nextDe.SetEdgeDepths(PositionType.Right, currDepth);
                currDepth = nextDe.GetDepth(PositionType.Left);
            }
            return currDepth;
        }

        /// <summary>
        /// Compute the labelling for all dirEdges in this star, as well
        /// as the overall labelling.
        /// </summary>
        /// <param name="geom"></param>
        public override void ComputeLabelling(GeometryGraph[] geom)
        {
            base.ComputeLabelling(geom);

            // determine the overall labelling for this DirectedEdgeStar
            // (i.e. for the node it is based at)
            _label = new Label(LocationType.Null);
            IEnumerator<EdgeEnd> it = GetEnumerator();
            while (it.MoveNext())
            {
                EdgeEnd ee = it.Current;
                Edge e = ee.Edge;
                Label eLabel = e.Label;
                for (int i = 0; i < 2; i++)
                {
                    LocationType eLoc = eLabel.GetLocation(i);
                    if (eLoc == LocationType.Interior || eLoc == LocationType.Boundary)
                        _label.SetLocation(i, LocationType.Interior);
                }
            }
        }

        /// <summary>
        /// Traverse the star of edges, maintaing the current location in the result
        /// area at this node (if any).
        /// If any L edges are found in the interior of the result, mark them as covered.
        /// </summary>
        public virtual void FindCoveredLineEdges()
        {
            // Since edges are stored in CCW order around the node,
            // as we move around the ring we move from the right to the left side of the edge

            /*
            * Find first DirectedEdge of result area (if any).
            * The interior of the result is on the RHS of the edge,
            * so the start location will be:
            * - Interior if the edge is outgoing
            * - Exterior if the edge is incoming
            */
            LocationType startLoc = LocationType.Null;
            foreach (DirectedEdge nextOut in Edges)
            {
                DirectedEdge nextIn   = nextOut.Sym;
                if (!nextOut.IsLineEdge) 
                {
                    if (nextOut.IsInResult)
                    {
                        startLoc = LocationType.Interior;
                        break;
                    }
                    if (nextIn.IsInResult)
                    {
                        startLoc = LocationType.Exterior;
                        break;
                    }
                }
            }
            // no A edges found, so can't determine if Curve edges are covered or not
            if (startLoc == LocationType.Null)
                return;

            /*
            * move around ring, keeping track of the current location
            * (Interior or Exterior) for the result area.
            * If Curve edges are found, mark them as covered if they are in the interior
            */
            LocationType currLoc = startLoc;
            foreach (DirectedEdge nextOut in Edges)
            {
                DirectedEdge nextIn   = nextOut.Sym;
                if (nextOut.IsLineEdge)
                    nextOut.Edge.IsCovered  = (currLoc == LocationType.Interior);
                else
                {
                    // edge is an Area edge
                    if (nextOut.IsInResult)
                        currLoc = LocationType.Exterior;
                    if (nextIn.IsInResult)
                        currLoc = LocationType.Interior;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetOutgoingDegree()
        {
            int degree = 0;
            foreach (DirectedEdge de in Edges)
                if (de.IsInResult) 
                    degree++;
            return degree;         
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public virtual int GetOutgoingDegree(EdgeRing er)
        {
            int degree = 0;
            foreach (DirectedEdge de in Edges)
                if (de.EdgeRing == er) 
                    degree++;
            return degree;
        }

        private IList<DirectedEdge> GetResultAreaEdges()
        {            
            if (_resultAreaEdgeList != null) 
                return _resultAreaEdgeList;
            _resultAreaEdgeList = new List<DirectedEdge>();
            foreach (DirectedEdge de in Edges)
            {
                if (de.IsInResult || de.Sym.IsInResult)
                    _resultAreaEdgeList.Add(de);
            }
            return _resultAreaEdgeList;         
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual DirectedEdge GetRightmostEdge()
        {
            IList<EdgeEnd> edges = Edges;
            int size = edges.Count;
            if (size < 1)
                return null;
            DirectedEdge de0 = (DirectedEdge)edges[0];
            if (size == 1)
                return de0;
            DirectedEdge deLast = (DirectedEdge)edges[size - 1];

            int quad0 = de0.Quadrant;
            int quad1 = deLast.Quadrant;
            if (QuadrantOp.IsNorthern(quad0) && QuadrantOp.IsNorthern(quad1))
                return de0;
            if (!QuadrantOp.IsNorthern(quad0) && !QuadrantOp.IsNorthern(quad1))
                return deLast;
            // edges are in different hemispheres - make sure we return one that is non-horizontal
            if (de0.Dy != 0)
                return de0;
            if (deLast.Dy != 0)
                return deLast;
            throw new TwoHorizontalEdgesException();
        }

        /// <summary>
        /// Insert a directed edge in the list.
        /// </summary>
        /// <param name="ee"></param>
        public override void Insert(EdgeEnd ee)
        {
            DirectedEdge de = (DirectedEdge)ee;
            InsertEdgeEnd(de, de);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void LinkAllDirectedEdges()
        {
            IList<EdgeEnd> temp = Edges;
            temp = null;    //Hack
            // find first area edge (if any) to start linking at
            DirectedEdge prevOut = null;
            DirectedEdge firstIn = null;
            // link edges in CW order
            for (int i = EdgeList.Count - 1; i >= 0; i--) 
            {
                DirectedEdge nextOut = (DirectedEdge)EdgeList[i];
                DirectedEdge nextIn = nextOut.Sym;
                if (firstIn == null)
                    firstIn = nextIn;
                if (prevOut != null)
                    nextIn.Next = prevOut;
                // record outgoing edge, in order to link the last incoming edge
                prevOut = nextOut;
            }
            firstIn.Next = prevOut;        
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="er"></param>
        public virtual void LinkMinimalDirectedEdges(EdgeRing er)
        {
            // find first area edge (if any) to start linking at
            DirectedEdge firstOut = null;
            DirectedEdge incoming = null;
            int state = ScanningForIncoming;
            // link edges in CW order
            for (int i = _resultAreaEdgeList.Count - 1; i >= 0; i--)
            {
                DirectedEdge nextOut = this._resultAreaEdgeList[i];
                DirectedEdge nextIn = nextOut.Sym;

                // record first outgoing edge, in order to link the last incoming edge
                if (firstOut == null && nextOut.EdgeRing == er)
                    firstOut = nextOut;

                switch (state)
                {
                    case ScanningForIncoming:
                        if (nextIn.EdgeRing != er)
                            continue;
                        incoming = nextIn;
                        state = LinkingToOutgoing;
                        break;
                    case LinkingToOutgoing:
                        if (nextOut.EdgeRing != er)
                            continue;
                        incoming.NextMin = nextOut;
                        state = ScanningForIncoming;
                        break;
                    default:
                        break;
                }
            }        
            if (state == LinkingToOutgoing) 
            {
                Assert.IsTrue(firstOut != null, "found null for first outgoing dirEdge");
                Assert.IsTrue(firstOut.EdgeRing == er, "unable to link last incoming dirEdge");
                incoming.NextMin = firstOut;
            }
        }

        /// <summary>
        /// Traverse the star of DirectedEdges, linking the included edges together.
        /// To link two dirEdges, the next pointer for an incoming dirEdge
        /// is set to the next outgoing edge.
        /// DirEdges are only linked if:
        /// they belong to an area (i.e. they have sides)
        /// they are marked as being in the result
        /// Edges are linked in CCW order (the order they are stored).
        /// This means that rings have their face on the Right
        /// (in other words, the topological location of the face is given by the RHS label of the DirectedEdge).
        /// PRECONDITION: No pair of dirEdges are both marked as being in the result.
        /// </summary>
        public virtual void LinkResultDirectedEdges()
        {
            // make sure edges are copied to resultAreaEdges list
            GetResultAreaEdges();
            // find first area edge (if any) to start linking at
            DirectedEdge firstOut = null;
            DirectedEdge incoming = null;
            int state = ScanningForIncoming;
            // link edges in CCW order
            for (int i = 0; i < _resultAreaEdgeList.Count; i++)
            {
                DirectedEdge nextOut = _resultAreaEdgeList[i];
                DirectedEdge nextIn = nextOut.Sym;

                // skip de's that we're not interested in
                if (!nextOut.Label.IsArea())
                    continue;

                // record first outgoing edge, in order to link the last incoming edge
                if (firstOut == null && nextOut.IsInResult)
                    firstOut = nextOut;

                switch (state)
                {
                    case ScanningForIncoming:
                        if (!nextIn.IsInResult)
                            continue;
                        incoming = nextIn;
                        state = LinkingToOutgoing;
                        break;
                    case LinkingToOutgoing:
                        if (!nextOut.IsInResult)
                            continue;
                        incoming.Next = nextOut;
                        state = ScanningForIncoming;
                        break;
                    default:
                        break;
                }
            }        
            if (state == LinkingToOutgoing) 
            {        
                if (firstOut == null)
                    throw new TopologyException("no outgoing dirEdge found", Coordinate);
                Assert.IsTrue(firstOut.IsInResult, "unable to link last incoming dirEdge");
                incoming.Next = firstOut;
            }
        }

        /// <summary>
        /// For each dirEdge in the star, merge the label .
        /// </summary>
        public virtual void MergeSymLabels()
        {
            foreach (DirectedEdge de in Edges)
            {
                Label label = de.Label;
                label.Merge(de.Sym.Label);
            }
        }

        /// <summary>
        /// Update incomplete dirEdge labels from the labelling for the node.
        /// </summary>
        /// <param name="nodeLabel"></param>
        public virtual void UpdateLabelling(Label nodeLabel)
        {
            foreach (DirectedEdge de in Edges)
            {
                Label label = de.Label;
                label.SetAllLocationsIfNull(0, nodeLabel.GetLocation(0));
                label.SetAllLocationsIfNull(1, nodeLabel.GetLocation(1));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public override void Write(StreamWriter outstream)
        {
            foreach (DirectedEdge de in Edges)
            {
                outstream.Write("out ");
                de.Write(outstream);
                outstream.WriteLine();
                outstream.Write("in ");
                de.Sym.Write(outstream);
                outstream.WriteLine();
            }
        }

        #endregion
    }
}