// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries.
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/23/2009 9:33:11 AM
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
    /// TransverseMercator is a class allowing the transverse mercator transform as transcribed
    /// from the 4.6 version of the Proj4 library (pj_tmerc.c)
    /// </summary>
    public class Transform : ITransform
    {
        #region Private Variables

        /// <summary>
        /// Pi/2
        /// </summary>
        protected const double HALF_PI = 1.5707963267948966; //= Math.PI/2;
        /// <summary>
        /// Math.Pi / 4
        /// </summary>
        protected const double FORT_PI = 0.78539816339744833; //= Math.PI/4;
        /// <summary>
        /// 2 * Math.Pi
        /// </summary>
        protected const double TWO_PI = Math.PI * 2;
        /// <summary>
        /// 1E-10
        /// </summary>
        protected const double EPS10 = 1E-10;
        /// <summary>
        /// Analytic Hk
        /// </summary>
        protected const int IS_ANAL_HK = 4;
        /// <summary>
        /// Analytic Conv
        /// </summary>
        protected const int IS_ANAL_CONV = 10;
        /// <summary>
        /// Analytic Xl Yl
        /// </summary>
        protected const int IS_ANAL_XL_YL = 1;
        /// <summary>
        /// Analytic Xp Yp
        /// </summary>
        protected const int IS_ANAL_XP_YP = 2;
        /// <summary>
        /// Radians to Degrees
        /// </summary>
        protected const double RAD_TO_DEG = 57.29577951308232;
        /// <summary>
        /// Degrees to Radians
        /// </summary>
        protected const double DEG_TO_RAD = .0174532925199432958;

        /// <summary>
        /// The integer index representing lambda values in arrays representing geodetic coordinates
        /// </summary>
        protected const int LAMBDA = 0;
        /// <summary>
        /// The integer index representing phi values in arrays representing geodetic coordinates
        /// </summary>
        protected const int PHI = 1;
        /// <summary>
        /// The integer index representing X values in arrays representing linear coordinates
        /// </summary>
        protected const int X = 0;
        /// <summary>
        /// The integer index representing Y values in arrays representing linear coordinates
        /// </summary>
        protected const int Y = 1;

        /// <summary>
        /// The integer index representing real values in arrays representing complex numbers
        /// </summary>
        protected const int R = 0;
        /// <summary>
        /// The integer index representing immaginary values in arrays representing complex numbers
        /// </summary>
        protected const int I = 1;

        private string _esriName;
        private string[] _proj4Aliases;
        private string _proj4Name;

        /// <summary>
        /// The major axis
        /// </summary>
        protected double A { get; set; }

        /// <summary>
        /// 1/a
        /// </summary>
        protected double Ra { get; set; }

        /// <summary>
        /// 1 - e^2;
        /// </summary>
        protected double OneEs { get; set; }

        /// <summary>
        /// 1/OneEs
        /// </summary>
        protected double ROneEs { get; set; }

        /// <summary>
        /// Eccentricity
        /// </summary>
        protected double E { get; set; }

        /// <summary>
        /// True if the spheroid is flattened
        /// </summary>
        protected bool IsElliptical { get; set; }

        /// <summary>
        /// Eccentricity Squared
        /// </summary>
        protected double Es { get; set; } // eccentricity squared

        /// <summary>
        /// Central Latitude
        /// </summary>
        protected double Phi0 { get; set; } // central latitude

        /// <summary>
        /// Central Longitude
        /// </summary>
        protected double Lam0 { get; set; } // central longitude

        /// <summary>
        /// False Easting
        /// </summary>
        protected double X0 { get; set; } // easting

        /// <summary>
        /// False Northing
        /// </summary>
        protected double Y0 { get; set; } // northing

        /// <summary>
        /// Scaling Factor
        /// </summary>
        protected double K0 { get; set; } // scaling factor

        /// <summary>
        /// Scaling to meter
        /// </summary>
        protected double ToMeter { get; set; } // cartesian scaling

        /// <summary>
        /// Scaling from meter
        /// </summary>
        protected double FromMeter { get; set; } // cartesian scaling from meter

        /// <summary>
        /// The B parameter, which should be consisten with the semi-minor axis
        /// </summary>
        protected double B { get; set; }

        /// <summary>
        /// For transforms that distinguish between polar, oblique and equitorial modes
        /// </summary>
        protected enum Modes
        {
            /// <summary>
            /// North Pole
            /// </summary>
            NorthPole = 0,
            /// <summary>
            /// South Pole
            /// </summary>
            SouthPole = 1,
            /// <summary>
            /// Equitorial
            /// </summary>
            Equitorial = 2,
            /// <summary>
            /// Oblique
            /// </summary>
            Oblique = 3
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the parameters from the projection info
        /// </summary>
        /// <param name="projectionInfo">The projection information used to control this transform</param>
        public void Init(ProjectionInfo projectionInfo)
        {
            // Setup protected values common to all the projections that inherit from this projection
            Es = projectionInfo.GeographicInfo.Datum.Spheroid.EccentricitySquared();
            if (projectionInfo.LatitudeOfOrigin != null) Phi0 = projectionInfo.Phi0;
            if (projectionInfo.CentralMeridian != null) Lam0 = projectionInfo.Lam0;
            if (projectionInfo.FalseEasting != null) X0 = projectionInfo.FalseEasting.Value;
            if (projectionInfo.FalseNorthing != null) Y0 = projectionInfo.FalseNorthing.Value;
            K0 = projectionInfo.ScaleFactor;
            A = projectionInfo.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            B = projectionInfo.GeographicInfo.Datum.Spheroid.PolarRadius;
            E = projectionInfo.GeographicInfo.Datum.Spheroid.Eccentricity();
            Ra = 1 / A;
            OneEs = 1 - Es;
            ROneEs = 1 / OneEs;

            //_datumParams = proj.GeographicInfo.Datum.ToWGS84;
            if (projectionInfo.Unit != null)
            {
                ToMeter = projectionInfo.Unit.Meters;
                FromMeter = 1 / projectionInfo.Unit.Meters;
            }
            else
            {
                ToMeter = 1;
                FromMeter = 1;
            }

            if (Es != 0)
            {
                IsElliptical = true;
            }
            OnInit(projectionInfo);
        }

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop,
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        public void Forward(double[] lp, int startIndex, int numPoints)
        {
            double[] xy = new double[lp.Length];
            OnForward(lp, xy, startIndex, numPoints);
            Array.Copy(xy, startIndex * 2, lp, startIndex * 2, numPoints * 2);
        }

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop,
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        public void Inverse(double[] xy, int startIndex, int numPoints)
        {
            double[] lp = new double[xy.Length];
            OnInverse(xy, lp, startIndex, numPoints);
            Array.Copy(lp, startIndex * 2, xy, startIndex * 2, numPoints * 2);
        }

        /// <summary>
        /// Special factor calculations for a factors calculation
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection</param>
        /// <param name="fac">The Factors</param>
        public void Special(double[] lp, ProjectionInfo p, Factors fac)
        {
            OnSpecial(lp, p, fac);
        }

        /// <summary>
        /// Uses an enumeration to generate a new instance of a known transform class
        /// </summary>
        /// <param name="value">The member of the KnownTransforms to instantiate</param>
        /// <returns>A new ITransform interface representing the specific transform</returns>
        public static ITransform FromKnownTransform(KnownTransform value)
        {
            string name = value.ToString();
            foreach (ITransform transform in TransformManager.DefaultTransformManager.Transforms)
            {
                if (transform.Name == name) return transform.Copy();
            }
            return null;
        }

        /// <summary>
        /// Allows for some custom code during a process method
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection coordinate system information</param>
        /// <param name="fac">The Factors</param>
        protected virtual void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            // some projections support this as part of a process routine,
            // this will not affect forward or inverse transforms
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string name of this projection as it appears in .prj files and
        /// Esri wkt.  This can also be several names separated by a semicolon.
        /// </summary>
        public string Name
        {
            get { return _esriName; }
            set { _esriName = value; }
        }

        /// <summary>
        /// Gets or sets the string name of this projection as it should appear in proj4 strings.
        /// This can also be several names deliminated by a semicolon.
        /// </summary>
        public string Proj4Name
        {
            get { return _proj4Name; }
            protected set { _proj4Name = value; }
        }

        /// <summary>
        /// Gets or sets a list of optional aliases that can be used in the place of the Proj4Name.
        /// This will only be used during reading, and will not be used during writing.
        /// </summary>
        public string[] Proj4Aliases
        {
            get { return _proj4Aliases; }
            protected set { _proj4Aliases = value; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected virtual void OnInit(ProjectionInfo projInfo)
        {
        }

        /// <summary>
        /// Transforms cartesian x, y to geodetic lambda, phi
        /// </summary>
        /// <param name="lp">in -> the lambda, phi coordinates</param>
        /// <param name="xy">out -> the cartesian x, y</param>
        /// <param name="startIndex">The zero based starting point index.  If this is 1, for instance, reading will skip the x and y of the first point and start at the second point.</param>
        /// <param name="numPoints">The integer count of the pairs of xy values to consider.</param>
        protected virtual void OnForward(double[] lp, double[] xy, int startIndex, int numPoints)
        {
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        /// <param name="startIndex">The zero based starting point index.  If this is 1, for instance, reading will skip the x and y of the first point and start at the second point.</param>
        /// <param name="numPoints">The integer count of the pairs of xy values to consider</param>
        protected virtual void OnInverse(double[] xy, double[] lp, int startIndex, int numPoints)
        {
        }

        #endregion

        #region ITransform Members

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}