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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 2:34:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// NewZealandMapGrid
    /// </summary>
    public class NewZealandMapGrid : Transform
    {
        #region Private Variables

        private const double SEC5_TO_RAD = 0.4848136811095359935899141023;
        private const double RAD_TO_SEC5 = 2.062648062470963551564733573;
        private const int NBF = 5;
        private const int NTPSI = 9;
        private const int NTPHI = 8;

        private readonly double[][] _bf;
        private readonly double[] _tphi;
        private readonly double[] _tpsi;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NewZealandMapGrid
        /// </summary>
        public NewZealandMapGrid()
        {
            Proj4Name = "nzmg";
            Name = "New_Zealand_Map_Grid";
            _bf = new double[6][];
            _bf[0] = new[] { .7557853228, 0.0 };
            _bf[1] = new[] { .249204646, .003371507 };
            _bf[2] = new[] { -.001541739, .041058560 };
            _bf[3] = new[] { -.10162907, .01727609 };
            _bf[4] = new[] { -.26623489, -.36249218 };
            _bf[5] = new[] { -.6870983, -1.1651967 };

            _tphi = new[] { 1.5627014243, .5185406398, -.03333098, -.1052906, -.0368594, .007317, .01220, .00394, -.0013 };
            _tpsi = new[]
                        {
                            .6399175073, -.1358797613, .063294409, -.02526853, .0117879, -.0055161, .0026906, -.001333,
                            .00067, -.00034
                        };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Ra = 1 / (A = 6378388.0);
            Lam0 = DEG_TO_RAD * 173;
            Phi0 = DEG_TO_RAD * -41;
            X0 = 2510000;
            Y0 = 6023150;
        }

        /// <inheritdoc />
        protected override void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;

                double[] p = new double[2];

                lp[phi] = (lp[phi] - Phi0) * RAD_TO_SEC5;
                p[R] = _tpsi[NTPSI];
                for (int j = NTPSI - 1; j >= 0; j--)
                {
                    p[R] = _tpsi[j] + lp[phi] * p[R];
                }
                p[R] *= lp[phi];
                p[I] = lp[lam];
                p = Proj.Zpoly1(p, _bf, NBF);
                xy[x] = p[I];
                xy[y] = p[R];
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
                int nn;
                double[] p = new double[2];
                double[] dp = new double[2];
                p[R] = xy[y];
                p[I] = xy[x];
                for (nn = 20; nn > 0; --nn)
                {
                    double[] fp;
                    double[] f = Proj.Zpolyd1(p, _bf, NBF, out fp);
                    f[R] -= xy[y];
                    f[I] -= xy[x];
                    double den = fp[R] * fp[R] + fp[I] * fp[I];
                    p[R] += dp[R] = -(f[R] * fp[R] + f[I] * fp[I]) / den;
                    p[I] += dp[I] = -(f[I] * fp[R] - f[R] * fp[I]) / den;
                    if ((Math.Abs(dp[R]) + Math.Abs(dp[I])) <= EPS10)
                        break;
                }
                if (nn > 0)
                {
                    lp[lam] = p[I];
                    lp[phi] = _tphi[NTPHI];
                    for (int j = NTPHI - 1; j >= 0; j--)
                    {
                        lp[phi] = _tphi[j] + p[R] * lp[phi];
                    }
                    lp[phi] = Phi0 + p[R] * lp[phi] * SEC5_TO_RAD;
                }
                else
                {
                    lp[lam] = lp[phi] = double.MaxValue;
                }
            }
        }

        #endregion

        #region Properties

        #endregion
    }
}