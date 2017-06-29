using System;
using System.Collections.Generic;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
    /// <summary>Represents a coordinate system based on interpretations of the Earth's shape and size.</summary>
    /// <remarks>
    /// 	<para>Over the course of history, advances in technology have given people the
    ///     ability to more accurately measure the shape and size of the Earth. Since countries
    ///     have built significant infrastructure based upon older coordinate systems, they
    ///     cannot immediately abandon them in favor of new ones. As a result, there are now
    ///     over fifty interpretations of Earth's shape and size in use all over the
    ///     world.</para>
    /// 	<para>Some datums, such as World Geodetic System 1984 (or WGS84 for short) are
    ///     becoming more widely used throughout the world, and this datum is used by nearly
    ///     all GPS devices. However, while the world is slowly standardizing its datums, some
    ///     datums will not be abandoned because they remain quite accurate for a specific,
    ///     local area.</para>
    /// 	<para>A datum on its own is nothing more than a more granular interpretation of an
    ///     ellipsoid. Typically, more specific coordinate transformation information is
    ///     further associated with a datum to produce meaningful information. For example,
    ///     Helmert and Molodensky coordinate conversion formulas use several local conversion
    ///     parameters for each datum.</para>
    /// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
    ///     immutable (its properties can only be set via constructors).</para>
    /// </remarks>
    /// <seealso cref="Ellipsoid">Ellipsoid Class</seealso>
