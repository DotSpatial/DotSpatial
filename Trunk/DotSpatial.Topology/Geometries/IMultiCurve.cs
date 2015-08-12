namespace DotSpatial.Topology.Geometries
{
    public interface IMultiCurve : IGeometryCollection
    {
        #region Properties

        bool IsClosed { get; }

        #endregion
    }
}