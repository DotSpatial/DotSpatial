namespace DotSpatial.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface ILiDARDataProvider : IDataProvider
    {
        /// <summary>
        /// Opens a new LiDAR LAS with the specified fileName
        /// </summary>
        /// <param name="fileName">The string file to open</param>
        /// <returns>An IImageData object</returns>
        new ILiDARData Open(string fileName);
    }
}