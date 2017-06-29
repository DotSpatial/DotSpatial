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
            int iStart = esriString.IndexOf("UNIT") + 5;
            int iEnd = esriString.IndexOf("]", iStart);
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
            switch (uomCode)
            {
                case 9001: _name = "Metre"; _meters = 1; break;
                case 9002: _name = "Foot"; _meters = 0.3048; break;
                case 9003: _name = "Us Survey Foot"; _meters = 0.304800609601219; break;
                case 9005: _name = "Clarke'S Foot"; _meters = 0.3047972654; break;
                case 9014: _name = "Fathom"; _meters = 1.8288; break;
                case 9030: _name = "Nautical Mile"; _meters = 1852; break;
                case 9031: _name = "German Legal Metre"; _meters = 1.0000135965; break;
                case 9033: _name = "Us Survey Chain"; _meters = 20.1168402336805; break;
                case 9034: _name = "Us Survey Link"; _meters = 0.201168402336805; break;
                case 9035: _name = "Us Survey Mile"; _meters = 1609.34721869444; break;
                case 9036: _name = "Kilometre"; _meters = 1000; break;
                case 9037: _name = "Clarke'S Yard"; _meters = 0.9143917962; break;
                case 9038: _name = "Clarke'S Chain"; _meters = 20.1166195164; break;
                case 9039: _name = "Clarke'S Link"; _meters = 0.201166195164; break;
                case 9040: _name = "British Yard (Sears 1922)"; _meters = 0.914398414616029; break;
                case 9041: _name = "British Foot (Sears 1922)"; _meters = 0.304799471538676; break;
                case 9042: _name = "British Chain (Sears 1922)"; _meters = 20.1167651215526; break;
                case 9043: _name = "British Link (Sears 1922)"; _meters = 0.201167651215526; break;
                case 9050: _name = "British Yard (Benoit 1895 A)"; _meters = 0.9143992; break;
                case 9051: _name = "British Foot (Benoit 1895 A)"; _meters = 0.304799733333333; break;
                case 9052: _name = "British Chain (Benoit 1895 A)"; _meters = 20.1167824; break;
                case 9053: _name = "British Link (Benoit 1895 A)"; _meters = 0.201167824; break;
                case 9060: _name = "British Yard (Benoit 1895 B)"; _meters = 0.914399204289812; break;
                case 9061: _name = "British Foot (Benoit 1895 B)"; _meters = 0.304799734763271; break;
                case 9062: _name = "British Chain (Benoit 1895 B)"; _meters = 20.1167824943759; break;
                case 9063: _name = "British Link (Benoit 1895 B)"; _meters = 0.201167824943759; break;
                case 9070: _name = "British Foot (1865)"; _meters = 0.304800833333333; break;
                case 9080: _name = "Indian Foot"; _meters = 0.304799510248147; break;
                case 9081: _name = "Indian Foot (1937)"; _meters = 0.30479841; break;
                case 9082: _name = "Indian Foot (1962)"; _meters = 0.3047996; break;
                case 9083: _name = "Indian Foot (1975)"; _meters = 0.3047995; break;
                case 9084: _name = "Indian Yard"; _meters = 0.914398530744441; break;
                case 9085: _name = "Indian Yard (1937)"; _meters = 0.91439523; break;
                case 9086: _name = "Indian Yard (1962)"; _meters = 0.9143988; break;
                case 9087: _name = "Indian Yard (1975)"; _meters = 0.9143985; break;
                case 9093: _name = "Statute Mile"; _meters = 1609.344; break;
                case 9094: _name = "Gold Coast Foot"; _meters = 0.304799710181509; break;
                case 9095: _name = "British Foot (1936)"; _meters = 0.3048007491; break;
                case 9096: _name = "Yard"; _meters = 0.9144; break;
                case 9097: _name = "Chain"; _meters = 20.1168; break;
                case 9098: _name = "Link"; _meters = 0.201168; break;
                case 9099: _name = "British Yard (Sears 1922 Truncated)"; _meters = 0.914398; break;
                case 9204: _name = "Bin Width 330 Us Survey Foot"; _meters = 100.584201168402; break;
                case 9205: _name = "Bin Width 165 Us Survey Foot"; _meters = 50.2921005842012; break;
                case 9206: _name = "Bin Width 82.5 Us Survey Foot"; _meters = 25.1460502921006; break;
                case 9207: _name = "Bin Width 37.5 Metre"; _meters = 37.5; break;
                case 9208: _name = "Bin Width 25 Metre"; _meters = 25; break;
                case 9209: _name = "Bin Width 12.5 Metre"; _meters = 12.5; break;
                case 9210: _name = "Bin Width 6.25 Metre"; _meters = 6.25; break;
                case 9211: _name = "Bin Width 3.125 Metre"; _meters = 3.125; break;
                case 9300: _name = "British Foot (Sears 1922 Truncated)"; _meters = 0.304799333333333; break;
                case 9301: _name = "British Chain (Sears 1922 Truncated)"; _meters = 20.116756; break;
                case 9302: _name = "British Link (Sears 1922 Truncated)"; _meters = 0.20116756; break;
            }
        }
    }
}