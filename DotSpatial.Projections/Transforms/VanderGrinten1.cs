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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 9:07:59 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// VanderGrinten1
    /// </summary>
    public class VanderGrinten1 : Transform
    {
        #region Private Variables

        private const double TOL = 1E-10;
        private const double THIRD = .33333333333333333333;
        private const double C227 = .07407407407407407407;
        private const double PI43 = 4.18879020478639098458;
        private const double PISQ = 9.86960440108935861869;
        private const double TPISQ = 19.73920880217871723738;
        private const double HPISQ = 4.93480220054467930934;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of VanderGrinten1
        /// </summary>
        public VanderGrinten1()
        {
            Proj4Name = "vandg";
            Name = "Van_der_Grinten_I";
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
                double p2 = Math.Abs(lp[phi] / HALF_PI);
                if ((p2 - TOL) > 1)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //ProjectionException(20);
                }
                if (p2 > 1)
                    p2 = 1;
                if (Math.Abs(lp[phi]) <= TOL)
                {
                    xy[x] = lp[lam];
                    xy[y] = 0;
                }
                else if (Math.Abs(lp[lam]) <= TOL || Math.Abs(p2 - 1) < TOL)
                {
                    xy[x] = 0;
                    xy[y] = Math.PI * Math.Tan(.5 * Math.Asin(p2));
                    if (lp[phi] < 0) xy[y] = -xy[y];
                }
                else
                {
                    double al = .5 * Math.Abs(Math.PI / lp[lam] - lp[lam] / Math.PI);
                    double al2 = al * al;
                    double g = Math.Sqrt(1 - p2 * p2);
                    g = g / (p2 + g - 1);
                    double g2 = g * g;
                    p2 = g * (2 / p2 - 1);
                    p2 = p2 * p2;
                    xy[x] = g - p2;
                    g = p2 + al2;
                    xy[x] = Math.PI * (al * xy[x] + Math.Sqrt(al2 * xy[x] * xy[x] - g * (g2 - p2))) / g;
                    if (lp[lam] < 0) xy[x] = -xy[x];
                    xy[y] = Math.Abs(xy[x] / Math.PI);
                    xy[y] = 1 - xy[y] * (xy[y] + 2 * al);
                    if (xy[y] < -TOL)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    if (xy[y] < 0) xy[y] = 0;
                    else xy[y] = Math.Sqrt(xy[y]) * (lp[phi] < 0 ? -Math.PI : Math.PI);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double t, ay;

                double x2 = xy[x] * xy[x];
                if ((ay = Math.Abs(xy[y])) < TOL)
                {
                    lp[phi] = 0;
                    t = x2 * x2 + TPISQ * (x2 + HPISQ);
                    lp[lam] = Math.Abs(xy[x]) <= TOL ? 0 : .5 * (x2 - PISQ + Math.Sqrt(t)) / xy[x];
                    return;
                }
                double y2 = xy[y] * xy[y];
                double r = x2 + y2;
                double r2 = r * r;
                double c1 = -Math.PI * ay * (r + PISQ);
                double c3 = r2 + TWO_PI * (ay * r + Math.PI * (y2 + Math.PI * (ay + HALF_PI)));
                double c2 = c1 + PISQ * (r - 3 * y2);
                double c0 = Math.PI * ay;
                c2 /= c3;
                double al = c1 / c3 - THIRD * c2 * c2;
                double m = 2 * Math.Sqrt(-THIRD * al);
                double d = C227 * c2 * c2 * c2 + (c0 * c0 - THIRD * c2 * c1) / c3;
                if (((t = Math.Abs(d = 3 * d / (al * m))) - TOL) > 1)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //ProjectionException(20);
                }
                d = t > 1 ? (d > 0 ? 0 : Math.PI) : Math.Acos(d);
                lp[phi] = Math.PI * (m * Math.Cos(d * THIRD + PI43) - THIRD * c2);
                if (xy[y] < 0) lp[phi] = -lp[phi];
                t = r2 + PISQ * (x2 - y2 + HPISQ);
                lp[lam] = Math.Abs(xy[x]) <= TOL ? 0 : .5 * (r - PISQ + (t <= 0 ? 0 : Math.Sqrt(t))) / xy[x];
            }
        }

        #endregion
    }
}