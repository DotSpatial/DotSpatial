using DotSpatial.Topology.EdgeGraph;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Dissolve
{
    /// <summary>
    /// A graph containing <see cref="DissolveHalfEdge"/>s.
    /// </summary>
    public class DissolveEdgeGraph : EdgeGraph.EdgeGraph
    {
        #region Methods

        protected override HalfEdge CreateEdge(Coordinate p0)
        {
            return new DissolveHalfEdge(p0);
        }

        #endregion
    }
}