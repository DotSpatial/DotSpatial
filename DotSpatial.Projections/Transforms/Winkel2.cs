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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 9:26:57 AM
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
    /// Winkel2
    /// </summary>
    public class Winkel2 : Transform
    {
        #region Private Variables

        private const int MAX_ITER = 10;
        private const double LOOP_TOL = 1e-7;
        private const double TWO_D_PI = 0.636619772367581343;
        private double _cosphi1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Winkel2
        /// </summary>
        public Winkel2()
        {
            Proj4Name = "wink2";
            Name = "Winkel_II";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _cosphi1 = Math.Cos(projInfo.Phi1);
        }

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

                xy[y] = lp[phi] * TWO_D_PI;
                double k = Math.PI * Math.Sin(lp[phi]);
                lp[phi] *= 1.8;
                for (j = MAX_ITER; j > 0; --j)
                {
                    double v;
                    lp[phi] -= v = (lp[phi] + Math.Sin(lp[phi]) - k) /
                                   (1 + Math.Cos(lp[phi]));
                    if (Math.Abs(v) < LOOP_TOL)
                        break;
                }
                if (j == 0)
                    lp[phi] = (lp[phi] < 0) ? -HALF_PI : HALF_PI;
                else
                    lp[phi] *= 0.5;
                xy[x] = 0.5 * lp[lam] * (Math.Cos(lp[phi]) + _cosphi1);
                xy[y] = FORT_PI * (Math.Sin(lp[phi]) + xy[y]);
            }
        }

        #endregion
    }
}