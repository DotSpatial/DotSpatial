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
        /// Indicates whether the satellite is providing additional corrective
        /// signals to increase precision.
        /// </summary>
        /// <remarks>This property will return a value of <strong>True</strong>
        /// if the GPS satellite has been identified as a WAAS, EGNOS or MSAS satellite.
        /// These satellites are geostationary (unlike typical NAVSTAR GPS satellites)
        /// and re-broadcast correction signals from ground stations.  When this property
        /// is true, the GPS device has improved precision.</remarks>
        [Category("Behavior")]
        [Description("Indicates whether the satellite is providing additional corrective signals to increase precision.")]
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
        /// Returns the government project responsible for launching the satellite.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the government project responsible for launching the satellite.")]
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

#if !PocketPC

        /// <summary>
        /// Returns the atomic clock currently in service.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the atomic clock currently in service.")]
        [Browsable(true)]
#endif
        public SatelliteAtomicClockType AtomicClockType
        {
            get
            {
                switch (_pseudorandomNumber)
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

#if !PocketPC

        /// <summary>
        /// Returns the launch block of the satellite.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the launch block of the satellite.")]
        [Browsable(true)]
#endif
        public SatelliteBlock Block
        {
            get
            {
                switch (_pseudorandomNumber)
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

#if !PocketPC

        /// <summary>
        /// Returns the date the satellite was placed into orbit.
        /// </summary>
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

                switch (_pseudorandomNumber)
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

#if !PocketPC

        /// <summary>
        /// Returns the date the satellite was placed into service.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the date the satellite was placed into service.")]
        [Browsable(true)]
#endif
        public DateTime DateCommissioned
        {
            get
            {
                switch (_pseudorandomNumber)
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

                    #endregion Temporarily offline or decommissioned

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

                    #endregion Currently operational

                    default:
                        return DateTime.MinValue;
                }
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the date the satellite was removed from service.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the date the satellite was removed from service.")]
        [Browsable(true)]
#endif
        public DateTime DateDecommissioned
        {
            get
            {
                switch (_pseudorandomNumber)
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

#if !PocketPC

        /// <summary>
        /// Returns the friendly name of the satellite.
        /// </summary>
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

                switch (_pseudorandomNumber)
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

                    #endregion Standard NAVSTAR satellites

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

                    #endregion Wide Area Augmentation System (WAAS)

                    #region EGNOS  (European)

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
                        // A placeholder for the SatellitSignalBar control that
                        // is displaying no satellites. Otherwise the control is
                        // not visible.
                        return "Unknown";
                    default:
                        return "New Satellite";
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// REturns the string equivalent of this object using ht ecurrent cul
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
        IIR
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