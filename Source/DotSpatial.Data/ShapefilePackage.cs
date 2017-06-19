// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// A container for shapefile components.
    /// </summary>
    public class ShapefilePackage
    {
        /// <summary>
        /// Gets or sets the shapefile.
        /// </summary>
        public Stream ShpFile { get; set; }

        /// <summary>
        /// Gets or sets the shapefile index.
        /// </summary>
        public Stream ShxFile { get; set; }

        /// <summary>
        /// Gets or sets the shapefile database.
        /// </summary>
        public Stream DbfFile { get; set; }

        /// <summary>
        /// Gets or sets the shapefile projection.
        /// </summary>
        public Stream PrjFile { get; set; }
    }
}
