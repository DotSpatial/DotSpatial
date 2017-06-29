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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:08:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//         Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Bonne
    /// </summary>
    public class Bonne : EllipticalTransform
    {
        #region Private Variables

        private double _am1;
        private double _cphi1;
        private double[] _en;
        private double _m1;
        private double _phi1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Bonne
        /// </summary>
        public Bonne()
        {
            Name = "Bonne";
            Proj4Name = "bonne";
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
                double e, c;
                double rh = _am1 + _m1 - Proj.Mlfn(lp[phi], e = Math.Sin(lp[phi]), c = Math.Cos(lp[phi]), _en);
                e = c * lp[lam] / (rh * Math.Sqrt(1 - Es * e * e));
                xy[x] = rh * Math.Sin(e);
                xy[y] = _am1 - rh * Math.Cos(e);
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
                double rh = _cphi1 + _phi1 - lp[phi];
                if (Math.Abs(rh) > EPS10)
                {
                    double e;
                    xy[x] = rh * Math.Sin(e = lp[lam] * Math.Cos(lp[phi]) / rh);
                    xy[y] = _cphi1 - rh * Math.Cos(e);
                }
                else
                    xy[x] = xy[y] = 0;
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
                double rh = Proj.Hypot(xy[x], xy[y] = _cphi1 - xy[y]);
                lp[phi] = _cphi1 + _phi1 - rh;
                if (Math.Abs(lp[phi]) > HALF_PI)
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                if (Math.Abs(Math.Abs(lp[phi]) - HALF_PI) <= EPS10)
                    lp[lam] = 0;
                else
                    lp[lam] = rh * Math.Atan2(xy[x], xy[y]) / Math.Cos(lp[phi]);
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
                double s;

                double rh = Proj.Hypot(xy[x], xy[y] = _am1 - xy[y]);
                lp[phi] = Proj.InvMlfn(_am1 + _m1 - rh, Es, _en);
                if ((s = Math.Abs(lp[phi])) < HALF_PI)
                {
                    s = Math.Sin(lp[phi]);
                    lp[lam] = rh * Math.Atan2(xy[x], xy[y]) *
                              Math.Sqrt(1 - Es * s * s) / Math.Cos(lp[phi]);
                }
                else if (Math.Abs(s - HALF_PI) <= EPS10)
                    lp[lam] = 0;
                else
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _phi1 = projInfo.Phi1;
            if (Math.Abs(_phi1) < EPS10) throw new ProjectionException(-23);
            if (Es > 0)
            {
                _en = Proj.Enfn(Es);
                double c;
                _m1 = Proj.Mlfn(_phi1, _am1 = Math.Sin(_phi1),
                                c = Math.Cos(_phi1), _en);
                _am1 = c / (Math.Sqrt(1 - Es * _am1 * _am1) * _am1);
            }
            else
            {
                if (Math.Abs(_phi1) + EPS10 >= HALF_PI)
                    _cphi1 = 0;
                else
                    _cphi1 = 1 / Math.Tan(_phi1);
            }
        }

        #endregion
    }
}