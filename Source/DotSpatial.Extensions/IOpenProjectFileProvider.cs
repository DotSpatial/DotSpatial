using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Project file provider with capability to open files.
    /// </summary>
    [InheritedExport]
    public interface IOpenProjectFileProvider : IProjectFileProvider
    {
        /// <summary>
        /// Opens the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True, if the file was opened.</returns>
        bool Open(string fileName);
    }
}