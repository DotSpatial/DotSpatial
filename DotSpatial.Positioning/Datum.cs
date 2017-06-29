// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Collections.Generic;

#if !PocketPC || DesignTime

using System.ComponentModel;
using System.Linq;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a coordinate system based on interpretations of the Earth's shape and size.
    /// </summary>
    /// <seealso cref="Ellipsoid">Ellipsoid Class</seealso>
    /// <remarks><para>Over the course of history, advances in technology have given people the
    /// ability to more accurately measure the shape and size of the Earth. Since countries
    /// have built significant infrastructure based upon older coordinate systems, they
    /// cannot immediately abandon them in favor of new ones. As a result, there are now
    /// over fifty interpretations of Earth's shape and size in use all over the
    /// world.</para>
    ///   <para>Some datums, such as World Geodetic System 1984 (or WGS84 for short) are
    /// becoming more widely used throughout the world, and this datum is used by nearly
    /// all GPS devices. However, while the world is slowly standardizing its datums, some
    /// datums will not be abandoned because they remain quite accurate for a specific,
    /// local area.</para>
    ///   <para>A datum on its own is nothing more than a more granular interpretation of an
    /// ellipsoid. Typically, more specific coordinate transformation information is
    /// further associated with a datum to produce meaningful information. For example,
    /// Helmert and Molodensky coordinate conversion formulas use several local conversion
    /// parameters for each datum.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be set via constructors).</para></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Datum : IEquatable<Datum>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly int _epsgNumber = 32767;
        /// <summary>
        ///
        /// </summary>
        private readonly Longitude _primeMeridian;
        /// <summary>
        ///
        /// </summary>
        private readonly Ellipsoid _ellipsoid;
        /// <summary>
        ///
        /// </summary>
        private readonly string _name;

        /// <summary>
        ///
        /// </summary>
        private static readonly List<Datum> _datums = new List<Datum>(16);
        /// <summary>
        ///
        /// </summary>
        private static readonly List<Datum> _epsgDatums = new List<Datum>(512);

        #region Fields

        #region EPSG Datums

        /// <summary>
        /// Not specified (based on the Airy 1830 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedAiry1830 = new Datum(6001, Ellipsoid.FromEpsgNumber(7001), "Not specified (based on Airy 1830 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Airy Modified 1849 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedAiryModified1849 = new Datum(6002, Ellipsoid.FromEpsgNumber(7002), "Not specified (based on Airy Modified 1849 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Australian National Spheroid ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedAustralianNationalSpheroid = new Datum(6003, Ellipsoid.FromEpsgNumber(7003), "Not specified (based on Australian National Spheroid)");
        /// <summary>
        /// Not specified (based on the Bessel 1841 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedBessel1841 = new Datum(6004, Ellipsoid.FromEpsgNumber(7004), "Not specified (based on Bessel 1841 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Bessel Modified ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedBesselModified = new Datum(6005, Ellipsoid.FromEpsgNumber(7005), "Not specified (based on Bessel Modified ellipsoid)");
        /// <summary>
        /// Not specified (based on the Bessel Namibia (GLM) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedBesselNamibiaGlm = new Datum(6006, Ellipsoid.FromEpsgNumber(7046), "Not specified (based on Bessel Namibia ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1858 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1858 = new Datum(6007, Ellipsoid.FromEpsgNumber(7007), "Not specified (based on Clarke 1858 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1866 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1866 = new Datum(6008, Ellipsoid.FromEpsgNumber(7008), "Not specified (based on Clarke 1866 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1866 Michigan ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1866Michigan = new Datum(6009, Ellipsoid.FromEpsgNumber(7009), "Not specified (based on Clarke 1866 Michigan ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 (Benoit) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880Benoit = new Datum(6010, Ellipsoid.FromEpsgNumber(7010), "Not specified (based on Clarke 1880 (Benoit) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 (IGN) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880IGN = new Datum(6011, Ellipsoid.FromEpsgNumber(7011), "Not specified (based on Clarke 1880 (IGN) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 (RGS) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880RGS = new Datum(6012, Ellipsoid.FromEpsgNumber(7012), "Not specified (based on Clarke 1880 (RGS) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 (Arc) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880Arc = new Datum(6013, Ellipsoid.FromEpsgNumber(7013), "Not specified (based on Clarke 1880 (Arc) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 (SGA 1922) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880SGA1922 = new Datum(6014, Ellipsoid.FromEpsgNumber(7014), "Not specified (based on Clarke 1880 (SGA 1922) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest 1830 (1937 Adjustment) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest1937Adjustment = new Datum(6015, Ellipsoid.FromEpsgNumber(7015), "Not specified (based on Everest 1830 (1937 Adjustment) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest 1830 (1967 Definition) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest1967Definition = new Datum(6016, Ellipsoid.FromEpsgNumber(7016), "Not specified (based on Everest 1830 (1967 Definition) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest 1830 Modified ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest1830Modified = new Datum(6018, Ellipsoid.FromEpsgNumber(7018), "Not specified (based on Everest 1830 Modified ellipsoid)");
        /// <summary>
        /// Not specified (based on the GRS 1980 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedGrs1980 = new Datum(6019, Ellipsoid.FromEpsgNumber(7019), "Not specified (based on GRS 1980 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Helmert 1906 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedHelmert1906 = new Datum(6020, Ellipsoid.FromEpsgNumber(7020), "Not specified (based on Helmert 1906 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Indonesian National Spheroid ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedIndonesianNationalSpheroid = new Datum(6021, Ellipsoid.FromEpsgNumber(7021), "Not specified (based on Indonesian National Spheroid)");
        /// <summary>
        /// Not specified (based on the International 1924 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedInternational1924 = new Datum(6022, Ellipsoid.FromEpsgNumber(7022), "Not specified (based on International 1924 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Krassowsky 1940 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedKrassowsky1940 = new Datum(6024, Ellipsoid.FromEpsgNumber(7024), "Not specified (based on Krassowsky 1940 ellipsoid)");
        /// <summary>
        /// Not specified (based on the NWL 9D ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedNwl9D = new Datum(6025, Ellipsoid.FromEpsgNumber(7025), "Not specified (based on NWL 9D ellipsoid)");
        /// <summary>
        /// Not specified (based on the Plessis 1817 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedPlessis1817 = new Datum(6027, Ellipsoid.FromEpsgNumber(7027), "Not specified (based on Plessis 1817 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Struve 1860 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedStruve1860 = new Datum(6028, Ellipsoid.FromEpsgNumber(7028), "Not specified (based on Struve 1860 ellipsoid)");
        /// <summary>
        /// Not specified (based on the War Office ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedWarOffice = new Datum(6029, Ellipsoid.FromEpsgNumber(7029), "Not specified (based on War Office ellipsoid)");
        /// <summary>
        /// Not specified (based on the WGS 84 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedWGS84 = new Datum(6030, Ellipsoid.FromEpsgNumber(7030), "Not specified (based on WGS 84 ellipsoid)");
        /// <summary>
        /// Not specified (based on the GEM 10C ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedGem10C = new Datum(6031, Ellipsoid.FromEpsgNumber(7031), "Not specified (based on GEM 10C ellipsoid)");
        /// <summary>
        /// Not specified (based on the OSU86F ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedOsu86F = new Datum(6032, Ellipsoid.FromEpsgNumber(7032), "Not specified (based on OSU86F ellipsoid)");
        /// <summary>
        /// Not specified (based on the OSU91A ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedOsu91A = new Datum(6033, Ellipsoid.FromEpsgNumber(7033), "Not specified (based on OSU91A ellipsoid)");
        /// <summary>
        /// Not specified (based on the Clarke 1880 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1880 = new Datum(6034, Ellipsoid.FromEpsgNumber(7034), "Not specified (based on Clarke 1880 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Sphere ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedSphere = new Datum(6035, Ellipsoid.FromEpsgNumber(7035), "Not specified (based on Authalic Sphere)");
        /// <summary>
        /// Not specified (based on the GRS 1967 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedGrs1967 = new Datum(6036, Ellipsoid.FromEpsgNumber(7036), "Not specified (based on GRS 1967 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Average Terrestrial System 1977 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedAverageTerrestrialSystem1977 = new Datum(6041, Ellipsoid.FromEpsgNumber(7041), "Not specified (based on Average Terrestrial System 1977 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest (1830 Definition) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest1830Definition = new Datum(6042, Ellipsoid.FromEpsgNumber(7042), "Not specified (based on Everest (1830 Definition) ellipsoid)");
        /// <summary>
        /// Not specified (based on the WGS 72 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedWGS72 = new Datum(6043, Ellipsoid.FromEpsgNumber(7043), "Not specified (based on WGS 72 ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest 1830 (1962 Definition) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest18301962Definition = new Datum(6044, Ellipsoid.FromEpsgNumber(7044), "Not specified (based on Everest 1830 (1962 Definition) ellipsoid)");
        /// <summary>
        /// Not specified (based on the Everest 1830 (1975 Definition) ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedEverest1975Definition = new Datum(6045, Ellipsoid.FromEpsgNumber(7045), "Not specified (based on Everest 1830 (1975 Definition) ellipsoid)");
        /// <summary>
        /// Not specified (based on the GRS 1980 Authalic Sphere ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedGrs1980AuthalicSphere = new Datum(6047, Ellipsoid.FromEpsgNumber(7048), "Not specified (based on GRS 1980 Authalic Sphere)");
        /// <summary>
        /// Not specified (based on the Clarke 1866 Authalic Sphere ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedClarke1866AuthalicSphere = new Datum(6052, Ellipsoid.FromEpsgNumber(7052), "Not specified (based on Clarke 1866 Authalic Sphere)");
        /// <summary>
        /// Not specified (based on the International 1924 Authalic Sphere ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedInternational1924AuthalicSphere = new Datum(6053, Ellipsoid.FromEpsgNumber(7057), "Not specified (based on International 1924 Authalic Sphere)");
        /// <summary>
        /// Not specified (based on the Hughes 1980 ellipsoid)
        /// </summary>
        public static readonly Datum UnspecifiedHughes1980 = new Datum(6054, Ellipsoid.FromEpsgNumber(7058), "Not specified (based on Hughes 1980 ellipsoid)");
        /// <summary>
        /// Greek
        /// </summary>
        public static readonly Datum Greek = new Datum(6120, Ellipsoid.FromEpsgNumber(7004), "Greek");
        /// <summary>
        /// Greek Geodetic Reference System 1987
        /// </summary>
        public static readonly Datum GreekGeodeticReferenceSystem1987 = new Datum(6121, Ellipsoid.FromEpsgNumber(7019), "Greek Geodetic Reference System 1987");
        /// <summary>
        /// Average Terrestrial System 1977
        /// </summary>
        public static readonly Datum AverageTerrestrialSystem1977 = new Datum(6122, Ellipsoid.FromEpsgNumber(7041), "Average Terrestrial System 1977");
        /// <summary>
        /// Kartastokoordinaattijarjestelma (1966)
        /// </summary>
        public static readonly Datum Kartastokoordinaattijarjestelma1966 = new Datum(6123, Ellipsoid.FromEpsgNumber(7022), "Kartastokoordinaattijarjestelma (1966)");
        /// <summary>
        /// Rikets koordinatsystem 1990
        /// </summary>
        public static readonly Datum Riketskoordinatsystem1990 = new Datum(6124, Ellipsoid.FromEpsgNumber(7004), "Rikets koordinatsystem 1990");
        /// <summary>
        /// Samboja
        /// </summary>
        public static readonly Datum Samboja = new Datum(6125, Ellipsoid.FromEpsgNumber(7004), "Samboja");
        /// <summary>
        /// Lithuania 1994 (ETRS89)
        /// </summary>
        public static readonly Datum Lithuania1994Etrs89 = new Datum(6126, Ellipsoid.FromEpsgNumber(7019), "Lithuania 1994 (ETRS89)");
        /// <summary>
        /// Tete
        /// </summary>
        public static readonly Datum Tete = new Datum(6127, Ellipsoid.FromEpsgNumber(7008), "Tete");
        /// <summary>
        /// Madzansua
        /// </summary>
        public static readonly Datum Madzansua = new Datum(6128, Ellipsoid.FromEpsgNumber(7008), "Madzansua");
        /// <summary>
        /// Observatario
        /// </summary>
        public static readonly Datum Observatario = new Datum(6129, Ellipsoid.FromEpsgNumber(7008), "Observatario");
        /// <summary>
        /// Moznet (ITRF94)
        /// </summary>
        public static readonly Datum MoznetItrf94 = new Datum(6130, Ellipsoid.FromEpsgNumber(7030), "Moznet (ITRF94)");
        /// <summary>
        /// Indian 1960
        /// </summary>
        public static readonly Datum Indian1960 = new Datum(6131, Ellipsoid.FromEpsgNumber(7015), "Indian 1960");
        /// <summary>
        /// Represents the Indian datum.
        /// </summary>
        public static readonly Datum Indian = Indian1960;
        /// <summary>
        /// Final Datum 1958
        /// </summary>
        public static readonly Datum FinalDatum1958 = new Datum(6132, Ellipsoid.FromEpsgNumber(7012), "Final Datum 1958");
        /// <summary>
        /// Estonia 1992
        /// </summary>
        public static readonly Datum Estonia1992 = new Datum(6133, Ellipsoid.FromEpsgNumber(7019), "Estonia 1992");
        /// <summary>
        /// PDO Survey Datum 1993
        /// </summary>
        public static readonly Datum PdoSurveyDatum1993 = new Datum(6134, Ellipsoid.FromEpsgNumber(7012), "PDO Survey Datum 1993");
        /// <summary>
        /// Old Hawaiian
        /// </summary>
        public static readonly Datum OldHawaiian = new Datum(6135, Ellipsoid.FromEpsgNumber(7008), "Old Hawaiian");
        /// <summary>
        /// St. Lawrence Island
        /// </summary>
        public static readonly Datum StLawrenceIsland = new Datum(6136, Ellipsoid.FromEpsgNumber(7008), "St. Lawrence Island");
        /// <summary>
        /// St. Paul Island
        /// </summary>
        public static readonly Datum StPaulIsland = new Datum(6137, Ellipsoid.FromEpsgNumber(7008), "St. Paul Island");
        /// <summary>
        /// St. George Island
        /// </summary>
        public static readonly Datum StGeorgeIsland = new Datum(6138, Ellipsoid.FromEpsgNumber(7008), "St. George Island");
        /// <summary>
        /// Puerto Rico
        /// </summary>
        public static readonly Datum PuertoRico = new Datum(6139, Ellipsoid.FromEpsgNumber(7008), "Puerto Rico");
        /// <summary>
        /// NAD83 Canadian Spatial Reference System
        /// </summary>
        public static readonly Datum Nad83CanadianSpatialReferenceSystem = new Datum(6140, Ellipsoid.FromEpsgNumber(7019), "NAD83 Canadian Spatial Reference System");
        /// <summary>
        /// Israel
        /// </summary>
        public static readonly Datum Israel = new Datum(6141, Ellipsoid.FromEpsgNumber(7019), "Israel");
        /// <summary>
        /// Locodjo 1965
        /// </summary>
        public static readonly Datum Locodjo1965 = new Datum(6142, Ellipsoid.FromEpsgNumber(7012), "Locodjo 1965");
        /// <summary>
        /// Abidjan 1987
        /// </summary>
        public static readonly Datum Abidjan1987 = new Datum(6143, Ellipsoid.FromEpsgNumber(7012), "Abidjan 1987");
        /// <summary>
        /// Kalianpur 1937
        /// </summary>
        public static readonly Datum Kalianpur1937 = new Datum(6144, Ellipsoid.FromEpsgNumber(7015), "Kalianpur 1937");
        /// <summary>
        /// Kalianpur 1962
        /// </summary>
        public static readonly Datum Kalianpur1962 = new Datum(6145, Ellipsoid.FromEpsgNumber(7044), "Kalianpur 1962");
        /// <summary>
        /// Kalianpur 1975
        /// </summary>
        public static readonly Datum Kalianpur1975 = new Datum(6146, Ellipsoid.FromEpsgNumber(7045), "Kalianpur 1975");
        /// <summary>
        /// Hanoi 1972
        /// </summary>
        public static readonly Datum Hanoi1972 = new Datum(6147, Ellipsoid.FromEpsgNumber(7024), "Hanoi 1972");
        /// <summary>
        /// Hartebeesthoek94
        /// </summary>
        public static readonly Datum Hartebeesthoek94 = new Datum(6148, Ellipsoid.FromEpsgNumber(7030), "Hartebeesthoek94");
        /// <summary>
        /// CH1903
        /// </summary>
        public static readonly Datum Ch1903 = new Datum(6149, Ellipsoid.FromEpsgNumber(7004), "CH1903");
        /// <summary>
        /// CH1903+
        /// </summary>
        public static readonly Datum Ch1903Plus = new Datum(6150, Ellipsoid.FromEpsgNumber(7004), "CH1903+");
        /// <summary>
        /// Swiss Terrestrial Reference Frame 1995
        /// </summary>
        public static readonly Datum SwissTerrestrialReferenceFrame1995 = new Datum(6151, Ellipsoid.FromEpsgNumber(7019), "Swiss Terrestrial Reference Frame 1995");
        /// <summary>
        /// NAD83 (High Accuracy Regional Network)
        /// </summary>
        public static readonly Datum Nad83HighAccuracyRegionalNetwork = new Datum(6152, Ellipsoid.FromEpsgNumber(7019), "NAD83 (High Accuracy Regional Network)");
        /// <summary>
        /// Rassadiran
        /// </summary>
        public static readonly Datum Rassadiran = new Datum(6153, Ellipsoid.FromEpsgNumber(7022), "Rassadiran");
        /// <summary>
        /// European Datum 1950(1977)
        /// </summary>
        public static readonly Datum EuropeanDatum1977 = new Datum(6154, Ellipsoid.FromEpsgNumber(7022), "European Datum 1950(1977)");
        /// <summary>
        /// Dabola 1981
        /// </summary>
        public static readonly Datum Dabola1981 = new Datum(6155, Ellipsoid.FromEpsgNumber(7011), "Dabola 1981");
        /// <summary>
        /// Jednotne Trigonometricke Site Katastralni
        /// </summary>
        public static readonly Datum JednotneTrigonometrickeSiteKatastralni = new Datum(6156, Ellipsoid.FromEpsgNumber(7004), "Jednotne Trigonometricke Site Katastralni");
        /// <summary>
        /// Mount Dillon
        /// </summary>
        public static readonly Datum MountDillon = new Datum(6157, Ellipsoid.FromEpsgNumber(7007), "Mount Dillon");
        /// <summary>
        /// Naparima 1955
        /// </summary>
        public static readonly Datum Naparima1955 = new Datum(6158, Ellipsoid.FromEpsgNumber(7022), "Naparima 1955");
        /// <summary>
        /// European Libyan Datum 1979
        /// </summary>
        public static readonly Datum EuropeanLibyanDatum1979 = new Datum(6159, Ellipsoid.FromEpsgNumber(7022), "European Libyan Datum 1979");
        /// <summary>
        /// Chos Malal 1914
        /// </summary>
        public static readonly Datum ChosMalal1914 = new Datum(6160, Ellipsoid.FromEpsgNumber(7022), "Chos Malal 1914");
        /// <summary>
        /// Pampa del Castillo
        /// </summary>
        public static readonly Datum PampadelCastillo = new Datum(6161, Ellipsoid.FromEpsgNumber(7022), "Pampa del Castillo");
        /// <summary>
        /// Korean Datum 1985
        /// </summary>
        public static readonly Datum KoreanDatum1985 = new Datum(6162, Ellipsoid.FromEpsgNumber(7004), "Korean Datum 1985");
        /// <summary>
        /// Yemen National Geodetic Network 1996
        /// </summary>
        public static readonly Datum YemenNationalGeodeticNetwork1996 = new Datum(6163, Ellipsoid.FromEpsgNumber(7030), "Yemen National Geodetic Network 1996");
        /// <summary>
        /// South Yemen
        /// </summary>
        public static readonly Datum SouthYemen = new Datum(6164, Ellipsoid.FromEpsgNumber(7024), "South Yemen");
        /// <summary>
        /// Bissau
        /// </summary>
        public static readonly Datum Bissau = new Datum(6165, Ellipsoid.FromEpsgNumber(7022), "Bissau");
        /// <summary>
        /// Korean Datum 1995
        /// </summary>
        public static readonly Datum KoreanDatum1995 = new Datum(6166, Ellipsoid.FromEpsgNumber(7030), "Korean Datum 1995");
        /// <summary>
        /// New Zealand Geodetic Datum 2000
        /// </summary>
        public static readonly Datum NewZealandGeodeticDatum2000 = new Datum(6167, Ellipsoid.FromEpsgNumber(7019), "New Zealand Geodetic Datum 2000");
        /// <summary>
        /// Accra
        /// </summary>
        public static readonly Datum Accra = new Datum(6168, Ellipsoid.FromEpsgNumber(7029), "Accra");
        /// <summary>
        /// American Samoa 1962
        /// </summary>
        public static readonly Datum AmericanSamoa1962 = new Datum(6169, Ellipsoid.FromEpsgNumber(7008), "American Samoa 1962");
        /// <summary>
        /// Sistema de Referencia Geocentrico para America del Sur 1995
        /// </summary>
        public static readonly Datum SistemadeReferenciaGeocentricoparaAmericadelSur1995 = new Datum(6170, Ellipsoid.FromEpsgNumber(7019), "Sistema de Referencia Geocentrico para America del Sur 1995");
        /// <summary>
        /// Reseau Geodesique Francais 1993
        /// </summary>
        public static readonly Datum ReseauGeodesiqueFrancais1993 = new Datum(6171, Ellipsoid.FromEpsgNumber(7019), "Reseau Geodesique Francais 1993");
        /// <summary>
        /// Posiciones Geodesicas Argentinas
        /// </summary>
        public static readonly Datum PosicionesGeodesicasArgentinas = new Datum(6172, Ellipsoid.FromEpsgNumber(7019), "Posiciones Geodesicas Argentinas");
        /// <summary>
        /// IRENET95
        /// </summary>
        public static readonly Datum Irenet95 = new Datum(6173, Ellipsoid.FromEpsgNumber(7019), "IRENET95");
        /// <summary>
        /// Sierra Leone Colony 1924
        /// </summary>
        public static readonly Datum SierraLeoneColony1924 = new Datum(6174, Ellipsoid.FromEpsgNumber(7029), "Sierra Leone Colony 1924");
        /// <summary>
        /// Sierra Leone 1968
        /// </summary>
        public static readonly Datum SierraLeone1968 = new Datum(6175, Ellipsoid.FromEpsgNumber(7012), "Sierra Leone 1968");
        /// <summary>
        /// Australian Antarctic Datum 1998
        /// </summary>
        public static readonly Datum AustralianAntarcticDatum1998 = new Datum(6176, Ellipsoid.FromEpsgNumber(7019), "Australian Antarctic Datum 1998");
        /// <summary>
        /// Pulkovo 1942/83
        /// </summary>
        public static readonly Datum Pulkovo1983 = new Datum(6178, Ellipsoid.FromEpsgNumber(7024), "Pulkovo 1942/83");
        /// <summary>
        /// Pulkovo 1942/58
        /// </summary>
        public static readonly Datum Pulkovo1958 = new Datum(6179, Ellipsoid.FromEpsgNumber(7024), "Pulkovo 1942/58");
        /// <summary>
        /// Estonia 1997
        /// </summary>
        public static readonly Datum Estonia1997 = new Datum(6180, Ellipsoid.FromEpsgNumber(7019), "Estonia 1997");
        /// <summary>
        /// Luxembourg 1930
        /// </summary>
        public static readonly Datum Luxembourg1930 = new Datum(6181, Ellipsoid.FromEpsgNumber(7022), "Luxembourg 1930");
        /// <summary>
        /// Azores Occidental Islands 1939
        /// </summary>
        public static readonly Datum AzoresOccidentalIslands1939 = new Datum(6182, Ellipsoid.FromEpsgNumber(7022), "Azores Occidental Islands 1939");
        /// <summary>
        /// Represents the Observatorio Meteorologico datum of 1939.
        /// </summary>
        public static readonly Datum ObservatorioMeteorologico1939 = AzoresOccidentalIslands1939;
        /// <summary>
        /// Azores Central Islands 1948
        /// </summary>
        public static readonly Datum AzoresCentralIslands1948 = new Datum(6183, Ellipsoid.FromEpsgNumber(7022), "Azores Central Islands 1948");
        /// <summary>
        /// Represents the Graciosa Base SW datum of 1948.
        /// </summary>
        public static readonly Datum GraciosaBaseSw1948 = AzoresCentralIslands1948;
        /// <summary>
        /// Azores Oriental Islands 1940
        /// </summary>
        public static readonly Datum AzoresOrientalIslands1940 = new Datum(6184, Ellipsoid.FromEpsgNumber(7022), "Azores Oriental Islands 1940");
        /// <summary>
        /// Madeira 1936
        /// </summary>
        public static readonly Datum Madeira1936 = new Datum(6185, Ellipsoid.FromEpsgNumber(7022), "Madeira 1936");
        /// <summary>
        /// OSNI 1952
        /// </summary>
        public static readonly Datum Osni1952 = new Datum(6188, Ellipsoid.FromEpsgNumber(7001), "OSNI 1952");
        /// <summary>
        /// Red Geodesica Venezolana
        /// </summary>
        public static readonly Datum RedGeodesicaVenezolana = new Datum(6189, Ellipsoid.FromEpsgNumber(7019), "Red Geodesica Venezolana");
        /// <summary>
        /// Posiciones Geodesicas Argentinas 1998
        /// </summary>
        public static readonly Datum PosicionesGeodesicasArgentinas1998 = new Datum(6190, Ellipsoid.FromEpsgNumber(7019), "Posiciones Geodesicas Argentinas 1998");
        /// <summary>
        /// Albanian 1987
        /// </summary>
        public static readonly Datum Albanian1987 = new Datum(6191, Ellipsoid.FromEpsgNumber(7024), "Albanian 1987");
        /// <summary>
        /// Douala 1948
        /// </summary>
        public static readonly Datum Douala1948 = new Datum(6192, Ellipsoid.FromEpsgNumber(7022), "Douala 1948");
        /// <summary>
        /// Manoca 1962
        /// </summary>
        public static readonly Datum Manoca1962 = new Datum(6193, Ellipsoid.FromEpsgNumber(7011), "Manoca 1962");
        /// <summary>
        /// Qornoq 1927
        /// </summary>
        public static readonly Datum Qornoq1927 = new Datum(6194, Ellipsoid.FromEpsgNumber(7022), "Qornoq 1927");
        /// <summary>
        /// Scoresbysund 1952
        /// </summary>
        public static readonly Datum Scoresbysund1952 = new Datum(6195, Ellipsoid.FromEpsgNumber(7022), "Scoresbysund 1952");
        /// <summary>
        /// Ammassalik 1958
        /// </summary>
        public static readonly Datum Ammassalik1958 = new Datum(6196, Ellipsoid.FromEpsgNumber(7022), "Ammassalik 1958");
        /// <summary>
        /// Garoua
        /// </summary>
        public static readonly Datum GarouaRGS = new Datum(6197, Ellipsoid.FromEpsgNumber(7012), "Garoua RGS");
        /// <summary>
        /// Kousseri
        /// </summary>
        public static readonly Datum Kousseri = new Datum(6198, Ellipsoid.FromEpsgNumber(7012), "Kousseri");
        /// <summary>
        /// Egypt 1930
        /// </summary>
        public static readonly Datum Egypt1930 = new Datum(6199, Ellipsoid.FromEpsgNumber(7022), "Egypt 1930");
        /// <summary>
        /// Pulkovo 1995
        /// </summary>
        public static readonly Datum Pulkovo1995 = new Datum(6200, Ellipsoid.FromEpsgNumber(7024), "Pulkovo 1995");
        /// <summary>
        /// Adindan
        /// </summary>
        public static readonly Datum Adindan = new Datum(6201, Ellipsoid.FromEpsgNumber(7012), "Adindan");
        /// <summary>
        /// Australian Geodetic Datum 1966
        /// </summary>
        public static readonly Datum AustralianGeodeticDatum1966 = new Datum(6202, Ellipsoid.FromEpsgNumber(7003), "Australian Geodetic Datum 1966");
        /// <summary>
        /// Australian Geodetic Datum 1984
        /// </summary>
        public static readonly Datum AustralianGeodeticDatum1984 = new Datum(6203, Ellipsoid.FromEpsgNumber(7003), "Australian Geodetic Datum 1984");
        /// <summary>
        /// Ain el Abd 1970
        /// </summary>
        public static readonly Datum AinelAbd1970 = new Datum(6204, Ellipsoid.FromEpsgNumber(7022), "Ain el Abd 1970");
        /// <summary>
        /// Afgooye
        /// </summary>
        public static readonly Datum Afgooye = new Datum(6205, Ellipsoid.FromEpsgNumber(7024), "Afgooye");
        /// <summary>
        /// Agadez
        /// </summary>
        public static readonly Datum Agadez = new Datum(6206, Ellipsoid.FromEpsgNumber(7011), "Agadez");
        /// <summary>
        /// Lisbon 1937
        /// </summary>
        public static readonly Datum Lisbon1937 = new Datum(6207, Ellipsoid.FromEpsgNumber(7022), "Lisbon 1937");
        /// <summary>
        /// Aratu
        /// </summary>
        public static readonly Datum Aratu = new Datum(6208, Ellipsoid.FromEpsgNumber(7022), "Aratu");
        /// <summary>
        /// Arc 1950
        /// </summary>
        public static readonly Datum Arc1950 = new Datum(6209, Ellipsoid.FromEpsgNumber(7013), "Arc 1950");
        /// <summary>
        /// Arc 1960
        /// </summary>
        public static readonly Datum Arc1960 = new Datum(6210, Ellipsoid.FromEpsgNumber(7012), "Arc 1960");
        /// <summary>
        /// Batavia
        /// </summary>
        public static readonly Datum Batavia = new Datum(6211, Ellipsoid.FromEpsgNumber(7004), "Batavia");
        /// <summary>
        /// Barbados 1938
        /// </summary>
        public static readonly Datum Barbados1938 = new Datum(6212, Ellipsoid.FromEpsgNumber(7012), "Barbados 1938");
        /// <summary>
        /// Beduaram
        /// </summary>
        public static readonly Datum Beduaram = new Datum(6213, Ellipsoid.FromEpsgNumber(7011), "Beduaram");
        /// <summary>
        /// Beijing 1954
        /// </summary>
        public static readonly Datum Beijing1954 = new Datum(6214, Ellipsoid.FromEpsgNumber(7024), "Beijing 1954");
        /// <summary>
        /// Reseau National Belge 1950
        /// </summary>
        public static readonly Datum ReseauNationalBelge1950 = new Datum(6215, Ellipsoid.FromEpsgNumber(7022), "Reseau National Belge 1950");
        /// <summary>
        /// Bermuda 1957
        /// </summary>
        public static readonly Datum Bermuda1957 = new Datum(6216, Ellipsoid.FromEpsgNumber(7008), "Bermuda 1957");
        /// <summary>
        /// Bogota 1975
        /// </summary>
        public static readonly Datum Bogota1975 = new Datum(6218, Ellipsoid.FromEpsgNumber(7022), "Bogota 1975");
        /// <summary>
        /// Bukit Rimpah
        /// </summary>
        public static readonly Datum BukitRimpah = new Datum(6219, Ellipsoid.FromEpsgNumber(7004), "Bukit Rimpah");
        /// <summary>
        /// Camacupa
        /// </summary>
        public static readonly Datum Camacupa = new Datum(6220, Ellipsoid.FromEpsgNumber(7012), "Camacupa");
        /// <summary>
        /// Campo Inchauspe
        /// </summary>
        public static readonly Datum CampoInchauspe = new Datum(6221, Ellipsoid.FromEpsgNumber(7022), "Campo Inchauspe");
        /// <summary>
        /// Cape
        /// </summary>
        public static readonly Datum Cape = new Datum(6222, Ellipsoid.FromEpsgNumber(7013), "Cape");
        /// <summary>
        /// Carthage
        /// </summary>
        public static readonly Datum Carthage = new Datum(6223, Ellipsoid.FromEpsgNumber(7011), "Carthage");
        /// <summary>
        /// Chua
        /// </summary>
        public static readonly Datum Chua = new Datum(6224, Ellipsoid.FromEpsgNumber(7022), "Chua");
        /// <summary>
        /// Corrego Alegre
        /// </summary>
        public static readonly Datum CorregoAlegre = new Datum(6225, Ellipsoid.FromEpsgNumber(7022), "Corrego Alegre");
        /// <summary>
        /// Cote d'Ivoire
        /// </summary>
        public static readonly Datum CotedIvoire = new Datum(6226, Ellipsoid.FromEpsgNumber(7011), "Cote d'Ivoire");
        /// <summary>
        /// Deir ez Zor
        /// </summary>
        public static readonly Datum DeirezZor = new Datum(6227, Ellipsoid.FromEpsgNumber(7011), "Deir ez Zor");
        /// <summary>
        /// Douala
        /// </summary>
        public static readonly Datum Douala = new Datum(6228, Ellipsoid.FromEpsgNumber(7011), "Douala");
        /// <summary>
        /// Egypt 1907
        /// </summary>
        public static readonly Datum Egypt1907 = new Datum(6229, Ellipsoid.FromEpsgNumber(7020), "Egypt 1907");
        /// <summary>
        /// European Datum 1950
        /// </summary>
        public static readonly Datum EuropeanDatum1950 = new Datum(6230, Ellipsoid.FromEpsgNumber(7022), "European Datum 1950");
        /// <summary>
        /// European Datum 1987
        /// </summary>
        public static readonly Datum EuropeanDatum1987 = new Datum(6231, Ellipsoid.FromEpsgNumber(7022), "European Datum 1987");
        /// <summary>
        /// Fahud
        /// </summary>
        public static readonly Datum Fahud = new Datum(6232, Ellipsoid.FromEpsgNumber(7012), "Fahud");
        /// <summary>
        /// Gandajika 1970
        /// </summary>
        public static readonly Datum Gandajika1970 = new Datum(6233, Ellipsoid.FromEpsgNumber(7022), "Gandajika 1970");
        /// <summary>
        /// Garoua
        /// </summary>
        public static readonly Datum GarouaIGN = new Datum(6234, Ellipsoid.FromEpsgNumber(7011), "Garoua IGN");
        /// <summary>
        /// Guyane Francaise
        /// </summary>
        public static readonly Datum GuyaneFrancaise = new Datum(6235, Ellipsoid.FromEpsgNumber(7022), "Guyane Francaise");
        /// <summary>
        /// Hu Tzu Shan
        /// </summary>
        public static readonly Datum HuTzuShan = new Datum(6236, Ellipsoid.FromEpsgNumber(7022), "Hu Tzu Shan");
        /// <summary>
        /// Hungarian Datum 1972
        /// </summary>
        public static readonly Datum HungarianDatum1972 = new Datum(6237, Ellipsoid.FromEpsgNumber(7036), "Hungarian Datum 1972");
        /// <summary>
        /// Indonesian Datum 1974
        /// </summary>
        public static readonly Datum IndonesianDatum1974 = new Datum(6238, Ellipsoid.FromEpsgNumber(7021), "Indonesian Datum 1974");
        /// <summary>
        /// Indian 1954
        /// </summary>
        public static readonly Datum Indian1954 = new Datum(6239, Ellipsoid.FromEpsgNumber(7015), "Indian 1954");
        /// <summary>
        /// Indian 1975
        /// </summary>
        public static readonly Datum Indian1975 = new Datum(6240, Ellipsoid.FromEpsgNumber(7015), "Indian 1975");
        /// <summary>
        /// Jamaica 1875
        /// </summary>
        public static readonly Datum Jamaica1875 = new Datum(6241, Ellipsoid.FromEpsgNumber(7034), "Jamaica 1875");
        /// <summary>
        /// Jamaica 1969
        /// </summary>
        public static readonly Datum Jamaica1969 = new Datum(6242, Ellipsoid.FromEpsgNumber(7008), "Jamaica 1969");
        /// <summary>
        /// Kalianpur 1880
        /// </summary>
        public static readonly Datum Kalianpur1880 = new Datum(6243, Ellipsoid.FromEpsgNumber(7042), "Kalianpur 1880");
        /// <summary>
        /// Kandawala
        /// </summary>
        public static readonly Datum Kandawala = new Datum(6244, Ellipsoid.FromEpsgNumber(7015), "Kandawala");
        /// <summary>
        /// Kertau 1968
        /// </summary>
        public static readonly Datum Kertau1968 = new Datum(6245, Ellipsoid.FromEpsgNumber(7018), "Kertau 1968");
        /// <summary>
        /// Kuwait Oil Company
        /// </summary>
        public static readonly Datum KuwaitOilCompany = new Datum(6246, Ellipsoid.FromEpsgNumber(7012), "Kuwait Oil Company");
        /// <summary>
        /// La Canoa
        /// </summary>
        public static readonly Datum LaCanoa = new Datum(6247, Ellipsoid.FromEpsgNumber(7022), "La Canoa");
        /// <summary>
        /// Provisional South American Datum 1956
        /// </summary>
        public static readonly Datum ProvisionalSouthAmericanDatum1956 = new Datum(6248, Ellipsoid.FromEpsgNumber(7022), "Provisional South American Datum 1956");
        /// <summary>
        /// Lake
        /// </summary>
        public static readonly Datum Lake = new Datum(6249, Ellipsoid.FromEpsgNumber(7022), "Lake");
        /// <summary>
        /// Leigon
        /// </summary>
        public static readonly Datum Leigon = new Datum(6250, Ellipsoid.FromEpsgNumber(7012), "Leigon");
        /// <summary>
        /// Liberia 1964
        /// </summary>
        public static readonly Datum Liberia1964 = new Datum(6251, Ellipsoid.FromEpsgNumber(7012), "Liberia 1964");
        /// <summary>
        /// Lome
        /// </summary>
        public static readonly Datum Lome = new Datum(6252, Ellipsoid.FromEpsgNumber(7011), "Lome");
        /// <summary>
        /// Luzon 1911
        /// </summary>
        public static readonly Datum Luzon1911 = new Datum(6253, Ellipsoid.FromEpsgNumber(7008), "Luzon 1911");
        /// <summary>
        /// Hito XVIII 1963
        /// </summary>
        public static readonly Datum HitoXviii1963 = new Datum(6254, Ellipsoid.FromEpsgNumber(7022), "Hito XVIII 1963");
        /// <summary>
        /// Represents the Provisional South Chilean datum of 1963.
        /// </summary>
        public static readonly Datum ProvisionalSouthChilean1963 = HitoXviii1963;
        /// <summary>
        /// Herat North
        /// </summary>
        public static readonly Datum HeratNorth = new Datum(6255, Ellipsoid.FromEpsgNumber(7022), "Herat North");
        /// <summary>
        /// Mahe 1971
        /// </summary>
        public static readonly Datum Mahe1971 = new Datum(6256, Ellipsoid.FromEpsgNumber(7012), "Mahe 1971");
        /// <summary>
        /// Makassar
        /// </summary>
        public static readonly Datum Makassar = new Datum(6257, Ellipsoid.FromEpsgNumber(7004), "Makassar");
        /// <summary>
        /// European Terrestrial Reference System 1989
        /// </summary>
        public static readonly Datum EuropeanTerrestrialReferenceSystem1989 = new Datum(6258, Ellipsoid.FromEpsgNumber(7019), "European Terrestrial Reference System 1989");
        /// <summary>
        /// Malongo 1987
        /// </summary>
        public static readonly Datum Malongo1987 = new Datum(6259, Ellipsoid.FromEpsgNumber(7022), "Malongo 1987");
        /// <summary>
        /// Manoca
        /// </summary>
        public static readonly Datum Manoca = new Datum(6260, Ellipsoid.FromEpsgNumber(7012), "Manoca");
        /// <summary>
        /// Merchich
        /// </summary>
        public static readonly Datum Merchich = new Datum(6261, Ellipsoid.FromEpsgNumber(7011), "Merchich");
        /// <summary>
        /// Massawa
        /// </summary>
        public static readonly Datum Massawa = new Datum(6262, Ellipsoid.FromEpsgNumber(7004), "Massawa");
        /// <summary>
        /// Minna
        /// </summary>
        public static readonly Datum Minna = new Datum(6263, Ellipsoid.FromEpsgNumber(7012), "Minna");
        /// <summary>
        /// Mhast
        /// </summary>
        public static readonly Datum Mhast = new Datum(6264, Ellipsoid.FromEpsgNumber(7022), "Mhast");
        /// <summary>
        /// Monte Mario
        /// </summary>
        public static readonly Datum MonteMario = new Datum(6265, Ellipsoid.FromEpsgNumber(7022), "Monte Mario");
        /// <summary>
        /// M'poraloko
        /// </summary>
        public static readonly Datum Mporaloko = new Datum(6266, Ellipsoid.FromEpsgNumber(7011), "M'poraloko");
        /// <summary>
        /// North American Datum 1927
        /// </summary>
        public static readonly Datum NorthAmericanDatum1927 = new Datum(6267, Ellipsoid.FromEpsgNumber(7008), "North American Datum 1927");
        /// <summary>
        /// NAD Michigan
        /// </summary>
        public static readonly Datum NadMichigan = new Datum(6268, Ellipsoid.FromEpsgNumber(7009), "NAD Michigan");
        /// <summary>
        /// North American Datum 1983
        /// </summary>
        public static readonly Datum NorthAmericanDatum1983 = new Datum(6269, Ellipsoid.FromEpsgNumber(7019), "North American Datum 1983");
        /// <summary>
        /// Nahrwan 1967
        /// </summary>
        public static readonly Datum Nahrwan1967 = new Datum(6270, Ellipsoid.FromEpsgNumber(7012), "Nahrwan 1967");
        /// <summary>
        /// Naparima 1972
        /// </summary>
        public static readonly Datum Naparima1972 = new Datum(6271, Ellipsoid.FromEpsgNumber(7022), "Naparima 1972");
        /// <summary>
        /// New Zealand Geodetic Datum 1949
        /// </summary>
        public static readonly Datum NewZealandGeodeticDatum1949 = new Datum(6272, Ellipsoid.FromEpsgNumber(7022), "New Zealand Geodetic Datum 1949");
        /// <summary>
        /// Represents the Geodetic Datum of 1949.
        /// </summary>
        public static readonly Datum GeodeticDatum1949 = NewZealandGeodeticDatum1949;
        /// <summary>
        /// NGO 1948
        /// </summary>
        public static readonly Datum Ngo1948 = new Datum(6273, Ellipsoid.FromEpsgNumber(7005), "NGO 1948");
        /// <summary>
        /// Datum 73
        /// </summary>
        public static readonly Datum Datum73 = new Datum(6274, Ellipsoid.FromEpsgNumber(7022), "Datum 73");
        /// <summary>
        /// Nouvelle Triangulation Francaise
        /// </summary>
        public static readonly Datum NouvelleTriangulationFrancaise = new Datum(6275, Ellipsoid.FromEpsgNumber(7011), "Nouvelle Triangulation Francaise");
        /// <summary>
        /// NSWC 9Z-2
        /// </summary>
        public static readonly Datum Nswc9Z2 = new Datum(6276, Ellipsoid.FromEpsgNumber(7025), "NSWC 9Z-2");
        /// <summary>
        /// OSGB 1936
        /// </summary>
        public static readonly Datum Osgb1936 = new Datum(6277, Ellipsoid.FromEpsgNumber(7001), "OSGB 1936");
        /// <summary>
        /// Represents the Ordnance Survey of Great Britain datum of 1936.
        /// </summary>
        public static readonly Datum OrdnanceSurveyGreatBritain1936 = Osgb1936;
        /// <summary>
        /// OSGB 1970 (SN)
        /// </summary>
        public static readonly Datum Osgb1970Sn = new Datum(6278, Ellipsoid.FromEpsgNumber(7001), "OSGB 1970 (SN)");
        /// <summary>
        /// OS (SN) 1980
        /// </summary>
        public static readonly Datum OsSn1980 = new Datum(6279, Ellipsoid.FromEpsgNumber(7001), "OS (SN) 1980");
        /// <summary>
        /// Padang 1884
        /// </summary>
        public static readonly Datum Padang1884 = new Datum(6280, Ellipsoid.FromEpsgNumber(7004), "Padang 1884");
        /// <summary>
        /// Palestine 1923
        /// </summary>
        public static readonly Datum Palestine1923 = new Datum(6281, Ellipsoid.FromEpsgNumber(7010), "Palestine 1923");
        /// <summary>
        /// Congo 1960 Pointe Noire
        /// </summary>
        public static readonly Datum Congo1960PointeNoire = new Datum(6282, Ellipsoid.FromEpsgNumber(7011), "Congo 1960 Pointe Noire");
        /// <summary>
        /// Geocentric Datum of Australia 1994
        /// </summary>
        public static readonly Datum GeocentricDatumofAustralia1994 = new Datum(6283, Ellipsoid.FromEpsgNumber(7019), "Geocentric Datum of Australia 1994");
        /// <summary>
        /// Pulkovo 1942
        /// </summary>
        public static readonly Datum Pulkovo1942 = new Datum(6284, Ellipsoid.FromEpsgNumber(7024), "Pulkovo 1942");
        /// <summary>
        /// Qatar 1974
        /// </summary>
        public static readonly Datum Qatar1974 = new Datum(6285, Ellipsoid.FromEpsgNumber(7022), "Qatar 1974");
        /// <summary>
        /// Qatar 1948
        /// </summary>
        public static readonly Datum Qatar1948 = new Datum(6286, Ellipsoid.FromEpsgNumber(7020), "Qatar 1948");
        /// <summary>
        /// Qornoq
        /// </summary>
        public static readonly Datum Qornoq = new Datum(6287, Ellipsoid.FromEpsgNumber(7022), "Qornoq");
        /// <summary>
        /// Loma Quintana
        /// </summary>
        public static readonly Datum LomaQuintana = new Datum(6288, Ellipsoid.FromEpsgNumber(7022), "Loma Quintana");
        /// <summary>
        /// Amersfoort
        /// </summary>
        public static readonly Datum Amersfoort = new Datum(6289, Ellipsoid.FromEpsgNumber(7004), "Amersfoort");
        /// <summary>
        /// South American Datum 1969
        /// </summary>
        public static readonly Datum SouthAmericanDatum1969 = new Datum(6291, Ellipsoid.FromEpsgNumber(7036), "South American Datum 1969");
        /// <summary>
        /// Sapper Hill 1943
        /// </summary>
        public static readonly Datum SapperHill1943 = new Datum(6292, Ellipsoid.FromEpsgNumber(7022), "Sapper Hill 1943");
        /// <summary>
        /// Schwarzeck
        /// </summary>
        public static readonly Datum Schwarzeck = new Datum(6293, Ellipsoid.FromEpsgNumber(7046), "Schwarzeck");
        /// <summary>
        /// Segora
        /// </summary>
        public static readonly Datum Segora = new Datum(6294, Ellipsoid.FromEpsgNumber(7004), "Segora");
        /// <summary>
        /// Serindung
        /// </summary>
        public static readonly Datum Serindung = new Datum(6295, Ellipsoid.FromEpsgNumber(7004), "Serindung");
        /// <summary>
        /// Sudan
        /// </summary>
        public static readonly Datum Sudan = new Datum(6296, Ellipsoid.FromEpsgNumber(7011), "Sudan");
        /// <summary>
        /// Tananarive 1925
        /// </summary>
        public static readonly Datum Tananarive1925 = new Datum(6297, Ellipsoid.FromEpsgNumber(7022), "Tananarive 1925");
        /// <summary>
        /// Timbalai 1948
        /// </summary>
        public static readonly Datum Timbalai1948 = new Datum(6298, Ellipsoid.FromEpsgNumber(7016), "Timbalai 1948");
        /// <summary>
        /// TM65
        /// </summary>
        public static readonly Datum Tm65 = new Datum(6299, Ellipsoid.FromEpsgNumber(7002), "TM65");
        /// <summary>
        /// Represents the Ireland datum of 1965.
        /// </summary>
        public static readonly Datum Ireland1965 = Tm65;
        /// <summary>
        /// Geodetic Datum of 1965
        /// </summary>
        public static readonly Datum GeodeticDatumof1965 = new Datum(6300, Ellipsoid.FromEpsgNumber(7002), "Geodetic Datum of 1965");
        /// <summary>
        /// Tokyo
        /// </summary>
        public static readonly Datum Tokyo = new Datum(6301, Ellipsoid.FromEpsgNumber(7004), "Tokyo");
        /// <summary>
        /// Trinidad 1903
        /// </summary>
        public static readonly Datum Trinidad1903 = new Datum(6302, Ellipsoid.FromEpsgNumber(7007), "Trinidad 1903");
        /// <summary>
        /// Trucial Coast 1948
        /// </summary>
        public static readonly Datum TrucialCoast1948 = new Datum(6303, Ellipsoid.FromEpsgNumber(7020), "Trucial Coast 1948");
        /// <summary>
        /// Voirol 1875
        /// </summary>
        public static readonly Datum Voirol1875 = new Datum(6304, Ellipsoid.FromEpsgNumber(7011), "Voirol 1875");
        /// <summary>
        /// Bern 1938
        /// </summary>
        public static readonly Datum Bern1938 = new Datum(6306, Ellipsoid.FromEpsgNumber(7004), "Bern 1938");
        /// <summary>
        /// Nord Sahara 1959
        /// </summary>
        public static readonly Datum NordSahara1959 = new Datum(6307, Ellipsoid.FromEpsgNumber(7012), "Nord Sahara 1959");
        /// <summary>
        /// Stockholm 1938
        /// </summary>
        public static readonly Datum Stockholm1938 = new Datum(6308, Ellipsoid.FromEpsgNumber(7004), "Stockholm 1938");
        /// <summary>
        /// Yacare
        /// </summary>
        public static readonly Datum Yacare = new Datum(6309, Ellipsoid.FromEpsgNumber(7022), "Yacare");
        /// <summary>
        /// Yoff
        /// </summary>
        public static readonly Datum Yoff = new Datum(6310, Ellipsoid.FromEpsgNumber(7011), "Yoff");
        /// <summary>
        /// Zanderij
        /// </summary>
        public static readonly Datum Zanderij = new Datum(6311, Ellipsoid.FromEpsgNumber(7022), "Zanderij");
        /// <summary>
        /// Militar-Geographische Institut
        /// </summary>
        public static readonly Datum MilitarGeographischeInstitut = new Datum(6312, Ellipsoid.FromEpsgNumber(7004), "Militar-Geographische Institut");
        /// <summary>
        /// Represents the Hermannskogel datum.
        /// </summary>
        public static readonly Datum HermannskogelDatum = MilitarGeographischeInstitut;
        /// <summary>
        /// Reseau National Belge 1972
        /// </summary>
        public static readonly Datum ReseauNationalBelge1972 = new Datum(6313, Ellipsoid.FromEpsgNumber(7022), "Reseau National Belge 1972");
        /// <summary>
        /// Deutsches Hauptdreiecksnetz
        /// </summary>
        public static readonly Datum DeutschesHauptdreiecksnetz = new Datum(6314, Ellipsoid.FromEpsgNumber(7004), "Deutsches Hauptdreiecksnetz");
        /// <summary>
        /// Conakry 1905
        /// </summary>
        public static readonly Datum Conakry1905 = new Datum(6315, Ellipsoid.FromEpsgNumber(7011), "Conakry 1905");
        /// <summary>
        /// Dealul Piscului 1933
        /// </summary>
        public static readonly Datum DealulPiscului1933 = new Datum(6316, Ellipsoid.FromEpsgNumber(7022), "Dealul Piscului 1933");
        /// <summary>
        /// Dealul Piscului 1970
        /// </summary>
        public static readonly Datum DealulPiscului1970 = new Datum(6317, Ellipsoid.FromEpsgNumber(7024), "Dealul Piscului 1970");
        /// <summary>
        /// National Geodetic Network
        /// </summary>
        public static readonly Datum NationalGeodeticNetwork = new Datum(6318, Ellipsoid.FromEpsgNumber(7030), "National Geodetic Network");
        /// <summary>
        /// Kuwait Utility
        /// </summary>
        public static readonly Datum KuwaitUtility = new Datum(6319, Ellipsoid.FromEpsgNumber(7019), "Kuwait Utility");
        /// <summary>
        /// World Geodetic System 1972
        /// </summary>
        public static readonly Datum WorldGeodeticSystem1972 = new Datum(6322, Ellipsoid.FromEpsgNumber(7043), "World Geodetic System 1972");
        /// <summary>
        /// WGS 72 Transit Broadcast Ephemeris
        /// </summary>
        public static readonly Datum WGS72TransitBroadcastEphemeris = new Datum(6324, Ellipsoid.FromEpsgNumber(7043), "WGS 72 Transit Broadcast Ephemeris");
        /// <summary>
        /// World Geodetic System 1984
        /// </summary>
        public static readonly Datum WorldGeodeticSystem1984 = new Datum(6326, Ellipsoid.FromEpsgNumber(7030), "World Geodetic System 1984");
        /// <summary>
        /// Anguilla 1957
        /// </summary>
        public static readonly Datum Anguilla1957 = new Datum(6600, Ellipsoid.FromEpsgNumber(7012), "Anguilla 1957");
        /// <summary>
        /// Antigua 1943
        /// </summary>
        public static readonly Datum Antigua1943 = new Datum(6601, Ellipsoid.FromEpsgNumber(7012), "Antigua 1943");
        /// <summary>
        /// Dominica 1945
        /// </summary>
        public static readonly Datum Dominica1945 = new Datum(6602, Ellipsoid.FromEpsgNumber(7012), "Dominica 1945");
        /// <summary>
        /// Grenada 1953
        /// </summary>
        public static readonly Datum Grenada1953 = new Datum(6603, Ellipsoid.FromEpsgNumber(7012), "Grenada 1953");
        /// <summary>
        /// Montserrat 1958
        /// </summary>
        public static readonly Datum Montserrat1958 = new Datum(6604, Ellipsoid.FromEpsgNumber(7012), "Montserrat 1958");
        /// <summary>
        /// St. Kitts 1955
        /// </summary>
        public static readonly Datum StKitts1955 = new Datum(6605, Ellipsoid.FromEpsgNumber(7012), "St. Kitts 1955");
        /// <summary>
        /// Represents the Fort Thomas datum of 1955.
        /// </summary>
        public static readonly Datum FortThomas1955 = StKitts1955;
        /// <summary>
        /// St. Lucia 1955
        /// </summary>
        public static readonly Datum StLucia1955 = new Datum(6606, Ellipsoid.FromEpsgNumber(7012), "St. Lucia 1955");
        /// <summary>
        /// St. Vincent 1945
        /// </summary>
        public static readonly Datum StVincent1945 = new Datum(6607, Ellipsoid.FromEpsgNumber(7012), "St. Vincent 1945");
        /// <summary>
        /// North American Datum 1927 (1976)
        /// </summary>
        public static readonly Datum NorthAmericanDatum1976 = new Datum(6608, Ellipsoid.FromEpsgNumber(7008), "North American Datum 1927 (1976)");
        /// <summary>
        /// North American Datum 1927 (CGQ77)
        /// </summary>
        public static readonly Datum NorthAmericanDatum1927Cgq77 = new Datum(6609, Ellipsoid.FromEpsgNumber(7008), "North American Datum 1927 (CGQ77)");
        /// <summary>
        /// Xian 1980
        /// </summary>
        public static readonly Datum Xian1980 = new Datum(6610, Ellipsoid.FromEpsgNumber(7049), "Xian 1980");
        /// <summary>
        /// Hong Kong 1980
        /// </summary>
        public static readonly Datum HongKong1980 = new Datum(6611, Ellipsoid.FromEpsgNumber(7022), "Hong Kong 1980");
        /// <summary>
        /// Japanese Geodetic Datum 2000
        /// </summary>
        public static readonly Datum JapaneseGeodeticDatum2000 = new Datum(6612, Ellipsoid.FromEpsgNumber(7019), "Japanese Geodetic Datum 2000");
        /// <summary>
        /// Gunung Segara
        /// </summary>
        public static readonly Datum GunungSegara = new Datum(6613, Ellipsoid.FromEpsgNumber(7004), "Gunung Segara");
        /// <summary>
        /// Qatar National Datum 1995
        /// </summary>
        public static readonly Datum QatarNationalDatum1995 = new Datum(6614, Ellipsoid.FromEpsgNumber(7022), "Qatar National Datum 1995");
        /// <summary>
        /// Porto Santo 1936
        /// </summary>
        public static readonly Datum PortoSanto1936 = new Datum(6615, Ellipsoid.FromEpsgNumber(7022), "Porto Santo 1936");
        /// <summary>
        /// Selvagem Grande
        /// </summary>
        public static readonly Datum SelvagemGrande = new Datum(6616, Ellipsoid.FromEpsgNumber(7022), "Selvagem Grande");
        /// <summary>
        /// South American Datum 1969
        /// </summary>
        public static readonly Datum SouthAmericanDatum1969Sad69 = new Datum(6618, Ellipsoid.FromEpsgNumber(7050), "South American Datum 1969 (SAD69)");
        /// <summary>
        /// SWEREF99
        /// </summary>
        public static readonly Datum Sweref99 = new Datum(6619, Ellipsoid.FromEpsgNumber(7019), "SWEREF99");
        /// <summary>
        /// Point 58
        /// </summary>
        public static readonly Datum Point58 = new Datum(6620, Ellipsoid.FromEpsgNumber(7012), "Point 58");
        /// <summary>
        /// Fort Marigot
        /// </summary>
        public static readonly Datum FortMarigot = new Datum(6621, Ellipsoid.FromEpsgNumber(7022), "Fort Marigot");
        /// <summary>
        /// Guadeloupe 1948
        /// </summary>
        public static readonly Datum Guadeloupe1948 = new Datum(6622, Ellipsoid.FromEpsgNumber(7022), "Guadeloupe 1948");
        /// <summary>
        /// Centre Spatial Guyanais 1967
        /// </summary>
        public static readonly Datum CentreSpatialGuyanais1967 = new Datum(6623, Ellipsoid.FromEpsgNumber(7022), "Centre Spatial Guyanais 1967");
        /// <summary>
        /// Reseau Geodesique Francais Guyane 1995
        /// </summary>
        public static readonly Datum ReseauGeodesiqueFrancaisGuyane1995 = new Datum(6624, Ellipsoid.FromEpsgNumber(7019), "Reseau Geodesique Francais Guyane 1995");
        /// <summary>
        /// Martinique 1938
        /// </summary>
        public static readonly Datum Martinique1938 = new Datum(6625, Ellipsoid.FromEpsgNumber(7022), "Martinique 1938");
        /// <summary>
        /// Reunion 1947
        /// </summary>
        public static readonly Datum Reunion1947 = new Datum(6626, Ellipsoid.FromEpsgNumber(7022), "Reunion 1947");
        /// <summary>
        /// Reseau Geodesique de la Reunion 1992
        /// </summary>
        public static readonly Datum ReseauGeodesiquedelaReunion1992 = new Datum(6627, Ellipsoid.FromEpsgNumber(7019), "Reseau Geodesique de la Reunion 1992");
        /// <summary>
        /// Tahiti 52
        /// </summary>
        public static readonly Datum Tahiti52 = new Datum(6628, Ellipsoid.FromEpsgNumber(7022), "Tahiti 52");
        /// <summary>
        /// Tahaa 54
        /// </summary>
        public static readonly Datum Tahaa54 = new Datum(6629, Ellipsoid.FromEpsgNumber(7022), "Tahaa 54");
        /// <summary>
        /// IGN72 Nuku Hiva
        /// </summary>
        public static readonly Datum IGN72NukuHiva = new Datum(6630, Ellipsoid.FromEpsgNumber(7022), "IGN72 Nuku Hiva");
        /// <summary>
        /// K0 1949
        /// </summary>
        public static readonly Datum K01949 = new Datum(6631, Ellipsoid.FromEpsgNumber(7022), "K0 1949");
        /// <summary>
        /// Combani 1950
        /// </summary>
        public static readonly Datum Combani1950 = new Datum(6632, Ellipsoid.FromEpsgNumber(7022), "Combani 1950");
        /// <summary>
        /// IGN56 Lifou
        /// </summary>
        public static readonly Datum IGN56Lifou = new Datum(6633, Ellipsoid.FromEpsgNumber(7022), "IGN56 Lifou");
        /// <summary>
        /// IGN72 Grande Terre
        /// </summary>
        public static readonly Datum IGN72GrandeTerre = new Datum(6634, Ellipsoid.FromEpsgNumber(7022), "IGN72 Grande Terre");
        /// <summary>
        /// ST87 Ouvea
        /// </summary>
        public static readonly Datum St87Ouvea1924 = new Datum(6635, Ellipsoid.FromEpsgNumber(7022), "ST87 Ouvea");
        /// <summary>
        /// Petrels 1972
        /// </summary>
        public static readonly Datum Petrels1972 = new Datum(6636, Ellipsoid.FromEpsgNumber(7022), "Petrels 1972");
        /// <summary>
        /// Pointe Geologie Perroud 1950
        /// </summary>
        public static readonly Datum PointeGeologiePerroud1950 = new Datum(6637, Ellipsoid.FromEpsgNumber(7022), "Pointe Geologie Perroud 1950");
        /// <summary>
        /// Saint Pierre et Miquelon 1950
        /// </summary>
        public static readonly Datum SaintPierreetMiquelon1950 = new Datum(6638, Ellipsoid.FromEpsgNumber(7008), "Saint Pierre et Miquelon 1950");
        /// <summary>
        /// MOP78
        /// </summary>
        public static readonly Datum Mop78 = new Datum(6639, Ellipsoid.FromEpsgNumber(7022), "MOP78");
        /// <summary>
        /// Reseau de Reference des Antilles Francaises 1991
        /// </summary>
        public static readonly Datum ReseaudeReferencedesAntillesFrancaises1991 = new Datum(6640, Ellipsoid.FromEpsgNumber(7030), "Reseau de Reference des Antilles Francaises 1991");
        /// <summary>
        /// IGN53 Mare
        /// </summary>
        public static readonly Datum IGN53Mare = new Datum(6641, Ellipsoid.FromEpsgNumber(7022), "IGN53 Mare");
        /// <summary>
        /// ST84 Ile des Pins
        /// </summary>
        public static readonly Datum St84IledesPins = new Datum(6642, Ellipsoid.FromEpsgNumber(7022), "ST84 Ile des Pins");
        /// <summary>
        /// ST71 Belep
        /// </summary>
        public static readonly Datum St71Belep = new Datum(6643, Ellipsoid.FromEpsgNumber(7022), "ST71 Belep");
        /// <summary>
        /// NEA74 Noumea
        /// </summary>
        public static readonly Datum Nea74Noumea = new Datum(6644, Ellipsoid.FromEpsgNumber(7022), "NEA74 Noumea");
        /// <summary>
        /// Reseau Geodesique Nouvelle Caledonie 1991
        /// </summary>
        public static readonly Datum ReseauGeodesiqueNouvelleCaledonie1991 = new Datum(6645, Ellipsoid.FromEpsgNumber(7022), "Reseau Geodesique Nouvelle Caledonie 1991");
        /// <summary>
        /// Grand Comoros
        /// </summary>
        public static readonly Datum GrandComoros = new Datum(6646, Ellipsoid.FromEpsgNumber(7022), "Grand Comoros");
        /// <summary>
        /// International Terrestrial Reference Frame 1988
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1988 = new Datum(6647, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1988");
        /// <summary>
        /// International Terrestrial Reference Frame 1989
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1989 = new Datum(6648, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1989");
        /// <summary>
        /// International Terrestrial Reference Frame 1990
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1990 = new Datum(6649, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1990");
        /// <summary>
        /// International Terrestrial Reference Frame 1991
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1991 = new Datum(6650, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1991");
        /// <summary>
        /// International Terrestrial Reference Frame 1992
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1992 = new Datum(6651, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1992");
        /// <summary>
        /// International Terrestrial Reference Frame 1993
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1993 = new Datum(6652, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1993");
        /// <summary>
        /// International Terrestrial Reference Frame 1994
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1994 = new Datum(6653, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1994");
        /// <summary>
        /// International Terrestrial Reference Frame 1996
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1996 = new Datum(6654, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1996");
        /// <summary>
        /// International Terrestrial Reference Frame 1997
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame1997 = new Datum(6655, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 1997");
        /// <summary>
        /// International Terrestrial Reference Frame 2000
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame2000 = new Datum(6656, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 2000");
        /// <summary>
        /// Reykjavik 1900
        /// </summary>
        public static readonly Datum Reykjavik1900 = new Datum(6657, Ellipsoid.FromEpsgNumber(7051), "Reykjavik 1900");
        /// <summary>
        /// Hjorsey 1955
        /// </summary>
        public static readonly Datum Hjorsey1955 = new Datum(6658, Ellipsoid.FromEpsgNumber(7022), "Hjorsey 1955");
        /// <summary>
        /// Islands Network 1993
        /// </summary>
        public static readonly Datum IslandsNetwork1993 = new Datum(6659, Ellipsoid.FromEpsgNumber(7019), "Islands Network 1993");
        /// <summary>
        /// Helle 1954
        /// </summary>
        public static readonly Datum Helle1954 = new Datum(6660, Ellipsoid.FromEpsgNumber(7022), "Helle 1954");
        /// <summary>
        /// Latvia 1992
        /// </summary>
        public static readonly Datum Latvia1992 = new Datum(6661, Ellipsoid.FromEpsgNumber(7019), "Latvia 1992");
        /// <summary>
        /// Porto Santo 1995
        /// </summary>
        public static readonly Datum PortoSanto1995 = new Datum(6663, Ellipsoid.FromEpsgNumber(7022), "Porto Santo 1995");
        /// <summary>
        /// Azores Oriental Islands 1995
        /// </summary>
        public static readonly Datum AzoresOrientalIslands1995 = new Datum(6664, Ellipsoid.FromEpsgNumber(7022), "Azores Oriental Islands 1995");
        /// <summary>
        /// Azores Central Islands 1995
        /// </summary>
        public static readonly Datum AzoresCentralIslands1995 = new Datum(6665, Ellipsoid.FromEpsgNumber(7022), "Azores Central Islands 1995");
        /// <summary>
        /// Lisbon 1890
        /// </summary>
        public static readonly Datum Lisbon1890 = new Datum(6666, Ellipsoid.FromEpsgNumber(7004), "Lisbon 1890");
        /// <summary>
        /// Iraq-Kuwait Boundary Datum 1992
        /// </summary>
        public static readonly Datum IraqKuwaitBoundaryDatum1992 = new Datum(6667, Ellipsoid.FromEpsgNumber(7030), "Iraq-Kuwait Boundary Datum 1992");
        /// <summary>
        /// European Datum 1979
        /// </summary>
        public static readonly Datum EuropeanDatum1979 = new Datum(6668, Ellipsoid.FromEpsgNumber(7022), "European Datum 1979");
        /// <summary>
        /// Istituto Geografico Militaire 1995
        /// </summary>
        public static readonly Datum IstitutoGeograficoMilitaire1995 = new Datum(6670, Ellipsoid.FromEpsgNumber(7030), "Istituto Geografico Militaire 1995");
        /// <summary>
        /// Voirol 1879
        /// </summary>
        public static readonly Datum Voirol1879 = new Datum(6671, Ellipsoid.FromEpsgNumber(7011), "Voirol 1879");
        /// <summary>
        /// Chatham Islands Datum 1971
        /// </summary>
        public static readonly Datum ChathamIslandsDatum1971 = new Datum(6672, Ellipsoid.FromEpsgNumber(7022), "Chatham Islands Datum 1971");
        /// <summary>
        /// Chatham Islands Datum 1979
        /// </summary>
        public static readonly Datum ChathamIslandsDatum1979 = new Datum(6673, Ellipsoid.FromEpsgNumber(7022), "Chatham Islands Datum 1979");
        /// <summary>
        /// Sistema de Referencia Geocentrico para America del Sur 2000
        /// </summary>
        public static readonly Datum SistemadeReferenciaGeocentricoparaAmericadelSur2000 = new Datum(6674, Ellipsoid.FromEpsgNumber(7019), "Sistema de Referencia Geocentrico para America del Sur 2000");
        /// <summary>
        /// Guam 1963
        /// </summary>
        public static readonly Datum Guam1963 = new Datum(6675, Ellipsoid.FromEpsgNumber(7008), "Guam 1963");
        /// <summary>
        /// Vientiane 1982
        /// </summary>
        public static readonly Datum Vientiane1982 = new Datum(6676, Ellipsoid.FromEpsgNumber(7024), "Vientiane 1982");
        /// <summary>
        /// Lao 1993
        /// </summary>
        public static readonly Datum Lao1993 = new Datum(6677, Ellipsoid.FromEpsgNumber(7024), "Lao 1993");
        /// <summary>
        /// Lao National Datum 1997
        /// </summary>
        public static readonly Datum LaoNationalDatum1997 = new Datum(6678, Ellipsoid.FromEpsgNumber(7024), "Lao National Datum 1997");
        /// <summary>
        /// Jouik 1961
        /// </summary>
        public static readonly Datum Jouik1961 = new Datum(6679, Ellipsoid.FromEpsgNumber(7012), "Jouik 1961");
        /// <summary>
        /// Nouakchott 1965
        /// </summary>
        public static readonly Datum Nouakchott1965 = new Datum(6680, Ellipsoid.FromEpsgNumber(7012), "Nouakchott 1965");
        /// <summary>
        /// Mauritania 1999
        /// </summary>
        public static readonly Datum Mauritania1999RGS = new Datum(6681, Ellipsoid.FromEpsgNumber(7012), "Mauritania 1999 (Clark1980 RGS)");
        /// <summary>
        /// Gulshan 303
        /// </summary>
        public static readonly Datum Gulshan303 = new Datum(6682, Ellipsoid.FromEpsgNumber(7015), "Gulshan 303");
        /// <summary>
        /// Philippine Reference System 1992
        /// </summary>
        public static readonly Datum PhilippineReferenceSystem1992 = new Datum(6683, Ellipsoid.FromEpsgNumber(7008), "Philippine Reference System 1992");
        /// <summary>
        /// Gan 1970
        /// </summary>
        public static readonly Datum Gan1970 = new Datum(6684, Ellipsoid.FromEpsgNumber(7022), "Gan 1970");
        /// <summary>
        /// Gandajika
        /// </summary>
        public static readonly Datum Gandajika = new Datum(6685, Ellipsoid.FromEpsgNumber(7022), "Gandajika");
        /// <summary>
        /// Marco Geocentrico Nacional de Referencia
        /// </summary>
        public static readonly Datum MarcoGeocentricoNacionaldeReferencia = new Datum(6686, Ellipsoid.FromEpsgNumber(7019), "Marco Geocentrico Nacional de Referencia");
        /// <summary>
        /// Reseau Geodesique de la Polynesie Francaise
        /// </summary>
        public static readonly Datum ReseauGeodesiquedelaPolynesieFrancaise = new Datum(6687, Ellipsoid.FromEpsgNumber(7019), "Reseau Geodesique de la Polynesie Francaise");
        /// <summary>
        /// Fatu Iva 72
        /// </summary>
        public static readonly Datum FatuIva72 = new Datum(6688, Ellipsoid.FromEpsgNumber(7022), "Fatu Iva 72");
        /// <summary>
        /// IGN63 Hiva Oa
        /// </summary>
        public static readonly Datum IGN63HivaOa = new Datum(6689, Ellipsoid.FromEpsgNumber(7022), "IGN63 Hiva Oa");
        /// <summary>
        /// Tahiti 79
        /// </summary>
        public static readonly Datum Tahiti79 = new Datum(6690, Ellipsoid.FromEpsgNumber(7022), "Tahiti 79");
        /// <summary>
        /// Moorea 87
        /// </summary>
        public static readonly Datum Moorea87 = new Datum(6691, Ellipsoid.FromEpsgNumber(7022), "Moorea 87");
        /// <summary>
        /// Maupiti 83
        /// </summary>
        public static readonly Datum Maupiti83 = new Datum(6692, Ellipsoid.FromEpsgNumber(7022), "Maupiti 83");
        /// <summary>
        /// Nakhl-e Ghanem
        /// </summary>
        public static readonly Datum NakhlEGhanem = new Datum(6693, Ellipsoid.FromEpsgNumber(7030), "Nakhl-e Ghanem");
        /// <summary>
        /// Posiciones Geodesicas Argentinas 1994
        /// </summary>
        public static readonly Datum PosicionesGeodesicasArgentinas1994 = new Datum(6694, Ellipsoid.FromEpsgNumber(7019), "Posiciones Geodesicas Argentinas 1994");
        /// <summary>
        /// Katanga 1955
        /// </summary>
        public static readonly Datum Katanga1955 = new Datum(6695, Ellipsoid.FromEpsgNumber(7008), "Katanga 1955");
        /// <summary>
        /// Kasai 1953
        /// </summary>
        public static readonly Datum Kasai1953 = new Datum(6696, Ellipsoid.FromEpsgNumber(7012), "Kasai 1953");
        /// <summary>
        /// IGC 1962 Arc of the 6th Parallel South
        /// </summary>
        public static readonly Datum Igc1962Arcofthe6ThParallelSouth = new Datum(6697, Ellipsoid.FromEpsgNumber(7012), "IGC 1962 Arc of the 6th Parallel South");
        /// <summary>
        /// IGN 1962 Kerguelen
        /// </summary>
        public static readonly Datum IGN1962Kerguelen = new Datum(6698, Ellipsoid.FromEpsgNumber(7022), "IGN 1962 Kerguelen");
        /// <summary>
        /// Le Pouce 1934
        /// </summary>
        public static readonly Datum LePouce1934 = new Datum(6699, Ellipsoid.FromEpsgNumber(7012), "Le Pouce 1934");
        /// <summary>
        /// IGN Astro 1960
        /// </summary>
        public static readonly Datum IGNAstro1960 = new Datum(6700, Ellipsoid.FromEpsgNumber(7012), "IGN Astro 1960");
        /// <summary>
        /// Institut Geographique du Congo Belge 1955
        /// </summary>
        public static readonly Datum InstitutGeographiqueduCongoBelge1955 = new Datum(6701, Ellipsoid.FromEpsgNumber(7012), "Institut Geographique du Congo Belge 1955");
        /// <summary>
        /// Mauritania 1999
        /// </summary>
        public static readonly Datum Mauritania1999 = new Datum(6702, Ellipsoid.FromEpsgNumber(7019), "Mauritania 1999");
        /// <summary>
        /// Missao Hidrografico Angola y Sao Tome 1951
        /// </summary>
        public static readonly Datum MissaoHidrograficoAngolaySaoTome1951 = new Datum(6703, Ellipsoid.FromEpsgNumber(7012), "Missao Hidrografico Angola y Sao Tome 1951");
        /// <summary>
        /// Mhast (onshore)
        /// </summary>
        public static readonly Datum MhastOnshore = new Datum(6704, Ellipsoid.FromEpsgNumber(7022), "Mhast (onshore)");
        /// <summary>
        /// Mhast (offshore)
        /// </summary>
        public static readonly Datum MhastOffshore = new Datum(6705, Ellipsoid.FromEpsgNumber(7022), "Mhast (offshore)");
        /// <summary>
        /// Egypt Gulf of Suez S-650 TL
        /// </summary>
        public static readonly Datum EgyptGulfofSuezS650Tl = new Datum(6706, Ellipsoid.FromEpsgNumber(7020), "Egypt Gulf of Suez S-650 TL");
        /// <summary>
        /// Tern Island 1961
        /// </summary>
        public static readonly Datum TernIsland1961 = new Datum(6707, Ellipsoid.FromEpsgNumber(7022), "Tern Island 1961");
        /// <summary>
        /// Cocos Islands 1965
        /// </summary>
        public static readonly Datum CocosIslands1965 = new Datum(6708, Ellipsoid.FromEpsgNumber(7003), "Cocos Islands 1965");
        /// <summary>
        /// Represents the Anna 1 Astro datum of 1965.
        /// </summary>
        public static readonly Datum Anna1Astro1965 = CocosIslands1965;
        /// <summary>
        /// Iwo Jima 1945
        /// </summary>
        public static readonly Datum IwoJima1945 = new Datum(6709, Ellipsoid.FromEpsgNumber(7022), "Iwo Jima 1945");
        /// <summary>
        /// Represents the Astro Beacon "E" datum of 1945.
        /// </summary>
        public static readonly Datum AstroBeaconE1945 = IwoJima1945;
        /// <summary>
        /// St. Helena 1971
        /// </summary>
        public static readonly Datum StHelena1971 = new Datum(6710, Ellipsoid.FromEpsgNumber(7022), "St. Helena 1971");
        /// <summary>
        /// Represents the Astro DOS 71/4 datum.
        /// </summary>
        public static readonly Datum AstroDos714 = StHelena1971;
        /// <summary>
        /// Marcus Island 1952
        /// </summary>
        public static readonly Datum MarcusIsland1952 = new Datum(6711, Ellipsoid.FromEpsgNumber(7022), "Marcus Island 1952");
        /// <summary>
        /// Represents the Astronomical Station datum of 1952.
        /// </summary>
        public static readonly Datum AstronomicalStation1952 = MarcusIsland1952;
        /// <summary>
        /// Ascension Island 1958
        /// </summary>
        public static readonly Datum AscensionIsland1958 = new Datum(6712, Ellipsoid.FromEpsgNumber(7022), "Ascension Island 1958");
        /// <summary>
        /// Ayabelle Lighthouse
        /// </summary>
        public static readonly Datum AyabelleLighthouse = new Datum(6713, Ellipsoid.FromEpsgNumber(7012), "Ayabelle Lighthouse");
        /// <summary>
        /// Bellevue
        /// </summary>
        public static readonly Datum Bellevue = new Datum(6714, Ellipsoid.FromEpsgNumber(7022), "Bellevue");
        /// <summary>
        /// Camp Area Astro
        /// </summary>
        public static readonly Datum CampAreaAstro = new Datum(6715, Ellipsoid.FromEpsgNumber(7022), "Camp Area Astro");
        /// <summary>
        /// Phoenix Islands 1966
        /// </summary>
        public static readonly Datum PhoenixIslands1966 = new Datum(6716, Ellipsoid.FromEpsgNumber(7022), "Phoenix Islands 1966");
        /// <summary>
        /// Represents the Canton Astro datum of 1966.
        /// </summary>
        public static readonly Datum CantonAstro1966 = PhoenixIslands1966;
        /// <summary>
        /// Cape Canaveral
        /// </summary>
        public static readonly Datum CapeCanaveral = new Datum(6717, Ellipsoid.FromEpsgNumber(7008), "Cape Canaveral");
        /// <summary>
        /// Solomon 1968
        /// </summary>
        public static readonly Datum Solomon1968 = new Datum(6718, Ellipsoid.FromEpsgNumber(7022), "Solomon 1968");
        /// <summary>
        /// Represents the DOS datum of 1968.
        /// </summary>
        public static readonly Datum Dos1968 = Solomon1968;
        /// <summary>
        /// Represents the GUX 1 Astro datum.
        /// </summary>
        public static readonly Datum Gux1Astro = Solomon1968;
        /// <summary>
        /// Easter Island 1967
        /// </summary>
        public static readonly Datum EasterIsland1967 = new Datum(6719, Ellipsoid.FromEpsgNumber(7022), "Easter Island 1967");
        /// <summary>
        /// Fiji Geodetic Datum 1986
        /// </summary>
        public static readonly Datum FijiGeodeticDatum1986 = new Datum(6720, Ellipsoid.FromEpsgNumber(7043), "Fiji Geodetic Datum 1986");
        /// <summary>
        /// Fiji 1956
        /// </summary>
        public static readonly Datum Fiji1956 = new Datum(6721, Ellipsoid.FromEpsgNumber(7022), "Fiji 1956");
        /// <summary>
        /// South Georgia 1968
        /// </summary>
        public static readonly Datum SouthGeorgia1968 = new Datum(6722, Ellipsoid.FromEpsgNumber(7022), "South Georgia 1968");
        /// <summary>
        /// Represents the ISTS 061 Astro datum of 1968.
        /// </summary>
        public static readonly Datum Ists061Astro1968 = SouthGeorgia1968;
        /// <summary>
        /// Grand Cayman 1959
        /// </summary>
        public static readonly Datum GrandCayman1959 = new Datum(6723, Ellipsoid.FromEpsgNumber(7008), "Grand Cayman 1959");
        /// <summary>
        /// Diego Garcia 1969
        /// </summary>
        public static readonly Datum DiegoGarcia1969 = new Datum(6724, Ellipsoid.FromEpsgNumber(7022), "Diego Garcia 1969");
        /// <summary>
        /// Represents the ISTS 073 Astro datum of 1969.
        /// </summary>
        public static readonly Datum Ists073Astro1969 = DiegoGarcia1969;
        /// <summary>
        /// Johnston Island 1961
        /// </summary>
        public static readonly Datum JohnstonIsland1961 = new Datum(6725, Ellipsoid.FromEpsgNumber(7022), "Johnston Island 1961");
        /// <summary>
        /// Little Cayman 1961
        /// </summary>
        public static readonly Datum LittleCayman1961 = new Datum(6726, Ellipsoid.FromEpsgNumber(7008), "Little Cayman 1961");
        /// <summary>
        /// Represents the L. C. 5 Astro datum of 1961.
        /// </summary>
        public static readonly Datum Lc5Astro1961 = LittleCayman1961;
        /// <summary>
        /// Midway 1961
        /// </summary>
        public static readonly Datum Midway1961 = new Datum(6727, Ellipsoid.FromEpsgNumber(7022), "Midway 1961");
        /// <summary>
        /// Pico de la Nieves
        /// </summary>
        public static readonly Datum PicodelaNieves = new Datum(6728, Ellipsoid.FromEpsgNumber(7022), "Pico de la Nieves");
        /// <summary>
        /// Pitcairn 1967
        /// </summary>
        public static readonly Datum Pitcairn1967 = new Datum(6729, Ellipsoid.FromEpsgNumber(7022), "Pitcairn 1967");
        /// <summary>
        /// Santo 1965
        /// </summary>
        public static readonly Datum Santo1965 = new Datum(6730, Ellipsoid.FromEpsgNumber(7022), "Santo 1965");
        /// <summary>
        /// Viti Levu 1916
        /// </summary>
        public static readonly Datum VitiLevu1916 = new Datum(6731, Ellipsoid.FromEpsgNumber(7012), "Viti Levu 1916");
        /// <summary>
        /// Marshall Islands 1960
        /// </summary>
        public static readonly Datum MarshallIslands1960 = new Datum(6732, Ellipsoid.FromEpsgNumber(7053), "Marshall Islands 1960");
        /// <summary>
        /// Wake Island 1952
        /// </summary>
        public static readonly Datum WakeIsland1952 = new Datum(6733, Ellipsoid.FromEpsgNumber(7022), "Wake Island 1952");
        /// <summary>
        /// Tristan 1968
        /// </summary>
        public static readonly Datum Tristan1968 = new Datum(6734, Ellipsoid.FromEpsgNumber(7022), "Tristan 1968");
        /// <summary>
        /// Kusaie 1951
        /// </summary>
        public static readonly Datum Kusaie1951 = new Datum(6735, Ellipsoid.FromEpsgNumber(7022), "Kusaie 1951");
        /// <summary>
        /// Deception Island
        /// </summary>
        public static readonly Datum DeceptionIsland = new Datum(6736, Ellipsoid.FromEpsgNumber(7012), "Deception Island");
        /// <summary>
        /// Geocentric datum of Korea
        /// </summary>
        public static readonly Datum GeocentricdatumofKorea = new Datum(6737, Ellipsoid.FromEpsgNumber(7019), "Geocentric datum of Korea");
        /// <summary>
        /// Hong Kong 1963
        /// </summary>
        public static readonly Datum HongKong1963 = new Datum(6738, Ellipsoid.FromEpsgNumber(7007), "Hong Kong 1963");
        /// <summary>
        /// Hong Kong 1963(67)
        /// </summary>
        public static readonly Datum HongKong1967 = new Datum(6739, Ellipsoid.FromEpsgNumber(7022), "Hong Kong 1963(67)");
        /// <summary>
        /// Parametrop Zemp 1990
        /// </summary>
        public static readonly Datum ParametropZemp1990 = new Datum(6740, Ellipsoid.FromEpsgNumber(7054), "Parametrop Zemp 1990");
        /// <summary>
        /// Faroe Datum 1954
        /// </summary>
        public static readonly Datum FaroeDatum1954 = new Datum(6741, Ellipsoid.FromEpsgNumber(7022), "Faroe Datum 1954");
        /// <summary>
        /// Geodetic Datum of Malaysia 2000
        /// </summary>
        public static readonly Datum GeodeticDatumofMalaysia2000 = new Datum(6742, Ellipsoid.FromEpsgNumber(7019), "Geodetic Datum of Malaysia 2000");
        /// <summary>
        /// Karbala 1979 (Polservice)
        /// </summary>
        public static readonly Datum Karbala1979Polservice = new Datum(6743, Ellipsoid.FromEpsgNumber(7012), "Karbala 1979 (Polservice)");
        /// <summary>
        /// Nahrwan 1934
        /// </summary>
        public static readonly Datum Nahrwan1934 = new Datum(6744, Ellipsoid.FromEpsgNumber(7012), "Nahrwan 1934");
        /// <summary>
        /// Rauenberg Datum/83
        /// </summary>
        public static readonly Datum RauenbergDatum83 = new Datum(6745, Ellipsoid.FromEpsgNumber(7004), "Rauenberg Datum/83");
        /// <summary>
        /// Potsdam Datum/83
        /// </summary>
        public static readonly Datum PotsdamDatum83 = new Datum(6746, Ellipsoid.FromEpsgNumber(7004), "Potsdam Datum/83");
        /// <summary>
        /// Greenland 1996
        /// </summary>
        public static readonly Datum Greenland1996 = new Datum(6747, Ellipsoid.FromEpsgNumber(7019), "Greenland 1996");
        /// <summary>
        /// Vanua Levu 1915
        /// </summary>
        public static readonly Datum VanuaLevu1915 = new Datum(6748, Ellipsoid.FromEpsgNumber(7055), "Vanua Levu 1915");
        /// <summary>
        /// Reseau Geodesique de Nouvelle Caledonie 91-93
        /// </summary>
        public static readonly Datum ReseauGeodesiquedeNouvelleCaledonie93 = new Datum(6749, Ellipsoid.FromEpsgNumber(7019), "Reseau Geodesique de Nouvelle Caledonie 91-93");
        /// <summary>
        /// ST87 Ouvea
        /// </summary>
        public static readonly Datum St87Ouvea = new Datum(6750, Ellipsoid.FromEpsgNumber(7030), "ST87 Ouvea");
        /// <summary>
        /// Kertau (RSO)
        /// </summary>
        public static readonly Datum KertauRso = new Datum(6751, Ellipsoid.FromEpsgNumber(7056), "Kertau (RSO)");
        /// <summary>
        /// Represents the Kertau datum of 1948.
        /// </summary>
        public static readonly Datum Kertau1948 = KertauRso;
        /// <summary>
        /// Viti Levu 1912
        /// </summary>
        public static readonly Datum VitiLevu1912 = new Datum(6752, Ellipsoid.FromEpsgNumber(7055), "Viti Levu 1912");
        /// <summary>
        /// fk89
        /// </summary>
        public static readonly Datum Fk89 = new Datum(6753, Ellipsoid.FromEpsgNumber(7022), "fk89");
        /// <summary>
        /// Libyan Geodetic Datum 2006
        /// </summary>
        public static readonly Datum LibyanGeodeticDatum2006 = new Datum(6754, Ellipsoid.FromEpsgNumber(7022), "Libyan Geodetic Datum 2006");
        /// <summary>
        /// Datum Geodesi Nasional 1995
        /// </summary>
        public static readonly Datum DatumGeodesiNasional1995 = new Datum(6755, Ellipsoid.FromEpsgNumber(7030), "Datum Geodesi Nasional 1995");
        /// <summary>
        /// Vietnam 2000
        /// </summary>
        public static readonly Datum Vietnam2000 = new Datum(6756, Ellipsoid.FromEpsgNumber(7030), "Vietnam 2000");
        /// <summary>
        /// SVY21
        /// </summary>
        public static readonly Datum Svy21 = new Datum(6757, Ellipsoid.FromEpsgNumber(7030), "SVY21");
        /// <summary>
        /// Jamaica 2001
        /// </summary>
        public static readonly Datum Jamaica2001 = new Datum(6758, Ellipsoid.FromEpsgNumber(7030), "Jamaica 2001");
        /// <summary>
        /// CH1903 (Bern)
        /// </summary>
        public static readonly Datum Ch1903Bern = new Datum(6801, Ellipsoid.FromEpsgNumber(7004), 7.439583333, "CH1903 (Bern)");
        /// <summary>
        /// Bogota 1975 (Bogota)
        /// </summary>
        public static readonly Datum Bogota1975Bogota = new Datum(6802, Ellipsoid.FromEpsgNumber(7022), 74.08091667, "Bogota 1975 (Bogota)");
        /// <summary>
        /// Lisbon 1937 (Lisbon)
        /// </summary>
        public static readonly Datum Lisbon1937Lisbon = new Datum(6803, Ellipsoid.FromEpsgNumber(7022), -9.131906111, "Lisbon 1937 (Lisbon)");
        /// <summary>
        /// Makassar (Jakarta)
        /// </summary>
        public static readonly Datum MakassarJakarta = new Datum(6804, Ellipsoid.FromEpsgNumber(7004), 106.8077194, "Makassar (Jakarta)");
        /// <summary>
        /// Militar-Geographische Institut (Ferro)
        /// </summary>
        public static readonly Datum MilitarGeographischeInstitutFerro = new Datum(6805, Ellipsoid.FromEpsgNumber(7004), -17.66666667, "Militar-Geographische Institut (Ferro)");
        /// <summary>
        /// Monte Mario (Rome)
        /// </summary>
        public static readonly Datum MonteMarioRome = new Datum(6806, Ellipsoid.FromEpsgNumber(7022), 12.45233333, "Monte Mario (Rome)");
        /// <summary>
        /// Nouvelle Triangulation Francaise (Paris)
        /// </summary>
        public static readonly Datum NouvelleTriangulationFrancaiseParis = new Datum(6807, Ellipsoid.FromEpsgNumber(7011), 2.337291667, "Nouvelle Triangulation Francaise (Paris)");
        /// <summary>
        /// Padang 1884 (Jakarta)
        /// </summary>
        public static readonly Datum Padang1884Jakarta = new Datum(6808, Ellipsoid.FromEpsgNumber(7004), 106.8077194, "Padang 1884 (Jakarta)");
        /// <summary>
        /// Reseau National Belge 1950 (Brussels)
        /// </summary>
        public static readonly Datum ReseauNationalBelge1950Brussels = new Datum(6809, Ellipsoid.FromEpsgNumber(7022), 4.367975, "Reseau National Belge 1950 (Brussels)");
        /// <summary>
        /// Tananarive 1925 (Paris)
        /// </summary>
        public static readonly Datum Tananarive1925Paris = new Datum(6810, Ellipsoid.FromEpsgNumber(7022), 2.337291667, "Tananarive 1925 (Paris)");
        /// <summary>
        /// Voirol 1875 (Paris)
        /// </summary>
        public static readonly Datum Voirol1875Paris = new Datum(6811, Ellipsoid.FromEpsgNumber(7011), 2.337291667, "Voirol 1875 (Paris)");
        /// <summary>
        /// Batavia (Jakarta)
        /// </summary>
        public static readonly Datum BataviaJakarta = new Datum(6813, Ellipsoid.FromEpsgNumber(7004), 106.8077194, "Batavia (Jakarta)");
        /// <summary>
        /// Stockholm 1938 (Stockholm)
        /// </summary>
        public static readonly Datum Stockholm1938Stockholm = new Datum(6814, Ellipsoid.FromEpsgNumber(7004), 18.05827778, "Stockholm 1938 (Stockholm)");
        /// <summary>
        /// Greek (Athens)
        /// </summary>
        public static readonly Datum GreekAthens = new Datum(6815, Ellipsoid.FromEpsgNumber(7004), 23.7163375, "Greek (Athens)");
        /// <summary>
        /// Carthage (Paris)
        /// </summary>
        public static readonly Datum CarthageParis = new Datum(6816, Ellipsoid.FromEpsgNumber(7011), 2.337291667, "Carthage (Paris)");
        /// <summary>
        /// NGO 1948 (Oslo)
        /// </summary>
        public static readonly Datum Ngo1948Oslo = new Datum(6817, Ellipsoid.FromEpsgNumber(7005), 10.72291667, "NGO 1948 (Oslo)");
        /// <summary>
        /// S-JTSK (Ferro)
        /// </summary>
        public static readonly Datum SJtskFerro = new Datum(6818, Ellipsoid.FromEpsgNumber(7004), -17.66666667, "S-JTSK (Ferro)");
        /// <summary>
        /// Nord Sahara 1959 (Paris)
        /// </summary>
        public static readonly Datum NordSahara1959Paris = new Datum(6819, Ellipsoid.FromEpsgNumber(7012), 2.337291667, "Nord Sahara 1959 (Paris)");
        /// <summary>
        /// Gunung Segara (Jakarta)
        /// </summary>
        public static readonly Datum GunungSegaraJakarta = new Datum(6820, Ellipsoid.FromEpsgNumber(7004), 106.8077194, "Gunung Segara (Jakarta)");
        /// <summary>
        /// Voirol 1879 (Paris)
        /// </summary>
        public static readonly Datum Voirol1879Paris = new Datum(6821, Ellipsoid.FromEpsgNumber(7011), 2.337291667, "Voirol 1879 (Paris)");
        /// <summary>
        /// International Terrestrial Reference Frame 2005
        /// </summary>
        public static readonly Datum InternationalTerrestrialReferenceFrame2005 = new Datum(6896, Ellipsoid.FromEpsgNumber(7019), "International Terrestrial Reference Frame 2005");
        /// <summary>
        /// Ancienne Triangulation Francaise (Paris)
        /// </summary>
        public static readonly Datum AncienneTriangulationFrancaiseParis = new Datum(6901, Ellipsoid.FromEpsgNumber(7027), 2.337291667, "Ancienne Triangulation Francaise (Paris)");
        /// <summary>
        /// Nord de Guerre (Paris)
        /// </summary>
        public static readonly Datum NorddeGuerreParis = new Datum(6902, Ellipsoid.FromEpsgNumber(7027), 2.337291667, "Nord de Guerre (Paris)");
        /// <summary>
        /// Madrid 1870 (Madrid)
        /// </summary>
        public static readonly Datum Madrid1870Madrid = new Datum(6903, Ellipsoid.FromEpsgNumber(7028), -3.687938889, "Madrid 1870 (Madrid)");
        /// <summary>
        /// Lisbon 1890 (Lisbon)
        /// </summary>
        public static readonly Datum Lisbon1890Lisbon = new Datum(6904, Ellipsoid.FromEpsgNumber(7004), -9.131906111, "Lisbon 1890 (Lisbon)");

        #endregion EPSG Datums

        #region Non-EPSG Datums

        /// <summary>
        /// Represents the Estonia Coordinate System datum of 1937.
        /// </summary>
        public static readonly Datum EstoniaLocalDatum1937 = new Datum("Estonia Coordinate System 1937", Ellipsoid.Bessel1841);
        /// <summary>
        /// Represents the Old Egyptian datum of 1907.
        /// </summary>
        public static readonly Datum OldEgyptian1907 = new Datum("Old Egyptian 1907", Ellipsoid.Helmert1906);
        /// <summary>
        /// Represents the Oman datum.
        /// </summary>
        public static readonly Datum Oman = new Datum("Oman", Ellipsoid.Clarke1880);
        /// <summary>
        /// Represents the Pointe Noire datum.
        /// </summary>
        public static readonly Datum PointeNoire1948 = new Datum("Pointe Noire 1948", Ellipsoid.Clarke1880);
        /// <summary>
        /// Represents the Rome datum of 1940.
        /// </summary>
        public static readonly Datum Rome1940 = new Datum("Rome 1940", Ellipsoid.International1924);
        /// <summary>
        /// Represents the Sao Braz datum.
        /// </summary>
        public static readonly Datum SaoBraz = new Datum("Sao Braz", Ellipsoid.International1924);
        /// <summary>
        /// Represents the South Asia datum.
        /// </summary>
        public static readonly Datum SouthAsia = new Datum("South Asia", Ellipsoid.ModifiedFischer1960);
        /// <summary>
        /// Represents the Voirol datum of 1960.
        /// </summary>
        public static readonly Datum Voirol1960 = new Datum("Voirol 1960", Ellipsoid.Clarke1880);
        /// <summary>
        /// Represents the Wake Eniwetok datum of 1960.
        /// </summary>
        public static readonly Datum WakeEniwetok1960 = new Datum("Wake Eniwetok 1960", Ellipsoid.Hough1960);

        /// <summary>
        ///
        /// </summary>
        public static readonly Datum WorldGeodeticSystem1960 = new Datum("World Geodetic System 1960", Ellipsoid.Wgs1960);
        /// <summary>
        ///
        /// </summary>
        public static readonly Datum WorldGeodeticSystem1966 = new Datum("World Geodetic System 1966", Ellipsoid.Wgs1966);

        #endregion Non-EPSG Datums

        /* IMPORTANT: The Default field must be after Datum.WorldGeodeticSystem1984 is initialized, otherwise it will be null! */

        /// <summary>
        /// Represents the default datum used by the DotSpatial.Positioning, WGS1984.
        /// </summary>
        public static readonly Datum Default = WorldGeodeticSystem1984;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified name and reference ellipsoid.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        public Datum(string name, Ellipsoid ellipsoid)
        {
            _ellipsoid = ellipsoid;
            _name = name;
            _primeMeridian = Longitude.Empty;

            SanityCheck();

            // Add the datum to the collection
            _datums.Add(this);
        }

        /// <summary>
        /// Internal constructor for static list generation
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="name">The name.</param>
        internal Datum(int epsgNumber, Ellipsoid ellipsoid, string name)
        {
            _epsgNumber = epsgNumber;
            _ellipsoid = ellipsoid;
            _name = name;
            _primeMeridian = Longitude.Empty;

            SanityCheck();

            // Add the datum to the collection
            _epsgDatums.Add(this);
        }

        /// <summary>
        /// Internal constructor for static list generation
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="meridian">The meridian.</param>
        /// <param name="name">The name.</param>
        internal Datum(int epsgNumber, Ellipsoid ellipsoid, double meridian, string name)
        {
            _epsgNumber = epsgNumber;
            _ellipsoid = ellipsoid;
            _name = name;
            _primeMeridian = new Longitude(meridian);

            SanityCheck();

            // Add the datum to the collection
            _epsgDatums.Add(this);
        }

        /// <summary>
        /// Validates the datum. Called in the constructor.
        /// </summary>
        private void SanityCheck()
        {
            if (_ellipsoid == null)
                throw new ArgumentException("Datum constructiopn failed. Ellipsoid is null or invalid");
        }

        #endregion Constructors

        #region Public Members

        /// <summary>
        /// Returns the name of the datum.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// European Petroleum Survey Group number for this datum. The ESPG standards are now maintained by OGP
        /// (International Association of Oil and Gas Producers).
        /// </summary>
        public int EpsgNumber
        {
            get { return _epsgNumber; }
        }

        /// <summary>
        /// Returns the interpretation of Earth's shape associated with a datum.
        /// </summary>
        /// <value>Read only. An
        /// <see cref="Ellipsoid">Ellipsoid</see>
        /// object.</value>
        /// <example>
        /// This example gets information on the ellipsoid associated with the WGS84 datum.
        ///   <code lang="VB">
        /// ' Get information about the NAD1983 datum
        /// Dim MyDatum As Datum = Geodesy.GetDatum(DatumType.NorthAmerican1983)
        /// ' Get the ellipsoid associated with this datum
        /// Dim MyEllipsoid As Ellipsoid = MyDatum.Ellipsoid
        /// ' Write the semi-major axis of the ellipsoid
        /// Debug.WriteLine(MyEllipsoid.SemiMajorAxis.ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Get information about the NAD1983 datum
        /// Datum MyDatum = Geodesy.GetDatum(DatumType.NorthAmerican1983);
        /// // Get the ellipsoid associated with this datum
        /// Ellipsoid MyEllipsoid = MyDatum.Ellipsoid;
        /// // Write the semi-major axis of the ellipsoid
        /// Debug.WriteLine(MyEllipsoid.SemiMajorAxis.ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="Ellipsoid">Ellipsoid Class</seealso>
        /// <remarks>Each datum is associated with an ellipsoid, which is an interpretation of Earth's shape and
        /// size.</remarks>
        public Ellipsoid Ellipsoid
        {
            get
            {
                return _ellipsoid;
            }
        }

        /// <summary>
        /// Returns the prime meridian assocated with this Datum.
        /// </summary>
        /// <remarks>Most datums use Greenwich as the prime meridian. However, several systems offset coordinates
        /// using a local meridian. This value reflects that usage.</remarks>
        public Longitude PrimeMeridian
        {
            get { return _primeMeridian; }
        }

        #endregion Public Members

        #region Overrides

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Datum)
                return Equals((Datum)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _ellipsoid.GetHashCode() ^ _name.GetHashCode();
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Returns a Datum object matching the specified name.
        /// </summary>
        /// <param name="name">A <strong>String</strong> describing the name of an existing datum.</param>
        /// <returns>A <strong>Datum</strong> object matching the specified string, or null if no datum was found.</returns>
        public static Datum FromName(string name)
        {
            // Search the custom objects
            foreach (Datum item in _datums)
            {
                if (item.Name == name)
                    return item;
            }
            // Search the EPSG objects
            return _epsgDatums.Select((t, index) => _datums[index]).FirstOrDefault(item => item.Name == name);
        }

        /// <summary>
        /// Returns the datum corresponding to the EPSG code
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <returns></returns>
        public static Datum FromEpsgNumber(int epsgNumber)
        {
            // Search the EPSG objects
            return _epsgDatums.FirstOrDefault(item => item.EpsgNumber == epsgNumber);
        }

        #endregion Static Methods

        #region IEquatable<Datum> Members

        /// <summary>
        /// Compares the current instance to the specified datum object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Datum other)
        {
            return _ellipsoid.Equals(other.Ellipsoid)
                && _primeMeridian.Equals(other.PrimeMeridian);
        }

        #endregion IEquatable<Datum> Members
    }
}