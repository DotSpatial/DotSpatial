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
    public class AngularUnit : ProjDescriptor, IEsriString
    {
        #region Private Variables

        private string _name;
        private double _radians;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Unit
        /// </summary>
        public AngularUnit()
        {
            _radians = 0.017453292519943295; // assume degrees
            _name = "Degree";
        }

        #endregion

        #region Methods

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
        /// Gets or sets the constant to multiply against this unit to get radians.
        /// </summary>
        public double Radians
        {
            get { return _radians; }
            set { _radians = value; }
        }

        #endregion

        #region IEsriString Members

        /// <summary>
        /// Generates the Esri string from the values in this class
        /// </summary>
        /// <returns>The resulting esri string</returns>
        public string ToEsriString()
        {
            return @"UNIT[""" + _name + @"""," + Convert.ToString(_radians, CultureInfo.InvariantCulture) + "]";
        }

        /// <summary>
        /// Reads an esri string to determine the angular unit
        /// </summary>
        /// <param name="esriString">The esri string to read</param>
        public void ParseEsriString(string esriString)
        {
            if (System.String.IsNullOrEmpty(esriString))
                return;

            if (esriString.Contains("UNIT") == false) return;
            int iStart = esriString.IndexOf("UNIT") + 5;
            int iEnd = esriString.IndexOf("]", iStart);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _radians = double.Parse(terms[1], CultureInfo.InvariantCulture);
        }

        #endregion
    }
}