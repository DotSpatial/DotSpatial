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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 4:47:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// Jiri Kadlec         | 11/20/2010 |  Updated the Esri string of Web Mercator Auxiliary sphere projection
// Matthew K           | 01/10/2010 |  Removed Parameters dictionary
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using DotSpatial.Projections.AuthorityCodes;
using DotSpatial.Projections.Transforms;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Parameters based on http://trac.osgeo.org/proj/wiki/GenParms. Also, see http://home.comcast.net/~gevenden56/proj/manual.pdf
    /// </summary>
    public class ProjectionInfo : ProjDescriptor, IEsriString
    {
        #region Constants and Fields

        private double? _longitudeOf1st;

        private double? _longitudeOf2nd;

        private double? _scaleFactor;

        private string LongitudeOfCenterAlias;

        // stores the value of the actual parameter name that was used in the original (when the string came from WKT/Esri)
        private string LatitudeOfOriginAlias;

        private string FalseEastingAlias;

        private string FalseNorthingAlias;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProjectionInfo" /> class.
        /// </summary>
        public ProjectionInfo()
        {
            GeographicInfo = new GeographicInfo();
            Unit = new LinearUnit();
            _scaleFactor = 1; // if not specified, default to 1
            AuxiliarySphereType = AuxiliarySphereType.NotSpecified;
            NoDefs = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the athority, for example EPSG
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        ///   Gets or sets the athority code
        /// </summary>
        public int AuthorityCode { get; set; }

        /// <summary>
        ///   Gets or sets the auxiliary sphere type.
        /// </summary>
        public AuxiliarySphereType AuxiliarySphereType { get; set; }

        /// <summary>
        ///   The horizontal 0 point in geographic terms
        /// </summary>
        public double? CentralMeridian { get; set; }

        /// <summary>
        ///   Gets or sets the Reference Code
        /// </summary>
        [Obsolete("Use AuthorityCode instead")] // Marked obsolete in 1.7.
        public int EpsgCode { get; set; }

        /// <summary>
        ///   The false easting for this coordinate system
        /// </summary>
        public double? FalseEasting { get; set; }

        /// <summary>
        ///   The false northing for this coordinate system
        /// </summary>
        public double? FalseNorthing { get; set; }

        /// <summary>
        ///   Gets or sets a boolean indicating a geocentric latitude parameter
        /// </summary>
        public bool Geoc { get; set; }

        /// <summary>
        ///   The geographic information
        /// </summary>
        public GeographicInfo GeographicInfo { get; set; }

        /// <summary>
        ///   Gets or sets a boolean that indicates whether or not this
        ///   projection is geocentric.
        /// </summary>
        public bool IsGeocentric { get; set; }

        /// <summary>
        ///   True if this coordinate system is expressed only using geographic coordinates
        /// </summary>
        public bool IsLatLon { get; set; }

        /// <summary>
        ///   Gets or sets a boolean indicating whether this projection applies to the
        ///   southern coordinate system or not.
        /// </summary>
        public bool IsSouth { get; set; }

        /// <summary>
        ///   True if the transform is defined.  That doesn't really mean it accurately represents the named
        ///   projection, but rather it won't throw a null exception during transformation for the lack of
        ///   a transform definition.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Transform != null;
            }
        }

        /// <summary>
        ///   The zero point in geographic terms
        /// </summary>
        public double? LatitudeOfOrigin { get; set; }

        /// <summary>
        /// The longitude of center for this coordinate system
        /// </summary>
        public double? LongitudeOfCenter { get; set; }

        /// <summary>
        ///   Gets or sets the M.
        /// </summary>
        /// <value>
        ///   The M.
        /// </value>
        public double? M { get; set; }

        /// <summary>
        ///   Gets or sets the name of this projection information
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   A boolean that indicates whether to use the /usr/share/proj/proj_def.dat defaults file (proj4 parameter "no_defs").
        /// </summary>
        public bool NoDefs { get; set; }

        /// <summary>
        ///   Gets or sets a boolean for the over-ranging flag
        /// </summary>
        public bool Over { get; set; }

        /// <summary>
        ///   The scale factor for this coordinate system
        /// </summary>
        public double ScaleFactor
        {
            get
            {
                return _scaleFactor ?? 1;
            }

            set
            {
                _scaleFactor = value;
            }
        }

        /// <summary>
        ///   The line of latitude where the scale information is preserved.
        /// </summary>
        public double? StandardParallel1 { get; set; }

        /// <summary>
        /// The standard parallel 2.
        /// </summary>
        public double? StandardParallel2 { get; set; }

        /// <summary>
        ///   Gets or sets the transform that converts between geodetic coordinates and projected coordinates.
        /// </summary>
        public ITransform Transform { get; set; }

        /// <summary>
        ///   The unit being used for measurements.
        /// </summary>
        public LinearUnit Unit { get; set; }

        /// <summary>
        ///   Gets or sets the w.
        /// </summary>
        /// <value>
        ///   The w.
        /// </value>
        public double? W { get; set; }

        /// <summary>
        ///   Gets or sets the integer zone parameter if it is specified.
        /// </summary>
        public int? Zone { get; set; }

        /// <summary>
        ///   Gets or sets the alpha/ azimuth.
        /// </summary>
        /// <value>
        ///   ? Used with Oblique Mercator and possibly a few others. For our purposes this is exactly the same as azimuth
        /// </value>
        public double? alpha { get; set; }

        /// <summary>
        ///   Gets or sets the BNS.
        /// </summary>
        /// <value>
        ///   The BNS.
        /// </value>
        public int? bns { get; set; }

        /// <summary>
        ///   Gets or sets the czech.
        /// </summary>
        /// <value>
        ///   The czech.
        /// </value>
        public int? czech { get; set; }

        /// <summary>
        ///   Gets or sets the guam.
        /// </summary>
        /// <value>
        ///   The guam.
        /// </value>
        public bool? guam { get; set; }

        /// <summary>
        ///   Gets or sets the h.
        /// </summary>
        /// <value>
        ///   The h.
        /// </value>
        public double? h { get; set; }

        /// <summary>
        ///   Gets or sets the lat_ts.
        /// </summary>
        /// <value>
        ///   Latitude of true scale.
        /// </value>
        public double? lat_ts { get; set; }

        /// <summary>
        ///   Gets or sets the lon_1.
        /// </summary>
        /// <value>
        ///   The lon_1.
        /// </value>
        public double? lon_1 { get; set; }

        /// <summary>
        ///   Gets or sets the lon_2.
        /// </summary>
        /// <value>
        ///   The lon_2.
        /// </value>
        public double? lon_2 { get; set; }

        /// <summary>
        ///   Gets or sets the lonc.
        /// </summary>
        /// <value>
        ///   The lonc.
        /// </value>
        public double? lonc { get; set; }

        /// <summary>
        ///   Gets or sets the m. Named mGeneral to prevent CLS conflicts.
        /// </summary>
        /// <value>
        ///   The m.
        /// </value>
        public double? mGeneral { get; set; }

        /// <summary>
        ///   Gets or sets the n.
        /// </summary>
        /// <value>
        ///   The n.
        /// </value>
        public double? n { get; set; }

        /// <summary>
        ///   Gets or sets the no_rot.
        /// </summary>
        /// <value>
        ///   The no_rot. Seems to be used as a boolean.
        /// </value>
        public int? no_rot { get; set; }

        /// <summary>
        ///   Gets or sets the no_uoff.
        /// </summary>
        /// <value>
        ///   The no_uoff. Seems to be used as a boolean.
        /// </value>
        public int? no_uoff { get; set; }

        /// <summary>
        ///   Gets or sets the rot_conv.
        /// </summary>
        /// <value>
        ///   The rot_conv. Seems to be used as a boolean.
        /// </value>
        public int? rot_conv { get; set; }

        /// <summary>
        ///   Gets or sets the to_meter.
        /// </summary>
        /// <value>
        ///   Multiplier to convert map units to 1.0m
        /// </value>
        public double? to_meter { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the lon_1 parameter in radians
        /// </summary>
        /// <returns>
        /// The get lam 1.
        /// </returns>
        public double Lam1
        {
            get
            {
                if (StandardParallel1 != null)
                {
                    return StandardParallel1.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the lon_2 parameter in radians
        /// </summary>
        /// <returns>
        /// The get lam 2.
        /// </returns>
        public double Lam2
        {
            get
            {
                if (StandardParallel2 != null)
                {
                    return StandardParallel2.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the lat_1 parameter multiplied by radians
        /// </summary>
        /// <returns>
        /// The get phi 1.
        /// </returns>
        public double Phi1
        {
            get
            {
                if (StandardParallel1 != null)
                {
                    return StandardParallel1.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the lat_2 parameter multiplied by radians
        /// </summary>
        /// <returns>
        /// The get phi 2.
        /// </returns>
        public double Phi2
        {
            get
            {
                if (StandardParallel2 != null)
                {
                    return StandardParallel2.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
        }

        /// <summary>
        /// Sets the lambda 0, or central meridian in radial coordinates
        /// </summary>
        /// <param name="value">
        /// The value of Lambda 0 in radians
        /// </param>
        public double Lam0
        {
            get
            {
                if (CentralMeridian != null)
                {
                    return CentralMeridian.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
            set
            {
                CentralMeridian = value / GeographicInfo.Unit.Radians;
            }
        }

        /// <summary>
        /// Sets the phi 0 or latitude of origin in radial coordinates
        /// </summary>
        /// <param name="value">
        /// </param>
        public double Phi0
        {
            get
            {
                if (LatitudeOfOrigin != null)
                {
                    return LatitudeOfOrigin.Value * GeographicInfo.Unit.Radians;
                }

                return 0;
            }
            set
            {
                LatitudeOfOrigin = value / GeographicInfo.Unit.Radians;
            }
        }

        /// <summary>
        /// Expresses the entire projection as the Esri well known text format that can be found in .prj files
        /// </summary>
        /// <returns>
        /// The generated string
        /// </returns>
        public string ToEsriString()
        {
            Spheroid tempSpheroid = new Spheroid(Proj4Ellipsoid.WGS_1984);

            // changed by JK to fix the web mercator auxiliary sphere Esri string
            if (Name == "WGS_1984_Web_Mercator_Auxiliary_Sphere")
            {
                tempSpheroid = GeographicInfo.Datum.Spheroid;
                GeographicInfo.Datum.Spheroid = new Spheroid(Proj4Ellipsoid.WGS_1984);
            }

            // if (_auxiliarySphereType != AuxiliarySphereType.NotSpecified)
            // {
            // tempSpheroid = _geographicInfo.Datum.Spheroid;
            // _geographicInfo.Datum.Spheroid = new Spheroid(Proj4Ellipsoid.WGS_1984);
            // }
            string result = string.Empty;
            if (!IsLatLon)
            {
                result += String.Format(@"PROJCS[""{0}"",", Name);
            }

            result += GeographicInfo.ToEsriString();
            if (IsLatLon)
            {
                return result;
            }

            result += ", ";
            if (Transform != null)
            {
                // Since we can have semi-colon delimited names for aliases, we have to output just one in the WKT. Issue #297
                var name = Transform.Name.Contains(";")
                               ? Transform.Name.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0]
                               : Transform.Name;
                result += String.Format(@"PROJECTION[""{0}""],", name);
            }

            if (FalseEasting != null)
            {
                string alias = FalseEastingAlias ?? "False_Easting";
                result += @"PARAMETER[""" + alias + @"""," + Convert.ToString(FalseEasting / Unit.Meters, CultureInfo.InvariantCulture) + "],";
            }

            if (FalseNorthing != null)
            {
                string alias = FalseNorthingAlias ?? "False_Northing";
                result += @"PARAMETER[""" + alias + @"""," + Convert.ToString(FalseNorthing / Unit.Meters, CultureInfo.InvariantCulture) + "],";
            }

            if (CentralMeridian != null && CentralMeridianValid())
            {
                result += @"PARAMETER[""Central_Meridian""," + Convert.ToString(CentralMeridian, CultureInfo.InvariantCulture) + "],";
            }

            if (StandardParallel1 != null)
            {
                result += @"PARAMETER[""Standard_Parallel_1""," + Convert.ToString(StandardParallel1, CultureInfo.InvariantCulture) + "],";
            }

            if (StandardParallel2 != null)
            {
                result += @"PARAMETER[""Standard_Parallel_2""," + Convert.ToString(StandardParallel2, CultureInfo.InvariantCulture) + "],";
            }

            if (_scaleFactor != null)
            {
                result += @"PARAMETER[""Scale_Factor""," + Convert.ToString(_scaleFactor, CultureInfo.InvariantCulture) + "],";
            }

            if (alpha != null)
            {
                result += @"PARAMETER[""Azimuth""," + Convert.ToString(alpha, CultureInfo.InvariantCulture) + "],";
            }

            if (LongitudeOfCenter != null)
            {
                string alias = LongitudeOfCenterAlias ?? "Longitude_Of_Center";
                result += @"PARAMETER[""" + alias + @"""," + Convert.ToString(LongitudeOfCenter, CultureInfo.InvariantCulture) + "],";
            }

            if (_longitudeOf1st != null)
            {
                result += @"PARAMETER[""Longitude_Of_1st""," + Convert.ToString(_longitudeOf1st, CultureInfo.InvariantCulture) + "],";
            }

            if (_longitudeOf2nd != null)
            {
                result += @"PARAMETER[""Longitude_Of_2nd""," + Convert.ToString(_longitudeOf2nd, CultureInfo.InvariantCulture) + "],";
            }

            if (LatitudeOfOrigin != null)
            {
                string alias = LatitudeOfOriginAlias ?? "Latitude_Of_Origin";
                result += @"PARAMETER[""" + alias + @"""," + Convert.ToString(LatitudeOfOrigin, CultureInfo.InvariantCulture) + "],";
            }

            // changed by JK to fix the web mercator auxiliary sphere Esri string
            if (Name == "WGS_1984_Web_Mercator_Auxiliary_Sphere")
            {
                result += @"PARAMETER[""Auxiliary_Sphere_Type""," + ((int)AuxiliarySphereType) + ".0],";
            }

            result += Unit.ToEsriString() + "]";
            // changed by JK to fix the web mercator auxiliary sphere Esri string
            if (Name == "WGS_1984_Web_Mercator_Auxiliary_Sphere")
            {
                GeographicInfo.Datum.Spheroid = new Spheroid(Proj4Ellipsoid.WGS_1984);
                GeographicInfo.Datum.Spheroid = tempSpheroid;
            }

            return result;
        }

        /// <summary>
        /// Using the specified code, this will attempt to look up the related reference information
        ///   from the appropriate pcs code.
        /// </summary>
        /// <param name="epsgCode">
        /// The epsg Code.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when there is no projection for given epsg code</exception>
        public static ProjectionInfo FromEpsgCode(int epsgCode)
        {
            return FromAuthorityCode("EPSG", epsgCode);
        }

        /// <summary>
        /// Using the specified code, this will attempt to look up the related reference information from the appropriate authority code.
        /// </summary>
        /// <param name="authority"> The authority. </param>
        /// <param name="code">  The code. </param>
        /// <returns>ProjectionInfo</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when there is no projection for given authority and code</exception>
        public static ProjectionInfo FromAuthorityCode(string authority, int code)
        {
            var pi = AuthorityCodeHandler.Instance[string.Format("{0}:{1}", authority, code)];
            if (pi != null)
            {
                // we need to copy the projection information because the Authority Codes implementation returns its one and only
                // in memory copy of the ProjectionInfo. Passing it to the caller might introduce unintended results.
                var info = FromProj4String(pi.ToProj4String());
                info.Name = pi.Name;
                info.NoDefs = true;
                info.Authority = authority;
                info.AuthorityCode = code;
                info.EpsgCode = code;
                return info;
            }

            throw new ArgumentOutOfRangeException("authority", "Authority Code not found.");
        }

        /// <summary>
        /// Parses the entire projection from an Esri string.  In some cases, this will have
        ///   default projection information since only geographic information is obtained.
        /// </summary>
        /// <param name="esriString">
        /// The Esri string to parse
        /// </param>
        public static ProjectionInfo FromEsriString(string esriString)
        {
            if (String.IsNullOrWhiteSpace(esriString))
            {
                // Return a default 'empty' projection
                return new ProjectionInfo();
            }

            //special case for Krovak Projection
            //todo use a lookup table instead of hard coding the projection here
            if (esriString.Contains("Krovak"))
                return KnownCoordinateSystems.Projected.NationalGrids.SJTSKKrovakEastNorth;

            var info = new ProjectionInfo();
            info.NoDefs = true;
            if (!info.TryParseEsriString(esriString))
            {
                throw new InvalidEsriFormatException(esriString);
            }

            return info;
        }

        /// <summary>
        /// Creates a new projection and automatically reads in the proj4 string
        /// </summary>
        /// <param name="proj4String">
        /// The proj4String to read in while defining the projection
        /// </param>
        public static ProjectionInfo FromProj4String(string proj4String)
        {
            var info = new ProjectionInfo();
            info.ParseProj4String(proj4String);
            return info;
        }

        /// <summary>
        /// Open a given prj fileName
        /// </summary>
        /// <param name="prjFilename">
        /// </param>
        public static ProjectionInfo Open(string prjFilename)
        {
            StreamReader sr = File.OpenText(prjFilename);
            string prj = sr.ReadLine();
            sr.Close();

            return FromEsriString(prj);
        }

        /// <summary>
        /// Gets a boolean that is true if the Esri WKT string created by the projections matches.
        ///   There are multiple ways to write the same projection, but the output Esri WKT string
        ///   should be a good indicator of whether or not they are the same.
        /// </summary>
        /// <param name="other">
        /// The other projection to compare with.
        /// </param>
        /// <returns>
        /// Boolean, true if the projections are the same.
        /// </returns>
        public bool Equals(ProjectionInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return ToEsriString().Equals(other.ToEsriString()) || ToProj4String().Equals(other.ToProj4String());
        }

        /// <summary>
        /// If this is a geographic coordinate system, this will show decimal degrees.  Otherwise,
        ///   this will show the linear unit units.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        /// <returns>
        /// The get unit text.
        /// </returns>
        public string GetUnitText(double quantity)
        {
            if (Geoc || IsLatLon)
            {
                if (Math.Abs(quantity) < 1)
                {
                    return "of a decimal degree";
                }

                return quantity == 1 ? "decimal degree" : "decimal degrees";
            }

            if (Math.Abs(quantity) < 1)
            {
                return "of a " + Unit.Name;
            }

            if (Math.Abs(quantity) == 1)
            {
                return Unit.Name;
            }

            if (Math.Abs(quantity) > 1)
            {
                // The following are frequently followed by specifications, so adding s doesn't work
                if (Unit.Name.Contains("Foot") || Unit.Name.Contains("foot"))
                {
                    return Unit.Name
                        .Replace("Foot", "Feet")
                        .Replace("foot", "Feet");
                }

                if (Unit.Name.Contains("Yard") || Unit.Name.Contains("yard"))
                {
                    return Unit.Name
                        .Replace("Yard", "Yards")
                        .Replace("yard", "Yards");
                }

                if (Unit.Name.Contains("Chain") || Unit.Name.Contains("chain"))
                {
                    return Unit.Name
                        .Replace("Chain", "Chains")
                        .Replace("chain", "Chains");
                }

                if (Unit.Name.Contains("Link") || Unit.Name.Contains("link"))
                {
                    return Unit.Name
                        .Replace("Link", "Links")
                        .Replace("link", "Links");
                }

                return Unit.Name + "s";
            }

            return Unit.Name;
        }

        /// <summary>
        /// Exports this projection info by saving it to a *.prj file.
        /// </summary>
        /// <param name="prjFilename">
        /// The prj file to save to
        /// </param>
        public void SaveAs(string prjFilename)
        {
            if (File.Exists(prjFilename))
            {
                File.Delete(prjFilename);
            }

            using (StreamWriter sw = File.CreateText(prjFilename))
            {
                sw.WriteLine(ToEsriString());
            }
        }

        /// <summary>
        /// Returns a representaion of this object as a Proj4 string.
        /// </summary>
        /// <returns>
        /// The to proj 4 string.
        /// </returns>
        public string ToProj4String()
        {
            //enforce invariant culture
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var result = new StringBuilder();

            Append(result, "x_0", FalseEasting);
            Append(result, "y_0", FalseNorthing);
            if (_scaleFactor != 1)
            {
                Append(result, "k_0", _scaleFactor);
            }
            
            Append(result, "lat_0", LatitudeOfOrigin);
            Append(result, "lon_0", CentralMeridian);
            Append(result, "lat_1", StandardParallel1);
            Append(result, "lat_2", StandardParallel2);

            if (Over)
            {
                result.Append(" +over");
            }

            if (Geoc)
            {
                result.Append(" +geoc");
            }

            Append(result, "alpha", alpha);
            Append(result, "lonc", LongitudeOfCenter);
            Append(result, "zone", Zone);

            if (IsLatLon)
            {
                Append(result, "proj", "longlat");
            }
            else
            {
                if (Transform != null)
                {
                    Append(result, "proj", Transform.Proj4Name);
                }

                // skips over to_meter if this is geographic or defaults to 1
                if (Unit.Meters != 1)
                {
                    // we don't create +units=m or +units=f instead we use.
                    Append(result, "to_meter", Unit.Meters);
                }
            }

            result.Append(GeographicInfo.ToProj4String());

            if (IsSouth)
            {
                result.Append(" +south");
            }

            if (NoDefs)
            {
                result.Append(" +no_defs");
            }

            //reset the culture info
            Thread.CurrentThread.CurrentCulture = originalCulture;

            return result.ToString();
        }

        /// <summary>
        /// This overrides ToString to get the Esri name of the projection.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return Name;
            }

            if (Transform != null && !string.IsNullOrEmpty(Transform.Name))
            {
                return Transform.Name;
            }

            if (IsLatLon)
            {
                if (GeographicInfo == null || string.IsNullOrEmpty(GeographicInfo.Name))
                {
                    return "LatLon";
                }

                return GeographicInfo.Name;
            }

            return ToProj4String();
        }

        #endregion

        #region Methods

        private bool CentralMeridianValid()
        {
            // CentralMeridian (lam0) is calculated for these coordinate system, but IS NOT part of their Esri WKTs
            return Transform.Name.ToLower() != "hotine_oblique_mercator_azimuth_natural_origin" &&
                   Transform.Name.ToLower() != "hotine_oblique_mercator_azimuth_center";
        }

        private static void Append(StringBuilder result, string name, object value)
        {
            if (value == null) return;

            // The round-trip ("R") format specifier guarantees that a numeric value that is converted to a string will be parsed back into the same numeric value. 
            // This format is supported only for the Single, Double, and BigInteger types.
            if (value is double)
            {
                value = ((double) value).ToString("R", CultureInfo.InvariantCulture);
            }
            else if (value is float)
            {
                value = ((float) value).ToString("R", CultureInfo.InvariantCulture);
            }

            result.AppendFormat(CultureInfo.InvariantCulture, " +{0}={1}", name, value);
        }

        private static double? GetParameter(string name, string esriString)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(esriString))
                return null;

            double? result = null;
            int iStart = esriString.IndexOf(@"PARAMETER[""" + name + "\"", StringComparison.InvariantCultureIgnoreCase);
            if (iStart >= 0)
            {
                iStart += 13 + name.Length;
                int iEnd = esriString.IndexOf(']', iStart);
                string tst = esriString.Substring(iStart, iEnd - iStart);
                result = double.Parse(tst, CultureInfo.InvariantCulture);
            }

            return result;
        }

        private static double? GetParameter(IEnumerable<string> parameterNames, ref string alias, string esriString)
        {
            if (parameterNames == null || String.IsNullOrEmpty(esriString))
                return null;

            // Return the first result that returns a value
            foreach (string parameterName in parameterNames)
            {
                var result = GetParameter(parameterName, esriString);
                if (result != null)
                {
                    alias = parameterName;
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempts to parse known parameters from the set of proj4 parameters
        /// </summary>
        private void ParseProj4String(string proj4String)
        {
            if (string.IsNullOrEmpty(proj4String))
            {
                return;
            }

            // If it has a zone, and the projection is tmerc, it actually needs the utm initialization
            var tmercIsUtm = proj4String.Contains("zone=");
            if (tmercIsUtm)
            {
                ScaleFactor = 0.9996; // Default scale factor for utm
                                      // This also needed to write correct Esri String from given projection 
            }

            string[] sections = proj4String.Split('+');
            foreach (string str in sections)
            {
                string s = str.Trim();
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                // single token commands
                if (s == "no_defs")
                {
                    NoDefs = true;
                    continue;
                }
                if (s == "over")
                {
                    Over = true;
                    continue;
                }
                if (s == "geoc")
                {
                    Geoc = GeographicInfo.Datum.Spheroid.EccentricitySquared() != 0;
                    continue;
                }
                else if (s == "south")
                {
                    IsSouth = true;
                    continue;
                }
                else if (s == "R_A")
                {
                    //+R_A tells PROJ.4 to use a spherical radius that
                    //gives a sphere with the same surface area as the original ellipsoid.  I
                    //imagine I added this while trying to get the results to match PCI's GCTP
                    //based implementation though the history isn't clear on the details.
                    //the R_A parameter indicates that an authalic auxiliary sphere should be used.
                    //from http://pdl.perl.org/?docs=Transform/Proj4&title=PDL::Transform::Proj4#r_a
                    AuxiliarySphereType = AuxiliarySphereType.Authalic;
                }
                else if (s == "to")
                {
                    // some "+to" parameters exist... e.g., DutchRD. but I'm not sure what to do with them.
                    // they seem to specify a second projection
                    Trace.WriteLine(
                        String.Format("ProjectionInfo.ParseProj4String: command 'to' not supported and the portion of the string after 'to' will not be processed in '{0}'", proj4String));
                    break;
                }

                // parameters
                string[] set = s.Split('=');
                if (set.Length != 2)
                {
                    Trace.WriteLine(
                        String.Format("ProjectionInfo.ParseProj4String: command '{0}' not understood in '{1}'", s, proj4String));
                    continue;
                }

                string name = set[0].Trim();
                string value = set[1].Trim();
                switch (name)
                {
                    case "lonc":
                        LongitudeOfCenter = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "alpha":
                        alpha = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "x_0":
                        FalseEasting = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "y_0":
                        FalseNorthing = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "k":
                    case "k_0":
                        _scaleFactor = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "lat_0":
                        LatitudeOfOrigin = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "lat_1":
                        StandardParallel1 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "lat_2":
                        StandardParallel2 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "lon_0":
                        CentralMeridian = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "lat_ts":
                        StandardParallel1 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;

                    case "zone":
                        Zone = int.Parse(value, CultureInfo.InvariantCulture);
                        break;
                  
                    case "proj":

                        if (value == "longlat")
                        {
                            IsLatLon = true;
                        }

                        Transform = tmercIsUtm
                            ? new UniversalTransverseMercator()
                            : TransformManager.DefaultTransformManager.GetProj4(value);

                        break;

                    case "to_meter":

                        Unit.Meters = double.Parse(value, CultureInfo.InvariantCulture);
                        if (Unit.Meters == .3048)
                        {
                            Unit.Name = "Foot"; // International Foot
                        }
                        else if (Unit.Meters > .3048 && Unit.Meters < .305)
                        {
                            Unit.Name = "Foot_US";
                        }

                        break;

                    case "units":
                        if (value == "m")
                        {
                            // do nothing, since the default is meter anyway.
                        }
                        else if (value == "ft" || value == "f")
                        {
                            Unit.Name = "Foot";
                            Unit.Meters = .3048;
                        }
                        else if (value == "us-ft")
                        {
                            Unit.Name = "Foot_US";
                            Unit.Meters = .304800609601219;
                        }

                        break;

                    case "pm":
                        GeographicInfo.Meridian.pm = value;
                        //// added by Jiri Kadlec - pm should also specify the CentralMeridian
                        //if (value != null)
                        //{
                        //    CentralMeridian = GeographicInfo.Meridian.Longitude;
                        //}
                        break;

                    case "datum":

                        // Even though th ellipsoid is set by its known definition, we permit overriding it with a specifically defined ellps parameter
                        GeographicInfo.Datum = new Datum(value);
                        break;

                    case "nadgrids":
                        GeographicInfo.Datum.NadGrids = value.Split(',');

                        if (value != "@null")
                        {
                            GeographicInfo.Datum.DatumType = DatumType.GridShift;
                        }

                        break;

                    case "towgs84":
                        GeographicInfo.Datum.InitializeToWgs84(value.Split(','));
                        break;

                    case "ellps":

                        // Even though th ellipsoid is set by its known definition, we permit overriding it with a specifically defined ellps parameter
                        // generally ellps will not be used in the same string as a,b,rf,R.
                        GeographicInfo.Datum.Spheroid = new Spheroid(value);
                        break;

                    case "a":
                    case "R":
                        GeographicInfo.Datum.Spheroid.EquatorialRadius = double.Parse(value, CultureInfo.InvariantCulture);
                        GeographicInfo.Datum.Spheroid.KnownEllipsoid = Proj4Ellipsoid.Custom; // This will provide same spheroid on export to Proj4 string
                        break;

                    case "b":
                        GeographicInfo.Datum.Spheroid.PolarRadius = double.Parse(value, CultureInfo.InvariantCulture);
                        GeographicInfo.Datum.Spheroid.KnownEllipsoid = Proj4Ellipsoid.Custom; // This will provide same spheroid on export to Proj4 string
                        break;

                    case "rf":
                        Debug.Assert(GeographicInfo.Datum.Spheroid.EquatorialRadius > 0, "a must appear before rf");
                        GeographicInfo.Datum.Spheroid.InverseFlattening = double.Parse(
                            value, CultureInfo.InvariantCulture);
                        break;

                    default:
                        Trace.WriteLine(string.Format("Unrecognized parameter skipped {0}.", name));
                        break;
                }
            }

            if (Transform != null)
            {
                Transform.Init(this);
            }
        }

        /// <summary>
        /// This will try to read the string, but if the validation fails, then it will return false,
        ///   rather than throwing an exception.
        /// </summary>
        /// <param name="esriString">
        /// The string to test and define this projection if possible.
        /// </param>
        /// <returns>
        /// Boolean, true if at least the GEOGCS tag was found.
        /// </returns>
        public bool TryParseEsriString(string esriString)
        {
            if (esriString == null)
            {
                return false;
            }

            if (!esriString.Contains("GEOGCS"))
            {
                return false;
            }

            if (esriString.Contains("PROJCS") == false)
            {
                GeographicInfo.ParseEsriString(esriString);
                IsLatLon = true;
                Transform = new LongLat();
                Transform.Init(this);
                return true;
            }

            int iStart = esriString.IndexOf(@""",", StringComparison.Ordinal);
            Name = esriString.Substring(8, iStart - 8);
            int iEnd = esriString.IndexOf("PARAMETER", StringComparison.Ordinal);
            string gcs;
            if (iEnd != -1)
            {
                gcs = esriString.Substring(iStart + 1, iEnd - (iStart + 2));
            }
            else
            {
                // an odd Esri projection string that doesn't have PARAMETER
                gcs = esriString.Substring(iStart + 1);
            }
            GeographicInfo.ParseEsriString(gcs);

            FalseEasting = GetParameter(new string[] { "False_Easting", "Easting_At_False_Origin" }, ref FalseEastingAlias, esriString);
            FalseNorthing = GetParameter(new string[] { "False_Northing", "Northing_At_False_Origin" }, ref FalseNorthingAlias, esriString);
            CentralMeridian = GetParameter("Central_Meridian", esriString);
            // Esri seems to indicate that these should be treated the same, but they aren't here... http://support.esri.com/en/knowledgebase/techarticles/detail/39992
            // CentralMeridian = GetParameter(new string[] { "Longitude_Of_Center", "Central_Meridian", "Longitude_Of_Origin" }, ref LongitudeOfCenterAlias, esriString);
            LongitudeOfCenter = GetParameter("Longitude_Of_Center", esriString);
            StandardParallel1 = GetParameter("Standard_Parallel_1", esriString);
            StandardParallel2 = GetParameter("Standard_Parallel_2", esriString);
            _scaleFactor = GetParameter("Scale_Factor", esriString);
            alpha = GetParameter("Azimuth", esriString);
            _longitudeOf1st = GetParameter("Longitude_Of_1st", esriString);
            _longitudeOf2nd = GetParameter("Longitude_Of_2nd", esriString);
            LatitudeOfOrigin = GetParameter(new[] { "Latitude_Of_Origin", "Latitude_Of_Center", "Central_Parallel" }, ref LatitudeOfOriginAlias, esriString);
            iStart = esriString.LastIndexOf("UNIT", StringComparison.Ordinal);
            string unit = esriString.Substring(iStart, esriString.Length - iStart);
            Unit.ParseEsriString(unit);

            if (esriString.Contains("PROJECTION"))
            {
                iStart = esriString.IndexOf("PROJECTION", StringComparison.Ordinal) + 12;
                iEnd = esriString.IndexOf("]", iStart, StringComparison.Ordinal) - 1;
                string projection = esriString.Substring(iStart, iEnd - iStart);

                Transform = TransformManager.DefaultTransformManager.GetProjection(projection);
                Transform.Init(this);
            }

            double? auxType = GetParameter("Auxiliary_Sphere_Type", esriString);
            if (auxType != null)
            {
                // While the Esri implementation sort of tip-toes around the datum transform,
                // we simply ensure that the spheroid becomes properly spherical based on the
                // parameters we have here.  (The sphereoid will be read as WGS84).
                AuxiliarySphereType = (AuxiliarySphereType)auxType;
                if (AuxiliarySphereType == AuxiliarySphereType.SemimajorAxis)
                {
                    // added by Jiri to properly re-initialize the 'web mercator auxiliary sphere' transform
                    Transform = KnownCoordinateSystems.Projected.World.WebMercator.Transform;
                }
                else if (AuxiliarySphereType == AuxiliarySphereType.SemiminorAxis)
                {
                    double r = GeographicInfo.Datum.Spheroid.PolarRadius;
                    GeographicInfo.Datum.Spheroid = new Spheroid(r);
                }
                else if (AuxiliarySphereType == AuxiliarySphereType.Authalic
                         || AuxiliarySphereType == AuxiliarySphereType.AuthalicWithConvertedLatitudes)
                {
                    double a = GeographicInfo.Datum.Spheroid.EquatorialRadius;
                    double b = GeographicInfo.Datum.Spheroid.PolarRadius;
                    double r =
                        Math.Sqrt(
                            (a * a
                             +
                             a * b * b
                             / (Math.Sqrt(a * a - b * b) * Math.Log((a + Math.Sqrt(a * a - b * b)) / b, Math.E)))
                            / 2);
                    GeographicInfo.Datum.Spheroid = new Spheroid(r);
                }
            }

            if (FalseEasting != null)
            {
                FalseEasting = FalseEasting * Unit.Meters;
            }

            if (FalseNorthing != null)
            {
                FalseNorthing = FalseNorthing * Unit.Meters;
            }

            return true;
        }

        #endregion

        #region IEsriString Members

        /// <summary>
        /// Re-sets the parameters of this projection info by parsing the esri projection string
        /// </summary>
        /// <param name="esriString">The projection information string in Esri WKT format</param>
        public void ParseEsriString(string esriString)
        {
            TryParseEsriString(esriString);
        }

        #endregion
    }
}