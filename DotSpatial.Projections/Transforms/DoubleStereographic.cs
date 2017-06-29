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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 9:06:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************
// Bart Adriaanse      | 30/11/2013 |  Copied from ObliqueStereographicAlternative as this is supposed to be equivalent
// (Deltares)

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// DoubleStereographic
    /// </summary>
    public class DoubleStereographic : Transform
    {
        #region Private Variables

        private double _cosc0;
        private Gauss _gauss;
        private double _phic0;
        private double _r2;
        private double _sinc0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DoubleStereographic
        /// </summary>
        public DoubleStereographic()
        {
            Proj4Name = "sterea";
            Name = "Double_Stereographic";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            Array.Copy(lp, startIndex * 2, xy, startIndex * 2, numPoints * 2);
            _gauss.Forward(xy, startIndex, numPoints);
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                // Grab from xy instead of lp because of the Gauss transform
                double phi = xy[i * 2 + PHI];
                double lam = xy[i * 2 + LAMBDA];
                int x = i * 2 + X;
                int y = i * 2 + Y;
                double sinc = Math.Sin(phi);
                double cosc = Math.Cos(phi);
                double cosl = Math.Cos(lam);
                double k = K0 * _r2 / (1 + _sinc0 * sinc + _cosc0 * cosc * cosl);
                xy[x] = k * cosc * Math.Sin(lam);
                xy[y] = k * (_cosc0 * sinc - _sinc0 * cosc * cosl);
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
                double rho;

                xy[x] /= K0;
                xy[y] /= K0;
                if ((rho = Proj.Hypot(xy[x], xy[y])) > 0)
                {
                    double c = 2 * Math.Atan2(rho, _r2);
                    double sinc = Math.Sin(c);
                    double cosc = Math.Cos(c);
                    lp[phi] = Math.Asin(cosc * _sinc0 + xy[y] * sinc * _cosc0 / rho);
                    lp[lam] = Math.Atan2(xy[x] * sinc, rho * _cosc0 * cosc -
                                                       xy[y] * _sinc0 * sinc);
                }
                else
                {
                    lp[phi] = _phic0;
                    lp[lam] = 0;
                }
            }
            _gauss.Inverse(lp, startIndex, numPoints);
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double r = 0;
            _gauss = new Gauss(E, Phi0, ref _phic0, ref r);
            _sinc0 = Math.Sin(_phic0);
            _cosc0 = Math.Cos(_phic0);
            _r2 = 2 * r;
        }

        #endregion

        #region Properties

        #endregion
    }
}