// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// This is enumerates not only the specific types, but very specifically
    /// that the types are the Topology variety, and not simply the vector variety
    /// </summary>
    public enum GeometryType
    {
        /// <summary>
        /// A non-geometry rectangle that strictly shows extents
        /// </summary>
        Envelope,
        /// <summary>
        /// A collective group of geometries
        /// </summary>
        GeometryCollection,
        /// <summary>
        /// A closed linestring that doesn't self intersect
        /// </summary>
        LinearRing,
        /// <summary>
        /// Any linear collection of points joined by line segments
        /// </summary>
        LineString,
        /// <summary>
        /// A collection of independant LineStrings that are not connected
        /// </summary>
        MultiLineString,
        /// <summary>
        /// A Grouping of points
        /// </summary>
        MultiPoint,
        /// <summary>
        /// A Collection of unconnected polygons
        /// </summary>
        MultiPolygon,
        /// <summary>
        /// A single coordinate location
        /// </summary>
        Point,
        /// <summary>
        /// At least one Linear Ring shell with any number of linear ring "holes"
        /// </summary>
        Polygon,
        /// <summary>
        /// Any other type
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Shapefile Shape types enumeration
    /// </summary>
    public enum ShapeGeometryType
    {
        /// <summary>
        /// Null Shape
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// Point
        /// </summary>
        Point = 1,

        /// <summary>
        /// LineString
        /// </summary>
        LineString = 3,

        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 5,

        /// <summary>
        /// MultiPoint
        /// </summary>
        MultiPoint = 8,

        /// <summary>
        /// PointMZ
        /// </summary>
        PointZM = 11,

        /// <summary>
        /// PolyLineMZ
        /// </summary>
        LineStringZM = 13,

        /// <summary>
        /// PolygonMZ
        /// </summary>
        PolygonZM = 15,

        /// <summary>
        /// MultiPointMZ
        /// </summary>
        MultiPointZM = 18,

        /// <summary>
        /// PointM
        /// </summary>
        PointM = 21,

        /// <summary>
        /// LineStringM
        /// </summary>
        LineStringM = 23,

        /// <summary>
        /// PolygonM
        /// </summary>
        PolygonM = 25,

        /// <summary>
        /// MultiPointM
        /// </summary>
        MultiPointM = 28,

        /// <summary>
        /// MultiPatch
        /// </summary>
        MultiPatch = 31,

        /// <summary>
        /// PointZ
        /// </summary>
        PointZ = 9,

        /// <summary>
        /// LineStringZ
        /// </summary>
        LineStringZ = 10,

        /// <summary>
        /// PolygonZ
        /// </summary>
        PolygonZ = 19,

        /// <summary>
        /// MultiPointZ
        /// </summary>
        MultiPointZ = 20,
    }

    /// <summary>
    /// Field Types
    /// </summary>
    public enum VectorFieldType
    {
        // from GDAL 1.3.1
        /// <summary>
        /// Integer
        /// </summary>
        OFTInteger = 0,
        /// <summary>
        /// Integer List
        /// </summary>
        OFTIntegerList = 1,
        /// <summary>
        /// Real
        /// </summary>
        OFTReal = 2,
        /// <summary>
        /// Real List
        /// </summary>
        OFTRealList = 3,
        /// <summary>
        /// String
        /// </summary>
        OFTString = 4,
        /// <summary>
        /// String List
        /// </summary>
        OFTStringList = 5,
        /// <summary>
        /// Wide String
        /// </summary>
        OFTWideString = 6,
        /// <summary>
        /// Side String List
        /// </summary>
        OFTWideStringList = 7,
        /// <summary>
        /// Binary
        /// </summary>
        OFTBinary = 8,
        /// <summary>
        /// Date
        /// </summary>
        OFTDate = 9,
        /// <summary>
        /// Time
        /// </summary>
        OFTTime = 10,

        /// <summary>
        /// DateTime
        /// </summary>
        OFTDateTime = 11,
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = -1
    };

    /// <summary>
    /// Vector Geometry Types
    /// </summary>
    public enum VectorGeometryType : long
    {
        // subset from GDAL 1.3.1
        /// <summary>
        /// Unknown
        /// </summary>
        wkbUnknown = 0,             /* non-standard */
        /// <summary>
        /// Well Known Binary Point
        /// </summary>
        wkbPoint = 1,               /* rest are standard WKB type codes */
        /// <summary>
        /// Well Known Binary LineString
        /// </summary>
        wkbLineString = 2,

        /// <summary>
        /// Well Known Binary Polygon
        /// </summary>
        wkbPolygon = 3,
        /// <summary>
        /// Well Known Binary MultiPoint
        /// </summary>
        wkbMultiPoint = 4,
        /// <summary>
        /// Well Known Binary MultiLineString
        /// </summary>
        wkbMultiLineString = 5,

        /// <summary>
        /// Well Known Binary MultiPolygon
        /// </summary>
        wkbMultiPolygon = 6,

        /// <summary>
        /// Well Known Binary Geometry Collection
        /// </summary>
        wkbGeometryCollection = 7,

        /// <summary>
        /// Well Known Binary Linear Ring
        /// /* non-standard, just for createGeometry() */
        /// </summary>
        wkbLinearRing = 101,

        /// <summary>
        /// Well Known Binary Point with Z value
        /// </summary>
        wkbPoint25D = (long)0x80000001,   /* 2.5D extensions as per 99-402 */
        /// <summary>
        /// Well Known Binary Line String with Z values
        /// </summary>
        wkbLineString25D = (long)0x80000002,

        /// <summary>
        /// Well Known Binary Polygon with Z values
        /// </summary>
        wkbPolygon25D = (long)0x80000003,

        /// <summary>
        /// Well Known Binary MultiPoint with Z values
        /// </summary>
        wkbMultiPoint25D = (long)0x80000004,

        /// <summary>
        /// Well Known Binary LineString with z values
        /// </summary>
        wkbMultiLineString25D = (long)0x80000005,

        /// <summary>
        /// Well Known Binary MultiPolygon with z values
        /// </summary>
        wkbMultiPolygon25D = (long)0x80000006
    };
}