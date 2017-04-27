// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
#if !PocketPC || DesignTime

using System.ComponentModel;
using System.Linq;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents information about a GPS satellite in orbit above Earth.
    /// </summary>
    /// <remarks><para>GPS devices are able to isolate information about GPS satellites in orbit. Each
    /// satellite's <see cref="Satellite.PseudorandomNumber">unique identifier</see>,
    ///   <see cref="Satellite.SignalToNoiseRatio">radio signal strength</see>,
    ///   <see cref="Satellite.Azimuth">azimuth</see> and
    ///   <see cref="Satellite.Elevation">elevation</see> are available once its
    /// radio signal is detected.</para>
    ///   <para>Properties in this class are updated automatically as new information is
    /// received from the GPS device.</para>
    ///   <para><img src="Satellite.jpg"/></para></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public struct Satellite : IFormattable, IEquatable<Satellite>, IComparable<Satellite>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly int _pseudorandomNumber;
        /// <summary>
        ///
        /// </summary>
        private Azimuth _azimuth;
        /// <summary>
        ///
        /// </summary>
        private Elevation _elevation;
        /// <summary>
        ///
        /// </summary>
        private SignalToNoiseRatio _signalToNoiseRatio;
        /// <summary>
        ///
        /// </summary>
        private bool _isFixed;
        /// <summary>
        ///
        /// </summary>
        private DateTime _lastSignalReceived;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified unique identifier.
        /// </summary>
        /// <param name="pseudorandomNumber">The pseudorandom number.</param>
        public Satellite(int pseudorandomNumber)
            : this(pseudorandomNumber, Azimuth.Empty, Elevation.Empty, SignalToNoiseRatio.Empty, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber">The pseudorandom number.</param>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="elevation">The elevation.</param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation)
            : this(pseudorandomNumber, azimuth, elevation, SignalToNoiseRatio.Empty, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber">The pseudorandom number.</param>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="elevation">The elevation.</param>
        /// <param name="signalToNoiseRatio">The signal to noise ratio.</param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation, SignalToNoiseRatio signalToNoiseRatio)
            : this(pseudorandomNumber, azimuth, elevation, signalToNoiseRatio, false)
        { }

        /// <summary>
        /// Creates a new instance using the specified unique identifier, location around the horizon, and elevation up from the horizon.
        /// </summary>
        /// <param name="pseudorandomNumber">The pseudorandom number.</param>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="elevation">The elevation.</param>
        /// <param name="signalToNoiseRatio">The signal to noise ratio.</param>
        /// <param name="isFixed">if set to <c>true</c> [is fixed].</param>
        public Satellite(int pseudorandomNumber, Azimuth azimuth, Elevation elevation, SignalToNoiseRatio signalToNoiseRatio, bool isFixed)
        {
            _pseudorandomNumber = pseudorandomNumber;
            _azimuth = azimuth;
            _elevation = elevation;
            _signalToNoiseRatio = signalToNoiseRatio;
            _isFixed = isFixed;
            _lastSignalReceived = DateTime.MinValue;
        }

        #endregion Constructors

        #region Public Properties

#if !PocketPC

        /// <summary>
        /// Returns the unique identifier of the satellite.
        /// </summary>
        [Category("Data")]
        [Description("Returns the unique identifier of the satellite.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
#endif
        public int PseudorandomNumber
        {
            get
            {
                return _pseudorandomNumber;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the horizontal direction towards the satellite from the current location.
        /// </summary>
        /// <value>The azimuth.</value>
        [Category("Location")]
        [Description("Returns the horizontal direction towards the satellite from the current location.")]
        [Browsable(true)]
#endif
        public Azimuth Azimuth
        {
            get
            {
                return _azimuth;
            }
            set
            {
                if (_azimuth.Equals(value))
                    return;

                _azimuth = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the vertical direction towards the satellite from the current
        /// location.
        /// </summary>
        /// <value>The elevation.</value>
        [Category("Location")]
        [Description("Returns the vertical direction towards the satellite from the current location.")]
        [Browsable(true)]
#endif
        public Elevation Elevation
        {
            get
            {
                return _elevation;
            }
            set
            {
                if (_elevation.Equals(value))
                    return;

                _elevation = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the strength of the satellite's radio signal as it is being
        /// received.
        /// </summary>
        /// <value>The signal to noise ratio.</value>
        [Category("Radio Signal")]
        [Description("Returns the strength of the satellite's radio signal as it is being received.")]
        [Browsable(true)]
#endif
        public SignalToNoiseRatio SignalToNoiseRatio
        {
            get
            {
                return _signalToNoiseRatio;
            }
            set
            {
                if (_signalToNoiseRatio.Equals(value))
                    return;

                _signalToNoiseRatio = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the date and time that the satellite's signal was detected.
        /// </summary>
        /// <value>The last signal received.</value>
        [Category("Statistics")]
        [Description("Returns the date and time that the satellite's signal was detected.")]
        [Browsable(true)]
#endif
        public DateTime LastSignalReceived
        {
            get
            {
                return _lastSignalReceived;
            }
            set
            {
                if (_lastSignalReceived.Equals(value))
                    return;

                _lastSignalReceived = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns whether the satellite's signal is being used to calculate the current
        /// location.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed; otherwise, <c>false</c>.</value>
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is being used to calculate the current location.")]
        [Browsable(true)]
#endif
        public bool IsFixed
        {
            get
            {
                return _isFixed;
            }
            set
            {
                if (_isFixed.Equals(value))
                    return;

                _isFixed = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns whether the satellite's signal is currently being detected.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is currently being detected.")]
        [Browsable(true)]
#endif
        public bool IsTracked
        {
            get
            {
                return !_signalToNoiseRatio.IsEmpty;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the amount of time elapsed since the signal was last received.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal is currently being detected.")]
        [Browsable(true)]
#endif
        public TimeSpan SignalAge
        {
            get
            {
                return DateTime.Now.Subtract(_lastSignalReceived);
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns whether the satellite's signal has recently been received.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether the satellite's signal has recently been received.")]
        [Browsable(true)]
#endif
        public bool IsActive
        {
            get
            {
                return _isFixed || SignalAge.TotalSeconds < 10.0;
            }
        }

#if !PocketPC

        /// <summary>
        /// Indicates whether the current satellite with this PRN is providing additional corrective
        /// signals to increase precision.
        /// </summary>
        /// <remarks>This property will return a value of <strong>True</strong>
        /// if the GPS satellite has been identified as a WAAS, EGNOS or MSAS satellite.
        /// These satellites are geostationary (unlike typical NAVSTAR GPS satellites)
        /// and re-broadcast correction signals from ground stations.  When this property
        /// is true, the GPS device has improved precision.</remarks>
        [Category("Behavior")]
        [Description("Indicates whether the current satellite with this PRN is providing additional corrective signals to increase precision.")]
        [Browsable(true)]
#endif
        public bool IsDifferentialGpsSatellite
        {
            get
            {
                SatelliteClass satelliteClass = Class;
                return satelliteClass.Equals(SatelliteClass.Waas)
                       || satelliteClass.Equals(SatelliteClass.Egnos)
                       || satelliteClass.Equals(SatelliteClass.Msas);
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the government project responsible for launching the current satellite with this PRN.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the government project responsible for launching the current satellite with this PRN.")]
        [Browsable(true)]
#endif
        public SatelliteClass Class
        {
            get
            {
                switch (_pseudorandomNumber)
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
                    case 01: // USA-232
                    case 02: // USA-180
                    case 03: // USA-258
                    case 04: // USA-96 (In reserve)
                    case 05: // USA-206
                    case 06: // USA-251
                    case 07: // USA-201
                    case 08: // USA-262
                    case 09: // USA-256
                    case 10: // USA-265
                    case 11: // USA-145
                    case 12: // USA-192
                    case 13: // USA-132
                    case 14: // USA-154
                    case 15: // USA-196
                    case 16: // USA-166
                    case 17: // USA-183
                    case 18: // USA-156
                    case 19: // USA-177
                    case 20: // USA-150
                    case 21: // USA-168
                    case 22: // USA-175
                    case 23: // USA-178
                    case 24: // USA-239
                    case 25: // USA-213
                    case 26: // USA-260
                    case 27: // USA-242
                    case 28: // USA-151
                    case 29: // USA-199
                    case 30: // USA-248
                    case 31: // USA-190
                    case 32: // USA-266
                        return SatelliteClass.Navstar;
                    default:
                        return SatelliteClass.Unknown;
                }
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the atomic clock currently in service for the satellite with this PRN.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the atomic clock currently in service for the satellite with this PRN.")]
        [Browsable(true)]
#endif
        public SatelliteAtomicClockType AtomicClockType
        {
            get
            {
                // Data was compiled from the US Military: ftp:// tycho.usno.navy.mil/pub/gps/gpstd.txt
                switch (_pseudorandomNumber)
                {
                    case 01: // USA-232
                    case 02: // USA-180
                    case 03: // USA-258
                    case 04: // USA-96
                    case 05: // USA-206
                    case 06: // USA-251
                    case 07: // USA-201
                    case 09: // USA-256
                    case 10: // USA-265
                    case 11: // USA-145
                    case 12: // USA-192
                    case 13: // USA-132
                    case 14: // USA-154
                    case 15: // USA-196
                    case 16: // USA-166
                    case 17: // USA-183
                    case 18: // USA-156
                    case 19: // USA-177
                    case 20: // USA-150
                    case 21: // USA-168
                    case 22: // USA-175
                    case 23: // USA-178
                    case 25: // USA-213
                    case 26: // USA-260
                    case 27: // USA-242
                    case 28: // USA-151
                    case 29: // USA-199
                    case 30: // USA-248
                    case 31: // USA-190
                    case 32: // USA-266
                        return SatelliteAtomicClockType.Rubidium;
                    case 08: // USA-262
                    case 24: // USA-239
                        return SatelliteAtomicClockType.Cesium;
                    default:
                        return SatelliteAtomicClockType.Unknown;
                }
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the launch block of the current satellite with this PRN.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the launch block of the current satellite with this PRN.")]
        [Browsable(true)]
#endif
        public SatelliteBlock Block
        {
            get
            {
                // Data was compiled from Wikipedia: https://en.wikipedia.org/wiki/List_of_GPS_satellites
                switch (_pseudorandomNumber)
                {
                    case 01: // USA-232
                    case 03: // USA-258
                    case 06: // USA-251
                    case 08: // USA-262
                    case 09: // USA-256
                    case 10: // USA-265
                    case 24: // USA-239
                    case 25: // USA-213
                    case 26: // USA-260
                    case 27: // USA-242
                    case 30: // USA-248
                    case 32: // USA-266
                        return SatelliteBlock.IIF;
                    case 02: // USA-180
                    case 11: // USA-145
                    case 13: // USA-132
                    case 14: // USA-154
                    case 16: // USA-166
                    case 18: // USA-156
                    case 19: // USA-177
                    case 20: // USA-150
                    case 21: // USA-168
                    case 22: // USA-175
                    case 23: // USA-178
                    case 28: // USA-151
                        return SatelliteBlock.IIR;
                    case 05: // USA-206
                    case 07: // USA-201
                    case 12: // USA-192
                    case 15: // USA-196
                    case 17: // USA-183
                    case 29: // USA-199
                    case 31: // USA-190
                        return SatelliteBlock.IIRM;
                }

                return SatelliteBlock.Unknown;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the date the current satellite with this PRN was placed into orbit.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the date the current satellite with this PRN was placed into orbit.")]
        [Browsable(true)]
#endif
        public DateTime LaunchDate
        {
            get
            {
                // Data was compiled from the US Military: ftp:// tycho.usno.navy.mil/pub/gps/gpstd.txt
                switch (_pseudorandomNumber)
                {
                    case 01: // USA-232
                        return new DateTime(2011, 07, 16);
                    case 02: // USA-180
                        return new DateTime(2004, 11, 06);
                    case 03: // USA-258
                        return new DateTime(2014, 10, 29);
                    case 04:
                        // USA-96 (In reserve)
                        break;
                    case 05: // USA-206
                        return new DateTime(2009, 08, 17);
                    case 06: // USA-251
                        return new DateTime(2014, 05, 17);
                    case 07: // USA-201
                        return new DateTime(2008, 03, 15);
                    case 08: // USA-262
                        return new DateTime(2015, 07, 15);
                    case 09: // USA-256
                        return new DateTime(2014, 08, 02);
                    case 10: // USA-265
                        return new DateTime(2015, 10, 31);
                    case 11: // USA-145
                        return new DateTime(1999, 10, 07);
                    case 12: // USA-192
                        return new DateTime(2006, 11, 17);
                    case 13: // USA-132
                        return new DateTime(1997, 07, 23);
                    case 14: // USA-154
                        return new DateTime(2000, 11, 10);
                    case 15: // USA-196
                        return new DateTime(2007, 10, 17);
                    case 16: // USA-166
                        return new DateTime(2003, 01, 29);
                    case 17: // USA-183
                        return new DateTime(2005, 09, 26);
                    case 18: // USA-156
                        return new DateTime(2001, 01, 30);
                    case 19: // USA-177
                        return new DateTime(2004, 03, 20);
                    case 20: // USA-150
                        return new DateTime(2000, 05, 11);
                    case 21: // USA-168
                        return new DateTime(2003, 03, 31);
                    case 22: // USA-175
                        return new DateTime(2003, 12, 21);
                    case 23: // USA-178
                        return new DateTime(2004, 06, 23);
                    case 24: // USA-239
                        return new DateTime(2012, 10, 04);
                    case 25: // USA-213
                        return new DateTime(2010, 05, 28);
                    case 26: // USA-260
                        return new DateTime(2015, 03, 25);
                    case 27: // USA-242
                        return new DateTime(2013, 05, 15);
                    case 28: // USA-151
                        return new DateTime(2000, 07, 16);
                    case 29: // USA-199
                        return new DateTime(2007, 12, 20);
                    case 30: // USA-248
                        return new DateTime(2014, 02, 21);
                    case 31: // USA-190
                        return new DateTime(2006, 09, 25);
                    case 32: // USA-266
                        return new DateTime(2016, 02, 05);
                }

                return DateTime.MinValue;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the latest date when a satellite with this PRN was placed into service.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the latest date when a satellite with this PRN was placed into service.")]
        [Browsable(true)]
#endif
        public DateTime CommissionDate
        {
            get
            {
                // Data was compiled from the US Military: ftp:// tycho.usno.navy.mil/pub/gps/gpstd.txt
                switch (_pseudorandomNumber)
                {
                    case 01:
                        return new DateTime(2011, 10, 14);
                    case 02:
                        return new DateTime(2004, 11, 22);
                    case 03:
                        return new DateTime(2014, 12, 12);
                    case 04:
                        // Unknown
                        break;
                    case 05:
                        return new DateTime(2009, 08, 27);
                    case 06:
                        return new DateTime(2014, 06, 10);
                    case 07:
                        return new DateTime(2008, 03, 24);
                    case 08:
                        return new DateTime(2015, 08, 12);
                    case 09:
                        return new DateTime(2014, 09, 17);
                    case 10:
                        return new DateTime(2015, 12, 09);
                    case 11:
                        return new DateTime(2000, 01, 03);
                    case 12:
                        return new DateTime(2006, 12, 13);
                    case 13:
                        return new DateTime(1998, 01, 31);
                    case 14:
                        return new DateTime(2000, 12, 10);
                    case 15:
                        return new DateTime(2007, 10, 31);
                    case 16:
                        return new DateTime(2003, 02, 18);
                    case 17:
                        return new DateTime(2005, 12, 16);
                    case 18:
                        return new DateTime(2001, 02, 15);
                    case 19:
                        return new DateTime(2004, 04, 05);
                    case 20:
                        return new DateTime(2000, 06, 01);
                    case 21:
                        return new DateTime(2003, 04, 12);
                    case 22:
                        return new DateTime(2004, 01, 12);
                    case 23:
                        return new DateTime(2004, 07, 09);
                    case 24:
                        return new DateTime(2012, 11, 14);
                    case 25:
                        return new DateTime(2010, 08, 27);
                    case 26:
                        return new DateTime(2015, 04, 20);
                    case 27:
                        return new DateTime(2013, 06, 21);
                    case 28:
                        return new DateTime(2000, 08, 17);
                    case 29:
                        return new DateTime(2008, 01, 02);
                    case 30:
                        return new DateTime(2014, 05, 30);
                    case 31:
                        return new DateTime(2006, 10, 12);
                    case 32:
                        return new DateTime(2016, 03, 09);
                }

                return DateTime.MinValue;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the latest date when a satellite with this PRN was removed from service.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the latest date when a satellite with this PRN was removed from service.")]
        [Browsable(true)]
#endif
        public DateTime DecommissionDate
        {
            get
            {
                // Data was compiled from the US Military: ftp:// tycho.usno.navy.mil/pub/gps/gpsb2.txt
                switch (_pseudorandomNumber)
                {
                    case 01:
                        return new DateTime(2008, 03, 17);
                    case 02:
                        return new DateTime(2004, 05, 12);
                    case 03:
                        // Unknown
                        break;
                    case 04:
                        return new DateTime(2015, 11, 03);
                    case 05:
                        return new DateTime(2013, 05, 01);
                    case 06:
                        // Unknown
                    case 07:
                        return new DateTime(2007, 12, 20);
                    case 08:
                        // Unknown
                        break;
                    case 09:
                        // Unknown
                        break;
                    case 10:
                        // Unknown
                        break;
                    case 11:
                        // Unknown
                        break;
                    case 12:
                        // Unknown
                        break;
                    case 13:
                        // Unknown
                        break;
                    case 14:
                        return new DateTime(2000, 04, 14);
                    case 15:
                        return new DateTime(2007, 03, 15);
                    case 16:
                        return new DateTime(2000, 10, 13);
                    case 17:
                        return new DateTime(2005, 02, 23);
                    case 18:
                        return new DateTime(2000, 08, 18);
                    case 19:
                        return new DateTime(2001, 09, 11);
                    case 20:
                        return new DateTime(1996, 12, 13);
                    case 21:
                        return new DateTime(2003, 01, 27);
                    case 22:
                        return new DateTime(2003, 08, 06);
                    case 23:
                        return new DateTime(2004, 02, 13);
                    case 24:
                        // Unknown
                        break;
                    case 25:
                        return new DateTime(2009, 12, 18);
                    case 26:
                        // Unknown
                        break;
                    case 27:
                        // Unknown
                        break;
                    case 28:
                        return new DateTime(1997, 08, 15);
                    case 29:
                        return new DateTime(2007, 10, 23);
                    case 30:
                        // Unknown
                        break;
                    case 31:
                        return new DateTime(2005, 10, 24);
                    case 32:
                        // Unknown
                        break;
                }

                return DateTime.MinValue;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the friendly name of the current satellite with this PRN.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the friendly name of the current satellite with this PRN.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
#endif
        public string Name
        {
            get
            {
                // Data was compiled from Wikipedia: https://en.wikipedia.org/wiki/List_of_GPS_satellites
                switch (_pseudorandomNumber)
                {
                    #region Standard NAVSTAR satellites
                    
                    case 01:
                        return "USA-232";
                    case 02:
                        return "USA-180";
                    case 03:
                        return "USA-258";
                    case 04:
                        // USA-96 (In reserve)
                        break;
                    case 05:
                        return "USA-206";
                    case 06:
                        return "USA-251";
                    case 07:
                        return "USA-201";
                    case 08:
                        return "USA-262";
                    case 09:
                        return "USA-256";
                    case 10:
                        return "USA-265";
                    case 11:
                        return "USA-145";
                    case 12:
                        return "USA-192";
                    case 13:
                        return "USA-132";
                    case 14:
                        return "USA-154";
                    case 15:
                        return "USA-196";
                    case 16:
                        return "USA-166";
                    case 17:
                        return "USA-183";
                    case 18:
                        return "USA-156";
                    case 19:
                        return "USA-177";
                    case 20:
                        return "USA-150";
                    case 21:
                        return "USA-168";
                    case 22:
                        return "USA-175";
                    case 23:
                        return "USA-178";
                    case 24:
                        return "USA-239";
                    case 25:
                        return "USA-213";
                    case 26:
                        return "USA-260";
                    case 27:
                        return "USA-242";
                    case 28:
                        return "USA-151";
                    case 29:
                        return "USA-199";
                    case 30:
                        return "USA-248";
                    case 31:
                        return "USA-190";
                    case 32:
                        return "USA-266";

                    #endregion Standard NAVSTAR satellites

                    #region Wide Area Augmentation System (WAAS)

                    // Data was compiled from Wikipedia: http://en.wikipedia.org/wiki/WAAS

                    case 35:
                        return "Atlantic Ocean Region-West (WAAS)";
                    case 51:
                        return "Anik F1R (WAAS)";
                    case 47:
                        return "Pacific Ocean Region (WAAS)";
                    case 48:
                        return "Galaxy 15 (WAAS)";

                    #endregion Wide Area Augmentation System (WAAS)

                    #region EGNOS (European)

                    case 33:
                        return "Atlantic Ocean Region-East (EGNOS)";
                    case 37:
                        return "ARTEMIS (EGNOS)";
                    case 39:
                        return "Indian Ocean Region-West (EGNOS)";
                    case 44:
                        return "Indian Ocean Region-East (EGNOS)";

                    #endregion EGNOS  (European)

                    #region MSAS (Japan)

                    case 42:
                        return "MTSAT-1";
                    case 50:
                        return "MTSAT-2";

                    #endregion MSAS (Japan)

                    case 0:
                        // A placeholder for the SatelliteSignalBar control that
                        // is displaying no satellites. Otherwise the control is
                        // not visible.
                        return "Unknown";
                }

                return "New Satellite";
            }
        }

        /// <summary>
        /// Returns the aliases of the current satellite with this PRN.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the aliases of the current satellite with this PRN.")]
        [Browsable(true)]
        public string[] Aliases
        {
            get
            {
                // Data was compiled from Wikipedia: https://en.wikipedia.org/wiki/List_of_GPS_satellites
                switch (_pseudorandomNumber)
                {
                    case 01:
                        return new[]
                        {
                            "USA-232",
                            "GPS IIF-2",
                            "GPS SVN-63"
                        };
                    case 02:
                        return new[]
                        {
                            "USA-180",
                            "GPS IIR-13",
                            "GPS SVN-61"
                        };
                    case 03:
                        return new[]
                        {
                            "USA-258",
                            "GPS IIF-8",
                            "GPS SVN-69",
                            "NAVSTAR 72"
                        };
                    case 04:
                        // USA-96 (In reserve)
                        break;
                    case 05:
                        return new[]
                        {
                            "USA-206",
                            "GPS SVN-50",
                            "PRN-05",
                            "NAVSTAR 64"
                        };
                    case 06:
                        return new[]
                        {
                            "USA-251",
                            "GPS IIF-6",
                            "GPS SVN-6",
                            "NAVSTAR 70"
                        };
                    case 07:
                        return new[]
                        {
                            "USA-201",
                            "GPS IIR-19(M)",
                            "GPS IIRM-6",
                            "GPS SVN-48"
                        };
                    case 08:
                        return new[]
                        {
                            "USA-262",
                            "GPS IIF-10",
                            "GPS SVN-72",
                            "NAVSTAR 74"
                        };
                    case 09:
                        return new[]
                        {
                            "USA-256",
                            "GPS IIF-7",
                            "GPS SVN-68",
                            "NAVSTAR 71"
                        };
                    case 10:
                        return new[]
                        {
                            "USA-265",
                            "GPS IIF-11",
                            "GPS SVN-73",
                            "NAVSTAR 75"
                        };
                    case 11:
                        return new[]
                        {
                            "USA-145",
                            "GPS IIR-3",
                            "GPS SVN-46"
                        };
                    case 12:
                        return new[]
                        {
                            "USA-192",
                            "GPS IIR-16(M)",
                            "GPS IIRM-3",
                            "GPS SVN-58"
                        };
                    case 13:
                        return new[]
                        {
                            "USA-132",
                            "GPS IIR-2",
                            "GPS SVN-43"
                        };
                    case 14:
                        return new[]
                        {
                            "USA-154",
                            "GPS IIR-6",
                            "GPS SVN-41"
                        };
                    case 15:
                        return new[]
                        {
                            "USA-196",
                            "GPS IIR-17(M)",
                            "GPS IIRM-4",
                            "GPS SVN-55"
                        };
                    case 16:
                        return new[]
                        {
                            "USA-166",
                            "GPS IIR-8",
                            "GPS SVN-56"
                        };
                    case 17:
                        return new[]
                        {
                            "USA-183",
                            "GPS IIR-14(M)",
                            "GPS IIRM-1",
                            "GPS SVN-53"
                        };
                    case 18:
                        return new[]
                        {
                            "USA-156",
                            "GPS IIR-7",
                            "GPS SVN-54"
                        };
                    case 19:
                        return new[]
                        {
                            "USA-177",
                            "GPS IIR-11",
                            "GPS SVN-59"
                        };
                    case 20:
                        return new[]
                        {
                            "USA-150",
                            "GPS IIR-4",
                            "GPS SVN-51"
                        };
                    case 21:
                        return new[]
                        {
                            "USA-168",
                            "GPS IIR-9",
                            "GPS SVN-45"
                        };
                    case 22:
                        return new[]
                        {
                            "USA-175",
                            "GPS IIR-10",
                            "GPS SVN-47"
                        };
                    case 23:
                        return new[]
                        {
                            "USA-178",
                            "GPS IIR-12",
                            "GPS SVN-60"
                        };
                    case 24:
                        return new[]
                        {
                            "USA-239",
                            "GPS IIF-3",
                            "GPS SVN-65"
                        };
                    case 25:
                        return new[]
                        {
                            "USA-213",
                            "GPS IIF SV-1",
                            "GPS SVN-62",
                            "NAVSTAR 65"
                        };
                    case 26:
                        return new[]
                        {
                            "USA-260",
                            "GPS IIF-9",
                            "GPS SVN-71",
                            "NAVSTAR 73"
                        };
                    case 27:
                        return new[]
                        {
                            "USA-242",
                            "GPS IIF-4",
                            "GPS IIF SV-5",
                            "Vega"
                        };
                    case 28:
                        return new[]
                        {
                            "USA-151",
                            "GPS IIR-5",
                            "GPS SVN-44"
                        };
                    case 29:
                        return new[]
                        {
                            "USA-199",
                            "GPS IIR-18(M)",
                            "GPS IIRM-5",
                            "GPS SVN-57"
                        };
                    case 30:
                        return new[]
                        {
                            "USA-248",
                            "GPS IIF-5",
                            "GPS SVN-64",
                            "NAVSTAR 69"
                        };
                    case 31:
                        return new[]
                        {
                            "USA-190",
                            "GPS IIR-15(M)",
                            "GPS IIRM-2",
                            "GPS SVN-52"
                        };
                    case 32:
                        return new[]
                        {
                            "USA-266",
                            "GPS IIF-12",
                            "GPS SVN-70",
                            "NAVSTAR 76"
                        };
                }

                return default(string[]);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// REturns the string equivalent of this object using the current culture
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Satellite)
                return Equals((Satellite)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _pseudorandomNumber;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static methods

        /// <summary>
        /// Returns the satellites in the specified in the list which are marked as fixed.
        /// </summary>
        /// <param name="satellites">The satellites.</param>
        /// <returns></returns>
        public static List<Satellite> GetFixedSatellites(List<Satellite> satellites)
        {
            return satellites.Where(satellite => satellite.IsFixed).ToList();
        }

        #endregion Static methods

        #region IEquatable<Satellite> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Satellite other)
        {
            return _pseudorandomNumber.Equals(other.PseudorandomNumber);
        }

        #endregion IEquatable<Satellite> Members

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            return Name + " (" + _pseudorandomNumber.ToString(format, formatProvider) + "): "
                        + _azimuth.ToString(format, formatProvider) + culture.TextInfo.ListSeparator + " "
                        + _elevation.ToString(format, formatProvider) + culture.TextInfo.ListSeparator + " "
                        + _signalToNoiseRatio.ToString(format, formatProvider);
        }

        #endregion IFormattable Members

        #region IComparable<Satellite> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.</returns>
        public int CompareTo(Satellite other)
        {
            // Fixed satellites come first, then non-fixed satellites.
            // Second, satellites are sorted by signal, strongest signal first.
            if (_isFixed && !other.IsFixed)
                return 1;
            if (!_isFixed && other.IsFixed)
                return -1;
            return other.SignalToNoiseRatio.CompareTo(_signalToNoiseRatio);
        }

        #endregion IComparable<Satellite> Members
    }

    /// <summary>
    /// Represents information about a satellite when a satellite-related event is raised.
    /// </summary>
    /// <example>This example demonstrates how to use this class when raising an event.
    ///   <code lang="VB">
    /// ' Start a new receiver
    /// Dim MyReceiver As New Receiver()
    /// ' Declare a new event
    /// Dim MySatelliteEvent As EventHandler
    /// ' Get a handle on the first satellite in the receiver's collection
    /// Dim MySatellite As Satellite = MyReceiver.Satellites(0)
    /// Sub Main()
    /// ' Raise our custom event
    /// RaiseEvent MySatelliteEvent(Me, New SatelliteEventArgs(MySatellite))
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Start a new receiver
    /// Receiver MyReceiver = new Receiver();
    /// // Declare a new event
    /// EventHandler MySatelliteEvent;
    /// // Create an Satellite of 90°
    /// Satellite MySatellite = new Satellite(90);
    /// void Main()
    /// {
    /// // Raise our custom event
    /// MySatelliteEvent(this, New SatelliteEventArgs(MySatellite));
    /// }
    ///   </code>
    ///   </example>
    ///
    /// <seealso cref="Satellite">SatelliteCollection Class</seealso>
    ///
    /// <seealso cref="Azimuth">Azimuth Property (Satellite Class)</seealso>
    ///
    /// <seealso cref="Elevation">Elevation Property (Satellite Class)</seealso>
    ///
    /// <seealso cref="SignalToNoiseRatio">SignalToNoiseRatio Property (Satellite Class)</seealso>
    /// <remarks>This object is used primarily by the <see cref="Satellite">Satellite</see>
    /// class to provide notification when information such as <see cref="Azimuth">azimuth</see>,
    /// <see cref="Elevation">elevation</see>, or <see cref="SignalToNoiseRatio">radio signal strength</see>
    /// has changed.</remarks>
    public sealed class SatelliteEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Satellite _satellite;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="satellite">The satellite.</param>
        public SatelliteEventArgs(Satellite satellite)
        {
            _satellite = satellite;
        }

        /// <summary>
        /// Indicates which satellite is the target of the event.
        /// </summary>
        /// <value>A <strong>Satellite</strong> object containing modified information.</value>
        public Satellite Satellite
        {
            get
            {
                return _satellite;
            }
        }
    }

    /// <summary>
    /// The event args for a list of satellites
    /// </summary>
    public sealed class SatelliteListEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly IList<Satellite> _satellites;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="satellites">The satellites.</param>
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

    /// <summary>
    /// Indicates the government project responsible for a GPS satellite.
    /// </summary>
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

    /// <summary>
    /// Indicates the launch block of a group of NAVSTAR GPS satellites.
    /// </summary>
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
        IIR,
        /// <summary>Represents Block IIR-M.</summary>
        IIRM,
        /// <summary>Represents Block IIF.</summary>
        IIF
    }

    /// <summary>
    /// Indicates the main atomic clock in service aboard a GPS satellite.
    /// </summary>
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