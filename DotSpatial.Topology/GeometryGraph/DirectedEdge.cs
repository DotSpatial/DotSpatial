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

using System.IO;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    ///
    /// </summary>
    public class DirectedEdge : EdgeEnd
    {
        #region Private Variables

        /// <summary>
        /// The depth of each side (position) of this edge.
        /// The 0 element of the array is never used.
        /// </summary>
        private readonly int[] _depth = { 0, -999, -999 };

        private EdgeRing _edgeRing;  // the EdgeRing that this edge is part of

        private bool _isForward;
        private bool _isInResult;
        private bool _isVisited;
        private EdgeRing _minEdgeRing;  // the MinimalEdgeRing that this edge is part of

        private DirectedEdge _next;  // the next edge in the edge ring for the polygon containing this edge
        private DirectedEdge _nextMin;  // the next edge in the MinimalEdgeRing that contains this edge
        private DirectedEdge _sym; // the symmetric edge

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a directed edge
        /// </summary>
        /// <param name="inEdge">The edge to use in order to create a directed edge</param>
        /// <param name="inIsForward">A boolean that forces whether or not this edge is counted as forward</param>
        public DirectedEdge(Edge inEdge, bool inIsForward)
            : base(inEdge)
        {
            _isForward = inIsForward;
            if (_isForward)
                base.Init(inEdge.GetCoordinate(0), inEdge.GetCoordinate(1));
            else
            {
                int n = inEdge.NumPoints - 1;
                base.Init(inEdge.GetCoordinate(n), inEdge.GetCoordinate(n - 1));
            }
            ComputeDirectedLabel();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compute the label in the appropriate orientation for this DirEdge.
        /// </summary>
        private void ComputeDirectedLabel()
        {
            Label = new Label(Edge.Label);
            if (!_isForward)
                Label.Flip();
        }

        /// <summary>
        /// Retrieves an integer that describes the depth
        /// </summary>
        /// <param name="position">A Positions enumeration</param>
        /// <returns>An integer showing the depth</returns>
        public virtual int GetDepth(PositionType position)
        {
            return _depth[(int)position];
        }

        /// <summary>
        /// Sets the depth value for this edge based on the specified position
        /// </summary>
        /// <param name="position">A Position</param>
        /// <param name="depthVal">The integer depth to specify</param>
        /// <exception cref="TopologyException">Assigned depths do not match</exception>
        public virtual void SetDepth(PositionType position, int depthVal)
        {
            if (_depth[(int)position] != -999)
                if (_depth[(int)position] != depthVal)
                    throw new TopologyException(TopologyText.TopologyException_Depth, Coordinate);
            _depth[(int)position] = depthVal;
        }

        /// <summary>
        /// Set both edge depths.
        /// One depth for a given side is provided.
        /// The other is computed depending on the Location
        /// transition and the depthDelta of the edge.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="depth"></param>
        public virtual void SetEdgeDepths(PositionType position, int depth)
        {
            // get the depth transition delta from R to Curve for this directed Edge
            int depthDelta = Edge.DepthDelta;
            if (!_isForward)
                depthDelta = -depthDelta;

            // if moving from Curve to R instead of R to Curve must change sign of delta
            int directionFactor = 1;
            if (position == PositionType.Left)
                directionFactor = -1;

            PositionType oppositePos = Position.Opposite(position);
            int delta = depthDelta * directionFactor;
            int oppositeDepth = depth + delta;
            SetDepth(position, depth);
            SetDepth(oppositePos, oppositeDepth);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public override void Write(StreamWriter outstream)
        {
            base.Write(outstream);
            outstream.Write(" " + _depth[(int)PositionType.Left] + "/" + _depth[(int)PositionType.Right]);
            outstream.Write(" (" + DepthDelta + ")");
            if (_isInResult)
                outstream.Write(" inResult");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void WriteEdge(StreamWriter outstream)
        {
            Write(outstream);
            outstream.Write(" ");
            if (_isForward)
                Edge.Write(outstream);
            else Edge.WriteReverse(outstream);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtains the chaing in depth
        /// </summary>
        public virtual int DepthDelta
        {
            get
            {
                int depthDelta = Edge.DepthDelta;
                if (!_isForward)
                    depthDelta = -depthDelta;
                return depthDelta;
            }
        }

        /// <summary>
        /// Gets or sets the EdgeRing
        /// </summary>
        public virtual EdgeRing EdgeRing
        {
            get { return _edgeRing; }
            set { _edgeRing = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether this edge is directed forward
        /// </summary>
        public virtual bool IsForward
        {
            get { return _isForward; }
            protected set { _isForward = value; }
        }

        /// <summary>
        /// Gets a boolean that is true if this edge is in the result
        /// </summary>
        public virtual bool IsInResult
        {
            get { return _isInResult; }
            set { _isInResult = value; }
        }

        /// <summary>
        /// This is an interior Area edge if
        /// its label is an Area label for both Geometries
        /// and for each Geometry both sides are in the interior.
        /// </summary>
        /// <returns><c>true</c> if this is an interior Area edge.</returns>
        public virtual bool IsInteriorAreaEdge
        {
            get
            {
                bool isInteriorAreaEdge = true;
                for (int i = 0; i < 2; i++)
                {
                    if (!(Label.IsArea(i)
                          && Label.GetLocation(i, PositionType.Left) == LocationType.Interior
                          && Label.GetLocation(i, PositionType.Right) == LocationType.Interior))
                    {
                        isInteriorAreaEdge = false;
                    }
                }
                return isInteriorAreaEdge;
            }
        }

        /// <summary>
        /// This edge is a line edge if
        /// at least one of the labels is a line label
        /// any labels which are not line labels have all Locations = Exterior.
        /// </summary>
        public virtual bool IsLineEdge
        {
            get
            {
                bool isLine = Label.IsLine(0) || Label.IsLine(1);
                bool isExteriorIfArea0 =
                    !Label.IsArea(0) || Label.AllPositionsEqual(0, LocationType.Exterior);
                bool isExteriorIfArea1 =
                    !Label.IsArea(1) || Label.AllPositionsEqual(1, LocationType.Exterior);
                return isLine && isExteriorIfArea0 && isExteriorIfArea1;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that is true if this edge has been visited
        /// </summary>
        public virtual bool IsVisited
        {
            get { return _isVisited; }
            set { _isVisited = value; }
        }

        /// <summary>
        /// Gets or sets the minimum Edge Ring
        /// </summary>
        public virtual EdgeRing MinEdgeRing
        {
            get { return _minEdgeRing; }
            set { _minEdgeRing = value; }
        }

        /// <summary>
        /// Gets or sets the next directed edge relative to this directed edge
        /// </summary>
        public virtual DirectedEdge Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        /// Gets or sets a directed edge for Next Min
        /// </summary>
        public virtual DirectedEdge NextMin
        {
            get { return _nextMin; }
            set { _nextMin = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual DirectedEdge Sym
        {
            get { return _sym; }
            set { _sym = value; }
        }

        /// <summary>
        /// VisitedEdge get property returns <c>true</c> if bot Visited
        /// and Sym.Visited are <c>true</c>.
        /// VisitedEdge set property marks both DirectedEdges attached to a given Edge.
        /// This is used for edges corresponding to lines, which will only
        /// appear oriented in a single direction in the result.
        /// </summary>
        public virtual bool VisitedEdge
        {
            get
            {
                return _isVisited && _sym.IsVisited;
            }
            set
            {
                _isVisited = value;
                _sym.IsVisited = value;
            }
        }

        #endregion

        #region Static

        /// <summary>
        /// Computes the factor for the change in depth when moving from one location to another.
        /// E.g. if crossing from the Interior to the Exterior the depth decreases, so the factor is -1.
        /// </summary>
        public static int DepthFactor(LocationType currLocation, LocationType nextLocation)
        {
            if (currLocation == LocationType.Exterior && nextLocation == LocationType.Interior)
                return 1;
            if (currLocation == LocationType.Interior && nextLocation == LocationType.Exterior)
                return -1;
            return 0;
        }

        #endregion
    }
}