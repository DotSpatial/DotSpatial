// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Contains extension methods for NetTopologySuite.Geometries.Geometry.
    /// </summary>
    public static class GeometryExt
    {
        #region Methods

        /// <summary>
        /// Gets the maximal M value of the Geometry.
        /// </summary>
        /// <param name="geo">Geometry to get the maximal M value from.</param>
        /// <returns>0 if no M there were no M values. Otherwise the maximal M value found.</returns>
        public static double MaxM(this Geometry geo)
        {
            if (geo.Coordinates.Any(_ => _ is CoordinateM))
            {
                double maxCoordinateM = geo.Coordinates.Where(_ => _ is CoordinateM).Select(_ => _.M).Max();

                if (geo.Coordinates.Any(_ => _ is CoordinateZM))
                {
                    var maxCoordinateZM = geo.Coordinates.Where(_ => _ is CoordinateZM).Select(_ => _.M).Max();

                    if (maxCoordinateZM > maxCoordinateM)
                    {
                        maxCoordinateM = maxCoordinateZM;
                    }
                }

                return maxCoordinateM;
            }
            else if (geo.Coordinates.Any(_ => _ is CoordinateZM))
            {
                return geo.Coordinates.Where(_ => _ is CoordinateZM).Select(_ => _.M).Max();
            }

            return 0;
        }

        /// <summary>
        /// Gets the minimal M value of the Geometry.
        /// </summary>
        /// <param name="geo">Geometry to get the minimal M value from.</param>
        /// <returns>0 if there were no M values. Otherwise the minimal M value found.</returns>
        public static double MinM(this Geometry geo)
        {
            if (geo.Coordinates.Any(_ => _ is CoordinateM))
            {
                double minCoordinateM = geo.Coordinates.Where(_ => _ is CoordinateM).Select(_ => _.M).Min();

                if (geo.Coordinates.Any(_ => _ is CoordinateZM))
                {
                    var minCoordinateZM = geo.Coordinates.Where(_ => _ is CoordinateZM).Select(_ => _.M).Min();

                    if (minCoordinateZM < minCoordinateM)
                    {
                        minCoordinateM = minCoordinateZM;
                    }
                }

                return minCoordinateM;
            }
            else if (geo.Coordinates.Any(_ => _ is CoordinateZM))
            {
                return geo.Coordinates.Where(_ => _ is CoordinateZM).Select(_ => _.M).Min();
            }

            return 0;
        }

        /// <summary>
        /// Gets the maximal Z value of the Geometry.
        /// </summary>
        /// <param name="geo">Geometry to get the maximal M value from.</param>
        /// <returns>0 if there were no Z values. Otherwise the maximal Z value found.</returns>
        public static double MaxZ(this Geometry geo)
        {
            // CoordinateZM is also of type CoordinateZ
            if (geo.Coordinates.Any(_ => _ is CoordinateZ))
            {
                return geo.Coordinates.Where(_ => _ is CoordinateZ).Select(_ => _.Z).Max();
            }

            return 0;
        }

        /// <summary>
        /// Gets the minimal Z value of the Geometry.
        /// </summary>
        /// <param name="geo">Geometry to get the minimal Z value from.</param>
        /// <returns>0 if there were no Z values. Otherwise the minimal Z value found.</returns>
        public static double MinZ(this Geometry geo)
        {
            // CoordinateZM is also of type CoordinateZ
            if (geo.Coordinates.Any(_ => _ is CoordinateZ))
            {
                return geo.Coordinates.Where(_ => _ is CoordinateZ).Select(_ => _.Z).Min();
            }

            return 0;
        }

        /// <summary>
        /// Rotates the geometry by the given radian angle around the origin.
        /// </summary>
        /// <param name="self">this.</param>
        /// <param name="origin">Coordinate the geometry gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public static void Rotate(this Geometry self, Coordinate origin, double radAngle)
        {
            switch (self.OgcGeometryType)
            {
                case OgcGeometryType.Point:
                    Point pnt = self as Point;
                    if (pnt != null)
                    {
                        pnt.Coordinate.RotateCoordinateRad(origin, radAngle);
                    }

                    break;
                case OgcGeometryType.LineString:
                    LineString l = self as LineString;
                    if (l != null)
                    {
                        foreach (Coordinate c in l.Coordinates)
                        {
                            c.RotateCoordinateRad(origin, radAngle);
                        }
                    }

                    break;
                case OgcGeometryType.Polygon:
                    Polygon p = self as Polygon;
                    if (p != null)
                    {
                        p.Shell.Rotate(origin, radAngle);
                        foreach (var h in p.Holes)
                            h.Rotate(origin, radAngle);
                    }

                    break;
                case OgcGeometryType.MultiPoint:
                case OgcGeometryType.MultiLineString:
                case OgcGeometryType.MultiPolygon:
                case OgcGeometryType.GeometryCollection:
                    GeometryCollection geocol = self as GeometryCollection;
                    if (geocol != null)
                    {
                        foreach (Geometry geo in geocol.Geometries)
                            geo.Rotate(origin, radAngle);
                    }

                    break;
                default: throw new NotSupportedException("Can't handle OgcGeometryType " + self.OgcGeometryType);
            }
        }

        /// <summary>
        /// Rotates the given coordinate by the given radian angle around the origin.
        /// </summary>
        /// <param name="coord">The coordinate that gets rotated.</param>
        /// <param name="origin">Coordinate the geometry gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        private static void RotateCoordinateRad(this Coordinate coord, Coordinate origin, double radAngle)
        {
            double x = origin.X + ((Math.Cos(radAngle) * (coord.X - origin.X)) - (Math.Sin(radAngle) * (coord.Y - origin.Y)));
            double y = origin.Y + ((Math.Sin(radAngle) * (coord.X - origin.X)) + (Math.Cos(radAngle) * (coord.Y - origin.Y)));
            coord.X = Math.Round(x, 2);
            coord.Y = Math.Round(y, 2);
        }

        #endregion
    }
}