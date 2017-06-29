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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 1:59:49 PM
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
    /// Loximuthal
    /// </summary>
    public class Loximuthal : Transform
    {
        #region Private Variables

        private const double EPS = 1E-8;
        private double _cosphi1;
        private double _phi1;
        private double _tanphi1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Loximuthal
        /// </summary>
        public Loximuthal()
        {
            Proj4Name = "loxim";
            Name = "Loximuthal";
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
                xy[y] = lp[phi] - _phi1;
                if (Math.Abs(xy[y]) < EPS)
                    xy[x] = lp[lam] * _cosphi1;
                else
                {
                    xy[x] = FORT_PI + 0.5 * lp[phi];
                    if (Math.Abs(xy[x]) < EPS || Math.Abs(Math.Abs(xy[x]) - HALF_PI) < EPS)
                        xy[x] = 0;
                    else
                        xy[x] = lp[lam] * xy[y] / Math.Log(Math.Tan(xy[x]) / _tanphi1);
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
                lp[phi] = xy[y] + _phi1;
                if (Math.Abs(xy[y]) < EPS)
                    lp[lam] = xy[x] / _cosphi1;
                else if (Math.Abs(lp[lam] = FORT_PI + 0.5 * lp[phi]) < EPS ||
                         Math.Abs(Math.Abs(lp[lam]) - HALF_PI) < EPS)
                    lp[lam] = 0;
                else
                    lp[lam] = xy[x] * Math.Log(Math.Tan(lp[lam]) / _tanphi1) / xy[y];
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.StandardParallel1 != null)
            {
                _phi1 = projInfo.Phi1;
                _cosphi1 = Math.Cos(_phi1);
                if (_cosphi1 < EPS) throw new ProjectionException(22);
            }
            _tanphi1 = Math.Tan(FORT_PI + 0.5 * _phi1);
        }

        #endregion
    }
}