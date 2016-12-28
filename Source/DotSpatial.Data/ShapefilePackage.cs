using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// a container for shapefile components
    /// </summary>
    public class ShapefilePackage
    {
        /// <summary>
        /// shapefile
        /// </summary>
        public Stream ShpFile { get; set; }
        /// <summary>
        /// shapefile index
        /// </summary>
        public Stream ShxFile { get; set; }
        /// <summary>
        /// shapefile database
        /// </summary>
        public Stream DbfFile { get; set; }
        /// <summary>
        /// shapefile projection
        /// </summary>
        public Stream PrjFile { get; set; }
    }
}
