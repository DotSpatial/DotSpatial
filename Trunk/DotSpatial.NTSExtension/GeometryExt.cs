using System;
using GeoAPI.Geometries;

namespace DotSpatial.NTSExtension
{
    public static class GeometryExt
    {

        /// <summary>
        /// Rotates the geometry by the given radian angle around the origin.
        /// </summary>
        /// <param name="self"/>
        /// <param name="origin">Coordinate the geometry gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public static void Rotate(this IGeometry self, Coordinate origin, double radAngle)
        {
            IPoint pnt = self as IPoint;
            if (pnt != null)
            {
                RotateCoordinateRad(origin, ref pnt.Coordinate.X, ref pnt.Coordinate.Y, radAngle);
                return;
            }

            IPolygon p = self as IPolygon;
            if (p != null)
            {
                p.Shell.Rotate(origin, radAngle);
                foreach (var h in p.Holes)
                    h.Rotate(origin, radAngle);
                return;
            }

            ILineString l = self as ILineString;
            if (l != null)
            {
                for (int i = 0; i < l.Coordinates.Length; i++)
                    RotateCoordinateRad(origin, ref l.Coordinates[i].X, ref l.Coordinates[i].Y, radAngle);
                return;
            }

            IGeometryCollection geocol = self as IGeometryCollection;
            if (geocol != null)
            {
                foreach (IGeometry geo in geocol.Geometries)
                    geo.Rotate(origin, radAngle);
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
            double x = origin.X + (Math.Cos(radAngle) * (coordX - origin.X) - Math.Sin(radAngle) * (coordY - origin.Y));
            double y = origin.Y + (Math.Sin(radAngle) * (coordX - origin.X) + Math.Cos(radAngle) * (coordY - origin.Y));
            coordX = x;
            coordY = y;
        }

    }
}
