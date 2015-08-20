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
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a flattened sphere which approximates Earth's size and shape.
    /// </summary>
    /// <remarks><para>Mathematics involving points on Earth's surface are difficult to perform with
    /// precision because the Earth's surface is rugged. In order to maximize precision,
    /// scientists developed "ellipsoids," smooth ellipsoidal shapes (known as "oblate
    /// spheriods" or flattened spheres) which attempt to approximate Earth's exact shape.
    /// Like datums, ellipsoids have been subject to frequent revisions thanks to advances
    /// in technology, yet countries cannot quickly abandon outdated ellipsoids because so
    /// much infrastructure is built upon them. As a result, multiple ellipsoids are
    /// tracked and utilized when converting coordinates from one locale to another. Today,
    /// there are approximately thirty known ellipsoids upon which an estimated 120
    /// individual coordinate systems are built.</para>
    ///   <para>This class is typically used during coordinate conversion to convert from one
    /// interpretation of Earth's shape to another. All known worldwide ellipsoids such as
    /// WGS84 and Clarke 1880 are provided as static (Shared in Visual Basic) fields. Most
    /// developers will not have to use this class until coordinates must be plotted on a
    /// map. For most purposes, using the default ellipsoid of WGS84 is sufficient.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be set via constructors).</para></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Ellipsoid : IEquatable<Ellipsoid>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private int _epsgNumber = 32767;
        /// <summary>
        ///
        /// </summary>
        private Distance _equatorialRadius;
        /// <summary>
        ///
        /// </summary>
        private double _equatorialRadiusMeters; // Cached for frequent use during calculations
        /// <summary>
        ///
        /// </summary>
        private Distance _polarRadius;
        /// <summary>
        ///
        /// </summary>
        private double _polarRadiusMeters;      // Cached for frequent use during calculations
        /// <summary>
        ///
        /// </summary>
        private string _name;
        /// <summary>
        ///
        /// </summary>
        private double _flattening;
        /// <summary>
        ///
        /// </summary>
        private double _inverseFlattening;
        /// <summary>
        ///
        /// </summary>
        private double _eccentricity;
        /// <summary>
        ///
        /// </summary>
        private double _eccentricitySquared;

        /// <summary>
        ///
        /// </summary>
        private static readonly List<Ellipsoid> _ellipsoids = new List<Ellipsoid>(32);
        /// <summary>
        ///
        /// </summary>
        private static readonly List<Ellipsoid> _epsgEllipsoids = new List<Ellipsoid>(32);

        #region Fields

        #region EPSG Ellipsoids

        /// <summary>
        /// Represents the Airy ellipsoid of 1830.
        /// </summary>
        public static readonly Ellipsoid Airy1830 = new Ellipsoid(7001, 6377563.396, 299.3249646, 0, "Airy 1830");
        /// <summary>
        /// Represents the Modified Airy ellipsoid.
        /// </summary>
        public static readonly Ellipsoid AiryModified1949 = new Ellipsoid(7002, 6377340.189, 299.3249646, 0, "Airy Modified 1849");
        /// <summary>
        /// Represents the Australian National ellipsoid of 1965.
        /// </summary>
        public static readonly Ellipsoid AustralianNational1965 = new Ellipsoid(7003, 6378160, 298.25, 0, "Australian National Spheroid");
        /// <summary>
        /// Represents the Bessel ellipsoid of 1841.
        /// </summary>
        public static readonly Ellipsoid Bessel1841 = new Ellipsoid(7004, 6377397.155, 299.1528128, 0, "Bessel 1841");
        /// <summary>
        /// Represents the Bessel Modified ellipsoid of 1841.
        /// </summary>
        public static readonly Ellipsoid Bessel1841Mod = new Ellipsoid(7005, 6377492.018, 299.1528128, 0, "Bessel Modified");
        /// <summary>
        /// Represents the Bessel (Namibia) ellipsoid of 1841.
        /// </summary>
        public static readonly Ellipsoid Bessel1841Namibia = new Ellipsoid(7006, 6377483.865, 299.1528128, 0, "Bessel Namibia");
        /// <summary>
        /// Represents the Clarke ellipsoid of 1858.
        /// </summary>
        public static readonly Ellipsoid Clarke1858 = new Ellipsoid(7007, 20926348 * 0.3047972651151, 0, 20855233 * 0.3047972651151, "Clarke 1858");
        /// <summary>
        /// Represents the Clarke ellipsoid of 1866.
        /// </summary>
        public static readonly Ellipsoid Clarke1866 = new Ellipsoid(7008, 6378206.4, 0, 6356583.8, "Clarke 1866");
        /// <summary>
        /// Represents the Clarke (Michigan) ellipsoid of 1866.
        /// </summary>
        public static readonly Ellipsoid Clarke1866Michigan = new Ellipsoid(7009, 20926631.53 * 0.3048006096012, 0, 20855688.67 * 0.3048006096012, "Clarke 1866 Michigan");
        /// <summary>
        /// Represents the Clarke (Benoit) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880Benoit = new Ellipsoid(7010, 6378300.789, 0, 6356566.435, "Clarke 1880 (Benoit)");
        /// <summary>
        /// Represents the Clarke (IGN) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880IGN = new Ellipsoid(7011, 6378249.2, 0, 6356515, "Clarke 1880 (IGN)");
        /// <summary>
        /// Represents the Clarke (RGS) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880RGS = new Ellipsoid(7012, 6378249.145, 293.465, 0, "Clarke 1880 (RGS)");
        /// <summary>
        /// Represents the Clarke (Arc) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880Arc = new Ellipsoid(7013, 6378249.145, 293.4663077, 0, "Clarke 1880 (Arc)");
        /// <summary>
        /// Represents the Clarke (SGA 1822) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880SGA = new Ellipsoid(7014, 6378249.2, 293.46598, 0, "Clarke 1880 (SGA 1922)");
        /// <summary>
        /// Represents the Everest (1937 Adjustment) ellipsoid of 1830.
        /// </summary>
        public static readonly Ellipsoid Everest1937 = new Ellipsoid(7015, 6377276.345, 300.8017, 0, "Everest 1830 (1937 Adjustment)");
        /// <summary>
        /// Represents the Everest (1967 Definition) ellipsoid of 1830.
        /// </summary>
        public static readonly Ellipsoid Everest1967 = new Ellipsoid(7016, 6377298.556, 300.8017, 0, "Everest 1830 (1967 Definition)");
        //No 7017 in EPSG
        /// <summary>
        /// Represents the Everest (Modified 1948) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Everest1830Modified = new Ellipsoid(7018, 6377304.063, 300.8017, 0, "Everest 1830 Modified");
        /// <summary>
        /// Represents the Geodetic Reference System ellipsoid of 1980.
        /// </summary>
        public static readonly Ellipsoid Grs80 = new Ellipsoid(7019, 6378137, 298.2572221, 0, "GRS 1980");
        /// <summary>
        /// Represents the Helmert ellipsoid of 1906.
        /// </summary>
        public static readonly Ellipsoid Helmert1906 = new Ellipsoid(7020, 6378200, 298.3, 0, "Helmert 1906");
        /// <summary>
        /// Represents the Indonesian ellipsoid of 1974.
        /// </summary>
        public static readonly Ellipsoid Indonesian1974 = new Ellipsoid(7021, 6378160, 298.247, 0, "Indonesian National Spheroid");
        /// <summary>
        /// Represents the International ellipsoid of 1909 (1924 alias).
        /// </summary>
        public static readonly Ellipsoid International1909 = new Ellipsoid(7022, 6378388, 297, 0, "International 1924");
        /// <summary>
        /// Represents the International ellipsoid of 1924.
        /// </summary>
        public static readonly Ellipsoid International1924 = new Ellipsoid(7022, 6378388, 297, 0, "International 1924");
        //No 7023 in EPSG
        /// <summary>
        /// Represents the Krassovsky ellipsoid of 1940.
        /// </summary>
        public static readonly Ellipsoid Krassovsky1940 = new Ellipsoid(7024, 6378245, 298.3, 0, "Krassowsky 1940");
        /// <summary>
        /// Represents the Naval Weapons Lab ellipsoid of 1965.
        /// </summary>
        public static readonly Ellipsoid Nwl9D = new Ellipsoid(7025, 6378145, 298.25, 0, "NWL 9D");
        //No 7026 in EPSG
        /// <summary>
        /// Represents the Plessis ellipsoid of 1817.
        /// </summary>
        public static readonly Ellipsoid Plessis1817 = new Ellipsoid(7027, 6376523, 308.64, 0, "Plessis 1817");
        /// <summary>
        /// Represents the Struve ellipsoid of 1860.
        /// </summary>
        public static readonly Ellipsoid Struve1860 = new Ellipsoid(7028, 6378298.3, 294.73, 0, "Struve 1860");
        /// <summary>
        /// Represents the War Office ellipsoid.
        /// </summary>
        public static readonly Ellipsoid WarOffice = new Ellipsoid(7029, 6378300, 296, 0, "War Office");
        /// <summary>
        /// Represents the World Geodetic System ellipsoid of 1984.
        /// </summary>
        public static readonly Ellipsoid Wgs1984 = new Ellipsoid(7030, 6378137, 298.2572236, 0, "WGS 84");
        /// <summary>
        /// Represents the GEM 10C Gravity Potential Model ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Gem10C = new Ellipsoid(7031, 6378137, 298.2572236, 0, "GEM 10C");
        /// <summary>
        /// Represents the OSU86 gravity potential (geoidal) model ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Osu86F = new Ellipsoid(7032, 6378136.2, 298.2572236, 0, "OSU86F");
        /// <summary>
        /// Represents the OSU91 gravity potential (geoidal) model ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Osu91A = new Ellipsoid(7033, 6378136.3, 298.2572236, 0, "OSU91A");
        /// <summary>
        /// Represents the Clarke ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880 = new Ellipsoid(7034, 20926202 * 0.3047972651151, 293.465, 20854895 * 0.3047972651151, "Clarke 1880");
        /// <summary>
        /// Represents the Authalic Sphere (r=6371000).
        /// </summary>
        public static readonly Ellipsoid AuthalicSphere = new Ellipsoid(7035, 6371000, 0, 6371000, "Authalic Sphere");
        /// <summary>
        /// Represents the Geodetic Reference System ellipsoid of 1967.
        /// </summary>
        public static readonly Ellipsoid Grs67 = new Ellipsoid(7036, 6378160, 298.2471674, 0, "GRS 1967");
        //No 7037 - 7040 in EPSG
        /// <summary>
        /// Represents the Average Terrestrial System ellipsoid of 1977.
        /// </summary>
        public static readonly Ellipsoid Ats1977 = new Ellipsoid(7041, 6378135, 298.257, 0, "Average Terrestrial System 1977");
        /// <summary>
        /// Represents the Everest (1830 Definition) ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Everest1830 = new Ellipsoid(7042, 20922931.8 * 0.3047995102481, 300.8017, 20853374.58 * 0.3047995102481, "Everest (1830 Definition)");
        /// <summary>
        /// Represents the World Geodetic System ellipsoid of 1972.
        /// </summary>
        public static readonly Ellipsoid Wgs1972 = new Ellipsoid(7043, 6378135, 298.26, 0, "WGS 72");
        /// <summary>
        /// Represents the Everest (1962 Definition) ellipsoid of 1830.
        /// </summary>
        public static readonly Ellipsoid Everest1962 = new Ellipsoid(7044, 6377301.243, 300.8017255, 0, "Everest 1830 (1962 Definition)");
        /// <summary>
        /// Represents the Everest (1975 Definition) ellipsoid of 1830.
        /// </summary>
        public static readonly Ellipsoid Everest1975 = new Ellipsoid(7045, 6377299.151, 300.8017255, 0, "Everest 1830 (1975 Definition)");
        /// <summary>
        /// Represents the Bessel (Japan) ellipsoid of 1841.
        /// </summary>
        public static readonly Ellipsoid Bessel1841Japan = new Ellipsoid(7046, 6377397.155, 299.1528128, 0, "Bessel Namibia (GLM)");
        //7047 depricated in EPSG
        /// <summary>
        /// Represents the GRS 1980 Authalic Sphere (r=6371007).
        /// </summary>
        public static readonly Ellipsoid Grs1980AuthalicSphere = new Ellipsoid(7048, 6371007, 0, 6371007, "GRS 1980 Authalic Sphere");
        /// <summary>
        /// Represents the Xian ellipsoid of 1980.
        /// </summary>
        public static readonly Ellipsoid Xian1980 = new Ellipsoid(7049, 6378140, 298.257, 0, "Xian 1980");
        /// <summary>
        /// Represents the IAU ellipsoid of 1976.
        /// </summary>
        public static readonly Ellipsoid Iau76 = Xian1980;
        /// <summary>
        /// Represents the Geodetic Reference System (SAD69) ellipsoid of 1967.
        /// </summary>
        public static readonly Ellipsoid Grs67Sad69 = new Ellipsoid(7050, 6378160, 298.25, 0, "GRS 1967 (SAD69)");
        /// <summary>
        /// Represents the Danish ellipsoid of 1876.
        /// </summary>
        public static readonly Ellipsoid Danish1876 = new Ellipsoid(7051, 6377019.27, 300, 0, "Danish 1876");
        /// <summary>
        /// Represents the Andrae (Danish 1876 alternate) ellipsoid of 1876.
        /// </summary>
        public static readonly Ellipsoid Andrae = Danish1876;
        /// <summary>
        /// Represents the Common Sphere (Clarke 1866 Authalic Sphere alias).
        /// </summary>
        public static readonly Ellipsoid NormalSphere = new Ellipsoid(7052, 6370997, 0, 6370997, "Clarke 1866 Authalic Sphere");
        /// <summary>
        /// Represents the Clarke 1866 Authalic Sphere (r=6370997).
        /// </summary>
        public static readonly Ellipsoid Clarke1866AuthalicSphere = NormalSphere;
        /// <summary>
        /// Represents the Hough ellipsoid of 1960.
        /// </summary>
        public static readonly Ellipsoid Hough1960 = new Ellipsoid(7053, 6378270, 297, 0, "Hough 1960");
        /// <summary>
        /// Represents the PZ90 ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Pz1990 = new Ellipsoid(7054, 6378136, 298.2578393, 0, "PZ-90");
        /// <summary>
        /// Represents the Clarke (international foot) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Clarke1880InternationalFoot = new Ellipsoid(7055, 20926202 * 0.3048, 0, 20854895 * 0.3048, "Clarke 1880 (international foot)");
        /// <summary>
        /// Represents the Everest (RSO 1969) ellipsoid of 1880.
        /// </summary>
        public static readonly Ellipsoid Everest1880Rso = new Ellipsoid(7056, 6377295.664, 300.8017, 0, "Everest 1830 (RSO 1969)");
        /// <summary>
        /// Represents the International 1924 Authalic Sphere.
        /// </summary>
        public static readonly Ellipsoid International1924AuthalicSphere = new Ellipsoid(7057, 6371228, 0, 6371228, "International 1924 Authalic Sphere");
        /// <summary>
        /// Represents the Hughes ellipsoid of 1980.
        /// </summary>
        public static readonly Ellipsoid Hughes1980 = new Ellipsoid(7058, 6378273, 0, 6356889.449, "Hughes 1980");

        #endregion EPSG Ellipsoids

        #region Non-EPSG Ellipsoids

        /// <summary>
        /// Represents the Applied Physics ellipsoid of 1965.
        /// </summary>
        public static readonly Ellipsoid Apl49 = new Ellipsoid("Appl. Physics. 1965", new Distance(6378137.0, DistanceUnit.Meters), 298.25);
        /// <summary>
        /// Represents the Comm. des Poids et Mesures ellipsoid of 1799.
        /// </summary>
        public static readonly Ellipsoid Cpm = new Ellipsoid("Comm. des Poids et Mesures 1799", new Distance(6375738.7, DistanceUnit.Meters), 334.29);
        /// <summary>
        /// Represents the Delambre (Belgium) ellipsoid of 1810.
        /// </summary>
        public static readonly Ellipsoid Delmabre = new Ellipsoid("Delambre 1810 (Belgium)", new Distance(6376428, DistanceUnit.Meters), 311.5);
        /// <summary>
        /// Represents the Engelis ellipsoid of 1985.
        /// </summary>
        public static readonly Ellipsoid Engelis = new Ellipsoid("Engelis 1985", new Distance(6376428, DistanceUnit.Meters), 311.5);
        /// <summary>
        /// Represents the Fisher ellipsoid of 1960.
        /// </summary>
        public static readonly Ellipsoid Fischer1960 = new Ellipsoid("Fisher (Mercury Datum) 1960", new Distance(6378166.0, DistanceUnit.Meters), new Distance(6356784.283666, DistanceUnit.Meters));
        /// <summary>
        /// Represents the Modified Fisher ellipsoid of 1960.
        /// </summary>
        public static readonly Ellipsoid ModifiedFischer1960 = new Ellipsoid("Modified Fisher 1960", new Distance(6378155.0, DistanceUnit.Meters), new Distance(6356773.3205, DistanceUnit.Meters));
        /// <summary>
        /// Represents the Fisher ellipsoid of 1968.
        /// </summary>
        public static readonly Ellipsoid Fischer1968 = new Ellipsoid("Fisher 1968", new Distance(6378150.0, DistanceUnit.Meters), new Distance(6356768.337303, DistanceUnit.Meters));
        /// <summary>
        /// Represents the New International ellipsoid of 1967.
        /// </summary>
        public static readonly Ellipsoid NewInternational1967 = new Ellipsoid("New International 1967", new Distance(6378157.5, DistanceUnit.Meters), new Distance(6356772.2, DistanceUnit.Meters));
        /// <summary>
        /// Represents the Kaula ellipsoid of 1961.
        /// </summary>
        public static readonly Ellipsoid Kaula = new Ellipsoid("Kaula 1961", new Distance(6378163.0, DistanceUnit.Meters), 298.24);
        /// <summary>
        /// Represents the Lerch ellipsoid of 1979.
        /// </summary>
        public static readonly Ellipsoid Lerch = new Ellipsoid("Lerch 1979", new Distance(6378139.0, DistanceUnit.Meters), 298.257);
        /// <summary>
        /// Represents the MERIT ellipsoid of 1983.
        /// </summary>
        public static readonly Ellipsoid Merit = new Ellipsoid("Merit 1983", new Distance(6378137.0, DistanceUnit.Meters), 298.257);
        /// <summary>
        /// Represents the Maupertius ellipsoid of 1738.
        /// </summary>
        public static readonly Ellipsoid Maupertius = new Ellipsoid("Maupertius 1738", new Distance(639730.0, DistanceUnit.Meters), 191);
        /// <summary>
        /// Represents the Southeast Asia (Modified Fisher ellipsoid of 1960) ellipsoid.
        /// </summary>
        public static readonly Ellipsoid SoutheastAsia = new Ellipsoid("Southeast Asia", new Distance(6378155.0, DistanceUnit.Meters), new Distance(6356773.3205, DistanceUnit.Meters));
        /// <summary>
        /// Represents the SGS ellipsoid of 1985.
        /// </summary>
        public static readonly Ellipsoid Sgs1985 = new Ellipsoid("SGS 85", new Distance(6378136.0, DistanceUnit.Meters), new Distance(6356751.301569, DistanceUnit.Meters));
        /// <summary>
        /// Represents the South American ellipsoid of 1969.
        /// </summary>
        public static readonly Ellipsoid SouthAmerican1969 = new Ellipsoid("South American 1969", new Distance(6378160.0, DistanceUnit.Meters), new Distance(6356774.719, DistanceUnit.Meters));
        /// <summary>
        /// Represents the Walbeck ellipsoid.
        /// </summary>
        public static readonly Ellipsoid Walbeck = new Ellipsoid("Walbeck", new Distance(6376896.0, DistanceUnit.Meters), new Distance(6355834.8467, DistanceUnit.Meters));
        /// <summary>
        /// Represents the World Geodetic System ellipsoid of 1960.
        /// </summary>
        public static readonly Ellipsoid Wgs1960 = new Ellipsoid("WGS 60", new Distance(6378165.0, DistanceUnit.Meters), new Distance(6356783.286959, DistanceUnit.Meters));
        /// <summary>
        /// Represents the World Geodetic System ellipsoid of 1966.
        /// </summary>
        public static readonly Ellipsoid Wgs1966 = new Ellipsoid("WGS 1966", new Distance(6378145.0, DistanceUnit.Meters), new Distance(6356759.769356, DistanceUnit.Meters));

        #endregion Non-EPSG Ellipsoids

        /* IMPORTANT: The Default field must be after Ellipsoid.Wgs1984 is initialized, otherwise it will be null! */

        /// <summary>
        /// Represents the default ellipsoid, WGS1984.
        /// </summary>
        public static readonly Ellipsoid Default = Wgs1984;

        #endregion Fields

        #region Constructors
        
