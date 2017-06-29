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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 12:39:34 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Globalization;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Meridian
    /// </summary>
    public class Meridian : ProjDescriptor, IEsriString
    {
        #region Private Variables

        private int _code;
        private double _longitude;
        private string _name;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a new instance of Meridian
        /// </summary>
        public Meridian()
            : this(Proj4Meridian.Greenwich) // by default.
        {
            
        }

        /// <summary>
        /// Generates a custom meridian given a name and a longitude
        /// </summary>
        /// <param name="longitude">The longitude to use</param>
        /// <param name="name">The string name for this meridian</param>
        public Meridian(double longitude, string name)
        {
            _longitude = longitude;
            _name = name;
        }

        /// <summary>
        /// Creates a new meridian from one of the known, proj4 meridian locations.
        /// Presumably the longitudes here correspond to various standard meridians
        /// rather than some arbitrary longitudes of capital cities.
        /// </summary>
        /// <param name="standardMeridian">One of the enumerations listed</param>
        public Meridian(Proj4Meridian standardMeridian)
        {
            AssignMeridian(standardMeridian);
        }

        /// <summary>
        /// Creates a new meridian from one of the known, proj4 meridian locations.
        /// </summary>
        /// <param name="standardMeridianName">The string name of the meridian to use</param>
        public Meridian(string standardMeridianName)
        {
            Proj4Meridian meridian;
            if (Enum.TryParse(standardMeridianName, true, out meridian))
            {
                AssignMeridian(meridian);
            }
        }

        /// <summary>
        /// Gets or sets the code.  Setting this will not impact the longitude or name.
        /// In order to read a standard code (8901-8913) use the ReadCode method.
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// Reads the integer code (possibly only an internal GDAL code) for the Meridian.
        /// The value can be from 8901 (Greenwich) to 8913 (Oslo).  This will also alter
        /// the name and longitude for this meridian.
        /// </summary>
        /// <param name="code">The integer meridian code.</param>
        public void ReadCode(int code)
        {
            switch (code)
            {
                case 8901: AssignMeridian(Proj4Meridian.Greenwich); break;
                case 8902: AssignMeridian(Proj4Meridian.Lisbon); break;
                case 8903: AssignMeridian(Proj4Meridian.Paris); break;
                case 8904: AssignMeridian(Proj4Meridian.Bogota); break;
                case 8905: AssignMeridian(Proj4Meridian.Madrid); break;
                case 8906: AssignMeridian(Proj4Meridian.Rome); break;
                case 8907: AssignMeridian(Proj4Meridian.Bern); break;
                case 8908: AssignMeridian(Proj4Meridian.Jakarta); break;
                case 8909: AssignMeridian(Proj4Meridian.Ferro); break;
                case 8910: AssignMeridian(Proj4Meridian.Brussels); break;
                case 8911: AssignMeridian(Proj4Meridian.Stockholm); break;
                case 8912: AssignMeridian(Proj4Meridian.Athens); break;
                case 8913: AssignMeridian(Proj4Meridian.Oslo); break;
            }
        }

        /// <summary>
        /// Changes the longitude to correspond with the specified standard meridian
        /// </summary>
        /// <param name="standardMeridian"></param>
        public void AssignMeridian(Proj4Meridian standardMeridian)
        {
            _name = standardMeridian.ToString();
            switch (standardMeridian)
            {
                case Proj4Meridian.Greenwich:
                    _longitude = 0;
                    _code = 8901;
                    break;
                case Proj4Meridian.Lisbon:
                    _longitude = -9.131906111;
                    _code = 8902;
                    break;
                case Proj4Meridian.Paris:
                    _longitude = 2.337229167;
                    _code = 8903;
                    break;
                case Proj4Meridian.Bogota:
                    _longitude = -74.08091667;
                    _code = 8904;
                    break;
                case Proj4Meridian.Madrid:
                    _longitude = -3.687938889;
                    _code = 8905;
                    break;
                case Proj4Meridian.Rome:
                    _longitude = 12.45233333;
                    _code = 8906;
                    break;
                case Proj4Meridian.Bern:
                    _longitude = 7.439583333;
                    _code = 8907;
                    break;
                case Proj4Meridian.Jakarta:
                    _longitude = 106.8077194;
                    _code = 8908;
                    break;
                case Proj4Meridian.Ferro:
                    _longitude = -17.66666667;
                    _code = 8909;
                    break;
                case Proj4Meridian.Brussels:
                    _longitude = 4.367975;
                    _code = 8910;
                    break;
                case Proj4Meridian.Stockholm:
                    _longitude = 18.05827778;
                    _code = 8911;
                    break;
                case Proj4Meridian.Athens:
                    _longitude = 23.7163375;
                    _code = 8912;
                    break;
                case Proj4Meridian.Oslo:
                    _longitude = 10.72291667;
                    _code = 8913;
                    break;
            }
        }

        private void FindNameByValue(double pmv)
        {
            const double precison = .0000001;
            if (Math.Abs(pmv) < precison)
            {
                AssignMeridian(Proj4Meridian.Greenwich);
            }
            else if (Math.Abs(pmv - -9.131906111) < precison)
            {
                AssignMeridian(Proj4Meridian.Lisbon);
            }
            else if (Math.Abs(pmv - 2.337229167) < precison)
            {
                AssignMeridian(Proj4Meridian.Paris);
            }
            else if (Math.Abs(pmv - -74.08091667) < precison)
            {
                AssignMeridian(Proj4Meridian.Bogota);
            }
            else if (Math.Abs(pmv - -3.687938889) < precison)
            {
                AssignMeridian(Proj4Meridian.Madrid);
            }
            else if (Math.Abs(pmv - 12.45233333) < precison)
            {
                AssignMeridian(Proj4Meridian.Rome);
            }
            else if (Math.Abs(pmv - 7.439583333) < precison)
            {
                AssignMeridian(Proj4Meridian.Bern);
            }
            else if (Math.Abs(pmv - 106.8077194) < precison)
            {
                AssignMeridian(Proj4Meridian.Jakarta);
            }
            else if (Math.Abs(pmv - -17.66666667) < precison)
            {
                AssignMeridian(Proj4Meridian.Ferro);
            }
            else if (Math.Abs(pmv - 4.367975) < precison)
            {
                AssignMeridian(Proj4Meridian.Brussels);
            }
            else if (Math.Abs(pmv - 18.05827778) < precison)
            {
                AssignMeridian(Proj4Meridian.Stockholm);
            }
            else if (Math.Abs(pmv - 23.7163375) < precison)
            {
                AssignMeridian(Proj4Meridian.Athens);
            }
            else if (Math.Abs(pmv - 10.72291667) < precison)
            {
                AssignMeridian(Proj4Meridian.Oslo);
            }
            else
            {
                _name = "Custom";
                _code = 0;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the longitude where the prime meridian for this geographic coordinate occurs.
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        /// <summary>
        /// Gets or sets the pm.
        /// </summary>
        /// <value>
        /// Alternate prime meridian (typically a city name).
        /// </value>
        public string pm
        {
            get
            {
                if (_longitude != 0)
                    return _longitude.ToString(CultureInfo.InvariantCulture);
                return null;
            }
            set
            {
                // maybe we have a numeric value
                double lon;
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out lon))
                {
                    _longitude = lon;
                    // Try to find a standard name that has a close longitude.
                    FindNameByValue(lon);
                }
                // otherwise try parse as city name
                else
                {
                    Proj4Meridian meridian;
                    if (Enum.TryParse(value, true, out meridian))
                    {
                        AssignMeridian(meridian);
                    }    
                }
            }
        }

        #endregion Properties

        #region IEsriString Members

        /// <summary>
        /// Writes the longitude and prime meridian content to the esri string
        /// </summary>
        /// <returns>A string that is formatted for an esri prj file</returns>
        public string ToEsriString()
        {
            return @"PRIMEM[""" + _name + @"""," + Convert.ToString(_longitude, CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Reads content from an esri string, learning information about the prime meridian
        /// </summary>
        /// <param name="esriString"></param>
        public void ParseEsriString(string esriString)
        {
            if (String.IsNullOrEmpty(esriString))
                return;

            if (esriString.Contains("PRIMEM") == false) return;
            int iStart = esriString.IndexOf("PRIMEM", StringComparison.Ordinal) + 7;
            int iEnd = esriString.IndexOf("]", iStart, StringComparison.Ordinal);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _longitude = double.Parse(terms[1], CultureInfo.InvariantCulture);
        }

        #endregion

        /// <summary>
        /// Returns a representaion of this object as a Proj4 string.
        /// </summary>
        /// <returns></returns>
        public string ToProj4String()
        {
            if (String.IsNullOrWhiteSpace(pm))
                return null;
            return String.Format(" +pm={0}", pm);
        }
    }
}