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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:18:14 AM
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
    /// GeostationarySatellite
    /// </summary>
    public class GeostationarySatellite : EllipticalTransform
    {
        #region Private Variables

        private double _c;
        private double _h;
        private double _radiusG;
        private double _radiusG1;
        private double _radiusP;
        private double _radiusP2;
        private double _radiusPInv2;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeostationarySatellite
        /// </summary>
        public GeostationarySatellite()
        {
            Name = "Geostationary_Satellite";
            Proj4Name = "geos";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void SphericalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                /* Calculation of the three components of the vector from satellite to
                    ** position on earth surface (lon, lat).*/
                double tmp = Math.Cos(lp[phi]);
                double vx = Math.Cos(lp[lam]) * tmp;
                double vy = Math.Sin(lp[lam]) * tmp;
                double vz = Math.Sin(lp[phi]);

                /* Check visibility.*/
                if (((_radiusG - vx) * vx - vy * vy - vz * vz) < 0)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }

                /* Calculation based on view angles from satellite.*/
                tmp = _radiusG - vx;
                xy[x] = _radiusG1 * Math.Atan(vy / tmp);
                xy[y] = _radiusG1 * Math.Atan(vz / Proj.Hypot(vy, tmp));
            }
        }

        /// <inheritdoc />
        protected override void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                /* Calculation of geocentric latitude. */
                lp[phi] = Math.Atan(_radiusP2 * Math.Tan(lp[phi]));

                /* Calculation of the three components of the vector from satellite to
                    ** position on earth surface (lon, lat).*/
                double r = (_radiusP) / Proj.Hypot(_radiusP * Math.Cos(lp[phi]), Math.Sin(lp[phi]));
                double vx = r * Math.Cos(lp[lam]) * Math.Cos(lp[phi]);
                double vy = r * Math.Sin(lp[lam]) * Math.Cos(lp[phi]);
                double vz = r * Math.Sin(lp[phi]);

                /* Check visibility. */
                if (((_radiusG - vx) * vx - vy * vy - vz * vz * _radiusPInv2) < 0)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    // throw new ProjectionException(20);
                }

                /* Calculation based on view angles from satellite. */
                double tmp = _radiusG - vx;
                xy[x] = _radiusG1 * Math.Atan(vy / tmp);
                xy[y] = _radiusG1 * Math.Atan(vz / Proj.Hypot(vy, tmp));
            }
        }

        /// <inheritdoc />
        protected override void SphericalInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double det;

                /* Setting three components of vector from satellite to position.*/
                double vx = -1.0;
                double vy = Math.Tan(xy[x] / (_radiusG - 1.0));
                double vz = Math.Tan(xy[y] / (_radiusG - 1.0)) * Math.Sqrt(1.0 + vy * vy);

                /* Calculation of terms in cubic equation and determinant.*/
                double a = vy * vy + vz * vz + vx * vx;
                double b = 2 * _radiusG * vx;
                if ((det = (b * b) - 4 * a * _c) < 0)
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    // throw new ProjectionException(20);
                }

                /* Calculation of three components of vector from satellite to position.*/
                double k = (-b - Math.Sqrt(det)) / (2 * a);
                vx = _radiusG + k * vx;
                vy *= k;
                vz *= k;

                /* Calculation of longitude and latitude.*/
                lp[lam] = Math.Atan2(vy, vx);
                lp[phi] = Math.Atan(vz * Math.Cos(lp[lam]) / vx);
            }
        }

        /// <inheritdoc />
        protected override void EllipticalInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double det;

                /* Setting three components of vector from satellite to position.*/
                double vx = -1.0;
                double vy = Math.Tan(xy[x] / _radiusG1);
                double vz = Math.Tan(xy[y] / _radiusG1) * Proj.Hypot(1.0, vy);

                /* Calculation of terms in cubic equation and determinant.*/
                double a = vz / _radiusP;
                a = vy * vy + a * a + vx * vx;
                double b = 2 * _radiusG * vx;
                if ((det = (b * b) - 4 * a * _c) < 0)
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }

                /* Calculation of three components of vector from satellite to position.*/
                double k = (-b - Math.Sqrt(det)) / (2 * a);
                vx = _radiusG + k * vx;
                vy *= k;
                vz *= k;

                /* Calculation of longitude and latitude.*/
                lp[lam] = Math.Atan2(vy, vx);
                lp[phi] = Math.Atan(vz * Math.Cos(lp[lam]) / vx);
                lp[phi] = Math.Atan(_radiusPInv2 * Math.Tan(lp[phi]));
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if ((_h = projInfo.h.Value) <= 0) throw new ProjectionException(-30);
            if (Phi0 == 0) throw new ProjectionException(-46);
            _radiusG = 1 + (_radiusG1 = _h / A);
            _c = _radiusG * _radiusG - 1.0;
            if (IsElliptical)
            {
                _radiusP = Math.Sqrt(OneEs);
                _radiusP2 = OneEs;
                _radiusPInv2 = ROneEs;
            }
            else
            {
                _radiusP = _radiusP2 = _radiusPInv2 = 1.0;
            }
        }

        #endregion
    }
}