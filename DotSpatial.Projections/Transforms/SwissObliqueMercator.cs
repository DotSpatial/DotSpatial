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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 10:09:09 AM
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
    /// SwissObliqueMercator
    /// </summary>
    public class SwissObliqueMercator : Transform
    {
        #region Private Variables

        private const double EPS = 1E-10;
        private const int NITER = 6;
        private double _c;
        private double _cosp0;
        private double _hlfE;
        private double _k;
        private double _kR;
        private double _sinp0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SwissObliqueMercator
        /// </summary>
        public SwissObliqueMercator()
        {
            Proj4Name = "somerc";
            Name = "Swiss_Oblique_Mercator";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double phip0;

            _hlfE = 0.5 * E;
            double cp = Math.Cos(Phi0);
            cp *= cp;
            _c = Math.Sqrt(1 + Es * cp * cp * ROneEs);
            double sp = Math.Sin(Phi0);
            _cosp0 = Math.Cos(phip0 = Proj.Aasin(_sinp0 = sp / _c));
            sp *= E;
            _k = Math.Log(Math.Tan(FORT_PI + 0.5 * phip0)) - _c * (
                                                                      Math.Log(Math.Tan(FORT_PI + 0.5 * Phi0)) - _hlfE *
                                                                      Math.Log((1 + sp) / (1 - sp)));
            _kR = K0 * Math.Sqrt(OneEs) / (1 - sp * sp);
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
                double sp = E * Math.Sin(lp[phi]);
                double phip = 2 * Math.Atan(Math.Exp(_c * (
                                                              Math.Log(Math.Tan(FORT_PI + 0.5 * lp[phi])) -
                                                              _hlfE * Math.Log((1 + sp) / (1 - sp)))
                                                     + _k)) - HALF_PI;
                double lamp = _c * lp[lam];
                double cp = Math.Cos(phip);
                double phipp = Proj.Aasin(_cosp0 * Math.Sin(phip) - _sinp0 * cp * Math.Cos(lamp));
                double lampp = Proj.Aasin(cp * Math.Sin(lamp) / Math.Cos(phipp));
                xy[x] = _kR * lampp;
                xy[y] = _kR * Math.Log(Math.Tan(FORT_PI + 0.5 * phipp));
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
                int j;

                double phipp = 2 * (Math.Atan(Math.Exp(xy[y] / _kR)) - FORT_PI);
                double lampp = xy[x] / _kR;
                double cp = Math.Cos(phipp);
                double phip = Proj.Aasin(_cosp0 * Math.Sin(phipp) + _sinp0 * cp * Math.Cos(lampp));
                double lamp = Proj.Aasin(cp * Math.Sin(lampp) / Math.Cos(phip));
                double con = (_k - Math.Log(Math.Tan(FORT_PI + 0.5 * phip))) / _c;
                for (j = NITER; j > 0; --j)
                {
                    double esp = E * Math.Sin(phip);
                    double delp = (con + Math.Log(Math.Tan(FORT_PI + 0.5 * phip)) - _hlfE *
                                   Math.Log((1 + esp) / (1 - esp))) *
                                  (1 - esp * esp) * Math.Cos(phip) * ROneEs;
                    phip -= delp;
                    if (Math.Abs(delp) < EPS)
                        break;
                }
                if (j <= 0)
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //ProjectionException(20);
                }
                lp[phi] = phip;
                lp[lam] = lamp / _c;
            }
        }

        #endregion
    }
}