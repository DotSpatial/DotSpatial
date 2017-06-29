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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:01:37 PM
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
    /// ObliqueCylindricalEqualArea
    /// </summary>
    public class ObliqueCylindricalEqualArea : Transform
    {
        #region Private Variables

        private double _cosphi;
        private double _rok;
        private double _rtk;
        private double _singam;
        private double _sinphi;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ObliqueCylindricalEqualArea
        /// </summary>
        public ObliqueCylindricalEqualArea()
        {
            Proj4Name = "ocea";
            Name = "Oblique_Cylindrical_Equal_Area";
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
                xy[y] = Math.Sin(lp[lam]);
                double t = Math.Cos(lp[lam]);
                xy[x] = Math.Atan((Math.Tan(lp[phi]) * _cosphi + _sinphi * xy[y]) / t);
                if (t < 0) xy[x] += Math.PI;
                xy[x] *= _rtk;
                xy[y] = _rok * (_sinphi * Math.Sin(lp[phi]) - _cosphi * Math.Cos(lp[phi]) * xy[y]);
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
                double s;
                xy[y] /= _rok;
                xy[x] /= _rtk;
                double t = Math.Sqrt(1 - xy[y] * xy[y]);
                lp[phi] = Math.Asin(xy[y] * _sinphi + t * _cosphi * (s = Math.Sin(xy[x])));
                lp[lam] = Math.Atan2(t * _sinphi * s - xy[y] * _cosphi,
                                     t * Math.Cos(xy[x]));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            const double phi0 = 0.0;

            _rok = A / K0;
            _rtk = A * K0;
            if (projInfo.alpha.HasValue)
            {
                double alpha = projInfo.alpha.FromDegreesToRadians();
                double lonz = projInfo.lonc.FromDegreesToRadians();
                _singam = Math.Atan(-Math.Cos(alpha) / (-Math.Sin(phi0) * Math.Sin(alpha))) + lonz;
                _sinphi = Math.Asin(Math.Cos(phi0) * Math.Sin(alpha));
            }
            else
            {
                double phi1 = projInfo.Phi1;
                double phi2 = projInfo.Phi2;
                double lam1 = projInfo.lon_1.FromDegreesToRadians();
                double lam2 = projInfo.lon_2.FromDegreesToRadians();
                _singam = Math.Atan2(Math.Cos(phi1) * Math.Sin(phi2) * Math.Cos(lam1) -
                                     Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(lam2),
                                     Math.Sin(phi1) * Math.Cos(phi2) * Math.Sin(lam2) -
                                     Math.Cos(phi1) * Math.Sin(phi2) * Math.Sin(lam1));
                _sinphi = Math.Atan(-Math.Cos(_singam - lam1) / Math.Tan(phi1));
            }
            Lam0 = _singam + HALF_PI;
            _cosphi = Math.Cos(_sinphi);
            _sinphi = Math.Sin(_sinphi);
            _singam = Math.Sin(_singam);
        }

        #endregion
    }
}