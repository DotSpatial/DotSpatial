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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:23:39 PM
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
    /// Eckert2
    /// </summary>
    public class Eckert2 : Transform
    {
        #region Private Variables

        private const double FXC = 0.46065886596178063902;
        private const double FYC = 1.44720250911653531871;
        private const double C13 = 0.33333333333333333333;
        private const double ONEEPS = 1.0000001;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Eckert2
        /// </summary>
        public Eckert2()
        {
            Proj4Name = "eck2";
            Name = "Eckert_II";
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
                xy[x] = FXC * lp[lam] * (xy[y] = Math.Sqrt(4 - 3 * Math.Sin(Math.Abs(lp[phi]))));
                xy[y] = FYC * (2 - xy[y]);
                if (lp[phi] < 0) xy[y] = -xy[y];
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
                lp[lam] = xy[x] / (FXC * (lp[phi] = 2 - Math.Abs(xy[y]) / FYC));
                lp[phi] = (4 - lp[phi] * lp[phi]) * C13;
                if (Math.Abs(lp[phi]) >= 1)
                {
                    if (Math.Abs(lp[phi]) > ONEEPS)
                    {
                        lp[lam] = double.NaN;
                        lp[phi] = double.NaN;
                        continue;
                        //throw new ProjectionException(20);
                    }
                    lp[phi] = lp[phi] < 0 ? -HALF_PI : HALF_PI;
                }
                else
                    lp[phi] = Math.Asin(lp[phi]);
                if (xy[y] < 0)
                    lp[phi] = -lp[phi];
            }
        }

        #endregion
    }
}