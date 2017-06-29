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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 10:15:38 AM
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
    /// AlbersEqualArea
    /// </summary>
    public class AlbersEqualArea : Transform
    {
        #region Private Variables

        private const int N_ITER = 15;
        private const double EPSILON = 1.0E-7;
        private const double TOL = 1E-10;
        private const double TOL7 = 1E-7;

        private double _c;
        private double _dd;
        private double _ec;
        private double _n;
        private double _n2;
        private double _phi1;
        private double _phi2;
        private double _rho;
        private double _rho0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AlbersEqualArea
        /// </summary>
        public AlbersEqualArea()
        {
            Name = "Albers";
            Proj4Name = "aea";
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

                if ((_rho = _c - (IsElliptical
                                      ? _n * Proj.Qsfn(Math.Sin(lp[phi]),
                                                       E, OneEs)
                                      : _n2 * Math.Sin(lp[phi]))) < 0)
                {
                    xy[x] = double.NaN;
                    xy[y] = double.NaN;
                    continue;
                    //throw new ProjectionException(20);
                }
                _rho = _dd * Math.Sqrt(_rho);
                xy[x] = _rho * Math.Sin(lp[lam] *= _n);
                xy[y] = _rho0 - _rho * Math.Cos(lp[lam]);
            }
        }

        private static double PhiFn(double qs, double te, double toneEs)
        {
            double dphi;
            double myPhi = Math.Asin(.5 * qs);
            if (te < EPSILON) return (myPhi);
            int i = N_ITER;
            do
            {
                double sinpi = Math.Sin(myPhi);
                double cospi = Math.Cos(myPhi);
                double con = te * sinpi;
                double com = 1 - con * con;
                dphi = .5 * com * com / cospi * (qs / toneEs -
                                                 sinpi / com + .5 / te * Math.Log((1 - con) / (1 + con)));
                myPhi += dphi;
            }
            while (Math.Abs(dphi) > TOL && --i > 0);
            return (i != 0 ? myPhi : double.MaxValue);
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
                    lp[phi] = _rho / _dd;
                    if (IsElliptical)
                    {
                        lp[phi] = (_c - lp[phi] * lp[phi]) / _n;
                        if (Math.Abs(_ec - Math.Abs(lp[phi])) > TOL7)
                        {
                            if ((lp[phi] = PhiFn(lp[phi], E, OneEs)) == double.MaxValue)
                            {
                                lp[phi] = double.NaN;
                                lp[lam] = double.NaN;
                                continue;
                                //throw new ProjectionException(20);
                            }
                        }
                        else
                        {
                            lp[phi] = lp[phi] < 0 ? -HALF_PI : HALF_PI;
                        }
                    }
                    else if (Math.Abs(lp[phi] = (_c - lp[phi] * lp[phi]) / _n2) <= 1)
                    {
                        lp[phi] = Math.Asin(lp[phi]);
                    }
                    else
                    {
                        lp[phi] = lp[phi] < 0 ? -HALF_PI : HALF_PI;
                    }
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
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _phi1 = projInfo.Phi1;
            _phi2 = projInfo.Phi2;
            Setup();
        }

        /// <summary>
        /// Internal code handling the setup operations for the transform
        /// </summary>
        protected void Setup()
        {
            double sinphi;
            if (Math.Abs(_phi1 + _phi2) < EPS10)
            {
                throw new ProjectionException(-21);
            }
            _n = sinphi = Math.Sin(_phi1);
            double cosphi = Math.Cos(_phi1);
            bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
            if (IsElliptical)
            {
                double m1 = Proj.Msfn(sinphi, cosphi, Es);
                double ml1 = Proj.Qsfn(sinphi, E, OneEs);
                if (secant)
                { /* secant cone */
                    sinphi = Math.Sin(_phi2);
                    cosphi = Math.Cos(_phi2);
                    double m2 = Proj.Msfn(sinphi, cosphi, Es);
                    double ml2 = Proj.Qsfn(sinphi, E, OneEs);
                    _n = (m1 * m1 - m2 * m2) / (ml2 - ml1);
                }
                _ec = 1 - .5 * OneEs * Math.Log((1 - E) / (1 + E)) / E;
                _c = m1 * m1 + _n * ml1;
                _dd = 1 / _n;
                _rho0 = _dd * Math.Sqrt(_c - _n * Proj.Qsfn(Math.Sin(Phi0), E, OneEs));
            }
            else
            {
                if (secant) _n = .5 * (_n + Math.Sin(_phi2));
                _n2 = _n + _n;
                _c = cosphi * cosphi + _n2 * sinphi;
                _dd = 1 / _n;
                _rho0 = _dd * Math.Sqrt(_c - _n2 * Math.Sin(Phi0));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Phi1 parameter
        /// </summary>
        protected double Phi1
        {
            get { return _phi1; }
            set { _phi1 = value; }
        }

        /// <summary>
        /// Gets or sets the Phi2 parameter
        /// </summary>
        protected double Phi2
        {
            get { return _phi2; }
            set { _phi2 = value; }
        }

        #endregion
    }
}