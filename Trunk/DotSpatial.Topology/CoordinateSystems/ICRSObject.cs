namespace DotSpatial.Topology.CoordinateSystems
{
    /// <summary>
    /// Base Interface for CRSBase Object types.
    /// </summary>
    public interface ICRSObject
    {
        #region Properties

        /// <summary>
        /// Gets the CRS type.
        /// </summary>
        CRSTypes Type { get; }

        #endregion
    }
}