// ReSharper disable UnusedMember.Global

        // Needed for xml serialization\deserialization
        internal Ellipsoid()
        {
            
        }
// ReSharper restore UnusedMember.Global

        /// <summary>
        /// Creates a new instance with the specified type, name, equatorial raduis and polar radius.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="equatorialRadius">The equatorial radius.</param>
        /// <param name="polarRadius">The polar radius.</param>
        /// <remarks>This constructor allows user-defined ellipsoids to be created for specialized applications.</remarks>
        public Ellipsoid(string name, Distance equatorialRadius, Distance polarRadius)
        {
            _name = name;
            _equatorialRadius = equatorialRadius;
            _polarRadius = polarRadius;

            // Perform calculations
            Calculate();

            SanityCheck();

            // And add it to the list
            _ellipsoids.Add(this);
        }

        /// <summary>
        /// Internal contructor for static list generation
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="equatorialRadius">The equatorial radius.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        public Ellipsoid(string name, Distance equatorialRadius, double inverseFlattening)
        {
            _name = name;
            _equatorialRadius = equatorialRadius;
            _inverseFlattening = inverseFlattening;

            // Perform calculations
            Calculate();

            SanityCheck();

            // And add it to the list
            _ellipsoids.Add(this);
        }

        /// <summary>
        /// Internal contructor for static list generation
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <param name="a">A.</param>
        /// <param name="invf">The invf.</param>
        /// <param name="b">The b.</param>
        /// <param name="name">The name.</param>
        internal Ellipsoid(int epsgNumber, double a, double invf, double b, string name)
        {
            _name = name;
            _epsgNumber = epsgNumber;
            _equatorialRadius = Distance.FromMeters(a);
            _polarRadius = Distance.FromMeters(b);
            _inverseFlattening = invf;
            Calculate();

            SanityCheck();

            _epsgEllipsoids.Add(this);
        }

        /// <summary>
        /// Creates a new instance from the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Ellipsoid(XmlReader reader)
        {
            ReadXml(reader);
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Validates the ellipsoid. Called in the constructor.
        /// </summary>
        private void SanityCheck()
        {
            if ((_equatorialRadius.IsEmpty && _inverseFlattening == 0) || (_equatorialRadius.IsEmpty && _polarRadius.IsEmpty))
                throw new ArgumentException("The radii and inverse flattening of an allipsoid cannot be zero.   Please specify either the equatorial and polar radius, or the equatorial radius and the inverse flattening for this ellipsoid.");
        }

        /// <summary>
        /// Calculates the common ellipsoid properties. Called from the constructor
        /// </summary>
        private void Calculate()
        {
            double a = _equatorialRadius.ToMeters().Value;
            double b = _polarRadius.ToMeters().Value;
            double invf = _inverseFlattening;

            // Check the input. If a minor axis wasn't supplied, calculate it.
            if (b == 0) b = -(((1.0 / invf) * a) - a);

            _polarRadius = Distance.FromMeters(b);

            _flattening = (_equatorialRadius.ToMeters().Value - _polarRadius.ToMeters().Value) / _equatorialRadius.ToMeters().Value;
            _inverseFlattening = 1.0 / _flattening;
            _eccentricity = Math.Sqrt((Math.Pow(_equatorialRadius.Value, 2) - Math.Pow(_polarRadius.Value, 2)) / Math.Pow(_equatorialRadius.Value, 2));
            _eccentricitySquared = Math.Pow(Eccentricity, 2);

            // This is used very frequently by calculations.  Since ellipsoids do not change, there's
            // no need to call .ToMeters() thousands of times.
            _equatorialRadiusMeters = _equatorialRadius.ToMeters().Value;
            _polarRadiusMeters = _polarRadius.ToMeters().Value;
        }

        #endregion Private Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Ellipsoid)
                return Equals((Ellipsoid)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _equatorialRadius.GetHashCode() ^ _polarRadius.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _name;
        }

        #endregion Overrides

        #region Public Properties

        /// <summary>
        /// European Petroleum Survey Group number for this ellipsoid. The ESPG standards are now maintained by OGP
        /// (International Association of Oil and Gas Producers).
        /// </summary>
        public int EpsgNumber
        {
            get { return _epsgNumber; }
        }

        /// <summary>
        /// Indicates the descriptive name of the ellipsoid.
        /// </summary>
        /// <value>A <strong>String</strong> containing the name of the ellipsoid.</value>
        /// <remarks>This property is typically used to display ellipsoid information on a user interface.</remarks>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Represents the distance from Earth's center to the equator.
        /// </summary>
        /// <value>A <strong>Distance</strong> object.</value>
        /// <seealso cref="PolarRadius">PolarRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>PolarRadius</strong> property
        /// to define an ellipsoidal shape. This property returns the same value as the
        /// <strong>SemiMajorAxis</strong> property.</remarks>
        public Distance EquatorialRadius
        {
            get { return _equatorialRadius; }
        }

        /// <summary>
        /// Represents the distance from Earth's center to the North or South pole.
        /// </summary>
        /// <value>A <strong>Distance</strong> object.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>EquatorialRadius</strong>
        /// property to define an ellipsoidal shape. This property returns the same value as
        /// the <strong>SemiMinorAxis</strong> property.</remarks>
        public Distance PolarRadius
        {
            get { return _polarRadius; }
        }

        /// <summary>
        /// Represents the distance from Earth's center to the equator.
        /// </summary>
        /// <value>A <strong>Distance</strong> containing Earth's equatorial radius.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>SemiMinorAxis</strong>
        /// property to define an ellipsoidal shape. This property returns the same value as
        /// the <strong>EquatorialRadius</strong> property.</remarks>
        public Distance SemiMajorAxis
        {
            get { return _equatorialRadius; }
        }

        /// <summary>
        /// Represents the distance from Earth's center to the North or South pole.
        /// </summary>
        /// <value>A <strong>Distance</strong> containing Earth's polar radius.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>SemiMajorAxis</strong>
        /// property to define an ellipsoidal shape. This property returns the same value as
        /// the <strong>PolarRadius</strong> property.</remarks>
        public Distance SemiMinorAxis
        {
            get { return _polarRadius; }
        }

        /// <summary>
        /// Indicates if the ellipsoid is describing a perfect sphere.
        /// </summary>
        /// <remarks>Mathematical formulas such as map projection and coordinate conversion can be
        /// optimized if the ellipsoid they are working with is spherical. For more precise
        /// results, however, spherical ellipsoids should not be used. This property, when used
        /// correctly, can improve performance for mathematics when coordinate precision is less of
        /// a concern, such as viewing a map from a high altitude.</remarks>
        public bool IsSpherical
        {
            get { return _equatorialRadius.Equals(_polarRadius); }
        }

        /// <summary>
        /// Indicates the inverse of the shape of an ellipsoid relative to a sphere.
        /// </summary>
        /// <value>A <strong>Double</strong> containing the ellipsoid's flattening.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property is used frequently in equations. Inverse flattening is defined as
        /// one divided by the <strong>Flattening</strong> property.:</remarks>
        public double InverseFlattening
        {
            get { return _inverseFlattening; }
        }

        /// <summary>
        /// Indicates the shape of the ellipsoid relative to a sphere.
        /// </summary>
        /// <value>A <strong>Double</strong> containing the ellipsoid's flattening.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property compares the equatorial radius with the polar radius to measure the
        /// amount that the ellipsoid is "squished" vertically.</remarks>
        public double Flattening
        {
            get
            {
                return _flattening;
            }
        }

        /// <summary>
        /// Returns the rate of flattening of the ellipsoid.
        /// </summary>
        /// <value>A <strong>Double</strong> measuring how elongated the ellipsoid is.</value>
        /// <remarks>The eccentricity is a positive number less than 1, or 0 in the case of a circle.
        /// The greater the eccentricity is, the larger the ratio of the equatorial radius to the
        /// polar radius is, and therefore the more elongated the ellipse is.</remarks>
        public double Eccentricity
        {
            get
            {
                return _eccentricity;
            }
        }

        /// <summary>
        /// Returns the square of the eccentricity.
        /// </summary>
        /// <remarks>This property returns the value of the <strong>Eccentricity</strong> property,
        /// squared. It is used frequently during coordinate conversion formulas.</remarks>
        public double EccentricitySquared
        {
            get
            {
                return _eccentricitySquared;
            }
        }

        #endregion Public Properties

        #region Internal Propertis

        /// <summary>
        /// Gets the polar radius meters.
        /// </summary>
        internal double PolarRadiusMeters
        {
            get
            {
                return _polarRadiusMeters;
            }
        }

        /// <summary>
        /// Gets the equatorial radius meters.
        /// </summary>
        internal double EquatorialRadiusMeters
        {
            get
            {
                return _equatorialRadiusMeters;
            }
        }

        /// <summary>
        /// Gets the semi major axis meters.
        /// </summary>
        internal double SemiMajorAxisMeters
        {
            get
            {
                return _equatorialRadiusMeters;
            }
        }

        /// <summary>
        /// Gets the semi minor meters.
        /// </summary>
        internal double SemiMinorMeters
        {
            get
            {
                return _polarRadiusMeters;
            }
        }

        #endregion Internal Propertis

        #region Static Methods

        /// <summary>
        /// Returns a Ellipsoid object matching the specified name.
        /// </summary>
        /// <param name="name">A <strong>String</strong> describing the name of an existing Ellipsoid.</param>
        /// <returns>A <strong>Ellipsoid</strong> object matching the specified string, or null if no Ellipsoid was found.</returns>
        public static Ellipsoid FromName(string name)
        {
            // Search the custom objects
            foreach (Ellipsoid item in _ellipsoids)
            {
                if (item.Name == name)
                    return item;
            }
            // Search the EPSG objects
            return _epsgEllipsoids.FirstOrDefault(item => item.Name == name);
        }

        /// <summary>
        /// Returns the datum corresponding to the EPSG code
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <returns></returns>
        public static Ellipsoid FromEpsgNumber(int epsgNumber)
        {
            // Search the EPSG objects
            return _epsgEllipsoids.FirstOrDefault(item => item.EpsgNumber == epsgNumber);
        }

        #endregion Static Methods

        #region IEquatable<Ellipsoid> Members

        /// <summary>
        /// Returns whether the current ellipsoid has the same value as the specified ellipsoid.
        /// </summary>
        /// <param name="other">An <strong>Ellipsoid</strong> object to compare against.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the equatorial radius and polar radius
        /// of both ellipsoids are equal.  When both radii are equal, all other calculated properties will also
        /// be equal.  The name of the ellipsoid is not compared.</returns>
        public bool Equals(Ellipsoid other)
        {
            if (other == null)
                return false;

            return other.EquatorialRadius.Equals(_equatorialRadius)
                && other.PolarRadius.Equals(_polarRadius);
        }

        /// <summary>
        /// Returns whether the current ellipsoid has the same value as the specified ellipsoid.
        /// </summary>
        /// <param name="other">An <strong>Ellipsoid</strong> object to compare against.</param>
        /// <param name="decimals">An <strong>integer</strong> specifies the precision for the comparison.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the equatorial radius and polar radius
        /// of both ellipsoids are equal.  When both radii are equal, all other calculated properties will also
        /// be equal.  The name of the ellipsoid is not compared.</returns>
        public bool Equals(Ellipsoid other, int decimals)
        {
            if (other == null)
                return false;

            return other.EquatorialRadius.Equals(_equatorialRadius, decimals)
                && other.PolarRadius.Equals(_polarRadius, decimals);
        }

        #endregion IEquatable<Ellipsoid> Members

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            /*   The GML specification defines an ellipsoid as follows:
             *
             *   <gml:Ellipsoid gml:id="ogrcrs44">
             *     <gml:ellipsoidName>GRS 1980</gml:ellipsoidName>
             *     <gml:ellipsoidID>
             *       <gml:name gml:codeSpace="urn:ogc:def:ellipsoid:EPSG::">7019</gml:name>
             *     </gml:ellipsoidID>
             *     <gml:semiMajorAxis gml:uom="urn:ogc:def:uom:EPSG::9001">6378137</gml:semiMajorAxis>
             *     <gml:secondDefiningParameter>
             *       <gml:inverseFlattening gml:uom="urn:ogc:def:uom:EPSG::9201">298.257222101</gml:inverseFlattening>
             *     </gml:secondDefiningParameter>
             *   </gml:Ellipsoid>
             *
             */

            writer.WriteStartElement(Xml.GML_XML_PREFIX, "Ellipsoid", Xml.GML_XML_NAMESPACE);

            writer.WriteElementString(Xml.GML_XML_PREFIX, "ellipsoidName", Xml.GML_XML_NAMESPACE, _name);

            writer.WriteStartElement(Xml.GML_XML_PREFIX, "ellipsoidID", Xml.GML_XML_NAMESPACE);
            writer.WriteString(_epsgNumber.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(Xml.GML_XML_PREFIX, "semiMajorAxis", Xml.GML_XML_NAMESPACE);
            writer.WriteString(SemiMajorAxis.ToMeters().Value.ToString("G17"));
            writer.WriteEndElement();

            writer.WriteStartElement(Xml.GML_XML_PREFIX, "semiMinorAxis", Xml.GML_XML_NAMESPACE);
            writer.WriteString(SemiMinorAxis.ToMeters().Value.ToString("G17"));
            writer.WriteEndElement();

            writer.WriteStartElement(Xml.GML_XML_PREFIX, "secondDefiningParameter", Xml.GML_XML_NAMESPACE);
            writer.WriteElementString(Xml.GML_XML_PREFIX, "inverseFlattening", Xml.GML_XML_NAMESPACE, InverseFlattening.ToString("G17"));
            writer.WriteEndElement();

            writer.WriteEndElement();

            SanityCheck();
            Calculate();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // Read until we have an element
            while (!reader.EOF && reader.NodeType != XmlNodeType.Element)
                reader.Read();

            // If we're at EOF, exit
            if (reader.EOF)
                return;

            // Remember the current depth. We'll keep reading until we return to this depth
            int depth = reader.Depth;
         
            // Notify of the read
            OnReadXml(reader);

            // Have we returned to the original depth?  If we're at a deeper depth,
            // keep reading.
            while (!reader.EOF
                && reader.Depth > depth)
            {
                // Is this an element?  If not, keep reading deeper
                while (!reader.EOF
                    && reader.Depth > depth
                    && reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                }

                // If this is an element, process it
                if (reader.NodeType == XmlNodeType.Element)
                    OnReadXml(reader);
            }

            reader.Read();
        }

        /// <summary>
        /// Called when [read XML].
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void OnReadXml(XmlReader reader)
        {
            switch (reader.LocalName)
            {
                case "ellipsoidName":
                    _name = reader.ReadElementContentAsString();
                    break;
                case "ellipsoidID":
                    _epsgNumber = reader.ReadElementContentAsInt();
                    break;
                case "semiMajorAxis":
                    _equatorialRadius = new Distance(reader.ReadElementContentAsDouble(), DistanceUnit.Meters);
                    break;
                case "semiMinorAxis":
                    _polarRadius = new Distance(reader.ReadElementContentAsDouble(), DistanceUnit.Meters);
                    break;
                case "secondDefiningParameter":
                    // Read deeper
                    reader.Read();
                    break;
                case "inverseFlattening":
                    _inverseFlattening = reader.ReadElementContentAsDouble();
                    break;
                default:
                    // Read deeper
                    reader.Read();
                    break;
            }
        }

        #endregion IXmlSerializable Members
    }
}