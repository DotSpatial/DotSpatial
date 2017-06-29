using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
    /// <summary>Represents a measurement of an object's rate of travel.</summary>
    /// <remarks>
    /// 	<para>This structure is used to measure the rate at which something moves in a
    ///     given period of time. This structure supports several different unit types in both
    ///     Imperial and Metric measurement systems. A speed is measured in two parts: a
    ///     numeric value and a label indicating the units of measurement.</para>
    /// 	<para>Speed measurements can be converted to their equivalent values in other unit
    ///     types through the use of several conversion methods such as ToMetersPerSecond,
    ///     <strong>ToFeetPerSecond</strong>, <strong>ToKilometersPerHour</strong>, and others.
    ///     Three methods, <strong>ToImperialUnitType</strong>,
    ///     <strong>ToMetricUnitType</strong> and <strong>ToLocalUnitType</strong> also exist
    ///     for converting a speed measurement to the most readable unit type (i.e. 1 meter vs.
    ///     0.0001 kilometers) in any local culture.</para>
    /// 	<para>This structure is a <em>DotSpatial.Positioning</em> "parseable type" whose value can
    ///     be freely converted to and from <strong>String</strong> objects via the
    ///     <strong>ToString</strong> and <strong>Parse</strong> methods.</para>
    /// 	<para>Instances of this structure are guaranteed to be thread-safe because it is
    ///     immutable (its properties can only be modified via constructors).</para>
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.SpeedConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Speed : IFormattable, IComparable<Speed>, IEquatable<Speed>, IXmlSerializable
    {
        private double _Value;
        private SpeedUnit _Units;

        #region Constants

        private const double StatuteMPHPerKnot = 0.8689762;
        private const double KPHPerKnot = 0.5399568;
        private const double FPSPerKnot = 0.5924838;
        private const double MPSPerKnot = 1.943845;
        private const double KPSPerKnot = 1943.845;

        private const double KnotsPerStatuteMPH = 1.150779;
        private const double KPHPerStatuteMPH = 0.6213712;
        private const double FPSPerStatuteMPH = 0.6818182;
        private const double MPSPerStatuteMPH = 2.236936;
        private const double KPSPerStatuteMPH = 2236.936;

        private const double KnotsPerKPH = 1.852;
        private const double StatuteMPHPerKPH = 1.609344;
        private const double FPSPerKPH = 1.09728;
        private const double MPSPerKPH = 3.6;
        private const double KPSPerKPH = 3600;

        private const double KnotsPerKPS = 0.0005144444;
        private const double StatuteMPHPerKPS = 0.000447;
        private const double FPSPerKPS = 0.0003048;
        private const double MPSPerKPS = 0.001;
        private const double KPHPerKPS = 0.0002777778;

        private const double KnotsPerFPS = 1.68781;
        private const double StatuteMPHPerFPS = 1.466667;
        private const double KPHPerFPS = 0.9113444;
        private const double MPSPerFPS = 3.28084;
        private const double KPSPerFPS = 3280.84;

        private const double KnotsPerMPS = 0.5144444;
        private const double StatuteMPHPerMPS = 0.447;
        private const double FPSPerMPS = 0.3048;
        private const double KPHPerMPS = 0.2777778;
        private const double KPSPerMPS = 1000;

        //private const int DefaultPrecisionDigits = 10;

        #endregion

        #region Fields

        /// <remarks>
        /// The speed of light is used to determine distances such as the meter, which is
        /// defined as the distance travelled by light for 1/299792458th of a second.
        /// </remarks>
        /// <summary>Represents a speed of zero.</summary>
        public static readonly Speed Empty = new Speed(0, SpeedUnit.MetersPerSecond);
        /// <summary>Represents a speed of zero.</summary>
        public static readonly Speed AtRest = new Speed(0, SpeedUnit.MetersPerSecond);
        /// <summary>Returns the rate of travel of light in a vacuum.</summary>
        public static readonly Speed SpeedOfLight = new Speed(299792458, SpeedUnit.MetersPerSecond);
        /// <summary>Represents the largest possible speed.</summary>
        public static readonly Speed Maximum = new Speed(Double.MaxValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();
        /// <summary>Represents the smallest possible speed.</summary>
        public static readonly Speed Minimum = new Speed(Double.MinValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();

        /// <summary>Returns the rate of travel of sound waves at sea level.</summary>
        public static readonly Speed SpeedOfSoundAtSeaLevel = new Speed(340.29, SpeedUnit.MetersPerSecond);
        /// <summary>Represents an infinite speed.</summary>
        public static readonly Speed Infinity = new Speed(double.PositiveInfinity, SpeedUnit.MetersPerSecond);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Speed Invalid = new Speed(double.NaN, SpeedUnit.KilometersPerSecond);

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance using the specified value and unit type.</summary>
        /// <remarks>This is the most frequently used constructor of the speed class.</remarks>
        /// <param name="value">A <strong>Double</strong> containing a speed measurement.</param>
        /// <param name="units">A <strong>SpeedUnit</strong> value describing the value's type.</param>
        public Speed(double value, SpeedUnit units)
        {
            _Value = value;
            _Units = units;
        }

        /// <param name="value">
        ///	 <para>A <strong>String</strong> in any of the following formats (or a variation
        ///	 depending on the local culture):</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>vv uu</td>
        ///					 <td>vv.v uu</td>
        ///					 <td>vvuu</td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para>Where <strong>vv.v</strong> indicates a numeric value, and uu is any of the
        ///	 following case-insensitive units:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>FT/S</td>
        ///					 <td>FT/SEC</td>
        ///					 <td>FEET PER SECOND</td>
        ///					 <td>FEET PER SEC</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>FEET/SEC</td>
        ///					 <td>km/h</td>
        ///					 <td>KM/H</td>
        ///					 <td>KM/HR</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>KILOMETERS/HOUR</td>
        ///					 <td>KM/S</td>
        ///					 <td>KM/SEC</td>
        ///					 <td>KM/SECOND</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>KNOT</td>
        ///					 <td>KNOTS</td>
        ///					 <td>KTS</td>
        ///					 <td>M/S</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>M/SEC</td>
        ///					 <td>M/SECOND</td>
        ///					 <td>MPH</td>
        ///					 <td>MI/HR</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>MILES/HOUR</td>
        ///					 <td></td>
        ///					 <td></td>
        ///					 <td></td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para><em><font size="2">To request additional unit types, please send feedback by
        ///	 clicking the link at the bottom of this page.</font></em></para>
        /// </param>
        /// <remarks>
        /// This powerful method is designed to simplify the process of parsing values read
        /// from a data store or typed in by the user.
        /// </remarks>
        public Speed(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        public Speed(string value, CultureInfo culture)
        {
            string NumericPortion = null;
            int Count = 0;
            string Unit = null;

            // Anything to do°
            if (value == null || value.Length == 0)
            {
                _Value = 0;
                _Units = SpeedUnit.MetersPerSecond;
            }
            try
            {

                // Convert to uppercase and remove commas
                value = value.ToUpper(CultureInfo.InvariantCulture).Replace(culture.NumberFormat.NumberGroupSeparator, "");
                if (value == "INFINITY")
                {
                    _Value = Speed.Infinity.Value;
                    _Units = Speed.Infinity.Units;
                }
                if (value == "EMPTY")
                {
                    _Value = 0;
                    _Units = SpeedUnit.MetersPerSecond;
                }
                // Try to extract the unit
                Count = value.Length - 1;
                while (Count >= 0)
                {
                    if (Char.IsNumber(value, Count))
                    {
                        Count++;
                        break;
                    }
                    else
                    {
                        Count--;
                    }
                }
                Unit = value.Substring(Count).Trim();
                // Get the numeric portion
                NumericPortion = value.Substring(0, Count);
                try
                {
                    _Value = double.Parse(NumericPortion, culture);
                }
                catch (Exception ex)
                {
#if PocketPC
					throw new ArgumentException(Properties.Resources.Speed_InvalidNumericPortion, ex);
#else
                    throw new ArgumentException(Properties.Resources.Speed_InvalidNumericPortion, "value", ex);
#endif
                }
                //catch
                //{
                //    throw new ArgumentException(Properties.Resources.Speed_InvalidNumericPortion), "value");
                //}
                // Replace synonyms
                string TempUnits = Unit.ToUpper(CultureInfo.InvariantCulture).Trim()
                    // Replace "per" synonyms
                    .Replace(" PER ", "/")
                    .Replace("-PER-", "/")
                    .Replace(" / ", "/")
                    .Replace(".", "")
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
                switch (TempUnits)
                {
                    case "FT/S":
                        _Units = SpeedUnit.FeetPerSecond;
                        break;
                    case "KPH":
                        _Units = SpeedUnit.KilometersPerHour;
                        break;
                    case "KM/S":
                        _Units = SpeedUnit.KilometersPerSecond;
                        break;
                    case "K":
                        _Units = SpeedUnit.Knots;
                        break;
                    case "M/S":
                        _Units = SpeedUnit.MetersPerSecond;
                        break;
                    case "MI/H":
                        _Units = SpeedUnit.StatuteMilesPerHour;
                        break;
                    default:
                        throw new FormatException(Properties.Resources.Speed_InvalidUnitPortion);
                }
            }
            catch (Exception ex)
            {
#if PocketPC
				throw new ArgumentException(Properties.Resources.Speed_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Speed_InvalidFormat, "value", ex);
#endif
            }
            //catch
            //{
            //    throw new ArgumentException(Properties.Resources.Speed_InvalidFormat), "value");
            //}
        }

        public Speed(XmlReader reader)
        {
            // Initialize all fields
            _Value = Double.NaN;
            _Units = 0;
            
            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties


        /// <summary>Returns the numeric portion of the speed measurement.</summary>
        /// <remarks>
        /// This property is combined with the
        /// <see cref="Units">Units</see> property to form a complete
        /// speed measurement. 
        /// </remarks>
        public double Value
        {
            get
            {
                return _Value;
            }
        }

        /// <remarks>
        /// 	<para>Following proper scientific practices, speed measurements are always made
        ///  using a value paired with a unit type. </para>
        /// 	<para><img src="BestPractice.jpg"/></para><para><strong>Always explicitly
        ///  convert to a specific speed unit type before performing
        ///  mathematics.</strong></para>
        /// 	<para>Since the Units property of the Speed class can be modified, it is not
        ///  safe to assume that a speed measurement will always be of a certain unit type.
        ///  Therefore, use a conversion method such as <see cref="Speed.ToKilometersPerHour">
        ///  ToKilometersPerHour</see> or <see cref="Speed.ToStatuteMilesPerHour">
        ///  ToStatuteMilesPerHour</see> to ensure that the speed is in the correct unit
        ///  type before perfoming mathematics.</para>
        /// </remarks>
        /// <value>
        /// A value from the <see cref="SpeedUnit">SpeedUnits</see> enumeration.
        /// </value>
        /// <summary>Returns the units portion of the speed measurement.</summary>
        public SpeedUnit Units
        {
            get
            {
                return _Units;
            }
        }

        /// <summary>Indicates if the measurement is zero.</summary>
        public bool IsEmpty
        {
            get
            {
                return _Value == 0;
            }
        }

        /// <summary>Indicates if the unit of measurement is a Metric unit type.</summary>
        public bool IsMetric
        {
            get
            {
                return _Units == SpeedUnit.KilometersPerHour
                    || _Units == SpeedUnit.KilometersPerSecond
                    || _Units == SpeedUnit.MetersPerSecond;
            }
        }

        /// <summary>Indicates if the measurement is infinite.</summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_Value);
            }
        }

        /// <summary>
        /// Indicates if the current instance is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_Value); }
        }

        #endregion

        #region Public Methods

        /// <summary>Returns a copy of the current instance.</summary>
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
            return new Speed(Math.Round(_Value, decimals), _Units);
        }

        /// <summary>Returns the current instance converted to feet per second.</summary>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToFeetPerSecond() //'Implements ISpeed.ToFeetPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * StatuteMPHPerFPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPHPerFPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPSPerFPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return this;
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPSPerFPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KnotsPerFPS, SpeedUnit.FeetPerSecond);
                default:
                    return Speed.Empty;
            }
        }

        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        /// <summary>Converts the current measurement into kilometers per hour.</summary>
        public Speed ToKilometersPerHour() //'Implements ISpeed.ToKilometersPerHour
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * StatuteMPHPerKPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerHour:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPSPerKPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPSPerKPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KnotsPerKPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPSPerKPH, SpeedUnit.KilometersPerHour);
                default:
                    return Speed.Empty;
            }
        }

        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        /// <summary>Converts the current measurement into kilometers per second.</summary>
        public Speed ToKilometersPerSecond() //'Implements ISpeed.ToKilometersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * StatuteMPHPerKPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPHPerKPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPSPerKPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPSPerKPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KnotsPerKPS, SpeedUnit.KilometersPerSecond);
                default:
                    return Speed.Empty;
            }
        }

        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        /// <summary>Returns the current instance converted to knots.</summary>
        public Speed ToKnots() //'Implements ISpeed.ToKnots
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * StatuteMPHPerKnot, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPHPerKnot, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPSPerKnot, SpeedUnit.Knots);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPSPerKnot, SpeedUnit.Knots);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPSPerKnot, SpeedUnit.Knots);
                case SpeedUnit.Knots:
                    return this;
                default:
                    return Speed.Empty;
            }
        }

        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        /// <summary>Returns the current instance converted to meters per second.</summary>
        public Speed ToMetersPerSecond() //'Implements ISpeed.ToMetersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * StatuteMPHPerMPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPHPerMPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPSPerMPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPSPerMPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return this;
                case SpeedUnit.Knots:
                    return new Speed(Value * KnotsPerMPS, SpeedUnit.MetersPerSecond);
                default:
                    return Speed.Empty;
            }
        }

        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        /// <summary>Returns the current instance converted to miles per hours (MPH).</summary>
        public Speed ToStatuteMilesPerHour()
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return this;
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPHPerStatuteMPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPSPerStatuteMPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPSPerStatuteMPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPSPerStatuteMPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KnotsPerStatuteMPH, SpeedUnit.StatuteMilesPerHour);
                default:
                    return Speed.Empty;
            }
        }

        /// <summary>Returns the current instance converted to the specified unit type.</summary>
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
                    return Speed.Empty;
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
            Speed Temp = ToStatuteMilesPerHour();
            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToFeetPerSecond();
            return Temp;
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
            Speed Temp = ToKilometersPerHour();
            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToMetersPerSecond();
            // And so on until we find the right unit
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToKilometersPerSecond();
            return Temp;
        }

        /// <returns>A <strong>Speed</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a Speed becomes smaller, it may make more sense to the 
        /// user to be expressed in a smaller unit type.  For example, a Speed of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// find the smallest unit type and convert the unit to the user's local
        /// numeric system (Imperial or Metric).</remarks>
        /// <summary>
        /// Returns the current instance converted to the most readable Imperial or Metric
        /// unit type depending on the local culture.
        /// </summary>
        public Speed ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            else
                return ToImperialUnitType();
        }

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified
        /// format.
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the total distance traveled at the current speed for the specified
        /// time.
        /// </summary>
        /// <returns>A <strong>Distance</strong> representing the distance travelled at
        /// the current speed for the specified length of time.</returns>
        public Distance ToDistance(TimeSpan time)
        {
            return new Distance(ToMetersPerSecond().Value * time.TotalMilliseconds / 1000.0, DistanceUnit.Meters).ToLocalUnitType();
        }

        #endregion

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

        /// <summary>Returns a unique code for the current instance used in hash tables.</summary>
        public override int GetHashCode()
        {
            return ToMetersPerSecond().Value.GetHashCode();
        }

        /// <summary>Outputs the speed measurement as a formatted string.</summary>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion

        #region Static Members

        /// <summary>Creates a speed measurement based on a string value.</summary>
        /// <param name="value">
        ///	 <para>A <strong>String</strong> in any of the following formats (or variation
        ///	 depending on the local culture):</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>vu</td>
        /// 
        ///					 <td>vv.vu</td>
        /// 
        ///					 <td>v u</td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para>where <strong>vv.v</strong> is a decimal value and <strong>u</strong> is a
        ///	 unit type made from words in the following list:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>FEET</td>
        /// 
        ///					 <td>FOOT</td>
        /// 
        ///					 <td>METER</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>METERS</td>
        /// 
        ///					 <td>METRE</td>
        /// 
        ///					 <td>METRES</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>KILOMETER</td>
        /// 
        ///					 <td>KILOMETRE</td>
        /// 
        ///					 <td>KILOMETERS</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>KILOMETRES</td>
        /// 
        ///					 <td>KNOT</td>
        /// 
        ///					 <td>KNOTS</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>MILE</td>
        /// 
        ///					 <td>MILES</td>
        /// 
        ///					 <td>STATUTE MILE</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>STATUTE MILES</td>
        /// 
        ///					 <td>F</td>
        /// 
        ///					 <td>FT</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>M</td>
        /// 
        ///					 <td>KM</td>
        /// 
        ///					 <td>K</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>PER</td>
        /// 
        ///					 <td>-PER-</td>
        /// 
        ///					 <td>/</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>SECOND</td>
        /// 
        ///					 <td>SEC</td>
        /// 
        ///					 <td>S</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>HOUR</td>
        /// 
        ///					 <td>HR</td>
        /// 
        ///					 <td>H</td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para>For example, "12 miles per hour" is acceptable because the words "miles,"
        ///	 "per," and "hour" are found in the above list. Some combinations are not supported,
        ///	 such as "feet/hour." The word combination should look similar to a value from the
        ///	 
        /// <see cref="SpeedUnit">SpeedUnit</see> enumeration.</para>
        /// </param>
        /// <remarks>
        /// This powerful method simplifies the process of processing values read from a data
        /// store or entered via the user. This method even supports some natural language
        /// processing ability by understanding words (see list above). This method can parse any
        /// value created via the ToString method.
        /// </remarks>
        /// <returns>A new <strong>Speed</strong> object with the specified value and 
        /// units.</returns>
        public static Speed Parse(string value)
        {
            return new Speed(value);
        }

        public static Speed Parse(string value, CultureInfo culture)
        {
            return new Speed(value, culture);
        }

        public static Speed FromKnots(double value)
        {
            return new Speed(value, SpeedUnit.Knots);
        }

        public static Speed FromStatuteMilesPerHour(double value)
        {
            return new Speed(value, SpeedUnit.StatuteMilesPerHour);
        }

        public static Speed FromKilometersPerHour(double value)
        {
            return new Speed(value, SpeedUnit.KilometersPerHour);
        }

        public static Speed FromKilometersPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.KilometersPerSecond);
        }

        public static Speed FromFeetPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.FeetPerSecond);
        }

        public static Speed FromMetersPerSecond(double value)
        {
            return new Speed(value, SpeedUnit.MetersPerSecond);
        }

        public static SpeedUnit ParseSpeedUnit(string value)
        {
#if !PocketPC || Framework20
            return (SpeedUnit)Enum.Parse(typeof(SpeedUnit), value, true);
#else
			return (SpeedUnit)Enum.ToObject(typeof(SpeedUnit), value);
#endif
        }

        /// <summary>Returns a random distance between 0 and 200 kilometers per hour.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Speed Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>Returns a random distance between 0 and 200 kilometers per hour.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        /// <param name="generator">A <strong>Random</strong> object used to generate random values.</param>
        public static Speed Random(Random generator)
        {
            return new Speed(generator.NextDouble() * 200, SpeedUnit.KilometersPerHour).ToLocalUnitType();
        }

        #endregion

        #region Operators

        public static Speed operator +(Speed left, Speed right)
        {
            return left.Add(right);
        }

        public static Speed operator -(Speed left, Speed right)
        {
            return left.Subtract(right);
        }

        public static Speed operator *(Speed left, Speed right)
        {
            return left.Multiply(right);
        }

        public static Speed operator /(Speed left, Speed right)
        {
            return left.Divide(right);
        }

        public static bool operator <(Speed left, Speed right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Speed left, Speed right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        public static bool operator ==(Speed left, Speed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Speed left, Speed right)
        {
            return !(left == right);
        }

        public static bool operator >=(Speed left, Speed right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        public static bool operator >(Speed left, Speed right)
        {
            return left.CompareTo(right) > 0;
        }

        public Speed Add(Speed value)
        {
            return new Speed(Value + value.ToUnitType(Units).Value, Units);
        }

        public Speed Add(double value)
        {
            return new Speed(_Value + value, Units);
        }

        public Speed Subtract(Speed value)
        {
            return new Speed(Value - value.ToUnitType(Units).Value, Units);
        }

        public Speed Subtract(double value)
        {
            return new Speed(_Value - value, Units);
        }

        public Speed Multiply(Speed value)
        {
            return new Speed(_Value * value.ToUnitType(_Units).Value, _Units);
        }

        public Speed Multiply(double value)
        {
            return new Speed(_Value * value, Units);
        }

        public Speed Divide(Speed value)
        {
            return new Speed(_Value / value.ToUnitType(_Units).Value, _Units);
        }

        public Speed Divide(double value)
        {
            return new Speed(Value / value, Units);
        }

        /// <summary>Returns the current instance increased by one.</summary>
        public Speed Increment()
        {
            return new Speed(Value + 1.0, Units);
        }

        /// <summary>Returns the current instance decreased by one.</summary>
        public Speed Decrement()
        {
            return new Speed(Value - 1.0, Units);
        }

        /// <summary>Indicates if the current instance is smaller than the specified speed.</summary>
        public bool IsLessThan(Speed value)
        {
            return CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates if the current instance is smaller or equivalent to than the specified
        /// speed.
        /// </summary>
        public bool IsLessThanOrEqualTo(Speed value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        /// <summary>Indicates if the current instance is larger than the specified speed.</summary>
        public bool IsGreaterThan(Speed value)
        {
            return CompareTo(value) > 0;
        }

        /// <summary>
        /// Indicates if the current instance is larger or equivalent to than the specified
        /// speed.
        /// </summary>
        public bool IsGreaterThanOrEqualTo(Speed value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion

        #region Conversions

        public static explicit operator Speed(string value)
        {
            return new Speed(value, CultureInfo.CurrentCulture);
        }

        public static explicit operator string(Speed value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region IEquatable<Speed> Members

        public bool Equals(Speed other)
        {
            return _Value == other.ToUnitType(_Units).Value;
        }

        public bool Equals(Speed other, int decimals)
        {
            return Math.Round(_Value, decimals) == Math.Round(other.ToUnitType(_Units).Value, decimals);
        }

        #endregion

        #region IComparable<Speed> Members

        /// <summary>Compares the current instance to the specified speed.</summary>
        public int CompareTo(Speed other)
        {
            return _Value.CompareTo(other.ToUnitType(_Units).Value);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified format
        /// and culture information.
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            int StartChar = 0;
            int EndChar = 0;
            string SubFormat = null;

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
                if (format.Replace("U", "").Length != 0)
                    format = Value.ToString(format, culture);

                // Is there a units specifier°
                StartChar = format.IndexOf("U");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("U");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Show the unit based on the length
                    switch (SubFormat.Length)
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
                throw new ArgumentException(Properties.Resources.Speed_InvalidFormat, "value", ex);
#endif
            }
            //catch
            //{
            //    throw new ArgumentException(Properties.Resources.Speed_InvalidFormat), "value");
            //}		
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Units", _Units.ToString());
            writer.WriteElementString("Value", _Value.ToString("G17", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            // Move to the <Units> element
            if (!reader.IsStartElement("Units"))
                reader.ReadToDescendant("Units");

            _Units = (SpeedUnit)Enum.Parse(typeof(SpeedUnit), reader.ReadElementContentAsString(), false);
            _Value = reader.ReadElementContentAsDouble();
        }

        #endregion
    }

    /// <summary>Indicates the unit of measure for speed measurements.</summary>
    /// <remarks>
    /// This enumeration is used by the
    /// <see cref="Speed.Units">Units</see> property of the
    /// <see cref="Speed">Speed</see>
    /// class in conjunction with the <see cref="Speed.Value">Value</see>
    /// property to describe a speed measurement.
    /// </remarks>
    /// <seealso cref="Speed.Units">Units Property (Speed Class)</seealso>
    /// <seealso cref="Speed">Speed Class</seealso>
    public enum SpeedUnit : int
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