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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:39:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// PutinsP1
    /// </summary>
    public class PutinsP1 : Transform
    {
        #region Private Variables

        private const double CX = 1.89490;
        private const double CY = 0.94745;
        private const double CA = -0.5;
        private const double CB = 0.30396355092701331433;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PutinsP1
        /// </summary>
        public PutinsP1()
        {
            Proj4Name = "putp1";
            Name = "Putins_P1";
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
                xy[y] = CY * lp[phi];
                xy[x] = CX * lp[lam] * (CA + Proj.Asqrt(1 - CB * lp[phi] * lp[phi]));
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
                lp[phi] = xy[y] / CY;
                lp[lam] = xy[x] / (CX * (CA + Proj.Asqrt(1 - CB * lp[phi] * lp[phi])));
            }
        }

        #endregion
    }
}