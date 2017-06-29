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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:01:45 PM
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
    /// CrasterParabolic
    /// </summary>
    public class CrasterParabolic : Transform
    {
        #region Private Variables

        private const double XM = 0.97720502380583984317;
        private const double RXM = 1.02332670794648848847;
        private const double YM = 3.06998012383946546542;
        private const double RYM = 0.32573500793527994772;
        private const double THIRD = 0.333333333333333333;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CrasterParabolic
        /// </summary>
        public CrasterParabolic()
        {
            Proj4Name = "crast";
            Name = "Craster_Parabolic";
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
                lp[phi] *= THIRD;
                xy[x] = XM * lp[lam] * (2 * Math.Cos(lp[phi] + lp[phi]) - 1);
                xy[y] = YM * Math.Sin(lp[phi]);
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
                lp[phi] = 3 * Math.Asin(xy[y] * RYM);
                lp[lam] = xy[x] * RXM / (2 * Math.Cos((lp[phi] + lp[phi]) * THIRD) - 1);
            }
        }

        #endregion
    }
}