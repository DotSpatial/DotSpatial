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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 5:14:06 PM
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
    /// EquidistantConic
    /// </summary>
    public class EquidistantConic : Transform
    {
        #region Private Variables

        private double _c;
        private double[] _en;
        private double _n;
        private double _phi1;
        private double _phi2;
        private double _rho;
        private double _rho0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of EquidistantConic
        /// </summary>
        public EquidistantConic()
        {
            Proj4Name = "eqdc";
            Name = "Equidistant_Conic";
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
                _rho = _c - (IsElliptical
                                 ? Proj.Mlfn(lp[phi], Math.Sin(lp[phi]),
                                             Math.Cos(lp[phi]), _en)
                                 : lp[phi]);
                xy[x] = _rho * Math.Sin(lp[lam] *= _n);
                xy[y] = _rho0 - _rho * Math.Cos(lp[lam]);
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
                if ((_rho = Proj.Hypot(xy[x], xy[y] = _rho0 - xy[y])) != 0.0)
                {
                    if (_n < 0)
                    {
                        _rho = -_rho;
                        xy[x] = -xy[x];
                        xy[y] = -xy[y];
                    }
                    lp[phi] = _c - _rho;
                    if (IsElliptical)
                        lp[phi] = Proj.InvMlfn(lp[phi], Es, _en);
                    lp[lam] = Math.Atan2(xy[x], xy[y]) / _n;
                }
                else
                {
                    lp[lam] = 0;
                    lp[phi] = _n > 0 ? HALF_PI : -HALF_PI;
                }
            }
        }

        /// <summary>
        /// This exists in the case that we ever develop code to perform the special proj4 calculations
        /// </summary>
        /// <param name="lp"></param>
        /// <param name="p"></param>
        /// <param name="fac"></param>
        protected override void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            double sinphi = Math.Sin(lp[PHI]);
            double cosphi = Math.Cos(lp[PHI]);
            fac.Code = fac.Code | AnalyticModes.IsAnalHk;
            fac.H = 1;
            fac.K = _n * (_c - (IsElliptical
                                    ? Proj.Mlfn(lp[PHI], sinphi,
                                                cosphi, _en)
                                    : lp[PHI])) / Proj.Msfn(sinphi, cosphi, Es);
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double sinphi;

            if (projInfo.StandardParallel1 != null) _phi1 = projInfo.StandardParallel1.Value * Math.PI / 180;
            if (projInfo.StandardParallel2 != null) _phi2 = projInfo.StandardParallel2.Value * Math.PI / 180;

            if (Math.Abs(_phi1 + _phi2) < EPS10) throw new ProjectionException(-21);
            _en = Proj.Enfn(Es);
            _n = sinphi = Math.Sin(_phi1);
            double cosphi = Math.Cos(_phi1);
            bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
            if (IsElliptical)
            {
                double m1 = Proj.Msfn(sinphi, cosphi, Es);
                double ml1 = Proj.Mlfn(_phi1, sinphi, cosphi, _en);
                if (secant)
                {
                    /* secant cone */
                    sinphi = Math.Sin(_phi2);
                    cosphi = Math.Cos(_phi2);
                    _n = (m1 - Proj.Msfn(sinphi, cosphi, Es)) /
                         (Proj.Mlfn(_phi2, sinphi, cosphi, _en) - ml1);
                }
                _c = ml1 + m1 / _n;
                _rho0 = _c - Proj.Mlfn(Phi0, Math.Sin(Phi0),
                                       Math.Cos(Phi0), _en);
            }
            else
            {
                if (secant)
                    _n = (cosphi - Math.Cos(_phi2)) / (_phi2 - _phi1);
                _c = _phi1 + Math.Cos(_phi1) / _n;
                _rho0 = _c - Phi0;
            }
        }

        #endregion
    }
}