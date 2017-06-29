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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/20/2009 5:01:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /// <summary>
    /// MeridinalDistance
    /// </summary>
    public static class MeridionalDistance
    {
        private const double C00 = 1;
        private const double C02 = 0.25;
        private const double C04 = .046875;
        private const double C06 = .01953125;
        private const double C08 = .01068115234375;
        private const double C22 = .75;
        private const double C44 = .46875;
        private const double C46 = .01302083333333333333;
        private const double C48 = .00712076822916666666;
        private const double C66 = .36458333333333333333;
        private const double C68 = .00569661458333333333;
        private const double C88 = .3076171875;
        private const double EPS = 1e-11;
        private const int MAX_ITER = 10;
        private const int EN_SIZE = 5;

        /// <summary>
        /// Formerly pj_enfn from Proj4
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        public static double[] GetEn(double es)
        {
            double t;
            double[] en = new double[EN_SIZE];
            en[0] = C00 - es * (C02 + es * (C04 + es * (C06 + es * C08)));
            en[1] = es * (C22 - es * (C04 + es * (C06 + es * C08)));
            en[2] = (t = es * es) * (C44 - es * (C46 + es * C48));
            en[3] = (t *= es) * (C66 - es * C68);
            en[4] = t * es * C88;
            return en;
        }

        /// <summary>
        /// Formerly pj_mlfn
        /// Given geodetic angular displacement phi, this calculates the equivalent meridional distance
        /// </summary>
        /// <param name="phi">The geodetic angular displacement</param>
        /// <param name="sphi"></param>
        /// <param name="cphi"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static double MeridionalLength(double phi, double sphi, double cphi, double[] en)
        {
            cphi *= sphi;
            sphi *= sphi;
            return (en[0] * phi - cphi * (en[1] + sphi * (en[2] + sphi * (en[3] + sphi * en[4]))));
        }

        /// <summary>
        /// Formerly pj_inv_mlfn
        /// Given the linear distance, this calculates the equivalent geodetic angular displacement
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="es"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static double AngularDistance(double arg, double es, double[] en)
        {
            double k = 1 / (1 - es);
            double phi = arg;
            for (int i = MAX_ITER; i > 0; --i)
            { /* rarely goes over 2 iterations */
                double s = Math.Sin(phi);
                double t = 1 - es * s * s;
                phi -= t = (MeridionalLength(phi, s, Math.Cos(phi), en) - arg) * (t * Math.Sqrt(t)) * k;
                if (Math.Abs(t) < EPS)
                    return phi;
            }
            return phi;
            // throw new ProjectionException(17);
        }
    }
}