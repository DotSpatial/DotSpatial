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
    /// <summary>
    /// Represents the measurement of a straight line between between two points on
    /// Earth's surface.
    /// </summary>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.DistanceConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Distance : IFormattable, IComparable<Distance>, IEquatable<Distance>, IXmlSerializable
    {
        private double _Value;
        private DistanceUnit _Units;

        #region Constants

        private const double FeetPerMeter = 3.2808399;
        private const double FeetPerCentimeter = 0.032808399;
        private const double FeetPerStatuteMile = 5280;
        private const double FeetPerKilometer = 3280.8399;
        private const double FeetPerInch = 0.0833333333333333;
        private const double FeetPerNauticalMile = 6076.11549;
        private const double InchesPerMeter = 39.3700787;
        private const double InchesPerCentimeter = 0.393700787;
        private const double InchesPerStatuteMile = 63360;
        private const double InchesPerKilometer = 39370.0787;
        private const double InchesPerFoot = 12.0;
        private const double InchesPerNauticalMile = 72913.3858;
        private const double StatuteMilesPerMeter = 0.000621371192;
        private const double StatuteMilesPerCentimeter = 0.00000621371192;
        private const double StatuteMilesPerKilometer = 0.621371192;
        private const double StatuteMilesPerInch = 0.0000157828283;
        private const double StatuteMilesPerFoot = 0.000189393939;
        private const double StatuteMilesPerNauticalMile = 1.15077945;
        private const double NauticalMilesPerMeter = 0.000539956803;
        private const double NauticalMilesPerCentimeter = 0.00000539956803;
        private const double NauticalMilesPerKilometer = 0.539956803;
        private const double NauticalMilesPerInch = 0.0000137149028;
        private const double NauticalMilesPerFoot = 0.000164578834;
        private const double NauticalMilesPerStatuteMile = 0.868976242;
        private const double CentimetersPerStatuteMile = 160934.4;
        private const double CentimetersPerKilometer = 100000;
        private const double CentimetersPerFoot = 30.48;
        private const double CentimetersPerInch = 2.54;
        private const double CentimetersPerMeter = 100;
        private const double CentimetersPerNauticalMile = 185200;
        private const double MetersPerStatuteMile = 1609.344;
        private const double MetersPerCentimeter = 0.01;
        private const double MetersPerKilometer = 1000;
        private const double MetersPerFoot = 0.3048;
        private const double MetersPerInch = 0.0254;
        private const double MetersPerNauticalMile = 1852;
        private const double KilometersPerMeter = 0.001;
        private const double KilometersPerCentimeter = 0.00001;
        private const double KilometersPerStatuteMile = 1.609344;
        private const double KilometersPerFoot = 0.0003048;
        private const double KilometersPerInch = 0.0000254;
        private const double KilometersPerNauticalMile = 1.852;

        #endregion

        #region Fields

        /// <summary>
        /// Returns the distance from the center of the Earth to the equator according to the
        /// WGS1984 ellipsoid.
        /// </summary>
        /// <seealso cref="EarthsPolarRadiusWgs1984">EarthsPolarRadiusWgs1984 
        /// Field</seealso>
        /// <seealso cref="EarthsAverageRadius">EarthsAverageRadius Field</seealso>
        public static readonly Distance EarthsEquatorialRadiusWgs1984 = new Distance(6378137.0, DistanceUnit.Meters);
        /// <summary>
        /// Represents an infinite distance.
        /// </summary>
        /// <remarks>This field is typically used to indicate the absence of a distance limit.  For example,
        /// the Layer class of GIS.NET uses Infinity to indicate that the layer is drawn no matter how far
        /// out the user zooms away from it.  </remarks>
        public static readonly Distance Infinity = new Distance(double.PositiveInfinity, DistanceUnit.Meters);
        /// <summary>
        /// Returns the distance from the center of the Earth to a pole according to the
        /// WGS1984 ellipsoid.
        /// </summary>
        /// <seealso cref="EarthsEquatorialRadiusWgs1984">EarthsEquatorialRadiusWgs1984 Field</seealso>
        /// <seealso cref="EarthsAverageRadius">EarthsAverageRadius Field</seealso>
        public static readonly Distance EarthsPolarRadiusWgs1984 = new Distance(6356752.314245, DistanceUnit.Meters);
        /// <summary>Returns the average radius of the Earth.</summary>
        /// <seealso cref="EarthsEquatorialRadiusWgs1984">EarthsEquatorialRadiusWgs1984 Field</seealso>
        /// <seealso cref="EarthsPolarRadiusWgs1984">EarthsPolarRadiusWgs1984 Field</seealso>
        public static readonly Distance EarthsAverageRadius = new Distance(6378100, DistanceUnit.Meters);
        public static readonly Distance Empty = new Distance(0, DistanceUnit.Meters).ToLocalUnitType();
        public static readonly Distance SeaLevel = new Distance(0, DistanceUnit.Meters).ToLocalUnitType();
        public static readonly Distance Maximum = new Distance(double.MaxValue, DistanceUnit.Kilometers).ToLocalUnitType();
        public static readonly Distance Minimum = new Distance(double.MinValue, DistanceUnit.Kilometers).ToLocalUnitType();
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Distance Invalid = new Distance(double.NaN, DistanceUnit.Kilometers);

        #endregion

        #region  Constructors
        
        /// <summary>Creates a new instance using the specified value and unit type.</summary>
        /// <example>
        /// This example uses a constructor to create a new distance of 50km.
        /// <code lang="VB">
        /// Dim MyDistance As New Distance(50, DistanceUnit.Kilometers)
        /// </code>
        /// <code lang="C#">
        /// Distance MyDistance = new Distance(50, DistanceUnit.Kilometers);
        /// </code>
        /// </example>
        public Distance(double value, DistanceUnit units)
        {
            _Value = value;
            _Units = units;
        }

        /// <summary>Creates a new instance from the the specified string.</summary>
        /// <param name="value">
        ///	 <para>A <strong>String</strong> in any format accepted by the
        ///	 <see cref="Parse(string)">Parse</see>
        ///	 method.</para>
        /// </param>
        /// <remarks>
        /// This powerful constructor is typically used to initialize an instance with a
        /// string-based distance measurement, such as one entered by a user or read from a file.
        /// This constructor can accept any output created via the
        /// <see cref="ToString()">ToString</see> method.
        /// </remarks>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid distance measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the distance measurement was not recognized.<br/>
        /// 2. The distance unit type was not recognized or not specified.</exception>
        /// <example>
        /// This example demonstrates how the to use this constructor.
        /// <code lang="VB">
        /// Dim MyDistance As Distance
        /// ' Create a distance of 50 kilometers
        /// MyDistance = New Distance("50 km")
        /// ' Create a distance of 14,387 miles, then convert it into inches
        /// MyDistance = New Distance("14,387 statute miles").ToInches
        /// ' Parse an untrimmed measurement into 50 feet
        /// MyDistance = New Distance("	50 '	   ")
        /// </code>
        /// <code lang="C#">
        /// Distance MyDistance;
        /// // Create a distance of 50 kilometers
        /// MyDistance = new Distance("50 km");
        /// // Create a distance of 14,387 miles, then convert it into inches
        /// MyDistance = new Distance("14,387 statute miles").ToInches;
        /// // Parse an untrimmed measurement into 50 feet
        /// MyDistance = new Distance("	50 '	   ");
        /// </code>
        /// </example>
        public Distance(string value)
            : this(value, CultureInfo.CurrentCulture)
        {
        }

        public Distance(string value, CultureInfo culture)
        {
            // Anything to do?
            if (value == null || value.Length == 0)
            {
                // Return a blank distance in Imperial or English system
                _Value = 0;
                _Units = DistanceUnit.Meters;
                return;
            }

            // Default to the current culture if none is given
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            //string NumericPortion = null;
            //int Count = 0;
            //string Unit = null;

            try
            {
                // Trim surrounding spaces and switch to uppercase
                value = value.Trim().ToUpper(CultureInfo.InvariantCulture).Replace(culture.NumberFormat.NumberGroupSeparator, "");
                // Is it infinity?
                if (value == "INFINITY")
                {
                    _Value = Infinity.Value;
                    _Units = DistanceUnit.Meters;
                    return;
                }
                if (value == "SEALEVEL" || value == "SEA LEVEL")
                {
                    _Value = SeaLevel.Value;
                    _Units = DistanceUnit.Meters;
                    return;
                }
                if (value == "EMPTY")
                {
                    _Value = 0;
                    _Units = DistanceUnit.Meters;
                    return;
                }
                // Go until the first non-number character
                int Count = 0;
                while (Count < value.Length)
                {
                    string subValue = value.Substring(Count, 1);
                    if ((subValue == "0") || (subValue == "1") || (subValue == "2") || (subValue == "3")
                    || (subValue == "4") || (subValue == "5") || (subValue == "6") || (subValue == "7")
                    || (subValue == "8") || (subValue == "9")
                    || (subValue == culture.NumberFormat.NumberGroupSeparator)
                    || (subValue == culture.NumberFormat.NumberDecimalSeparator))
                        // Allow continuation
                        Count++;
                    else
                        // Non-numeric character!
                        break;
                }
                string Unit = value.Substring(Count).Trim();
                // Get the numeric portion
                string NumericPortion;
                if (Count > 0)
                    NumericPortion = value.Substring(0, Count);
                else
                    NumericPortion = "0";
                try
                {
                    _Value = double.Parse(NumericPortion, culture);
                }
                catch (Exception ex)
                {
#if PocketPC
                    throw new ArgumentException(Properties.Resources.Distance_InvalidNumericPortion, ex);
#else
                    throw new ArgumentException(Properties.Resources.Distance_InvalidNumericPortion, "value", ex);
#endif
                }
                //catch
                //{
                //    throw new ArgumentException(Properties.Resources.Distance_InvalidNumericPortion), "value");
                //}
                // Try to interpret the measurement
                if ((Unit == "M") || (Unit == "M.") || (Unit == "METERS") || (Unit == "METRES")
                    || (Unit == "METRE") || (Unit == "METER"))
                    _Units = DistanceUnit.Meters;
                else if ((Unit == "CM") || (Unit == "CM.") || (Unit == "CENTIMETER")
                    || (Unit == "CENTIMETERS") || (Unit == "CENTIMETRE") || (Unit == "CENTIMETRES"))
                    _Units = DistanceUnit.Centimeters;
                else if ((Unit == "KM") || (Unit == "KM.") || (Unit == "KILOMETRES")
                    || (Unit == "KILOMETERS") || (Unit == "KILOMETRE") || (Unit == "KILOMETER"))
                    _Units = DistanceUnit.Kilometers;
                else if ((Unit == "MI") || (Unit == "MI.") || (Unit == "MILE")
                    || (Unit == "MILES") || (Unit == "STATUTE MILES"))
                    _Units = DistanceUnit.StatuteMiles;
                else if ((Unit == "NM") || (Unit == "NM.") || (Unit == "NAUTICAL MILE")
                    || (Unit == "NAUTICAL MILES"))
                    _Units = DistanceUnit.NauticalMiles;
                else if ((Unit == "IN") || (Unit == "IN.") || (Unit == "\"") || (Unit == "INCHES")
                    || (Unit == "INCH"))
                    _Units = DistanceUnit.Inches;
                else if ((Unit == "FT") || (Unit == "FT.") || (Unit == "'") || (Unit == "FOOT")
                    || (Unit == "FEET"))
                    _Units = DistanceUnit.Feet;
                else
                {
                    if (_Value == 0)
                    {
                        if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                        {
                            _Units = DistanceUnit.Meters;
                        }
                        else
                        {
                            _Units = DistanceUnit.Feet;
                        }
                    }
                    else
                    {
                        throw new ArgumentException(Properties.Resources.Distance_InvalidUnitPortion, "value");
                    }
                }
                // Return the new distance class
                //return new Distance(_Value, _Units);
            }
            catch (Exception ex)
            {
#if PocketPC
                throw new ArgumentException(Properties.Resources.Distance_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Distance_InvalidFormat, "value", ex);
#endif
            }
            //catch
            //{
            //    throw new ArgumentException(Properties.Resources.Distance_InvalidFormat), "value");
            //}
        }

        /// <summary>
        /// Creates a new instance from the specified XML.
        /// </summary>
        /// <param name="reader"></param>
        public Distance(XmlReader reader)
        {
            // Initialize all fields
            _Value = Double.NaN;
            _Units = 0;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the numeric portion of a distance measurement.</summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>
        /// This property is paired with the <see cref="Units">Units</see> property to form a complete distance
        /// measurement.  
        /// </remarks>
        /// <example>
        /// This example demonstrates how to use the Value property to modify a distance object.  The object 
        /// is then converted to kilometers.
        /// <code lang="VB">
        /// ' Declare a distance of 0 mi.
        /// Dim MyDistance As New Distance(0, DistanceUnit.StatuteMiles)
        /// ' Change the distance to 100 mi.
        /// MyDistance.Value = 100
        /// ' Change the distance to 12.3456 mi.
        /// MyDistance.Value = 12.3456
        /// ' Convert the measurement into kilometers
        /// MyDistance = MyDistance.ToKilometers
        /// </code>
        /// <code lang="C#">
        /// // Declare a distance of 0 mi.
        /// Distance MyDistance = new Distance(0, DistanceUnit.StatuteMiles);
        /// // Change the distance to 100 mi.
        /// MyDistance.Value = 100;
        /// // Change the distance to 12.3456 mi.
        /// MyDistance.Value = 12.3456;
        /// // Convert the measurement into kilometers
        /// MyDistance = MyDistance.ToKilometers;
        /// </code>
        /// </example>
        /// <seealso cref="Units">Units Property</seealso>
        public double Value
        {
            get
            {
                return _Value;
            }
        }

        /// <summary>Describes the unit portion of a distance measurement.</summary>
        /// <value>
        /// A value from the <see cref="DistanceUnit">DistanceUnit</see> enumeration. Default 
        /// is <strong>DistanceUnit.Meters</strong>.
        /// </value>
        /// <remarks>
        /// <para>Each distance measurement is comprised of a numeric <see cref="Value">value</see>
        /// and a unit type.  This property describes the numeric value so that it may be
        /// explicitly identified. An instance of the <strong>Distance</strong> class may have a value 
        /// of zero, but it is impossible to have an unspecified unit type.</para>
        /// 
        /// <para><img src="BestPractice.jpg"/></para><para><strong>Use conversion methods instead of setting the
        /// Units property</strong></para>
        /// 
        /// <para>When the Units property is changed, no conversion is performed on the
        /// Value property. This could lead to mathematical errors which are difficult to debug. Use
        /// conversion methods such as ToFeet or ToMeters instead.</para>
        /// 
        /// <para>
        /// This example demonstrates poor programming when trying to add 100 feet to 100 meters
        /// by changing the Units property of the Distance2 object.
        /// <code lang="VB">
        /// ' Declare two distances
        /// Dim Distance1 As New Distance(50, DistanceUnit.Meters)
        /// Dim Distance2 As New Distance(100, DistanceUnit.Feet)
        /// ' Store their sum in another variable
        /// Dim Distance3 As New Distance(0, DistanceUnit.Meters)
        /// ' INCORRECT: Changing Units property does not convert Distance2!
        /// Distance2.Units = DistanceUnit.Meters
        /// Distance3.Value = Distance1.Value + Distance2.Value
        /// </code>
        /// <code lang="C#">
        /// // Declare two distances
        /// Distance Distance1 = new Distance(50, DistanceUnit.Meters);
        /// Distance Distance2 = new Distance(100, DistanceUnit.Feet);
        /// // Store their sum in another variable
        /// Distance Distance3 = new Distance(0, DistanceUnit.Meters);
        /// // INCORRECT: Changing Units property does not convert Distance2!
        /// Distance2.Units = DistanceUnit.Meters;
        /// Distance3.Value = Distance1.Value + Distance2.Value;
        /// </code>
        /// The correct technique is to use a conversion method to change the unit type instead
        /// of modifying the Units property.
        /// <code lang="VB">
        /// ' Declare two distances
        /// Dim Distance1 As New Distance(50, DistanceUnit.Meters)
        /// Dim Distance2 As New Distance(100, DistanceUnit.Feet)
        /// ' Store their sum in another variable
        /// Dim Distance3 As New Distance(0, DistanceUnit.Meters)
        /// ' CORRECT: The ToMeters method is used to standardize unit types
        /// Distance3.Value = Distance1.ToMeters().Value + Distance2.ToMeters().Value
        /// </code>
        /// <code lang="C#">
        /// // Declare two distances
        /// Distance Distance1 = new Distance(50, DistanceUnit.Meters);
        /// Distance Distance2 = new Distance(100, DistanceUnit.Feet);
        /// // Store their sum in another variable
        /// Distance Distance3 = new Distance(0, DistanceUnit.Meters);
        /// // CORRECT: The ToMeters method is used to standardize unit types
        /// Distance3.Value = Distance1.ToMeters().Value + Distance2.ToMeters().Value;
        /// </code>
        /// </para>
        /// </remarks>
        /// <seealso cref="Value">Value Property</seealso>
        public DistanceUnit Units
        {
            get
            {
                return _Units;
            }
        }

        /// <summary>
        /// Returns whether the value is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                return double.IsNaN(_Value);
            }
        }

        /// <summary>
        /// Returns whether the value is zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Value == 0;
            }
        }

        /// <summary>
        /// Returns whether the unit of measurement is metric.
        /// </summary>
        public bool IsMetric
        {
            get
            {
                return _Units == DistanceUnit.Centimeters
                    || _Units == DistanceUnit.Meters
                    || _Units == DistanceUnit.Kilometers;
            }
        }

        /// <summary>
        /// Returns whether the value is infinite.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_Value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the time required to travel the entire distance at the specified speed.
        /// </summary>
        /// <param name="speed">A <strong>Speed</strong> object representing a travel speed.</param>
        /// <returns>A <strong>TimeSpan</strong> object representing the total time required to travel the entire distance.</returns>
        public TimeSpan GetMinimumTravelTime(Speed speed)
        {
            //Dim AdjustedDestination As Position = destination.ToDatum(Datum)
            double TravelDistance = ToMeters().Value;
            double TravelSpeed = speed.ToMetersPerSecond().Value;
            // Perform the calculation
            return new TimeSpan((long)(TravelDistance / TravelSpeed * TimeSpan.TicksPerSecond));
        }

        /// <summary>
        /// Returns the speed required to travel the entire distance in the specified time.
        /// </summary>
        /// <param name="time">A <strong>TimeSpan</strong> object representing the time to travel the entire distance.</param>
        /// <returns>A <strong>Speed</strong> object representing the speed required to travel the distance in exactly the time specified.</returns>
        public Speed GetMinimumTravelSpeed(TimeSpan time)
        {
            double TravelDistance = ToMeters().Value;
            // Perform the calculation
            return new Speed(TravelDistance / time.TotalSeconds, SpeedUnit.MetersPerSecond);
        }


        /// <summary>Converts the current measurement into feet.</summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// <example>
        /// This example converts various distances into feet.  Note that the ToFeet method converts distances
        /// from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Inches)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Kilometers)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToFeet.ToString)
        /// Debug.WriteLine(Distance2.ToFeet.ToString)
        /// Debug.WriteLine(Distance3.ToFeet.ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Inches);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Kilometers);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToFeet().ToString());
        /// Debug.WriteLine(Distance2.ToFeet().ToString());
        /// Debug.WriteLine(Distance3.ToFeet().ToString());
        /// </code>
        /// </example>
        public Distance ToFeet()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_Value * FeetPerMeter, DistanceUnit.Feet);
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * FeetPerCentimeter, DistanceUnit.Feet);
                case DistanceUnit.Feet:
                    return this;
                case DistanceUnit.Inches:
                    return new Distance(_Value * FeetPerInch, DistanceUnit.Feet);
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * FeetPerKilometer, DistanceUnit.Feet);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * FeetPerStatuteMile, DistanceUnit.Feet);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * FeetPerNauticalMile, DistanceUnit.Feet);
                default:
                    return Distance.Empty;
            }
        }

        /// <summary>Converts the current measurement into inches.</summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <example>
        /// This example converts various distances into inches.  Note that the ToInches method converts distances
        /// from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Kilometers)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToInches.ToString)
        /// Debug.WriteLine(Distance2.ToInches.ToString)
        /// Debug.WriteLine(Distance3.ToInches.ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Kilometers);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToInches().ToString());
        /// Debug.WriteLine(Distance2.ToInches().ToString());
        /// Debug.WriteLine(Distance3.ToInches().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        public Distance ToInches()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_Value * InchesPerMeter, DistanceUnit.Inches);
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * InchesPerCentimeter, DistanceUnit.Inches);
                case DistanceUnit.Feet:
                    return new Distance(_Value * InchesPerFoot, DistanceUnit.Inches);
                case DistanceUnit.Inches:
                    return this;
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * InchesPerKilometer, DistanceUnit.Inches);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * InchesPerStatuteMile, DistanceUnit.Inches);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * InchesPerNauticalMile, DistanceUnit.Inches);
                default:
                    return Distance.Empty;
            }
        }

        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into kilometers.</summary>
        /// <example>
        /// This example converts various distances into kilometers.  Note that the ToKilometers method converts 
        /// distances from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToKilometers.ToString)
        /// Debug.WriteLine(Distance2.ToKilometers.ToString)
        /// Debug.WriteLine(Distance3.ToKilometers.ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToKilometers().ToString());
        /// Debug.WriteLine(Distance2.ToKilometers().ToString());
        /// Debug.WriteLine(Distance3.ToKilometers().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        public Distance ToKilometers()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_Value * KilometersPerMeter, DistanceUnit.Kilometers);
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * KilometersPerCentimeter, DistanceUnit.Kilometers);
                case DistanceUnit.Feet:
                    return new Distance(_Value * KilometersPerFoot, DistanceUnit.Kilometers);
                case DistanceUnit.Inches:
                    return new Distance(_Value * KilometersPerInch, DistanceUnit.Kilometers);
                case DistanceUnit.Kilometers:
                    return this;
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * KilometersPerStatuteMile, DistanceUnit.Kilometers);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * KilometersPerNauticalMile, DistanceUnit.Kilometers);
                default:
                    return Distance.Empty;
            }
        }

        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into meters.</summary>
        /// <example>
        /// This example converts various distances into meters.  Note that the ToMeters method converts distances
        /// from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToMeters().ToString)
        /// Debug.WriteLine(Distance2.ToMeters().ToString)
        /// Debug.WriteLine(Distance3.ToMeters().ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToMeters().ToString());
        /// Debug.WriteLine(Distance2.ToMeters().ToString());
        /// Debug.WriteLine(Distance3.ToMeters().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        public Distance ToMeters()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return this;
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * MetersPerCentimeter, DistanceUnit.Meters);
                case DistanceUnit.Feet:
                    return new Distance(_Value * MetersPerFoot, DistanceUnit.Meters);
                case DistanceUnit.Inches:
                    return new Distance(_Value * MetersPerInch, DistanceUnit.Meters);
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * MetersPerKilometer, DistanceUnit.Meters);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * MetersPerStatuteMile, DistanceUnit.Meters);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * MetersPerNauticalMile, DistanceUnit.Meters);
                default:
                    return Distance.Empty;
            }
        }

        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into meters.</summary>
        /// <example>
        /// This example converts various distances into meters.  Note that the ToMeters method converts distances
        /// from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToMeters().ToString)
        /// Debug.WriteLine(Distance2.ToMeters().ToString)
        /// Debug.WriteLine(Distance3.ToMeters().ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToMeters().ToString());
        /// Debug.WriteLine(Distance2.ToMeters().ToString());
        /// Debug.WriteLine(Distance3.ToMeters().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        public Distance ToCentimeters()
        {
            switch (_Units)
            {
                case DistanceUnit.Centimeters:
                    return this;
                case DistanceUnit.Meters:
                    return new Distance(_Value * CentimetersPerMeter, DistanceUnit.Centimeters);
                case DistanceUnit.Feet:
                    return new Distance(_Value * CentimetersPerFoot, DistanceUnit.Centimeters);
                case DistanceUnit.Inches:
                    return new Distance(_Value * CentimetersPerInch, DistanceUnit.Centimeters);
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * CentimetersPerKilometer, DistanceUnit.Centimeters);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * CentimetersPerStatuteMile, DistanceUnit.Centimeters);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * CentimetersPerNauticalMile, DistanceUnit.Centimeters);
                default:
                    return Distance.Empty;
            }
        }

        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into nautical miles.</summary>
        /// <example>
        /// This example converts various distances into nautical miles.  Note that the ToNauticalMiles method 
        /// converts distances from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToNauticalMiles.ToString)
        /// Debug.WriteLine(Distance2.ToNauticalMiles.ToString)
        /// Debug.WriteLine(Distance3.ToNauticalMiles.ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToNauticalMiles().ToString());
        /// Debug.WriteLine(Distance2.ToNauticalMiles().ToString());
        /// Debug.WriteLine(Distance3.ToNauticalMiles().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        public Distance ToNauticalMiles()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_Value * NauticalMilesPerMeter, DistanceUnit.NauticalMiles);
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * NauticalMilesPerCentimeter, DistanceUnit.NauticalMiles);
                case DistanceUnit.Feet:
                    return new Distance(_Value * NauticalMilesPerFoot, DistanceUnit.NauticalMiles);
                case DistanceUnit.Inches:
                    return new Distance(_Value * NauticalMilesPerInch, DistanceUnit.NauticalMiles);
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * NauticalMilesPerKilometer, DistanceUnit.NauticalMiles);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_Value * NauticalMilesPerStatuteMile, DistanceUnit.NauticalMiles);
                case DistanceUnit.NauticalMiles:
                    return this;
                default:
                    return Distance.Empty;
            }
        }

        /// <returns>A new <strong>Distance</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into miles.</summary>
        /// <example>
        /// This example converts various distances into statute miles.  Note that the ToStatuteMiles method 
        /// converts distances from any source type.
        /// <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToStatuteMiles.ToString)
        /// Debug.WriteLine(Distance2.ToStatuteMiles.ToString)
        /// Debug.WriteLine(Distance3.ToStatuteMiles.ToString)
        /// </code>
        /// <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result 
        /// Debug.WriteLine(Distance1.ToStatuteMiles().ToString());
        /// Debug.WriteLine(Distance2.ToStatuteMiles().ToString());
        /// Debug.WriteLine(Distance3.ToStatuteMiles().ToString());
        /// </code>
        /// </example>
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        public Distance ToStatuteMiles()
        {
            switch (_Units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_Value * StatuteMilesPerMeter, DistanceUnit.StatuteMiles);
                case DistanceUnit.Centimeters:
                    return new Distance(_Value * StatuteMilesPerCentimeter, DistanceUnit.StatuteMiles);
                case DistanceUnit.Feet:
                    return new Distance(_Value * StatuteMilesPerFoot, DistanceUnit.StatuteMiles);
                case DistanceUnit.Inches:
                    return new Distance(_Value * StatuteMilesPerInch, DistanceUnit.StatuteMiles);
                case DistanceUnit.Kilometers:
                    return new Distance(_Value * StatuteMilesPerKilometer, DistanceUnit.StatuteMiles);
                case DistanceUnit.StatuteMiles:
                    return this;
                case DistanceUnit.NauticalMiles:
                    return new Distance(_Value * StatuteMilesPerNauticalMile, DistanceUnit.StatuteMiles);
                default:
                    return Distance.Empty;
            }
        }


        public Distance ToUnitType(DistanceUnit newUnits)
        {
            switch (newUnits)
            {
                case DistanceUnit.Centimeters:
                    return ToCentimeters();
                case DistanceUnit.Feet:
                    return ToFeet();
                case DistanceUnit.Inches:
                    return ToInches();
                case DistanceUnit.Kilometers:
                    return ToKilometers();
                case DistanceUnit.Meters:
                    return ToMeters();
                case DistanceUnit.NauticalMiles:
                    return ToNauticalMiles();
                case DistanceUnit.StatuteMiles:
                    return ToStatuteMiles();
                default:
                    return Distance.Empty;
            }
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the 
        /// user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// determine the smallest Imperial unit type.</remarks>
        public Distance ToImperialUnitType()
        {
            // Start with the largest possible unit
            Distance Temp = ToStatuteMiles();
            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToFeet();
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToInches();
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToCentimeters();
            return Temp;
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the 
        /// user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// determine the smallest metric unit type.</remarks>
        public Distance ToMetricUnitType()
        {
            // Yes. Start with the largest possible unit
            Distance Temp = ToKilometers();

            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToMeters();

            // And so on until we find the right unit
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToCentimeters();

            return Temp;
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the 
        /// user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// find the smallest unit type and convert the unit to the user's local
        /// numeric system (Imperial or Metric).</remarks>
        public Distance ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            else
                return ToImperialUnitType();
        }

        /// <summary>
        /// Returns the distance traveled at the current speed for the specified time.
        /// </summary>
        /// <param name="time">A length of time to travel.</param>
        /// <returns>A <strong>Distance</strong> representing the distance travelled at
        /// the current speed for the specified length of time.</returns>
        public Speed ToSpeed(TimeSpan time)
        {
            return new Speed(ToMeters().Value / (time.TotalMilliseconds / 1000.0), SpeedUnit.MetersPerSecond).ToLocalUnitType();
        }

        /// <summary>
        /// Returns a new instance rounded to the specified number of digits.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of digits to round off to.</param>
        /// <returns></returns>
        public Distance Round(int decimals)
        {
            return new Distance(Math.Round(_Value, decimals), _Units);
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the distance in the specified format.</returns>
        /// <param name="format">
        ///	 <para>A combination of symbols, spaces, and any of the following case-insensitive
        ///	 letters: <strong>#</strong> or <strong>0</strong> for the value property, and <strong>U</strong> for
        ///	 distance units. Here are some examples:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///				 <tr>
        ///					 <td>##0.## uu</td>
        ///					 <td>## uuuu</td>
        ///					 <td># u</td>
        ///					 <td>###</td>
        ///				 </tr>
        ///		 </table>
        ///	 </para>
        /// </param>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement using a custom format.
        /// <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu")
        /// </code>
        /// <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu");
        /// </code>
        /// </example>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #region Math Methods

        public Distance Add(Distance value)
        {
            return new Distance(_Value + value.ToUnitType(Units).Value, _Units);
        }

        public Distance Add(double value)
        {
            return new Distance(_Value + value, _Units);
        }

        public Distance Subtract(Distance value)
        {
            return new Distance(_Value - value.ToUnitType(Units).Value, _Units);
        }

        public Distance Subtract(double value)
        {
            return new Distance(_Value - value, _Units);
        }

        public Distance Multiply(Distance value)
        {
            return new Distance(_Value * value.ToUnitType(Units).Value, _Units);
        }

        public Distance Multiply(double value)
        {
            return new Distance(_Value * value, _Units);
        }

        public Distance Divide(Distance value)
        {
            return new Distance(_Value / value.ToUnitType(Units).Value, _Units);
        }

        public Distance Divide(double value)
        {
            return new Distance(_Value / value, _Units);
        }

        public Distance Increment()
        {
            return new Distance(_Value + 1.0, _Units);
        }

        public Distance Decrement()
        {
            return new Distance(_Value - 1.0, _Units);
        }

        public bool IsLessThan(Distance value)
        {
            return CompareTo(value) < 0;
        }

        public bool IsLessThanOrEqualTo(Distance value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        public bool IsGreaterThan(Distance value)
        {
            return CompareTo(value) > 0;
        }

        public bool IsGreaterThanOrEqualTo(Distance value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Compares the current instance to the specified object.
        /// </summary>
        /// <param name="obj">An <strong>Object</strong> to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Distance)
                // If the type is the same, compare the values
                return Equals((Distance)obj);
            else
                // Defer to the Object class
                return base.Equals(obj);
        }

        ///// <summary>
        ///// Returns a distance, in inches, matching the specified distance in pixels.
        ///// </summary>
        ///// <param name="pixels"></param>
        ///// <returns></returns>
        //public static Distance FromPixels(double pixels)
        //{
        //    return new Distance(pixels / CurrentPixelsPerInch, DistanceUnit.Inches);
        //}

        //        public static double CurrentPixelsPerInch
        //        {
        //            get
        //            {
        //                double pDpi;
        //#if !PocketPC
        //                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd((IntPtr)0))
        //                {
        //                    pDpi = (g.DpiX + g.DpiY) * 0.5;
        //                }
        //#else
        //                //            Pocket PC: 
        //                //            Portrait / Landscape QVGA (240x320, 96 dpi) 
        //                //            Portrait / Landscape VGA (480x640, 192 dpi) 
        //                //            Square screen (240x240, 96 dpi) 
        //                //            Square screen VGA (480x480, 192 dpi) 
        //                //            Smartphone: 
        //                //            Portrait (176x220, 96 dpi) 
        //                //            Portrait QVGA (240x320, 192 dpi) 
        //                switch (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width)
        //                {
        //                    case 240:
        //                        switch (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
        //                        {
        //                            case 320:
        //                                pDpi = 192;
        //                                break;
        //                            default:
        //                                pDpi = 96;
        //                                break;
        //                        }
        //                        break;
        //                    case 480:
        //                        switch (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
        //                        {
        //                            default:
        //                                pDpi = 192;
        //                                break;
        //                        }
        //                        break;
        //                    case 176:
        //                        switch (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
        //                        {
        //                            default:
        //                                pDpi = 96;
        //                                break;
        //                        }
        //                        break;
        //                    default:
        //                        pDpi = 96;
        //                        break;
        //                }
        //#endif
        //                return pDpi;
        //            }
        //        }

        public override int GetHashCode()
        {
            return ToMeters().Value.GetHashCode();
        }


        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the current distance in the default format.</returns>
        /// <remarks>The default format used is "##0.## uu" where <strong>uu</strong> is the distance unit type.
        /// The numeric format may vary depending on the machine's local culture.</remarks>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement.
        /// <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString
        /// </code>
        /// <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString();
        /// </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion

        #region Static Methods

        /// <summary>Returns a random distance between 0 and 1,000 meters.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Distance Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>Returns a random distance between 0 and 1,000 meters.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>

        public static Distance Random(Random generator)
        {
            return new Distance(generator.NextDouble() * 1000, DistanceUnit.Meters).ToLocalUnitType();
        }

        /// <summary>
        /// Returns a random distance between zero and the specified maximum.
        /// </summary>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static Distance Random(Distance maximum)
        {
            return new Distance(new Random(DateTime.Now.Millisecond).NextDouble() * maximum.Value, maximum.Units);
        }

        public static Distance FromCentimeters(double value)
        {
            return new Distance(value, DistanceUnit.Centimeters);
        }

        public static Distance FromFeet(double value)
        {
            return new Distance(value, DistanceUnit.Feet);
        }

        public static Distance FromInches(double value)
        {
            return new Distance(value, DistanceUnit.Inches);
        }

        public static Distance FromKilometers(double value)
        {
            return new Distance(value, DistanceUnit.Kilometers);
        }

        public static Distance FromMeters(double value)
        {
            return new Distance(value, DistanceUnit.Meters);
        }

        public static Distance FromNauticalMiles(double value)
        {
            return new Distance(value, DistanceUnit.NauticalMiles);
        }

        public static Distance FromStatuteMiles(double value)
        {
            return new Distance(value, DistanceUnit.StatuteMiles);
        }

        public static Distance FromCentimeters(int value)
        {
            return new Distance(value, DistanceUnit.Centimeters);
        }

        public static Distance FromFeet(int value)
        {
            return new Distance(value, DistanceUnit.Feet);
        }

        public static Distance FromInches(int value)
        {
            return new Distance(value, DistanceUnit.Inches);
        }

        public static Distance FromKilometers(int value)
        {
            return new Distance(value, DistanceUnit.Kilometers);
        }

        public static Distance FromMeters(int value)
        {
            return new Distance(value, DistanceUnit.Meters);
        }

        public static Distance FromNauticalMiles(int value)
        {
            return new Distance(value, DistanceUnit.NauticalMiles);
        }

        public static Distance FromStatuteMiles(int value)
        {
            return new Distance(value, DistanceUnit.StatuteMiles);
        }

        public static DistanceUnit ParseDistanceUnit(string value)
        {
#if !PocketPC || Framework20
            return (DistanceUnit)Enum.Parse(typeof(DistanceUnit), value, true);
#else
			return (DistanceUnit)Enum.ToObject(typeof(DistanceUnit), value);

//			value = value.ToLower(CultureInfo.InvariantCulture);
//			switch(value)
//			{
//				case "kilometers":
//					return DistanceUnit.Kilometers;
//				case "meters":
//					return DistanceUnit.Meters;
//				case "centimeters":
//					return DistanceUnit.Centimeters;
//				case "nauticalmiles":
//					return DistanceUnit.NauticalMiles;
//				case "statutemiles":
//					return DistanceUnit.StatuteMiles;
//				case "feet":
//					return DistanceUnit.Feet;
//				case "inches":
//					return DistanceUnit.Inches;
//				default:
//					throw new ArgumentException("The specified value could not be recognized as an DistanceUnit enumeration value.");
//			}
#endif
        }


        /// <summary>Converts a string-based distance measurement into a Distance object.</summary>
        /// <remarks>
        /// This powerful constructor is typically used to convert a string-based distance
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Distance</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.
        /// </remarks>
        /// <param name="value">
        ///	 <para>A <strong>String</strong> describing a case-insensitive distance measurement,
        ///	 in any of the following formats, where <strong>N</strong> represents a numeric
        ///	 value:</para>
        /// 
        ///	 <list type="bullet">
        ///		 <item>N m</item>
        ///		 <item>N meters</item>
        ///		 <item>N meter</item>
        ///		 <item>N metre</item>
        ///		 <item>N metres</item>
        ///		 <item>N km</item>
        ///		 <item>N kilometers</item>
        ///		 <item>N kilometer</item>
        ///		 <item>N kilometre</item>
        ///		 <item>N kilometres</item>
        ///		 <item>N ft</item>
        ///		 <item>N'</item>
        ///		 <item>N foot</item>
        ///		 <item>N feet</item>
        ///		 <item>N in</item>
        ///		 <item>N"</item>
        ///		 <item>N inch</item>
        ///		 <item>N inches</item>
        ///		 <item>N mi</item>
        ///		 <item>N mile</item>
        ///		 <item>N miles</item>
        ///		 <item>N nm</item>
        ///		 <item>N nautical mile</item>
        ///		 <item>N nautical miles</item>
        ///	 </list>
        /// </param>
        /// <returns>
        /// A new Distance object containing the parsed <see cref="Value">value</see> and
        /// <see cref="Units">unit</see> type.
        /// </returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid distance measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the distance measurement was not recognized.<br/>
        /// 2. The distance unit type was not recognized or not specified.</exception>
        /// <example>
        /// This example demonstrates how the Parse method can convert several string formats into a Distance object.
        /// <code lang="VB">
        /// Dim NewDistance As Distance
        /// ' Create a distance of 50 kilometers
        /// NewDistance = Distance.Parse("50 km")
        /// ' Create a distance of 14,387 miles, then convert it into inches
        /// NewDistance = Distance.Parse("14,387 statute miles").ToInches
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewDistance = Distance.Parse("	50 '	   ")
        /// </code>
        /// <code lang="C#">
        /// Distance NewDistance;
        /// // Create a distance of 50 kilometers
        /// NewDistance = Distance.Parse("50 km");
        /// // Create a distance of 14,387 miles, then convert it into inches
        /// NewDistance = Distance.Parse("14,387 statute miles").ToInches;
        /// // Parse an untrimmed measurement into 50 feet
        /// NewDistance = Distance.Parse("	50 '	   ");
        /// </code>
        /// </example>
        public static Distance Parse(string value)
        {
            return new Distance(value, CultureInfo.CurrentCulture);
        }

        public static Distance Parse(string value, CultureInfo culture)
        {
            return new Distance(value, culture);
        }

        #endregion

        #region Operators

        public static Distance operator +(Distance left, Distance right)
        {
            return left.Add(right);
        }

        public static Distance operator -(Distance left, Distance right)
        {
            return left.Subtract(right);
        }

        public static Distance operator *(Distance left, Distance right)
        {
            return left.Multiply(right);
        }

        public static Distance operator /(Distance left, Distance right)
        {
            return left.Divide(right);
        }

        public static bool operator <(Distance left, Distance right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Distance left, Distance right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        public static bool operator ==(Distance left, Distance right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Distance left, Distance right)
        {
            return !(left.Equals(right));
        }

        public static bool operator >=(Distance left, Distance right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        public static bool operator >(Distance left, Distance right)
        {
            return left.CompareTo(right) > 0;
        }

        #endregion

        #region Conversions

        public static explicit operator Distance(string value)
        {
            return new Distance(value, CultureInfo.CurrentCulture);
        }

        public static explicit operator string(Distance value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region IEquatable<Distance> Members

        /// <summary>
        /// Compares the current instance to the specified distance object.
        /// </summary>
        /// <param name="other">A <strong>Distance</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks>This method compares the current instance to the specified object up to four digits of precision.</remarks>
        public bool Equals(Distance other)
        {
            return _Value.Equals(other.ToUnitType(_Units).Value);
        }

        /// <summary>
        /// Compares the current instance to the specified value, distance units, and precision.
        /// </summary>
        /// <param name="other">A <strong>Distance</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of digits to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks>This method compares the current instance to the specified object at up to the specified number of digits of precision.</remarks>
        public bool Equals(Distance other, int decimals)
        {
            return Math.Round(_Value, decimals) == Math.Round(other.ToUnitType(_Units).Value, decimals);
        }        

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and local culture.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the distance in the specified format.</returns>
        /// <param name="format">
        ///	 <para>A combination of symbols, spaces, and any of the following case-insensitive
        ///	 letters: <strong>#</strong> or <strong>0</strong> for the value property, and <strong>U</strong> for
        ///	 distance units. Here are some examples:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///				 <tr>
        ///					 <td>##0.## uu</td>
        ///					 <td>## uuuu</td>
        ///					 <td># u</td>
        ///					 <td>###</td>
        ///				 </tr>
        ///		 </table>
        ///	 </para>
        /// </param>
        /// <param name="formatProvider">
        /// Information about the culture to apply to the numeric format.
        /// </param>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement using a custom format and culture information.
        /// <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu", CultureInfo.CurrentUICulture)
        /// </code>
        /// <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu", CultureInfo.CurrentUICulture);
        /// </code>
        /// </example>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            try
            {
                // Use the default if "g" is passed
                if (String.Compare(format, "g", true, culture) == 0)
                {
                    format = "#" + culture.NumberFormat.NumberGroupSeparator + "##0.00 uu";
                }

                if (culture == null)
                    culture = CultureInfo.CurrentCulture;

                // Convert the format to uppercase
                format = format.ToUpper(CultureInfo.InvariantCulture);

                // Convert the localized format string to a US format
                format = format.Replace("V", "0");

                // Replace the "d" with "h" since degrees is the same as hours
                format = Value.ToString(format, culture);

                // Is there a units specifier?
                int StartChar = format.IndexOf("U");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent U characters or a period
                    int EndChar = format.LastIndexOf("U");
                    // Extract the sub-string
                    string SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Show the unit based on the length
                    switch (SubFormat.Length)
                    {
                        case 1:
                            switch (_Units)
                            {
                                case DistanceUnit.Centimeters:
                                    format = format.Replace("U", "cm");
                                    break;
                                case DistanceUnit.Feet:
                                    format = format.Replace("U", "'");
                                    break;
                                case DistanceUnit.Inches:
                                    format = format.Replace("U", "\"");
                                    break;
                                case DistanceUnit.Kilometers:
                                    format = format.Replace("U", "km");
                                    break;
                                case DistanceUnit.Meters:
                                    format = format.Replace("U", "m");
                                    break;
                                case DistanceUnit.StatuteMiles:
                                    format = format.Replace("U", "mi");
                                    break;
                                case DistanceUnit.NauticalMiles:
                                    format = format.Replace("U", "nm");
                                    break;
                            }
                            break;
                        case 2:
                            switch (_Units)
                            {
                                case DistanceUnit.Centimeters:
                                    format = format.Replace("UU", "cm");
                                    break;
                                case DistanceUnit.Feet:
                                    format = format.Replace("UU", "ft");
                                    break;
                                case DistanceUnit.Inches:
                                    format = format.Replace("UU", "in");
                                    break;
                                case DistanceUnit.Kilometers:
                                    format = format.Replace("UU", "km");
                                    break;
                                case DistanceUnit.Meters:
                                    format = format.Replace("UU", "m");
                                    break;
                                case DistanceUnit.StatuteMiles:
                                    format = format.Replace("UU", "mi");
                                    break;
                                case DistanceUnit.NauticalMiles:
                                    format = format.Replace("UU", "nm");
                                    break;
                            }
                            break;
                        case 3:
                            if (Value == 1)
                            {
                                switch (_Units)
                                {
                                    case DistanceUnit.Centimeters:
                                        format = format.Replace("UUU", "centimeter");
                                        break;
                                    case DistanceUnit.Feet:
                                        format = format.Replace("UUU", "foot");
                                        break;
                                    case DistanceUnit.Inches:
                                        format = format.Replace("UUU", "inch");
                                        break;
                                    case DistanceUnit.Kilometers:
                                        format = format.Replace("UUU", "kilometer");
                                        break;
                                    case DistanceUnit.Meters:
                                        format = format.Replace("UUU", "meter");
                                        break;
                                    case DistanceUnit.StatuteMiles:
                                        format = format.Replace("UUU", "mile");
                                        break;
                                    case DistanceUnit.NauticalMiles:
                                        format = format.Replace("UUU", "nautical mile");
                                        break;
                                }
                            }
                            else
                            {
                                switch (_Units)
                                {
                                    case DistanceUnit.Centimeters:
                                        format = format.Replace("UUU", "centimeters");
                                        break;
                                    case DistanceUnit.Feet:
                                        format = format.Replace("UUU", "feet");
                                        break;
                                    case DistanceUnit.Inches:
                                        format = format.Replace("UUU", "inches");
                                        break;
                                    case DistanceUnit.Kilometers:
                                        format = format.Replace("UUU", "kilometers");
                                        break;
                                    case DistanceUnit.Meters:
                                        format = format.Replace("UUU", "meters");
                                        break;
                                    case DistanceUnit.StatuteMiles:
                                        format = format.Replace("UUU", "miles");
                                        break;
                                    case DistanceUnit.NauticalMiles:
                                        format = format.Replace("UUU", "nautical miles");
                                        break;
                                }
                            }
                            break;
                    }
                }
                // Return the final value
                return format;
            }
            catch (Exception ex)
            {
                throw new FormatException(Properties.Resources.Distance_InvalidFormat, ex);
            }
            //catch
            //{
            //    throw new FormatException(Properties.Resources.Distance_InvalidFormat));
            //}		
        }

        #endregion

        #region IComparable<Distance> Members

        public int CompareTo(Distance other)
        {
            return _Value.CompareTo(other.ToUnitType(_Units).Value);
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

            _Units = (DistanceUnit)Enum.Parse(typeof(DistanceUnit), reader.ReadElementContentAsString(), true);
            _Value = reader.ReadElementContentAsDouble();
        }

        #endregion
    }

    /// <summary>Indicates the unit of measure for distance measurements.</summary>
    /// <remarks>
    /// This enumeration is most frequently used by the
    /// <see cref="Distance.Units">Units</see> property of the
    /// <see cref="Distance">Distance</see>
    /// class in conjunction with the <see cref="Distance.Value">Value</see>
    /// property to describe a straight-line distance.
    /// </remarks>
    /// <seealso cref="Distance.Value">Value Property (Distance Class)</seealso>
    /// <seealso cref="Distance.Units">Units Property (Distance Class)</seealso>
    public enum DistanceUnit : int
    {
        /// <summary>Metric System. Kilometers (thousands of meters).</summary>
        Kilometers,
        /// <summary>Metric System. 1/1000th of a kilometer.</summary>
        Meters,
        /// <summary>Metric System. 1/100th of a meter.</summary>
        Centimeters,
        /// <summary>Nautical miles, also known as "sea miles".</summary>
        NauticalMiles,
        /// <summary>Imperial System. A statute mile, most often referred to just as "mile."</summary>
        StatuteMiles,
        /// <summary>Imperial System. Feet.</summary>
        Feet,
        /// <summary>Imperial System. Inches.</summary>
        Inches
    }

 
}