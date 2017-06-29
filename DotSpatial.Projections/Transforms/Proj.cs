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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/31/2009 3:38:45 PM
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
    /// Proj  contains frequently used static helper methods for doing projection calculations
    /// </summary>
    public static class Proj
    {
        /// <summary>
        /// Effectively 1 but with a tolerance of 1E-14
        /// </summary>
        private const double ONE_TOL = 1.00000000000001;
        /// <summary>
        /// 1E-50
        /// </summary>
        private const double ATOL = 1E-50;

        private const int R = 0;
        private const int I = 1;

        /// <summary>
        /// Tolerant Arcsin
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Aasin(double v)
        {
            double av;
            if ((av = Math.Abs(v)) >= 1)
            {
                if (av < ONE_TOL)
                {
                    return double.NaN;
                    //ProjectionException(19);
                }
                return v < 0 ? -Math.PI / 2 : Math.PI / 2;
            }
            return Math.Asin(v);
        }

        /// <summary>
        /// Tolerant ArcCosine
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Aacos(double v)
        {
            double av;
            if ((av = Math.Abs(v)) >= 1)
            {
                if (av < ONE_TOL)
                {
                    return double.NaN;
                    //ProjectionException(19);
                }
                return v < 0 ? Math.PI : 0;
            }
            return Math.Acos(v);
        }

        /// <summary>
        /// Tollerant Sqrt
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Asqrt(double v)
        {
            return ((v <= 0) ? 0 : Math.Sqrt(v));
        }

        /// <summary>
        /// Tollerant Math.Atan method.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Aatan2(double n, double d)
        {
            return ((Math.Abs(n) < ATOL && Math.Abs(d) < ATOL) ? 0 : Math.Atan2(n, d));
        }

        /// <summary>
        /// Calculates the hypotenuse of a triangle: Sqrt(x*x + y*y);
        /// </summary>
        /// <param name="x">The length of one orthogonal leg of the triangle</param>
        /// <param name="y">The length of the other orthogonal leg of the triangle</param>
        /// <returns>The length of the diagonal.</returns>
        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double Adjlon(double lon)
        {
            if (Math.Abs(lon) <= Math.PI) return (lon);
            lon += Math.PI;  /* adjust to 0..2pi rad */
            lon -= 2 * Math.PI * Math.Floor(lon / (2 * Math.PI)); /* remove integral # of 'revolutions'*/
            lon -= Math.PI;  /* adjust back to -pi..pi rad */
            return (lon);
        }

        /// <summary>
        /// Determines latitude from authalic latitude
        /// </summary>
        /// <param name="es">Epsilon squared</param>
        /// <returns>The array of double values for the apa parameter</returns>
        public static double[] Authset(double es)
        {
            const double p00 = .33333333333333333333;
            const double p01 = .17222222222222222222;
            const double p02 = .10257936507936507936;
            const double p10 = .06388888888888888888;
            const double p11 = .06640211640211640211;
            const double p20 = .01641501294219154443;
            const int apaSize = 3;
            double[] apa = new double[apaSize];
            apa[0] = es * p00;
            double t = es * es;
            apa[0] += t * p01;
            apa[1] = t * p10;
            t *= es;
            apa[0] += t * p02;
            apa[1] += t * p11;
            apa[2] = t * p20;
            return apa;
        }

        /// <summary>
        /// Obtains the authalic latitude
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="apa"></param>
        /// <returns></returns>
        public static double AuthLat(double beta, double[] apa)
        {
            double t = beta + beta;
            return (beta + apa[0] * Math.Sin(t) + apa[1] * Math.Sin(t + t) + apa[2] * Math.Sin(t + t + t));
        }

        /// <summary>
        /// Obtains the EN parameters for the Meridional distance
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        public static double[] Enfn(double es)
        {
            return MeridionalDistance.GetEn(es);
        }

        /// <summary>
        /// Meridonal length function
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="sphi"></param>
        /// <param name="cphi"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static double Mlfn(double phi, double sphi, double cphi, double[] en)
        {
            return MeridionalDistance.MeridionalLength(phi, sphi, cphi, en);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="es"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static double InvMlfn(double arg, double es, double[] en)
        {
            return MeridionalDistance.AngularDistance(arg, es, en);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sinphi"></param>
        /// <param name="e"></param>
        /// <param name="oneEs"></param>
        /// <returns></returns>
        public static double Qsfn(double sinphi, double e, double oneEs)
        {
            if (e >= 1E-7)
            {
                double con = e * sinphi;
                return (oneEs * (sinphi / (1 - con * con) - (.5 / e) * Math.Log((1 - con) / (1 + con))));
            }
            return sinphi + sinphi;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="sinphi"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double Tsfn(double phi, double sinphi, double e)
        {
            sinphi *= e;
            double result = (Math.Tan(.5 * (Math.PI / 2 - phi))) / Math.Pow((1 - sinphi) / (1 + sinphi), .5 * e);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sinphi"></param>
        /// <param name="cosphi"></param>
        /// <param name="es"></param>
        /// <returns></returns>
        public static double Msfn(double sinphi, double cosphi, double es)
        {
            return (cosphi / Math.Sqrt(1 - es * sinphi * sinphi));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double Phi2(double ts, double e)
        {
            double dphi;
            const double tol = 1E-10;
            double eccnth = .5 * e;
            double phi = Math.PI / 2 - 2 * Math.Atan(ts);
            int i = 15;
            do
            {
                double con = e * Math.Sin(phi);
                dphi = Math.PI / 2 - 2 * Math.Atan(ts * Math.Pow((1 - con) / (1 + con), eccnth)) - phi;
                phi += dphi;
            } while ((Math.Abs(dphi) > tol) && (--i > 0));
            if (i <= 0)
            {
                return double.NaN;
                //ProjectionException(18);
            }
            return phi;
        }

        ///<summary>
        ///</summary>
        ///<param name="z"></param>
        ///<param name="c"></param>
        ///<param name="n"></param>
        ///<returns></returns>
        public static double[] Zpoly1(double[] z, double[][] c, int n)
        {
            double t;

            // make a copy of the array that we are going to modify and return.
            int size = c[n].Length;
            double[] a = new double[size];
            Array.Copy(c[n], a, size);

            while (n-- > 0)
            {
                a[R] = c[n][R] + z[R] * (t = a[R]) - z[I] * a[I];
                a[I] = c[n][I] + z[R] * a[I] + z[I] * t;
            }
            a[R] = z[R] * (t = a[R]) - z[I] * a[I];
            a[I] = z[R] * a[I] + z[I] * t;
            return a;
        }

        ///<summary>
        ///</summary>
        ///<param name="z"></param>
        ///<param name="c"></param>
        ///<param name="n"></param>
        ///<param name="der"></param>
        ///<returns></returns>
        public static double[] Zpolyd1(double[] z, double[][] c, int n, out double[] der)
        {
            double t;
            double[] b = new double[2];
            bool first = true;
            double[] a = c[n];
            while (n-- > 0)
            {
                if (first)
                {
                    first = false;
                    b = a;
                }
                else
                {
                    b[R] = a[R] + z[R] * (t = b[R]) - z[I] * b[I];
                    b[I] = a[I] + z[R] * b[I] + z[I] * t;
                }
                a[R] = c[n][R] + z[R] * (t = a[R]) - z[I] * a[I];
                a[I] = c[n][I] + z[R] * a[I] + z[I] * t;
            }
            b[R] = a[R] + z[R] * (t = b[R]) - z[I] * b[I];
            b[I] = a[I] + z[R] * b[I] + z[I] * t;
            a[R] = z[R] * (t = a[R]) - z[I] * z[I];
            a[I] = z[R] * a[I] + z[I] * t;
            der = b;
            return a;
        }
    }
}