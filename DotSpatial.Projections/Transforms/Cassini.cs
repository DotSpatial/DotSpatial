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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:20:34 PM
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
    /// Cassini
    /// </summary>
    public class Cassini : EllipticalTransform
    {
        #region Private Variables

        private const double C1 = .16666666666666666666;
        private const double C2 = .00833333333333333333;
        private const double C3 = .04166666666666666666;
        private const double C4 = .33333333333333333333;
        private const double C5 = .06666666666666666666;
        private double _a1;
        private double _a2;
        private double _c;
        private double _d2;
        private double _dd;
        private double[] _en;
        private double _m0;
        private double _n;
        private double _r;
        private double _t;
        private double _tn;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Cassini
        /// </summary>
        public Cassini()
        {
            Proj4Name = "cass";
            Name = "Cassini";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void SphericalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                xy[x] = Math.Asin(Math.Cos(lp[phi]) * Math.Sin(lp[lam]));
                xy[y] = Math.Atan2(Math.Tan(lp[phi]), Math.Cos(lp[lam])) - Phi0;
            }
        }

        /// <inheritdoc />
        protected override void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                xy[y] = Proj.Mlfn(lp[phi], _n = Math.Sin(lp[phi]), _c = Math.Cos(lp[phi]), _en);
                _n = 1 / Math.Sqrt(1 - Es * _n * _n);
                _tn = Math.Tan(lp[phi]);
                _t = _tn * _tn;
                _a1 = lp[lam] * _c;
                _c *= Es * _c / (1 - Es);
                _a2 = _a1 * _a1;
                xy[x] = _n * _a1 * (1 - _a2 * _t *
                                    (C1 - (8 - _t + 8 * _c) * _a2 * C2));
                xy[y] -= _m0 - _n * _tn * _a2 *
                         (.5 + (5 - _t + 6 * _c) * _a2 * C3);
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
                double ph1 = Proj.InvMlfn(_m0 + xy[y], Es, _en);
                _tn = Math.Tan(ph1);
                _t = _tn * _tn;
                _n = Math.Sin(ph1);
                _r = 1 / (1 - Es * _n * _n);
                _n = Math.Sqrt(_r);
                _r *= (1 - Es) * _n;
                _dd = xy[x] / _n;
                _d2 = _dd * _dd;
                lp[phi] = ph1 - (_n * _tn / _r) * _d2 *
                          (.5 - (1 + 3 * _t) * _d2 * C3);
                lp[lam] = _dd * (1 + _t * _d2 *
                                 (-C4 + (1 + 3 * _t) * _d2 * C5)) / Math.Cos(ph1);
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
                lp[phi] = Math.Asin(Math.Sin(_dd = xy[y] + Phi0) * Math.Cos(xy[x]));
                lp[lam] = Math.Atan2(Math.Tan(xy[x]), Math.Cos(_dd));
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (!IsElliptical) return;
            _en = Proj.Enfn(Es);
            _m0 = Proj.Mlfn(Phi0, Math.Sin(Phi0), Math.Cos(Phi0), _en);
        }

        #endregion
    }
}