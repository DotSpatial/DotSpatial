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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 11:22:02 AM
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
    /// Mollweide also acts as the base for Wag4 and Wag5
    /// </summary>
    public class Mollweide : Transform
    {
        #region Private Variables

        private const int MAX_ITER = 10;
        private const double LOOP_TOL = 1E-7;
        private double _cP;
        private double _cX;
        private double _cY;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Mollweide
        /// </summary>
        public Mollweide()
        {
            Proj4Name = "moll";
            Name = "Mollweide";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the x coefficient
        /// </summary>
        protected double Cx
        {
            get { return _cX; }
            set { _cX = value; }
        }

        /// <summary>
        /// Gets or sets the y coefficient value
        /// </summary>
        protected double Cy
        {
            get { return _cY; }
            set { _cY = value; }
        }

        /// <summary>
        /// Gets or sets the P coefficient
        /// </summary>
        protected double Cp
        {
            get { return _cP; }
            set { _cP = value; }
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

                double k = _cP * Math.Sin(lp[phi]);
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
                xy[x] = _cX * lp[lam] * Math.Cos(lp[phi]);
                xy[y] = _cY * Math.Sin(lp[phi]);
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
                lp[phi] = Proj.Aasin(xy[y] / _cY);
                lp[lam] = xy[x] / (_cX * Math.Cos(lp[phi]));
                lp[phi] += lp[phi];
                lp[phi] = Proj.Aasin((lp[phi] + Math.Sin(lp[phi])) / _cP);
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Setup(HALF_PI);
        }

        /// <summary>
        /// Finalizes the setup based on the provided P paraemter
        /// </summary>
        /// <param name="p"></param>
        protected void Setup(double p)
        {
            double p2 = p + p;
            Es = 0;
            double sp = Math.Sin(p);
            double r = Math.Sqrt(TWO_PI * sp / (p2 + Math.Sin(p2)));
            _cX = 2 * r / Math.PI;
            _cY = r / sp;
            _cP = p2 + Math.Sin(p2);
        }

        #endregion
    }
}