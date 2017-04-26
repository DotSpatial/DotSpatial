// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:55:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleaned up the raw source from code project
// ********************************************************************************************************

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VEdgeNode.
    /// </summary>
    internal class VEdgeNode : VNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VEdgeNode"/> class.
        /// </summary>
        /// <param name="e">The edge of this VEdgeNode.</param>
        /// <param name="flipped">Indicates whether the edge should be flipped.</param>
        public VEdgeNode(VoronoiEdge e, bool flipped)
        {
            Edge = e;
            Flipped = flipped;
        }

        #region Properties

        /// <summary>
        /// Gets the edge.
        /// </summary>
        public VoronoiEdge Edge { get; }

        /// <summary>
        /// Gets a value indicating whether the edge is flipped.
        /// </summary>
        public bool Flipped { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Performs a parabolic cut on the edge.
        /// </summary>
        /// <param name="ys">The ys.</param>
        /// <param name="x">The x.</param>
        /// <returns>The resulting x.</returns>
        public double Cut(double ys, double x)
        {
            if (!Flipped) return x - Fortune.ParabolicCut(Edge.LeftData.X, Edge.LeftData.Y, Edge.RightData.X, Edge.RightData.Y, ys);
            return x - Fortune.ParabolicCut(Edge.RightData.X, Edge.RightData.Y, Edge.LeftData.X, Edge.LeftData.Y, ys);
        }

        #endregion
    }
}