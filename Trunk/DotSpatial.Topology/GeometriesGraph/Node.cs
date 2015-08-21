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
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    ///
    /// </summary>
    public class Node : GraphComponent
    {
        #region Fields

        /// <summary>
        /// Only non-null if this node is precise.
        /// </summary>
        private Coordinate _coord;
        private EdgeEndStar _edges;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="edges"></param>
        public Node(Coordinate coord, EdgeEndStar edges)
        {
            _coord = coord;
            _edges = edges;
            Label = new Label(0, Location.Null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// A Coordinate for this node
        /// </summary>
        public override Coordinate Coordinate
        {
            get { return _coord; }
            protected set { _coord = value; }
        }

        /// <summary>
        /// Gets the edges for this node
        /// </summary>
        public EdgeEndStar Edges
        {
            get { return _edges; }
            protected set { _edges = value; }
        }

        /// <summary>
        /// Gets a boolean that is true if this node is isolated
        /// </summary>
        public override bool IsIsolated
        {
            get
            {
                return (Label.GeometryCount == 1);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add the edge to the list of edges at this node.
        /// </summary>
        /// <param name="e"></param>
        public void Add(EdgeEnd e)
        {
            // Assert: start pt of e is equal to node point
            _edges.Insert(e);
            e.Node = this;
        }

        /// <summary>
        /// Basic nodes do not compute IMs.
        /// </summary>
        /// <param name="im"></param>
        public override void ComputeIM(IntersectionMatrix im) { }

        /// <summary>
        /// The location for a given eltIndex for a node will be one
        /// of { Null, Interior, Boundary }.
        /// A node may be on both the boundary and the interior of a point;
        /// in this case, the rule is that the node is considered to be in the boundary.
        /// The merged location is the maximum of the two input values.
        /// </summary>
        /// <param name="label2"></param>
        /// <param name="eltIndex"></param>
        public Location ComputeMergedLocation(Label label2, int eltIndex)
        {
            Location loc = Label.GetLocation(eltIndex);
            if (!label2.IsNull(eltIndex)) 
            {
                Location nLoc = label2.GetLocation(eltIndex);
                if (loc != Location.Boundary) 
                    loc = nLoc;
            }
            return loc;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="n"></param>
        public void MergeLabel(Node n)
        {
            MergeLabel(n.Label);
        }

        /// <summary>
        /// To merge labels for two nodes,
        /// the merged location for each LabelElement is computed.
        /// The location for the corresponding node LabelElement is set to the result,
        /// as long as the location is non-null.
        /// </summary>
        /// <param name="label2"></param>
        public void MergeLabel(Label label2)
        {
            for (int i = 0; i < 2; i++) 
            {
                Location loc = ComputeMergedLocation(label2, i);
                Location thisLoc = Label.GetLocation(i);
                if (thisLoc == Location.Null) 
                    Label.SetLocation(i, loc);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="onLocation"></param>
        public void SetLabel(int argIndex, Location onLocation)
        {
            if (Label == null)
                Label = new Label(argIndex, onLocation);
            else Label.SetLocation(argIndex, onLocation);
        }

        /// <summary>
        /// Updates the label of a node to BOUNDARY,
        /// obeying the mod-2 boundaryDetermination rule.
        /// </summary>
        /// <param name="argIndex"></param>
        public void SetLabelBoundary(int argIndex)
        {
            if (Label == null) return;

            // determine the current location for the point (if any)
            Location loc = Location.Null;
            if (Label != null)
                loc = Label.GetLocation(argIndex);
            // flip the loc
            Location newLoc;
            switch (loc)
            {
            case Location.Boundary:
                newLoc = Location.Interior; 
                break;
            case Location.Interior:
                newLoc = Location.Boundary; 
                break;
            default:
                newLoc = Location.Boundary; 
                break;
            }
            Label.SetLocation(argIndex, newLoc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _coord + " " + _edges;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public void Write(TextWriter outstream)
        {
            outstream.WriteLine("node " + _coord + " lbl: " + Label);
        }

        #endregion
    }
}