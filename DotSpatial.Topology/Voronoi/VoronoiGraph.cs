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
// The Original Code is from a code project example:
// http://www.codeproject.com/KB/recipes/fortunevoronoi.aspx
// which is protected under the Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:42:11 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Changed to strong typed generic HashSet classes
// ********************************************************************************************************

namespace DotSpatial.Topology.Voronoi
{
    /// <summary>
    /// Voronoi Graph
    /// </summary>
    public class VoronoiGraph
    {
        /// <summary>
        /// The collection of VoronoiEdges.  The Left and Right points are from the
        /// original set of points that are bisected by the edge.  The A and B
        /// Vectors are the endpoints of the edge itself.
        /// </summary>
        public readonly HashSet<VoronoiEdge> Edges = new HashSet<VoronoiEdge>();

        /// <summary>
        /// The vertices that join the voronoi polygon edges (not the original points)
        /// </summary>
        public readonly HashSet<Vector2> Vertices = new HashSet<Vector2>();
    }
}