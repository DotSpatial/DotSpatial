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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2009 9:25:51 AM
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
    /// LambertAzimuthalEqualArea
    /// </summary>
    public class LambertAzimuthalEqualArea : EllipticalTransform
    {
        #region Private Variables

        private double[] _apa;
        private double _cosb1;
        private double _dd;
        private Modes _mode;
        private double _qp;
        private double _rq;
        private double _sinb1;
        private double _xmf;
        private double _ymf;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LambertAzimuthalEqualArea
        /// </summary>
        public LambertAzimuthalEqualArea()
        {
            Name = "Lambert_Azimuthal_Equal_Area";
            Proj4Name = "laea";
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
                double sinb = 0, cosb = 0, b = 0;
                double coslam = Math.Cos(lp[lam]);
                double sinlam = Math.Sin(lp[lam]);
                double sinphi = Math.Sin(lp[phi]);
                double q = Proj.Qsfn(sinphi, E, OneEs);
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    sinb = q / _qp;
                    cosb = Math.Sqrt(1 - sinb * sinb);
                }
                switch (_mode)
                {
                    case Modes.Oblique:
                        b = 1 + _sinb1 * sinb + _cosb1 * cosb * coslam;
                        break;
                    case Modes.Equitorial:
                        b = 1 + cosb * coslam;
                        break;
                    case Modes.NorthPole:
                        b = HALF_PI + lp[phi];
                        q = _qp - q;
                        break;
                    case Modes.SouthPole:
                        b = lp[phi] - HALF_PI;
                        q = _qp + q;
                        break;
                }
                if (Math.Abs(b) < EPS10)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                switch (_mode)
                {
                    case Modes.Oblique:
                        xy[y] = _ymf * (b = Math.Sqrt(2 / b)) * (_cosb1 * sinb - _sinb1 * cosb * coslam);
                        xy[x] = _xmf * b * cosb * sinlam;
                        break;
                    case Modes.Equitorial:
                        xy[y] = (b = Math.Sqrt(2 / (1 + cosb * coslam))) * sinb * _ymf;
                        xy[x] = _xmf * b * cosb * sinlam;
                        break;
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (q >= 0)
                        {
                            xy[x] = (b = Math.Sqrt(q)) * sinlam;
                            xy[y] = coslam * (_mode == Modes.SouthPole ? b : -b);
                        }
                        else
                        {
                            xy[x] = xy[y] = 0;
                        }
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
                if (_mode == Modes.Equitorial || _mode == Modes.Oblique)
                {
                    if (_mode == Modes.Equitorial) xy[x] = 1 + cosphi * coslam;
                    if (_mode == Modes.Oblique) xy[y] = 1 + _sinb1 * sinphi + _cosb1 * cosphi * coslam;
                    if (xy[y] <= EPS10)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    xy[x] = (xy[y] = Math.Sqrt(2 / xy[y])) * cosphi * Math.Sin(lp[lam]);
                    xy[y] *= _mode == Modes.Equitorial ? sinphi : _cosb1 * sinphi - _sinb1 * cosphi * coslam;
                }
                else
                {
                    if (_mode == Modes.NorthPole) coslam = -coslam;
                    if (Math.Abs(lp[phi] + Phi0) < EPS10)
                    {
                        xy[x] = double.NaN;
                        xy[y] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    xy[y] = FORT_PI - lp[phi] * .5;
                    xy[y] = 2 * (_mode == Modes.SouthPole ? Math.Cos(xy[y]) : Math.Sin(xy[y]));
                    xy[x] = xy[y] * Math.Sin(lp[lam]);
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
                double ab;
                double cx = xy[x];
                double cy = xy[y];
                if (_mode == Modes.Equitorial || _mode == Modes.Oblique)
                {
                    double rho = Proj.Hypot(cx /= _dd, cy * _dd);
                    if (rho < EPS10)
                    {
                        lp[lam] = 0;
                        lp[phi] = Phi0;
                        return;
                    }
                    double sCe;
                    double cCe = Math.Cos(sCe = 2 * Math.Asin(.5 * rho / _rq));
                    cx *= (sCe = Math.Sin(sCe));
                    if (_mode == Modes.Oblique)
                    {
                        ab = cCe * _sinb1 + y * sCe * _cosb1 / rho;
                        //q = _qp*(ab);
                        cy = rho * _cosb1 * cCe - cy * _sinb1 * sCe;
                    }
                    else
                    {
                        ab = cy * sCe / rho;
                        //q = _qp*(ab);
                        cy = rho * cCe;
                    }
                }
                else
                {
                    if (_mode == Modes.NorthPole) cy = -cy;
                    double q;
                    if ((q = (cx * cx + cy * cy)) == 0)
                    {
                        lp[lam] = 0;
                        lp[phi] = Phi0;
                        return;
                    }
                    ab = 1 - q / _qp;
                    if (_mode == Modes.SouthPole) ab = -ab;
                }
                lp[lam] = Math.Atan2(cx, cy);
                lp[phi] = Proj.AuthLat(Math.Asin(ab), _apa);
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
                double cosz = 0, sinz = 0;
                double cx = xy[x];
                double cy = xy[y];
                double rh = Proj.Hypot(cx, cy);
                if ((lp[phi] = rh * .5) > 1)
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                lp[phi] = 2 * Math.Asin(lp[phi]);
                if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    sinz = Math.Sin(lp[phi]);
                    cosz = Math.Cos(lp[phi]);
                }
                switch (_mode)
                {
                    case Modes.Equitorial:
                        lp[phi] = Math.Abs(rh) <= EPS10 ? 0 : Math.Asin(cy * sinz / rh);
                        cx *= sinz;
                        cy = cosz * rh;
                        break;
                    case Modes.Oblique:
                        lp[phi] = Math.Abs(rh) <= EPS10 ? Phi0 : Math.Asin(cosz * _sinb1 + cy * sinz * _sinb1 / rh);
                        cx *= sinz * _cosb1;
                        cy = (cosz - Math.Sin(lp[phi]) * _sinb1) * rh;
                        break;
                    case Modes.NorthPole:
                        cy = -cy;
                        lp[phi] = HALF_PI - lp[phi];
                        break;
                    case Modes.SouthPole:
                        lp[phi] -= HALF_PI;
                        break;
                }
                lp[lam] = (cy == 0 && (_mode == Modes.Equitorial || _mode == Modes.Oblique)) ? 0 : Math.Atan2(cx, cy);
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double t = Math.Abs(Phi0);
            if (Math.Abs(t - HALF_PI) < EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else if (Math.Abs(t) < EPS10)
            {
                _mode = Modes.Equitorial;
            }
            else
            {
                _mode = Modes.Oblique;
            }
            if (Es == 0)
            {
                IsElliptical = false;
                _mode = Modes.Oblique;
                _sinb1 = Math.Sin(Phi0);
                _cosb1 = Math.Cos(Phi0);
                return;
            }
            IsElliptical = true;

            _qp = Proj.Qsfn(1, E, OneEs);
            _apa = Proj.Authset(Es);
            switch (_mode)
            {
                case Modes.NorthPole:
                case Modes.SouthPole:
                    _dd = 1;
                    break;
                case Modes.Equitorial:
                    _dd = 1 / (_rq = Math.Sqrt(.5 * _qp));
                    _xmf = 1;
                    _ymf = .5 * _qp;
                    break;
                case Modes.Oblique:
                    _rq = Math.Sqrt(.5 * _qp);
                    double sinphi = Math.Sin(Phi0);
                    _sinb1 = Proj.Qsfn(sinphi, E, OneEs);
                    _cosb1 = Math.Sqrt(1 - _sinb1 * _sinb1);
                    _dd = Math.Cos(Phi0) / (Math.Sqrt(1 - Es * sinphi * sinphi) * _rq * _cosb1);
                    _ymf = _xmf = _rq / _dd;
                    _xmf *= _dd;
                    break;
            }
        }

        #endregion
    }
}