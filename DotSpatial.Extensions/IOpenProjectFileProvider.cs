using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    [InheritedExport]
    public interface IOpenProjectFileProvider : IProjectFileProvider
    {
        /// <summary>
        /// Opens the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        bool Open(string fileName);
    }
}