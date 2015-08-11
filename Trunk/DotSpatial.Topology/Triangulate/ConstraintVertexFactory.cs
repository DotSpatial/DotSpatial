using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Triangulate
{
    /// <summary>
    /// An interface for factories which create a {@link ConstraintVertex}
    /// </summary>
    /// <author>Martin Davis</author>
    public interface ConstraintVertexFactory
    {
        #region Methods

        ConstraintVertex CreateVertex(Coordinate p, Segment constraintSeg);

        #endregion
    }
}