// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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