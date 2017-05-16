﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using GeoAPI.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Contains extension methods for GeoAPI.Geometries.IGeometry.
    /// </summary>
    public static class GeometryExt
    {
        #region Methods

        /// <summary>
        /// Rotates the geometry by the given radian angle around the origin.
        /// </summary>
        /// <param name="self">this</param>
        /// <param name="origin">Coordinate the geometry gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public static void Rotate(this IGeometry self, Coordinate origin, double radAngle)
        {
            switch (self.OgcGeometryType)
            {
                case OgcGeometryType.Point:
                    IPoint pnt = self as IPoint;
                    if (pnt != null)
                    {
                        RotateCoordinateRad(origin, ref pnt.Coordinate.X, ref pnt.Coordinate.Y, radAngle);
                    }

                    break;
                case OgcGeometryType.LineString:
                    ILineString l = self as ILineString;
                    if (l != null)
                    {
                        foreach (Coordinate c in l.Coordinates)
                            RotateCoordinateRad(origin, ref c.X, ref c.Y, radAngle);
                    }

                    break;
                case OgcGeometryType.Polygon:
                    IPolygon p = self as IPolygon;
                    if (p != null)
                    {
                        p.Shell.Rotate(origin, radAngle);
                        foreach (var h in p.Holes)
                            h.Rotate(origin, radAngle);
                    }

                    break;
                case OgcGeometryType.GeometryCollection:
                    IGeometryCollection geocol = self as IGeometryCollection;
                    if (geocol != null)
                    {
                        foreach (IGeometry geo in geocol.Geometries)
                            geo.Rotate(origin, radAngle);
                    }

                    break;
            }
        }

        /// <summary>
        /// Rotates the given coordinate by the given radian angle around the origin.
        /// </summary>
        /// <param name="origin">Coordinate the geometry gets rotated around.</param>
        /// <param name="coordX">X-value of the coordinate that gets rotated.</param>
        /// <param name="coordY">Y-value of the coordinate that gets rotated.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        private static void RotateCoordinateRad(Coordinate origin, ref double coordX, ref double coordY, double radAngle)
        {
            double x = origin.X + ((Math.Cos(radAngle) * (coordX - origin.X)) - (Math.Sin(radAngle) * (coordY - origin.Y)));
            double y = origin.Y + ((Math.Sin(radAngle) * (coordX - origin.X)) + (Math.Cos(radAngle) * (coordY - origin.Y)));
            coordX = x;
            coordY = y;
        }

        #endregion
    }
}