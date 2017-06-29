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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 2:57:42 PM
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
    /// LambertConformalConic
    /// </summary>
    public class LambertConformalConic : Transform
    {
        #region Private Variables

        private double _c;
        private bool _ellipse;
        private double _n;
        private double _phi1;
        private double _phi2;
        private double _rho;
        private double _rho0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Lambert Conformal Conic projection
        /// </summary>
        public LambertConformalConic()
        {
            // Updated per http://dotspatial.codeplex.com/discussions/277125
            Name = "Lambert_Conformal_Conic;Lambert_Conformal_Conic_2SP";
            Proj4Name = "lcc";
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
                if (Math.Abs(Math.Abs(lp[phi]) - HALF_PI) < EPS10)
                {
                    if (lp[phi] * _n <= 0)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    _rho = 0;
                }
                else
                {
                    double cx;
                    if (_ellipse)
                    {
                        cx = Math.Pow(Proj.Tsfn(lp[phi], Math.Sin(lp[phi]), E), _n);
                    }
                    else
                    {
                        cx = Math.Pow(Math.Tan(Math.PI / 4 + .5 * lp[phi]), -_n);
                    }
                    _rho = _c * cx;
                }
                double nLam = lp[lam] * _n;
                xy[x] = K0 * (_rho * Math.Sin(nLam));
                xy[y] = K0 * (_rho0 - _rho * Math.Cos(nLam));
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
                double cx = xy[x] / K0;
                double cy = _rho0 - xy[y] / K0;

                _rho = Math.Sqrt(cx * cx + cy * cy);

                if (_rho != 0.0)
                {
                    if (_n < 0)
                    {
                        _rho = -_rho;
                        cx = -cx;
                        cy = -cy;
                    }
                    if (_ellipse)
                    {
                        double temp = Math.Pow(_rho / _c, 1 / _n);
                        lp[phi] = Proj.Phi2(temp, E);
                        if (lp[phi] == double.MaxValue)
                        {
                            lp[lam] = double.NaN;
                            lp[phi] = double.NaN;
                            continue;
                            //throw new ProjectionException(20);
                        }
                    }
                    else
                    {
                        lp[phi] = 2 * Math.Atan(Math.Pow(_c / _rho, 1 / _n)) - HALF_PI;
                    }
                    lp[lam] = Math.Atan2(cx, cy) / _n;
                }
                else
                {
                    lp[lam] = 0;
                    lp[phi] = _n > 0 ? HALF_PI : -HALF_PI;
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double sinphi;
            _phi1 = projInfo.Phi1;
            if (projInfo.StandardParallel2 != null)
            {
                _phi2 = projInfo.Phi2;
            }
            else
            {
                _phi2 = _phi1;
                _phi1 = projInfo.Phi0;
            }
            if (Math.Abs(_phi1 + _phi2) < EPS10)
            {
                throw new ProjectionException(21);
            }
            _n = sinphi = Math.Sin(_phi1);
            double cosphi = Math.Cos(_phi1);
            bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
            _ellipse = projInfo.GeographicInfo.Datum.Spheroid.IsOblate();
            if (_ellipse)
            {
                double m1 = Proj.Msfn(sinphi, cosphi, Es);
                double ml1 = Proj.Tsfn(_phi1, sinphi, E);
                if (secant)
                {
                    sinphi = Math.Sin(_phi2);
                    _n = Math.Log(m1 / Proj.Msfn(sinphi, Math.Cos(_phi2), Es));
                    _n = _n / Math.Log(ml1 / Proj.Tsfn(_phi2, sinphi, E));
                }
                _rho0 = m1 * Math.Pow(ml1, -_n) / _n;
                _c = _rho0;
                if (Math.Abs(Math.Abs(Phi0) - HALF_PI) < EPS10)
                {
                    _rho0 = 0;
                }
                else
                {
                    _rho0 *= Math.Pow(Proj.Tsfn(Phi0, Math.Sin(Phi0), E), _n);
                }
            }
            else
            {
                if (secant)
                {
                    _n = Math.Log(cosphi / Math.Cos(_phi2)) /
                         Math.Log(Math.Tan(Math.PI / 4 + .5 * _phi2) /
                                  Math.Tan(Math.PI / 4 + .5 * _phi1));
                    _c = cosphi * Math.Pow(Math.Tan(Math.PI / 4 + .5 * _phi1), _n) / _n;
                }
                if (Math.Abs(Math.Abs(Phi0) - HALF_PI) < EPS10)
                {
                    _rho0 = 0;
                }
                else
                {
                    _rho0 = _c * Math.Pow(Math.Tan(Math.PI / 4 + .5 * Phi0), -_n);
                }
            }
        }

        /// <summary>
        /// Special factor calculations for a factors calculation
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection</param>
        /// <param name="fac">The Factors</param>
        protected override void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            if (Math.Abs(Math.Abs(lp[PHI]) - HALF_PI) < EPS10)
            {
                if ((lp[PHI] * _n) <= 0) return;
                _rho = 0;
            }
            else
            {
                _rho = _c * (_ellipse ? Math.Pow(Proj.Tsfn(lp[PHI], Math.Sin(lp[PHI]),
                                                           p.GeographicInfo.Datum.Spheroid.Eccentricity()), _n)
                                 : Math.Pow(Math.Tan(Math.PI / 4 + .5 * lp[PHI]), -_n));
            }
            fac.Code = AnalyticModes.IsAnalHk | AnalyticModes.IsAnalConv;
            fac.K = fac.H = p.ScaleFactor * _n * _rho / Proj.Msfn(Math.Sin(lp[PHI]), Math.Cos(lp[PHI]), p.GeographicInfo.Datum.Spheroid.EccentricitySquared());
            fac.Conv = -_n * lp[LAMBDA];
        }

        #endregion
    }
}