// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
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
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DotSpatial.Positioning;
#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a measurement of an object's rate of travel.
    /// </summary>
    /// <remarks><para>This structure is used to measure the rate at which something moves in a
    /// given period of time. This structure supports several different unit types in both
    /// Imperial and Metric measurement systems. A speed is measured in two parts: a
    /// numeric value and a label indicating the units of measurement.</para>
    ///   <para>Speed measurements can be converted to their equivalent values in other unit
    /// types through the use of several conversion methods such as ToMetersPerSecond,
    ///   <strong>ToFeetPerSecond</strong>, <strong>ToKilometersPerHour</strong>, and others.
    /// Three methods, <strong>ToImperialUnitType</strong>,
    ///   <strong>ToMetricUnitType</strong> and <strong>ToLocalUnitType</strong> also exist
    /// for converting a speed measurement to the most readable unit type (i.e. 1 meter vs.
    /// 0.0001 kilometers) in any local culture.</para>
    ///   <para>This structure is a <em>DotSpatial.Positioning</em> "parseable type" whose value can
    /// be freely converted to and from <strong>String</strong> objects via the
    ///   <strong>ToString</strong> and <strong>Parse</strong> methods.</para>
    ///   <para>Instances of this structure are guaranteed to be thread-safe because it is
    /// immutable (its properties can only be modified via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.SpeedConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Speed : IFormattable, IComparable<Speed>, IEquatable<Speed>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _value;
        /// <summary>
        ///
        /// </summary>
        private SpeedUnit _units;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MPH_PER_KNOT = 0.8689762;
        /// <summary>
        ///
        /// </summary>
        private const double KPH_PER_KNOT = 0.5399568;
        /// <summary>
        ///
        /// </summary>
        private const double FPS_PER_KNOT = 0.5924838;
        /// <summary>
        ///
        /// </summary>
        private const double MPS_PER_KNOT = 1.943845;
        /// <summary>
        ///
        /// </summary>
        private const double KPS_PER_KNOT = 1943.845;

        /// <summary>
        ///
        /// </summary>
        private const double KNOTS_PER_STATUTE_MPH = 1.150779;
        /// <summary>
        ///
        /// </summary>
        private const double KPH_PER_STATUTE_MPH = 0.6213712;
        /// <summary>
        ///
        /// </summary>
        private const double FPS_PER_STATUTE_MPH = 0.6818182;
        /// <summary>
        ///
        /// </summary>
        private const double MPS_PER_STATUTE_MPH = 2.236936;
        /// <summary>
        ///
        /// </summary>
        private const double KPS_PER_STATUTE_MPH = 2236.936;

        /// <summary>
        ///
        /// </summary>
        private const double KNOTS_PER_KPH = 1.852;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MPH_PER_KPH = 1.609344;
        /// <summary>
        ///
        /// </summary>
        private const double FPS_PER_KPH = 1.09728;
        /// <summary>
        ///
        /// </summary>
        private const double MPS_PER_KPH = 3.6;
        /// <summary>
        ///
        /// </summary>
        private const double KPS_PER_KPH = 3600;

        /// <summary>
        ///
        /// </summary>
        private const double KNOTS_PER_KPS = 0.0005144444;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MPH_PER_KPS = 0.000447;
        /// <summary>
        ///
        /// </summary>
        private const double FPS_PER_KPS = 0.0003048;
        /// <summary>
        ///
        /// </summary>
        private const double MPS_PER_KPS = 0.001;
        /// <summary>
        ///
        /// </summary>
        private const double KPH_PER_KPS = 0.0002777778;

        /// <summary>
        ///
        /// </summary>
        private const double KNOTS_PER_FPS = 1.68781;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MPH_PER_FPS = 1.466667;
        /// <summary>
        ///
        /// </summary>
        private const double KPH_PER_FPS = 0.9113444;
        /// <summary>
        ///
        /// </summary>
        private const double MPS_PER_FPS = 3.28084;
        /// <summary>
        ///
        /// </summary>
        private const double KPS_PER_FPS = 3280.84;

        /// <summary>
        ///
        /// </summary>
        private const double KNOTS_PER_MPS = 0.5144444;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MPH_PER_MPS = 0.447;
        /// <summary>
        ///
        /// </summary>
        private const double FPS_PER_MPS = 0.3048;
        /// <summary>
        ///
        /// </summary>
        private const double KPH_PER_MPS = 0.2777778;
        /// <summary>
        ///
        /// </summary>
        private const double KPS_PER_MPS = 1000;

        // private const int DefaultPrecisionDigits = 10;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Represents a speed of zero.
        /// </summary>
        public static readonly Speed Empty = new(0, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Represents a speed of zero.
        /// </summary>
        public static readonly Speed AtRest = new(0, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Returns the rate of travel of light in a vacuum.
        /// </summary>
        public static readonly Speed SpeedOfLight = new(299792458, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Represents the largest possible speed.
        /// </summary>
        public static readonly Speed Maximum = new Speed(Double.MaxValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();
        /// <summary>
        /// Represents the smallest possible speed.
        /// </summary>
        public static readonly Speed Minimum = new Speed(Double.MinValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();

        /// <summary>
        /// Returns the rate of travel of sound waves at sea level.
        /// </summary>
        public static readonly Speed SpeedOfSoundAtSeaLevel = new(340.29, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Represents an infinite speed.
        /// </summary>
        public static readonly Speed Infinity = new(double.PositiveInfinity, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Speed Invalid = new(double.NaN, SpeedUnit.KilometersPerSecond);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value and unit type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="units">The units.</param>
        /// <remarks>This is the most frequently used constructor of the speed class.</remarks>
        public Speed(double value, SpeedUnit units)
        {
            _value = value;
            _units = units;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>This powerful method is designed to simplify the process of parsing values read
        /// from a data store or typed in by the user.</remarks>
        public Speed(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public Speed(string value, CultureInfo culture)
        {
            string unit;

            // Anything to do°
            if (string.IsNullOrEmpty(value))
            {
                _value = 0;
                _units = SpeedUnit.MetersPerSecond;
                return;
            }
            try
            {
                // Convert to uppercase and remove commas
                value = value.ToUpper(CultureInfo.InvariantCulture).Replace(culture.NumberFormat.NumberGroupSeparator, string.Empty);
                if (value == "INFINITY")
                {
                    _value = Infinity.Value;
                    _units = Infinity.Units;
                }
                if (value == "EMPTY")
                {
                    _value = 0;
                    _units = SpeedUnit.MetersPerSecond;
                }

                int count = value.Length - 1;
                while (count >= 0)
                {
                    if (Char.IsNumber(value, count))
                    {
                        count++;
                        break;
                    }
                    count--;
                }
                unit = value.Substring(count).Trim();
                string numericPortion = value.Substring(0, count);
                double.TryParse(numericPortion, out _value);

                string tempUnits = unit.ToUpper(CultureInfo.InvariantCulture).Trim()
                    // Replace "per" synonyms
                    .Replace(" PER ", "/")
                    .Replace("-PER-", "/")
                    .Replace(" / ", "/")
                    .Replace(".", string.Empty)
                    // Replace "hour" synonyms
                    .Replace("HOURS", "H")
                    .Replace("HOUR", "H")
                    .Replace("HR", "H")
                    // Replace "second" synonyms
                    .Replace("SECONDS", "S")
                    .Replace("SECOND", "S")
                    .Replace("SEC", "S")
                    // Replace "feet" synonyms
                    .Replace("'", "FT")
                    .Replace("FEET", "FT")
                    .Replace("FOOT", "FT")
                    // Replace "kilometer" synonyms
                    .Replace("KILOMETRES", "KM")
                    .Replace("KILOMETERS", "KM")
                    .Replace("KM/H", "KPH")
                    .Replace("KMH", "KPH")
                    .Replace("KPS", "KM/S")
                    .Replace("KILOMETER", "KM")
                    .Replace("KILOMETRE", "KM")
                    // Replace "meter" synonyms
                    .Replace("METERS", "M")
                    .Replace("METRES", "M")
                    .Replace("METER", "M")
                    .Replace("METRE", "M")
                    // Replace "miles" synonyms
                    .Replace("MPH", "MI/H")
                    .Replace("MILES", "MI")
                    .Replace("MILE", "MI")
                    .Replace("STATUTE MILES", "MI")
                    .Replace("STATUTE MILE", "MI")
                    // Replace "knot" synonyms
                    .Replace("KNOTS", "K")
                    .Replace("KNOT", "K")
                    .Replace("KTS", "K")
                    .Replace("KT", "K");
                // Try to interpret the measurement
                switch (tempUnits)
                {
                    case "FT/S":
                        _units = SpeedUnit.FeetPerSecond;
                        break;
                    case "KPH":
                        _units = SpeedUnit.KilometersPerHour;
                        break;
                    case "KM/S":
                        _units = SpeedUnit.KilometersPerSecond;
                        break;
                    case "K":
                        _units = SpeedUnit.Knots;
                        break;
                    case "M/S":
                        _units = SpeedUnit.MetersPerSecond;
                        break;
                    case "MI/H":
                        _units = SpeedUnit.StatuteMilesPerHour;
                        break;
                    default:
                        throw new FormatException(Resources.Speed_InvalidUnitPortion);
                }
            }
            catch (Exception ex)
            {
#if PocketPC
				throw new ArgumentException(Properties.Resources.Speed_InvalidFormat, ex);
#else
                throw new ArgumentException(Resources.Speed_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> struct.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Speed(XmlReader reader)
        {
            // Initialize all fields
            _value = Double.NaN;
            _units = 0;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the numeric portion of the speed measurement.
        /// </summary>
        /// <remarks>This property is combined with the
        /// <see cref="Units">Units</see> property to form a complete
        /// speed measurement.</remarks>
        public double Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Returns the units portion of the speed measurement.
        /// </summary>
        /// <value>A value from the <see cref="SpeedUnit">SpeedUnits</see> enumeration.</value>
        /// <remarks><para>Following proper scientific practices, speed measurements are always made
        /// using a value paired with a unit type. </para>
        ///   <para><img src="BestPractice.jpg"/></para><para><strong>Always explicitly
        /// convert to a specific speed unit type before performing
        /// mathematics.</strong></para>
        ///   <para>Since the Units property of the Speed class can be modified, it is not
        /// safe to assume that a speed measurement will always be of a certain unit type.
        /// Therefore, use a conversion method such as <see cref="Speed.ToKilometersPerHour">
        /// ToKilometersPerHour</see> or <see cref="Speed.ToStatuteMilesPerHour">
        /// ToStatuteMilesPerHour</see> to ensure that the speed is in the correct unit
        /// type before performing mathematics.</para></remarks>
        public SpeedUnit Units
        {
            get
            {
                return _units;
            }
        }

        /// <summary>
        /// Indicates if the measurement is zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _value == 0;
            }
        }

        /// <summary>
        /// Indicates if the unit of measurement is a Metric unit type.
        /// </summary>
        public bool IsMetric
        {
            get
            {
                return _units == SpeedUnit.KilometersPerHour
                    || _units == SpeedUnit.KilometersPerSecond
                    || _units == SpeedUnit.MetersPerSecond;
            }
        }

        /// <summary>
        /// Indicates if the measurement is infinite.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_value);
            }
        }

        /// <summary>
        /// Indicates if the current instance is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_value); }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public Speed Clone()
        {
            return new Speed(Value, Units);
        }

        /// <summary>
        /// Returns a new instance rounded to the specified number of digits.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of digits to round off to.</param>
        /// <returns></returns>
        public Speed Round(int decimals)
        {
            return new Speed(Math.Round(_value, decimals), _units);
        }

        /// <summary>
        /// Returns the current instance converted to feet per second.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToFeetPerSecond() //'Implements ISpeed.ToFeetPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return this;
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_FPS, SpeedUnit.FeetPerSecond);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into kilometers per hour.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToKilometersPerHour() //'Implements ISpeed.ToKilometersPerHour
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerHour:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_KPH, SpeedUnit.KilometersPerHour);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into kilometers per second.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToKilometersPerSecond() //'Implements ISpeed.ToKilometersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_KPS, SpeedUnit.KilometersPerSecond);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Returns the current instance converted to knots.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToKnots() //'Implements ISpeed.ToKnots
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.Knots:
                    return this;
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Returns the current instance converted to meters per second.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToMetersPerSecond() //'Implements ISpeed.ToMetersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return this;
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_MPS, SpeedUnit.MetersPerSecond);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Returns the current instance converted to miles per hours (MPH).
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToStatuteMilesPerHour()
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return this;
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Returns the current instance converted to the specified unit type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed ToUnitType(SpeedUnit value)
        {
            switch (value)
            {
                case SpeedUnit.FeetPerSecond:
                    return ToFeetPerSecond();
                case SpeedUnit.KilometersPerHour:
                    return ToKilometersPerHour();
                case SpeedUnit.KilometersPerSecond:
                    return ToKilometersPerSecond();
                case SpeedUnit.Knots:
                    return ToKnots();
                case SpeedUnit.MetersPerSecond:
                    return ToMetersPerSecond();
                case SpeedUnit.StatuteMilesPerHour:
                    return ToStatuteMilesPerHour();
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Returns the current instance converted to the most readable Imperial unit
        /// type.
        /// </summary>
        /// <returns>A <strong>Speed</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a Speed becomes smaller, it may make more sense to the
        /// user to be expressed in a smaller unit type.  For example, a Speed of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// determine the smallest Imperial unit type.</remarks>
        public Speed ToImperialUnitType()
        {
            // Start with the largest possible unit
            Speed temp = ToStatuteMilesPerHour();
            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToFeetPerSecond();
            return temp;
        }

        /// <summary>
        /// Returns the current instance converted to the most readable Metric unit
        /// type.
        /// </summary>
        /// <returns>A <strong>Speed</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a Speed becomes smaller, it may make more sense to the
        /// user to be expressed in a smaller unit type.  For example, a Speed of
        /// 0.001 kilometers per second might be better expressed as 1 meter per second.  This method will
        /// determine the smallest metric unit type.</remarks>
        public Speed ToMetricUnitType()
        {
            // Start with the largest possible unit
            Speed temp = ToKilometersPerHour();
            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToMetersPerSecond();
            // And so on until we find the right unit
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToKilometersPerSecond();
            return temp;
        }

        /// <summary>
        /// Returns the current instance converted to the most readable Imperial or Metric
        /// unit type depending on the local culture.
        /// </summary>
        /// <returns>A <strong>Speed</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a Speed becomes smaller, it may make more sense to the
        /// user to be expressed in a smaller unit type.  For example, a Speed of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// find the smallest unit type and convert the unit to the user's local
        /// numeric system (Imperial or Metric).</remarks>
        public Speed ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            return ToImperialUnitType();
        }

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified
        /// format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the total distance traveled at the current speed for the specified
        /// time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>A <strong>Distance</strong> representing the distance travelled at
        /// the current speed for the specified length of time.</returns>
        public Distance ToDistance(TimeSpan time)
        {
            return new Distance(ToMetersPerSecond().Value * time.TotalMilliseconds / 1000.0, DistanceUnit.Meters).ToLocalUnitType();
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current instance to the specified arbitrary value.
        /// </summary>
        /// <param name="obj">An <strong>Object</strong> representing a value to compare.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Speed)
                return Equals((Speed)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code for the current instance used in hash tables.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return ToMetersPerSecond().Value.GetHashCode();
        }

        /// <summary>
        /// Outputs the speed measurement as a formatted string.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion Overrides

        #region Static Members

        /// <summary>
        /// Creates a speed measurement based on a string value.
        /// </summary>
        /// <param name="value"><para>A <strong>String</strong> in any of the following formats (or variation
        /// depending on the local culture):</para>
        ///   <para>
        ///   <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>vu</td>
        ///   <td>vv.vu</td>
        ///   <td>v u</td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para>
        ///   <para>where <strong>vv.v</strong> is a decimal value and <strong>u</strong> is a
        /// unit type made from words in the following list:</para>
        ///   <para>
        ///   <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>FEET</td>
        ///   <td>FOOT</td>
        ///   <td>METER</td>
        ///   </tr>
        ///   <tr>
        ///   <td>METERS</td>
        ///   <td>METRE</td>
        ///   <td>METRES</td>
        ///   </tr>
        ///   <tr>
        ///   <td>KILOMETER</td>
        ///   <td>KILOMETRE</td>
        ///   <td>KILOMETERS</td>
        ///   </tr>
        ///   <tr>
        ///   <td>KILOMETRES</td>
        ///   <td>KNOT</td>
        ///   <td>KNOTS</td>
        ///   </tr>
        ///   <tr>
        ///   <td>MILE</td>
        ///   <td>MILES</td>
        ///   <td>STATUTE MILE</td>
        ///   </tr>
        ///   <tr>
        ///   <td>STATUTE MILES</td>
        ///   <td>F</td>
        ///   <td>FT</td>
        ///   </tr>
        ///   <tr>
        ///   <td>M</td>
        ///   <td>KM</td>
        ///   <td>K</td>
        ///   </tr>
        ///   <tr>
        ///   <td>PER</td>
        ///   <td>-PER-</td>
        ///   <td>/</td>
        ///   </tr>
        ///   <tr>
        ///   <td>SECOND</td>
        ///   <td>SEC</td>
        ///   <td>S</td>
        ///   </tr>
        ///   <tr>
        ///   <td>HOUR</td>
        ///   <td>HR</td>
        ///   <td>H</td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para>
        ///   <para>For example, "12 miles per hour" is acceptable because the words "miles,"
        /// "per," and "hour" are found in the above list. Some combinations are not supported,
        /// such as "feet/hour." The word combination should look similar to a value from the
        ///   <see cref="SpeedUnit">SpeedUnit</see> enumeration.</para></param>
        /// <returns>A new <strong>Speed</strong> object with the specified value and
        /// units.</returns>
        /// <remarks>This powerful method simplifies the process of processing values read from a data
        /// store or entered via the user. This method even supports some natural language
        /// processing ability by understanding words (see list above). This method can parse any
        /// value created via the ToString method.</remarks>
        public static Speed Parse(string value)
        {
            return new Speed(value);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Speed Parse(string value, CultureInfo culture)
        {
            return new Speed(value, culture);
        }

        /// <summary>
        /// Froms the knots.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromKnots(double value)
        {
            return new Speed(value, SpeedUnit.Knots);
        }

        /// <summary>
        /// Froms the statute miles per hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromStatuteMilesPerHour(double value)
        {
            return new Speed(value, SpeedUnit.StatuteMilesPerHour);
        }

        /// <summary>
        /// Froms the kilometers per hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromKilometersPerHour(double value)
        {
            return new Speed(value, SpeedUnit.KilometersPerHour);
        }

        /// <summary>
        /// Froms the kilometers per second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromKilometersPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.KilometersPerSecond);
        }

        /// <summary>
        /// Froms the feet per second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromFeetPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.FeetPerSecond);
        }

        /// <summary>
        /// Froms the meters per second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Speed FromMetersPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.MetersPerSecond);
        }

        /// <summary>
        /// Parses the speed unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static SpeedUnit ParseSpeedUnit(string value)
        {
#if !PocketPC || Framework20
            return (SpeedUnit)Enum.Parse(typeof(SpeedUnit), value, true);
#else
			return (SpeedUnit)Enum.ToObject(typeof(SpeedUnit), value);
#endif
        }

        /// <summary>
        /// Returns a random distance between 0 and 200 kilometers per hour.
        /// </summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Speed Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random distance between 0 and 200 kilometers per hour.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to generate random values.</param>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Speed Random(Random generator)
        {
            return new Speed(generator.NextDouble() * 200, SpeedUnit.KilometersPerHour).ToLocalUnitType();
        }

        #endregion Static Members

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Speed operator +(Speed left, Speed right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Speed operator -(Speed left, Speed right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Speed operator *(Speed left, Speed right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Speed operator /(Speed left, Speed right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Speed left, Speed right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Speed left, Speed right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Speed left, Speed right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Speed left, Speed right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Speed left, Speed right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Speed left, Speed right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Add(Speed value)
        {
            return new Speed(Value + value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Add(double value)
        {
            return new Speed(_value + value, Units);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Subtract(Speed value)
        {
            return new Speed(Value - value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Subtract(double value)
        {
            return new Speed(_value - value, Units);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Multiply(Speed value)
        {
            return new Speed(_value * value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Multiply(double value)
        {
            return new Speed(_value * value, Units);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Divide(Speed value)
        {
            return new Speed(_value / value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Speed Divide(double value)
        {
            return new Speed(Value / value, Units);
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns></returns>
        public Speed Increment()
        {
            return new Speed(Value + 1.0, Units);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns></returns>
        public Speed Decrement()
        {
            return new Speed(Value - 1.0, Units);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified speed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThan(Speed value)
        {
            return CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates if the current instance is smaller or equivalent to than the specified
        /// speed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThanOrEqualTo(Speed value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        /// <summary>
        /// Indicates if the current instance is larger than the specified speed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThan(Speed value)
        {
            return CompareTo(value) > 0;
        }

        /// <summary>
        /// Indicates if the current instance is larger or equivalent to than the specified
        /// speed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThanOrEqualTo(Speed value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="DotSpatial.Positioning.Speed"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Speed(string value)
        {
            return new Speed(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Speed"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(Speed value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IEquatable<Speed> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Speed other)
        {
            return _value == other.ToUnitType(_units).Value;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns></returns>
        public bool Equals(Speed other, int decimals)
        {
            return Math.Round(_value, decimals) == Math.Round(other.ToUnitType(_units).Value, decimals);
        }

        #endregion IEquatable<Speed> Members

        #region IComparable<Speed> Members

        /// <summary>
        /// Compares the current instance to the specified speed.
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
        public int CompareTo(Speed other)
        {
            return _value.CompareTo(other.ToUnitType(_units).Value);
        }

        #endregion IComparable<Speed> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified format
        /// and culture information.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            string subFormat;

            try
            {
                // Convert the format to uppercase
                format = format.ToUpper(CultureInfo.InvariantCulture);

                // Use the default if "g" is passed
                if (String.Compare(format, "G", true, culture) == 0)
                    format = "#" + culture.NumberFormat.NumberGroupSeparator + "##0.00 U";

                // Replace "V" with zeroes
                format = format.Replace("V", "0");

                // Replace the "d" with "h" since degrees is the same as hours
                if (format.Replace("U", string.Empty).Length != 0)
                    format = Value.ToString(format, culture);

                // Is there a units specifier°
                int startChar = format.IndexOf("U");
                if (startChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    int endChar = format.LastIndexOf("U");
                    // Extract the sub-string
                    subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Show the unit based on the length
                    switch (subFormat.Length)
                    {
                        case 1:
                            switch (Units)
                            {
                                case SpeedUnit.FeetPerSecond:
                                    format = format.Replace("U", "ft/s");
                                    break;
                                case SpeedUnit.KilometersPerHour:
                                    format = format.Replace("U", "km/h");
                                    break;
                                case SpeedUnit.KilometersPerSecond:
                                    format = format.Replace("U", "km/s");
                                    break;
                                case SpeedUnit.Knots:
                                    format = format.Replace("U", "kts");
                                    break;
                                case SpeedUnit.MetersPerSecond:
                                    format = format.Replace("U", "m/s");
                                    break;
                                case SpeedUnit.StatuteMilesPerHour:
                                    format = format.Replace("U", "MPH");
                                    break;
                            }
                            break;
                        case 2:
                            switch (Units)
                            {
                                case SpeedUnit.FeetPerSecond:
                                    format = format.Replace("UU", "ft/sec");
                                    break;
                                case SpeedUnit.KilometersPerHour:
                                    format = format.Replace("UU", "km/hour");
                                    break;
                                case SpeedUnit.KilometersPerSecond:
                                    format = format.Replace("UU", "km/sec");
                                    break;
                                case SpeedUnit.Knots:
                                    format = format.Replace("UU", "kts");
                                    break;
                                case SpeedUnit.MetersPerSecond:
                                    format = format.Replace("UU", "m/sec");
                                    break;
                                case SpeedUnit.StatuteMilesPerHour:
                                    format = format.Replace("UU", "mi/hour");
                                    break;
                            }
                            break;
                        case 3:
                            switch (Units)
                            {
                                case SpeedUnit.FeetPerSecond:
                                    format = format.Replace("UUU", "feet/second");
                                    break;
                                case SpeedUnit.KilometersPerHour:
                                    format = format.Replace("UUU", "kilometers/hour");
                                    break;
                                case SpeedUnit.KilometersPerSecond:
                                    format = format.Replace("UUU", "kilometers/second");
                                    break;
                                case SpeedUnit.Knots:
                                    format = format.Replace("UUU", "knots");
                                    break;
                                case SpeedUnit.MetersPerSecond:
                                    format = format.Replace("UUU", "meters/second");
                                    break;
                                case SpeedUnit.StatuteMilesPerHour:
                                    format = format.Replace("UUU", "miles/hour");
                                    break;
                            }
                            break;
                        case 4:
                            switch (Units)
                            {
                                case SpeedUnit.FeetPerSecond:
                                    format = format.Replace("UUUU", "feet per second");
                                    break;
                                case SpeedUnit.KilometersPerHour:
                                    format = format.Replace("UUUU", "kilometers per hour");
                                    break;
                                case SpeedUnit.KilometersPerSecond:
                                    format = format.Replace("UUUU", "kilometers per second");
                                    break;
                                case SpeedUnit.Knots:
                                    format = format.Replace("UUUU", "knots");
                                    break;
                                case SpeedUnit.MetersPerSecond:
                                    format = format.Replace("UUUU", "meters per second");
                                    break;
                                case SpeedUnit.StatuteMilesPerHour:
                                    format = format.Replace("UUUU", "miles per hour");
                                    break;
                            }
                            break;
                    }
                }
                // Return the final value
                return format;
            }
            catch (Exception ex)
            {
#if PocketPC
                throw new ArgumentException(Properties.Resources.Speed_InvalidFormat, ex);
#else
                throw new ArgumentException(Resources.Speed_InvalidFormat, "format", ex);
#endif
            }
            // catch
            //{
            //    throw new ArgumentException(Properties.Resources.Speed_InvalidFormat), "value");
            //}
        }

        #endregion IFormattable Members

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
            writer.WriteElementString("Units", _units.ToString());
            writer.WriteElementString("Value", _value.ToString("G17", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // Move to the <Units> element
            if (!reader.IsStartElement("Units"))
                reader.ReadToDescendant("Units");

            _units = (SpeedUnit)Enum.Parse(typeof(SpeedUnit), reader.ReadElementContentAsString(), false);
            _value = reader.ReadElementContentAsDouble();

            reader.Read();
        }

        #endregion IXmlSerializable Members
    }

    /// <summary>
    /// Indicates the unit of measure for speed measurements.
    /// </summary>
    /// <seealso cref="Speed.Units">Units Property (Speed Class)</seealso>
    ///
    /// <seealso cref="Speed">Speed Class</seealso>
    /// <remarks>This enumeration is used by the
    /// <see cref="Speed.Units">Units</see> property of the
    /// <see cref="Speed">Speed</see>
    /// class in conjunction with the <see cref="Speed.Value">Value</see>
    /// property to describe a speed measurement.</remarks>
    public enum SpeedUnit
    {
        /// <summary>The number of nautical miles travelled in one hour.</summary>
        Knots,
        /// <summary>The number of statute miles travelled in one hour, also known as MPH.</summary>
        StatuteMilesPerHour,
        /// <summary>The number of kilometers travelled in one hour, also known as KPH.</summary>
        KilometersPerHour,
        /// <summary>The number of kilometers travelled in one second, also known as
        /// KM/S.</summary>
        KilometersPerSecond,
        /// <summary>The number of feet travelled in one second, also known as FT/S.</summary>
        FeetPerSecond,
        /// <summary>The number of meters travelled in one hour, also known as M/S.</summary>
        MetersPerSecond
    }
}