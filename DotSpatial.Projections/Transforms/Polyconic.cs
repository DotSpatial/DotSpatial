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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:15:20 PM
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
    /// Polygconic
    /// </summary>
    public class Polyconic : EllipticalTransform
    {
        #region Private Variables

        private const double TOL = 1E-10;
        private const double CONV = 1E-10;
        private const int N_ITER = 10;
        private const int ITER = 20;
        private const double ITOL = 1E-12;
        private double[] _en;
        private double _ml0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Polygconic
        /// </summary>
        public Polyconic()
        {
            Proj4Name = "poly";
            Name = "Polyconic";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (IsElliptical)
            {
                _en = Proj.Enfn(Es);
                _ml0 = Proj.Mlfn(Phi0, Math.Sin(Phi0), Math.Cos(Phi0), _en);
            }
            else
            {
                _ml0 = -Phi0;
            }
        }

        /// <inheritdoc />
        protected override void EllipticalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                if (Math.Abs(lp[phi]) <= TOL)
                {
                    xy[x] = lp[lam];
                    xy[y] = -_ml0;
                }
                else
                {
                    double sp = Math.Sin(lp[phi]);
                    double cp;
                    double ms = Math.Abs(cp = Math.Cos(lp[phi])) > TOL ? Proj.Msfn(sp, cp, Es) / sp : 0;
                    xy[x] = ms * Math.Sin(lp[lam] *= sp);
                    xy[y] = (Proj.Mlfn(lp[phi], sp, cp, _en) - _ml0) + ms * (1 - Math.Cos(lp[lam]));
                }
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
                if (Math.Abs(lp[phi]) <= TOL)
                {
                    xy[x] = lp[lam];
                    xy[y] = _ml0;
                }
                else
                {
                    double cot = 1 / Math.Tan(lp[phi]);
                    double e;
                    xy[x] = Math.Sin(e = lp[lam] * Math.Sin(lp[phi])) * cot;
                    xy[y] = lp[phi] - Phi0 + cot * (1 - Math.Cos(e));
                }
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
                xy[y] += _ml0;
                if (Math.Abs(xy[y]) <= TOL)
                {
                    lp[lam] = xy[x];
                    lp[phi] = 0;
                }
                else
                {
                    double c;
                    int j;
                    double r = xy[y] * xy[y] + xy[x] * xy[x];
                    for (lp[phi] = xy[y], j = ITER; j > 0; --j)
                    {
                        double sp = Math.Sin(lp[phi]);
                        double cp;
                        double s2Ph = sp * (cp = Math.Cos(lp[phi]));
                        if (Math.Abs(cp) < ITOL)
                        {
                            lp[lam] = double.NaN;
                            lp[phi] = double.NaN;
                            continue;
                            //ProjectionException(20);
                        }
                        double mlp;
                        c = sp * (mlp = Math.Sqrt(1 - Es * sp * sp)) / cp;
                        double ml = Proj.Mlfn(lp[phi], sp, cp, _en);
                        double mlb = ml * ml + r;
                        mlp = OneEs / (mlp * mlp * mlp);
                        double dPhi;
                        lp[phi] += (dPhi =
                                    (ml + ml + c * mlb - 2 * xy[y] * (c * ml + 1)) / (Es * s2Ph * (mlb - 2 * xy[y] * ml) / c +
                                                                                      2 * (xy[y] - ml) * (c * mlp - 1 / s2Ph) - mlp - mlp));
                        if (Math.Abs(dPhi) <= ITOL)
                            break;
                    }
                    if (j == 0)
                    {
                        lp[lam] = double.NaN;
                        lp[phi] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    c = Math.Sin(lp[phi]);
                    lp[lam] = Math.Asin(xy[x] * Math.Tan(lp[phi]) * Math.Sqrt(1 - Es * c * c)) / Math.Sin(lp[phi]);
                }
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
                if (Math.Abs(xy[y] = Phi0 + xy[y]) <= TOL)
                {
                    lp[lam] = xy[x];
                    lp[phi] = 0;
                }
                else
                {
                    lp[phi] = xy[y];
                    double b = xy[x] * xy[x] + xy[y] * xy[y];
                    int j = N_ITER;
                    double dphi;
                    do
                    {
                        double tp = Math.Tan(lp[phi]);
                        lp[phi] -= (dphi = (xy[y] * (lp[phi] * tp + 1) - lp[phi] -
                                            .5 * (lp[phi] * lp[phi] + b) * tp) /
                                           ((lp[phi] - xy[y]) / tp - 1));
                    } while (Math.Abs(dphi) > CONV && --j > 0);
                    if (j == 0)
                    {
                        lp[lam] = double.NaN;
                        lp[phi] = double.NaN;
                        continue;
                        //ProjectionException(20);
                    }
                    lp[lam] = Math.Asin(xy[x] * Math.Tan(lp[phi])) / Math.Sin(lp[phi]);
                }
            }
        }

        #endregion
    }
}