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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 2:42:59 PM
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
    /// BipolarObliqueConformalConic
    /// </summary>
    public class BipolarObliqueConformalConic : Transform
    {
        #region Private Variables

        private const double EPS = 1e-10;
        private const double ONEEPS = 1.000000001;
        private const int NITER = 10;
        private const double LAM_B = -.34894976726250681539;
        private const double N = .63055844881274687180;
        private const double F = 1.89724742567461030582;
        private const double AZAB = .81650043674686363166;
        private const double AZBA = 1.82261843856185925133;
        private const double T = 1.27246578267089012270;
        private const double RHOC = 1.20709121521568721927;
        private const double C_AZC = .69691523038678375519;
        private const double S_AZC = .71715351331143607555;
        private const double C45 = .70710678118654752469;
        private const double S45 = .70710678118654752410;
        private const double C20 = .93969262078590838411;
        private const double S20 = -.34202014332566873287;
        private const double R110 = 1.91986217719376253360;
        private const double R104 = 1.81514242207410275904;

        private bool _noskew;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BipolarObliqueConformalConic
        /// </summary>
        public BipolarObliqueConformalConic()
        {
            Proj4Name = "bipc";
            Name = "Bipolar_Oblique_Conformal_Conic";
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
                double tphi, t, al, az, z, av, sdlam;

                double cphi = Math.Cos(lp[phi]);
                double sphi = Math.Sin(lp[phi]);
                double cdlam = Math.Cos(sdlam = LAM_B - lp[lam]);
                sdlam = Math.Sin(sdlam);
                if (Math.Abs(Math.Abs(lp[phi]) - HALF_PI) < EPS10)
                {
                    az = lp[phi] < 0 ? Math.PI : 0;
                    tphi = double.MaxValue;
                }
                else
                {
                    tphi = sphi / cphi;
                    az = Math.Atan2(sdlam, C45 * (tphi - cdlam));
                }
                bool tag = az > AZBA;
                if (tag)
                {
                    cdlam = Math.Cos(sdlam = lp[lam] + R110);
                    sdlam = Math.Sin(sdlam);
                    z = S20 * sphi + C20 * cphi * cdlam;
                    if (Math.Abs(z) > 1)
                    {
                        if (Math.Abs(z) > ONEEPS)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //throw new ProjectionException(20);
                        }
                        z = z < 0 ? -1 : 1;
                    }
                    else
                        z = Math.Acos(z);
                    if (tphi != double.MaxValue)
                        az = Math.Atan2(sdlam, (C20 * tphi - S20 * cdlam));
                    av = AZAB;
                    xy[y] = RHOC;
                }
                else
                {
                    z = S45 * (sphi + cphi * cdlam);
                    if (Math.Abs(z) > 1)
                    {
                        if (Math.Abs(z) > ONEEPS)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //throw new ProjectionException(20);
                        }
                        z = z < 0 ? -1 : 1;
                    }
                    else
                        z = Math.Acos(z);
                    av = AZBA;
                    xy[y] = -RHOC;
                }
                if (z < 0)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                double r = F * (t = Math.Pow(Math.Tan(.5 * z), N));
                if ((al = .5 * (R104 - z)) < 0)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                al = (t + Math.Pow(al, N)) / T;
                if (Math.Abs(al) > 1)
                {
                    if (Math.Abs(al) > ONEEPS)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    al = al < 0 ? -1 : 1;
                }
                else
                    al = Math.Acos(al);
                if (Math.Abs(t = N * (av - az)) < al)
                    r /= Math.Cos(al + (tag ? t : -t));
                xy[x] = r * Math.Sin(t);
                xy[y] += (tag ? -r : r) * Math.Cos(t);
                if (_noskew)
                {
                    t = xy[x];
                    xy[x] = -xy[x] * C_AZC - xy[y] * S_AZC;
                    xy[y] = -xy[y] * C_AZC + t * S_AZC;
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
                double r, rp;
                double z = 0, az, s, c, av;
                int j;

                if (_noskew)
                {
                    double t = xy[x];
                    xy[x] = -xy[x] * C_AZC + xy[y] * S_AZC;
                    xy[y] = -xy[y] * C_AZC - t * S_AZC;
                }
                bool neg = (xy[x] < 0);
                if (neg)
                {
                    xy[y] = RHOC - xy[y];
                    s = S20;
                    c = C20;
                    av = AZAB;
                }
                else
                {
                    xy[y] += RHOC;
                    s = S45;
                    c = C45;
                    av = AZBA;
                }
                double rl = rp = r = Proj.Hypot(xy[x], xy[y]);
                double fAz = Math.Abs(az = Math.Atan2(xy[x], xy[y]));
                for (j = NITER; j > 0; --j)
                {
                    z = 2 * Math.Atan(Math.Pow(r / F, 1 / N));
                    double al = Math.Acos((Math.Pow(Math.Tan(.5 * z), N) +
                                           Math.Pow(Math.Tan(.5 * (R104 - z)), N)) / T);
                    if (fAz < al)
                        r = rp * Math.Cos(al + (neg ? az : -az));
                    if (Math.Abs(rl - r) < EPS)
                        break;
                    rl = r;
                }
                if (j == 0)
                {
                    lp[phi] = double.NaN;
                    lp[lam] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                az = av - az / N;
                lp[phi] = Math.Asin(s * Math.Cos(z) + c * Math.Sin(z) * Math.Cos(az));
                lp[lam] = Math.Atan2(Math.Sin(az), c / Math.Tan(z) - s * Math.Cos(az));
                if (neg)
                    lp[lam] -= R110;
                else
                    lp[lam] = LAM_B - lp[lam];
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.bns.HasValue)
            {
                if (projInfo.bns != 0)
                    _noskew = true;
            }
        }

        #endregion
    }
}