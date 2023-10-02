// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents the measurement of a straight line between between two points on
    /// Earth's surface.
    /// </summary>
    [TypeConverter("DotSpatial.Positioning.Design.DistanceConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
    public struct Distance : IFormattable, IComparable<Distance>, IEquatable<Distance>, IXmlSerializable
    {

        #region Constants

        private const double FATHOMS_PER_CENTIMETER = 0.0054680665;
        private const double FATHOMS_PER_METER = 0.5468066492;
        private const double FATHOMS_PER_FOOT = 0.1666666667;
        private const double FATHOMS_PER_INCH = 0.0138888889;
        private const double FATHOMS_PER_KILOMETER = 546.8066491689;
        private const double FATHOMS_PER_STATUTE_MILE = 880.0017600035;
        private const double FATHOMS_PER_NAUTICAL_MILE = 1012.6859142607;

        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_METER = 3.2808399;
        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_CENTIMETER = 0.032808399;
        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_STATUTE_MILE = 5280;
        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_KILOMETER = 3280.8399;
        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_INCH = 0.0833333333333333;
        /// <summary>
        ///
        /// </summary>
        private const double FEET_PER_NAUTICAL_MILE = 6076.11549;
        /// <summary>
        /// 1 fathom is equal to 6 ft
        /// </summary>
        private const double FEET_PER_FATHOM = 6;

        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_METER = 39.3700787;
        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_CENTIMETER = 0.393700787;
        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_STATUTE_MILE = 63360;
        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_KILOMETER = 39370.0787;
        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_FOOT = 12.0;
        /// <summary>
        ///
        /// </summary>
        private const double INCHES_PER_NAUTICAL_MILE = 72913.3858;
        /// <summary>
        /// 1 fathom is equal to 72 inches
        /// </summary>
        private const double INCHES_PER_FATHOM = 72;

        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_METER = 0.000621371192;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_CENTIMETER = 0.00000621371192;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_KILOMETER = 0.621371192;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_INCH = 0.0000157828283;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_FOOT = 0.000189393939;
        /// <summary>
        ///
        /// </summary>
        private const double STATUTE_MILES_PER_NAUTICAL_MILE = 1.15077945;
        /// <summary>
        /// 1 fathom is equal to 0.0011363614 mi
        /// </summary>
        private const double STATUTE_MILES_PER_FATHOM = 0.0011363614;

        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_METER = 0.000539956803;
        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_CENTIMETER = 0.00000539956803;
        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_KILOMETER = 0.539956803;
        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_INCH = 0.0000137149028;
        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_FOOT = 0.000164578834;
        /// <summary>
        ///
        /// </summary>
        private const double NAUTICAL_MILES_PER_STATUTE_MILE = 0.868976242;
        /// <summary>
        /// 1 fathom is equal to 0.000987473 nautical mile
        /// </summary>
        private const double NAUTICAL_MILES_PER_FATHOM = 0.000987473;

        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_STATUTE_MILE = 160934.4;
        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_KILOMETER = 100000;
        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_FOOT = 30.48;
        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_INCH = 2.54;
        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_METER = 100;
        /// <summary>
        ///
        /// </summary>
        private const double CENTIMETERS_PER_NAUTICAL_MILE = 185200;
        /// <summary>
        /// 1 fathom is equal to 182.88 cm
        /// </summary>
        private const double CENTIMETERS_PER_FATHOM = 182.88;

        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_STATUTE_MILE = 1609.344;
        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_CENTIMETER = 0.01;
        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_KILOMETER = 1000;
        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_FOOT = 0.3048;
        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_INCH = 0.0254;
        /// <summary>
        ///
        /// </summary>
        private const double METERS_PER_NAUTICAL_MILE = 1852;
        /// <summary>
        /// 1 fathom is equal to 1.8288 m
        /// </summary>
        private const double METERS_PER_FATHOM = 1.8288;

        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_METER = 0.001;
        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_CENTIMETER = 0.00001;
        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_STATUTE_MILE = 1.609344;
        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_FOOT = 0.0003048;
        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_INCH = 0.0000254;
        /// <summary>
        ///
        /// </summary>
        private const double KILOMETERS_PER_NAUTICAL_MILE = 1.852;
        /// <summary>
        /// 1 fathom is equal to 0.0018288 km
        /// </summary>
        private const double KILOMETERS_PER_FATHOM = 0.0018288;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Returns the distance from the center of the Earth to the equator according to the WGS1984 ellipsoid.
        /// </summary>
        /// <seealso cref="EarthsPolarRadiusWgs1984">EarthsPolarRadiusWgs1984
        /// Field</seealso>
        ///
        /// <seealso cref="EarthsAverageRadius">EarthsAverageRadius Field</seealso>
        public static readonly Distance EarthsEquatorialRadiusWgs1984 = new(6378137.0, DistanceUnit.Meters);
        /// <summary>
        /// Represents an infinite distance.
        /// </summary>
        public static readonly Distance Infinity = new(double.PositiveInfinity, DistanceUnit.Meters);
        /// <summary>
        /// Returns the distance from the center of the Earth to a pole according to the WGS1984 ellipsoid.
        /// </summary>
        /// <seealso cref="EarthsEquatorialRadiusWgs1984">EarthsEquatorialRadiusWgs1984 Field</seealso>
        ///
        /// <seealso cref="EarthsAverageRadius">EarthsAverageRadius Field</seealso>
        public static readonly Distance EarthsPolarRadiusWgs1984 = new(6356752.314245, DistanceUnit.Meters);
        /// <summary>
        /// Returns the average radius of the Earth.
        /// </summary>
        /// <seealso cref="EarthsEquatorialRadiusWgs1984">EarthsEquatorialRadiusWgs1984 Field</seealso>
        ///
        /// <seealso cref="EarthsPolarRadiusWgs1984">EarthsPolarRadiusWgs1984 Field</seealso>
        public static readonly Distance EarthsAverageRadius = new(6378100, DistanceUnit.Meters);
        /// <summary>
        ///
        /// </summary>
        public static readonly Distance Empty = new Distance(0, DistanceUnit.Meters).ToLocalUnitType();
        /// <summary>
        ///
        /// </summary>
        public static readonly Distance SeaLevel = new Distance(0, DistanceUnit.Meters).ToLocalUnitType();
        /// <summary>
        ///
        /// </summary>
        public static readonly Distance Maximum = new Distance(double.MaxValue, DistanceUnit.Kilometers).ToLocalUnitType();
        /// <summary>
        ///
        /// </summary>
        public static readonly Distance Minimum = new Distance(double.MinValue, DistanceUnit.Kilometers).ToLocalUnitType();
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Distance Invalid = new(double.NaN, DistanceUnit.Kilometers);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value and unit type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="units">The units.</param>
        /// <example>
        /// This example uses a constructor to create a new distance of 50km.
        ///   <code lang="VB">
        /// Dim MyDistance As New Distance(50, DistanceUnit.Kilometers)
        ///   </code>
        ///   <code lang="C#">
        /// Distance MyDistance = new Distance(50, DistanceUnit.Kilometers);
        ///   </code>
        ///   </example>
        public Distance(double value, DistanceUnit units)
        {
            Value = value;
            Units = units;
        }

        /// <summary>
        /// Creates a new instance from the the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid distance measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the distance measurement was not recognized.<br/>
        /// 2. The distance unit type was not recognized or not specified.</exception>
        ///
        /// <example>
        /// This example demonstrates how the to use this constructor.
        ///   <code lang="VB">
        /// Dim MyDistance As Distance
        /// ' Create a distance of 50 kilometers
        /// MyDistance = New Distance("50 km")
        /// ' Create a distance of 14, 387 miles, then convert it into inches
        /// MyDistance = New Distance("14, 387 statute miles").ToInches
        /// ' Parse an untrimmed measurement into 50 feet
        /// MyDistance = New Distance("	50 '	   ")
        ///   </code>
        ///   <code lang="C#">
        /// Distance MyDistance;
        /// // Create a distance of 50 kilometers
        /// MyDistance = new Distance("50 km");
        /// // Create a distance of 14, 387 miles, then convert it into inches
        /// MyDistance = new Distance("14, 387 statute miles").ToInches;
        /// // Parse an untrimmed measurement into 50 feet
        /// MyDistance = new Distance("	50 '	   ");
        ///   </code>
        ///   </example>
        /// <remarks>This powerful constructor is typically used to initialize an instance with a
        /// string-based distance measurement, such as one entered by a user or read from a file.
        /// This constructor can accept any output created via the
        /// <see cref="ToString()">ToString</see> method.</remarks>
        public Distance(string value)
            : this(value, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Distance"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public Distance(string value, CultureInfo culture)
        {
            // Anything to do?
            if (string.IsNullOrEmpty(value))
            {
                // Return a blank distance in Imperial or English system
                Value = 0;
                Units = DistanceUnit.Meters;
                return;
            }

            // Default to the current culture if none is given
            culture ??= CultureInfo.CurrentCulture;

            // Trim surrounding spaces and switch to uppercase
            value = value.Trim()
                .ToUpper(CultureInfo.InvariantCulture)
                .Replace(culture.NumberFormat.NumberGroupSeparator, string.Empty);
            // Is it infinity?
            if (value == "INFINITY")
            {
                Value = Infinity.Value;
                Units = DistanceUnit.Meters;
                return;
            }

            if (value is "SEALEVEL" or "SEA LEVEL")
            {
                Value = SeaLevel.Value;
                Units = DistanceUnit.Meters;
                return;
            }

            if (value == "EMPTY")
            {
                Value = 0;
                Units = DistanceUnit.Meters;
                return;
            }
            // Go until the first non-number character
            int count = 0;
            while (count < value.Length)
            {
                string subValue = value[count].ToString(CultureInfo.InvariantCulture);
                if (char.IsDigit(subValue, 0) ||
                    subValue == culture.NumberFormat.NumberGroupSeparator ||
                    subValue == culture.NumberFormat.NumberDecimalSeparator ||
                    subValue == culture.NumberFormat.NegativeSign)
                {
                    // Allow continuation
                    count++;
                }
                else
                {
                    // Non-numeric character!
                    break;
                }
            }

            string unit = value[count..].Trim();
            // Get the numeric portion
            string numericPortion = count > 0 ? value[..count] : "0";
            try
            {
                Value = double.Parse(numericPortion, culture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Resources.Distance_InvalidNumericPortion, nameof(value), ex);
            }

            // Try to interpret the measurement
            if (unit is "M" or "M." or "METERS" or "METRES" or "METRE" or "METER")
            {
                Units = DistanceUnit.Meters;
            }
            else if (unit is "CM" or "CM." or "CENTIMETER" or "CENTIMETERS" or "CENTIMETRE" or "CENTIMETRES")
            {
                Units = DistanceUnit.Centimeters;
            }
            else if (unit is "KM" or "KM." or "KILOMETRES" or "KILOMETERS" or "KILOMETRE" or "KILOMETER")
            {
                Units = DistanceUnit.Kilometers;
            }
            else if (unit is "MI" or "MI." or "MILE" or "MILES" or "STATUTE MILES")
            {
                Units = DistanceUnit.StatuteMiles;
            }
            else if (unit is "NM" or "NM." or "NAUTICAL MILE" or "NAUTICAL MILES")
            {
                Units = DistanceUnit.NauticalMiles;
            }
            else if (unit is "IN" or "IN." or "\"" or "INCHES" or "INCH")
            {
                Units = DistanceUnit.Inches;
            }
            else if (unit is "FT" or "FT." or "'" or "FOOT" or "FEET")
            {
                Units = DistanceUnit.Feet;
            }
            else if (unit is "FM" or "FATH" or "FATHOM")
            {
                Units = DistanceUnit.Fathom;
            }
            else
            {
                throw new ArgumentException(Resources.Distance_InvalidUnitPortion, nameof(value));
            }
        }

        /// <summary>
        /// Creates a new instance from the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Distance(XmlReader reader)
        {
            // Initialize all fields
            Value = double.NaN;
            Units = 0;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the numeric portion of a distance measurement.
        /// </summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <example>
        /// This example demonstrates how to use the Value property to modify a distance object.  The object
        /// is then converted to kilometers.
        ///   <code lang="VB">
        /// ' Declare a distance of 0 mi.
        /// Dim MyDistance As New Distance(0, DistanceUnit.StatuteMiles)
        /// ' Change the distance to 100 mi.
        /// MyDistance.Value = 100
        /// ' Change the distance to 12.3456 mi.
        /// MyDistance.Value = 12.3456
        /// ' Convert the measurement into kilometers
        /// MyDistance = MyDistance.ToKilometers
        ///   </code>
        ///   <code lang="C#">
        /// // Declare a distance of 0 mi.
        /// Distance MyDistance = new Distance(0, DistanceUnit.StatuteMiles);
        /// // Change the distance to 100 mi.
        /// MyDistance.Value = 100;
        /// // Change the distance to 12.3456 mi.
        /// MyDistance.Value = 12.3456;
        /// // Convert the measurement into kilometers
        /// MyDistance = MyDistance.ToKilometers;
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="Units">Units Property</seealso>
        /// <remarks>This property is paired with the <see cref="Units">Units</see> property to form a complete distance
        /// measurement.</remarks>
        public double Value { get; private set; }

        /// <summary>
        /// Describes the unit portion of a distance measurement.
        /// </summary>
        /// <value>A value from the <see cref="DistanceUnit">DistanceUnit</see> enumeration. Default
        /// is <strong>DistanceUnit.Meters</strong>.</value>
        /// <seealso cref="Value">Value Property</seealso>
        /// <remarks><para>Each distance measurement is comprised of a numeric <see cref="Value">value</see>
        /// and a unit type.  This property describes the numeric value so that it may be
        /// explicitly identified. An instance of the <strong>Distance</strong> class may have a value
        /// of zero, but it is impossible to have an unspecified unit type.</para>
        ///   <para><img src="BestPractice.jpg"/></para><para><strong>Use conversion methods instead of setting the
        /// Units property</strong></para>
        ///   <para>When the Units property is changed, no conversion is performed on the
        /// Value property. This could lead to mathematical errors which are difficult to debug. Use
        /// conversion methods such as ToFeet or ToMeters instead.</para>
        ///   <para>
        /// This example demonstrates poor programming when trying to add 100 feet to 100 meters
        /// by changing the Units property of the Distance2 object.
        ///   <code lang="VB">
        /// ' Declare two distances
        /// Dim Distance1 As New Distance(50, DistanceUnit.Meters)
        /// Dim Distance2 As New Distance(100, DistanceUnit.Feet)
        /// ' Store their sum in another variable
        /// Dim Distance3 As New Distance(0, DistanceUnit.Meters)
        /// ' INCORRECT: Changing Units property does not convert Distance2!
        /// Distance2.Units = DistanceUnit.Meters
        /// Distance3.Value = Distance1.Value + Distance2.Value
        ///   </code>
        ///   <code lang="C#">
        /// // Declare two distances
        /// Distance Distance1 = new Distance(50, DistanceUnit.Meters);
        /// Distance Distance2 = new Distance(100, DistanceUnit.Feet);
        /// // Store their sum in another variable
        /// Distance Distance3 = new Distance(0, DistanceUnit.Meters);
        /// // INCORRECT: Changing Units property does not convert Distance2!
        /// Distance2.Units = DistanceUnit.Meters;
        /// Distance3.Value = Distance1.Value + Distance2.Value;
        ///   </code>
        /// The correct technique is to use a conversion method to change the unit type instead
        /// of modifying the Units property.
        ///   <code lang="VB">
        /// ' Declare two distances
        /// Dim Distance1 As New Distance(50, DistanceUnit.Meters)
        /// Dim Distance2 As New Distance(100, DistanceUnit.Feet)
        /// ' Store their sum in another variable
        /// Dim Distance3 As New Distance(0, DistanceUnit.Meters)
        /// ' CORRECT: The ToMeters method is used to standardize unit types
        /// Distance3.Value = Distance1.ToMeters().Value + Distance2.ToMeters().Value
        ///   </code>
        ///   <code lang="C#">
        /// // Declare two distances
        /// Distance Distance1 = new Distance(50, DistanceUnit.Meters);
        /// Distance Distance2 = new Distance(100, DistanceUnit.Feet);
        /// // Store their sum in another variable
        /// Distance Distance3 = new Distance(0, DistanceUnit.Meters);
        /// // CORRECT: The ToMeters method is used to standardize unit types
        /// Distance3.Value = Distance1.ToMeters().Value + Distance2.ToMeters().Value;
        ///   </code>
        ///   </para></remarks>
        public DistanceUnit Units { get; private set; }

        /// <summary>
        /// Returns whether the value is invalid or unspecified.
        /// </summary>
        public bool IsInvalid => double.IsNaN(Value);

        /// <summary>
        /// Returns whether the value is zero.
        /// </summary>
        public bool IsEmpty => Value == 0;

        /// <summary>
        /// Returns whether the unit of measurement is metric.
        /// </summary>
        public bool IsMetric
        {
            get => Units is DistanceUnit.Centimeters
                    or DistanceUnit.Meters
                    or DistanceUnit.Kilometers;
        }

        /// <summary>
        /// Returns whether the value is infinite.
        /// </summary>
        public bool IsInfinity => double.IsInfinity(Value);

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the time required to travel the entire distance at the specified speed.
        /// </summary>
        /// <param name="speed">A <strong>Speed</strong> object representing a travel speed.</param>
        /// <returns>A <strong>TimeSpan</strong> object representing the total time required to travel the entire distance.</returns>
        public TimeSpan GetMinimumTravelTime(Speed speed)
        {
            // Dim AdjustedDestination As Position = destination.ToDatum(Datum)
            double travelDistance = ToMeters().Value;
            double travelSpeed = speed.ToMetersPerSecond().Value;
            // Perform the calculation
            return new TimeSpan((long)(travelDistance / travelSpeed * TimeSpan.TicksPerSecond));
        }

        /// <summary>
        /// Returns the speed required to travel the entire distance in the specified time.
        /// </summary>
        /// <param name="time">A <strong>TimeSpan</strong> object representing the time to travel the entire distance.</param>
        /// <returns>A <strong>Speed</strong> object representing the speed required to travel the distance in exactly the time specified.</returns>
        public Speed GetMinimumTravelSpeed(TimeSpan time)
        {
            double travelDistance = ToMeters().Value;
            // Perform the calculation
            return new Speed(travelDistance / time.TotalSeconds, SpeedUnit.MetersPerSecond);
        }

        /// <summary>
        /// Converts the current measurement into feet.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        ///
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <example>
        /// This example converts various distances into feet.  notice that the ToFeet method converts distances
        /// from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Inches)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Kilometers)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToFeet.ToString)
        /// Debug.WriteLine(Distance2.ToFeet.ToString)
        /// Debug.WriteLine(Distance3.ToFeet.ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Inches);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Kilometers);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToFeet().ToString());
        /// Debug.WriteLine(Distance2.ToFeet().ToString());
        /// Debug.WriteLine(Distance3.ToFeet().ToString());
        ///   </code>
        ///   </example>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToFeet()
        {
            return Units switch
            {
                DistanceUnit.Meters => new Distance(Value * FEET_PER_METER, DistanceUnit.Feet),
                DistanceUnit.Centimeters => new Distance(Value * FEET_PER_CENTIMETER, DistanceUnit.Feet),
                DistanceUnit.Feet => this,
                DistanceUnit.Inches => new Distance(Value * FEET_PER_INCH, DistanceUnit.Feet),
                DistanceUnit.Kilometers => new Distance(Value * FEET_PER_KILOMETER, DistanceUnit.Feet),
                DistanceUnit.StatuteMiles => new Distance(Value * FEET_PER_STATUTE_MILE, DistanceUnit.Feet),
                DistanceUnit.NauticalMiles => new Distance(Value * FEET_PER_NAUTICAL_MILE, DistanceUnit.Feet),
                DistanceUnit.Fathom => new Distance(Value * FEET_PER_FATHOM, DistanceUnit.Feet),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into inches.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into inches.  notice that the ToInches method converts distances
        /// from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Kilometers)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToInches.ToString)
        /// Debug.WriteLine(Distance2.ToInches.ToString)
        /// Debug.WriteLine(Distance3.ToInches.ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Kilometers);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToInches().ToString());
        /// Debug.WriteLine(Distance2.ToInches().ToString());
        /// Debug.WriteLine(Distance3.ToInches().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToInches()
        {
            return Units switch
            {
                DistanceUnit.Meters => new Distance(Value * INCHES_PER_METER, DistanceUnit.Inches),
                DistanceUnit.Centimeters => new Distance(Value * INCHES_PER_CENTIMETER, DistanceUnit.Inches),
                DistanceUnit.Feet => new Distance(Value * INCHES_PER_FOOT, DistanceUnit.Inches),
                DistanceUnit.Inches => this,
                DistanceUnit.Kilometers => new Distance(Value * INCHES_PER_KILOMETER, DistanceUnit.Inches),
                DistanceUnit.StatuteMiles => new Distance(Value * INCHES_PER_STATUTE_MILE, DistanceUnit.Inches),
                DistanceUnit.NauticalMiles => new Distance(Value * INCHES_PER_NAUTICAL_MILE, DistanceUnit.Inches),
                DistanceUnit.Fathom => new Distance(Value * INCHES_PER_FATHOM, DistanceUnit.Inches),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into kilometers.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into kilometers. notice that the ToKilometers method converts
        /// distances from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToKilometers.ToString)
        /// Debug.WriteLine(Distance2.ToKilometers.ToString)
        /// Debug.WriteLine(Distance3.ToKilometers.ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToKilometers().ToString());
        /// Debug.WriteLine(Distance2.ToKilometers().ToString());
        /// Debug.WriteLine(Distance3.ToKilometers().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToKilometers()
        {
            return Units switch
            {
                DistanceUnit.Meters => new Distance(Value * KILOMETERS_PER_METER, DistanceUnit.Kilometers),
                DistanceUnit.Centimeters => new Distance(Value * KILOMETERS_PER_CENTIMETER, DistanceUnit.Kilometers),
                DistanceUnit.Feet => new Distance(Value * KILOMETERS_PER_FOOT, DistanceUnit.Kilometers),
                DistanceUnit.Inches => new Distance(Value * KILOMETERS_PER_INCH, DistanceUnit.Kilometers),
                DistanceUnit.Kilometers => this,
                DistanceUnit.StatuteMiles => new Distance(Value * KILOMETERS_PER_STATUTE_MILE, DistanceUnit.Kilometers),
                DistanceUnit.NauticalMiles => new Distance(Value * KILOMETERS_PER_NAUTICAL_MILE, DistanceUnit.Kilometers),
                DistanceUnit.Fathom => new Distance(Value * KILOMETERS_PER_FATHOM, DistanceUnit.Kilometers),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into meters.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into meters.  notice that the ToMeters method converts distances
        /// from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToMeters().ToString)
        /// Debug.WriteLine(Distance2.ToMeters().ToString)
        /// Debug.WriteLine(Distance3.ToMeters().ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToMeters().ToString());
        /// Debug.WriteLine(Distance2.ToMeters().ToString());
        /// Debug.WriteLine(Distance3.ToMeters().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToMeters()
        {
            return Units switch
            {
                DistanceUnit.Meters => this,
                DistanceUnit.Centimeters => new Distance(Value * METERS_PER_CENTIMETER, DistanceUnit.Meters),
                DistanceUnit.Feet => new Distance(Value * METERS_PER_FOOT, DistanceUnit.Meters),
                DistanceUnit.Inches => new Distance(Value * METERS_PER_INCH, DistanceUnit.Meters),
                DistanceUnit.Kilometers => new Distance(Value * METERS_PER_KILOMETER, DistanceUnit.Meters),
                DistanceUnit.StatuteMiles => new Distance(Value * METERS_PER_STATUTE_MILE, DistanceUnit.Meters),
                DistanceUnit.NauticalMiles => new Distance(Value * METERS_PER_NAUTICAL_MILE, DistanceUnit.Meters),
                DistanceUnit.Fathom => new Distance(Value * METERS_PER_FATHOM, DistanceUnit.Meters),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into meters.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into meters.  notice that the ToMeters method converts distances
        /// from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToMeters().ToString)
        /// Debug.WriteLine(Distance2.ToMeters().ToString)
        /// Debug.WriteLine(Distance3.ToMeters().ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToMeters().ToString());
        /// Debug.WriteLine(Distance2.ToMeters().ToString());
        /// Debug.WriteLine(Distance3.ToMeters().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToCentimeters()
        {
            return Units switch
            {
                DistanceUnit.Centimeters => this,
                DistanceUnit.Meters => new Distance(Value * CENTIMETERS_PER_METER, DistanceUnit.Centimeters),
                DistanceUnit.Feet => new Distance(Value * CENTIMETERS_PER_FOOT, DistanceUnit.Centimeters),
                DistanceUnit.Inches => new Distance(Value * CENTIMETERS_PER_INCH, DistanceUnit.Centimeters),
                DistanceUnit.Kilometers => new Distance(Value * CENTIMETERS_PER_KILOMETER, DistanceUnit.Centimeters),
                DistanceUnit.StatuteMiles => new Distance(Value * CENTIMETERS_PER_STATUTE_MILE, DistanceUnit.Centimeters),
                DistanceUnit.NauticalMiles => new Distance(Value * CENTIMETERS_PER_NAUTICAL_MILE, DistanceUnit.Centimeters),
                DistanceUnit.Fathom => new Distance(Value * CENTIMETERS_PER_FATHOM, DistanceUnit.Centimeters),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into nautical miles.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into nautical miles. notice that the ToNauticalMiles method
        /// converts distances from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToNauticalMiles.ToString)
        /// Debug.WriteLine(Distance2.ToNauticalMiles.ToString)
        /// Debug.WriteLine(Distance3.ToNauticalMiles.ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToNauticalMiles().ToString());
        /// Debug.WriteLine(Distance2.ToNauticalMiles().ToString());
        /// Debug.WriteLine(Distance3.ToNauticalMiles().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToNauticalMiles()
        {
            return Units switch
            {
                DistanceUnit.Meters => new Distance(Value * NAUTICAL_MILES_PER_METER, DistanceUnit.NauticalMiles),
                DistanceUnit.Centimeters => new Distance(Value * NAUTICAL_MILES_PER_CENTIMETER, DistanceUnit.NauticalMiles),
                DistanceUnit.Feet => new Distance(Value * NAUTICAL_MILES_PER_FOOT, DistanceUnit.NauticalMiles),
                DistanceUnit.Inches => new Distance(Value * NAUTICAL_MILES_PER_INCH, DistanceUnit.NauticalMiles),
                DistanceUnit.Kilometers => new Distance(Value * NAUTICAL_MILES_PER_KILOMETER, DistanceUnit.NauticalMiles),
                DistanceUnit.StatuteMiles => new Distance(Value * NAUTICAL_MILES_PER_STATUTE_MILE, DistanceUnit.NauticalMiles),
                DistanceUnit.NauticalMiles => this,
                DistanceUnit.Fathom => new Distance(Value * NAUTICAL_MILES_PER_FATHOM, DistanceUnit.NauticalMiles),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into miles.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various distances into statute miles.  notice that the ToStatuteMiles method
        /// converts distances from any source type.
        ///   <code lang="VB">
        /// ' Create distances of different unit types
        /// Dim Distance1 As New Distance(10, DistanceUnit.Feet)
        /// Dim Distance2 As New Distance(20, DistanceUnit.StatuteMiles)
        /// Dim Distance3 As New Distance(50, DistanceUnit.Inches)
        /// ' Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToStatuteMiles.ToString)
        /// Debug.WriteLine(Distance2.ToStatuteMiles.ToString)
        /// Debug.WriteLine(Distance3.ToStatuteMiles.ToString)
        ///   </code>
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to feet and output the result
        /// Debug.WriteLine(Distance1.ToStatuteMiles().ToString());
        /// Debug.WriteLine(Distance2.ToStatuteMiles().ToString());
        /// Debug.WriteLine(Distance3.ToStatuteMiles().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToMeters">ToMeters Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        /// 
        /// <seealso cref="ToFathom">ToFathom Method</seealso>
        /// <remarks>This method will perform a conversion into feet regardless of the current unit
        /// type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToStatuteMiles()
        {
            return Units switch
            {
                DistanceUnit.Meters => new Distance(Value * STATUTE_MILES_PER_METER, DistanceUnit.StatuteMiles),
                DistanceUnit.Centimeters => new Distance(Value * STATUTE_MILES_PER_CENTIMETER, DistanceUnit.StatuteMiles),
                DistanceUnit.Feet => new Distance(Value * STATUTE_MILES_PER_FOOT, DistanceUnit.StatuteMiles),
                DistanceUnit.Inches => new Distance(Value * STATUTE_MILES_PER_INCH, DistanceUnit.StatuteMiles),
                DistanceUnit.Kilometers => new Distance(Value * STATUTE_MILES_PER_KILOMETER, DistanceUnit.StatuteMiles),
                DistanceUnit.StatuteMiles => this,
                DistanceUnit.NauticalMiles => new Distance(Value * STATUTE_MILES_PER_NAUTICAL_MILE, DistanceUnit.StatuteMiles),
                DistanceUnit.Fathom => new Distance(Value * STATUTE_MILES_PER_FATHOM, DistanceUnit.StatuteMiles),
                _ => Empty,
            };
        }

        /// <summary>
        /// Converts the current measurement into fathoms.
        /// </summary>
        /// <returns>A new <strong>Distance</strong> object containing the converted value.</returns>
        /// <example>
        /// This example converts various distances into fathoms. Notice that the ToFathom method converts distances from any source type.
        ///   <code lang="C#">
        /// // Create distances of different unit types
        /// Distance Distance1 = new Distance(10, DistanceUnit.Feet);
        /// Distance Distance2 = new Distance(20, DistanceUnit.StatuteMiles);
        /// Distance Distance3 = new Distance(50, DistanceUnit.Inches);
        /// // Convert the distance measurements to fathom and output the result
        /// Debug.WriteLine(Distance1.ToFathom().ToString());
        /// Debug.WriteLine(Distance2.ToFathom().ToString());
        /// Debug.WriteLine(Distance3.ToFathom().ToString());
        ///   </code>
        ///   </example>
        /// <seealso cref="ToFathom">ToCentimeters Method</seealso>
        /// 
        /// <seealso cref="ToFeet">ToFeet Method</seealso>
        ///
        /// <seealso cref="ToInches">ToInches Method</seealso>
        ///
        /// <seealso cref="ToKilometers">ToKilometers Method</seealso>
        ///
        /// <seealso cref="ToNauticalMiles">ToNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToStatuteMiles">ToStatuteMiles Method</seealso>
        /// <remarks>This method will perform a conversion into fathoms regardless of the current unit type. You may convert from any unit type to any unit type.</remarks>
        public Distance ToFathom()
        {
            return Units switch
            {
                DistanceUnit.Centimeters => new Distance(Value * FATHOMS_PER_CENTIMETER, DistanceUnit.Fathom),
                DistanceUnit.Meters => new Distance(Value * FATHOMS_PER_METER, DistanceUnit.Fathom),
                DistanceUnit.Feet => new Distance(Value * FATHOMS_PER_FOOT, DistanceUnit.Fathom),
                DistanceUnit.Inches => new Distance(Value * FATHOMS_PER_INCH, DistanceUnit.Fathom),
                DistanceUnit.Kilometers => new Distance(Value * FATHOMS_PER_KILOMETER, DistanceUnit.Fathom),
                DistanceUnit.StatuteMiles => new Distance(Value * FATHOMS_PER_STATUTE_MILE, DistanceUnit.Fathom),
                DistanceUnit.NauticalMiles => new Distance(Value * FATHOMS_PER_NAUTICAL_MILE, DistanceUnit.Fathom),
                DistanceUnit.Fathom => this,
                _ => Empty,
            };
        }

        /// <summary>
        /// Toes the type of the unit.
        /// </summary>
        /// <param name="newUnits">The new units.</param>
        /// <returns></returns>
        public Distance ToUnitType(DistanceUnit newUnits)
        {
            return newUnits switch
            {
                DistanceUnit.Centimeters => ToCentimeters(),
                DistanceUnit.Feet => ToFeet(),
                DistanceUnit.Inches => ToInches(),
                DistanceUnit.Kilometers => ToKilometers(),
                DistanceUnit.Meters => ToMeters(),
                DistanceUnit.NauticalMiles => ToNauticalMiles(),
                DistanceUnit.StatuteMiles => ToStatuteMiles(),
                DistanceUnit.Fathom => ToFathom(),
                _ => Empty,
            };
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter. This method will determine the smallest Imperial unit type.</remarks>
        public Distance ToImperialUnitType()
        {
            // Start with the largest possible unit
            Distance temp = ToStatuteMiles();
            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
            {
                temp = temp.ToFeet();
            }

            if (Math.Abs(temp.Value) < 1.0)
            {
                temp = temp.ToInches();
            }

            if (Math.Abs(temp.Value) < 1.0)
            {
                temp = temp.ToCentimeters();
            }

            return temp;
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter. This method will determine the smallest metric unit type.</remarks>
        public Distance ToMetricUnitType()
        {
            // Yes. Start with the largest possible unit
            Distance temp = ToKilometers();

            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
            {
                temp = temp.ToMeters();
            }

            // And so on until we find the right unit
            if (Math.Abs(temp.Value) < 1.0)
            {
                temp = temp.ToCentimeters();
            }

            return temp;
        }

        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will find the smallest unit type and convert the unit to the user's local
        /// numeric system (Imperial or Metric).</remarks>
        public Distance ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
            {
                return ToMetricUnitType();
            }

            return ToImperialUnitType();
        }

        /// <summary>
        /// Returns the distance traveled at the current speed for the specified time.
        /// </summary>
        /// <param name="time">A length of time to travel.</param>
        /// <returns>A <strong>Distance</strong> representing the distance travelled at the current speed for the specified length of time.</returns>
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
            return new Distance(Math.Round(Value, decimals), Units);
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <param name="format"><para>A combination of symbols, spaces, and any of the following case-insensitive
        /// letters: <strong>#</strong> or <strong>0</strong> for the value property, and <strong>U</strong> for
        /// distance units. Here are some examples:</para>
        ///   <para>
        ///   <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///   <tr>
        ///   <td>##0.## uu</td>
        ///   <td>## uuuu</td>
        ///   <td># u</td>
        ///   <td>###</td>
        ///   </tr>
        ///   </table>
        ///   </para></param>
        /// <returns>A <strong>String</strong> containing the distance in the specified format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement using a custom format.
        ///   <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu")
        ///   </code>
        ///   <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu");
        ///   </code>
        ///   </example>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #region Math Methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Add(Distance value)
        {
            return new Distance(Value + value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Add(double value)
        {
            return new Distance(Value + value, Units);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Subtract(Distance value)
        {
            return new Distance(Value - value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Subtract(double value)
        {
            return new Distance(Value - value, Units);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Multiply(Distance value)
        {
            return new Distance(Value * value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Multiply(double value)
        {
            return new Distance(Value * value, Units);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Divide(Distance value)
        {
            return new Distance(Value / value.ToUnitType(Units).Value, Units);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Distance Divide(double value)
        {
            return new Distance(Value / value, Units);
        }

        /// <summary>
        /// Increments this instance.
        /// </summary>
        /// <returns></returns>
        public Distance Increment()
        {
            return new Distance(Value + 1.0, Units);
        }

        /// <summary>
        /// Decrements this instance.
        /// </summary>
        /// <returns></returns>
        public Distance Decrement()
        {
            return new Distance(Value - 1.0, Units);
        }

        /// <summary>
        /// Determines whether [is less than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThan(Distance value)
        {
            return CompareTo(value) < 0;
        }

        /// <summary>
        /// Determines whether [is less than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThanOrEqualTo(Distance value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        /// <summary>
        /// Determines whether [is greater than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThan(Distance value)
        {
            return CompareTo(value) > 0;
        }

        /// <summary>
        /// Determines whether [is greater than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThanOrEqualTo(Distance value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion Math Methods

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current instance to the specified object.
        /// </summary>
        /// <param name="obj">An <strong>Object</strong> to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Distance distance)
            {
                // If the type is the same, compare the values
                return Equals(distance);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return ToMeters().Value.GetHashCode();
        }

        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the current distance in the default format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement.
        ///   <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString
        ///   </code>
        ///   <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString();
        ///   </code>
        ///   </example>
        /// <remarks>The default format used is "##0.## uu" where <strong>uu</strong> is the distance unit type.
        /// The numeric format may vary depending on the machine's local culture.</remarks>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Returns a random distance between 0 and 1, 000 meters.
        /// </summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Distance Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random distance between 0 and 1, 000 meters.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Distance Random(Random generator)
        {
            return new Distance(generator.NextDouble() * 1000, DistanceUnit.Meters).ToLocalUnitType();
        }

        /// <summary>
        /// Returns a random distance between zero and the specified maximum.
        /// </summary>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        public static Distance Random(Distance maximum)
        {
            return new Distance(new Random(DateTime.Now.Millisecond).NextDouble() * maximum.Value, maximum.Units);
        }

        /// <summary>
        /// Froms the centimeters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromCentimeters(double value)
        {
            return new Distance(value, DistanceUnit.Centimeters);
        }

        /// <summary>
        /// Froms the fathoms.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromFathom(double value)
        {
            return new Distance(value, DistanceUnit.Fathom);
        }

        /// <summary>
        /// Froms the feet.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromFeet(double value)
        {
            return new Distance(value, DistanceUnit.Feet);
        }

        /// <summary>
        /// Froms the inches.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromInches(double value)
        {
            return new Distance(value, DistanceUnit.Inches);
        }

        /// <summary>
        /// Froms the kilometers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromKilometers(double value)
        {
            return new Distance(value, DistanceUnit.Kilometers);
        }

        /// <summary>
        /// Froms the meters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromMeters(double value)
        {
            return new Distance(value, DistanceUnit.Meters);
        }

        /// <summary>
        /// Froms the nautical miles.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromNauticalMiles(double value)
        {
            return new Distance(value, DistanceUnit.NauticalMiles);
        }

        /// <summary>
        /// Froms the statute miles.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromStatuteMiles(double value)
        {
            return new Distance(value, DistanceUnit.StatuteMiles);
        }

        /// <summary>
        /// Froms the centimeters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromCentimeters(int value)
        {
            return new Distance(value, DistanceUnit.Centimeters);
        }

        /// <summary>
        /// Froms the fathoms.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromFathom(int value)
        {
            return new Distance(value, DistanceUnit.Fathom);
        }

        /// <summary>
        /// Froms the feet.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromFeet(int value)
        {
            return new Distance(value, DistanceUnit.Feet);
        }

        /// <summary>
        /// Froms the inches.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromInches(int value)
        {
            return new Distance(value, DistanceUnit.Inches);
        }

        /// <summary>
        /// Froms the kilometers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromKilometers(int value)
        {
            return new Distance(value, DistanceUnit.Kilometers);
        }

        /// <summary>
        /// Froms the meters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromMeters(int value)
        {
            return new Distance(value, DistanceUnit.Meters);
        }

        /// <summary>
        /// Froms the nautical miles.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromNauticalMiles(int value)
        {
            return new Distance(value, DistanceUnit.NauticalMiles);
        }

        /// <summary>
        /// Froms the statute miles.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromStatuteMiles(int value)
        {
            return new Distance(value, DistanceUnit.StatuteMiles);
        }

        /// <summary>
        /// Parses the distance unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DistanceUnit ParseDistanceUnit(string value)
        {
            return (DistanceUnit)Enum.Parse(typeof(DistanceUnit), value, true);
        }

        /// <summary>
        /// Converts a string-based distance measurement into a Distance object.
        /// </summary>
        /// <param name="value"><para>A <strong>String</strong> describing a case-insensitive distance measurement,
        /// in any of the following formats, where <strong>N</strong> represents a numeric
        /// value:</para>
        ///   <list type="bullet">
        ///   <item>N m</item>
        ///   <item>N meters</item>
        ///   <item>N meter</item>
        ///   <item>N metre</item>
        ///   <item>N metres</item>
        ///   <item>N km</item>
        ///   <item>N kilometers</item>
        ///   <item>N kilometer</item>
        ///   <item>N kilometre</item>
        ///   <item>N kilometres</item>
        ///   <item>N ft</item>
        ///   <item>N'</item>
        ///   <item>N foot</item>
        ///   <item>N feet</item>
        ///   <item>N in</item>
        ///   <item>N"</item>
        ///   <item>N inch</item>
        ///   <item>N inches</item>
        ///   <item>N mi</item>
        ///   <item>N mile</item>
        ///   <item>N miles</item>
        ///   <item>N nm</item>
        ///   <item>N nautical mile</item>
        ///   <item>N nautical miles</item>
        ///   </list></param>
        /// <returns>A new Distance object containing the parsed <see cref="Value">value</see> and
        /// <see cref="Units">unit</see> type.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid distance measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the distance measurement was not recognized.<br/>
        /// 2. The distance unit type was not recognized or not specified.</exception>
        ///
        /// <example>
        /// This example demonstrates how the Parse method can convert several string formats into a Distance object.
        ///   <code lang="VB">
        /// Dim NewDistance As Distance
        /// ' Create a distance of 50 kilometers
        /// NewDistance = Distance.Parse("50 km")
        /// ' Create a distance of 14, 387 miles, then convert it into inches
        /// NewDistance = Distance.Parse("14, 387 statute miles").ToInches
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewDistance = Distance.Parse("	50 '	   ")
        ///   </code>
        ///   <code lang="C#">
        /// Distance NewDistance;
        /// // Create a distance of 50 kilometers
        /// NewDistance = Distance.Parse("50 km");
        /// // Create a distance of 14, 387 miles, then convert it into inches
        /// NewDistance = Distance.Parse("14, 387 statute miles").ToInches;
        /// // Parse an untrimmed measurement into 50 feet
        /// NewDistance = Distance.Parse("	50 '	   ");
        ///   </code>
        ///   </example>
        /// <remarks>This powerful constructor is typically used to convert a string-based distance
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Distance</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.</remarks>
        public static Distance Parse(string value)
        {
            return new Distance(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Distance Parse(string value, CultureInfo culture)
        {
            return new Distance(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Distance operator +(Distance left, Distance right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Distance operator -(Distance left, Distance right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Distance operator *(Distance left, Distance right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Distance operator /(Distance left, Distance right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Distance left, Distance right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Distance left, Distance right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Distance left, Distance right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Distance left, Distance right)
        {
            return !(left.Equals(right));
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Distance left, Distance right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Distance left, Distance right)
        {
            return left.CompareTo(right) > 0;
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="DotSpatial.Positioning.Distance"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Distance(string value)
        {
            return new Distance(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Distance"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(Distance value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IEquatable<Distance> Members

        /// <summary>
        /// Compares the current instance to the specified distance object.
        /// </summary>
        /// <param name="other">A <strong>Distance</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks>This method compares the current instance to the specified object up to four digits of precision.</remarks>
        public bool Equals(Distance other)
        {
            return Value.Equals(other.ToUnitType(Units).Value);
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
            return Math.Round(Value, decimals) == Math.Round(other.ToUnitType(Units).Value, decimals);
        }

        #endregion IEquatable<Distance> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and local culture.
        /// </summary>
        /// <param name="format"><para>A combination of symbols, spaces, and any of the following case-insensitive
        /// letters: <strong>#</strong> or <strong>0</strong> for the value property, and <strong>U</strong> for
        /// distance units. Here are some examples:</para>
        ///   <para>
        ///   <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///   <tr>
        ///   <td>##0.## uu</td>
        ///   <td>## uuuu</td>
        ///   <td># u</td>
        ///   <td>###</td>
        ///   </tr>
        ///   </table>
        ///   </para></param>
        /// <param name="formatProvider">Information about the culture to apply to the numeric format.</param>
        /// <returns>A <strong>String</strong> containing the distance in the specified format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a distance measurement using a custom format and culture information.
        ///   <code lang="VB">
        /// ' Declare a distance of 75 miles
        /// Dim MyDistance As New Distance(75, DistanceUnit.StatuteMiles)
        /// ' Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu", CultureInfo.CurrentUICulture)
        ///   </code>
        ///   <code lang="C#">
        /// // Declare a distance of 75 miles
        /// Distance MyDistance = new Distance(75, DistanceUnit.StatuteMiles);
        /// // Set the text box to the distance, formatted as a string
        /// MyTextBox.Text = MyDistance.ToString("## uuu", CultureInfo.CurrentUICulture);
        ///   </code>
        ///   </example>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            try
            {
                // Use the default if "g" is passed
                if (string.Compare(format, "g", true, culture) == 0)
                {
                    format = "#" + culture.NumberFormat.NumberGroupSeparator + "##0.00 uu";
                }

                // Convert the format to uppercase
                format = format.ToUpper(CultureInfo.InvariantCulture);

                // Convert the localized format string to a US format
                format = format.Replace("V", "0");

                // Replace the "d" with "h" since degrees is the same as hours
                format = Value.ToString(format, culture);

                // Is there a units specifier?
                int startChar = format.IndexOf("U");
                if (startChar > -1)
                {
                    // Yes. Look for subsequent U characters or a period
                    int endChar = format.LastIndexOf("U");
                    // Extract the sub-string
                    string subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Show the unit based on the length
                    switch (subFormat.Length)
                    {
                        case 1:
                            switch (Units)
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
                                case DistanceUnit.Fathom:
                                    format = format.Replace("U", "fm");
                                    break;
                            }

                            break;
                        case 2:
                            switch (Units)
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
                                case DistanceUnit.Fathom:
                                    format = format.Replace("UU", "fm");
                                    break;
                            }

                            break;
                        case 3:
                            if (Value == 1)
                            {
                                switch (Units)
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
                                    case DistanceUnit.Fathom:
                                        format = format.Replace("UUU", "fathom");
                                        break;
                                }
                            }
                            else
                            {
                                switch (Units)
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
                                    case DistanceUnit.Fathom:
                                        format = format.Replace("UUU", "fathom");
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
                throw new FormatException(Resources.Distance_InvalidFormat, ex);
            }
        }

        #endregion IFormattable Members

        #region IComparable<Distance> Members

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
        public int CompareTo(Distance other)
        {
            return Value.CompareTo(other.ToUnitType(Units).Value);
        }

        #endregion IComparable<Distance> Members

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
            writer.WriteElementString("Units", Units.ToString());
            writer.WriteElementString("Value", Value.ToString("G17", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // Move to the <Units> element
            if (!reader.IsStartElement("Units"))
            {
                reader.ReadToDescendant("Units");
            }

            Units = (DistanceUnit)Enum.Parse(typeof(DistanceUnit), reader.ReadElementContentAsString(), true);
            Value = reader.ReadElementContentAsDouble();
            reader.Read();
        }

        #endregion IXmlSerializable Members
    }

    /// <summary>
    /// Indicates the unit of measure for distance measurements.
    /// </summary>
    /// <seealso cref="Distance.Value">Value Property (Distance Class)</seealso>
    ///
    /// <seealso cref="Distance.Units">Units Property (Distance Class)</seealso>
    /// <remarks>This enumeration is most frequently used by the <see cref="Distance.Units">Units</see> property of the
    /// <see cref="Distance">Distance</see> class in conjunction with the <see cref="Distance.Value">Value</see>
    /// property to describe a straight-line distance.</remarks>
    public enum DistanceUnit
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
        Inches,
        /// <summary>Fathom</summary>
        Fathom
    }
}