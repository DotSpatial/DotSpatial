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

namespace DotSpatial.Topology.IO
{
    /// <summary>
    /// WkbGeometryTypes
    /// </summary>
    public enum WKBGeometryType
    {
        /// <summary>
        /// Point.
        /// </summary>
        WKBPoint = 1,

        /// <summary>
        /// LineString.
        /// </summary>
        WKBLineString = 2,

        /// <summary>
        /// Polygon.
        /// </summary>
        WKBPolygon = 3,

        /// <summary>
        /// MultiPoint.
        /// </summary>
        WKBMultiPoint = 4,

        /// <summary>
        /// MultiLineString.
        /// </summary>
        WKBMultiLineString = 5,

        /// <summary>
        /// MultiPolygon.
        /// </summary>
        WKBMultiPolygon = 6,

        /// <summary>
        /// GeometryCollection.
        /// </summary>
        WKBGeometryCollection = 7,

        /// <summary>
        /// Point with Z coordinate.
        /// </summary>
        WKBPointZ = 1001,

        /// <summary>
        /// LineString with Z coordinate.
        /// </summary>
        WKBLineStringZ = 1002,

        /// <summary>
        /// Polygon with Z coordinate.
        /// </summary>
        WKBPolygonZ = 1003,

        /// <summary>
        /// MultiPoint with Z coordinate.
        /// </summary>
        WKBMultiPointZ = 1004,

        /// <summary>
        /// MultiLineString with Z coordinate.
        /// </summary>
        WKBMultiLineStringZ = 1005,

        /// <summary>
        /// MultiPolygon with Z coordinate.
        /// </summary>
        WKBMultiPolygonZ = 1006,

        /// <summary>
        /// GeometryCollection with Z coordinate.
        /// </summary>
        WKBGeometryCollectionZ = 1007,

        /// <summary>
        /// Point with M ordinate value.
        /// </summary>
        WKBPointM = 2001,

        /// <summary>
        /// LineString with M ordinate value.
        /// </summary>
        WKBLineStringM = 2002,

        /// <summary>
        /// Polygon with M ordinate value.
        /// </summary>
        WKBPolygonM = 2003,

        /// <summary>
        /// MultiPoint with M ordinate value.
        /// </summary>
        WKBMultiPointM = 2004,

        /// <summary>
        /// MultiLineString with M ordinate value.
        /// </summary>
        WKBMultiLineStringM = 2005,

        /// <summary>
        /// MultiPolygon with M ordinate value.
        /// </summary>
        WKBMultiPolygonM = 2006,

        /// <summary>
        /// GeometryCollection with M ordinate value.
        /// </summary>
        WKBGeometryCollectionM = 2007,

        /// <summary>
        /// Point with Z coordinate and M ordinate value.
        /// </summary>
        WKBPointZM = 3001,

        /// <summary>
        /// LineString with Z coordinate and M ordinate value.
        /// </summary>
        WKBLineStringZM = 3002,

        /// <summary>
        /// Polygon with Z coordinate and M ordinate value.
        /// </summary>
        WKBPolygonZM = 3003,

        /// <summary>
        /// MultiPoint with Z coordinate and M ordinate value.
        /// </summary>
        WKBMultiPointZM = 3004,

        /// <summary>
        /// MultiLineString with Z coordinate and M ordinate value.
        /// </summary>
        WKBMultiLineStringZM = 3005,

        /// <summary>
        /// MultiPolygon with Z coordinate and M ordinate value.
        /// </summary>
        WKBMultiPolygonZM = 3006,

        /// <summary>
        /// GeometryCollection with Z coordinate and M ordinate value.
        /// </summary>
        WKBGeometryCollectionZM = 3007
    };
}