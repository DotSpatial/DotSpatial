// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries.
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 2:26:13 PM
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
    /// WinkelTripel
    /// </summary>
    public class WinkelTripel : Transform
    {
        #region Private Variables

        private double _cosphi1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WinkelTripel
        /// </summary>
        public WinkelTripel()
        {
            Name = "Winkel_Tripel";
            Proj4Name = "wintri";
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
                double c, d;

                if ((d = Math.Acos(Math.Cos(lp[phi]) * Math.Cos(c = 0.5 * lp[lam]))) != 0)
                {
                    xy[x] = 2 * d * Math.Cos(lp[phi]) * Math.Sin(c) * (xy[y] = 1 / Math.Sin(d));
                    xy[y] *= d * Math.Sin(lp[phi]);
                }
                else
                    xy[x] = xy[y] = 0;

                xy[x] = (xy[x] + lp[lam] * _cosphi1) * 0.5;
                xy[y] = (xy[y] + lp[phi]) * 0.5;
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
                _cosphi1 = Math.Cos(projInfo.Phi1);
                if (_cosphi1 == 0) throw new ProjectionException(22);
            }
            else
            {
                /* 50d28' or acos(2/pi) */
                _cosphi1 = 0.636619772367581343;
            }
        }

        #endregion
    }
}