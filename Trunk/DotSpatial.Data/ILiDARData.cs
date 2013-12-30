using System;

namespace DotSpatial.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface ILiDARData
    {
        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void ReadFile(String filename);
    }
}