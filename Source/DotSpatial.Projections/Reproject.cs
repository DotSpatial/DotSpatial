// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 9:10:23 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Reproject
    /// </summary>
    public static class Reproject
    {
        #region Private Variables

        private const double EPS = 1E-12;

        #endregion

        #region Methods

        /// <summary>
        /// This method reprojects the affine transform coefficients.  This is used for projection on the fly,
        /// where image transforms can take advantage of an affine projection, but does not have the power of
        /// a full projective transform and gets less and less accurate as the image covers larger and larger
        /// areas.  Since most image layers represent small rectangular areas, this should not be a problem in
        /// most cases.  the affine array should be ordered as follows:
        /// X' = [0] + [1] * Column + [2] * Row
        /// Y' = [3] + [4] * Column + [5] * Row
        /// </summary>
        /// <param name="affine">The array of double affine coefficients.</param>
        /// <param name="numRows">The number of rows to use for the lower bounds.  Value of 0 or less will be set to 1.</param>
        /// <param name="numCols">The number of columns to use to get the right bounds.  Values of 0 or less will be set to 1.</param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>The transformed coefficients</returns>
        public static double[] ReprojectAffine(double[] affine, double numRows, double numCols, ProjectionInfo source, ProjectionInfo dest)
        {
            if (numRows <= 0) numRows = 1;
            if (numCols <= 0) numCols = 1;

            double[] vertices = new double[6];
            // Top left
            vertices[0] = affine[0];
            vertices[1] = affine[3];
            // Top right
            vertices[2] = affine[0] + affine[1] * numCols;
            vertices[3] = affine[3] + affine[4] * numCols;
            // Bottom Left
            vertices[4] = affine[0] + affine[2] * numRows;
            vertices[5] = affine[3] + affine[5] * numRows;
            double[] z = new double[3];
            ReprojectPoints(vertices, z, source, dest, 0, 3);
            double[] affineResult = new double[6];

            affineResult[0] = vertices[0];
            affineResult[1] = (vertices[2] - vertices[0]) / numCols;
            affineResult[2] = (vertices[4] - vertices[0]) / numRows;

            affineResult[3] = vertices[1];
            affineResult[4] = (vertices[3] - vertices[1]) / numCols;
            affineResult[5] = (vertices[5] - vertices[1]) / numRows;

            return affineResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xy">The xy array should be in interleaved set of xy coordinates like [x1, y1, x2, y2, ... xn, yn]</param>
        /// <param name="z">The z array is the array of all the z values</param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public static void ReprojectPoints(double[] xy, double[] z, ProjectionInfo source, ProjectionInfo dest, int startIndex, int numPoints)
        {
            ReprojectPoints(xy, z, source, 1.0, dest, 1.0, null, startIndex, numPoints);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xy">The xy array should be in interleaved set of xy coordinates like [x1, y1, x2, y2, ... xn, yn]</param>
        /// <param name="z">The z array is the array of all the z values</param>
        /// <param name="source"></param>
        /// <param name="srcZtoMeter"></param>
        /// <param name="dest"></param>
        /// <param name="dstZtoMeter"></param>
        /// <param name="idt"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public static void ReprojectPoints(double[] xy, double[] z, ProjectionInfo source, double srcZtoMeter, ProjectionInfo dest, double dstZtoMeter, IDatumTransform idt, int startIndex, int numPoints)
        {
            double toMeter = source.Unit.Meters;

            // Geocentric coordinates are centered at the core of the earth.  Z is up toward the north pole.
            // The X axis goes from the center of the earth through Greenwich.
            // The Y axis passes through 90E.
            // This section converts from geocentric coordinates to geodetic ones if necessary.
            if (source.IsGeocentric)
            {
                if (z == null)
                {
                    throw new ProjectionException(45);
                }
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (toMeter != 1)
                    {
                        xy[i * 2] *= toMeter;
                        xy[i * 2 + 1] *= toMeter;
                    }
                }
                GeocentricGeodetic g = new GeocentricGeodetic(source.GeographicInfo.Datum.Spheroid);
                g.GeocentricToGeodetic(xy, z, startIndex, numPoints);
            }

            // Transform source points to lam/phi if they are not already
            ConvertToLatLon(source, xy, z, srcZtoMeter, startIndex, numPoints);

            double fromGreenwich = source.GeographicInfo.Meridian.Longitude * source.GeographicInfo.Unit.Radians;
            if (fromGreenwich != 0)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (xy[2 * i] != double.PositiveInfinity)
                        xy[2 * i] += fromGreenwich;
                }
            }
            // DATUM TRANSFORM IF NEEDED
            if (idt == null)
            {
                if (!source.GeographicInfo.Datum.Matches(dest.GeographicInfo.Datum))
                {
                    DatumTransform(source, dest, xy, z, startIndex, numPoints);
                }
            }
            else
            {
                idt.Transform(source, dest, xy, z, startIndex, numPoints);
            }

            // Adjust to new prime meridian if there is one in the destination cs
            fromGreenwich = dest.GeographicInfo.Meridian.Longitude * dest.GeographicInfo.Unit.Radians;
            if (fromGreenwich != 0)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (xy[i * 2] != double.PositiveInfinity)
                    {
                        xy[i * 2] -= fromGreenwich;
                    }
                }
            }

            if (dest.IsGeocentric)
            {
                if (z == null)
                {
                    throw new ProjectionException(45);
                }
                GeocentricGeodetic g = new GeocentricGeodetic(dest.GeographicInfo.Datum.Spheroid);
                g.GeodeticToGeocentric(xy, z, startIndex, numPoints);
                double frmMeter = 1 / dest.Unit.Meters;
                if (frmMeter != 1)
                {
                    for (int i = startIndex; i < numPoints; i++)
                    {
                        if (xy[i * 2] != double.PositiveInfinity)
                        {
                            xy[i * 2] *= frmMeter;
                            xy[i * 2 + 1] *= frmMeter;
                        }
                    }
                }
            }
            else
            {
                ConvertToProjected(dest, xy, z, dstZtoMeter, startIndex, numPoints);
            }
        }

        private static void ConvertToProjected(ProjectionInfo dest, double[] xy, double[] z, double dstZtoMeter, int startIndex, int numPoints)
        {
            double frmMeter = 1 / dest.Unit.Meters;
            double frmZMeter = 1 / dstZtoMeter;
            bool geoc = dest.Geoc;
            double lam0 = dest.Lam0;
            double roneEs = 1 / (1 - dest.GeographicInfo.Datum.Spheroid.EccentricitySquared());
            bool over = dest.Over;
            double x0 = 0;
            double y0 = 0;
            if (dest.FalseEasting.HasValue) x0 = dest.FalseEasting.Value;
            if (dest.FalseNorthing.HasValue) y0 = dest.FalseNorthing.Value;
            double a = dest.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            for (int i = startIndex; i < numPoints; i++)
            {
                double lam = xy[2 * i];
                double phi = xy[2 * i + 1];
                double t = Math.Abs(phi) - Math.PI / 2;
                if (t > EPS || Math.Abs(lam) > 10)
                {
                    xy[2 * i] = double.PositiveInfinity;
                    xy[2 * i + 1] = double.PositiveInfinity;
                    continue;
                }
                if (Math.Abs(t) <= EPS)
                {
                    xy[2 * i + 1] = phi < 0 ? -Math.PI / 2 : Math.PI / 2;
                }
                else if (geoc)
                {
                    xy[2 * i + 1] = Math.Atan(roneEs * Math.Tan(phi));
                }
                xy[2 * i] -= lam0;
                if (!over)
                {
                    xy[2 * i] = Adjlon(xy[2 * i]);
                }
            }

            // break this out because we don't want a chatty call to extension transforms
            dest.Transform.Forward(xy, startIndex, numPoints);

            if (dstZtoMeter == 1.0)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    xy[2 * i] = frmMeter * (a * xy[2 * i] + x0);
                    xy[2 * i + 1] = frmMeter * (a * xy[2 * i + 1] + y0);
                }
            }
            else
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    xy[2 * i] = frmMeter * (a * xy[2 * i] + x0);
                    xy[2 * i + 1] = frmMeter * (a * xy[2 * i + 1] + y0);
                    z[i] *= frmZMeter;
                }
            }
        }

        private static void DatumTransform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints)
        {
            Spheroid wgs84 = new Spheroid(Proj4Ellipsoid.WGS_1984);
            Datum sDatum = source.GeographicInfo.Datum;
            Datum dDatum = dest.GeographicInfo.Datum;

            /* -------------------------------------------------------------------- */
            /*      We cannot do any meaningful datum transformation if either      */
            /*      the source or destination are of an unknown datum type          */
            /*      (ie. only a +ellps declaration, no +datum).  This is new        */
            /*      behavior for PROJ 4.6.0.                                        */
            /* -------------------------------------------------------------------- */
            if (sDatum.DatumType == DatumType.Unknown ||
                dDatum.DatumType == DatumType.Unknown) return;

            /* -------------------------------------------------------------------- */
            /*      Short cut if the datums are identical.                          */
            /* -------------------------------------------------------------------- */

            if (sDatum.Matches(dDatum)) return;

            // proj4 actually allows some tollerance here
            if (sDatum.DatumType == dDatum.DatumType)
            {
                if (sDatum.Spheroid.EquatorialRadius == dDatum.Spheroid.EquatorialRadius)
                {
                    if (Math.Abs(sDatum.Spheroid.EccentricitySquared() - dDatum.Spheroid.EccentricitySquared()) < 0.000000000050)
                    {
                        // The tolerence is to allow GRS80 and WGS84 to escape without being transformed at all.
                        return;
                    }
                }
            }

            double srcA = sDatum.Spheroid.EquatorialRadius;
            double srcEs = sDatum.Spheroid.EccentricitySquared();

            double dstA = dDatum.Spheroid.EquatorialRadius;
            double dstEs = dDatum.Spheroid.EccentricitySquared();

            /* -------------------------------------------------------------------- */
            /*      Create a temporary Z value if one is not provided.              */
            /* -------------------------------------------------------------------- */
            if (z == null)
            {
                z = new double[xy.Length / 2];
            }

            /* -------------------------------------------------------------------- */
            /*	If this datum requires grid shifts, then apply it to geodetic   */
            /*      coordinates.                                                    */
            /* -------------------------------------------------------------------- */
            if (sDatum.DatumType == DatumType.GridShift)
            {
                //        pj_apply_gridshift(pj_param(srcdefn->params,"snadgrids").s, 0,
                //                            point_count, point_offset, x, y, z );

                GridShift.Apply(source.GeographicInfo.Datum.NadGrids, false, xy, startIndex, numPoints);

                srcA = wgs84.EquatorialRadius;
                srcEs = wgs84.EccentricitySquared();
            }

            if (dDatum.DatumType == DatumType.GridShift)
            {
                dstA = wgs84.EquatorialRadius;
                dstEs = wgs84.EccentricitySquared();
            }

            /* ==================================================================== */
            /*      Do we need to go through geocentric coordinates?                */
            /* ==================================================================== */

            if (srcEs != dstEs || srcA != dstA
                || sDatum.DatumType == DatumType.Param3
                || sDatum.DatumType == DatumType.Param7
                || dDatum.DatumType == DatumType.Param3
                || dDatum.DatumType == DatumType.Param7)
            {
                /* -------------------------------------------------------------------- */
                /*      Convert to geocentric coordinates.                              */
                /* -------------------------------------------------------------------- */

                GeocentricGeodetic gc = new GeocentricGeodetic(sDatum.Spheroid);
                gc.GeodeticToGeocentric(xy, z, startIndex, numPoints);

                /* -------------------------------------------------------------------- */
                /*      Convert between datums.                                         */
                /* -------------------------------------------------------------------- */

                if (sDatum.DatumType == DatumType.Param3 || sDatum.DatumType == DatumType.Param7)
                {
                    PjGeocentricToWgs84(source, xy, z, startIndex, numPoints);
                }

                if (dDatum.DatumType == DatumType.Param3 || dDatum.DatumType == DatumType.Param7)
                {
                    PjGeocentricFromWgs84(dest, xy, z, startIndex, numPoints);
                }

                /* -------------------------------------------------------------------- */
                /*      Convert back to geodetic coordinates.                           */
                /* -------------------------------------------------------------------- */

                gc = new GeocentricGeodetic(dDatum.Spheroid);
                gc.GeocentricToGeodetic(xy, z, startIndex, numPoints);
            }

            /* -------------------------------------------------------------------- */
            /*      Apply grid shift to destination if required.                    */
            /* -------------------------------------------------------------------- */
            if (dDatum.DatumType == DatumType.GridShift)
            {
                //        pj_apply_gridshift(pj_param(dstdefn->params,"snadgrids").s, 1,
                //                            point_count, point_offset, x, y, z );
                GridShift.Apply(dest.GeographicInfo.Datum.NadGrids, true, xy, startIndex, numPoints);
            }
        }

        private static void ConvertToLatLon(ProjectionInfo source, double[] xy, double[] z, double srcZtoMeter, int startIndex, int numPoints)
        {
            double toMeter = 1.0;
            if (source.Unit != null) toMeter = source.Unit.Meters;
            double oneEs = 1 - source.GeographicInfo.Datum.Spheroid.EccentricitySquared();
            double ra = 1 / source.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            double x0 = 0;
            if (source.FalseEasting != null) x0 = source.FalseEasting.Value;
            double y0 = 0;
            if (source.FalseNorthing != null) y0 = source.FalseNorthing.Value;
            if (srcZtoMeter == 1.0)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (xy[i * 2] == double.PositiveInfinity || xy[i * 2 + 1] == double.PositiveInfinity)
                    {
                        // This might be error worthy, but not sure if we want to throw an exception here
                        continue;
                    }
                    // descale and de-offset
                    xy[i * 2] = (xy[i * 2] * toMeter - x0) * ra;
                    xy[i * 2 + 1] = (xy[i * 2 + 1] * toMeter - y0) * ra;
                }
            }
            else
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (xy[i * 2] == double.PositiveInfinity || xy[i * 2 + 1] == double.PositiveInfinity)
                    {
                        // This might be error worthy, but not sure if we want to throw an exception here
                        continue;
                    }
                    // descale and de-offset
                    xy[i * 2] = (xy[i * 2] * toMeter - x0) * ra;
                    xy[i * 2 + 1] = (xy[i * 2 + 1] * toMeter - y0) * ra;
                    z[i] *= srcZtoMeter;
                }
            }

            if (source.Transform != null)
            {
                source.Transform.Inverse(xy, startIndex, numPoints);
            }

            for (int i = startIndex; i < numPoints; i++)
            {
                double lam0 = source.Lam0;
                xy[i * 2] += lam0;
                if (!source.Over)
                {
                    xy[i*2] = Adjlon(xy[i*2]);
                }
                if (source.Geoc && Math.Abs(Math.Abs(xy[i * 2 + 1]) - Math.PI / 2) > EPS)
                {
                    xy[i * 2 + 1] = Math.Atan(oneEs * Math.Tan(xy[i * 2 + 1]));
                }
            }
        }

        private static double Adjlon(double lon)
        {
            if (Math.Abs(lon) <= Math.PI + Math.PI / 72) return (lon);
            lon += Math.PI;  /* adjust to 0..2pi rad */
            lon -= 2 * Math.PI * Math.Floor(lon / (2 * Math.PI)); /* remove integral # of 'revolutions'*/
            lon -= Math.PI;  /* adjust back to -pi..pi rad */
            return (lon);
        }

        private static void PjGeocentricToWgs84(ProjectionInfo source, double[] xy, double[] zArr, int startIndex, int numPoints)
        {
            double[] shift = source.GeographicInfo.Datum.ToWGS84;

            if (source.GeographicInfo.Datum.DatumType == DatumType.Param3)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;
                    xy[2 * i] = xy[2 * i] + shift[0]; // dx
                    xy[2 * i + 1] = xy[2 * i + 1] + shift[1]; // dy
                    zArr[i] = zArr[i] + shift[2];
                }
            }
            else
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;

                    double x = xy[2 * i];
                    double y = xy[2 * i + 1];
                    double z = zArr[i];
                    xy[2 * i] = shift[6] * (x - shift[5] * y + shift[4] * z) + shift[0];
                    xy[2 * i + 1] = shift[6] * (shift[5] * x + y - shift[3] * z) + shift[1];
                    zArr[i] = shift[6] * (-shift[4] * x + shift[3] * y + z) + shift[2];
                }
            }
        }

        private static void PjGeocentricFromWgs84(ProjectionInfo dest, double[] xy, double[] zArr, int startIndex, int numPoints)
        {
            double[] shift = dest.GeographicInfo.Datum.ToWGS84;

            if (dest.GeographicInfo.Datum.DatumType == DatumType.Param3)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;
                    xy[2 * i] = xy[2 * i] - shift[0]; // dx
                    xy[2 * i + 1] = xy[2 * i + 1] - shift[1]; // dy
                    zArr[i] = zArr[i] - shift[2];
                }
            }
            else
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;

                    double x = (xy[2 * i] - shift[0]) / shift[6];
                    double y = (xy[2 * i + 1] - shift[1]) / shift[6];
                    double z = (zArr[i] - shift[2]) / shift[6];
                    xy[2 * i] = x + shift[5] * y - shift[4] * z;
                    xy[2 * i + 1] = -shift[5] * x + y + shift[3] * z;
                    zArr[i] = shift[4] * x - shift[3] * y + z;
                }
            }
        }

        #endregion

        #region Properties

        #endregion
    }
}