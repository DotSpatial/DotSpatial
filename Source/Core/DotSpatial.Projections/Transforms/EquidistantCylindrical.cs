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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 5:08:43 PM
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
    /// EquidistantCylindrical
    /// </summary>
    public class EquidistantCylindrical : Transform
    {
        #region Private Variables

        private double _rc;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of EquidistantCylindrical
        /// </summary>
        public EquidistantCylindrical()
        {
            Proj4Name = "eqc";
            Name = "Equidistant_Cylindrical";
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
                xy[x] = _rc * lp[lam];
                xy[y] = lp[phi] - Phi0;
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
                lp[lam] = xy[x] / _rc;
                lp[phi] = xy[y] + Phi0;
            }
        }

        /// <inheritdoc />
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double phi = 0;
            if (projInfo.StandardParallel1 != null) phi = projInfo.StandardParallel1.Value * Math.PI / 180;
            _rc = Math.Cos(phi);
            if (_rc <= 0) throw new ProjectionException(24);
        }

        #endregion
    }
}