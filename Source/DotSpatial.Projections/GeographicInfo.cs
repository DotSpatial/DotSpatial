// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:00:36 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections
{
    public class GeographicInfo : ProjDescriptor, IEsriString
    {
        #region Private Variables

        private Datum _datum;
        private Meridian _meridian;
        private string _name;
        private AngularUnit _unit;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeographicInfo
        /// </summary>
        public GeographicInfo()
        {
            _datum = new Datum();
            _meridian = new Meridian();
            _unit = new AngularUnit();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the datum
        /// eg: D_WGS_1984
        /// </summary>
        public Datum Datum
        {
            get { return _datum; }
            set { _datum = value; }
        }

        /// <summary>
        /// Gets or sets the prime meridian longitude of the 0 mark, relative to Greenwitch
        /// </summary>
        public Meridian Meridian
        {
            get { return _meridian; }
            set { _meridian = value; }
        }

        /// <summary>
        /// Gets or sets the geographic coordinate system name
        /// eg: GCS_WGS_1984
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the units
        /// </summary>
        public AngularUnit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        #endregion

        #region IEsriString Members

        /// <summary>
        /// Generates an esri string from the information in this geographic info class, including the name, datum, meridian, and unit.
        /// </summary>
        /// <returns></returns>
        public string ToEsriString()
        {
            return @"GEOGCS[""" + _name + @"""," + Datum.ToEsriString() + "," + Meridian.ToEsriString() + "," + Unit.ToEsriString() + "]";
        }

        /// <summary>
        /// Reads an esri string in order to parse the datum, meridian and unit as well as the name.
        /// </summary>
        /// <param name="esriString">The string to parse</param>
        public void ParseEsriString(string esriString)
        {
            if (String.IsNullOrEmpty(esriString))
                return;

            if (esriString.Contains("GEOGCS") == false) return;
            int iStart = esriString.IndexOf("GEOGCS", StringComparison.Ordinal) + 8;
            int iEnd = esriString.IndexOf(@""",", iStart, StringComparison.Ordinal) - 1;
            if (iEnd >= iStart)
                _name = esriString.Substring(iStart, iEnd - iStart + 1);
            _datum.ParseEsriString(esriString);
            _meridian.ParseEsriString(esriString);
            _unit.ParseEsriString(esriString);
        }

        #endregion

        /// <summary>
        /// Returns a representaion of this object as a Proj4 string.
        /// </summary>
        /// <returns></returns>
        public string ToProj4String()
        {
            return Meridian.ToProj4String() + Datum.ToProj4String();
        }
    }
}