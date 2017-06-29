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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 3:23:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Factors
    /// </summary>
    public class Factors
    {
        /// <summary>
        /// max scale error
        /// </summary>
        public double A;
        /// <summary>
        /// min scale error
        /// </summary>
        public double B;
        /// <summary>
        /// Info as to analytics
        /// </summary>
        public AnalyticModes Code;

        /// <summary>
        /// Convergence
        /// </summary>
        public double Conv;

        /// <summary>
        /// Meridinal scale
        /// </summary>
        public double H;
        /// <summary>
        /// parallel scale
        /// </summary>
        public double K;
        /// <summary>
        /// Angular distortion
        /// </summary>
        public double Omega;

        /// <summary>
        /// Areal scale factor
        /// </summary>
        public double S;

        /// <summary>
        /// theta prime
        /// </summary>
        public double Thetap;

        /// <summary>
        /// derivatives of x for lambda
        /// </summary>
        public double Xl;
        /// <summary>
        /// derivatives of x for phi
        /// </summary>
        public double Xp;
        /// <summary>
        /// derivatives of y for lambda
        /// </summary>
        public double Yl;
        /// <summary>
        /// derivatives of y for phi
        /// </summary>
        public double Yp;
    }
}