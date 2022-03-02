using System;
using System.Collections.Generic;
using System.Globalization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning.Gps
{
    /// <summary>Represents information about a GPS satellite in orbit above Earth.</summary>
    /// <remarks>
    /// 	<para>GPS devices are able to isolate information about GPS satellites in orbit. Each
    ///  satellite's <see cref="Satellite.PseudorandomNumber">unique identifier</see>,
    ///  <see cref="Satellite.SignalToNoiseRatio">radio signal strength</see>,
    ///  <see cref="Satellite.Azimuth">azimuth</see> and
    ///  <see cref="Satellite.Elevation">elevation</see> are available once its
    ///  radio signal is detected.</para>
    /// 	<para>Properties in this class are updated automatically as new information is
    ///  received from the GPS device.</para>
    /// 	<para><img src="Satellite.jpg"/></para>
    /// </remarks>
    /// <seealso cref="SatelliteCollection">SatelliteCollection Class</seealso>
#if !PocketPC || DesignTime
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public struct Satellite : IFormattable, IEquatable<Satellite>, IComparable<Satellite>
    {
        private int _PseudorandomNumber;
        private Azimuth _Azimuth;
        private Elevation _Elevation;
        private SignalToNoiseRatio _SignalToNoiseRatio;
        private bool _IsFixed;
        private DateTime _LastSignalReceived;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified unique identifier.
        /// </summary>
        /// <param name="pseudorandomNumber"></param>
        public Satellite(int pseudorandomNumber)
            : this(pseudorandomNumber, Azimuth.Empty, Elevation.Empty, SignalToNoiseRatio.Empty, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation)
            : this(pseudorandomNumber, azimuth, elevation, SignalToNoiseRatio.Empty, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        /// <param name="signalToNoiseRatio"></param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation, SignalToNoiseRatio signalToNoiseRatio)
            : this(pseudorandomNumber, azimuth, elevation, signalToNoiseRatio, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber"></param>
        /// <param name="azimuth"></param>
        /// <param name="elevation"></param>
        /// <param name="signalToNoiseRatio"></param>
        /// <param name="isFixed"></param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation, SignalToNoiseRatio signalToNoiseRatio, bool isFixed)
        {
            _PseudorandomNumber = pseudorandomNumber;
            _Azimuth = azimuth;
            _Elevation = elevation;
            _SignalToNoiseRatio = signalToNoiseRatio;
            _IsFixed = isFixed;
            _LastSignalReceived = DateTime.MinValue;
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the unique identifier of the satellite.</summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the unique identifier of the satellite.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
#endif
        public int PseudorandomNumber
        {
            get
            {
                return _PseudorandomNumber;
            }
        }

        /// <summary>
        /// Returns the horizontal direction towards the satellite from the current location.
        /// </summary>
#if !PocketPC
        [Category("Location")]
        [Description("Returns the horizontal direction towards the satellite from the current location.")]
        [Browsable(true)]
#endif
        public Azimuth Azimuth
        {
            get
            {
                return _Azimuth;
            }
            set
            {
                if (_Azimuth.Equals(value))
                    return;

                _Azimuth = value;
            }
        }


        /// <summary>
        /// Returns the vertical direction towards the satellite from the current
        /// location.
        /// </summary>
#if !PocketPC
        [Category("Location")]
        [Description("Returns the vertical direction towards the satellite from the current location.")]
        [Browsable(true)]
#endif
        public Elevation Elevation
        {
            get
            {
                return _Elevation;
            }
            set
            {
                if (_Elevation.Equals(value))
                    return;

                _Elevation = value;
            }
        }


        /// <summary>
        /// Returns the strength of the satellite's radio signal as it is being
        /// received.
        /// </summary>
#if !PocketPC
        [Category("Radio Signal")]
        [Description("Returns the strength of the satellite's radio signal as it is being received.")]
        [Browsable(true)]
#endif
        public SignalToNoiseRatio SignalToNoiseRatio
        {
            get
            {
                return _SignalToNoiseRatio;
            }
            set
            {
                if (_SignalToNoiseRatio.Equals(value))
                    return;

                _SignalToNoiseRatio = value;
            }
        }

        /// <summary>Returns the date and time that the satellite's signal was detected.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the date and time that the satellite's signal was detected.")]
        [Browsable(true)]
#endif
        public DateTime LastSignalReceived
        {
            get
            {
                return _LastSignalReceived;
            }
            set
            {
                if (_LastSignalReceived.Equals(value))
                    return;

                _LastSignalReceived = value;
            }
        }

        /// <summary>
        /// Returns whether the satellite's signal is being used to calculate the current
        /// location.
        /// </summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is being used to calculate the current location.")]
        [Browsable(true)]
#endif
        public bool IsFixed
        {
            get
            {
                return _IsFixed;
            }
            set
            {
                if (_IsFixed.Equals(value))
                    return;

                _IsFixed = value;
            }
        }

        /// <summary>Returns whether the satellite's signal is currently being detected.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is currently being detected.")]
        [Browsable(true)]
#endif
        public bool IsTracked
        {
            get
            {
                return !_SignalToNoiseRatio.IsEmpty;
            }
        }

        /// <summary>Returns the amount of time elapsed since the signal was last received.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is currently being detected.")]
        [Browsable(true)]
#endif
        public TimeSpan SignalAge
        {
            get
            {
                return DateTime.Now.Subtract(_LastSignalReceived);
            }
        }

        /// <summary>Returns whether the satellite's signal has recently been received.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal has recently been received.")]
        [Browsable(true)]
#endif
        public bool IsActive
        {
            get
            {
                return _IsFixed || SignalAge.TotalSeconds < 10.0;
            }
        }

        /// <summary>
        /// Indicates whether the satellite is providing additional corrective
        /// signals to increase precision.
        /// </summary>
        /// <remarks>This property will return a value of <strong>True</strong>
        /// if the GPS satellite has been identified as a WAAS, EGNOS or MSAS satellite.
        /// These satellites are geostationary (unlike typical NAVSTAR GPS satellites)
        /// and re-broadcast correction signals from ground stations.  When this property
        /// is true, the GPS device has improved precision.</remarks>
#if !PocketPC
        [Category("Behavior")]
        [Description("Indicates whether the satellite is providing additional corrective signals to increase precision.")]
        [Browsable(true)]
#endif
        public bool IsDifferentialGpsSatellite
        {
            get
            {
                SatelliteClass satelliteClass = this.Class;
                return satelliteClass.Equals(SatelliteClass.Waas)
                       || satelliteClass.Equals(SatelliteClass.Egnos)
                       || satelliteClass.Equals(SatelliteClass.Msas);
            }
        }

        /// <summary>Returns the government project responsible for launching the satellite.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the government project responsible for launching the satellite.")]
        [Browsable(true)]
#endif
        public SatelliteClass Class
        {
            get
            {
                switch (_PseudorandomNumber)
                {
                    case 122:
                    case 134:
                    case 125:
                    case 135:
                    case 138:
                    case 35:
                    case 47:
                    case 48:
                    case 51:
                        return SatelliteClass.Waas;
                    case 120:
                    case 124:
                    case 126:
                    case 131:
                    case 33:
                    case 37:
                    case 39:
                    case 44:
                        return SatelliteClass.Egnos;
                    case 129:
                    case 137:
                    case 42:
                    case 50:
                        return SatelliteClass.Msas;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                        return SatelliteClass.Navstar;
                    default:
                        return SatelliteClass.Unknown;
                }
            }
        }

        /// <summary>Returns the atomic clock currently in service.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the atomic clock currently in service.")]
        [Browsable(true)]
#endif
        public SatelliteAtomicClockType AtomicClockType
        {
            get
            {
                switch (_PseudorandomNumber)
                {
                    /* Data was compiled from Wikipedia:
                     * http://en.wikipedia.org/wiki/List_of_GPS_satellite_launches
                     */

                    case 24:
                    case 27:
                    case 09:
                    case 03:
                    case 10:
                    case 30:
                    case 08:
                        return SatelliteAtomicClockType.Cesium;
                    case 32:
                    case 25:
                    case 26:
                    case 05:
                    case 04:
                    case 06:
                    case 13:
                    case 11:
                    case 20:
                    case 28:
                    case 14:
                    case 18:
                    case 16:
                    case 21:
                    case 22:
                    case 19:
                    case 23:
                    case 02:
                    case 17:
                    case 31:
                    case 12:
                    case 15:
                    case 29:
                    case 07:
                        return SatelliteAtomicClockType.Rubidium;
                    default:
                        return SatelliteAtomicClockType.Unknown;
                }
            }
        }

        /// <summary>Returns the launch block of the satellite.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the launch block of the satellite.")]
        [Browsable(true)]
#endif
        public SatelliteBlock Block
        {
            get
            {
                switch (_PseudorandomNumber)
                {
                    /* Compiled via the US Military
                     * ftp://tycho.usno.navy.mil/pub/gps/gpsb1.txt
                     */

                    case 32:
                    case 24:
                    case 25:
                    case 26:
                    case 27:                    
                    case 9:
                    case 5:
                    case 4:
                    case 6:
                    case 3:
                    case 10:
                    case 30:
                    case 08:
                        return SatelliteBlock.IIA;
                    case 13:
                    case 11:
                    case 20:
                    case 28:
                    case 14:
                    case 18:
                    case 16:
                    case 21:
                    case 22:
                    case 19:
                    case 23:
                    case 2:
                    case 17:
                    case 31:
                    case 12:
                    case 15:
                    case 29:
                    case 7:
                        return SatelliteBlock.IIR;                        
                    default:
                        return SatelliteBlock.Unknown;
                }
            }
        }

        /// <summary>Returns the date the satellite was placed into orbit.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the date the satellite was placed into orbit.")]
        [Browsable(true)]
#endif
        public DateTime DateLaunched
        {
            get
            {
                /* Data was compiled from Wikipedia:
                 * http://en.wikipedia.org/wiki/List_of_GPS_satellite_launches
                 */

                switch (_PseudorandomNumber)
                {
                    case 32:
                        return new DateTime(1990, 11, 26);
                    case 24:
                        return new DateTime(1991, 07, 04);
                    case 25:
                        return new DateTime(1992, 02, 23);
                    case 26:
                        return new DateTime(1992, 07, 07);
                    case 27:
                        return new DateTime(1992, 09, 09);
                    case 01:
                        return new DateTime(1992, 05, 13);
                    case 09:
                        return new DateTime(1993, 06, 26);
                    case 05:
                        return new DateTime(1993, 08, 30);
                    case 04:
                        return new DateTime(1993, 10, 28);
                    case 06:
                        return new DateTime(1994, 03, 10);
                    case 03:
                        return new DateTime(1996, 03, 28);
                    case 10:
                        return new DateTime(1996, 07, 16);
                    case 30:
                        return new DateTime(1996, 09, 12);

                    case 13:
                        return new DateTime(1997, 07, 23);
                    case 11:
                        return new DateTime(1999, 10, 07);
                    case 20:
                        return new DateTime(2000, 05, 11);
                    case 28:
                        return new DateTime(2000, 07, 16);
                    case 14:
                        return new DateTime(2000, 11, 10);
                    case 18:
                        return new DateTime(2001, 01, 30);
                    case 16:
                        return new DateTime(2003, 01, 29);
                    case 21:
                        return new DateTime(2003, 03, 31);
                    case 22:
                        return new DateTime(2003, 12, 21);
                    case 19:
                        return new DateTime(2004, 03, 20);
                    case 23:
                        return new DateTime(2004, 06, 23);
                    case 02:
                        return new DateTime(2004, 11, 06);

                    case 17:
                        return new DateTime(2005, 09, 26);
                    case 31:
                        return new DateTime(2006, 09, 25);
                    case 12:
                        return new DateTime(2006, 11, 17);
                    case 15:
                        return new DateTime(2007, 10, 17);
                    case 29:
                        return new DateTime(2007, 12, 20);
                    case 07:
                        return new DateTime(2008, 03, 15);

                    default:
                        return DateTime.MinValue;
                }
            }
        }

        /// <summary>Returns the date the satellite was placed into service.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the date the satellite was placed into service.")]
        [Browsable(true)]
#endif
        public DateTime DateCommissioned
        {
            get
            {
                switch (_PseudorandomNumber)
                {
                    /* Data was compiled from the US Military:
                     * ftp://tycho.usno.navy.mil/pub/gps/gpsb2.txt
                     */

                    #region Temporarily offline or decommissioned

                    //case 17:
                    //    return new DateTime(1990, 1, 6);              DECOMMISSIONED
                    //case 18:
                    //    return new DateTime(1990, 2, 14);              DECOMMISSIONED
                    //case 19:
                    //    return new DateTime(1989, 11, 23);              DECOMMISSIONED
                    //case 20:
                    //    return new DateTime(1990, 4, 18);              DECOMMISSIONED
                    //case 21:
                    //    return new DateTime(1990, 8, 22);              DECOMMISSIONED
                    //case 22:
                    //    return new DateTime(1993, 4, 4);              DECOMMISSIONED
                    //case 23:
                    //    return new DateTime(1990, 12, 10);              DECOMMISSIONED
                    case 32:
                        return new DateTime(2008, 2, 26);
                    case 24:
                        return new DateTime(1991, 8, 30);
                    case 25:
                        return new DateTime(1992, 3, 24);
                    case 26:
                        return new DateTime(1992, 7, 23);
                    case 27:
                        return new DateTime(1992, 9, 30);
                    //case 28:
                    //    return new DateTime(1992, 4, 25);              DECOMMISSIONED
                    //case 29:
                    //    return new DateTime(1993, 1, 5);              DECOMMISSIONED
                    case 30:
                        return new DateTime(1996, 10, 1);
                    //case 31:
                    //    return new DateTime(1993, 4, 13);              DECOMMISSIONED
                    case 01:
                        return new DateTime(1992, 12, 11);
                    case 03:
                        return new DateTime(1996, 4, 9);
                    case 04:
                        return new DateTime(1993, 11, 22);
                    case 05:
                        return new DateTime(1993, 9, 28);
                    case 06:
                        return new DateTime(1994, 3, 28);
                    //case 07:
                    //    return new DateTime(1993, 6, 12);              DECOMMISSIONED
                    case 08:
                        return new DateTime(1997, 12, 18);
                    case 09:
                        return new DateTime(1993, 7, 21);
                    case 10:
                        return new DateTime(1996, 8, 15);

                    #endregion

                    #region Currently operational

                    case 14:
                        return new DateTime(2000, 12, 10);
                    case 13:
                        return new DateTime(1998, 1, 31);
                    case 28:
                        return new DateTime(2000, 8, 17);
                    case 21:
                        return new DateTime(2003, 4, 12);
                    case 11:
                        return new DateTime(2000, 1, 3);
                    case 22:
                        return new DateTime(2004, 1, 12);
                    case 07:
                        return new DateTime(2008, 3, 24);
                    case 20:
                        return new DateTime(2000, 1, 1);
                    case 31:
                        return new DateTime(2006, 10, 12);
                    case 17:
                        return new DateTime(2005, 12, 16);
                    case 18:
                        return new DateTime(2001, 2, 15);
                    case 15:
                        return new DateTime(2007, 10, 31);
                    case 16:
                        return new DateTime(2003, 2, 19);
                    case 29:
                        return new DateTime(2008, 1, 2);
                    case 12:
                        return new DateTime(2006, 12, 13);
                    case 19:
                        return new DateTime(2004, 4, 5);
                    case 23:
                        return new DateTime(2004, 7, 9);
                    case 02:
                        return new DateTime(2004, 11, 22);

                    #endregion

                    default:
                        return DateTime.MinValue;
                }
            }
        }

        /// <summary>Returns the date the satellite was removed from service.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the date the satellite was removed from service.")]
        [Browsable(true)]
#endif
        public DateTime DateDecommissioned
        {
            get
            {
                switch (_PseudorandomNumber)
                {
                    case 2:
                        return new DateTime(2004, 5, 12);
                    case 14:
                        return new DateTime(2000, 4, 14);
                    default:
                        return new DateTime(0);
                }
            }
        }

        /// <summary>Returns the friendly name of the satellite.</summary>
#if !PocketPC
        [Category("Statistics")]
        [Description("Returns the friendly name of the satellite.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
#endif
        public string Name
        {
            get
            {
                /* Information was obtained from WikiPedia:
                 * http://en.wikipedia.org/wiki/List_of_GPS_satellite_launches
                 */

                switch (_PseudorandomNumber)
                {
                    #region Standard NAVSTAR satellites

                    case 32:
                        return "Navstar 2A-01";
                    case 24:
                        return "Navstar 2A-02";
                    case 25:
                        return "Navstar 2A-03";
                    case 26:
                        return "Navstar 2A-05";
                    case 27:
                        return "Navstar 2A-06";
                    case 1:
                        return "Navstar 2A-11";
                    case 9:
                        return "Navstar 2A-12";
                    case 5:
                        return "Navstar 2A-13";
                    case 4:
                        return "Navstar 2A-14";
                    case 6:
                        return "Navstar 2A-15";
                    case 3:
                        return "Navstar 2A-16";
                    case 10:
                        return "Navstar 2A-17";
                    case 30:
                        return "Navstar 2A-18";
                    case 13:
                        return "Navstar 43";
                    case 11:
                        return "Navstar 46";
                    case 20:
                        return "Navstar 47";
                    case 28:
                        return "Navstar 48";
                    case 14:
                        return "Navstar 49";
                    case 18:
                        return "Navstar 50";
                    case 16:
                        return "Navstar 51";
                    case 21:
                        return "Navstar 52";
                    case 22:
                        return "Navstar 53";
                    case 19:
                        return "Navstar 54";
                    case 23:
                        return "Navstar 55";
                    case 2:
                        return "Navstar 56";
                    case 17:
                        return "Navstar 57";
                    case 31:
                        return "GPS 2R-15";
                    case 12:
                        return "Navstar 59";
                    case 15:
                        return "GPS 2R-17";
                    case 29:
                        return "GPS 2R-18";
                    case 7:
                        return "Navstar 62";

                    #endregion

                    #region Wide Area Augmentation System (WAAS)

                    /* http://en.wikipedia.org/wiki/WAAS */

                    case 35:
                        return "Atlantic Ocean Region-West (WAAS)";
                    case 51:
                        return "Anik F1R (WAAS)";
                    case 47:
                        return "Pacific Ocean Region (WAAS)";
                    case 48:
                        return "Galaxy 15 (WAAS)";

                    #endregion

                    #region EGNOS  (European)

                    case 33:
                        return "Atlantic Ocean Region-East (EGNOS)";
                    case 37:
                        return "ARTEMIS (EGNOS)";
                    case 39:
                        return "Indian Ocean Region-West (EGNOS)";
                    case 44:
                        return "Indian Ocean Region-East (EGNOS)";

                    #endregion

                    #region MSAS (Japan)

                    case 42:
                        return "MTSAT-1";
                    case 50:
                        return "MTSAT-2";

                    #endregion

                    case 0:
                        // A placeholder for the SatellitSignalBar control that 
                        // is displaying no satellites. Otherwise the control is 
                        // not visible.
                        return "Unknown";
                    default:
                        return "New Satellite";
                }
            }
        }

        #endregion

        #region Public Methods

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is Satellite)
                return Equals((Satellite)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _PseudorandomNumber;
        }

        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns the satellites in the specified in the list which are marked as fixed.
        /// </summary>
        /// <param name="satellites"></param>
        /// <returns></returns>
        public static List<Satellite> GetFixedSatellites(List<Satellite> satellites)
        {
            List<Satellite> results = new List<Satellite>();
            for (int index = 0; index < satellites.Count; index++)
            {
                Satellite satellite = satellites[index];
                if (satellite.IsFixed)
                    results.Add(satellite);
            }
            return results;
        }

        #endregion

        #region IEquatable<Satellite> Members

        public bool Equals(Satellite other)
        {
            return _PseudorandomNumber.Equals(other.PseudorandomNumber);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            return Name + " (" + _PseudorandomNumber.ToString(format, formatProvider) + "): "
                        + _Azimuth.ToString(format, formatProvider) + culture.TextInfo.ListSeparator + " "
                        + _Elevation.ToString(format, formatProvider) + culture.TextInfo.ListSeparator + " "
                        + _SignalToNoiseRatio.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable<Satellite> Members

        public int CompareTo(Satellite other)
        {
            // Fixed satellites come first, then non-fixed satellites.
            // Second, satellites are sorted by signal, strongest signal first.
            if (_IsFixed && !other.IsFixed)
                return 1;
            if (!_IsFixed && other.IsFixed)
                return -1;
            return other.SignalToNoiseRatio.CompareTo(_SignalToNoiseRatio);
        }

        #endregion
    }

    /// <summary>
    /// Represents information about a satellite when a satellite-related event is raised.
    /// </summary>
    /// <remarks>This object is used primarily by the <see cref="Satellite">Satellite</see>
    /// class to provide notification when information such as <see cref="Satellite.Azimuth">azimuth</see>,
    /// <see cref="Satellite.Elevation">elevation</see>, or <see cref="Satellite.SignalToNoiseRatio">radio signal strength</see> 
    /// has changed.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Start a new receiver
    /// Dim MyReceiver As New Receiver()
    /// ' Declare a new event
    /// Dim MySatelliteEvent As EventHandler
    /// ' Get a handle on the first satellite in the receiver's collection
    /// Dim MySatellite As Satellite = MyReceiver.Satellites(0)
    /// 
    /// Sub Main()
    ///   ' Raise our custom event
    ///   RaiseEvent MySatelliteEvent(Me, New SatelliteEventArgs(MySatellite))
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// // Start a new receiver
    /// Receiver MyReceiver = new Receiver();
    /// // Declare a new event
    /// EventHandler MySatelliteEvent;
    /// // Create an Satellite of 90°
    /// Satellite MySatellite = new Satellite(90);
    /// 
    /// void Main()
    /// {
    ///   // Raise our custom event
    ///   MySatelliteEvent(this, New SatelliteEventArgs(MySatellite));
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="Satellite">SatelliteCollection Class</seealso>
    /// <seealso cref="Satellite.Azimuth">Azimuth Property (Satellite Class)</seealso>
    /// <seealso cref="Satellite.Elevation">Elevation Property (Satellite Class)</seealso>
    /// <seealso cref="Satellite.SignalToNoiseRatio">SignalToNoiseRatio Property (Satellite Class)</seealso>
    public sealed class SatelliteEventArgs : EventArgs
    {
        private Satellite _Satellite;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="Satellite"></param>

        public SatelliteEventArgs(Satellite satellite)
        {
            _Satellite = satellite;
        }

        /// <summary>
        /// Indicates which satellite is the target of the event.
        /// </summary>
        /// <value>A <strong>Satellite</strong> object containing modified information.</value>
        public Satellite Satellite 
        {
            get
            {
                return _Satellite;
            }
        }
    }

    public sealed class SatelliteListEventArgs : EventArgs
    {
        private IList<Satellite> _satellites;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="satellite"></param>
        public SatelliteListEventArgs(IList<Satellite> satellites)
        {
            _satellites = satellites;
        }

        /// <summary>
        /// Indicates which satellites are the target of the event.
        /// </summary>
        /// <value>A list of <strong>Satellite</strong> objects.</value>
        public IList<Satellite> Satellites
        {
            get
            {
                return _satellites;
            }
        }
    }

    /// <summary>Indicates the government project responsible for a GPS satellite.</summary>
    public enum SatelliteClass
    {
        /// <summary>The satellite could not be identified.</summary>
        Unknown,
        /// <summary>The satellite is part of the NAVSTAR project.</summary>
        Navstar,
        /// <summary>The satellite is part of the Wide Area Augmentation System (WAAS).</summary>
        Waas,
        /// <summary>
        /// The satellite is part of the European Geostationary Navigation Overlay Service
        /// (EGNOS).
        /// </summary>
        Egnos,
        /// <summary>
        /// The satellite is pard of Japan's MTSAT Satellite Augmentation System
        /// (MSAS).
        /// </summary>
        Msas
    }

    /// <summary>Indicates the launch block of a group of NAVSTAR GPS satellites.</summary>
    public enum SatelliteBlock
    {
        /// <summary>The block is unknown.</summary>
        Unknown,
        /// <summary>Represents Block I.</summary>
        I,
        /// <summary>Represents Block II.</summary>
        II,
        /// <summary>Represents Block IIA</summary>
        IIA,
        /// <summary>Represents Block IIR.</summary>
        IIR
    }

    /// <summary>Indicates the main atomic clock in service aboard a GPS satellite.</summary>
    public enum SatelliteAtomicClockType
    {
        /// <summary>The clock type is unknown.</summary>
        Unknown,
        /// <summary>The satellite's Cesium clock is currently in service.</summary>
        Cesium,
        /// <summary>The satellite's Rubidium clock is currently in service.</summary>
        Rubidium
    }
}