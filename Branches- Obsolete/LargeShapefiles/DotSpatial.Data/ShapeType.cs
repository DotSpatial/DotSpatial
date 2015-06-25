// ********************************************************************************************************
// Product Name: DotSpatial.Data.Vectors.Shapefiles.dll Alpha
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
// The Original Code is from DotSpatial.Data.Vectors.Shapefiles.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration listing the various valid shape types supported by Esri Shapefile formats
    /// </summary>
    public enum ShapeType : byte
    {
        /// <summary>
        /// 0 - No geometric data for the shape.
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// 1 - A point consists of a pair of double-precision coordinates in the order X,Y
        /// </summary>
        Point = 1,

        /// <summary>
        /// 3 - Each shape is a collection of vertices that should be connected to form a striaght line
        /// </summary>
        PolyLine = 3,

        /// <summary>
        /// 5 - Each shape is a closed linestring
        /// </summary>
        Polygon = 5,

        /// <summary>
        /// 8 - Each shape consists of severel, unconnected points
        /// </summary>
        MultiPoint = 8,

        /// <summary>
        /// 11 - Each shape is a point (X, Y) with Z and M value
        /// </summary>
        PointZ = 11,

        /// <summary>
        /// 13 - Each shape is a linestring with each vertex having Z and M value
        /// </summary>
        PolyLineZ = 13,

        /// <summary>
        /// 15 - Each shape is a closed linestring with each vertex having Z and M value
        /// </summary>
        PolygonZ = 15,

        /// <summary>
        /// 18 - Each shape has several unconnected points, each of which has Z and M value
        /// </summary>
        MultiPointZ = 18,

        /// <summary>
        /// 21 - Each shape has several unconnected points, each of which has M value
        /// </summary>
        PointM = 21,

        /// <summary>
        /// 23 - Each shape is made up of several points connected to form a line, each vertex having M value
        /// </summary>
        PolyLineM = 23,

        /// <summary>
        /// 25 - Each shape is a closed linestring with each vertex having M value
        /// </summary>
        PolygonM = 25,

        /// <summary>
        /// 28 - Each shape has several unconnected points, each of which has M value
        /// </summary>
        MultiPointM = 28,

        /// <summary>
        /// 31 - A MultiPatch consists of a number of surface patches.
        /// </summary>
        MultiPatch = 31
    }
}