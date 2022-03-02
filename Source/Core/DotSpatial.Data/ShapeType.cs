// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration listing the various valid shape types supported by Esri Shapefile formats.
    /// </summary>
    public enum ShapeType : byte
    {
        /// <summary>
        /// 0 - No shape type specified, or the shapetype is invalid
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// 1 - Each shape is a single point
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
        /// 11 - Each shape is a point with a Z value
        /// </summary>
        PointZ = 11,

        /// <summary>
        /// 13 - Each shape is a linestring with each vertex having a z value
        /// </summary>
        PolyLineZ = 13,

        /// <summary>
        /// 15 - Each shape is a closed linestring with each vertex having a z value
        /// </summary>
        PolygonZ = 15,

        /// <summary>
        /// 18 - Each shape has several unconnected points, each of which has a z value
        /// </summary>
        MultiPointZ = 18,

        /// <summary>
        /// 21 - Each shape has several unconnected points, each of which has an m and z value
        /// </summary>
        PointM = 21,

        /// <summary>
        /// 23 - Each shape is made up of several points connected to form a line, each vertex having an m and z value
        /// </summary>
        PolyLineM = 23,

        /// <summary>
        /// 25 - Each shape is a closed linestring with each vertex having a z value and m value
        /// </summary>
        PolygonM = 25,

        /// <summary>
        /// 28 - Each shape has several unconnected points, each of which has a z value and m value
        /// </summary>
        MultiPointM = 28,

        /// <summary>
        /// 31 - Not sure what this does
        /// </summary>
        MultiPatch = 31
    }
}