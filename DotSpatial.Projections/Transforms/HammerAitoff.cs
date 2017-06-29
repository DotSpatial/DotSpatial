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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 1:16:49 PM
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
    /// HammerAitoff
    /// </summary>
    public class HammerAitoff : Transform
    {
        #region Private Variables

        private double _m;
        private double _rm;
        private double _w;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HammerAitoff
        /// </summary>
        public HammerAitoff()
        {
            Proj4Name = "hammer";
            Name = "Hammer_Aitoff";
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
                double cosphi;
                double d = Math.Sqrt(2 / (1 + (cosphi = Math.Cos(lp[phi])) * Math.Cos(lp[lam] *= _w)));
                xy[x] = _m * d * cosphi * Math.Sin(lp[lam]);
                xy[y] = _rm * d * Math.Sin(lp[phi]);
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.W.HasValue)
            {
                _w = projInfo.W.Value;
                if (_w <= 0) throw new ProjectionException(27);
            }
            else
            {
                _w = .5;
            }
            if (projInfo.M.HasValue)
            {
                _m = projInfo.M.Value;
                if (_m <= 0) throw new ProjectionException(27);
            }
            else
            {
                _m = 1;
            }
            _rm = 1 / _m;
            _m /= _w;
        }

        #endregion
    }
}