#if !PocketPC || DesignTime
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Datum : IEquatable<Datum>
    {
        private readonly int _EPSGNumber = 32767;
        private readonly Longitude _PrimeMeridian;
        private readonly Ellipsoid _Ellipsoid;
        private readonly string _Name;

        private static readonly List<Datum> _Datums = new List<Datum>(16);
        private static List<Datum> _EPSGDatums = new List<Datum>(512);

        #region Fields

        #region EPSG Datums

        /// <summary> Not specified (based on the Airy 1830 ellipsoid)  </summary>
        /// <remarks> This datum uses the Airy 1830 ellipsoid </remarks>
        public static readonly Datum UnspecifiedAiry1830 = new Datum(6001, Ellipsoid.FromEPSGNumber(7001), "Not specified (based on Airy 1830 ellipsoid)");
        /// <summary> Not specified (based on the Airy Modified 1849 ellipsoid)  </summary>
        /// <remarks> This datum uses the Airy Modified 1849 ellipsoid </remarks>
        public static readonly Datum UnspecifiedAiryModified1849 = new Datum(6002, Ellipsoid.FromEPSGNumber(7002), "Not specified (based on Airy Modified 1849 ellipsoid)");
        /// <summary> Not specified (based on the Australian National Spheroid ellipsoid)  </summary>
        /// <remarks> This datum uses the Australian National Spheroid ellipsoid </remarks>
        public static readonly Datum UnspecifiedAustralianNationalSpheroid = new Datum(6003, Ellipsoid.FromEPSGNumber(7003), "Not specified (based on Australian National Spheroid)");
        /// <summary> Not specified (based on the Bessel 1841 ellipsoid)  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum UnspecifiedBessel1841 = new Datum(6004, Ellipsoid.FromEPSGNumber(7004), "Not specified (based on Bessel 1841 ellipsoid)");
        /// <summary> Not specified (based on the Bessel Modified ellipsoid)  </summary>
        /// <remarks> This datum uses the Bessel Modified ellipsoid </remarks>
        public static readonly Datum UnspecifiedBesselModified = new Datum(6005, Ellipsoid.FromEPSGNumber(7005), "Not specified (based on Bessel Modified ellipsoid)");
        /// <summary> Not specified (based on the Bessel Namibia (GLM) ellipsoid)  </summary>
        /// <remarks> This datum uses the Bessel Namibia (GLM) ellipsoid </remarks>
        public static readonly Datum UnspecifiedBesselNamibiaGLM = new Datum(6006, Ellipsoid.FromEPSGNumber(7046), "Not specified (based on Bessel Namibia ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1858 ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1858 ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1858 = new Datum(6007, Ellipsoid.FromEPSGNumber(7007), "Not specified (based on Clarke 1858 ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1866 ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1866 = new Datum(6008, Ellipsoid.FromEPSGNumber(7008), "Not specified (based on Clarke 1866 ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1866 Michigan ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1866 Michigan ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1866Michigan = new Datum(6009, Ellipsoid.FromEPSGNumber(7009), "Not specified (based on Clarke 1866 Michigan ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 (Benoit) ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 (Benoit) ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880Benoit = new Datum(6010, Ellipsoid.FromEPSGNumber(7010), "Not specified (based on Clarke 1880 (Benoit) ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 (IGN) ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880IGN = new Datum(6011, Ellipsoid.FromEPSGNumber(7011), "Not specified (based on Clarke 1880 (IGN) ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 (RGS) ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880RGS = new Datum(6012, Ellipsoid.FromEPSGNumber(7012), "Not specified (based on Clarke 1880 (RGS) ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 (Arc) ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 (Arc) ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880Arc = new Datum(6013, Ellipsoid.FromEPSGNumber(7013), "Not specified (based on Clarke 1880 (Arc) ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 (SGA 1922) ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 (SGA 1922) ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880_SGA1922 = new Datum(6014, Ellipsoid.FromEPSGNumber(7014), "Not specified (based on Clarke 1880 (SGA 1922) ellipsoid)");
        /// <summary> Not specified (based on the Everest 1830 (1937 Adjustment) ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest1830_1937Adjustment = new Datum(6015, Ellipsoid.FromEPSGNumber(7015), "Not specified (based on Everest 1830 (1937 Adjustment) ellipsoid)");
        /// <summary> Not specified (based on the Everest 1830 (1967 Definition) ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest 1830 (1967 Definition) ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest1830_1967Definition = new Datum(6016, Ellipsoid.FromEPSGNumber(7016), "Not specified (based on Everest 1830 (1967 Definition) ellipsoid)");
        /// <summary> Not specified (based on the Everest 1830 Modified ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest 1830 Modified ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest1830Modified = new Datum(6018, Ellipsoid.FromEPSGNumber(7018), "Not specified (based on Everest 1830 Modified ellipsoid)");
        /// <summary> Not specified (based on the GRS 1980 ellipsoid)  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum UnspecifiedGRS1980 = new Datum(6019, Ellipsoid.FromEPSGNumber(7019), "Not specified (based on GRS 1980 ellipsoid)");
        /// <summary> Not specified (based on the Helmert 1906 ellipsoid)  </summary>
        /// <remarks> This datum uses the Helmert 1906 ellipsoid </remarks>
        public static readonly Datum UnspecifiedHelmert1906 = new Datum(6020, Ellipsoid.FromEPSGNumber(7020), "Not specified (based on Helmert 1906 ellipsoid)");
        /// <summary> Not specified (based on the Indonesian National Spheroid ellipsoid)  </summary>
        /// <remarks> This datum uses the Indonesian National Spheroid ellipsoid </remarks>
        public static readonly Datum UnspecifiedIndonesianNationalSpheroid = new Datum(6021, Ellipsoid.FromEPSGNumber(7021), "Not specified (based on Indonesian National Spheroid)");
        /// <summary> Not specified (based on the International 1924 ellipsoid)  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum UnspecifiedInternational1924 = new Datum(6022, Ellipsoid.FromEPSGNumber(7022), "Not specified (based on International 1924 ellipsoid)");
        /// <summary> Not specified (based on the Krassowsky 1940 ellipsoid)  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum UnspecifiedKrassowsky1940 = new Datum(6024, Ellipsoid.FromEPSGNumber(7024), "Not specified (based on Krassowsky 1940 ellipsoid)");
        /// <summary> Not specified (based on the NWL 9D ellipsoid)  </summary>
        /// <remarks> This datum uses the NWL 9D ellipsoid </remarks>
        public static readonly Datum UnspecifiedNWL9D = new Datum(6025, Ellipsoid.FromEPSGNumber(7025), "Not specified (based on NWL 9D ellipsoid)");
        /// <summary> Not specified (based on the Plessis 1817 ellipsoid)  </summary>
        /// <remarks> This datum uses the Plessis 1817 ellipsoid </remarks>
        public static readonly Datum UnspecifiedPlessis1817 = new Datum(6027, Ellipsoid.FromEPSGNumber(7027), "Not specified (based on Plessis 1817 ellipsoid)");
        /// <summary> Not specified (based on the Struve 1860 ellipsoid)  </summary>
        /// <remarks> This datum uses the Struve 1860 ellipsoid </remarks>
        public static readonly Datum UnspecifiedStruve1860 = new Datum(6028, Ellipsoid.FromEPSGNumber(7028), "Not specified (based on Struve 1860 ellipsoid)");
        /// <summary> Not specified (based on the War Office ellipsoid)  </summary>
        /// <remarks> This datum uses the War Office ellipsoid </remarks>
        public static readonly Datum UnspecifiedWarOffice = new Datum(6029, Ellipsoid.FromEPSGNumber(7029), "Not specified (based on War Office ellipsoid)");
        /// <summary> Not specified (based on the WGS 84 ellipsoid)  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum UnspecifiedWGS84 = new Datum(6030, Ellipsoid.FromEPSGNumber(7030), "Not specified (based on WGS 84 ellipsoid)");
        /// <summary> Not specified (based on the GEM 10C ellipsoid)  </summary>
        /// <remarks> This datum uses the GEM 10C ellipsoid </remarks>
        public static readonly Datum UnspecifiedGEM10C = new Datum(6031, Ellipsoid.FromEPSGNumber(7031), "Not specified (based on GEM 10C ellipsoid)");
        /// <summary> Not specified (based on the OSU86F ellipsoid)  </summary>
        /// <remarks> This datum uses the OSU86F ellipsoid </remarks>
        public static readonly Datum UnspecifiedOSU86F = new Datum(6032, Ellipsoid.FromEPSGNumber(7032), "Not specified (based on OSU86F ellipsoid)");
        /// <summary> Not specified (based on the OSU91A ellipsoid)  </summary>
        /// <remarks> This datum uses the OSU91A ellipsoid </remarks>
        public static readonly Datum UnspecifiedOSU91A = new Datum(6033, Ellipsoid.FromEPSGNumber(7033), "Not specified (based on OSU91A ellipsoid)");
        /// <summary> Not specified (based on the Clarke 1880 ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1880 ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1880 = new Datum(6034, Ellipsoid.FromEPSGNumber(7034), "Not specified (based on Clarke 1880 ellipsoid)");
        /// <summary> Not specified (based on the Sphere ellipsoid)  </summary>
        /// <remarks> This datum uses the Sphere ellipsoid </remarks>
        public static readonly Datum UnspecifiedSphere = new Datum(6035, Ellipsoid.FromEPSGNumber(7035), "Not specified (based on Authalic Sphere)");
        /// <summary> Not specified (based on the GRS 1967 ellipsoid)  </summary>
        /// <remarks> This datum uses the GRS 1967 ellipsoid </remarks>
        public static readonly Datum UnspecifiedGRS1967 = new Datum(6036, Ellipsoid.FromEPSGNumber(7036), "Not specified (based on GRS 1967 ellipsoid)");
        /// <summary> Not specified (based on the Average Terrestrial System 1977 ellipsoid)  </summary>
        /// <remarks> This datum uses the Average Terrestrial System 1977 ellipsoid </remarks>
        public static readonly Datum UnspecifiedAverageTerrestrialSystem1977 = new Datum(6041, Ellipsoid.FromEPSGNumber(7041), "Not specified (based on Average Terrestrial System 1977 ellipsoid)");
        /// <summary> Not specified (based on the Everest (1830 Definition) ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest (1830 Definition) ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest_1830Definition = new Datum(6042, Ellipsoid.FromEPSGNumber(7042), "Not specified (based on Everest (1830 Definition) ellipsoid)");
        /// <summary> Not specified (based on the WGS 72 ellipsoid)  </summary>
        /// <remarks> This datum uses the WGS 72 ellipsoid </remarks>
        public static readonly Datum UnspecifiedWGS72 = new Datum(6043, Ellipsoid.FromEPSGNumber(7043), "Not specified (based on WGS 72 ellipsoid)");
        /// <summary> Not specified (based on the Everest 1830 (1962 Definition) ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest 1830 (1962 Definition) ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest1830_1962Definition = new Datum(6044, Ellipsoid.FromEPSGNumber(7044), "Not specified (based on Everest 1830 (1962 Definition) ellipsoid)");
        /// <summary> Not specified (based on the Everest 1830 (1975 Definition) ellipsoid)  </summary>
        /// <remarks> This datum uses the Everest 1830 (1975 Definition) ellipsoid </remarks>
        public static readonly Datum UnspecifiedEverest1830_1975Definition = new Datum(6045, Ellipsoid.FromEPSGNumber(7045), "Not specified (based on Everest 1830 (1975 Definition) ellipsoid)");
        /// <summary> Not specified (based on the GRS 1980 Authalic Sphere ellipsoid)  </summary>
        /// <remarks> This datum uses the GRS 1980 Authalic Sphere ellipsoid </remarks>
        public static readonly Datum UnspecifiedGRS1980AuthalicSphere = new Datum(6047, Ellipsoid.FromEPSGNumber(7048), "Not specified (based on GRS 1980 Authalic Sphere)");
        /// <summary> Not specified (based on the Clarke 1866 Authalic Sphere ellipsoid)  </summary>
        /// <remarks> This datum uses the Clarke 1866 Authalic Sphere ellipsoid </remarks>
        public static readonly Datum UnspecifiedClarke1866AuthalicSphere = new Datum(6052, Ellipsoid.FromEPSGNumber(7052), "Not specified (based on Clarke 1866 Authalic Sphere)");
        /// <summary> Not specified (based on the International 1924 Authalic Sphere ellipsoid)  </summary>
        /// <remarks> This datum uses the International 1924 Authalic Sphere ellipsoid </remarks>
        public static readonly Datum UnspecifiedInternational1924AuthalicSphere = new Datum(6053, Ellipsoid.FromEPSGNumber(7057), "Not specified (based on International 1924 Authalic Sphere)");
        /// <summary> Not specified (based on the Hughes 1980 ellipsoid)  </summary>
        /// <remarks> This datum uses the Hughes 1980 ellipsoid </remarks>
        public static readonly Datum UnspecifiedHughes1980 = new Datum(6054, Ellipsoid.FromEPSGNumber(7058), "Not specified (based on Hughes 1980 ellipsoid)");
        /// <summary> Greek  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Greek = new Datum(6120, Ellipsoid.FromEPSGNumber(7004), "Greek");
        /// <summary> Greek Geodetic Reference System 1987  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum GreekGeodeticReferenceSystem1987 = new Datum(6121, Ellipsoid.FromEPSGNumber(7019), "Greek Geodetic Reference System 1987");
        /// <summary> Average Terrestrial System 1977  </summary>
        /// <remarks> This datum uses the Average Terrestrial System 1977 ellipsoid </remarks>
        public static readonly Datum AverageTerrestrialSystem1977 = new Datum(6122, Ellipsoid.FromEPSGNumber(7041), "Average Terrestrial System 1977");
        /// <summary> Kartastokoordinaattijarjestelma (1966)  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Kartastokoordinaattijarjestelma_1966 = new Datum(6123, Ellipsoid.FromEPSGNumber(7022), "Kartastokoordinaattijarjestelma (1966)");
        /// <summary> Rikets koordinatsystem 1990  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Riketskoordinatsystem1990 = new Datum(6124, Ellipsoid.FromEPSGNumber(7004), "Rikets koordinatsystem 1990");
        /// <summary> Samboja  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Samboja = new Datum(6125, Ellipsoid.FromEPSGNumber(7004), "Samboja");
        /// <summary> Lithuania 1994 (ETRS89)  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Lithuania1994_ETRS89 = new Datum(6126, Ellipsoid.FromEPSGNumber(7019), "Lithuania 1994 (ETRS89)");
        /// <summary> Tete  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Tete = new Datum(6127, Ellipsoid.FromEPSGNumber(7008), "Tete");
        /// <summary> Madzansua  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Madzansua = new Datum(6128, Ellipsoid.FromEPSGNumber(7008), "Madzansua");
        /// <summary> Observatario  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Observatario = new Datum(6129, Ellipsoid.FromEPSGNumber(7008), "Observatario");
        /// <summary> Moznet (ITRF94)  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Moznet_ITRF94 = new Datum(6130, Ellipsoid.FromEPSGNumber(7030), "Moznet (ITRF94)");
        /// <summary> Indian 1960  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Indian1960 = new Datum(6131, Ellipsoid.FromEPSGNumber(7015), "Indian 1960");
        /// <summary>Represents the Indian datum.</summary>
        public static readonly Datum Indian = Indian1960;
        /// <summary> Final Datum 1958  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum FinalDatum1958 = new Datum(6132, Ellipsoid.FromEPSGNumber(7012), "Final Datum 1958");
        /// <summary> Estonia 1992  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Estonia1992 = new Datum(6133, Ellipsoid.FromEPSGNumber(7019), "Estonia 1992");
        /// <summary> PDO Survey Datum 1993  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum PDOSurveyDatum1993 = new Datum(6134, Ellipsoid.FromEPSGNumber(7012), "PDO Survey Datum 1993");
        /// <summary> Old Hawaiian  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum OldHawaiian = new Datum(6135, Ellipsoid.FromEPSGNumber(7008), "Old Hawaiian");
        /// <summary> St. Lawrence Island  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum StLawrenceIsland = new Datum(6136, Ellipsoid.FromEPSGNumber(7008), "St. Lawrence Island");
        /// <summary> St. Paul Island  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum StPaulIsland = new Datum(6137, Ellipsoid.FromEPSGNumber(7008), "St. Paul Island");
        /// <summary> St. George Island  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum StGeorgeIsland = new Datum(6138, Ellipsoid.FromEPSGNumber(7008), "St. George Island");
        /// <summary> Puerto Rico  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum PuertoRico = new Datum(6139, Ellipsoid.FromEPSGNumber(7008), "Puerto Rico");
        /// <summary> NAD83 Canadian Spatial Reference System  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum NAD83CanadianSpatialReferenceSystem = new Datum(6140, Ellipsoid.FromEPSGNumber(7019), "NAD83 Canadian Spatial Reference System");
        /// <summary> Israel  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Israel = new Datum(6141, Ellipsoid.FromEPSGNumber(7019), "Israel");
        /// <summary> Locodjo 1965  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Locodjo1965 = new Datum(6142, Ellipsoid.FromEPSGNumber(7012), "Locodjo 1965");
        /// <summary> Abidjan 1987  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Abidjan1987 = new Datum(6143, Ellipsoid.FromEPSGNumber(7012), "Abidjan 1987");
        /// <summary> Kalianpur 1937  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Kalianpur1937 = new Datum(6144, Ellipsoid.FromEPSGNumber(7015), "Kalianpur 1937");
        /// <summary> Kalianpur 1962  </summary>
        /// <remarks> This datum uses the Everest 1830 (1962 Definition) ellipsoid </remarks>
        public static readonly Datum Kalianpur1962 = new Datum(6145, Ellipsoid.FromEPSGNumber(7044), "Kalianpur 1962");
        /// <summary> Kalianpur 1975  </summary>
        /// <remarks> This datum uses the Everest 1830 (1975 Definition) ellipsoid </remarks>
        public static readonly Datum Kalianpur1975 = new Datum(6146, Ellipsoid.FromEPSGNumber(7045), "Kalianpur 1975");
        /// <summary> Hanoi 1972  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Hanoi1972 = new Datum(6147, Ellipsoid.FromEPSGNumber(7024), "Hanoi 1972");
        /// <summary> Hartebeesthoek94  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Hartebeesthoek94 = new Datum(6148, Ellipsoid.FromEPSGNumber(7030), "Hartebeesthoek94");
        /// <summary> CH1903  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum CH1903 = new Datum(6149, Ellipsoid.FromEPSGNumber(7004), "CH1903");
        /// <summary> CH1903+  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum CH1903plus = new Datum(6150, Ellipsoid.FromEPSGNumber(7004), "CH1903+");
        /// <summary> Swiss Terrestrial Reference Frame 1995  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum SwissTerrestrialReferenceFrame1995 = new Datum(6151, Ellipsoid.FromEPSGNumber(7019), "Swiss Terrestrial Reference Frame 1995");
        /// <summary> NAD83 (High Accuracy Regional Network)  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum NAD83_HighAccuracyRegionalNetwork = new Datum(6152, Ellipsoid.FromEPSGNumber(7019), "NAD83 (High Accuracy Regional Network)");
        /// <summary> Rassadiran  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Rassadiran = new Datum(6153, Ellipsoid.FromEPSGNumber(7022), "Rassadiran");
        /// <summary> European Datum 1950(1977)  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EuropeanDatum1950_1977 = new Datum(6154, Ellipsoid.FromEPSGNumber(7022), "European Datum 1950(1977)");
        /// <summary> Dabola 1981  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Dabola1981 = new Datum(6155, Ellipsoid.FromEPSGNumber(7011), "Dabola 1981");
        /// <summary> Jednotne Trigonometricke Site Katastralni  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum JednotneTrigonometrickeSiteKatastralni = new Datum(6156, Ellipsoid.FromEPSGNumber(7004), "Jednotne Trigonometricke Site Katastralni");
        /// <summary> Mount Dillon  </summary>
        /// <remarks> This datum uses the Clarke 1858 ellipsoid </remarks>
        public static readonly Datum MountDillon = new Datum(6157, Ellipsoid.FromEPSGNumber(7007), "Mount Dillon");
        /// <summary> Naparima 1955  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Naparima1955 = new Datum(6158, Ellipsoid.FromEPSGNumber(7022), "Naparima 1955");
        /// <summary> European Libyan Datum 1979  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EuropeanLibyanDatum1979 = new Datum(6159, Ellipsoid.FromEPSGNumber(7022), "European Libyan Datum 1979");
        /// <summary> Chos Malal 1914  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ChosMalal1914 = new Datum(6160, Ellipsoid.FromEPSGNumber(7022), "Chos Malal 1914");
        /// <summary> Pampa del Castillo  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PampadelCastillo = new Datum(6161, Ellipsoid.FromEPSGNumber(7022), "Pampa del Castillo");
        /// <summary> Korean Datum 1985  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum KoreanDatum1985 = new Datum(6162, Ellipsoid.FromEPSGNumber(7004), "Korean Datum 1985");
        /// <summary> Yemen National Geodetic Network 1996  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum YemenNationalGeodeticNetwork1996 = new Datum(6163, Ellipsoid.FromEPSGNumber(7030), "Yemen National Geodetic Network 1996");
        /// <summary> South Yemen  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum SouthYemen = new Datum(6164, Ellipsoid.FromEPSGNumber(7024), "South Yemen");
        /// <summary> Bissau  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Bissau = new Datum(6165, Ellipsoid.FromEPSGNumber(7022), "Bissau");
        /// <summary> Korean Datum 1995  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum KoreanDatum1995 = new Datum(6166, Ellipsoid.FromEPSGNumber(7030), "Korean Datum 1995");
        /// <summary> New Zealand Geodetic Datum 2000  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum NewZealandGeodeticDatum2000 = new Datum(6167, Ellipsoid.FromEPSGNumber(7019), "New Zealand Geodetic Datum 2000");
        /// <summary> Accra  </summary>
        /// <remarks> This datum uses the War Office ellipsoid </remarks>
        public static readonly Datum Accra = new Datum(6168, Ellipsoid.FromEPSGNumber(7029), "Accra");
        /// <summary> American Samoa 1962  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum AmericanSamoa1962 = new Datum(6169, Ellipsoid.FromEPSGNumber(7008), "American Samoa 1962");
        /// <summary> Sistema de Referencia Geocentrico para America del Sur 1995  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum SistemadeReferenciaGeocentricoparaAmericadelSur1995 = new Datum(6170, Ellipsoid.FromEPSGNumber(7019), "Sistema de Referencia Geocentrico para America del Sur 1995");
        /// <summary> Reseau Geodesique Francais 1993  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiqueFrancais1993 = new Datum(6171, Ellipsoid.FromEPSGNumber(7019), "Reseau Geodesique Francais 1993");
        /// <summary> Posiciones Geodesicas Argentinas  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum PosicionesGeodesicasArgentinas = new Datum(6172, Ellipsoid.FromEPSGNumber(7019), "Posiciones Geodesicas Argentinas");
        /// <summary> IRENET95  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum IRENET95 = new Datum(6173, Ellipsoid.FromEPSGNumber(7019), "IRENET95");
        /// <summary> Sierra Leone Colony 1924  </summary>
        /// <remarks> This datum uses the War Office ellipsoid </remarks>
        public static readonly Datum SierraLeoneColony1924 = new Datum(6174, Ellipsoid.FromEPSGNumber(7029), "Sierra Leone Colony 1924");
        /// <summary> Sierra Leone 1968  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum SierraLeone1968 = new Datum(6175, Ellipsoid.FromEPSGNumber(7012), "Sierra Leone 1968");
        /// <summary> Australian Antarctic Datum 1998  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum AustralianAntarcticDatum1998 = new Datum(6176, Ellipsoid.FromEPSGNumber(7019), "Australian Antarctic Datum 1998");
        /// <summary> Pulkovo 1942/83  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Pulkovo1942_83 = new Datum(6178, Ellipsoid.FromEPSGNumber(7024), "Pulkovo 1942/83");
        /// <summary> Pulkovo 1942/58  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Pulkovo1942_58 = new Datum(6179, Ellipsoid.FromEPSGNumber(7024), "Pulkovo 1942/58");
        /// <summary> Estonia 1997  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Estonia1997 = new Datum(6180, Ellipsoid.FromEPSGNumber(7019), "Estonia 1997");
        /// <summary> Luxembourg 1930  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Luxembourg1930 = new Datum(6181, Ellipsoid.FromEPSGNumber(7022), "Luxembourg 1930");
        /// <summary> Azores Occidental Islands 1939  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AzoresOccidentalIslands1939 = new Datum(6182, Ellipsoid.FromEPSGNumber(7022), "Azores Occidental Islands 1939");
        /// <summary>Represents the Observatorio Meteorologico datum of 1939.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum ObservatorioMeteorologico1939 = AzoresOccidentalIslands1939;
        /// <summary> Azores Central Islands 1948  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AzoresCentralIslands1948 = new Datum(6183, Ellipsoid.FromEPSGNumber(7022), "Azores Central Islands 1948");
        /// <summary>Represents the Graciosa Base SW datum of 1948.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum GraciosaBaseSW1948 = AzoresCentralIslands1948;
        /// <summary> Azores Oriental Islands 1940  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AzoresOrientalIslands1940 = new Datum(6184, Ellipsoid.FromEPSGNumber(7022), "Azores Oriental Islands 1940");
        /// <summary> Madeira 1936  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Madeira1936 = new Datum(6185, Ellipsoid.FromEPSGNumber(7022), "Madeira 1936");
        /// <summary> OSNI 1952  </summary>
        /// <remarks> This datum uses the Airy 1830 ellipsoid </remarks>
        public static readonly Datum OSNI1952 = new Datum(6188, Ellipsoid.FromEPSGNumber(7001), "OSNI 1952");
        /// <summary> Red Geodesica Venezolana  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum RedGeodesicaVenezolana = new Datum(6189, Ellipsoid.FromEPSGNumber(7019), "Red Geodesica Venezolana");
        /// <summary> Posiciones Geodesicas Argentinas 1998  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum PosicionesGeodesicasArgentinas1998 = new Datum(6190, Ellipsoid.FromEPSGNumber(7019), "Posiciones Geodesicas Argentinas 1998");
        /// <summary> Albanian 1987  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Albanian1987 = new Datum(6191, Ellipsoid.FromEPSGNumber(7024), "Albanian 1987");
        /// <summary> Douala 1948  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Douala1948 = new Datum(6192, Ellipsoid.FromEPSGNumber(7022), "Douala 1948");
        /// <summary> Manoca 1962  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Manoca1962 = new Datum(6193, Ellipsoid.FromEPSGNumber(7011), "Manoca 1962");
        /// <summary> Qornoq 1927  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Qornoq1927 = new Datum(6194, Ellipsoid.FromEPSGNumber(7022), "Qornoq 1927");
        /// <summary> Scoresbysund 1952  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Scoresbysund1952 = new Datum(6195, Ellipsoid.FromEPSGNumber(7022), "Scoresbysund 1952");
        /// <summary> Ammassalik 1958  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Ammassalik1958 = new Datum(6196, Ellipsoid.FromEPSGNumber(7022), "Ammassalik 1958");
        /// <summary> Garoua  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum GarouaRGS = new Datum(6197, Ellipsoid.FromEPSGNumber(7012), "Garoua RGS");
        /// <summary> Kousseri  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Kousseri = new Datum(6198, Ellipsoid.FromEPSGNumber(7012), "Kousseri");
        /// <summary> Egypt 1930  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Egypt1930 = new Datum(6199, Ellipsoid.FromEPSGNumber(7022), "Egypt 1930");
        /// <summary> Pulkovo 1995  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Pulkovo1995 = new Datum(6200, Ellipsoid.FromEPSGNumber(7024), "Pulkovo 1995");
        /// <summary> Adindan  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Adindan = new Datum(6201, Ellipsoid.FromEPSGNumber(7012), "Adindan");
        /// <summary> Australian Geodetic Datum 1966  </summary>
        /// <remarks> This datum uses the Australian National Spheroid ellipsoid </remarks>
        public static readonly Datum AustralianGeodeticDatum1966 = new Datum(6202, Ellipsoid.FromEPSGNumber(7003), "Australian Geodetic Datum 1966");
        /// <summary> Australian Geodetic Datum 1984  </summary>
        /// <remarks> This datum uses the Australian National Spheroid ellipsoid </remarks>
        public static readonly Datum AustralianGeodeticDatum1984 = new Datum(6203, Ellipsoid.FromEPSGNumber(7003), "Australian Geodetic Datum 1984");
        /// <summary> Ain el Abd 1970  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AinelAbd1970 = new Datum(6204, Ellipsoid.FromEPSGNumber(7022), "Ain el Abd 1970");
        /// <summary> Afgooye  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Afgooye = new Datum(6205, Ellipsoid.FromEPSGNumber(7024), "Afgooye");
        /// <summary> Agadez  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Agadez = new Datum(6206, Ellipsoid.FromEPSGNumber(7011), "Agadez");
        /// <summary> Lisbon 1937  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Lisbon1937 = new Datum(6207, Ellipsoid.FromEPSGNumber(7022), "Lisbon 1937");
        /// <summary> Aratu  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Aratu = new Datum(6208, Ellipsoid.FromEPSGNumber(7022), "Aratu");
        /// <summary> Arc 1950  </summary>
        /// <remarks> This datum uses the Clarke 1880 (Arc) ellipsoid </remarks>
        public static readonly Datum Arc1950 = new Datum(6209, Ellipsoid.FromEPSGNumber(7013), "Arc 1950");
        /// <summary> Arc 1960  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Arc1960 = new Datum(6210, Ellipsoid.FromEPSGNumber(7012), "Arc 1960");
        /// <summary> Batavia  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Batavia = new Datum(6211, Ellipsoid.FromEPSGNumber(7004), "Batavia");
        /// <summary> Barbados 1938  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Barbados1938 = new Datum(6212, Ellipsoid.FromEPSGNumber(7012), "Barbados 1938");
        /// <summary> Beduaram  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Beduaram = new Datum(6213, Ellipsoid.FromEPSGNumber(7011), "Beduaram");
        /// <summary> Beijing 1954  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Beijing1954 = new Datum(6214, Ellipsoid.FromEPSGNumber(7024), "Beijing 1954");
        /// <summary> Reseau National Belge 1950  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ReseauNationalBelge1950 = new Datum(6215, Ellipsoid.FromEPSGNumber(7022), "Reseau National Belge 1950");
        /// <summary> Bermuda 1957  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Bermuda1957 = new Datum(6216, Ellipsoid.FromEPSGNumber(7008), "Bermuda 1957");
        /// <summary> Bogota 1975  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Bogota1975 = new Datum(6218, Ellipsoid.FromEPSGNumber(7022), "Bogota 1975");
        /// <summary> Bukit Rimpah  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum BukitRimpah = new Datum(6219, Ellipsoid.FromEPSGNumber(7004), "Bukit Rimpah");
        /// <summary> Camacupa  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Camacupa = new Datum(6220, Ellipsoid.FromEPSGNumber(7012), "Camacupa");
        /// <summary> Campo Inchauspe  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum CampoInchauspe = new Datum(6221, Ellipsoid.FromEPSGNumber(7022), "Campo Inchauspe");
        /// <summary> Cape  </summary>
        /// <remarks> This datum uses the Clarke 1880 (Arc) ellipsoid </remarks>
        public static readonly Datum Cape = new Datum(6222, Ellipsoid.FromEPSGNumber(7013), "Cape");
        /// <summary> Carthage  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Carthage = new Datum(6223, Ellipsoid.FromEPSGNumber(7011), "Carthage");
        /// <summary> Chua  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Chua = new Datum(6224, Ellipsoid.FromEPSGNumber(7022), "Chua");
        /// <summary> Corrego Alegre  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum CorregoAlegre = new Datum(6225, Ellipsoid.FromEPSGNumber(7022), "Corrego Alegre");
        /// <summary> Cote d'Ivoire  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum CotedIvoire = new Datum(6226, Ellipsoid.FromEPSGNumber(7011), "Cote d'Ivoire");
        /// <summary> Deir ez Zor  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum DeirezZor = new Datum(6227, Ellipsoid.FromEPSGNumber(7011), "Deir ez Zor");
        /// <summary> Douala  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Douala = new Datum(6228, Ellipsoid.FromEPSGNumber(7011), "Douala");
        /// <summary> Egypt 1907  </summary>
        /// <remarks> This datum uses the Helmert 1906 ellipsoid </remarks>
        public static readonly Datum Egypt1907 = new Datum(6229, Ellipsoid.FromEPSGNumber(7020), "Egypt 1907");
        /// <summary> European Datum 1950  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EuropeanDatum1950 = new Datum(6230, Ellipsoid.FromEPSGNumber(7022), "European Datum 1950");
        /// <summary> European Datum 1987  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EuropeanDatum1987 = new Datum(6231, Ellipsoid.FromEPSGNumber(7022), "European Datum 1987");
        /// <summary> Fahud  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Fahud = new Datum(6232, Ellipsoid.FromEPSGNumber(7012), "Fahud");
        /// <summary> Gandajika 1970  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Gandajika1970 = new Datum(6233, Ellipsoid.FromEPSGNumber(7022), "Gandajika 1970");
        /// <summary> Garoua  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum GarouaIGN = new Datum(6234, Ellipsoid.FromEPSGNumber(7011), "Garoua IGN");
        /// <summary> Guyane Francaise  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum GuyaneFrancaise = new Datum(6235, Ellipsoid.FromEPSGNumber(7022), "Guyane Francaise");
        /// <summary> Hu Tzu Shan  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum HuTzuShan = new Datum(6236, Ellipsoid.FromEPSGNumber(7022), "Hu Tzu Shan");
        /// <summary> Hungarian Datum 1972  </summary>
        /// <remarks> This datum uses the GRS 1967 ellipsoid </remarks>
        public static readonly Datum HungarianDatum1972 = new Datum(6237, Ellipsoid.FromEPSGNumber(7036), "Hungarian Datum 1972");
        /// <summary> Indonesian Datum 1974  </summary>
        /// <remarks> This datum uses the Indonesian National Spheroid ellipsoid </remarks>
        public static readonly Datum IndonesianDatum1974 = new Datum(6238, Ellipsoid.FromEPSGNumber(7021), "Indonesian Datum 1974");
        /// <summary> Indian 1954  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Indian1954 = new Datum(6239, Ellipsoid.FromEPSGNumber(7015), "Indian 1954");
        /// <summary> Indian 1975  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Indian1975 = new Datum(6240, Ellipsoid.FromEPSGNumber(7015), "Indian 1975");
        /// <summary> Jamaica 1875  </summary>
        /// <remarks> This datum uses the Clarke 1880 ellipsoid </remarks>
        public static readonly Datum Jamaica1875 = new Datum(6241, Ellipsoid.FromEPSGNumber(7034), "Jamaica 1875");
        /// <summary> Jamaica 1969  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Jamaica1969 = new Datum(6242, Ellipsoid.FromEPSGNumber(7008), "Jamaica 1969");
        /// <summary> Kalianpur 1880  </summary>
        /// <remarks> This datum uses the Everest (1830 Definition) ellipsoid </remarks>
        public static readonly Datum Kalianpur1880 = new Datum(6243, Ellipsoid.FromEPSGNumber(7042), "Kalianpur 1880");
        /// <summary> Kandawala  </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Kandawala = new Datum(6244, Ellipsoid.FromEPSGNumber(7015), "Kandawala");
        /// <summary> Kertau 1968  </summary>
        /// <remarks> This datum uses the Everest 1830 Modified ellipsoid </remarks>
        public static readonly Datum Kertau1968 = new Datum(6245, Ellipsoid.FromEPSGNumber(7018), "Kertau 1968");
        /// <summary> Kuwait Oil Company  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum KuwaitOilCompany = new Datum(6246, Ellipsoid.FromEPSGNumber(7012), "Kuwait Oil Company");
        /// <summary> La Canoa  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum LaCanoa = new Datum(6247, Ellipsoid.FromEPSGNumber(7022), "La Canoa");
        /// <summary> Provisional South American Datum 1956  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ProvisionalSouthAmericanDatum1956 = new Datum(6248, Ellipsoid.FromEPSGNumber(7022), "Provisional South American Datum 1956");
        /// <summary> Lake  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Lake = new Datum(6249, Ellipsoid.FromEPSGNumber(7022), "Lake");
        /// <summary> Leigon  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Leigon = new Datum(6250, Ellipsoid.FromEPSGNumber(7012), "Leigon");
        /// <summary> Liberia 1964  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Liberia1964 = new Datum(6251, Ellipsoid.FromEPSGNumber(7012), "Liberia 1964");
        /// <summary> Lome  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Lome = new Datum(6252, Ellipsoid.FromEPSGNumber(7011), "Lome");
        /// <summary> Luzon 1911  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Luzon1911 = new Datum(6253, Ellipsoid.FromEPSGNumber(7008), "Luzon 1911");
        /// <summary> Hito XVIII 1963  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum HitoXVIII1963 = new Datum(6254, Ellipsoid.FromEPSGNumber(7022), "Hito XVIII 1963");
        /// <summary>Represents the Provisional South Chilean datum of 1963.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum ProvisionalSouthChilean1963 = HitoXVIII1963;
        /// <summary> Herat North  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum HeratNorth = new Datum(6255, Ellipsoid.FromEPSGNumber(7022), "Herat North");
        /// <summary> Mahe 1971  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Mahe1971 = new Datum(6256, Ellipsoid.FromEPSGNumber(7012), "Mahe 1971");
        /// <summary> Makassar  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Makassar = new Datum(6257, Ellipsoid.FromEPSGNumber(7004), "Makassar");
        /// <summary> European Terrestrial Reference System 1989  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum EuropeanTerrestrialReferenceSystem1989 = new Datum(6258, Ellipsoid.FromEPSGNumber(7019), "European Terrestrial Reference System 1989");
        /// <summary> Malongo 1987  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Malongo1987 = new Datum(6259, Ellipsoid.FromEPSGNumber(7022), "Malongo 1987");
        /// <summary> Manoca  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Manoca = new Datum(6260, Ellipsoid.FromEPSGNumber(7012), "Manoca");
        /// <summary> Merchich  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Merchich = new Datum(6261, Ellipsoid.FromEPSGNumber(7011), "Merchich");
        /// <summary> Massawa  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Massawa = new Datum(6262, Ellipsoid.FromEPSGNumber(7004), "Massawa");
        /// <summary> Minna  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Minna = new Datum(6263, Ellipsoid.FromEPSGNumber(7012), "Minna");
        /// <summary> Mhast  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Mhast = new Datum(6264, Ellipsoid.FromEPSGNumber(7022), "Mhast");
        /// <summary> Monte Mario  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum MonteMario = new Datum(6265, Ellipsoid.FromEPSGNumber(7022), "Monte Mario");
        /// <summary> M'poraloko  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Mporaloko = new Datum(6266, Ellipsoid.FromEPSGNumber(7011), "M'poraloko");
        /// <summary> North American Datum 1927  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum NorthAmericanDatum1927 = new Datum(6267, Ellipsoid.FromEPSGNumber(7008), "North American Datum 1927");
        /// <summary> NAD Michigan  </summary>
        /// <remarks> This datum uses the Clarke 1866 Michigan ellipsoid </remarks>
        public static readonly Datum NADMichigan = new Datum(6268, Ellipsoid.FromEPSGNumber(7009), "NAD Michigan");
        /// <summary> North American Datum 1983  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum NorthAmericanDatum1983 = new Datum(6269, Ellipsoid.FromEPSGNumber(7019), "North American Datum 1983");
        /// <summary> Nahrwan 1967  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Nahrwan1967 = new Datum(6270, Ellipsoid.FromEPSGNumber(7012), "Nahrwan 1967");
        /// <summary> Naparima 1972  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Naparima1972 = new Datum(6271, Ellipsoid.FromEPSGNumber(7022), "Naparima 1972");
        /// <summary> New Zealand Geodetic Datum 1949  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum NewZealandGeodeticDatum1949 = new Datum(6272, Ellipsoid.FromEPSGNumber(7022), "New Zealand Geodetic Datum 1949");
        /// <summary>Represents the Geodetic Datum of 1949.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum GeodeticDatum1949 = NewZealandGeodeticDatum1949;
        /// <summary> NGO 1948  </summary>
        /// <remarks> This datum uses the Bessel Modified ellipsoid </remarks>
        public static readonly Datum NGO1948 = new Datum(6273, Ellipsoid.FromEPSGNumber(7005), "NGO 1948");
        /// <summary> Datum 73  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Datum73 = new Datum(6274, Ellipsoid.FromEPSGNumber(7022), "Datum 73");
        /// <summary> Nouvelle Triangulation Francaise  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum NouvelleTriangulationFrancaise = new Datum(6275, Ellipsoid.FromEPSGNumber(7011), "Nouvelle Triangulation Francaise");
        /// <summary> NSWC 9Z-2  </summary>
        /// <remarks> This datum uses the NWL 9D ellipsoid </remarks>
        public static readonly Datum NSWC9Z_2 = new Datum(6276, Ellipsoid.FromEPSGNumber(7025), "NSWC 9Z-2");
        /// <summary> OSGB 1936  </summary>
        /// <remarks> This datum uses the Airy 1830 ellipsoid </remarks>
        public static readonly Datum OSGB1936 = new Datum(6277, Ellipsoid.FromEPSGNumber(7001), "OSGB 1936");
        /// <summary> Represents the Ordnance Survey of Great Britain datum of 1936. </summary>
        public static readonly Datum OrdnanceSurveyGreatBritain1936 = OSGB1936;
        /// <summary> OSGB 1970 (SN)  </summary>
        /// <remarks> This datum uses the Airy 1830 ellipsoid </remarks>
        public static readonly Datum OSGB1970_SN = new Datum(6278, Ellipsoid.FromEPSGNumber(7001), "OSGB 1970 (SN)");
        /// <summary> OS (SN) 1980  </summary>
        /// <remarks> This datum uses the Airy 1830 ellipsoid </remarks>
        public static readonly Datum OS_SN1980 = new Datum(6279, Ellipsoid.FromEPSGNumber(7001), "OS (SN) 1980");
        /// <summary> Padang 1884  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Padang1884 = new Datum(6280, Ellipsoid.FromEPSGNumber(7004), "Padang 1884");
        /// <summary> Palestine 1923  </summary>
        /// <remarks> This datum uses the Clarke 1880 (Benoit) ellipsoid </remarks>
        public static readonly Datum Palestine1923 = new Datum(6281, Ellipsoid.FromEPSGNumber(7010), "Palestine 1923");
        /// <summary> Congo 1960 Pointe Noire  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Congo1960PointeNoire = new Datum(6282, Ellipsoid.FromEPSGNumber(7011), "Congo 1960 Pointe Noire");
        /// <summary> Geocentric Datum of Australia 1994  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum GeocentricDatumofAustralia1994 = new Datum(6283, Ellipsoid.FromEPSGNumber(7019), "Geocentric Datum of Australia 1994");
        /// <summary> Pulkovo 1942  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Pulkovo1942 = new Datum(6284, Ellipsoid.FromEPSGNumber(7024), "Pulkovo 1942");
        /// <summary> Qatar 1974  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Qatar1974 = new Datum(6285, Ellipsoid.FromEPSGNumber(7022), "Qatar 1974");
        /// <summary> Qatar 1948  </summary>
        /// <remarks> This datum uses the Helmert 1906 ellipsoid </remarks>
        public static readonly Datum Qatar1948 = new Datum(6286, Ellipsoid.FromEPSGNumber(7020), "Qatar 1948");
        /// <summary> Qornoq  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Qornoq = new Datum(6287, Ellipsoid.FromEPSGNumber(7022), "Qornoq");
        /// <summary> Loma Quintana  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum LomaQuintana = new Datum(6288, Ellipsoid.FromEPSGNumber(7022), "Loma Quintana");
        /// <summary> Amersfoort  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Amersfoort = new Datum(6289, Ellipsoid.FromEPSGNumber(7004), "Amersfoort");
        /// <summary> South American Datum 1969  </summary>
        /// <remarks> This datum uses the GRS 1967 ellipsoid </remarks>
        public static readonly Datum SouthAmericanDatum1969 = new Datum(6291, Ellipsoid.FromEPSGNumber(7036), "South American Datum 1969");
        /// <summary> Sapper Hill 1943  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum SapperHill1943 = new Datum(6292, Ellipsoid.FromEPSGNumber(7022), "Sapper Hill 1943");
        /// <summary> Schwarzeck  </summary>
        /// <remarks> This datum uses the Bessel Namibia (GLM) ellipsoid </remarks>
        public static readonly Datum Schwarzeck = new Datum(6293, Ellipsoid.FromEPSGNumber(7046), "Schwarzeck");
        /// <summary> Segora  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Segora = new Datum(6294, Ellipsoid.FromEPSGNumber(7004), "Segora");
        /// <summary> Serindung  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Serindung = new Datum(6295, Ellipsoid.FromEPSGNumber(7004), "Serindung");
        /// <summary> Sudan  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Sudan = new Datum(6296, Ellipsoid.FromEPSGNumber(7011), "Sudan");
        /// <summary> Tananarive 1925  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tananarive1925 = new Datum(6297, Ellipsoid.FromEPSGNumber(7022), "Tananarive 1925");
        /// <summary> Timbalai 1948  </summary>
        /// <remarks> This datum uses the Everest 1830 (1967 Definition) ellipsoid </remarks>
        public static readonly Datum Timbalai1948 = new Datum(6298, Ellipsoid.FromEPSGNumber(7016), "Timbalai 1948");
        /// <summary> TM65  </summary>
        /// <remarks> This datum uses the Airy Modified 1849 ellipsoid </remarks>
        public static readonly Datum TM65 = new Datum(6299, Ellipsoid.FromEPSGNumber(7002), "TM65");
        /// <summary>Represents the Ireland datum of 1965.</summary>
        public static readonly Datum Ireland1965 = TM65;
        /// <summary> Geodetic Datum of 1965  </summary>
        /// <remarks> This datum uses the Airy Modified 1849 ellipsoid </remarks>
        public static readonly Datum GeodeticDatumof1965 = new Datum(6300, Ellipsoid.FromEPSGNumber(7002), "Geodetic Datum of 1965");
        /// <summary> Tokyo  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Tokyo = new Datum(6301, Ellipsoid.FromEPSGNumber(7004), "Tokyo");
        /// <summary> Trinidad 1903  </summary>
        /// <remarks> This datum uses the Clarke 1858 ellipsoid </remarks>
        public static readonly Datum Trinidad1903 = new Datum(6302, Ellipsoid.FromEPSGNumber(7007), "Trinidad 1903");
        /// <summary> Trucial Coast 1948  </summary>
        /// <remarks> This datum uses the Helmert 1906 ellipsoid </remarks>
        public static readonly Datum TrucialCoast1948 = new Datum(6303, Ellipsoid.FromEPSGNumber(7020), "Trucial Coast 1948");
        /// <summary> Voirol 1875  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Voirol1875 = new Datum(6304, Ellipsoid.FromEPSGNumber(7011), "Voirol 1875");
        /// <summary> Bern 1938  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Bern1938 = new Datum(6306, Ellipsoid.FromEPSGNumber(7004), "Bern 1938");
        /// <summary> Nord Sahara 1959  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum NordSahara1959 = new Datum(6307, Ellipsoid.FromEPSGNumber(7012), "Nord Sahara 1959");
        /// <summary> Stockholm 1938  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Stockholm1938 = new Datum(6308, Ellipsoid.FromEPSGNumber(7004), "Stockholm 1938");
        /// <summary> Yacare  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Yacare = new Datum(6309, Ellipsoid.FromEPSGNumber(7022), "Yacare");
        /// <summary> Yoff  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Yoff = new Datum(6310, Ellipsoid.FromEPSGNumber(7011), "Yoff");
        /// <summary> Zanderij  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Zanderij = new Datum(6311, Ellipsoid.FromEPSGNumber(7022), "Zanderij");
        /// <summary> Militar-Geographische Institut  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum MilitarGeographischeInstitut = new Datum(6312, Ellipsoid.FromEPSGNumber(7004), "Militar-Geographische Institut");
        /// <summary>Represents the Hermannskogel datum.</summary>
        public static readonly Datum HermannskogelDatum = MilitarGeographischeInstitut;
        /// <summary> Reseau National Belge 1972  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ReseauNationalBelge1972 = new Datum(6313, Ellipsoid.FromEPSGNumber(7022), "Reseau National Belge 1972");
        /// <summary> Deutsches Hauptdreiecksnetz  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum DeutschesHauptdreiecksnetz = new Datum(6314, Ellipsoid.FromEPSGNumber(7004), "Deutsches Hauptdreiecksnetz");
        /// <summary> Conakry 1905  </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Conakry1905 = new Datum(6315, Ellipsoid.FromEPSGNumber(7011), "Conakry 1905");
        /// <summary> Dealul Piscului 1933  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum DealulPiscului1933 = new Datum(6316, Ellipsoid.FromEPSGNumber(7022), "Dealul Piscului 1933");
        /// <summary> Dealul Piscului 1970  </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum DealulPiscului1970 = new Datum(6317, Ellipsoid.FromEPSGNumber(7024), "Dealul Piscului 1970");
        /// <summary> National Geodetic Network  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum NationalGeodeticNetwork = new Datum(6318, Ellipsoid.FromEPSGNumber(7030), "National Geodetic Network");
        /// <summary> Kuwait Utility  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum KuwaitUtility = new Datum(6319, Ellipsoid.FromEPSGNumber(7019), "Kuwait Utility");
        /// <summary> World Geodetic System 1972  </summary>
        /// <remarks> This datum uses the WGS 72 ellipsoid </remarks>
        public static readonly Datum WorldGeodeticSystem1972 = new Datum(6322, Ellipsoid.FromEPSGNumber(7043), "World Geodetic System 1972");
        /// <summary> WGS 72 Transit Broadcast Ephemeris  </summary>
        /// <remarks> This datum uses the WGS 72 ellipsoid </remarks>
        public static readonly Datum WGS72TransitBroadcastEphemeris = new Datum(6324, Ellipsoid.FromEPSGNumber(7043), "WGS 72 Transit Broadcast Ephemeris");
        /// <summary> World Geodetic System 1984  </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum WorldGeodeticSystem1984 = new Datum(6326, Ellipsoid.FromEPSGNumber(7030), "World Geodetic System 1984");
        /// <summary> Anguilla 1957  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Anguilla1957 = new Datum(6600, Ellipsoid.FromEPSGNumber(7012), "Anguilla 1957");
        /// <summary> Antigua 1943  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Antigua1943 = new Datum(6601, Ellipsoid.FromEPSGNumber(7012), "Antigua 1943");
        /// <summary> Dominica 1945  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Dominica1945 = new Datum(6602, Ellipsoid.FromEPSGNumber(7012), "Dominica 1945");
        /// <summary> Grenada 1953  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Grenada1953 = new Datum(6603, Ellipsoid.FromEPSGNumber(7012), "Grenada 1953");
        /// <summary> Montserrat 1958  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Montserrat1958 = new Datum(6604, Ellipsoid.FromEPSGNumber(7012), "Montserrat 1958");
        /// <summary> St. Kitts 1955  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum StKitts1955 = new Datum(6605, Ellipsoid.FromEPSGNumber(7012), "St. Kitts 1955");
        /// <summary>Represents the Fort Thomas datum of 1955.</summary>
        /// <remarks>This datum uses the Clarke ellipsoid of 1880.</remarks>
        public static readonly Datum FortThomas1955 = StKitts1955;
        /// <summary> St. Lucia 1955  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum StLucia1955 = new Datum(6606, Ellipsoid.FromEPSGNumber(7012), "St. Lucia 1955");
        /// <summary> St. Vincent 1945  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum StVincent1945 = new Datum(6607, Ellipsoid.FromEPSGNumber(7012), "St. Vincent 1945");
        /// <summary> North American Datum 1927 (1976)  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum NorthAmericanDatum1927_1976 = new Datum(6608, Ellipsoid.FromEPSGNumber(7008), "North American Datum 1927 (1976)");
        /// <summary> North American Datum 1927 (CGQ77)  </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum NorthAmericanDatum1927_CGQ77 = new Datum(6609, Ellipsoid.FromEPSGNumber(7008), "North American Datum 1927 (CGQ77)");
        /// <summary> Xian 1980  </summary>
        /// <remarks> This datum uses the Xian 1980 ellipsoid </remarks>
        public static readonly Datum Xian1980 = new Datum(6610, Ellipsoid.FromEPSGNumber(7049), "Xian 1980");
        /// <summary> Hong Kong 1980  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum HongKong1980 = new Datum(6611, Ellipsoid.FromEPSGNumber(7022), "Hong Kong 1980");
        /// <summary> Japanese Geodetic Datum 2000  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum JapaneseGeodeticDatum2000 = new Datum(6612, Ellipsoid.FromEPSGNumber(7019), "Japanese Geodetic Datum 2000");
        /// <summary> Gunung Segara  </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum GunungSegara = new Datum(6613, Ellipsoid.FromEPSGNumber(7004), "Gunung Segara");
        /// <summary> Qatar National Datum 1995  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum QatarNationalDatum1995 = new Datum(6614, Ellipsoid.FromEPSGNumber(7022), "Qatar National Datum 1995");
        /// <summary> Porto Santo 1936  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PortoSanto1936 = new Datum(6615, Ellipsoid.FromEPSGNumber(7022), "Porto Santo 1936");
        /// <summary> Selvagem Grande  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum SelvagemGrande = new Datum(6616, Ellipsoid.FromEPSGNumber(7022), "Selvagem Grande");
        /// <summary> South American Datum 1969  </summary>
        /// <remarks> This datum uses the GRS 1967 (SAD69) ellipsoid </remarks>
        public static readonly Datum SouthAmericanDatum1969_SAD69 = new Datum(6618, Ellipsoid.FromEPSGNumber(7050), "South American Datum 1969 (SAD69)");
        /// <summary> SWEREF99  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum SWEREF99 = new Datum(6619, Ellipsoid.FromEPSGNumber(7019), "SWEREF99");
        /// <summary> Point 58  </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Point58 = new Datum(6620, Ellipsoid.FromEPSGNumber(7012), "Point 58");
        /// <summary> Fort Marigot  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum FortMarigot = new Datum(6621, Ellipsoid.FromEPSGNumber(7022), "Fort Marigot");
        /// <summary> Guadeloupe 1948  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Guadeloupe1948 = new Datum(6622, Ellipsoid.FromEPSGNumber(7022), "Guadeloupe 1948");
        /// <summary> Centre Spatial Guyanais 1967  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum CentreSpatialGuyanais1967 = new Datum(6623, Ellipsoid.FromEPSGNumber(7022), "Centre Spatial Guyanais 1967");
        /// <summary> Reseau Geodesique Francais Guyane 1995  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiqueFrancaisGuyane1995 = new Datum(6624, Ellipsoid.FromEPSGNumber(7019), "Reseau Geodesique Francais Guyane 1995");
        /// <summary> Martinique 1938  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Martinique1938 = new Datum(6625, Ellipsoid.FromEPSGNumber(7022), "Martinique 1938");
        /// <summary> Reunion 1947  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Reunion1947 = new Datum(6626, Ellipsoid.FromEPSGNumber(7022), "Reunion 1947");
        /// <summary> Reseau Geodesique de la Reunion 1992  </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiquedelaReunion1992 = new Datum(6627, Ellipsoid.FromEPSGNumber(7019), "Reseau Geodesique de la Reunion 1992");
        /// <summary> Tahiti 52  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tahiti52 = new Datum(6628, Ellipsoid.FromEPSGNumber(7022), "Tahiti 52");
        /// <summary> Tahaa 54  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tahaa54 = new Datum(6629, Ellipsoid.FromEPSGNumber(7022), "Tahaa 54");
        /// <summary> IGN72 Nuku Hiva  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN72NukuHiva = new Datum(6630, Ellipsoid.FromEPSGNumber(7022), "IGN72 Nuku Hiva");
        /// <summary> K0 1949  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum K01949 = new Datum(6631, Ellipsoid.FromEPSGNumber(7022), "K0 1949");
        /// <summary> Combani 1950  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Combani1950 = new Datum(6632, Ellipsoid.FromEPSGNumber(7022), "Combani 1950");
        /// <summary> IGN56 Lifou  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN56Lifou = new Datum(6633, Ellipsoid.FromEPSGNumber(7022), "IGN56 Lifou");
        /// <summary> IGN72 Grande Terre  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN72GrandeTerre = new Datum(6634, Ellipsoid.FromEPSGNumber(7022), "IGN72 Grande Terre");
        /// <summary> ST87 Ouvea  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ST87Ouvea1924 = new Datum(6635, Ellipsoid.FromEPSGNumber(7022), "ST87 Ouvea");
        /// <summary> Petrels 1972  </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Petrels1972 = new Datum(6636, Ellipsoid.FromEPSGNumber(7022), "Petrels 1972");
        /// <summary> Pointe Geologie Perroud 1950 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PointeGeologiePerroud1950 = new Datum(6637, Ellipsoid.FromEPSGNumber(7022), "Pointe Geologie Perroud 1950");
        /// <summary> Saint Pierre et Miquelon 1950 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum SaintPierreetMiquelon1950 = new Datum(6638, Ellipsoid.FromEPSGNumber(7008), "Saint Pierre et Miquelon 1950");
        /// <summary> MOP78 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum MOP78 = new Datum(6639, Ellipsoid.FromEPSGNumber(7022), "MOP78");
        /// <summary> Reseau de Reference des Antilles Francaises 1991 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum ReseaudeReferencedesAntillesFrancaises1991 = new Datum(6640, Ellipsoid.FromEPSGNumber(7030), "Reseau de Reference des Antilles Francaises 1991");
        /// <summary> IGN53 Mare </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN53Mare = new Datum(6641, Ellipsoid.FromEPSGNumber(7022), "IGN53 Mare");
        /// <summary> ST84 Ile des Pins </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ST84IledesPins = new Datum(6642, Ellipsoid.FromEPSGNumber(7022), "ST84 Ile des Pins");
        /// <summary> ST71 Belep </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ST71Belep = new Datum(6643, Ellipsoid.FromEPSGNumber(7022), "ST71 Belep");
        /// <summary> NEA74 Noumea </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum NEA74Noumea = new Datum(6644, Ellipsoid.FromEPSGNumber(7022), "NEA74 Noumea");
        /// <summary> Reseau Geodesique Nouvelle Caledonie 1991 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiqueNouvelleCaledonie1991 = new Datum(6645, Ellipsoid.FromEPSGNumber(7022), "Reseau Geodesique Nouvelle Caledonie 1991");
        /// <summary> Grand Comoros </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum GrandComoros = new Datum(6646, Ellipsoid.FromEPSGNumber(7022), "Grand Comoros");
        /// <summary> International Terrestrial Reference Frame 1988 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1988 = new Datum(6647, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1988");
        /// <summary> International Terrestrial Reference Frame 1989 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1989 = new Datum(6648, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1989");
        /// <summary> International Terrestrial Reference Frame 1990 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1990 = new Datum(6649, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1990");
        /// <summary> International Terrestrial Reference Frame 1991 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1991 = new Datum(6650, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1991");
        /// <summary> International Terrestrial Reference Frame 1992 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1992 = new Datum(6651, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1992");
        /// <summary> International Terrestrial Reference Frame 1993 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1993 = new Datum(6652, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1993");
        /// <summary> International Terrestrial Reference Frame 1994 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1994 = new Datum(6653, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1994");
        /// <summary> International Terrestrial Reference Frame 1996 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1996 = new Datum(6654, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1996");
        /// <summary> International Terrestrial Reference Frame 1997 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame1997 = new Datum(6655, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 1997");
        /// <summary> International Terrestrial Reference Frame 2000 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame2000 = new Datum(6656, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 2000");
        /// <summary> Reykjavik 1900 </summary>
        /// <remarks> This datum uses the Danish 1876 ellipsoid </remarks>
        public static readonly Datum Reykjavik1900 = new Datum(6657, Ellipsoid.FromEPSGNumber(7051), "Reykjavik 1900");
        /// <summary> Hjorsey 1955 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Hjorsey1955 = new Datum(6658, Ellipsoid.FromEPSGNumber(7022), "Hjorsey 1955");
        /// <summary> Islands Network 1993 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum IslandsNetwork1993 = new Datum(6659, Ellipsoid.FromEPSGNumber(7019), "Islands Network 1993");
        /// <summary> Helle 1954 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Helle1954 = new Datum(6660, Ellipsoid.FromEPSGNumber(7022), "Helle 1954");
        /// <summary> Latvia 1992 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Latvia1992 = new Datum(6661, Ellipsoid.FromEPSGNumber(7019), "Latvia 1992");
        /// <summary> Porto Santo 1995 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PortoSanto1995 = new Datum(6663, Ellipsoid.FromEPSGNumber(7022), "Porto Santo 1995");
        /// <summary> Azores Oriental Islands 1995 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AzoresOrientalIslands1995 = new Datum(6664, Ellipsoid.FromEPSGNumber(7022), "Azores Oriental Islands 1995");
        /// <summary> Azores Central Islands 1995 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AzoresCentralIslands1995 = new Datum(6665, Ellipsoid.FromEPSGNumber(7022), "Azores Central Islands 1995");
        /// <summary> Lisbon 1890 </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Lisbon1890 = new Datum(6666, Ellipsoid.FromEPSGNumber(7004), "Lisbon 1890");
        /// <summary> Iraq-Kuwait Boundary Datum 1992 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Iraq_KuwaitBoundaryDatum1992 = new Datum(6667, Ellipsoid.FromEPSGNumber(7030), "Iraq-Kuwait Boundary Datum 1992");
        /// <summary> European Datum 1979 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EuropeanDatum1979 = new Datum(6668, Ellipsoid.FromEPSGNumber(7022), "European Datum 1979");
        /// <summary> Istituto Geografico Militaire 1995 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum IstitutoGeograficoMilitaire1995 = new Datum(6670, Ellipsoid.FromEPSGNumber(7030), "Istituto Geografico Militaire 1995");
        /// <summary> Voirol 1879 </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Voirol1879 = new Datum(6671, Ellipsoid.FromEPSGNumber(7011), "Voirol 1879");
        /// <summary> Chatham Islands Datum 1971 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ChathamIslandsDatum1971 = new Datum(6672, Ellipsoid.FromEPSGNumber(7022), "Chatham Islands Datum 1971");
        /// <summary> Chatham Islands Datum 1979 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ChathamIslandsDatum1979 = new Datum(6673, Ellipsoid.FromEPSGNumber(7022), "Chatham Islands Datum 1979");
        /// <summary> Sistema de Referencia Geocentrico para America del Sur 2000 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum SistemadeReferenciaGeocentricoparaAmericadelSur2000 = new Datum(6674, Ellipsoid.FromEPSGNumber(7019), "Sistema de Referencia Geocentrico para America del Sur 2000");
        /// <summary> Guam 1963 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Guam1963 = new Datum(6675, Ellipsoid.FromEPSGNumber(7008), "Guam 1963");
        /// <summary> Vientiane 1982 </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Vientiane1982 = new Datum(6676, Ellipsoid.FromEPSGNumber(7024), "Vientiane 1982");
        /// <summary> Lao 1993 </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum Lao1993 = new Datum(6677, Ellipsoid.FromEPSGNumber(7024), "Lao 1993");
        /// <summary> Lao National Datum 1997 </summary>
        /// <remarks> This datum uses the Krassowsky 1940 ellipsoid </remarks>
        public static readonly Datum LaoNationalDatum1997 = new Datum(6678, Ellipsoid.FromEPSGNumber(7024), "Lao National Datum 1997");
        /// <summary> Jouik 1961 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Jouik1961 = new Datum(6679, Ellipsoid.FromEPSGNumber(7012), "Jouik 1961");
        /// <summary> Nouakchott 1965 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Nouakchott1965 = new Datum(6680, Ellipsoid.FromEPSGNumber(7012), "Nouakchott 1965");
        /// <summary> Mauritania 1999 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Mauritania1999_RGS = new Datum(6681, Ellipsoid.FromEPSGNumber(7012), "Mauritania 1999 (Clark1980 RGS)");
        /// <summary> Gulshan 303 </summary>
        /// <remarks> This datum uses the Everest 1830 (1937 Adjustment) ellipsoid </remarks>
        public static readonly Datum Gulshan303 = new Datum(6682, Ellipsoid.FromEPSGNumber(7015), "Gulshan 303");
        /// <summary> Philippine Reference System 1992 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum PhilippineReferenceSystem1992 = new Datum(6683, Ellipsoid.FromEPSGNumber(7008), "Philippine Reference System 1992");
        /// <summary> Gan 1970 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Gan1970 = new Datum(6684, Ellipsoid.FromEPSGNumber(7022), "Gan 1970");
        /// <summary> Gandajika </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Gandajika = new Datum(6685, Ellipsoid.FromEPSGNumber(7022), "Gandajika");
        /// <summary> Marco Geocentrico Nacional de Referencia </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum MarcoGeocentricoNacionaldeReferencia = new Datum(6686, Ellipsoid.FromEPSGNumber(7019), "Marco Geocentrico Nacional de Referencia");
        /// <summary> Reseau Geodesique de la Polynesie Francaise </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiquedelaPolynesieFrancaise = new Datum(6687, Ellipsoid.FromEPSGNumber(7019), "Reseau Geodesique de la Polynesie Francaise");
        /// <summary> Fatu Iva 72 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum FatuIva72 = new Datum(6688, Ellipsoid.FromEPSGNumber(7022), "Fatu Iva 72");
        /// <summary> IGN63 Hiva Oa </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN63HivaOa = new Datum(6689, Ellipsoid.FromEPSGNumber(7022), "IGN63 Hiva Oa");
        /// <summary> Tahiti 79 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tahiti79 = new Datum(6690, Ellipsoid.FromEPSGNumber(7022), "Tahiti 79");
        /// <summary> Moorea 87 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Moorea87 = new Datum(6691, Ellipsoid.FromEPSGNumber(7022), "Moorea 87");
        /// <summary> Maupiti 83 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Maupiti83 = new Datum(6692, Ellipsoid.FromEPSGNumber(7022), "Maupiti 83");
        /// <summary> Nakhl-e Ghanem </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Nakhl_eGhanem = new Datum(6693, Ellipsoid.FromEPSGNumber(7030), "Nakhl-e Ghanem");
        /// <summary> Posiciones Geodesicas Argentinas 1994 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum PosicionesGeodesicasArgentinas1994 = new Datum(6694, Ellipsoid.FromEPSGNumber(7019), "Posiciones Geodesicas Argentinas 1994");
        /// <summary> Katanga 1955 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum Katanga1955 = new Datum(6695, Ellipsoid.FromEPSGNumber(7008), "Katanga 1955");
        /// <summary> Kasai 1953 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Kasai1953 = new Datum(6696, Ellipsoid.FromEPSGNumber(7012), "Kasai 1953");
        /// <summary> IGC 1962 Arc of the 6th Parallel South </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum IGC1962Arcofthe6thParallelSouth = new Datum(6697, Ellipsoid.FromEPSGNumber(7012), "IGC 1962 Arc of the 6th Parallel South");
        /// <summary> IGN 1962 Kerguelen </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IGN1962Kerguelen = new Datum(6698, Ellipsoid.FromEPSGNumber(7022), "IGN 1962 Kerguelen");
        /// <summary> Le Pouce 1934 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum LePouce1934 = new Datum(6699, Ellipsoid.FromEPSGNumber(7012), "Le Pouce 1934");
        /// <summary> IGN Astro 1960 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum IGNAstro1960 = new Datum(6700, Ellipsoid.FromEPSGNumber(7012), "IGN Astro 1960");
        /// <summary> Institut Geographique du Congo Belge 1955 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum InstitutGeographiqueduCongoBelge1955 = new Datum(6701, Ellipsoid.FromEPSGNumber(7012), "Institut Geographique du Congo Belge 1955");
        /// <summary> Mauritania 1999 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Mauritania1999 = new Datum(6702, Ellipsoid.FromEPSGNumber(7019), "Mauritania 1999");
        /// <summary> Missao Hidrografico Angola y Sao Tome 1951 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum MissaoHidrograficoAngolaySaoTome1951 = new Datum(6703, Ellipsoid.FromEPSGNumber(7012), "Missao Hidrografico Angola y Sao Tome 1951");
        /// <summary> Mhast (onshore) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Mhast_onshore = new Datum(6704, Ellipsoid.FromEPSGNumber(7022), "Mhast (onshore)");
        /// <summary> Mhast (offshore) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Mhast_offshore = new Datum(6705, Ellipsoid.FromEPSGNumber(7022), "Mhast (offshore)");
        /// <summary> Egypt Gulf of Suez S-650 TL </summary>
        /// <remarks> This datum uses the Helmert 1906 ellipsoid </remarks>
        public static readonly Datum EgyptGulfofSuezS_650TL = new Datum(6706, Ellipsoid.FromEPSGNumber(7020), "Egypt Gulf of Suez S-650 TL");
        /// <summary> Tern Island 1961 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum TernIsland1961 = new Datum(6707, Ellipsoid.FromEPSGNumber(7022), "Tern Island 1961");
        /// <summary> Cocos Islands 1965 </summary>
        /// <remarks> This datum uses the Australian National Spheroid ellipsoid </remarks>
        public static readonly Datum CocosIslands1965 = new Datum(6708, Ellipsoid.FromEPSGNumber(7003), "Cocos Islands 1965");
        /// <summary>Represents the Anna 1 Astro datum of 1965.</summary>
        public static readonly Datum Anna1Astro1965 = CocosIslands1965;
        /// <summary> Iwo Jima 1945 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum IwoJima1945 = new Datum(6709, Ellipsoid.FromEPSGNumber(7022), "Iwo Jima 1945");
        /// <summary>Represents the Astro Beacon "E" datum of 1945.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum AstroBeaconE1945 = IwoJima1945;
        /// <summary> St. Helena 1971 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum StHelena1971 = new Datum(6710, Ellipsoid.FromEPSGNumber(7022), "St. Helena 1971");
        /// <summary>Represents the Astro DOS 71/4 datum.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum AstroDos714 = StHelena1971;
        /// <summary> Marcus Island 1952 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum MarcusIsland1952 = new Datum(6711, Ellipsoid.FromEPSGNumber(7022), "Marcus Island 1952");
        /// <summary>Represents the Astronomical Station datum of 1952.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum AstronomicalStation1952 = MarcusIsland1952;
        /// <summary> Ascension Island 1958 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum AscensionIsland1958 = new Datum(6712, Ellipsoid.FromEPSGNumber(7022), "Ascension Island 1958");
        /// <summary> Ayabelle Lighthouse </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum AyabelleLighthouse = new Datum(6713, Ellipsoid.FromEPSGNumber(7012), "Ayabelle Lighthouse");
        /// <summary> Bellevue </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Bellevue = new Datum(6714, Ellipsoid.FromEPSGNumber(7022), "Bellevue");
        /// <summary> Camp Area Astro </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum CampAreaAstro = new Datum(6715, Ellipsoid.FromEPSGNumber(7022), "Camp Area Astro");
        /// <summary> Phoenix Islands 1966 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PhoenixIslands1966 = new Datum(6716, Ellipsoid.FromEPSGNumber(7022), "Phoenix Islands 1966");
        /// <summary>Represents the Canton Astro datum of 1966.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum CantonAstro1966 = PhoenixIslands1966;
        /// <summary> Cape Canaveral </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum CapeCanaveral = new Datum(6717, Ellipsoid.FromEPSGNumber(7008), "Cape Canaveral");
        /// <summary> Solomon 1968 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Solomon1968 = new Datum(6718, Ellipsoid.FromEPSGNumber(7022), "Solomon 1968");
        /// <summary>Represents the DOS datum of 1968.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum Dos1968 = Solomon1968;
        /// <summary>Represents the GUX 1 Astro datum.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum Gux1Astro = Solomon1968;
        /// <summary> Easter Island 1967 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum EasterIsland1967 = new Datum(6719, Ellipsoid.FromEPSGNumber(7022), "Easter Island 1967");
        /// <summary> Fiji Geodetic Datum 1986 </summary>
        /// <remarks> This datum uses the WGS 72 ellipsoid </remarks>
        public static readonly Datum FijiGeodeticDatum1986 = new Datum(6720, Ellipsoid.FromEPSGNumber(7043), "Fiji Geodetic Datum 1986");
        /// <summary> Fiji 1956 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Fiji1956 = new Datum(6721, Ellipsoid.FromEPSGNumber(7022), "Fiji 1956");
        /// <summary> South Georgia 1968 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum SouthGeorgia1968 = new Datum(6722, Ellipsoid.FromEPSGNumber(7022), "South Georgia 1968");
        /// <summary>Represents the ISTS 061 Astro datum of 1968.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum Ists061Astro1968 = SouthGeorgia1968;
        /// <summary> Grand Cayman 1959 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum GrandCayman1959 = new Datum(6723, Ellipsoid.FromEPSGNumber(7008), "Grand Cayman 1959");
        /// <summary> Diego Garcia 1969 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum DiegoGarcia1969 = new Datum(6724, Ellipsoid.FromEPSGNumber(7022), "Diego Garcia 1969");
        /// <summary>Represents the ISTS 073 Astro datum of 1969.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum Ists073Astro1969 = DiegoGarcia1969;
        /// <summary> Johnston Island 1961 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum JohnstonIsland1961 = new Datum(6725, Ellipsoid.FromEPSGNumber(7022), "Johnston Island 1961");
        /// <summary> Little Cayman 1961 </summary>
        /// <remarks> This datum uses the Clarke 1866 ellipsoid </remarks>
        public static readonly Datum LittleCayman1961 = new Datum(6726, Ellipsoid.FromEPSGNumber(7008), "Little Cayman 1961");
        /// <summary>Represents the L. C. 5 Astro datum of 1961.</summary>
        public static readonly Datum LC5Astro1961 = LittleCayman1961;
        /// <summary> Midway 1961 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Midway1961 = new Datum(6727, Ellipsoid.FromEPSGNumber(7022), "Midway 1961");
        /// <summary> Pico de la Nieves </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum PicodelaNieves = new Datum(6728, Ellipsoid.FromEPSGNumber(7022), "Pico de la Nieves");
        /// <summary> Pitcairn 1967 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Pitcairn1967 = new Datum(6729, Ellipsoid.FromEPSGNumber(7022), "Pitcairn 1967");
        /// <summary> Santo 1965 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Santo1965 = new Datum(6730, Ellipsoid.FromEPSGNumber(7022), "Santo 1965");
        /// <summary> Viti Levu 1916 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum VitiLevu1916 = new Datum(6731, Ellipsoid.FromEPSGNumber(7012), "Viti Levu 1916");
        /// <summary> Marshall Islands 1960 </summary>
        /// <remarks> This datum uses the Hough 1960 ellipsoid </remarks>
        public static readonly Datum MarshallIslands1960 = new Datum(6732, Ellipsoid.FromEPSGNumber(7053), "Marshall Islands 1960");
        /// <summary> Wake Island 1952 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum WakeIsland1952 = new Datum(6733, Ellipsoid.FromEPSGNumber(7022), "Wake Island 1952");
        /// <summary> Tristan 1968 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tristan1968 = new Datum(6734, Ellipsoid.FromEPSGNumber(7022), "Tristan 1968");
        /// <summary> Kusaie 1951 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Kusaie1951 = new Datum(6735, Ellipsoid.FromEPSGNumber(7022), "Kusaie 1951");
        /// <summary> Deception Island </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum DeceptionIsland = new Datum(6736, Ellipsoid.FromEPSGNumber(7012), "Deception Island");
        /// <summary> Geocentric datum of Korea </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum GeocentricdatumofKorea = new Datum(6737, Ellipsoid.FromEPSGNumber(7019), "Geocentric datum of Korea");
        /// <summary> Hong Kong 1963 </summary>
        /// <remarks> This datum uses the Clarke 1858 ellipsoid </remarks>
        public static readonly Datum HongKong1963 = new Datum(6738, Ellipsoid.FromEPSGNumber(7007), "Hong Kong 1963");
        /// <summary> Hong Kong 1963(67) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum HongKong1963_67 = new Datum(6739, Ellipsoid.FromEPSGNumber(7022), "Hong Kong 1963(67)");
        /// <summary> Parametrop Zemp 1990 </summary>
        /// <remarks> This datum uses the PZ-90 ellipsoid </remarks>
        public static readonly Datum ParametropZemp1990 = new Datum(6740, Ellipsoid.FromEPSGNumber(7054), "Parametrop Zemp 1990");
        /// <summary> Faroe Datum 1954 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum FaroeDatum1954 = new Datum(6741, Ellipsoid.FromEPSGNumber(7022), "Faroe Datum 1954");
        /// <summary> Geodetic Datum of Malaysia 2000 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum GeodeticDatumofMalaysia2000 = new Datum(6742, Ellipsoid.FromEPSGNumber(7019), "Geodetic Datum of Malaysia 2000");
        /// <summary> Karbala 1979 (Polservice) </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Karbala1979_Polservice = new Datum(6743, Ellipsoid.FromEPSGNumber(7012), "Karbala 1979 (Polservice)");
        /// <summary> Nahrwan 1934 </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum Nahrwan1934 = new Datum(6744, Ellipsoid.FromEPSGNumber(7012), "Nahrwan 1934");
        /// <summary> Rauenberg Datum/83 </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum RauenbergDatum_83 = new Datum(6745, Ellipsoid.FromEPSGNumber(7004), "Rauenberg Datum/83");
        /// <summary> Potsdam Datum/83 </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum PotsdamDatum_83 = new Datum(6746, Ellipsoid.FromEPSGNumber(7004), "Potsdam Datum/83");
        /// <summary> Greenland 1996 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum Greenland1996 = new Datum(6747, Ellipsoid.FromEPSGNumber(7019), "Greenland 1996");
        /// <summary> Vanua Levu 1915 </summary>
        /// <remarks> This datum uses the Clarke 1880 (international foot) ellipsoid </remarks>
        public static readonly Datum VanuaLevu1915 = new Datum(6748, Ellipsoid.FromEPSGNumber(7055), "Vanua Levu 1915");
        /// <summary> Reseau Geodesique de Nouvelle Caledonie 91-93 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum ReseauGeodesiquedeNouvelleCaledonie91_93 = new Datum(6749, Ellipsoid.FromEPSGNumber(7019), "Reseau Geodesique de Nouvelle Caledonie 91-93");
        /// <summary> ST87 Ouvea </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum ST87Ouvea = new Datum(6750, Ellipsoid.FromEPSGNumber(7030), "ST87 Ouvea");
        /// <summary> Kertau (RSO) </summary>
        /// <remarks> This datum uses the Everest 1830 (RSO 1969) ellipsoid </remarks>
        public static readonly Datum Kertau_RSO = new Datum(6751, Ellipsoid.FromEPSGNumber(7056), "Kertau (RSO)");
        /// <summary>Represents the Kertau datum of 1948.</summary>
        public static readonly Datum Kertau1948 = Kertau_RSO;
        /// <summary> Viti Levu 1912 </summary>
        /// <remarks> This datum uses the Clarke 1880 (international foot) ellipsoid </remarks>
        public static readonly Datum VitiLevu1912 = new Datum(6752, Ellipsoid.FromEPSGNumber(7055), "Viti Levu 1912");
        /// <summary> fk89 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum fk89 = new Datum(6753, Ellipsoid.FromEPSGNumber(7022), "fk89");
        /// <summary> Libyan Geodetic Datum 2006 </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum LibyanGeodeticDatum2006 = new Datum(6754, Ellipsoid.FromEPSGNumber(7022), "Libyan Geodetic Datum 2006");
        /// <summary> Datum Geodesi Nasional 1995 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum DatumGeodesiNasional1995 = new Datum(6755, Ellipsoid.FromEPSGNumber(7030), "Datum Geodesi Nasional 1995");
        /// <summary> Vietnam 2000 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Vietnam2000 = new Datum(6756, Ellipsoid.FromEPSGNumber(7030), "Vietnam 2000");
        /// <summary> SVY21 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum SVY21 = new Datum(6757, Ellipsoid.FromEPSGNumber(7030), "SVY21");
        /// <summary> Jamaica 2001 </summary>
        /// <remarks> This datum uses the WGS 84 ellipsoid </remarks>
        public static readonly Datum Jamaica2001 = new Datum(6758, Ellipsoid.FromEPSGNumber(7030), "Jamaica 2001");
        /// <summary> CH1903 (Bern) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum CH1903_Bern = new Datum(6801, Ellipsoid.FromEPSGNumber(7004), 7.439583333, "CH1903 (Bern)");
        /// <summary> Bogota 1975 (Bogota) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Bogota1975_Bogota = new Datum(6802, Ellipsoid.FromEPSGNumber(7022), 74.08091667, "Bogota 1975 (Bogota)");
        /// <summary> Lisbon 1937 (Lisbon) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Lisbon1937_Lisbon = new Datum(6803, Ellipsoid.FromEPSGNumber(7022), -9.131906111, "Lisbon 1937 (Lisbon)");
        /// <summary> Makassar (Jakarta) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Makassar_Jakarta = new Datum(6804, Ellipsoid.FromEPSGNumber(7004), 106.8077194, "Makassar (Jakarta)");
        /// <summary> Militar-Geographische Institut (Ferro) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Militar_GeographischeInstitut_Ferro = new Datum(6805, Ellipsoid.FromEPSGNumber(7004), -17.66666667, "Militar-Geographische Institut (Ferro)");
        /// <summary> Monte Mario (Rome) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum MonteMario_Rome = new Datum(6806, Ellipsoid.FromEPSGNumber(7022), 12.45233333, "Monte Mario (Rome)");
        /// <summary> Nouvelle Triangulation Francaise (Paris) </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum NouvelleTriangulationFrancaise_Paris = new Datum(6807, Ellipsoid.FromEPSGNumber(7011), 2.337291667, "Nouvelle Triangulation Francaise (Paris)");
        /// <summary> Padang 1884 (Jakarta) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Padang1884_Jakarta = new Datum(6808, Ellipsoid.FromEPSGNumber(7004), 106.8077194, "Padang 1884 (Jakarta)");
        /// <summary> Reseau National Belge 1950 (Brussels) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum ReseauNationalBelge1950_Brussels = new Datum(6809, Ellipsoid.FromEPSGNumber(7022), 4.367975, "Reseau National Belge 1950 (Brussels)");
        /// <summary> Tananarive 1925 (Paris) </summary>
        /// <remarks> This datum uses the International 1924 ellipsoid </remarks>
        public static readonly Datum Tananarive1925_Paris = new Datum(6810, Ellipsoid.FromEPSGNumber(7022), 2.337291667, "Tananarive 1925 (Paris)");
        /// <summary> Voirol 1875 (Paris) </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Voirol1875_Paris = new Datum(6811, Ellipsoid.FromEPSGNumber(7011), 2.337291667, "Voirol 1875 (Paris)");
        /// <summary> Batavia (Jakarta) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Batavia_Jakarta = new Datum(6813, Ellipsoid.FromEPSGNumber(7004), 106.8077194, "Batavia (Jakarta)");
        /// <summary> Stockholm 1938 (Stockholm) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Stockholm1938_Stockholm = new Datum(6814, Ellipsoid.FromEPSGNumber(7004), 18.05827778, "Stockholm 1938 (Stockholm)");
        /// <summary> Greek (Athens) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Greek_Athens = new Datum(6815, Ellipsoid.FromEPSGNumber(7004), 23.7163375, "Greek (Athens)");
        /// <summary> Carthage (Paris) </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Carthage_Paris = new Datum(6816, Ellipsoid.FromEPSGNumber(7011), 2.337291667, "Carthage (Paris)");
        /// <summary> NGO 1948 (Oslo) </summary>
        /// <remarks> This datum uses the Bessel Modified ellipsoid </remarks>
        public static readonly Datum NGO1948_Oslo = new Datum(6817, Ellipsoid.FromEPSGNumber(7005), 10.72291667, "NGO 1948 (Oslo)");
        /// <summary> S-JTSK (Ferro) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum S_JTSK_Ferro = new Datum(6818, Ellipsoid.FromEPSGNumber(7004), -17.66666667, "S-JTSK (Ferro)");
        /// <summary> Nord Sahara 1959 (Paris) </summary>
        /// <remarks> This datum uses the Clarke 1880 (RGS) ellipsoid </remarks>
        public static readonly Datum NordSahara1959_Paris = new Datum(6819, Ellipsoid.FromEPSGNumber(7012), 2.337291667, "Nord Sahara 1959 (Paris)");
        /// <summary> Gunung Segara (Jakarta) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum GunungSegara_Jakarta = new Datum(6820, Ellipsoid.FromEPSGNumber(7004), 106.8077194, "Gunung Segara (Jakarta)");
        /// <summary> Voirol 1879 (Paris) </summary>
        /// <remarks> This datum uses the Clarke 1880 (IGN) ellipsoid </remarks>
        public static readonly Datum Voirol1879_Paris = new Datum(6821, Ellipsoid.FromEPSGNumber(7011), 2.337291667, "Voirol 1879 (Paris)");
        /// <summary> International Terrestrial Reference Frame 2005 </summary>
        /// <remarks> This datum uses the GRS 1980 ellipsoid </remarks>
        public static readonly Datum InternationalTerrestrialReferenceFrame2005 = new Datum(6896, Ellipsoid.FromEPSGNumber(7019), "International Terrestrial Reference Frame 2005");
        /// <summary> Ancienne Triangulation Francaise (Paris) </summary>
        /// <remarks> This datum uses the Plessis 1817 ellipsoid </remarks>
        public static readonly Datum AncienneTriangulationFrancaise_Paris = new Datum(6901, Ellipsoid.FromEPSGNumber(7027), 2.337291667, "Ancienne Triangulation Francaise (Paris)");
        /// <summary> Nord de Guerre (Paris) </summary>
        /// <remarks> This datum uses the Plessis 1817 ellipsoid </remarks>
        public static readonly Datum NorddeGuerre_Paris = new Datum(6902, Ellipsoid.FromEPSGNumber(7027), 2.337291667, "Nord de Guerre (Paris)");
        /// <summary> Madrid 1870 (Madrid) </summary>
        /// <remarks> This datum uses the Struve 1860 ellipsoid </remarks>
        public static readonly Datum Madrid1870_Madrid = new Datum(6903, Ellipsoid.FromEPSGNumber(7028), -3.687938889, "Madrid 1870 (Madrid)");
        /// <summary> Lisbon 1890 (Lisbon) </summary>
        /// <remarks> This datum uses the Bessel 1841 ellipsoid </remarks>
        public static readonly Datum Lisbon1890_Lisbon = new Datum(6904, Ellipsoid.FromEPSGNumber(7004), -9.131906111, "Lisbon 1890 (Lisbon)");

        #endregion

        #region Non-EPSG Datums

        /// <summary>Represents the Estonia Coordinate System datum of 1937.</summary>
        public static readonly Datum EstoniaLocalDatum1937 = new Datum("Estonia Coordinate System 1937", Ellipsoid.Bessel1841);
        /// <summary>Represents the Old Egyptian datum of 1907.</summary>
        public static readonly Datum OldEgyptian1907 = new Datum("Old Egyptian 1907", Ellipsoid.Helmert1906);
        /// <summary>Represents the Oman datum.</summary>
        /// <remarks>This datum uses the Clarke ellipsoid of 1880.</remarks>
        public static readonly Datum Oman = new Datum("Oman", Ellipsoid.Clarke1880);
        /// <summary>Represents the Pointe Noire datum.</summary>
        /// <remarks>This datum uses the Clarke ellipsoid of 1880.</remarks>
        public static readonly Datum PointeNoire1948 = new Datum("Pointe Noire 1948", Ellipsoid.Clarke1880);
        /// <summary>Represents the Rome datum of 1940.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum Rome1940 = new Datum("Rome 1940", Ellipsoid.International1924);
        /// <summary>Represents the Sao Braz datum.</summary>
        /// <remarks>This datum uses the International ellipsoid of 1924.</remarks>
        public static readonly Datum SaoBraz = new Datum("Sao Braz", Ellipsoid.International1924);
        /// <summary>Represents the South Asia datum.</summary>
        public static readonly Datum SouthAsia = new Datum("South Asia", Ellipsoid.ModifiedFischer1960);
        /// <summary>Represents the Voirol datum of 1960.</summary>
        /// <remarks>This datum uses the Clarke ellipsoid of 1880.</remarks>
        public static readonly Datum Voirol1960 = new Datum("Voirol 1960", Ellipsoid.Clarke1880);
        /// <summary>Represents the Wake Eniwetok datum of 1960.</summary>
        public static readonly Datum WakeEniwetok1960 = new Datum("Wake Eniwetok 1960", Ellipsoid.Hough1960);

        public static readonly Datum WorldGeodeticSystem1960 = new Datum("World Geodetic System 1960", Ellipsoid.Wgs1960);
        public static readonly Datum WorldGeodeticSystem1966 = new Datum("World Geodetic System 1966", Ellipsoid.Wgs1966);

        #endregion

        /* IMPORTANT: The Default field must be after Datum.WorldGeodeticSystem1984 is initialized, otherwise it will be null! */

        /// <summary>Represents the default datum used by the DotSpatial.Positioning, WGS1984.</summary>
        public static readonly Datum Default = Datum.WorldGeodeticSystem1984;

        #endregion

        #region Constructors

        // Used for XML deserialization
        private Datum()
        { }

        /// <summary>Creates a new instance using the specified name and reference ellipsoid.</summary>
        /// <param name="name">A <strong>String</strong> containing the user-friendly name for the 
        /// datum.</param>
        /// <param name="ellipsoid">
        /// An <strong>Ellipsoid</strong> describing the shape of the Earth, used as a basis
        /// for the datum.
        /// </param>
        public Datum(string name, Ellipsoid ellipsoid)
        {
            _Ellipsoid = ellipsoid;
            _Name = name;
            _PrimeMeridian = Longitude.Empty;

            this.SanityCheck();

            // Add the datum to the collection
            _Datums.Add(this);
        }

        /// <summary>
        /// Internal constructor for static list generation
        /// </summary>
        /// <param name="epsgNumber"> The ID number for this ellipsoid in the EPSG database </param>
        /// <param name="ellipsoid"> The ellipsoid used by this datum </param>
        /// <param name="name"> The name of the datum </param>
        internal Datum(int epsgNumber, Ellipsoid ellipsoid, string name)
        {
            _EPSGNumber = epsgNumber;
            _Ellipsoid = ellipsoid;
            _Name = name;
            _PrimeMeridian = Longitude.Empty;

            this.SanityCheck();

            // Add the datum to the collection
            _EPSGDatums.Add(this);

        }

        /// <summary>
        /// Internal constructor for static list generation
        /// </summary>
        /// <param name="epsgNumber"> The ID number for this ellipsoid in the EPSG database </param>
        /// <param name="ellipsoid"> The ellipsoid used by this datum </param>
        /// <param name="meridian"> The adjustment to latitude coordinates for datums using a central meridian other than Greenwich </param>
        /// <param name="name"> The name of the datum </param>
        internal Datum(int epsgNumber, Ellipsoid ellipsoid, double meridian, string name)
        {
            _EPSGNumber = epsgNumber;
            _Ellipsoid = ellipsoid;
            _Name = name;
            _PrimeMeridian = new Longitude(meridian);

            this.SanityCheck();

            // Add the datum to the collection
            _EPSGDatums.Add(this);

        }

        /// <summary>
        /// Validates the datum. Called in the constructor.
        /// </summary>
        private void SanityCheck()
        {
            if (_Ellipsoid == null)
                throw new ArgumentException("Datum constructiopn failed. Ellipsoid is null or invalid");
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Returns the name of the datum.
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// European Petroleum Survey Group number for this datum. The ESPG standards are now maintained by OGP
        /// (International Association of Oil and Gas Producers).
        /// </summary>
        public int EPSGNumber
        {
            get { return _EPSGNumber; }
        }

        /// <summary>Returns the interpretation of Earth's shape associated with a datum.</summary>
        /// <value>
        /// Read only. An
        /// <see cref="Ellipsoid">Ellipsoid</see>
        /// object.
        /// </value>
        /// <remarks>Each datum is associated with an ellipsoid, which is an interpretation of Earth's shape and 
        /// size.</remarks>
        /// <example>
        ///     This example gets information on the ellipsoid associated with the WGS84 datum. 
        ///     <code lang="VB">
        /// ' Get information about the NAD1983 datum
        /// Dim MyDatum As Datum = Geodesy.GetDatum(DatumType.NorthAmerican1983)
        /// ' Get the ellipsoid associated with this datum
        /// Dim MyEllipsoid As Ellipsoid = MyDatum.Ellipsoid
        /// ' Write the semi-major axis of the ellipsoid
        /// Debug.WriteLine(MyEllipsoid.SemiMajorAxis.ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Get information about the NAD1983 datum
        /// Datum MyDatum = Geodesy.GetDatum(DatumType.NorthAmerican1983);
        /// // Get the ellipsoid associated with this datum
        /// Ellipsoid MyEllipsoid = MyDatum.Ellipsoid;
        /// // Write the semi-major axis of the ellipsoid
        /// Debug.WriteLine(MyEllipsoid.SemiMajorAxis.ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="Ellipsoid">Ellipsoid Class</seealso>
        public Ellipsoid Ellipsoid
        {
            get
            {
                return _Ellipsoid;
            }
        }

        /// <summary>
        /// Returns the prime meridian assocated with this Datum. 
        /// </summary>
        /// <remarks>
        /// Most datums use Greenwich as the prime meridian. However, several systems offset coordinates
        /// using a local meridian. This value reflects that usage.
        /// </remarks>
        public Longitude PrimeMeridian
        {
            get { return this._PrimeMeridian; }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return _Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Datum)
                return Equals((Datum)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _Ellipsoid.GetHashCode() ^ _Name.GetHashCode();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a Datum object matching the specified name.
        /// </summary>
        /// <param name="name">A <strong>String</strong> describing the name of an existing datum.</param>
        /// <returns>A <strong>Datum</strong> object matching the specified string, or null if no datum was found.</returns>
        public static Datum FromName(string name)
        {
            // Search the custom objects
            for (int index = 0; index < _Datums.Count; index++)
            {
                Datum item = _Datums[index];
                if (item.Name == name)
                    return item;
            }
            // Search the EPSG objects
            for (int index = 0; index < _EPSGDatums.Count; index++)
            {
                Datum item = _Datums[index];
                if (item.Name == name)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Returns the datum corresponding to the EPSG code
        /// </summary>
        /// <param name="epsgNumber"></param>
        /// <returns></returns>
        public static Datum FromEPSGNumber(int epsgNumber)
        {
            // Search the EPSG objects
            for (int index = 0; index < _EPSGDatums.Count; index++)
            {
                Datum item = _EPSGDatums[index];
                if (item.EPSGNumber == epsgNumber)
                    return item;
            }
            return null;
        }

        #endregion

        #region IEquatable<Datum> Members

        /// <summary>
        /// Compares the current instance to the specified datum object.
        /// </summary>
        public bool Equals(Datum other)
        {
            return this._Ellipsoid.Equals(other.Ellipsoid) 
                && this._PrimeMeridian.Equals(other.PrimeMeridian);
        }

        #endregion
    }

}