// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// Voronoi Graph.
    /// </summary>
    public class VoronoiGraph
    {
        #region Fields

        /// <summary>
        /// Gets the collection of VoronoiEdges. The Left and Right points are from the
        /// original set of points that are bisected by the edge. The A and B
        /// Vectors are the endpoints of the edge itself.
        /// </summary>
        public HashSet<VoronoiEdge> Edges { get; } = new HashSet<VoronoiEdge>();

        /// <summary>
        /// Gets the vertices that join the voronoi polygon edges (not the original points).
        /// </summary>
        public HashSet<Vector2> Vertices { get; } = new HashSet<Vector2>();

        #endregion
    }
}