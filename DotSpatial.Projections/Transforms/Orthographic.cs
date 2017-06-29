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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2009 4:24:11 PM
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
    /// Orthographic
    /// </summary>
    public class Orthographic : Transform
    {
        #region Private Variables

        private double _cosph0;
        private Modes _mode;
        private double _sinph0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Orthographic
        /// </summary>
        public Orthographic()
        {
            Name = "Orthographic";
            Proj4Name = "ortho";
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
                double cosphi = Math.Cos(lp[phi]);
                double coslam = Math.Cos(lp[lam]);
                switch (_mode)
                {
                    case Modes.Equitorial:
                        if (cosphi * coslam < -EPS10)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //ProjectionException(20);
                        }
                        xy[y] = Math.Sin(lp[phi]);
                        break;
                    case Modes.Oblique:
                        double sinphi;
                        if (_sinph0 * (sinphi = Math.Sin(lp[phi])) + _cosph0 * cosphi * coslam < -EPS10)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //ProjectionException(20);
                        }
                        xy[y] = _cosph0 * sinphi - _sinph0 * cosphi * coslam;
                        break;
                    default:
                        if (_mode == Modes.NorthPole) coslam = -coslam;
                        if (Math.Abs(lp[phi] - Phi0) - EPS10 > HALF_PI)
                        {
                            xy[x] = double.NaN;
                            xy[y] = double.NaN;
                            continue;
                            //ProjectionException(20);
                        }
                        xy[y] = cosphi * coslam;
                        break;
                }
                xy[x] = cosphi * Math.Sin(lp[lam]);
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
                double rh, sinc;
                double cx = xy[x];
                double cy = xy[y];
                if ((sinc = (rh = Proj.Hypot(cx, cy))) > 1)
                {
                    if ((sinc - 1) > EPS10)
                    {
                        lp[lam] = double.NaN;
                        lp[phi] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    sinc = 1;
                }
                double cosc = Math.Sqrt(1 - sinc * sinc);
                if (Math.Abs(rh) <= EPS10)
                {
                    lp[phi] = Phi0;
                    lp[lam] = 0;
                }
                else
                {
                    switch (_mode)
                    {
                        case Modes.NorthPole:
                            cy = -cy;
                            lp[phi] = Math.Acos(sinc);
                            break;
                        case Modes.SouthPole:
                            lp[phi] = -Math.Acos(sinc);
                            break;
                        case Modes.Equitorial:
                            lp[phi] = cy * sinc / rh;
                            cx *= sinc;
                            cy = cosc * rh;
                            if (Math.Abs(lp[phi]) >= 1)
                            {
                                lp[phi] = lp[phi] < 0 ? -HALF_PI : HALF_PI;
                            }
                            else
                            {
                                lp[phi] = Math.Asin(lp[phi]);
                            }
                            break;
                        case Modes.Oblique:
                            lp[phi] = cosc * _sinph0 + cy * sinc * _cosph0 / rh;
                            cy = (cosc - _sinph0 * lp[phi]) * rh;
                            cx *= sinc * _cosph0;
                            if (Math.Abs(lp[phi]) >= 1)
                            {
                                lp[phi] = lp[phi] < 0 ? -HALF_PI : HALF_PI;
                            }
                            else
                            {
                                lp[phi] = Math.Asin(lp[phi]);
                            }
                            break;
                    }
                    lp[lam] = (cy == 0 && (_mode == Modes.Oblique || _mode == Modes.Equitorial))
                                  ? (cx == 0 ? 0 : cx < 0 ? -HALF_PI : HALF_PI)
                                  : Math.Atan2(cx, cy);
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (Math.Abs(Math.Abs(Phi0) - HALF_PI) <= EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else if (Math.Abs(Phi0) > EPS10)
            {
                _mode = Modes.Oblique;
                _sinph0 = Math.Sin(Phi0);
                _cosph0 = Math.Cos(Phi0);
            }
            else
            {
                _mode = Modes.Equitorial;
            }
        }

        #endregion

        #region Properties

        #endregion
    }
}