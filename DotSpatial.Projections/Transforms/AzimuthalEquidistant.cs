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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 11:39:36 AM
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
    /// AzimuthalEquidistant
    /// </summary>
    public class AzimuthalEquidistant : EllipticalTransform
    {
        #region Private Variables

        private const double TOL = 1E-14;
        private double _cosph0;
        private double[] _en;
        private double _g;
        private double _he;
        private bool _isGuam;
        private double _m1;
        private Modes _mode;
        private double _mp;
        private double _n1;
        private double _sinph0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AzimuthalEquidistant
        /// </summary>
        public AzimuthalEquidistant()
        {
            Proj4Name = "aeqd";
            Name = "Azimuthal_Equidistant";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Phi0 = projInfo.Phi0;
            if (Math.Abs(Math.Abs(Phi0) - HALF_PI) < EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
                _sinph0 = Phi0 < 0 ? -1 : 1;
                _cosph0 = 0;
            }
            else if (Math.Abs(Phi0) < EPS10)
            {
                _mode = Modes.Equitorial;
                _sinph0 = 0;
                _cosph0 = 1;
            }
            else
            {
                _mode = Modes.Oblique;
                _sinph0 = Math.Sin(Phi0);
                _cosph0 = Math.Cos(Phi0);
            }
            if (Es == 0) return;
            _en = Proj.Enfn(Es);
            if (projInfo.guam.HasValue && projInfo.guam.Value)
            {
                _m1 = Proj.Mlfn(Phi0, _sinph0, _cosph0, _en);
                _isGuam = true;
            }
            else
            {
                switch (_mode)
                {
                    case Modes.NorthPole:
                        _mp = Proj.Mlfn(HALF_PI, 1, 0, _en);
                        break;
                    case Modes.SouthPole:
                        _mp = Proj.Mlfn(-HALF_PI, -1, 0, _en);
                        break;
                    case Modes.Equitorial:
                    case Modes.Oblique:
                        _n1 = 1 / Math.Sqrt(1 - Es * _sinph0 * _sinph0);
                        _g = _sinph0 * (_he = E / Math.Sqrt(OneEs));
                        _he *= _cosph0;
                        break;
                }
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
                switch (_mode)
                {
                    case Modes.Equitorial:
                    case Modes.Oblique:
                        if (_mode == Modes.Equitorial)
                        {
                            xy[y] = cosphi * coslam;
                        }
                        else
                        {
                            xy[y] = _sinph0 * sinphi + _cosph0 * cosphi * coslam;
                        }
                        if (Math.Abs(Math.Abs(xy[y]) - 1) < TOL)
                        {
                            if (xy[y] < 0)
                            {
                                xy[x] = double.NaN;
                                xy[y] = double.NaN;
                                continue;
                                //throw new ProjectionException(20);
                            }
                            xy[x] = xy[y] = 0;
                        }
                        else
                        {
                            xy[y] = Math.Acos(xy[y]);
                            xy[y] /= Math.Sin(xy[y]);
                            xy[x] = xy[y] * cosphi * Math.Sin(lp[lam]);
                            xy[y] *= (_mode == Modes.Equitorial)
                                         ? sinphi
                                         :
                                             _cosph0 * sinphi - _sinph0 * cosphi * coslam;
                        }
                        break;
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (_mode == Modes.NorthPole)
                        {
                            lp[phi] = -lp[phi];
                            coslam = -coslam;
                        }
                        if (Math.Abs(lp[phi] - HALF_PI) < EPS10)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //throw new ProjectionException(20);
                        }
                        xy[x] = (xy[y] = (HALF_PI + lp[phi])) * Math.Sin(lp[lam]);
                        xy[y] *= coslam;
                        break;
                }
            }
        }

        /// <inheritdoc />
        protected override void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            if (_isGuam)
            {
                GuamForward(lp, xy, startIndex, numPoints);
                return;
            }
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double coslam = Math.Cos(lp[lam]);
                double cosphi = Math.Cos(lp[phi]);
                double sinphi = Math.Sin(lp[phi]);
                switch (_mode)
                {
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (_mode == Modes.NorthPole) coslam = -coslam;
                        double rho;
                        xy[x] = (rho = Math.Abs(_mp - Proj.Mlfn(lp[phi], sinphi, cosphi, _en))) * Math.Sin(lp[lam]);
                        xy[y] = rho * coslam;
                        break;
                    case Modes.Equitorial:
                    case Modes.Oblique:
                        if (Math.Abs(lp[lam]) < EPS10 && Math.Abs(lp[phi] - Phi0) < EPS10)
                        {
                            xy[x] = xy[y] = 0;
                            break;
                        }
                        double t = Math.Atan2(OneEs * sinphi + Es * _n1 * _sinph0 *
                                              Math.Sqrt(1 - Es * sinphi * sinphi), cosphi);
                        double ct = Math.Cos(t);
                        double st = Math.Sin(t);
                        double az = Math.Atan2(Math.Sin(lp[lam]) * ct, _cosph0 * st - _sinph0 * coslam * ct);
                        double cA = Math.Cos(az);
                        double sA = Math.Sin(az);
                        double s = Math.Asin(Math.Abs(sA) < TOL
                                                 ?
                                                     (_cosph0 * st - _sinph0 * coslam * ct) / cA
                                                 :
                                                     Math.Sin(lp[lam]) * ct / sA);
                        double h = _he * cA;
                        double h2 = h * h;
                        double c = _n1 * s * (1 + s * s * (-h2 * (1 - h2) / 6 +
                                                           s * (_g * h * (1 - 2 * h2 * h2) / 8 +
                                                                s * ((h2 * (4 - 7 * h2) - 3 * _g * _g * (1 - 7 * h2)) /
                                                                     120 - s * _g * h / 48))));
                        xy[x] = c * sA;
                        xy[y] = c * cA;
                        break;
                }
            }
        }

        private void GuamForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double cosphi = Math.Cos(lp[phi]);
                double sinphi = Math.Sin(lp[phi]);
                double t = 1 / Math.Sqrt(1 - Es * sinphi * sinphi);
                xy[x] = lp[lam] * cosphi * t;
                xy[y] = Proj.Mlfn(lp[phi], sinphi, cosphi, _en) - _m1 +
                        .5 * lp[lam] * lp[lam] * cosphi * sinphi * t;
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
                double cRh;

                if ((cRh = Proj.Hypot(xy[x], xy[y])) > Math.PI)
                {
                    if (cRh - EPS10 > Math.PI)
                    {
                        lp[phi] = double.NaN;
                        lp[lam] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    cRh = Math.PI;
                }
                else if (cRh < EPS10)
                {
                    lp[phi] = Phi0;
                    lp[lam] = 0;
                    return;
                }
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    double sinc = Math.Sin(cRh);
                    double cosc = Math.Cos(cRh);
                    if (_mode == Modes.Equitorial)
                    {
                        lp[phi] = Proj.Aasin(xy[y] * sinc / cRh);
                        xy[x] *= sinc;
                        xy[y] = cosc * cRh;
                    }
                    else
                    {
                        lp[phi] = Proj.Aasin(cosc * _sinph0 + xy[y] * sinc * _cosph0 /
                                             cRh);
                        xy[y] = (cosc - _sinph0 * Math.Sin(lp[phi])) * cRh;
                        xy[x] *= sinc * _cosph0;
                    }
                    lp[lam] = xy[y] == 0 ? 0 : Math.Atan2(xy[x], xy[y]);
                }
                else if (_mode == Modes.NorthPole)
                {
                    lp[phi] = HALF_PI - cRh;
                    lp[lam] = Math.Atan2(xy[x], -xy[y]);
                }
                else
                {
                    lp[phi] = cRh - HALF_PI;
                    lp[lam] = Math.Atan2(xy[x], xy[y]);
                }
            }
        }

        /// <inheritdoc />
        protected override void EllipticalInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            if (_isGuam)
            {
                GuamInverse(xy, lp, startIndex, numPoints);
            }

            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double c;

                if ((c = Proj.Hypot(xy[x], xy[y])) < EPS10)
                {
                    lp[phi] = Phi0;
                    lp[lam] = 0;
                    return;
                }
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    double az;
                    double cosAz = Math.Cos(az = Math.Atan2(xy[x], xy[y]));
                    double t = _cosph0 * cosAz;
                    double b = Es * t / OneEs;
                    double a = -b * t;
                    b *= 3 * (1 - a) * _sinph0;
                    double d = c / _n1;
                    double e = d * (1 - d * d * (a * (1 + a) / 6 + b * (1 + 3 * a) * d / 24));
                    double f = 1 - e * e * (a / 2 + b * e / 6);
                    double psi = Proj.Aasin(_sinph0 * Math.Cos(e) + t * Math.Sin(e));
                    lp[lam] = Proj.Aasin(Math.Sin(az) * Math.Sin(e) / Math.Cos(psi));
                    if ((t = Math.Abs(psi)) < EPS10)
                        lp[phi] = 0;
                    else if (Math.Abs(t - HALF_PI) < 0)
                        lp[phi] = HALF_PI;
                    else
                    {
                        lp[phi] = Math.Atan((1 - Es * f * _sinph0 / Math.Sin(psi)) * Math.Tan(psi) /
                                            OneEs);
                    }
                }
                else
                {
                    /* Polar */
                    lp[phi] = Proj.InvMlfn(_mode == Modes.NorthPole ? _mp - c : _mp + c,
                                           Es, _en);
                    lp[lam] = Math.Atan2(xy[x], _mode == Modes.NorthPole ? -xy[y] : xy[y]);
                }
            }
        }

        private void GuamInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double t = 0;
                int j;

                double x2 = 0.5 * xy[x] * xy[x];
                lp[phi] = Phi0;
                for (j = 0; j < 3; ++j)
                {
                    t = E * Math.Sin(lp[phi]);
                    lp[phi] = Proj.InvMlfn(_m1 + xy[y] -
                                           x2 * Math.Tan(lp[phi]) * (t = Math.Sqrt(1 - t * t)), Es, _en);
                }
                lp[lam] = xy[x] * t / Math.Cos(lp[phi]);
            }
        }

        #endregion
    }
}