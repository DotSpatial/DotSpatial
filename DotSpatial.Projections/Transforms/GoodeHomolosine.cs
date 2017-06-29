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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:58:10 AM
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
    /// GoodeHomolosine
    /// </summary>
    public class GoodeHomolosine : Transform
    {
        #region Private Variables

        private const double Y_COR = 0.05280;
        private const double PHI_LIM = .71093078197902358062;
        private Mollweide _moll;
        private Sinusoidal _sinu;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GoodeHomolosine
        /// </summary>
        public GoodeHomolosine()
        {
            Proj4Name = "goode";
            Name = "Goode_Homolosine";
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
            Array.Copy(lp, startIndex * 2, xy, startIndex * 2, numPoints * 2);
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                int phi = i * 2 + PHI;
                //int lam = i * 2 + Lambda;
                //int x = i * 2 + X;
                int y = i * 2 + Y;
                if (Math.Abs(lp[PHI]) <= PHI_LIM)
                {
                    _sinu.Forward(xy, i * 2, 1);
                }
                else
                {
                    _moll.Forward(xy, i * 2, 1);
                    xy[y] -= lp[phi] >= 0 ? Y_COR : -Y_COR;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
            Array.Copy(lp, startIndex * 2, xy, startIndex * 2, numPoints * 2);
            for (int i = startIndex; i < startIndex + numPoints; i++)
            {
                //int phi = i*2 + Phi;
                //int lam = i*2 + Lambda;
                //int x = i*2 + X;
                int y = i * 2 + Y;
                if (Math.Abs(xy[Y]) <= PHI_LIM)
                {
                    _sinu.Inverse(xy, i * 2, 1);
                }
                else
                {
                    xy[y] += xy[y] >= 0 ? Y_COR : -Y_COR;
                    _moll.Inverse(xy, i * 2, 1);
                }
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _moll = new Mollweide();
            _moll.Init(projInfo);
            _sinu = new Sinusoidal();
            _sinu.Init(projInfo);
        }

        #endregion
    }
}