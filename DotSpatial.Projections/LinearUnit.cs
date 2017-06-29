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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 2:54:46 PM
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
    /// Unit
    /// </summary>
    public class LinearUnit : ProjDescriptor, IEsriString
    {
        #region Private Variables

        private double _meters;
        private string _name;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Unit
        /// </summary>
        public LinearUnit()
        {
            _meters = 1;
            _name = "Meter";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of this
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the constant to multiply with maps distances to get the distances in meters
        /// </summary>
        public double Meters
        {
            get { return _meters; }
            set { _meters = value; }
        }

        #endregion

        #region IEsriString Members

        /// <summary>
        /// Generates the part of the Esri well known text for this linear unit
        /// </summary>
        /// <returns>A string that contains the name and conversion factor to meters </returns>
        public string ToEsriString()
        {
            return @"UNIT[""" + _name + @"""," + Convert.ToString(_meters, CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Parses the UNIT member of Esri well known text into a linear unit
        /// </summary>
        /// <param name="esriString"></param>
        public void ParseEsriString(string esriString)
        {
            if (esriString.Contains("UNIT") == false) return;
            int iStart = esriString.IndexOf("UNIT", StringComparison.Ordinal) + 5;
            int iEnd = esriString.IndexOf("]", iStart, StringComparison.Ordinal);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _meters = double.Parse(terms[1], CultureInfo.InvariantCulture);
        }

        #endregion

        /// <summary>
        /// Interprets a UOM code.  For instance 9001 = meters.
        /// These codes and ratios were from GDAL unit_of_measure.csv, and I'm not
        /// sure if the numeric code is used outside of GDAL internal references.
        /// </summary>
        /// <param name="uomCode"></param>
        public void ReadCode(int uomCode)
        {
            var setUom = (Action<string, double>)
                ((name, meters) =>
                {
                    _name = name;
                    _meters = meters;
                });

            switch (uomCode)
            {
                case 9001: setUom("Metre", 1); break;
                case 9002: setUom("Foot", 0.3048); break;
                case 9003: setUom("Us Survey Foot", 0.304800609601219); break;
                case 9005: setUom("Clarke'S Foot", 0.3047972654); break;
                case 9014: setUom("Fathom", 1.8288); break;
                case 9030: setUom("Nautical Mile", 1852); break;
                case 9031: setUom("German Legal Metre", 1.0000135965); break;
                case 9033: setUom("Us Survey Chain", 20.1168402336805); break;
                case 9034: setUom("Us Survey Link", 0.201168402336805); break;
                case 9035: setUom("Us Survey Mile", 1609.34721869444); break;
                case 9036: setUom("Kilometre", 1000); break;
                case 9037: setUom("Clarke'S Yard", 0.9143917962); break;
                case 9038: setUom("Clarke'S Chain", 20.1166195164); break;
                case 9039: setUom("Clarke'S Link", 0.201166195164); break;
                case 9040: setUom("British Yard (Sears 1922)", 0.914398414616029); break;
                case 9041: setUom("British Foot (Sears 1922)", 0.304799471538676); break;
                case 9042: setUom("British Chain (Sears 1922)", 20.1167651215526); break;
                case 9043: setUom("British Link (Sears 1922)", 0.201167651215526); break;
                case 9050: setUom("British Yard (Benoit 1895 A)", 0.9143992); break;
                case 9051: setUom("British Foot (Benoit 1895 A)", 0.304799733333333); break;
                case 9052: setUom("British Chain (Benoit 1895 A)", 20.1167824); break;
                case 9053: setUom("British Link (Benoit 1895 A)", 0.201167824); break;
                case 9060: setUom("British Yard (Benoit 1895 B)", 0.914399204289812); break;
                case 9061: setUom("British Foot (Benoit 1895 B)", 0.304799734763271); break;
                case 9062: setUom("British Chain (Benoit 1895 B)", 20.1167824943759); break;
                case 9063: setUom("British Link (Benoit 1895 B)", 0.201167824943759); break;
                case 9070: setUom("British Foot (1865)", 0.304800833333333); break;
                case 9080: setUom("Indian Foot", 0.304799510248147); break;
                case 9081: setUom("Indian Foot (1937)", 0.30479841); break;
                case 9082: setUom("Indian Foot (1962)", 0.3047996); break;
                case 9083: setUom("Indian Foot (1975)", 0.3047995); break;
                case 9084: setUom("Indian Yard", 0.914398530744441); break;
                case 9085: setUom("Indian Yard (1937)", 0.91439523); break;
                case 9086: setUom("Indian Yard (1962)", 0.9143988); break;
                case 9087: setUom("Indian Yard (1975)", 0.9143985); break;
                case 9093: setUom("Statute Mile", 1609.344); break;
                case 9094: setUom("Gold Coast Foot", 0.304799710181509); break;
                case 9095: setUom("British Foot (1936)", 0.3048007491); break;
                case 9096: setUom("Yard", 0.9144); break;
                case 9097: setUom("Chain", 20.1168); break;
                case 9098: setUom("Link", 0.201168); break;
                case 9099: setUom("British Yard (Sears 1922 Truncated)", 0.914398); break;
                case 9204: setUom("Bin Width 330 Us Survey Foot", 100.584201168402); break;
                case 9205: setUom("Bin Width 165 Us Survey Foot", 50.2921005842012); break;
                case 9206: setUom("Bin Width 82.5 Us Survey Foot", 25.1460502921006); break;
                case 9207: setUom("Bin Width 37.5 Metre", 37.5); break;
                case 9208: setUom("Bin Width 25 Metre", 25); break;
                case 9209: setUom("Bin Width 12.5 Metre", 12.5); break;
                case 9210: setUom("Bin Width 6.25 Metre", 6.25); break;
                case 9211: setUom("Bin Width 3.125 Metre", 3.125); break;
                case 9300: setUom("British Foot (Sears 1922 Truncated)", 0.304799333333333); break;
                case 9301: setUom("British Chain (Sears 1922 Truncated)", 20.116756); break;
                case 9302: setUom("British Link (Sears 1922 Truncated)", 0.20116756); break;
            }
        }
    }
}