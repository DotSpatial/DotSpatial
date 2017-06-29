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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 11:39:09 AM
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
    /// Sinusoidal
    /// </summary>
    public class Sinusoidal : EllipticalTransform
    {
        #region Private Variables

        private double _cX;
        private double _cY;
        private double[] _en;
        private double _m;
        private double _n;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Sinusoidal
        /// </summary>
        public Sinusoidal()
        {
            Proj4Name = "sinu";
            Name = "Sinusoidal";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void SphericalForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                int lam = i * 2 + LAMBDA;
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double s, c;
                xy[x] = Proj.Mlfn(lp[phi], s = Math.Sin(lp[phi]), c = Math.Cos(lp[phi]), _en);
                xy[y] = lp[lam] * c / Math.Sqrt(1 - Es * s * s);
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
                xy[y] /= _cY;
                lp[phi] = _m > 0
                              ? Proj.Aasin((_m * xy[y] + Math.Sin(xy[y])) / _n)
                              :
                                  (_n != 1 ? Proj.Aasin(Math.Sin(xy[y]) / _n) : xy[y]);
                lp[lam] = xy[x] / (_cX * (_m + Math.Cos(xy[y])));
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
                double s, c;
                xy[y] = Proj.Mlfn(lp[phi], s = Math.Sin(lp[phi]), c = Math.Cos(lp[phi]), _en);
                xy[x] = lp[lam] * c / Math.Sqrt(1 - Es * s * s);
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
                double s;

                if ((s = Math.Abs(lp[phi] = Proj.InvMlfn(xy[y], Es, _en))) < HALF_PI)
                {
                    s = Math.Sin(lp[phi]);
                    lp[lam] = xy[x] * Math.Sqrt(1 - Es * s * s) / Math.Cos(lp[phi]);
                }
                else if ((s - EPS10) < HALF_PI)
                    lp[lam] = 0;
                else
                {
                    lp[lam] = double.NaN;
                    lp[phi] = double.NaN;
                    continue;
                    //ProjectionException(20);
                }
            }
        }

        /// <summary>
        /// Handles the original configuration of sinusoidal transforms
        /// </summary>
        protected void Setup()
        {
            _cX = (_cY = Math.Sqrt((_m + 1) / _n)) / (_m + 1);
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _en = Proj.Enfn(Es);
            if (IsElliptical == false)
            {
                _n = 1;
                _m = 0;
                Setup();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the double M value
        /// </summary>
        protected double M
        {
            get { return _m; }
            set { _m = value; }
        }

        /// <summary>
        /// Gets or sets the N value
        /// </summary>
        protected double N
        {
            get { return _n; }
            set { _n = value; }
        }

        #endregion
    }
}