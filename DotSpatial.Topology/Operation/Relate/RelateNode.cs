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

using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Relate
{
    /// <summary>
    /// A RelateNode is a Node that maintains a list of EdgeStubs
    /// for the edges that are incident on it.
    /// </summary>
    public class RelateNode : Node
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="edges"></param>
        public RelateNode(Coordinate coord, EdgeEndStar edges) : base(coord, edges) { }

        /// <summary>
        /// Update the IM with the contribution for this component.
        /// A component only contributes if it has a labelling for both parent geometries.
        /// </summary>
        public override void ComputeIm(IntersectionMatrix im)
        {
            im.SetAtLeastIfValid(Label.GetLocation(0), Label.GetLocation(1), DimensionType.Point);
        }

        /// <summary>
        /// Update the IM with the contribution for the EdgeEnds incident on this node.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateImFromEdges(IntersectionMatrix im)
        {
            ((EdgeEndBundleStar)Edges).UpdateIm(im);
        }
    }
}