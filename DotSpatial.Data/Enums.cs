// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Clarifies whether a value is cached in a local variable or updated dynamically
    /// </summary>
    public enum CacheTypes
    {
        /// <summary>
        /// The value is cached locally, rather than calculated on the fly
        /// </summary>
        Cached,

        /// <summary>
        /// The value is calculated each type, rather than using a local cache
        /// </summary>
        Dynamic
    }

    /// <summary>
    /// Byte order
    /// </summary>
    public enum ByteOrder
    {
        /// <summary>
        /// Big Endian
        /// </summary>
        BigEndian = 0x00,

        /// <summary>
        /// Little Endian
        /// </summary>
        LittleEndian = 0x01,
    }

    /// <summary>
    /// Filetypes
    /// </summary>
    public enum VectorFileType
    {
        /// <summary>
        /// An unrecognized file format
        /// </summary>
        INVALID = -1,
        /// <summary>
        /// The Esri Shapefile format (*.shp)
        /// </summary>
        Shapefile = 0,
        /// <summary>
        /// Comma separated values (*.csv)
        /// </summary>
        CSV = 1,

        /// <summary>
        /// Google Markup Language (a variant of the Keyhole Markup Language) (*.gml)
        /// </summary>
        GML = 2,

        /// <summary>
        /// MapInfo format (*.tab)
        /// </summary>
        MapInfo = 3,

        /// <summary>
        /// CAD format (Design) (*.dgn)
        /// </summary>
        MicrostationDGN = 4,

        /// <summary>
        /// S-57 published by International Hydrographic Organization (IHO). (*.000)
        /// </summary>
        S57 = 5,

        /// <summary>
        /// Spatial Data Transfer Standard (*.sdts)
        /// </summary>
        SDTS = 6,

        /// <summary>
        /// ?
        /// </summary>
        UKNTF = 7,

        /// <summary>
        /// Topologically Integrated Geographic Encoding and Referencing (*.tig)
        /// </summary>
        TIGER = 8,

        /// <summary>
        /// ?
        /// </summary>
        PostgreSQL = 9,

        /// <summary>
        /// MS Excel ODBC Query File? (*.DQY)
        /// maybe ODBC Data Source file? (*.DSN)
        /// </summary>
        ODBC = 10,

        /// <summary>
        /// ?
        /// </summary>
        PGeo = 11,

        /// <summary>
        /// ArcInfo Vector Coverage
        /// </summary>
        AVC = 12,

        /// <summary>
        /// Geospatial Data Abstraction Library (GDAL) File (*.VRT)
        /// </summary>
        VRT = 13,

        /// <summary>
        /// Record file?  (*.REC)
        /// </summary>
        REC = 14
    };
}