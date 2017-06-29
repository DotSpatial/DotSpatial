// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries.
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 1:23:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Krovak
    /// NOTES: According to EPSG the full Krovak projection method should have
    ///          the following parameters.  Within PROJ.4 the azimuth, and pseudo
    ///          standard parallel are hardcoded in the algorithm and can't be
    ///          altered from outside.  The others all have defaults to match the
    ///          common usage with Krovak projection.
    ///
    ///  lat_0 = latitude of centre of the projection
    ///
    ///  lon_0 = longitude of centre of the projection
    ///
    ///  ** = azimuth (true) of the centre line passing through the centre of the projection
    ///
    ///  ** = latitude of pseudo standard parallel
    ///
    ///  k  = scale factor on the pseudo standard parallel
    ///
    ///  x_0 = False Easting of the centre of the projection at the apex of the cone
    ///
    ///  y_0 = False Northing of the centre of the projection at the apex of the cone
    ///
    ///  _czech = specifies the EAST-NORTH GIS usage where the axis are reversed:
    ///  X [S-JTSK KROVAK EAST NORTH] = -Y [KROVAK]
    ///  Y [S-JTSK KROVAK EAST NORTH] = -X [KROVAK]
    /// </summary>
    public class Krovak : Transform
    {
        #region Private Variables

        //private double _cX;
        private bool _czech;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Krovak
        /// </summary>
        public Krovak()
        {
            Proj4Name = "krovak";
            Name = "Krovak";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                /* calculate xy from lat/lon */

                /* Constants, identical to inverse transform function */
                const double s45 = 0.785398163397448;
                const double s90 = 2 * s45;
                double fi0 = Phi0;

                /* Ellipsoid is used as Parameter in for.c and inv.c, therefore a must
                  be set to 1 here.
                  Ellipsoid Bessel 1841 a = 6377397.155m 1/f = 299.1528128,
                     e2=0.006674372230614;
                */

                const double a = 1;
                /* e2 = Es;*/
                /* 0.006674372230614; */
                const double e2 = 0.006674372230614;
                double e = Math.Sqrt(e2);

                double alfa = Math.Sqrt(1 + (e2 * Math.Pow(Math.Cos(fi0), 4)) / (1 - e2));

                const double uq = 1.04216856380474;
                double u0 = Math.Asin(Math.Sin(fi0) / alfa);
                double g = Math.Pow((1 + e * Math.Sin(fi0)) / (1 - e * Math.Sin(fi0)), alfa * e / 2);

                double k = Math.Tan(u0 / 2 + s45) / Math.Pow(Math.Tan(fi0 / 2 + s45), alfa) * g;

                double k1 = K0;
                double n0 = a * Math.Sqrt(1 - e2) / (1 - e2 * Math.Pow(Math.Sin(fi0), 2));
                const double s0 = 1.37008346281555;
                double n = Math.Sin(s0);
                double ro0 = k1 * n0 / Math.Tan(s0);
                const double ad = s90 - uq;

                /* Transformation */

                double gfi = Math.Pow(((1 + e * Math.Sin(lp[phi])) /
                                       (1 - e * Math.Sin(lp[phi]))), (alfa * e / 2));

                double u = 2 * (Math.Atan(k * Math.Pow(Math.Tan(lp[phi] / 2 + s45), alfa) / gfi) - s45);

                double deltav = -lp[lam] * alfa;

                double s = Math.Asin(Math.Cos(ad) * Math.Sin(u) + Math.Sin(ad) * Math.Cos(u) * Math.Cos(deltav));
                double d = Math.Asin(Math.Cos(u) * Math.Sin(deltav) / Math.Cos(s));
                double eps = n * d;
                double ro = ro0 * Math.Pow(Math.Tan(s0 / 2 + s45), n) / Math.Pow(Math.Tan(s / 2 + s45), n);

                /* x and y are reverted! */
                if (!_czech)
                {
                    xy[y] = ro * Math.Sin(eps) / a;
                    xy[x] = ro * Math.Cos(eps) / a;
                }
                else
                {
                    /* in Czech version, Y = -X and X = -Y */
                    xy[x] = -ro * Math.Sin(eps) / a;
                    xy[y] = -ro * Math.Cos(eps) / a;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            /* calculate lat/lon from xy */

            /* Constants, identisch wie in der Umkehrfunktion */
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                const double s45 = 0.785398163397448;
                const double s90 = 2 * s45;
                double fi0 = Phi0;

                /* Ellipsoid is used as Parameter in for.c and inv.c, therefore a must
                   be set to 1 here.
                   Ellipsoid Bessel 1841 a = 6377397.155m 1/f = 299.1528128,
                   e2=0.006674372230614;
                */

                const double a = 1;
                /* e2 = Es; */
                /* 0.006674372230614; */
                const double e2 = 0.006674372230614;
                double e = Math.Sqrt(e2);

                double alfa = Math.Sqrt(1 + (e2 * Math.Pow(Math.Cos(fi0), 4)) / (1 - e2));
                const double uq = 1.04216856380474;
                double u0 = Math.Asin(Math.Sin(fi0) / alfa);
                double g = Math.Pow((1 + e * Math.Sin(fi0)) / (1 - e * Math.Sin(fi0)), alfa * e / 2);

                double k = Math.Tan(u0 / 2 + s45) / Math.Pow(Math.Tan(fi0 / 2 + s45), alfa) * g;

                double k1 = K0;
                double n0 = a * Math.Sqrt(1 - e2) / (1 - e2 * Math.Pow(Math.Sin(fi0), 2));
                const double s0 = 1.37008346281555;
                double n = Math.Sin(s0);
                double ro0 = k1 * n0 / Math.Tan(s0);
                const double ad = s90 - uq;

                /* Transformation */
                /* revert y, x*/
                double xy0 = xy[x];
                xy[x] = xy[y];
                xy[y] = xy0;

                if (_czech)
                {
                    xy[x] *= -1.0;
                    xy[y] *= -1.0;
                }

                double ro = Math.Sqrt(xy[x] * xy[x] + xy[y] * xy[y]);
                double eps = Math.Atan2(xy[y], xy[x]);
                double d = eps / Math.Sin(s0);
                double s = 2 * (Math.Atan(Math.Pow(ro0 / ro, 1 / n) * Math.Tan(s0 / 2 + s45)) - s45);

                double u = Math.Asin(Math.Cos(ad) * Math.Sin(s) - Math.Sin(ad) * Math.Cos(s) * Math.Cos(d));
                double deltav = Math.Asin(Math.Cos(s) * Math.Sin(d) / Math.Cos(u));

                lp[lam] = Lam0 - deltav / alfa;

                /* ITERATION FOR lp[phi] */
                double fi1 = u;

                int ok = 0;
                do
                {
                    lp[phi] = 2 * (Math.Atan(Math.Pow(k, -1 / alfa) *
                                             Math.Pow(Math.Tan(u / 2 + s45), 1 / alfa) *
                                             Math.Pow((1 + e * Math.Sin(fi1)) / (1 - e * Math.Sin(fi1)), e / 2)
                                       ) - s45);

                    if (Math.Abs(fi1 - lp[phi]) < 0.000000000000001) ok = 1;
                    fi1 = lp[phi];
                } while (ok == 0);

                lp[lam] -= Lam0;
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        /// <remarks>The default value of CZECH is true: The X and Y axis are reversed and multiplied by -1 as typically used in GIS applications of Krovak</remarks>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            /* read some Parameters,
             * here Latitude Truescale */
            //double ts = 0;
            //if (projInfo.StandardParallel1 != null) ts = projInfo.StandardParallel1.Value*Math.PI/180;
            //_cX = ts;

            /* we want Bessel as fixed ellipsoid */
            A = 6377397.155;
            E = Math.Sqrt(Es = 0.006674372230614);

            /* if latitude of projection center is not set, use 49d30'N */
            Phi0 = projInfo.LatitudeOfOrigin != null ? projInfo.Phi0 : 0.863937979737193;

            /* if center long is not set use 42d30'E of Ferro - 17d40' for Ferro */
            /* that will correspond to using longitudes relative to greenwich    */
            /* as input and output, instead of lat/long relative to Ferro */
            Lam0 = projInfo.CentralMeridian != null ? projInfo.Lam0 : 0.7417649320975901 - 0.308341501185665;

            /* if scale not set default to 0.9999 */
            K0 = (projInfo.ScaleFactor != 1) ? projInfo.ScaleFactor : 0.9999;

            //Previous BAD CODE (commented out!)
            //K0 = projInfo.CentralMeridian != null ? projInfo.Lam0 : 0.9999;

            if (projInfo.czech.HasValue)
            {
                if (projInfo.czech.Value != 0) _czech = true;
            }
            else
            {
                //By default set Czech to TRUE
                _czech = true;
            }
        }

        #endregion
    }
}