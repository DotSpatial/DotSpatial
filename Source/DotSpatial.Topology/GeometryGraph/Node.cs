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
    public class Node : GraphComponent
    {
        private Coordinate _coord; // Only non-null if this node is precise.
        private EdgeEndStar _edges;

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="edges"></param>
        public Node(Coordinate coord, EdgeEndStar edges)
        {
            _coord = coord;
            _edges = edges;
            base.Label = new Label(0, LocationType.Null);
        }

        /// <summary>
        /// A Coordinate for this node
        /// </summary>
        public override Coordinate Coordinate
        {
            get { return _coord; }
            set { _coord = value; }
        }

        /// <summary>
        /// Gets the edges for this node
        /// </summary>
        public virtual EdgeEndStar Edges
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

        /// <summary>
        /// Basic nodes do not compute IMs.
        /// </summary>
        /// <param name="im"></param>
        public override void ComputeIm(IntersectionMatrix im) { }

        /// <summary>
        /// Add the edge to the list of edges at this node.
        /// </summary>
        /// <param name="e"></param>
        public virtual void Add(EdgeEnd e)
        {
            // Assert: start pt of e is equal to node point
            _edges.Insert(e);
            e.Node = this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="n"></param>
        public virtual void MergeLabel(Node n)
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
        public virtual void MergeLabel(Label label2)
        {
            for (int i = 0; i < 2; i++)
            {
                LocationType loc = ComputeMergedLocation(label2, i);
                LocationType thisLoc = Label.GetLocation(i);
                if (thisLoc == LocationType.Null)
                    Label.SetLocation(i, loc);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="onLocation"></param>
        public virtual void SetLabel(int argIndex, LocationType onLocation)
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
        public virtual void SetLabelBoundary(int argIndex)
        {
            // determine the current location for the point (if any)
            LocationType loc = LocationType.Null;
            if (Label != null)
                loc = Label.GetLocation(argIndex);
            // flip the loc
            LocationType newLoc;
            switch (loc)
            {
                case LocationType.Boundary:
                    newLoc = LocationType.Interior;
                    break;
                case LocationType.Interior:
                    newLoc = LocationType.Boundary;
                    break;
                default:
                    newLoc = LocationType.Boundary;
                    break;
            }
            base.Label.SetLocation(argIndex, newLoc);
        }

        /// <summary>
        /// The location for a given eltIndex for a node will be one
        /// of { Null, Interior, Boundary }.
        /// A node may be on both the boundary and the interior of a point;
        /// in this case, the rule is that the node is considered to be in the boundary.
        /// The merged location is the maximum of the two input values.
        /// </summary>
        /// <param name="label2"></param>
        /// <param name="eltIndex"></param>
        public virtual LocationType ComputeMergedLocation(Label label2, int eltIndex)
        {
            LocationType loc = Label.GetLocation(eltIndex);
            if (!label2.IsNull(eltIndex))
            {
                LocationType nLoc = label2.GetLocation(eltIndex);
                if (loc != LocationType.Boundary)
                    loc = nLoc;
            }
            return loc;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            outstream.WriteLine("node " + _coord + " lbl: " + Label);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _coord + " " + _edges;
        }
    }
}