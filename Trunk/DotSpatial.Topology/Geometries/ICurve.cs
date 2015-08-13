namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICurve : IGeometry
    {
        #region Properties

        ICoordinateSequence CoordinateSequence { get; }

        /// <summary>
        /// Gets a topologically complete IPoint for the last coordinate
        /// </summary>
        IPoint EndPoint { get; }

        /// <summary>
        /// If the first coordinate is the same as the final coordinate, then the
        /// linestring is closed.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// If the coordinates listed for the linestring are both closed and simple then
        /// they qualify as a linear ring.
        /// </summary>
        bool IsRing { get; }

        /// <summary>
        /// Gets a topologically complete IPoint for the first coordinate
        /// </summary>
        IPoint StartPoint { get; }

        #endregion
    }
}
