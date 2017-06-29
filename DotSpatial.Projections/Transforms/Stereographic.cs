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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/11/2009 10:18:01 AM
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
    /// Stereographic
    /// </summary>
    public class Stereographic : EllipticalTransform
    {
        #region Private Variables

        private const double TOL = 1E-8;
        private const int NITER = 8;
        private const double CONV = 1E-10;
        private double _akm1;
        private double _cosX1;
        private Modes _mode;
        private double _phits;
        private double _sinX1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Stereographic
        /// </summary>
        public Stereographic()
        {
            Name = "Stereographic";
            Proj4Name = "stere";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double sinX = 0.0, cosX = 0.0;

                double coslam = Math.Cos(lp[lam]);
                double sinlam = Math.Sin(lp[lam]);
                double sinphi = Math.Sin(lp[phi]);
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    double cx;
                    sinX = Math.Sin(cx = 2 * Math.Atan(Ssfn(lp[phi], sinphi, E)) - HALF_PI);
                    cosX = Math.Cos(cx);
                }
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    double a;
                    if (_mode == Modes.Oblique)
                    {
                        a = _akm1 / (_cosX1 * (1 + _sinX1 * sinX +
                                               _cosX1 * cosX * coslam));
                        xy[y] = a * (_cosX1 * sinX - _sinX1 * cosX * coslam);
                    }
                    else
                    {
                        a = 2 * _akm1 / (1 + cosX * coslam);
                        xy[y] = a * sinX;
                    }
                    xy[x] = a * cosX;
                }
                else
                {
                    if (_mode == Modes.SouthPole)
                    {
                        lp[phi] = -lp[phi];
                        coslam = -coslam;
                        sinphi = -sinphi;
                    }
                    xy[x] = _akm1 * Proj.Tsfn(lp[phi], sinphi, E);
                    xy[y] = -xy[x] * coslam;
                }
                xy[x] = xy[x] * sinlam;
            }
        }

        /// <inheritdoc />
        protected override void SphericalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double sinphi = Math.Sin(lp[phi]);
                double cosphi = Math.Cos(lp[phi]);
                double coslam = Math.Cos(lp[lam]);
                double sinlam = Math.Sin(lp[lam]);
                if (_mode == Modes.Equitorial || _mode == Modes.Oblique)
                {
                    if (_mode == Modes.Equitorial)
                    {
                        xy[y] = 1 + cosphi * coslam;
                    }
                    else
                    {
                        xy[y] = 1 + _sinX1 * sinphi + _cosX1 * cosphi * coslam;
                    }
                    if (xy[y] <= EPS10)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    xy[x] = (xy[y] = _akm1 / xy[y]) * cosphi * sinlam;
                    xy[y] *= (_mode == Modes.Equitorial)
                                 ? sinphi
                                 :
                                     _cosX1 * sinphi - _sinX1 * cosphi * coslam;
                }
                else
                {
                    if (_mode == Modes.NorthPole)
                    {
                        coslam = -coslam;
                        lp[phi] = -lp[phi];
                    }
                    if (Math.Abs(lp[phi] - HALF_PI) < TOL)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    xy[x] = sinlam * (xy[y] = _akm1 * Math.Tan(FORT_PI + .5 * lp[phi]));
                    xy[y] *= coslam;
                }
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

                double sinphi, tp = 0.0, phiL = 0.0, halfe = 0.0, halfpi = 0.0;
                int j;

                double rho = Proj.Hypot(xy[x], xy[y]);
                switch (_mode)
                {
                    case Modes.Oblique:
                    case Modes.Equitorial:
                        double cosphi = Math.Cos(tp = 2 * Math.Atan2(rho * _cosX1, _akm1));
                        sinphi = Math.Sin(tp);
                        if (rho == 0.0)
                            phiL = Math.Asin(cosphi * _sinX1);
                        else
                            phiL = Math.Asin(cosphi * _sinX1 + (xy[y] * sinphi * _cosX1 / rho));
                        tp = Math.Tan(.5 * (HALF_PI + phiL));
                        xy[x] *= sinphi;
                        xy[y] = rho * _cosX1 * cosphi - xy[y] * _sinX1 * sinphi;
                        halfpi = HALF_PI;
                        halfe = .5 * E;
                        break;
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (_mode == Modes.NorthPole) xy[y] = -xy[y];
                        phiL = HALF_PI - 2 * Math.Atan(tp = -rho / _akm1);
                        halfpi = -HALF_PI;
                        halfe = -.5 * E;
                        break;
                }
                for (j = NITER; j-- > 0; phiL = lp[phi])
                {
                    sinphi = E * Math.Sin(phiL);
                    lp[phi] = 2 * Math.Atan(tp * Math.Pow((1 + sinphi) / (1 - sinphi), halfe)) - halfpi;
                    if (Math.Abs(phiL - lp[phi]) < CONV)
                    {
                        if (_mode == Modes.SouthPole) lp[phi] = -lp[phi];
                        lp[lam] = (xy[x] == 0 && xy[y] == 0) ? 0 : Math.Atan2(xy[x], xy[y]);
                        return;
                    }
                }
                lp[lam] = double.NaN;
                lp[phi] = double.NaN;
                continue;
                //ProjectionException(20);
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
                double c, rh;

                double sinc = Math.Sin(c = 2 * Math.Atan((rh = Proj.Hypot(xy[x], xy[y])) / _akm1));
                double cosc = Math.Cos(c);
                lp[lam] = 0;
                switch (_mode)
                {
                    case Modes.Equitorial:
                        if (Math.Abs(rh) <= EPS10) lp[phi] = 0;
                        else lp[phi] = Math.Asin(xy[y] * sinc / rh);
                        if (cosc != 0 || xy[x] != 0) lp[lam] = Math.Atan2(xy[x] * sinc, cosc * rh);
                        break;
                    case Modes.Oblique:
                        if (Math.Abs(rh) <= EPS10) lp[phi] = Phi0;
                        else lp[phi] = Math.Asin(cosc * _sinX1 + xy[y] * sinc * _cosX1 / rh);
                        if ((c = cosc - _sinX1 * Math.Sin(lp[phi])) != 0 || xy[x] != 0)
                            lp[lam] = Math.Atan2(xy[x] * sinc * _cosX1, c * rh);
                        break;
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (_mode == Modes.NorthPole) xy[y] = -xy[y];
                        if (Math.Abs(rh) <= EPS10)
                        {
                            lp[phi] = Phi0;
                        }
                        else
                        {
                            lp[phi] = Math.Asin(_mode == Modes.SouthPole ? -cosc : cosc);
                        }
                        lp[lam] = (xy[x] == 0 && xy[y] == 0) ? 0 : Math.Atan2(xy[x], xy[y]);
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.StandardParallel1 != null)
            {
                _phits = projInfo.Phi1;
            }
            else
            {
                _phits = HALF_PI;
            }
            double t;
            if (Math.Abs((t = Math.Abs(Phi0)) - HALF_PI) < EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else
            {
                _mode = t > EPS10 ? Modes.Oblique : Modes.Equitorial;
            }
            _phits = Math.Abs(_phits);
            if (Es != 0)
            {
                switch (_mode)
                {
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (Math.Abs(_phits - HALF_PI) < EPS10)
                        {
                            _akm1 = 2 * K0 / Math.Sqrt(Math.Pow(1 + E, 1 + E) * Math.Pow(1 - E, 1 - E));
                        }
                        else
                        {
                            _akm1 = Math.Cos(_phits) / Proj.Tsfn(_phits, t = Math.Sin(_phits), E);
                            t *= E;
                            _akm1 /= Math.Sqrt(1 - t * t);
                        }
                        break;
                    case Modes.Equitorial:
                        _akm1 = 2 * K0;
                        break;
                    case Modes.Oblique:
                        t = Math.Sin(Phi0);
                        double x = 2 * Math.Atan(Ssfn(Phi0, t, E)) - HALF_PI;
                        t *= E;
                        _akm1 = 2 * K0 * Math.Cos(Phi0) / Math.Sqrt(1 - t * t);
                        _sinX1 = Math.Sin(x);
                        _cosX1 = Math.Cos(x);
                        break;
                }
            }
            else
            {
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    if (_mode == Modes.Oblique)
                    {
                        _sinX1 = Math.Sin(Phi0);
                        _cosX1 = Math.Cos(Phi0);
                    }
                    _akm1 = 2 * K0;
                }
                else
                {
                    _akm1 = Math.Abs(_phits - HALF_PI) >= EPS10 ?
                                                                    Math.Cos(_phits) / Math.Tan(FORT_PI - .5 * _phits) :
                                                                                                                           2 * K0;
                }
            }
        }

        #endregion

        private static double Ssfn(double phit, double sinphi, double eccen)
        {
            sinphi *= eccen;
            return (Math.Tan(.5 * (HALF_PI + phit)) *
                Math.Pow((1 - sinphi) / (1 + sinphi), .5 * eccen));
        }
    }
}