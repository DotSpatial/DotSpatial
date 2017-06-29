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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A EdgeEndStar is an ordered list of EdgeEnds around a node.
    /// They are maintained in CCW order (starting with the positive x-axis) around the node
    /// for efficient lookup and topology building.
    /// </summary>
    public abstract class EdgeEndStar
    {
        /// <summary>
        /// A map which maintains the edges in sorted order around the node.
        /// </summary>
        private readonly IDictionary _edgeMap = new SortedList();

        /// <summary>
        /// The location of the point for this star in Geometry i Areas.
        /// </summary>
        private readonly LocationType[] _ptInAreaLocation = { LocationType.Null, LocationType.Null };

        private IList _edgeList;

        /// <summary>
        /// Gets the EdgeMap
        /// </summary>
        protected IDictionary EdgeMap
        {
            get { return _edgeMap; }
        }

        /// <summary>
        /// A list of all outgoing edges in the result, in CCW order.
        /// </summary>
        protected IList EdgeList
        {
            get { return _edgeList; }
        }

        /// <returns>
        /// The coordinate for the node this star is based at.
        /// </returns>
        public virtual Coordinate Coordinate
        {
            get
            {
                IEnumerator it = GetEnumerator();
                if (!it.MoveNext())
                    return Coordinate.Empty;
                EdgeEnd e = (EdgeEnd)it.Current;
                return e.Coordinate;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Degree
        {
            get
            {
                return _edgeMap.Count;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList Edges
        {
            get
            {
                InitializeEdges();
                return _edgeList;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsAreaLabelsConsistent
        {
            get
            {
                ComputeEdgeEndLabels();
                return CheckAreaLabelsConsistent(0);
            }
        }

        /// <summary>
        /// Insert a EdgeEnd into this EdgeEndStar.
        /// </summary>
        /// <param name="e"></param>
        public abstract void Insert(EdgeEnd e);

        /// <summary>
        /// Insert an EdgeEnd into the map, and clear the edgeList cache,
        /// since the list of edges has now changed.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        protected void InsertEdgeEnd(EdgeEnd e, object obj)
        {
            // Diego Guidi says: i have inserted this line because if i try to add an object already present
            // in the list, a System.ArgumentException was thrown.
            if (_edgeMap.Contains(e))
                return;
            _edgeMap.Add(e, obj);
            _edgeList = null;    // edge list has changed - clear the cache
        }

        /// <summary>
        /// Iterator access to the ordered list of edges is optimized by
        /// copying the map collection to a list.  (This assumes that
        /// once an iterator is requested, it is likely that insertion into
        /// the map is complete).
        /// </summary>
        public virtual IEnumerator GetEnumerator()
        {
            return Edges.GetEnumerator();
        }

        /// <summary>
        /// Initializes the edges in the _edgeList
        /// </summary>
        protected void InitializeEdges()
        {
            if (_edgeList == null)
                _edgeList = new ArrayList(_edgeMap.Values);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ee"></param>
        /// <returns></returns>
        public virtual EdgeEnd GetNextCw(EdgeEnd ee)
        {
            InitializeEdges();
            int i = _edgeList.IndexOf(ee);
            int iNextCw = i - 1;
            if (i == 0)
                iNextCw = _edgeList.Count - 1;
            return (EdgeEnd)_edgeList[iNextCw];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        public virtual void ComputeLabelling(GeometryGraph[] geom)
        {
            ComputeEdgeEndLabels();
            // Propagate side labels  around the edges in the star
            // for each parent Geometry
            PropagateSideLabels(0);
            PropagateSideLabels(1);

            /*
            * If there are edges that still have null labels for a point
            * this must be because there are no area edges for that point incident on this node.
            * In this case, to label the edge for that point we must test whether the
            * edge is in the interior of the point.
            * To do this it suffices to determine whether the node for the edge is in the interior of an area.
            * If so, the edge has location Interior for the point.
            * In all other cases (e.g. the node is on a line, on a point, or not on the point at all) the edge
            * has the location Exterior for the point.
            *
            * Notice that the edge cannot be on the Boundary of the point, since then
            * there would have been a parallel edge from the Geometry at this node also labelled Boundary
            * and this edge would have been labelled in the previous step.
            *
            * This code causes a problem when dimensional collapses are present, since it may try and
            * determine the location of a node where a dimensional collapse has occurred.
            * The point should be considered to be on the Exterior
            * of the polygon, but locate() will return Interior, since it is passed
            * the original Geometry, not the collapsed version.
            *
            * If there are incident edges which are Line edges labelled Boundary,
            * then they must be edges resulting from dimensional collapses.
            * In this case the other edges can be labelled Exterior for this Geometry.
            *
            * MD 8/11/01 - NOT True!  The collapsed edges may in fact be in the interior of the Geometry,
            * which means the other edges should be labelled Interior for this Geometry.
            * Not sure how solve this...  Possibly labelling needs to be split into several phases:
            * area label propagation, symLabel merging, then finally null label resolution.
            */
            bool[] hasDimensionalCollapseEdge = { false, false };
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                Label label = e.Label;
                for (int geomi = 0; geomi < 2; geomi++)
                    if (label.IsLine(geomi) && label.GetLocation(geomi) == LocationType.Boundary)
                        hasDimensionalCollapseEdge[geomi] = true;
            }
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                Label label = e.Label;
                for (int geomi = 0; geomi < 2; geomi++)
                {
                    if (label.IsAnyNull(geomi))
                    {
                        LocationType loc;
                        if (hasDimensionalCollapseEdge[geomi])
                            loc = LocationType.Exterior;
                        else
                        {
                            Coordinate p = e.Coordinate;
                            loc = GetLocation(geomi, p, geom);
                        }
                        label.SetAllLocationsIfNull(geomi, loc);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeEdgeEndLabels()
        {
            // Compute edge label for each EdgeEnd
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd ee = (EdgeEnd)it.Current;
                ee.ComputeLabel();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="p"></param>
        /// <param name="geom"></param>
        /// <returns></returns>
        public virtual LocationType GetLocation(int geomIndex, Coordinate p, GeometryGraph[] geom)
        {
            // compute location only on demand
            if (_ptInAreaLocation[geomIndex] == LocationType.Null)
                _ptInAreaLocation[geomIndex] = SimplePointInAreaLocator.Locate(p, geom[geomIndex].Geometry);
            return _ptInAreaLocation[geomIndex];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <returns></returns>
        private bool CheckAreaLabelsConsistent(int geomIndex)
        {
            // Since edges are stored in CCW order around the node,
            // As we move around the ring we move from the right to the left side of the edge
            IList edges = Edges;
            // if no edges, trivially consistent
            if (edges.Count <= 0)
                return true;
            // initialize startLoc to location of last Curve side (if any)
            int lastEdgeIndex = edges.Count - 1;
            Label startLabel = ((EdgeEnd)edges[lastEdgeIndex]).Label;
            LocationType startLoc = startLabel.GetLocation(geomIndex, PositionType.Left);
            Assert.IsTrue(startLoc != LocationType.Null, "Found unlabelled area edge");

            LocationType currLoc = startLoc;
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                Label label = e.Label;
                // we assume that we are only checking a area
                Assert.IsTrue(label.IsArea(geomIndex), "Found non-area edge");
                LocationType leftLoc = label.GetLocation(geomIndex, PositionType.Left);
                LocationType rightLoc = label.GetLocation(geomIndex, PositionType.Right);
                // check that edge is really a boundary between inside and outside!
                if (leftLoc == rightLoc)
                    return false;
                // check side location conflict
                if (rightLoc != currLoc)
                    return false;
                currLoc = leftLoc;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        public virtual void PropagateSideLabels(int geomIndex)
        {
            // Since edges are stored in CCW order around the node,
            // As we move around the ring we move from the right to the left side of the edge
            LocationType startLoc = LocationType.Null;
            // initialize loc to location of last Curve side (if any)
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                Label label = e.Label;
                if (label.IsArea(geomIndex) && label.GetLocation(geomIndex, PositionType.Left) != LocationType.Null)
                    startLoc = label.GetLocation(geomIndex, PositionType.Left);
            }
            // no labelled sides found, so no labels to propagate
            if (startLoc == LocationType.Null)
                return;

            LocationType currLoc = startLoc;
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                Label label = e.Label;
                // set null On values to be in current location
                if (label.GetLocation(geomIndex, PositionType.On) == LocationType.Null)
                    label.SetLocation(geomIndex, PositionType.On, currLoc);
                // set side labels (if any)
                // if (label.IsArea())    //ORIGINAL
                if (label.IsArea(geomIndex))
                {
                    LocationType leftLoc = label.GetLocation(geomIndex, PositionType.Left);
                    LocationType rightLoc = label.GetLocation(geomIndex, PositionType.Right);
                    // if there is a right location, that is the next location to propagate
                    if (rightLoc != LocationType.Null)
                    {
                        if (rightLoc != currLoc)
                            throw new TopologyException(TopologyText.SideLocationConflict, e.Coordinate);
                        if (leftLoc == LocationType.Null)
                            throw new TopologyException(TopologyText.SingleNullSide, e.Coordinate);
                        currLoc = leftLoc;
                    }
                    else
                    {
                        /* RHS is null - LHS must be null too.
                        *  This must be an edge from the other point, which has no location
                        *  labelling for this point.  This edge must lie wholly inside or outside
                        *  the other point (which is determined by the current location).
                        *  Assign both sides to be the current location.
                        */
                        Assert.IsTrue(label.GetLocation(geomIndex, PositionType.Left) == LocationType.Null, "found single null side");
                        label.SetLocation(geomIndex, PositionType.Right, currLoc);
                        label.SetLocation(geomIndex, PositionType.Left, currLoc);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eSearch"></param>
        /// <returns></returns>
        public virtual int FindIndex(EdgeEnd eSearch)
        {
            GetEnumerator();   // force edgelist to be computed
            for (int i = 0; i < _edgeList.Count; i++)
            {
                EdgeEnd e = (EdgeEnd)_edgeList[i];
                if (e == eSearch)
                    return i;
            }
            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            for (IEnumerator it = GetEnumerator(); it.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)it.Current;
                e.Write(outstream);
            }
        }
    }
}