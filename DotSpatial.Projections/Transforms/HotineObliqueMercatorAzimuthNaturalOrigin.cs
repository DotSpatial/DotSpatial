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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/31/2009 2:35:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Steve Riddell       |  11/7/2011 |  Added to support Esri's "Hotine_Oblique_Mercator_Natural_Origin"
//                     |            |  Matches EPSG's Hotine Variant A, code 9812
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// OrthogonalMercator
    /// </summary>
    public class HotineObliqueMercatorAzimuthNaturalOrigin : Transform
    {
        #region Private Variables

        private const double TOLERANCE = 1E-7;
        private double _al;    // this is the documented 'A' divided by 'a' (i.e. 'a' is not included since DS de-scales by a)
        private double _alpha; // azimuth of initial line - AKA alpha-sub-c (an input)
        private double _bl;    // the docuemented 'B'
        private double _cosgam;
        private double _cosrot;
        private double _el;    // from EPSG doc: 'H', from Snyder: 'E'
        private bool _ellips; // false if sphere is being used instead of an ellipsoid
        private double _gamma; // AKA gamma-sub-zero; calculated internally
        private double _lamc;  // longitude of projection center - AKA lambda-sub-c (an input)
        private bool _rot;    // true if alpha=90 deg; Swiss & Hung
        private double _singam;
        private double _sinrot;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of OrthogonalMercator
        /// </summary>
        public HotineObliqueMercatorAzimuthNaturalOrigin()
        {
            Proj4Name = "omerc";
            Name = "Hotine_Oblique_Mercator_Azimuth_Natural_Origin";  // EPSG 9812 (Hotine Oblique Mercator Variant A)
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
                double ul, us;
                double vl = Math.Sin(_bl * lp[lam]);
                if (Math.Abs(Math.Abs(lp[phi]) - Math.PI / 2) <= EPS10)
                {
                    ul = lp[phi] < 0 ? -_singam : _singam;
                    us = _al * lp[phi] / _bl;
                }
                else
                {
                    double q = _el / (_ellips ? Math.Pow(Proj.Tsfn(lp[phi], Math.Sin(lp[phi]), E), _bl) : Tsfn0(lp[phi]));
                    double s = .5 * (q - 1 / q);
                    ul = 2.0 * (s * _singam - vl * _cosgam) / (q + 1.0 / q);
                    double con = Math.Cos(_bl * lp[lam]);
                    if (Math.Abs(con) >= TOLERANCE)
                    {
                        us = _al * Math.Atan((s * _cosgam + vl * _singam) / con) / _bl;
                        if (con < 0) us += Math.PI * _al / _bl;
                    }
                    else
                    {
                        us = _al * _bl * lp[lam];
                    }
                }
                if (Math.Abs(Math.Abs(ul) - 1.0) <= EPS10)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //ProjectionException(20);
                }
                double vs = .5 * _al * Math.Log((1 - ul) / (1 + ul)) / _bl;
                if (!_rot)
                {
                    xy[x] = us;
                    xy[y] = vs;
                }
                else
                {
                    xy[x] = vs * _cosrot + us * _sinrot;
                    xy[y] = us * _cosrot - vs * _sinrot;
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
                double us, vs;
                if (!_rot)
                {
                    us = xy[x];
                    vs = xy[y];
                }
                else
                {
                    vs = xy[x] * _cosrot - xy[y] * _sinrot;
                    us = xy[y] * _cosrot + xy[x] * _sinrot;
                }
                double q = Math.Exp(-_bl * vs / _al);
                double s = .5 * (q - 1 / q);
                double vl = Math.Sin(_bl * us / _al);
                double ul = 2 * (vl * _cosgam + s * _singam) / (q + 1 / q);
                if (Math.Abs(Math.Abs(ul) - 1) < EPS10)
                {
                    lp[lam] = 0;
                    lp[phi] = ul < 0 ? -HALF_PI : HALF_PI;
                }
                else
                {
                    lp[phi] = _el / Math.Sqrt((1 + ul) / (1 - ul));
                    if (_ellips)
                    {
                        if ((lp[phi] = Proj.Phi2(Math.Pow(lp[phi], 1 / _bl), E)) == double.MaxValue)
                        {
                            lp[lam] = double.NaN;
                            lp[phi] = double.NaN;
                            continue;
                            //ProjectionException(20);
                        }
                    }
                    else
                    {
                        lp[phi] = HALF_PI - 2 * Math.Atan(lp[phi]);
                    }
                    lp[lam] = -Math.Atan2((s * _cosgam - vl * _singam), Math.Cos(_bl * us / _al)) / _bl;
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double con;
            double f;
            double d;
            double toRadians = projInfo.GeographicInfo.Unit.Radians;
            _rot = !projInfo.no_rot.HasValue || projInfo.no_rot.Value == 0;

            _lamc = projInfo.LongitudeOfCenter.Value * toRadians;

            if (projInfo.alpha.HasValue)
                _alpha = projInfo.alpha.Value * toRadians;
            if (Math.Abs(_alpha) < TOLERANCE ||
                Math.Abs(Math.Abs(Phi0) - HALF_PI) <= TOLERANCE ||
                Math.Abs(Math.Abs(_alpha) - HALF_PI) <= TOLERANCE)
                throw new ProjectionException(32);

            _ellips = Es > 0;
            double com = _ellips ? Math.Sqrt(OneEs) : 1;
            if (Math.Abs(Phi0) > EPS10)
            {
                double sinph0 = Math.Sin(Phi0);
                double cosph0 = Math.Cos(Phi0);
                if (_ellips)
                {
                    con = 1 - Es * sinph0 * sinph0;
                    _bl = cosph0 * cosph0;
                    _bl = Math.Sqrt(1 + Es * _bl * _bl / OneEs);
                    _al = _bl * K0 * com / con;
                    d = _bl * com / (cosph0 * Math.Sqrt(con));
                }
                else
                {
                    _bl = 1;
                    _al = K0;
                    d = 1 / cosph0;
                }

                if ((f = d * d - 1) <= 0)
                {
                    f = 0;
                }
                else
                {
                    f = Math.Sqrt(f);
                    if (Phi0 < 0) f = -f;
                }
                _el = f += d;
                if (_ellips)
                {
                    _el *= Math.Pow(Proj.Tsfn(Phi0, sinph0, E), _bl);
                }
                else
                {
                    _el *= Tsfn0(Phi0);
                }
            }
            else
            {
                _bl = 1 / com;
                _al = K0;
                _el = d = f = 1;
            }

            _gamma = Math.Asin(Math.Sin(_alpha) / d);
            Lam0 = _lamc - Math.Asin((.5 * (f - 1 / f)) * Math.Tan(_gamma)) / _bl;
            projInfo.Lam0 = Lam0;

            _singam = Math.Sin(_gamma);
            _cosgam = Math.Cos(_gamma);

            _sinrot = Math.Sin(_alpha);
            _cosrot = Math.Cos(_alpha);
        }

        #endregion

        #region Properties

        #endregion

        #region private methods

        private static double Tsfn0(double x)
        {
            return Math.Tan(.5 * (HALF_PI - x));
        }

        #endregion
    }
}