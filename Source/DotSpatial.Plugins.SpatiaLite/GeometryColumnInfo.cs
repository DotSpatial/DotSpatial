// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// This class contains informations about geometry columns.
    /// </summary>
    public class GeometryColumnInfo
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the geometry column name.
        /// </summary>
        public string GeometryColumnName { get; set; }

        /// <summary>
        /// Gets or sets the geometry type.
        /// </summary>
        public string GeometryType { get; set; }

        /// <summary>
        /// Gets or sets the coordinate dimension.
        /// </summary>
        public int CoordDimension { get; set; }

        /// <summary>
        /// Gets or sets the table SRID.
        /// </summary>
        public int Srid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the spatial index is enabled.
        /// </summary>
        public bool SpatialIndexEnabled { get; set; }
    }
}