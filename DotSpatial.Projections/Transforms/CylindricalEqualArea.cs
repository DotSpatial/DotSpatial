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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:42:13 PM
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
    /// CylindricalEqualArea
    /// </summary>
    public class CylindricalEqualArea : EllipticalTransform
    {
        #region Private Variables

        private const double EPS = 1e-10;
        private double[] _apa;
        private double _qp;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CylindricalEqualArea
        /// </summary>
        public CylindricalEqualArea()
        {
            Name = "Cylindrical_Equal_Area";
            Proj4Name = "cea";
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
                xy[x] = K0 * lp[lam];
                xy[y] = .5 * Proj.Qsfn(Math.Sin(lp[phi]), E, OneEs) / K0;
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
                xy[x] = K0 * lp[lam];
                xy[y] = Math.Sin(lp[phi]) / K0;
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
                lp[phi] = Proj.AuthLat(Math.Asin(2 * xy[y] * K0 / _qp), _apa);
                lp[lam] = xy[x] / K0;
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
                double t;

                if ((t = Math.Abs(xy[y] *= K0)) - EPS <= 1)
                {
                    if (t >= 1)
                        lp[phi] = xy[y] < 0 ? -HALF_PI : HALF_PI;
                    else
                        lp[phi] = Math.Asin(xy[y]);
                    lp[lam] = xy[x] / K0;
                }
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
            double t;
            if (projInfo.StandardParallel1 != null)
                t = projInfo.StandardParallel1.Value.FromDegreesToRadians();
            else
                t = projInfo.lat_ts.Value.FromDegreesToRadians();
            if ((K0 = Math.Cos(t)) < 0) throw new ProjectionException(-24);
            if (!IsElliptical) return;
            t = Math.Sin(t);
            K0 /= Math.Sqrt(1 - Es * t * t);
            E = Math.Sqrt(Es);
            _apa = Proj.Authset(Es);
            _qp = Proj.Qsfn(1, E, OneEs);
        }

        #endregion
    }
}