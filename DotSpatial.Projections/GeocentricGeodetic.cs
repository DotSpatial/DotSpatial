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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 10:11:03 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    /*
    * Reference...
    * ============
    * Wenzel, H.-G.(1985): Hochauflösende Kugelfunktionsmodelle für
    * das Gravitationspotential der Erde. Wiss. Arb. Univ. Hannover
    * Nr. 137, p. 130-131.

    * Programmed by GGA- Leibniz-Institue of Applied Geophysics
    *               Stilleweg 2
    *               D-30655 Hannover
    *               Federal Republic of Germany
    *               Internet: www.gga-hannover.de
    *
    *               Hannover, March 1999, April 2004.
    *               see also: comments in statements
    * remarks:
    * Mathematically exact and because of symmetry of rotation-ellipsoid,
    * each point (X, Y, Z) has at least two solutions (Latitude1, Longitude1, Height1) and
    * (Latitude2, Longitude2, Height2). Is point=(0., 0., Z) (P=0.), so you get even
    * four solutions, 	every two symmetrical to the semi-minor axis.
    * Here Height1 and Height2 have at least a difference in order of
    * radius of curvature (e.g. (0, 0, b)=> (90., 0., 0.) or (-90., 0., -2b);
    * (a+100.)*(sqrt(2.)/2., sqrt(2.)/2., 0.) => (0., 45., 100.) or
    * (0., 225., -(2a+100.))).
    * The algorithm always computes (Latitude, Longitude) with smallest |Height|.
    * For normal computations, that means |Height|<10000.m, algorithm normally
    * converges after to 2-3 steps!!!
    * But if |Height| has the amount of length of ellipsoid's axis
    * (e.g. -6300000.m), 	algorithm needs about 15 steps.
    */

    /// <summary>
    /// Wenzel, H.-G.(1985): Hochauflösende Kugelfunktionsmodelle für
    /// das Gravitationspotential der Erde. Wiss. Arb. Univ. Hannover
    /// Nr. 137, p. 130-131.
    /// </summary>
    public class GeocentricGeodetic
    {
        /* local defintions and variables */
        /* end-criterium of loop, accuracy of sin(Latitude) */
        private const double GENAU = 1E-12;
        private const double GENAU2 = GENAU * GENAU;
        private const int MAXITER = 30;
        // private const double COS_67P5 = 0.38268343236508977;  /* cosine of 67.5 degrees */
        // private const double AD_C = 1.0026000;            /* Toms region 1 constant */
        private const double PI = 3.14159265358979323e0;
        private const double PI_OVER_2 = (PI / 2.0e0);
        private readonly double _a;
        private readonly double _a2;
        private readonly double _b;
        private readonly double _b2;
        private readonly double _e2;

        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeocentricGeodetic
        /// </summary>
        public GeocentricGeodetic(Spheroid gi)
        {
            _a = gi.EquatorialRadius;
            _b = gi.PolarRadius;
            _a2 = _a * _a;
            _b2 = _b * _b;
            _e2 = (_a2 - _b2) / _a2;
            //_ep2 = (_a2 - _b2)/_b2;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts lon, lat, height to x, y, z where lon and lat are in radians and everything else is meters
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="z"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public void GeodeticToGeocentric(double[] xy, double[] z, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                GeodeticToGeocentric(ref xy[i * 2], ref xy[i * 2 + 1], ref z[i]);
            }
        }

        private void GeodeticToGeocentric(ref double x, ref double y, ref double z)
        {
            double lon = x;
            double lat = y;
            double height = z;

            /*
             * The function Convert_Geodetic_To_Geocentric converts geodetic coordinates
             * (latitude, longitude, and height) to geocentric coordinates (X, Y, Z),
             * according to the current ellipsoid parameters.
             *
             *    Latitude  : Geodetic latitude in radians                     (input)
             *    Longitude : Geodetic longitude in radians                    (input)
             *    Height    : Geodetic height, in meters                       (input)
             *    X         : Calculated Geocentric X coordinate, in meters    (output)
             *    Y         : Calculated Geocentric Y coordinate, in meters    (output)
             *    Z         : Calculated Geocentric Z coordinate, in meters    (output)
             *
             */

            /*
            ** Don't blow up if Latitude is just a little out of the value
            ** range as it may just be a rounding issue.  Also removed longitude
            ** test, it should be wrapped by cos() and sin().  NFW for PROJ.4, Sep/2001.
            */
            if (lat < -PI_OVER_2 && lat > -1.001 * PI_OVER_2)
                lat = -PI_OVER_2;
            else if (lat > PI_OVER_2 && lat < 1.001 * PI_OVER_2)
                lat = PI_OVER_2;
            else if ((lat < -PI_OVER_2) || (lat > PI_OVER_2))
            { /* lat out of range */
                x = double.NaN;
                y = double.NaN;
                z = double.NaN;
                return;
                //throw new ProjectionException(11);
            }

            if (lon > PI)
                lon -= (2 * PI);
            double sinLat = Math.Sin(lat);
            double cosLat = Math.Cos(lat);
            double sin2Lat = sinLat * sinLat;                     /*  Square of sin(Latitude)  */
            double rn = _a / (Math.Sqrt(1.0e0 - _e2 * sin2Lat)); /*  Earth radius at location  */
            x = (rn + height) * cosLat * Math.Cos(lon);
            y = (rn + height) * cosLat * Math.Sin(lon);
            z = ((rn * (1 - _e2)) + height) * sinLat;
        }

        /// <summary>
        /// Converts x, y, z to lon, lat, height
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="z"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public void GeocentricToGeodetic(double[] xy, double[] z, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                GeocentricToGeodetic(ref xy[i * 2], ref xy[i * 2 + 1], ref z[i]);
            }
        }

        //private void RalphTomsMethod(ref double x, ref double y, ref double z)
        //{
        //    double W;        /* distance from Z axis */
        //    double W2;       /* square of distance from Z axis */
        //    double T0;       /* initial estimate of vertical component */
        //    double T1;       /* corrected estimate of vertical component */
        //    double S0;       /* initial estimate of horizontal component */
        //    double S1;       /* corrected estimate of horizontal component */
        //    double Sin_B0;   /* sin(B0), B0 is estimate of Bowring aux variable */
        //    double Sin3_B0;  /* cube of sin(B0) */
        //    double Cos_B0;   /* cos(B0) */
        //    double Sin_p1;   /* sin(phi1), phi1 is estimated latitude */
        //    double Cos_p1;   /* cos(phi1) */
        //    double Rn;       /* Earth radius at location */
        //    double Sum;      /* numerator of cos(phi1) */
        //    bool At_Pole = false;     /* indicates location is in polar region */

        //    double lon;
        //    double lat = 0;
        //    double height;

        //    if (x != 0.0)
        //    {
        //        lon = Math.Atan2(y, x);
        //    }
        //    else
        //    {
        //        if (y > 0)
        //        {
        //            lon = PI / 2;
        //        }
        //        else if (y < 0)
        //        {
        //            lon = -PI_OVER_2;
        //        }
        //        else
        //        {
        //            At_Pole = true;
        //            lon = 0.0;
        //            if (z > 0.0)
        //            {  /* north pole */
        //                lat = PI_OVER_2;
        //            }
        //            else if (z < 0.0)
        //            {  /* south pole */
        //                lat = -PI_OVER_2;
        //            }
        //            else
        //            {  /* center of earth */
        //                lat = PI_OVER_2;
        //                height = -_b;

        //                x = lon;
        //                y = lat;
        //                z = height;
        //                return;
        //            }
        //        }
        //    }
        //    W2 = x * x + y * y;
        //    W = Math.Sqrt(W2);
        //    T0 = z * AD_C;
        //    S0 = Math.Sqrt(T0 * T0 + W2);
        //    Sin_B0 = T0 / S0;
        //    Cos_B0 = W / S0;
        //    Sin3_B0 = Sin_B0 * Sin_B0 * Sin_B0;
        //    T1 = z + _b * _ep2 * Sin3_B0;
        //    Sum = W - _a * _e2 * Cos_B0 * Cos_B0 * Cos_B0;
        //    S1 = Math.Sqrt(T1 * T1 + Sum * Sum);
        //    Sin_p1 = T1 / S1;
        //    Cos_p1 = Sum / S1;
        //    Rn = _a / Math.Sqrt(1.0 - _e2 * Sin_p1 * Sin_p1);
        //    if (Cos_p1 >= COS_67P5)
        //    {
        //        height = W / Cos_p1 - Rn;
        //    }
        //    else if (Cos_p1 <= -COS_67P5)
        //    {
        //        height = W / -Cos_p1 - Rn;
        //    }
        //    else
        //    {
        //        height = z / Sin_p1 + Rn * (_e2 - 1.0);
        //    }
        //    if (At_Pole == false)
        //    {
        //        lat = Math.Atan(Sin_p1 / Cos_p1);
        //    }

        //    x = lon;
        //    y = lat;
        //    z = height;
        //}

        private void IterativeMethod(ref double x, ref double y, ref double z)
        {
            double lon;
            double height;

            double cosPhi;   /* cos of searched geodetic latitude */
            double sinPhi;     /* sin of searched geodetic latitude */
            double sinDiffPhi;    /* end-criterium: addition-theorem of sin(Latitude(iter)-Latitude(iter-1)) */
            //bool At_Pole = false;     /* indicates location is in polar region */

            double p = Math.Sqrt(x * x + y * y);
            double rr = Math.Sqrt(x * x + y * y + z * z); /* distance between center and location */

            /*	special cases for latitude and longitude */
            if (p / _a < GENAU)
            {
                /*  special case, if P=0. (X=0., Y=0.) */
                //At_Pole = true;
                lon = 0.0;

                /*  if (X, Y, Z)=(0., 0., 0.) then Height becomes semi-minor axis
                 *  of ellipsoid (=center of mass), Latitude becomes PI/2 */
                if (rr / _a < GENAU)
                {
                    //lat = PI_OVER_2;
                    //height   = -_b;
                    return;
                }
            }
            else
            {
                /*  ellipsoidal (geodetic) longitude
                 *  interval: -PI < Longitude <= +PI */
                lon = Math.Atan2(y, x);
            }

            /* --------------------------------------------------------------
             * Following iterative algorithm was developped by
             * "Institut für Erdmessung", University of Hannover, July 1988.
             * Internet: www.ife.uni-hannover.de
             * Iterative computation of CPHI, SPHI and Height.
             * Iteration of CPHI and SPHI to 10**-12 radian resp.
             * 2*10**-7 arcsec.
             * --------------------------------------------------------------
             */
            double ct = z / rr; // sin of geocentric latitude // looks like these two should be flipped (TD).
            double st = p / rr; // cos of geocentric latitude
            double rx = 1.0 / Math.Sqrt(1.0 - _e2 * (2.0 - _e2) * st * st);
            double cosPhi0 = st * (1.0 - _e2) * rx; /* cos of start or old geodetic latitude in iterations */
            double sinPhi0 = ct * rx; /* sin of start or old geodetic latitude in iterations */
            int iter = 0; /* # of continous iteration, max. 30 is always enough (s.a.) */

            /* loop to find sin(Latitude) resp. Latitude
             * until |sin(Latitude(iter)-Latitude(iter-1))| < genau */
            do
            {
                iter++;
                double earthRadius = _a / Math.Sqrt(1.0 - _e2 * sinPhi0 * sinPhi0);

                /*  ellipsoidal (geodetic) height */
                height = p * cosPhi0 + z * sinPhi0 - earthRadius * (1.0 - _e2 * sinPhi0 * sinPhi0);

                double rk = _e2 * earthRadius / (earthRadius + height);
                rx = 1.0 / Math.Sqrt(1.0 - rk * (2.0 - rk) * st * st);
                cosPhi = st * (1.0 - rk) * rx;
                sinPhi = ct * rx;
                sinDiffPhi = sinPhi * cosPhi0 - cosPhi * sinPhi0;
                cosPhi0 = cosPhi;
                sinPhi0 = sinPhi;
            }
            while (sinDiffPhi * sinDiffPhi > GENAU2 && iter < MAXITER);

            /*	ellipsoidal (geodetic) latitude */
            double lat = Math.Atan(sinPhi / Math.Abs(cosPhi));

            x = lon;
            y = lat;
            z = height;
            return;
        }

        /// <summary>
        /// Converts geocentric x, y, z coords to geodetic lon, lat, h
        /// </summary>
        private void GeocentricToGeodetic(ref double x, ref double y, ref double z)
        {
            IterativeMethod(ref x, ref y, ref z);
        }

        ///// <summary>
        ///// Converts geocentric x, y, z coords to geodetic lon, lat, h
        ///// </summary>
        //private void GeocentricToGeodeticOld(ref double x, ref double y, ref double z)
        //{
        //    double lat;
        //    double lon;
        //    double height;

        //    double cphi;     /* cos of searched geodetic latitude */
        //    double sphi;     /* sin of searched geodetic latitude */
        //    double sdphi;    /* end-criterium: addition-theorem of sin(Latitude(iter)-Latitude(iter-1)) */

        //    double p = Math.Sqrt(x*x+y*y);
        //    double rr = Math.Sqrt(x*x+y*y+z*z);

        //    /*	special cases for latitude and longitude */
        //    if (p/_a < GENAU)
        //    {
        //       /*  special case, if P=0. (X=0., Y=0.) */
        //        lon = 0;

        //        /*  if (X, Y, Z)=(0., 0., 0.) then Height becomes semi-minor axis
        //         *  of ellipsoid (=center of mass), Latitude becomes PI/2 */
        //        if (rr/_a < GENAU)
        //        {
        //            lat = PI/2;
        //            height = -_b;
        //            x = lon;
        //            y = lat;
        //            z = height;
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        /*  ellipsoidal (geodetic) longitude
        //         *  interval: -PI < Longitude <= +PI */
        //        lon = Math.Atan2(y, x);
        //    }

        //    /* --------------------------------------------------------------
        //     * Following iterative algorithm was developped by
        //     * "Institut für Erdmessung", University of Hannover, July 1988.
        //     * Internet: www.ife.uni-hannover.de
        //     * Iterative computation of CPHI, SPHI and Height.
        //     * Iteration of CPHI and SPHI to 10**-12 radian resp.
        //     * 2*10**-7 arcsec.
        //     * --------------------------------------------------------------
        //     */
        //    double ct = z/rr;
        //    double st = p/rr;
        //    double rx = 1.0/Math.Sqrt(1.0-_e2*(2.0-_e2)*st*st);
        //    double cphi0 = st*(1.0-_e2)*rx;
        //    double sphi0 = ct * rx; /* sin of start or old geodetic latitude in iterations */
        //    int iter = 0;

        //    /* loop to find sin(Latitude) resp. Latitude
        //     * until |sin(Latitude(iter)-Latitude(iter-1))| < genau */
        //    do
        //    {
        //        iter++;
        //        double rn = _a/Math.Sqrt(1.0-_e2*sphi0*sphi0);       /* Earth radius at location */

        //        /*  ellipsoidal (geodetic) height */
        //        height = p*cphi0+z*sphi0-rn*(1.0-_e2*sphi0*sphi0);

        //        double rk = _e2*rn/(rn+height);
        //        rx = 1.0/Math.Sqrt(1.0-rk*(2.0-rk)*st*st);
        //        cphi = st*(1.0-rk)*rx;
        //        sphi = ct*rx;
        //        sdphi = sphi*cphi0-cphi*sphi0;
        //        cphi0 = cphi;
        //        sphi0 = sphi;
        //    }
        //    while (sdphi*sdphi > GENAU2 && iter < MAXITER);

        //    /*	ellipsoidal (geodetic) latitude */
        //    lat=Math.Atan(sphi/Math.Abs(cphi));
        //    x = lon;
        //    y = lat;
        //    z = height;
        //    return;
        //}

        #endregion

        #region Properties

        #endregion

        //private readonly double _ep2;
    }
}