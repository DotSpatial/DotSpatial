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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 3:27:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Datum
    /// </summary>
    public class Datum : ProjDescriptor, IEsriString
    {
        private const double SEC_TO_RAD = 4.84813681109535993589914102357e-6;

        #region Private Variables

        private DatumType _datumtype;
        private string _description;
        private string[] _nadGrids;
        private string _name;
        private Spheroid _spheroid;
        private double[] _toWgs84;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a new instance of Datum
        /// </summary>
        public Datum()
        {
            _spheroid = new Spheroid();
            DatumType = DatumType.Unknown;
        }

        /// <summary>
        /// uses a string name of a standard datum to create a new instance of the Datum class
        /// </summary>
        /// <param name="standardDatum">The string name of the datum to use</param>
        public Datum(string standardDatum)
            : this()
        {
            Proj4DatumName = standardDatum;
        }

        /// <summary>
        /// Uses a Proj4Datums enumeration in order to specify a known datum to
        /// define the spheroid and to WGS calculation method and parameters
        /// </summary>
        /// <param name="standardDatum">The Proj4Datums enumeration specifying the known datum</param>
        public Datum(Proj4Datum standardDatum)
            : this(standardDatum.ToString())
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Returns a representaion of this object as a Proj4 string.
        /// </summary>
        /// <returns></returns>
        public string ToProj4String()
        {
            // if you have a datum name you don't need to say anything about the Spheroid
            string str = Proj4DatumName;
            if (str == null)
            {
                switch (DatumType)
                {
                    case DatumType.Unknown:
                    case DatumType.WGS84:
                        break;

                    case DatumType.Param3:
                        Debug.Assert(_toWgs84.Length >= 3);
                        str = String.Format(CultureInfo.InvariantCulture, " +towgs84={0},{1},{2}", _toWgs84[0], _toWgs84[1], _toWgs84[2]);
                        break;

                    case DatumType.Param7:
                        Debug.Assert(_toWgs84.Length >= 7);
                        str = String.Format(CultureInfo.InvariantCulture, " +towgs84={0},{1},{2},{3},{4},{5},{6}",
                                            _toWgs84[0],
                                            _toWgs84[1],
                                            _toWgs84[2],
                                            _toWgs84[3] / SEC_TO_RAD,
                                            _toWgs84[4] / SEC_TO_RAD,
                                            _toWgs84[5] / SEC_TO_RAD,
                                            (_toWgs84[6] - 1) * 1000000.0);
                        break;

                    case DatumType.GridShift:
                        str = String.Format(" +nadgrids={0}", String.Join(",", NadGrids));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("DatumType");
                }
                return str + Spheroid.ToProj4String();
            }
            else
            {
                return String.Format(" +datum={0}", Proj4DatumName);
            }
        }

        /// <summary>
        /// Compares two datums to see if they are actually describing the same thing and
        /// therefore don't need to be transformed.
        /// </summary>
        /// <param name="otherDatum">The other datum to compare against</param>
        /// <returns>The boolean result of the operation.</returns>
        public bool Matches(Datum otherDatum)
        {
            if (_datumtype != otherDatum.DatumType) return false;
            if (_datumtype == DatumType.WGS84) return true;
            if (_spheroid.EquatorialRadius != otherDatum.Spheroid.EquatorialRadius) return false;
            if (_spheroid.PolarRadius != otherDatum.Spheroid.PolarRadius) return false;
            if (_datumtype == DatumType.Param3)
            {
                if (_toWgs84[0] != otherDatum.ToWGS84[0]) return false;
                if (_toWgs84[1] != otherDatum.ToWGS84[1]) return false;
                if (_toWgs84[2] != otherDatum.ToWGS84[2]) return false;
                return true;
            }
            if (_datumtype == DatumType.Param7)
            {
                if (_toWgs84[0] != otherDatum.ToWGS84[0]) return false;
                if (_toWgs84[1] != otherDatum.ToWGS84[1]) return false;
                if (_toWgs84[2] != otherDatum.ToWGS84[2]) return false;
                if (_toWgs84[3] != otherDatum.ToWGS84[3]) return false;
                if (_toWgs84[4] != otherDatum.ToWGS84[4]) return false;
                if (_toWgs84[5] != otherDatum.ToWGS84[5]) return false;
                if (_toWgs84[6] != otherDatum.ToWGS84[6]) return false;
                return true;
            }
            if (_datumtype == DatumType.GridShift)
            {
                if (_nadGrids.Length != otherDatum.NadGrids.Length) return false;
                return !_nadGrids.Where((t, i) => t != otherDatum.NadGrids[i]).Any();
            }
            return false;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the name of the datum defining the spherical characteristics of the model of the earth
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The spheroid of the earth, defining the maximal radius and the flattening factor
        /// </summary>
        public Spheroid Spheroid
        {
            get { return _spheroid; }
            set { _spheroid = value; }
        }

        /// <summary>
        /// Gets or sets the set of double parameters, (this can either be 3 or 7 parameters)
        /// used to transform this
        /// </summary>
        public double[] ToWGS84
        {
            get { return _toWgs84; }
            set { _toWgs84 = value; }
        }

        /// <summary>
        /// Gets or sets the datum type, which clarifies how to perform transforms to WGS84
        /// </summary>
        public DatumType DatumType
        {
            get { return _datumtype; }
            set { _datumtype = value; }
        }

        /// <summary>
        /// Gets or sets an english description for this datum
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the array of string nadGrid
        /// </summary>
        public string[] NadGrids
        {
            get { return _nadGrids; }
            set { _nadGrids = value; }
        }

        #endregion Properties

        /// <summary>
        /// Lookups the proj4 datum based on the stored name.
        /// </summary>
        public string Proj4DatumName
        {
            get
            {
                if (Name == null)
                    return null;

                switch (Name)
                {
                    case "D_WGS_1984":
                        return "WGS84";

                    case "D_Greek":
                        return "GGRS87";

                    case "D_North_American_1983":
                        return "NAD83";

                    case "D_North_American_1927":
                        return "NAD27";

                    default:
                        // not sure where to lookup the remaining, missing values.
                        // also we could include this information in the datum.xml file where much of this information resides.
                        return null;
                }
            }
            set
            {
                string id = value.ToLower();
                switch (id)
                {
                    case "wgs84":
                        _toWgs84 = new double[] { 0, 0, 0 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.WGS_1984);
                        _description = "WGS 1984";
                        _name = "D_WGS_1984";
                        _datumtype = DatumType.WGS84;
                        break;
                    case "ggrs87":
                        _toWgs84 = new[] { -199.87, 74.79, 246.62 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.GRS_1980);
                        _description = "Greek Geodetic Reference System 1987";
                        _datumtype = DatumType.Param3;
                        _name = "D_Greek";
                        break;
                    case "nad83":
                        _toWgs84 = new double[] { 0, 0, 0 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.GRS_1980);
                        _description = "North American Datum 1983";
                        _datumtype = DatumType.WGS84;
                        _name = "D_North_American_1983";
                        break;
                    case "nad27":
                        _nadGrids = new[] { "@conus", "@ntv1_can", "@ntv2_0", "@alaska" };
                        _spheroid = new Spheroid(Proj4Ellipsoid.Clarke_1866);
                        _description = "North American Datum 1927";
                        _name = "D_North_American_1927";
                        _datumtype = DatumType.GridShift;
                        break;
                    case "potsdam":
                        _toWgs84 = new[] { 606.0, 23.0, 413.0 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.Bessel_1841);
                        _description = "Potsdam Rauenberg 1950 DHDN";
                        _datumtype = DatumType.Param3;
                        break;
                    case "carthage":
                        _toWgs84 = new[] { -263.0, 6, 413 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.ClarkeModified_1880);
                        _description = "Carthage 1934 Tunisia";
                        _datumtype = DatumType.Param3;
                        break;
                    case "hermannskogel":
                        _toWgs84 = new[] { 653.0, -212.0, 449 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.Bessel_1841);
                        _description = "Hermannskogel";
                        _datumtype = DatumType.Param3;
                        break;
                    case "ire65":
                        _toWgs84 = new[] { 482.530, -130.569, 564.557, -1.042, -.214, -.631, 8.15 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.AiryModified);
                        _description = "Ireland 1965";
                        _datumtype = DatumType.Param7;
                        break;
                    case "nzgd49":
                        _toWgs84 = new[] { 59.47, -5.04, 187.44, 0.47, -0.1, 1.024, -4.5993 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.International_1909);
                        _description = "New Zealand";
                        _datumtype = DatumType.Param7;
                        break;
                    case "osgb36":
                        _toWgs84 = new[] { 446.448, -125.157, 542.060, 0.1502, 0.2470, 0.8421, -20.4894 };
                        _spheroid = new Spheroid(Proj4Ellipsoid.Airy_1830);
                        _description = "Airy 1830";
                        _datumtype = DatumType.Param7;
                        break;
                }
            }
        }

        #region IEsriString Members

        /// <summary>
        /// Creates an esri well known text string for the datum part of the string
        /// </summary>
        /// <returns>The datum portion of the esri well known text</returns>
        public string ToEsriString()
        {
            return @"DATUM[""" + _name + @"""," + Spheroid.ToEsriString() + "]";
        }

        /// <summary>
        /// parses the datum from the esri string
        /// </summary>
        /// <param name="esriString">The string to parse values from</param>
        public void ParseEsriString(string esriString)
        {
            if (System.String.IsNullOrEmpty(esriString))
                return;

            if (esriString.Contains("DATUM") == false) return;
            int iStart = esriString.IndexOf("DATUM") + 7;
            int iEnd = esriString.IndexOf(@""",", iStart) - 1;
            if (iEnd < iStart) return;
            _name = esriString.Substring(iStart, iEnd - iStart + 1);

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string fileName = null;
            string currentAssemblyLocation = currentAssembly.Location;
            if (!String.IsNullOrEmpty(currentAssemblyLocation))
            {
                fileName = Path.GetDirectoryName(currentAssemblyLocation) + "\\datums.xml";
            }
            Stream datumStream = File.Exists(fileName) ? File.Open(fileName, FileMode.Open) : currentAssembly.GetManifestResourceStream("DotSpatial.Projections.XML.datums.xml");

            if (datumStream != null)
            {
                XmlTextReader reader = new XmlTextReader(datumStream);

                while (reader.Read())
                {
                    if (reader.AttributeCount == 0) continue;
                    reader.MoveToAttribute("Name");
                    if (reader.Value != _name) continue;
                    reader.MoveToAttribute("Type");
                    if (string.IsNullOrEmpty(reader.Value)) break;
                    DatumType = (DatumType)Enum.Parse(typeof(DatumType), reader.Value);
                    switch (DatumType)
                    {
                        case DatumType.Param3:
                            {
                                double[] transform = new double[3];
                                for (int i = 0; i < 3; i++)
                                {
                                    reader.MoveToAttribute("P" + (i + 1));
                                    if (!string.IsNullOrEmpty(reader.Value))
                                    {
                                        transform[i] = double.Parse(reader.Value, CultureInfo.InvariantCulture);
                                    }
                                }
                                ToWGS84 = transform;
                            }
                            break;
                        case DatumType.Param7:
                            {
                                double[] transform = new double[7];
                                for (int i = 0; i < 7; i++)
                                {
                                    reader.MoveToAttribute("P" + (i + 1));
                                    if (!string.IsNullOrEmpty(reader.Value))
                                    {
                                        transform[i] = double.Parse(reader.Value, CultureInfo.InvariantCulture);
                                    }
                                }
                                ToWGS84 = transform;
                                break;
                            }
                        case DatumType.GridShift:
                            reader.MoveToAttribute("Shift");
                            if (string.IsNullOrEmpty(reader.Value)) continue;
                            NadGrids = reader.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            break;
                    }
                    break;
                }
            }

            _spheroid.ParseEsriString(esriString);
        }

        #endregion

        /// <summary>
        /// Initializes to WGS84.
        /// </summary>
        /// <param name="values">The values.</param>
        public void InitializeToWgs84(string[] values)
        {
            _toWgs84 = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                _toWgs84[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
            }

            if (_toWgs84.Length != 3 && _toWgs84.Length != 7)
                throw new ArgumentOutOfRangeException("Unrecognized ToWgs84 array length. The number of elements in the array should be 3 or 7");

            if (_toWgs84.Length == 3)
            {
                _datumtype = DatumType.Param3;
            }
            else
            {
                // checking to see if several blank values were included.
                if (_toWgs84[3] == 0.0 && _toWgs84[4] == 0.0 && _toWgs84[5] == 0.0 && _toWgs84[6] == 0.0)
                {
                    _datumtype = DatumType.Param3;
                }
                else
                {
                    _datumtype = DatumType.Param7;
                    // Transform from arc seconds to radians
                    _toWgs84[3] *= SEC_TO_RAD;
                    _toWgs84[4] *= SEC_TO_RAD;
                    _toWgs84[5] *= SEC_TO_RAD;
                    // transform from parts per millon to scaling factor
                    _toWgs84[6] = (_toWgs84[6] / 1000000.0) + 1;
                }
            }
        }
    }
}