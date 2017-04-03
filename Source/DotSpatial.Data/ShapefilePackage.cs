using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// A container for shapefile components.
    /// </summary>
    public class ShapefilePackage
    {
        /// <summary>
        /// Gets or sets shapefile
        /// </summary>
        public Stream ShpFile { get; set; }

        /// <summary>
        /// Gets os sets shapefile index.
        /// </summary>
        public Stream ShxFile { get; set; }

        /// <summary>
        /// Gets or sets shapefile database.
        /// </summary>
        public Stream DbfFile { get; set; }

        /// <summary>
        /// Gets or sets shapefile projection.
        /// </summary>
        public Stream PrjFile { get; set; }
    }
}
