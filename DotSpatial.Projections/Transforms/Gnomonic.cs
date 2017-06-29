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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:40:14 AM
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
    /// Gnomonic
    /// </summary>
    public class Gnomonic : Transform
    {
        #region Private Variables

        private double _cosph0;
        private Modes _mode;
        private double _sinph0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Gnomonic
        /// </summary>
        public Gnomonic()
        {
            Proj4Name = "gnom";
            Name = "Gnomonic";
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
                double sinphi = Math.Sin(lp[phi]);
                double cosphi = Math.Cos(lp[phi]);
                double coslam = Math.Cos(lp[lam]);
                switch (_mode)
                {
                    case Modes.Equitorial:
                        xy[y] = cosphi * coslam;
                        break;
                    case Modes.Oblique:
                        xy[y] = _sinph0 * sinphi + _cosph0 * cosphi * coslam;
                        break;
                    case Modes.SouthPole:
                        xy[y] = -sinphi;
                        break;
                    case Modes.NorthPole:
                        xy[y] = sinphi;
                        break;
                }
                if (xy[y] <= EPS10)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                xy[x] = (xy[y] = 1 / xy[y]) * cosphi * Math.Sin(lp[lam]);
                switch (_mode)
                {
                    case Modes.Equitorial:
                        xy[y] *= sinphi;
                        break;
                    case Modes.Oblique:
                        xy[y] *= _cosph0 * sinphi - _sinph0 * cosphi * coslam;
                        break;
                    case Modes.NorthPole:
                    case Modes.SouthPole:
                        if (_mode == Modes.NorthPole) coslam = -coslam;
                        xy[y] *= cosphi * coslam;
                        break;
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
                double rh = Proj.Hypot(xy[x], xy[y]);
                double sinz = Math.Sin(lp[phi] = Math.Atan(rh));
                double cosz = Math.Sqrt(1 - sinz * sinz);
                if (Math.Abs(rh) <= EPS10)
                {
                    lp[phi] = Phi0;
                    lp[lam] = 0;
                }
                else
                {
                    switch (_mode)
                    {
                        case Modes.Oblique:
                            lp[phi] = cosz * _sinph0 + xy[y] * sinz * _cosph0 / rh;
                            if (Math.Abs(lp[phi]) >= 1)
                                lp[phi] = lp[phi] > 0 ? HALF_PI : -HALF_PI;
                            else
                                lp[phi] = Math.Asin(lp[phi]);
                            xy[y] = (cosz - _sinph0 * Math.Sin(lp[phi])) * rh;
                            xy[x] *= sinz * _cosph0;
                            break;
                        case Modes.Equitorial:
                            lp[phi] = xy[y] * sinz / rh;
                            if (Math.Abs(lp[phi]) >= 1)
                                lp[phi] = lp[phi] > 0 ? HALF_PI : -HALF_PI;
                            else
                                lp[phi] = Math.Asin(lp[phi]);
                            xy[y] = cosz * rh;
                            xy[x] *= sinz;
                            break;
                        case Modes.SouthPole:
                            lp[phi] -= HALF_PI;
                            break;
                        case Modes.NorthPole:
                            lp[phi] = HALF_PI - lp[phi];
                            xy[y] = -xy[y];
                            break;
                    }
                    lp[lam] = Math.Atan2(xy[x], xy[y]);
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (Math.Abs(Math.Abs(Phi0) - HALF_PI) < EPS10)
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            else if (Math.Abs(Phi0) < EPS10)
                _mode = Modes.Equitorial;
            else
            {
                _mode = Modes.Oblique;
                _sinph0 = Math.Sin(Phi0);
                _cosph0 = Math.Cos(Phi0);
            }
        }

        #endregion

        #region Properties

        #endregion
    }
}