using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    ///
    /// </summary>
    [InheritedExport]
    public interface ISaveProjectFileProvider : IProjectFileProvider
    {
        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="graph">The control graph.</param>
        void Save(string fileName, string graph);
    }
}