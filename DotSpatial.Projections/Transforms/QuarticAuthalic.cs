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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:31:25 PM
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
    /// QuarticAuthalic
    /// </summary>
    public class QuarticAuthalic : Transform
    {
        #region Private Variables

        private double _cP;
        private double _cX;
        private double _cY;
        private bool _tanMode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of QuarticAuthalic
        /// </summary>
        public QuarticAuthalic()
        {
            Proj4Name = "qua_aut";
            Name = "Quartic_Authalic";
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
                xy[x] = _cX * lp[lam] * Math.Cos(lp[phi]);
                xy[y] = _cY;
                lp[phi] *= _cP;
                double c = Math.Cos(lp[phi]);
                if (_tanMode)
                {
                    xy[x] *= c * c;
                    xy[y] *= Math.Tan(lp[phi]);
                }
                else
                {
                    xy[x] /= c;
                    xy[y] *= Math.Sin(lp[phi]);
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
                xy[y] /= _cY;
                double c = Math.Cos(lp[phi] = _tanMode ? Math.Atan(xy[y]) : Proj.Aasin(xy[y]));
                lp[phi] /= _cP;
                lp[lam] = xy[x] / (_cX * Math.Cos(lp[phi] /= _cP));
                if (_tanMode)
                    lp[lam] /= c * c;
                else
                    lp[lam] *= c;
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Setup(2, 2, false);
        }

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="mode"></param>
        protected void Setup(double p, double q, bool mode)
        {
            _cX = q / p;
            _cY = p;
            _cP = 1 / q;
            _tanMode = mode;
        }

        #endregion

        #region Properties

        #endregion
    }
}