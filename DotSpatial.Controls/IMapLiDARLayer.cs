using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for supporting LiDAR point cloud file reading and writing operations
    /// </summary>
    public interface IMapLiDARLayer : IMapLayer
    {
        /// <summary>
        /// The specialized DataSet for reading the large LAS LiDAR point cloud files
        /// </summary>
        new ILiDARData DataSet { get; set; }
    }
}