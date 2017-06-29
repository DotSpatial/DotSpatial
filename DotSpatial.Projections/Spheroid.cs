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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:09:16 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Spheroid (Defaults to WGS84)
    /// </summary>
    [Serializable]
    public class Spheroid : ProjDescriptor, IEsriString
    {
        #region Private Variables

        private string _code;
        private double _equatorialRadius;
        private Proj4Ellipsoid _knownEllipsoid;
        private string _name;
        private double _polarRadius;
        private Dictionary<Proj4Ellipsoid, string> _proj4Names;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a new instance of Spheroid
        /// </summary>
        public Spheroid()
        {
            AddNames();
            AssignKnownEllipsoid(Proj4Ellipsoid.WGS_1984);
        }

        /// <summary>
        /// Creates a new spheroid using an the equatorial radius in meters and
        /// a flattening coefficient that is the inverse flattening factor.
        /// eg. for WGS84 (6378137.0, 298.257223563)
        /// </summary>
        /// <param name="equatorialRadius">The semi-major axis</param>
        /// <param name="inverseFlattening">The inverse of the flattening factor</param>
        public Spheroid(double equatorialRadius, double inverseFlattening)
        {
            _equatorialRadius = equatorialRadius;
            InverseFlattening = inverseFlattening;
            AddNames();
            AssignKnownEllipsoid(Proj4Ellipsoid.WGS_1984);
        }

        /// <summary>
        /// For perfect spheres, you just need to specify one radius, which will be
        /// applied to both radii.  You can then directly change the polar or
        /// equatorial radius if necessary using the properties.
        /// </summary>
        /// <param name="radius">The radius of the sphere</param>
        public Spheroid(double radius)
        {
            _polarRadius = radius;
            _equatorialRadius = radius;
            AddNames();
        }

        /// <summary>
        /// The ellps parameter in a proj4 string will only work with certain
        /// pre-defined spheroids, enumerated in the Proj4Ellipsoids enumeration.
        /// Custom spheroids can be specified but will use the a and b parameters
        /// when creating a proj4 parameter instead of using the ellps parameter.
        /// </summary>
        /// <param name="knownEllipse">Any of several predefined geographic ellipses</param>
        public Spheroid(Proj4Ellipsoid knownEllipse)
        {
            AddNames();
            AssignKnownEllipsoid(knownEllipse);
        }

        /// <summary>
        /// Given the proj4 code, this will set the radii correctly.
        /// </summary>
        /// <param name="proj4Ellips"></param>
        public Spheroid(string proj4Ellips)
        {
            AddNames();
            foreach (KeyValuePair<Proj4Ellipsoid, string> pair in _proj4Names)
            {
                if (pair.Value == proj4Ellips)
                {
                    AssignKnownEllipsoid(pair.Key);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets or sets the two character Synthetic Environment Data
        /// Representation and Interchange Specification (SEDRIS) code
        /// eg. AA represents Airy 1830.  Setting this will not modify the values,
        /// To read a SEDRIS Code use the constructor or AssignKnownEllipsoid overload.
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// Assigns a known projection that is defined by a two character SEDRIS code,
        /// using nothing but that code.
        /// </summary>
        /// <param name="code">The two character SEDRIS code defining 22 distinct ellipsoids.</param>
        public void ReadSedrisCode(string code)
        {
            _code = code;
            switch (code)
            {
                case "AA":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Airy_1830);
                    break;
                case "AM":
                    AssignKnownEllipsoid(Proj4Ellipsoid.AiryModified);
                    break;
                case "AN":
                case "SA":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Austrailia_SouthAmerica);
                    break;
                case "BR":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Bessel_1841);
                    break;
                case "BN":
                    AssignKnownEllipsoid(Proj4Ellipsoid.BesselNamibia);
                    break;
                case "CC":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Clarke_1866);
                    break;
                case "CD":
                    AssignKnownEllipsoid(Proj4Ellipsoid.ClarkeModified_1880);
                    break;
                case "EA":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_1830);
                    break;
                case "EE":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_1948);
                    break;
                case "EC":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_1956);
                    break;
                case "ED":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_1969);
                    break;
                case "EF":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_Pakistan);
                    break;
                case "EB":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Everest_SS);
                    break;
                case "FA":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Fischer_1960);
                    break;
                case "RF":
                    AssignKnownEllipsoid(Proj4Ellipsoid.GRS_1980);
                    break;
                case "HE":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Helmert_1906);
                    break;
                case "HO":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Hough);
                    break;
                case "ID":
                    _equatorialRadius = 6378160;
                    InverseFlattening = 298.247;
                    break;
                case "IN":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Indonesian_1974);
                    break;
                case "KA":
                    AssignKnownEllipsoid(Proj4Ellipsoid.Krassovsky_1942);
                    break;
                case "WD":
                    AssignKnownEllipsoid(Proj4Ellipsoid.WGS_1972);
                    break;
                case "WE":
                    AssignKnownEllipsoid(Proj4Ellipsoid.WGS_1984);
                    break;
            }
        }

        /// <summary>
        /// Given the enumeration of known ellipsoids, this will redefine this spheroid
        /// so that it matches the A and B coefficients for the known ellipsoids.
        /// </summary>
        /// <param name="knownEllipse">The known Ellipse</param>
        public void AssignKnownEllipsoid(Proj4Ellipsoid knownEllipse)
        {
            _name = knownEllipse.ToString();
            _knownEllipsoid = knownEllipse;
            switch (knownEllipse)
            {
                case Proj4Ellipsoid.Airy_1830:
                    _equatorialRadius = 6377563.396;
                    _polarRadius = 6356256.910;
                    _code = "AA";
                    break;
                case Proj4Ellipsoid.AiryModified:
                    _equatorialRadius = 6377340.189;
                    _polarRadius = 6356034.446;
                    _code = "AM";
                    break;
                case Proj4Ellipsoid.Andrae_1876:
                    _equatorialRadius = 6377104.43;
                    InverseFlattening = 300;
                    break;
                case Proj4Ellipsoid.AppPhysics_1965:
                    _equatorialRadius = 6378137.0;
                    InverseFlattening = 298.25;
                    break;
                case Proj4Ellipsoid.Austrailia_SouthAmerica:
                    _equatorialRadius = 6378160.0;
                    InverseFlattening = 298.25;
                    _code = "AN";
                    break;
                case Proj4Ellipsoid.Bessel_1841:
                    _equatorialRadius = 6377397.155;
                    InverseFlattening = 299.1528128;
                    _code = "BR";
                    break;
                case Proj4Ellipsoid.BesselNamibia:
                    _equatorialRadius = 6377483.865;
                    InverseFlattening = 299.1528128;
                    _code = "BN";
                    break;
                case Proj4Ellipsoid.Clarke_1866:
                    _equatorialRadius = 6378206.4;
                    _polarRadius = 6356583.8;
                    _code = "CC";
                    break;
                case Proj4Ellipsoid.ClarkeModified_1880:
                    _equatorialRadius = 6378249.145;
                    InverseFlattening = 293.4663;
                    _code = "CD";
                    break;
                case Proj4Ellipsoid.CPM_1799:
                    _equatorialRadius = 6375738.7;
                    InverseFlattening = 334.29;
                    break;
                case Proj4Ellipsoid.Custom: 
                    // Nothing for Custom
                    break;
                case Proj4Ellipsoid.Delambre_1810:
                    _equatorialRadius = 6376428;
                    InverseFlattening = 311.5;
                    break;
                case Proj4Ellipsoid.Engelis_1985:
                    _equatorialRadius = 6378136.05;
                    InverseFlattening = 298.2566;
                    break;
                case Proj4Ellipsoid.Everest_1830:
                    _equatorialRadius = 6377276.345;
                    InverseFlattening = 300.8017;
                    _code = "EA";
                    break;
                case Proj4Ellipsoid.Everest_1948:
                    _equatorialRadius = 6377304.063;
                    InverseFlattening = 300.8017;
                    _code = "EE";
                    break;
                case Proj4Ellipsoid.Everest_1956:
                    _equatorialRadius = 6377301.243;
                    InverseFlattening = 300.8017;
                    _code = "EC";
                    break;
                case Proj4Ellipsoid.Everest_1969:
                    _equatorialRadius = 6377295.664;
                    InverseFlattening = 300.8017;
                    _code = "ED";
                    break;
                case Proj4Ellipsoid.Everest_Pakistan:
                    _equatorialRadius = 6377309.613;
                    InverseFlattening = 300.8017;
                    _code = "EF";
                    break;
                case Proj4Ellipsoid.Everest_SS:
                    _equatorialRadius = 6377298.556;
                    InverseFlattening = 300.8017;
                    _code = "EB";
                    break;
                case Proj4Ellipsoid.Fischer_1960:
                    _equatorialRadius = 6378166;
                    InverseFlattening = 298.3;
                    break;
                case Proj4Ellipsoid.Fischer_1968:
                    _equatorialRadius = 6378150;
                    InverseFlattening = 298.3;
                    break;
                case Proj4Ellipsoid.FischerModified_1960:
                    _equatorialRadius = 6378155;
                    InverseFlattening = 298.3;
                    _code = "FA";
                    break;
                case Proj4Ellipsoid.GRS_1967:
                    _equatorialRadius = 6378160.0;
                    InverseFlattening = 298.2471674270;
                    break;
                case Proj4Ellipsoid.GRS_1980:
                    _equatorialRadius = 6378137.0;
                    InverseFlattening = 298.257222101;
                    _code = "RF";
                    break;
                case Proj4Ellipsoid.Helmert_1906:
                    _equatorialRadius = 6378200;
                    InverseFlattening = 298.3;
                    _code = "HE";
                    break;
                case Proj4Ellipsoid.Hough:
                    _equatorialRadius = 6378270.0;
                    InverseFlattening = 297;
                    _code = "HO";
                    break;
                case Proj4Ellipsoid.IAU_1976:
                    _equatorialRadius = 6378140.0;
                    InverseFlattening = 298.257;
                    break;
                case Proj4Ellipsoid.Indonesian_1974:
                    _equatorialRadius = 6378160;
                    InverseFlattening = 298.247;
                    _code = "ID";
                    break;
                case Proj4Ellipsoid.International_1909:
                    _equatorialRadius = 6378388.0;
                    InverseFlattening = 297;
                    _code = "IN";
                    break;
                case Proj4Ellipsoid.InternationalNew_1967:
                    _equatorialRadius = 6378157.5;
                    _polarRadius = 6356772.2;
                    break;
                case Proj4Ellipsoid.Krassovsky_1942:
                    _equatorialRadius = 6378245.0;
                    InverseFlattening = 298.3;
                    _code = "KA";
                    break;
                case Proj4Ellipsoid.Kaula_1961:
                    _equatorialRadius = 6378163;
                    InverseFlattening = 298.24;
                    break;
                case Proj4Ellipsoid.Lerch_1979:
                    _equatorialRadius = 6378139;
                    InverseFlattening = 298.257;
                    break;
                case Proj4Ellipsoid.Maupertius_1738:
                    _equatorialRadius = 6397300;
                    InverseFlattening = 191;
                    break;
                case Proj4Ellipsoid.Merit_1983:
                    _equatorialRadius = 6378137.0;
                    InverseFlattening = 298.257;
                    break;
                case Proj4Ellipsoid.NavalWeaponsLab_1965:
                    _equatorialRadius = 6378145.0;
                    InverseFlattening = 298.25;
                    break;
                case Proj4Ellipsoid.Plessis_1817:
                    _equatorialRadius = 6376523;
                    _polarRadius = 6355863;
                    break;
                case Proj4Ellipsoid.SoutheastAsia:
                    _equatorialRadius = 6378155.0;
                    _polarRadius = 6356773.3205;
                    break;
                case Proj4Ellipsoid.SovietGeodeticSystem_1985:
                    _equatorialRadius = 6378136.0;
                    InverseFlattening = 298.257;
                    break;
                case Proj4Ellipsoid.Sphere:
                    _equatorialRadius = 6370997.0;
                    _polarRadius = 6370997.0;
                    break;
                case Proj4Ellipsoid.Walbeck:
                    _equatorialRadius = 6376896.0;
                    _polarRadius = 6355834.8467;
                    break;
                case Proj4Ellipsoid.WGS_1960:
                    _name = "WGS_1960";
                    _equatorialRadius = 6378165.0;
                    InverseFlattening = 298.3;
                    break;
                case Proj4Ellipsoid.WGS_1966:
                    _name = "WGS_1966";
                    _equatorialRadius = 6378145.0;
                    InverseFlattening = 298.25;
                    break;
                case Proj4Ellipsoid.WGS_1972:
                    _name = "WGS_1972";
                    _code = "WD";
                    _equatorialRadius = 6378135.0;
                    InverseFlattening = 298.26;
                    break;
                case Proj4Ellipsoid.WGS_1984:
                    _name = "WGS_1984";
                    _code = "WE";
                    _equatorialRadius = 6378137.0;
                    InverseFlattening = 298.257223563;
                    break;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Each of the enumerated known ellipsoids is encoded by an ellps parameter specified by
        /// the corresponding string value.  Ellipsoids that are not found here or are specified
        /// as "Custom" in the enuemration will be replaced with an 'a' and a 'b' parameter instead.
        /// </summary>
        public Dictionary<Proj4Ellipsoid, string> Proj4Names
        {
            get { return _proj4Names; }
        }

        /// <summary>
        /// Calculates the flattening factor, (a - b) / a.
        /// </summary>
        /// <returns></returns>
        public double FlatteningFactor()
        {
            return (_equatorialRadius - _polarRadius) / _equatorialRadius;
        }

        /// <summary>
        /// Uses the current known ellipsoid to return a code name for the proj4 string if possible.
        /// Otherwise, this returns the radial parameters a and b.
        /// </summary>
        /// <returns></returns>
        public string ToProj4String()
        {
            return _knownEllipsoid == Proj4Ellipsoid.Custom
                ? String.Format(" +a={0} +b={1}", _equatorialRadius, _polarRadius)
                : String.Format(" +ellps={0}", _proj4Names[_knownEllipsoid]);
        }

        /// <summary>
        /// Calculates the eccentrity according to e = sqrt(2f - f^2) where f is the flattening factor.
        /// </summary>
        /// <returns></returns>
        public double Eccentricity()
        {
            double f = FlatteningFactor();
            return Math.Sqrt(2 * f - f * f);
        }

        /// <summary>
        /// Calculates the square of eccentricity according to es = (2f - f^2) where f is the flattening factor.
        /// </summary>
        /// <returns></returns>
        public double EccentricitySquared()
        {
            double f = FlatteningFactor();
            return 2 * f - f * f;
        }

        /// <summary>
        /// Calculates the inverse of the flattening factor, commonly saved to Esri projections,
        /// or else provided as the "rf" parameter for Proj4 strings.  This is simply calculated
        /// as a / (a - b) where a is the semi-major axis and b is the semi-minor axis.
        /// </summary>
        /// <returns></returns>
        public double GetInverseFlattening()
        {
            if (_polarRadius == _equatorialRadius) return 0; // prevent divide by zero for spheres.
            return (_equatorialRadius) / (_equatorialRadius - _polarRadius);
        }

        /// <summary>
        /// Gets a boolean that is true if the spheroid has been flattened.
        /// </summary>
        /// <returns>Boolean, true if the spheroid is oblate (or flattened)</returns>
        public bool IsOblate()
        {
            return (_polarRadius < _equatorialRadius);
        }

        private void AddNames()
        {
            _proj4Names = new Dictionary<Proj4Ellipsoid, string>();
            _proj4Names.Add(Proj4Ellipsoid.Airy_1830, "airy");
            _proj4Names.Add(Proj4Ellipsoid.AiryModified, "mod_airy");
            _proj4Names.Add(Proj4Ellipsoid.Andrae_1876, "andrae");
            _proj4Names.Add(Proj4Ellipsoid.AppPhysics_1965, "APL4.9");
            _proj4Names.Add(Proj4Ellipsoid.Austrailia_SouthAmerica, "aust_SA");
            _proj4Names.Add(Proj4Ellipsoid.Bessel_1841, "bessel");
            _proj4Names.Add(Proj4Ellipsoid.BesselNamibia, "bess_nam");
            _proj4Names.Add(Proj4Ellipsoid.Clarke_1866, "clrk66");
            _proj4Names.Add(Proj4Ellipsoid.ClarkeModified_1880, "clrk80");
            _proj4Names.Add(Proj4Ellipsoid.CPM_1799, "CPM");
            _proj4Names.Add(Proj4Ellipsoid.Custom, string.Empty);
            _proj4Names.Add(Proj4Ellipsoid.Delambre_1810, "delmbr");
            _proj4Names.Add(Proj4Ellipsoid.Engelis_1985, "engelis");
            _proj4Names.Add(Proj4Ellipsoid.Everest_1830, "evrst30");
            _proj4Names.Add(Proj4Ellipsoid.Everest_1948, "evrst48");
            _proj4Names.Add(Proj4Ellipsoid.Everest_1956, "evrst56");
            _proj4Names.Add(Proj4Ellipsoid.Everest_1969, "evrst69");
            _proj4Names.Add(Proj4Ellipsoid.Everest_SS, "evrstSS");
            _proj4Names.Add(Proj4Ellipsoid.Fischer_1960, "fschr60");
            _proj4Names.Add(Proj4Ellipsoid.FischerModified_1960, "fschr60m");
            _proj4Names.Add(Proj4Ellipsoid.Fischer_1968, "fschr68");
            _proj4Names.Add(Proj4Ellipsoid.GRS_1967, "GRS67");
            _proj4Names.Add(Proj4Ellipsoid.GRS_1980, "GRS80");
            _proj4Names.Add(Proj4Ellipsoid.Helmert_1906, "helmert");
            _proj4Names.Add(Proj4Ellipsoid.Hough, "hough");
            _proj4Names.Add(Proj4Ellipsoid.IAU_1976, "IAU76");
            _proj4Names.Add(Proj4Ellipsoid.International_1909, "intl");
            _proj4Names.Add(Proj4Ellipsoid.InternationalNew_1967, "new_intl");
            _proj4Names.Add(Proj4Ellipsoid.Krassovsky_1942, "krass");
            _proj4Names.Add(Proj4Ellipsoid.Lerch_1979, "lerch");
            _proj4Names.Add(Proj4Ellipsoid.Maupertius_1738, "mprts");
            _proj4Names.Add(Proj4Ellipsoid.Merit_1983, "MERIT");
            _proj4Names.Add(Proj4Ellipsoid.NavalWeaponsLab_1965, "NWL9D");
            _proj4Names.Add(Proj4Ellipsoid.Plessis_1817, "plessis");
            _proj4Names.Add(Proj4Ellipsoid.SoutheastAsia, "SEasia");
            _proj4Names.Add(Proj4Ellipsoid.SovietGeodeticSystem_1985, "SGS85");
            _proj4Names.Add(Proj4Ellipsoid.Sphere, "sphere");
            _proj4Names.Add(Proj4Ellipsoid.Walbeck, "walbeck");
            _proj4Names.Add(Proj4Ellipsoid.WGS_1960, "WGS60");
            _proj4Names.Add(Proj4Ellipsoid.WGS_1966, "WGS66");
            _proj4Names.Add(Proj4Ellipsoid.WGS_1972, "WGS72");
            _proj4Names.Add(Proj4Ellipsoid.WGS_1984, "WGS84");
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Sets the value by using the current semi-major axis (Equatorial Radius) in order to
        /// calculate the semi-minor axis (Polar Radius).
        /// </summary>
        public double InverseFlattening
        {
            get
            {
                if (_polarRadius == _equatorialRadius)
                {
                    return 0;
                }
                return _equatorialRadius / (_equatorialRadius - _polarRadius);
            }
            set
            {
                Debug.Assert(EquatorialRadius > 0, "EquatorialRadius must be set.");
                if (value == 0)
                {
                    _polarRadius = _equatorialRadius;
                }
                else
                {
                    _polarRadius = _equatorialRadius - _equatorialRadius / value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the string name of the spheroid.
        /// e.g.: WGS_1984
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// A spheroid is a pole flattened (oblate) sphere, with the radii of two axes being equal and longer
        /// than the third.  This is the radial measure of one of these major axes in meters.
        /// e.g. 6, 378, 137 for WGS 84
        /// </summary>
        public double EquatorialRadius
        {
            get { return _equatorialRadius; }
            set { _equatorialRadius = value; }
        }

        /// <summary>
        /// A spheroid is a pole flattened (oblate) sphere, with the radii of two axes being equal and longer
        /// than the third.  This is the radial measure of the smaller polar axis in meters.  One option is
        /// to specify this directly, but it can also be calculated using the major axis and the flattening factor.
        /// </summary>
        public double PolarRadius
        {
            get { return _polarRadius; }
            set { _polarRadius = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Proj4Ellipsoid KnownEllipsoid
        {
            get { return _knownEllipsoid; }
            set
            {
                AssignKnownEllipsoid(value);
            }
        }

        #endregion Properties

        #region IEsriString Members

        /// <summary>
        /// Converts the spheroid parameters into a valid esri expression that uses the semi-major axis
        /// and the reciprocal flattening factor
        /// </summary>
        /// <returns></returns>
        public string ToEsriString()
        {
            return @"SPHEROID[""" + _name + @"""," + Convert.ToString(_equatorialRadius, CultureInfo.InvariantCulture) + "," + Convert.ToString(GetInverseFlattening(), CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Reads the Esri string to define the spheroid, which controls how flattened the earth's radius is
        /// </summary>
        /// <param name="esriString"></param>
        public void ParseEsriString(string esriString)
        {
            if (esriString.Contains("SPHEROID") == false) return;
            int iStart = esriString.IndexOf("SPHEROID", StringComparison.Ordinal) + 9;
            int iEnd = esriString.IndexOf("]", iStart, StringComparison.Ordinal);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _equatorialRadius = double.Parse(terms[1], CultureInfo.InvariantCulture);
            InverseFlattening = double.Parse(terms[2], CultureInfo.InvariantCulture);

            //added by jirikadlec2 - set the 'knownEllipsoid' to the name if possible
            _knownEllipsoid = Proj4Ellipsoid.Custom;
            foreach (Proj4Ellipsoid proj4ellps in _proj4Names.Keys)
            {
                if (proj4ellps.ToString() == _name)
                {
                    _knownEllipsoid = proj4ellps;
                    break;
                }
            }
        }

        #endregion
    }
}