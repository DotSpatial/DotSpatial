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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:56:10 PM
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
    /// Eckert4
    /// </summary>
    public class Eckert4 : Transform
    {
        #region Private Variables

        private const double CX = .42223820031577120149;
        private const double CY = 1.32650042817700232218;
        private const double CP = 3.57079632679489661922;
        private const double EPS = 1E-7;
        private const int NITER = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Eckert4
        /// </summary>
        public Eckert4()
        {
            Proj4Name = "eck4";
            Name = "Eckert_IV";
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
                int j;

                double p = CP * Math.Sin(lp[phi]);
                double v = lp[phi] * lp[phi];
                lp[phi] *= 0.895168 + v * (0.0218849 + v * 0.00826809);
                for (j = NITER; j > 0; --j)
                {
                    double c = Math.Cos(lp[phi]);
                    double s = Math.Sin(lp[phi]);
                    lp[phi] -= v = (lp[phi] + s * (c + 2) - p) /
                                   (1 + c * (c + 2) - s * s);
                    if (Math.Abs(v) < EPS)
                        break;
                }
                if (j == 0)
                {
                    xy[x] = CX * lp[lam];
                    xy[y] = lp[phi] < 0 ? -CY : CY;
                }
                else
                {
                    xy[x] = CX * lp[lam] * (1 + Math.Cos(lp[phi]));
                    xy[y] = CY * Math.Sin(lp[phi]);
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
                double c;
                lp[phi] = Proj.Aasin(xy[y] / CY);
                lp[lam] = xy[x] / (CX * (1 + (c = Math.Cos(lp[phi]))));
                lp[phi] = Proj.Aasin((lp[phi] + Math.Sin(lp[phi]) * (c + 2)) / CP);
            }
        }

        #endregion
    }
}