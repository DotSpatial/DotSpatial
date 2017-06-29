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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 9:13:41 AM
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
    /// Gauss
    /// </summary>
    public class Gauss : Transform
    {
        #region Private Variables

        private const int MAX_ITER = 20;
        private const double DEL_TOL = 1E-14;
        private readonly double _c;
        private readonly double _e;
        private readonly double _k;
        private readonly double _ratexp;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Gauss
        /// </summary>
        public Gauss(double e, double phi0, ref double chi, ref double rc)
        {
            double es = e * e;
            _e = e;
            double sphi = Math.Sin(phi0);
            double cphi = Math.Cos(phi0);
            cphi *= cphi;
            rc = Math.Sqrt(1 - es) / (1 - es * sphi * sphi);
            _c = Math.Sqrt(1 + es * cphi * cphi / (1 - es));
            chi = Math.Asin(sphi / _c);
            _ratexp = .5 * _c * e;
            _k = Math.Tan(.5 * chi + Math.PI / 4) / (Math.Pow(Math.Tan(.5 * phi0 + Math.PI / 4), _c) * Srat(e * sphi, _ratexp));
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnForward(double[] elp, double[] result, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                result[phi] = 2 * Math.Atan(_k *
                                            Math.Pow(Math.Tan(.5 * elp[phi] + Math.PI / 4), _c) *
                                            Srat(_e * Math.Sin(elp[phi]), _ratexp)) - Math.PI / 2;
                result[lam] = _c * elp[lam];
            }
        }

        /// <inheritdoc />
        protected override void OnInverse(double[] slp, double[] result, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int j;
                result[lam] = slp[lam] / _c;
                double alpha = Math.Tan(.5 * slp[phi] + Math.PI / 4) / _k;
                double num = Math.Pow(alpha, 1 / _c);
                for (j = MAX_ITER; j > 0; --j)
                {
                    double temp = Srat(_e * Math.Sin(slp[phi]), -.5 * _e);
                    result[phi] = 2 * Math.Atan(num * temp) - Math.PI / 2;
                    if (Math.Abs(result[phi] - slp[phi]) < DEL_TOL) break;
                    slp[phi] = result[phi];
                }
                if (j != 0) continue;
                result[lam] = double.NaN;
                result[phi] = double.NaN;
                continue;
                //throw new ProjectionException(17);
            }
        }

        private static double Srat(double esinp, double exp)
        {
            return Math.Pow((1 - esinp) / (1 + esinp), exp);
        }

        #endregion
    }
}