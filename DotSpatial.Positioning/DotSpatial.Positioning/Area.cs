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
using System.Globalization;
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
    /// Represents the measurement of surface area of a polygon on Earth's
    /// surface.
    /// </summary>
    /// <example>
    /// This example demonstrates how to create an <strong>Area</strong> structure and
    /// convert it to another unit type.
    ///   <code lang="VB">
    /// ' Declare a Area of 50 meters
    /// Dim Area1 As New Area(50, AreaUnit.SquareMeters)
    /// ' Convert it into acres
    /// Dim Area2 As Area = Area2.ToAcres()
    ///   </code>
    ///   <code lang="CS">
    /// // Declare a Area of 50 meters
    /// Area Area1 = new Area(50, AreaUnit.SquareMeters);
    /// // Convert it into acres
    /// Area Area2 = Area2.ToAcres();
    ///   </code>
    ///   </example>
    /// <remarks><para>This structure is used to represent measurements of arbitrary polygons on
    /// Earth's surface. Measurements can be converted to different unit types, such as
    /// acres, square kilometers, and square miles.</para>
    ///   <para>Instances of this structure are guaranteed to be thread-safe because they are
    /// immutable (properties can only be modified via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.AreaConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Area : IFormattable, IComparable<Area>, IEquatable<Area>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _value;
        /// <summary>
        ///
        /// </summary>
        private AreaUnit _units;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_METER = 0.0002471054;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_CENTIMETER = 2.471054e-8;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_STATUTE_MILE = 640;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_KILOMETER = 247.1054;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_INCH = 1.594225e-7;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_NAUTICAL_MILE = 847.547736;
        /// <summary>
        ///
        /// </summary>
        private const double ACRES_PER_SQUARE_FOOT = 2.29568411e-5;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_METER = 10.76391;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_CENTIMETER = 0.001076391;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_STATUTE_MILE = 27878400;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_KILOMETER = 10763910.4;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_INCH = 0.00694444444;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_SQUARE_NAUTICAL_MILE = 36919179.4;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_FEET_PER_ACRE = 43560;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_METER = 1550.003;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_CENTIMETER = 0.1550003;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_STATUTE_MILE = 4014489600;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_KILOMETER = 1.5500031e09;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_FOOT = 144;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_SQUARE_NAUTICAL_MILE = 5.31636183e9;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_INCHES_PER_ACRE = 6272640;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_METER = 3.861022e-7;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_CENTIMETER = 3.861022e-11;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_KILOMETER = 0.3861022;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_INCH = 2.490977e-10;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_FOOT = 3.58700643e-8;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_SQUARE_NAUTICAL_MILE = 1.32429334;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_STATUTE_MILES_PER_ACRE = 0.0015625;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_METER = 2.9155335e-07;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_CENTIMETER = 2.9155335e-11;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_KILOMETER = 0.29155335;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_INCH = 1.88098559e-10;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_FOOT = 2.70861925e-8;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_SQUARE_STATUTE_MILE = 0.755119709;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_NAUTICAL_MILES_PER_ACRE = 0.00117987455;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_STATUTE_MILE = 2.58998811e10;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_KILOMETER = 10000000000;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_FOOT = 929.0304;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_INCH = 6.4516;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_METER = 10000;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_SQUARE_NAUTICAL_MILE = 34299040000;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_CENTIMETERS_PER_ACRE = 40468564.2;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_STATUTE_MILE = 2589988.11;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_CENTIMETER = 0.0001;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_KILOMETER = 1000000;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_FOOT = 0.09290304;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_INCH = 0.00064516;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_ACRE = 4046.85642;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_METERS_PER_SQUARE_NAUTICAL_MILE = 3429904;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_METER = 0.000001;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_CENTIMETER = 1e-10;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_STATUTE_MILE = 2.589988;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_FOOT = 9.290304e-8;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_INCH = 6.4516e-10;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_SQUARE_NAUTICAL_MILE = 3.429904;
        /// <summary>
        ///
        /// </summary>
        private const double SQUARE_KILOMETERS_PER_ACRE = 0.004046856;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Represents an area with no value.
        /// </summary>
        public static readonly Area Empty = new Area(0.0, AreaUnit.SquareMeters).ToLocalUnitType();

        /// <summary>
        /// Represents an area of infinite value.
        /// </summary>
        public static readonly Area Infinity = new Area(double.PositiveInfinity, AreaUnit.SquareMeters).ToLocalUnitType();

        /// <summary>
        /// Represents the largest possible area which can be stored.
        /// </summary>
        public static readonly Area Maximum = new Area(double.MaxValue, AreaUnit.SquareKilometers).ToLocalUnitType();

        /// <summary>
        /// Represents the smallest possible area which can be stored.
        /// </summary>
        public static readonly Area Minimum = new Area(double.MinValue, AreaUnit.SquareKilometers).ToLocalUnitType();

        /// <summary>
        /// Represents an invalid or unspecified area.
        /// </summary>
        public static readonly Area Invalid = new Area(double.NaN, AreaUnit.SquareMeters);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value and unit type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="units">The units.</param>
        /// <example>
        /// This example uses a constructor to create a new <strong>Area</strong> of fifty
        /// square kilometers.
        ///   <code lang="VB">
        /// Dim MyArea As New Area(50, AreaUnit.SquareKilometers)
        ///   </code>
        ///   <code lang="CS">
        /// Area MyArea = new Area(50, AreaUnit.SquareKilometers);
        ///   </code>
        ///   </example>
        public Area(double value, AreaUnit units)
        {
            _value = value;
            _units = units;
        }

        /// <summary>
        /// Creates a new instance using the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
        /// 2. The Area unit type was not recognized or not specified.</exception>
        ///
        /// <example>
        /// This example demonstrates how the to use this constructor.
        ///   <code lang="VB">
        /// Dim MyArea As Area
        /// ' Create a Area of 50 square kilometers
        /// MyArea = New Area("50 sq. km")
        /// ' Create a Area of 14, 387 miles, then convert it into square inches
        /// MyArea = New Area("14, 387 sq. statute miles").ToSquareInches()
        /// ' Create a Area of 50 square feet
        /// MyArea = New Area("    50 sq '       ")
        ///   </code>
        ///   <code lang="CS">
        /// Area MyArea;
        /// ' Create a Area of 50 square kilometers
        /// MyArea = new Area("50 sq. km");
        /// ' Create a Area of 14, 387 miles, then convert it into square inches
        /// MyArea = new Area("14, 387 sq. statute miles").ToSquareInches();
        /// ' Create a Area of 50 square feet
        /// MyArea = new Area("    50 sq '       ");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Area</strong> object.</returns>
        ///
        /// <seealso cref="Parse(System.String)">Parse(string) Method</seealso>
        /// <remarks>This powerful constructor is used to convert an area measurement in the form of a
        /// string into an object, such as one entered by a user or read from a file. This
        /// constructor can accept any output created via the <see cref="ToString()">ToString</see>
        /// method.</remarks>
        public Area(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance using the specified string and culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
        /// 2. The Area unit type was not recognized or not specified.</exception>
        ///
        /// <example>
        /// This example demonstrates how the to use this constructor.
        ///   <code lang="VB">
        /// Dim MyArea As Area
        /// ' Create a Area of 50 square kilometers
        /// MyArea = New Area("50 sq. km", CultureInfo.CurrentCulture)
        /// ' Create a Area of 14, 387 miles, then convert it into square inches
        /// MyArea = New Area("14, 387 sq. statute miles", CultureInfo.CurrentCulture).ToSquareInches()
        /// ' Create a Area of 50 square feet
        /// MyArea = New Area("    50 sq '       ", CultureInfo.CurrentCulture)
        ///   </code>
        ///   <code lang="CS">
        /// Area MyArea;
        /// ' Create a Area of 50 square kilometers
        /// MyArea = new Area("50 sq. km", CultureInfo.CurrentCulture);
        /// ' Create a Area of 14, 387 miles, then convert it into square inches
        /// MyArea = new Area("14, 387 sq. statute miles", CultureInfo.CurrentCulture).ToSquareInches();
        /// ' Create a Area of 50 square feet
        /// MyArea = new Area("    50 sq '       ", CultureInfo.CurrentCulture);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Area</strong> object.</returns>
        ///
        /// <seealso cref="Parse(System.String)">Parse(string) Method</seealso>
        /// <remarks>This powerful constructor is used to convert an area measurement in the form of a
        /// string into an object, such as one entered by a user or read from a file. This
        /// constructor can accept any output created via the <see cref="ToString()">ToString</see>
        /// method.</remarks>
        public Area(string value, CultureInfo culture)
        {
            // Anything to do?
            if (string.IsNullOrEmpty(value))
            {
                _value = 0;
                _units = AreaUnit.SquareCentimeters;
                return;
            }

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            string unit;

            try
            {
                // Convert to uppercase and remove commas
                value = value.Trim();
                if (String.Compare(value, Properties.Resources.Common_Infinity, true, culture) == 0)
                {
                    _value = double.PositiveInfinity;
                    _units = AreaUnit.SquareNauticalMiles;
                    return;
                }
                if (String.Compare(value, Properties.Resources.Common_Empty, true, culture) == 0)
                {
                    _value = 0;
                    _units = AreaUnit.SquareCentimeters;
                    return;
                }

                // Clean up the value
                value = value.ToUpper(culture).Replace(culture.NumberFormat.NumberGroupSeparator, string.Empty);

                // Go until the first non-number character
                int count = 0;
                while (count < value.Length)
                {
                    string digit = value.Substring(count, 1);
                    if (digit == "0" || digit == "1" || digit == "2" || digit == "3"
                        || digit == "4" || digit == "5" || digit == "6" || digit == "7"
                        || digit == "8" || digit == "9"
                        || digit == culture.NumberFormat.NumberGroupSeparator
                        || digit == culture.NumberFormat.NumberDecimalSeparator)
                        // Allow continuation
                        count = count + 1;
                    else
                        // Non-numeric character!
                        break;
                }
                unit = value.Substring(count).Trim();
                // Get the numeric portion
                string numericPortion = count > 0 ? value.Substring(0, count) : "0";
#if PocketPC
                try
                {
                    _Value = double.Parse(NumericPortion, NumberStyles.Any, culture);
                }
                catch
                {
                    throw new ArgumentException(Properties.Resources.Area_InvalidNumericPortion, "value");
                }
#else
                if (!double.TryParse(numericPortion, NumberStyles.Any, culture, out _value))
                    throw new ArgumentException(Properties.Resources.Area_InvalidNumericPortion, "value");
#endif
                // Try to interpret the measurement
                // Remove any notion of "square"
                unit = unit.Replace("SQUARE", "S").Replace("SQ.", "S").Replace("SQ", "S").Replace(" ", string.Empty).Trim();

                switch (unit)
                {
                    case "A":
                    case "AC":
                    case "ACRE":
                    case "ACRES":
                        _units = AreaUnit.Acres;
                        break;
                    case "SCM":
                    case "SCM.":
                    case "SCENTIMETER":
                    case "SCENTIMETERS":
                    case "SCENTIMETRE":
                    case "SCENTIMETRES":
                        _units = AreaUnit.SquareCentimeters;
                        break;
                    case "SM":
                    case "SM.":
                    case "SMETERS":
                    case "SMETRES":
                    case "SMETRE":
                    case "SMETER":
                        _units = AreaUnit.SquareMeters;
                        break;
                    case "SKM":
                    case "SKM.":
                    case "SKILOMETRES":
                    case "SKILOMETERS":
                    case "SKILOMETRE":
                    case "SKILOMETER":
                        _units = AreaUnit.SquareKilometers;
                        break;
                    case "SMI":
                    case "SMI.":
                    case "SMILE":
                    case "SMILES":
                    case "SSTATUTEMILES":
                        _units = AreaUnit.SquareStatuteMiles;
                        break;
                    case "SNM":
                    case "SNM.":
                    case "SNAUTICALMILE":
                    case "SNAUTICALMILES":
                        _units = AreaUnit.SquareNauticalMiles;
                        break;
                    case "SIN":
                    case "SIN.":
                    case "S\"":
                    case "SINCHES":
                    case "SINCH":
                        _units = AreaUnit.SquareInches;
                        break;
                    case "SFT":
                    case "SFT.":
                    case "S'":
                    case "SFOOT":
                    case "SFEET":
                        _units = AreaUnit.SquareFeet;
                        break;
                    default:
                        if (_value == 0)
                        {
                            _units = RegionInfo.CurrentRegion.IsMetric ? AreaUnit.SquareMeters : AreaUnit.SquareFeet;
                        }
                        else
                        {
                            throw new ArgumentException(Properties.Resources.Area_InvalidUnitPortion, "value");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
#if PocketPC
                    throw new ArgumentException(Properties.Resources.Area_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Area_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Area(XmlReader reader)
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
        /// Returns the units portion of an area measurement.
        /// </summary>
        /// <value>An <strong>AreaUnit</strong> value. Default is <strong>Meters</strong>.</value>
        /// <seealso cref="Value">Value Property</seealso>
        /// <remarks>Each area measurement consists of a numeric value paired with a unit type
        /// describing the value. It is not possible to create an area measurement without also
        /// specifying a value.</remarks>
        public AreaUnit Units
        {
            get
            {
                return _units;
            }
        }

        /// <summary>
        /// Returns the numeric portion of an area measurement.
        /// </summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <seealso cref="Units">Units Property</seealso>
        /// <remarks>This property is paired with the <strong>Units</strong> property to form a
        /// complete area measurement.</remarks>
        public double Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Indicates if the value of the current instance is zero.
        /// </summary>
        /// <value>A <strong>Boolean</strong>, <strong>True</strong> if the <strong>Value</strong>
        /// property is zero.</value>
        public bool IsEmpty
        {
            get
            {
                return _value == 0;
            }
        }

        /// <summary>
        /// Indicates if the current instance is using a Metric unit.
        /// </summary>
        /// <value>A <strong>Boolean</strong>, <strong>True</strong> if the <strong>Units</strong>
        /// property is <strong>SquareCentimeters</strong>, <strong>SquareMeters</strong> or
        /// <strong>SquareKilometers</strong>.</value>
        /// <remarks>This property is typically used to see if an area measurement is in a unit type
        /// used by a specific culture. Area measurements can be adjusted to either Metric or
        /// Imperial units using the <strong>ToMetricUnitType</strong> and
        /// <strong>ToImperialUnitType</strong> methods.</remarks>
        public bool IsMetric
        {
            get
            {
                return _units == AreaUnit.SquareCentimeters
                    || _units == AreaUnit.SquareMeters
                    || _units == AreaUnit.SquareKilometers;
            }
        }

        /// <summary>
        /// Indicates if the current instance represents an infinite value.
        /// </summary>
        /// <value>A <strong>Boolean</strong>, <strong>True</strong> if the current instance
        /// represents an infinite value.</value>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_value);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converts the current measurement into square feet.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        ///
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        ///
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        ///
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        ///
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square feet.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareKilometers)
        /// ' Convert the Area measurements to square feet and output the result
        /// Debug.WriteLine(Area1.ToSquareFeet().ToString())
        /// Debug.WriteLine(Area2.ToSquareFeet().ToString())
        /// Debug.WriteLine(Area3.ToSquareFeet().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareInches);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareKilometers);
        /// // Convert the Area measurements to square feet and output the result
        /// Console.WriteLine(Area1.ToSquareFeet().ToString());
        /// Console.WriteLine(Area2.ToSquareFeet().ToString());
        /// Console.WriteLine(Area3.ToSquareFeet().ToString());
        ///   </code>
        ///   </example>
        /// <remarks>This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.</remarks>
        public Area ToSquareFeet()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_FEET_PER_ACRE, AreaUnit.SquareFeet);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_CENTIMETER, AreaUnit.SquareFeet);
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_METER, AreaUnit.SquareFeet);
                case AreaUnit.SquareFeet:
                    return this;
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_INCH, AreaUnit.SquareFeet);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_KILOMETER, AreaUnit.SquareFeet);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_STATUTE_MILE, AreaUnit.SquareFeet);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_FEET_PER_SQUARE_NAUTICAL_MILE, AreaUnit.SquareFeet);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square inches.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square inches.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareKilometers)
        /// ' Convert the Area measurements to square inches and output the result
        /// Debug.WriteLine(Area1.ToSquareInches().ToString())
        /// Debug.WriteLine(Area2.ToSquareInches().ToString())
        /// Debug.WriteLine(Area3.ToSquareInches().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareKilometers);
        /// // Convert the Area measurements to square inches and output the result
        /// Console.WriteLine(Area1.ToSquareInches().ToString());
        /// Console.WriteLine(Area2.ToSquareInches().ToString());
        /// Console.WriteLine(Area3.ToSquareInches().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        ///
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        ///
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        ///
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        /// <remarks>This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.</remarks>
        public Area ToSquareInches()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_INCHES_PER_ACRE, AreaUnit.SquareInches);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_CENTIMETER, AreaUnit.SquareInches);
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_METER, AreaUnit.SquareInches);
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_FOOT, AreaUnit.SquareInches);
                case AreaUnit.SquareInches:
                    return this;
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_KILOMETER, AreaUnit.SquareInches);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_STATUTE_MILE, AreaUnit.SquareInches);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_INCHES_PER_SQUARE_NAUTICAL_MILE, AreaUnit.SquareInches);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square kilometers.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square kilometers.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square kilometers and output the result
        /// Debug.WriteLine(Area1.ToSquareKilometers().ToString())
        /// Debug.WriteLine(Area2.ToSquareKilometers().ToString())
        /// Debug.WriteLine(Area3.ToSquareKilometers().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square kilometers and output the result
        /// Console.WriteLine(Area1.ToSquareKilometers().ToString());
        /// Console.WriteLine(Area2.ToSquareKilometers().ToString());
        /// Console.WriteLine(Area3.ToSquareKilometers().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        ///
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        ///
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        ///
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        /// <remarks>This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.</remarks>
        public Area ToSquareKilometers()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_KILOMETERS_PER_ACRE, AreaUnit.SquareKilometers);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_CENTIMETER, AreaUnit.SquareKilometers);
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_METER, AreaUnit.SquareKilometers);
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_FOOT, AreaUnit.SquareKilometers);
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_INCH, AreaUnit.SquareKilometers);
                case AreaUnit.SquareKilometers:
                    return this;
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_STATUTE_MILE, AreaUnit.SquareKilometers);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_KILOMETERS_PER_SQUARE_NAUTICAL_MILE, AreaUnit.SquareKilometers);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square meters.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square meters.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square meters and output the result
        /// Debug.WriteLine(Area1.ToSquareMeters().ToString())
        /// Debug.WriteLine(Area2.ToSquareMeters().ToString())
        /// Debug.WriteLine(Area3.ToSquareMeters().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square meters and output the result
        /// Console.WriteLine(Area1.ToSquareMeters().ToString());
        /// Console.WriteLine(Area2.ToSquareMeters().ToString());
        /// Console.WriteLine(Area3.ToSquareMeters().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        ///
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        ///
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        ///
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        ///
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        /// <remarks>This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.</remarks>
        public Area ToSquareMeters()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_METERS_PER_ACRE, AreaUnit.SquareMeters);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_CENTIMETER, AreaUnit.SquareMeters);
                case AreaUnit.SquareMeters:
                    return this;
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_FOOT, AreaUnit.SquareMeters);
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_INCH, AreaUnit.SquareMeters);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_KILOMETER, AreaUnit.SquareMeters);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_STATUTE_MILE, AreaUnit.SquareMeters);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_METERS_PER_SQUARE_NAUTICAL_MILE, AreaUnit.SquareMeters);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square nautical miles.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square nautical miles.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square nautical miles and output the result
        /// Debug.WriteLine(Area1.ToSquareNauticalMiles().ToString())
        /// Debug.WriteLine(Area2.ToSquareNauticalMiles().ToString())
        /// Debug.WriteLine(Area3.ToSquareNauticalMiles().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square nautical miles and output the result
        /// Console.WriteLine(Area1.ToSquareNauticalMiles().ToString());
        /// Console.WriteLine(Area2.ToSquareNauticalMiles().ToString());
        /// Console.WriteLine(Area3.ToSquareNauticalMiles().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        ///
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        ///
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        ///
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        ///
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        /// <remarks>This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.</remarks>
        public Area ToSquareNauticalMiles()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_ACRE, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_CENTIMETER, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_METER, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_FOOT, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_INCH, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_KILOMETER, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_NAUTICAL_MILES_PER_SQUARE_STATUTE_MILE, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareNauticalMiles:
                    return this;
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square miles.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted
        /// value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square miles.
        ///   <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square statute miles and output the result
        /// Debug.WriteLine(Area1.ToSquareStatuteMiles().ToString())
        /// Debug.WriteLine(Area2.ToSquareStatuteMiles().ToString())
        /// Debug.WriteLine(Area3.ToSquareStatuteMiles().ToString())
        ///   </code>
        ///   <code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square statute miles and output the result
        /// Console.WriteLine(Area1.ToSquareStatuteMiles().ToString());
        /// Console.WriteLine(Area2.ToSquareStatuteMiles().ToString());
        /// Console.WriteLine(Area3.ToSquareStatuteMiles().ToString());
        ///   </code>
        ///   </example>
        ///
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        ///
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        ///
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        ///
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        ///
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        /// <remarks>This method will perform a conversion regardless of the current unit type. A
        /// "statute mile" is frequently referred to as "mile" by itself.</remarks>
        public Area ToSquareStatuteMiles()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_ACRE, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_CENTIMETER, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_METER, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_FOOT, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_INCH, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_KILOMETER, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareStatuteMiles:
                    return this;
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_STATUTE_MILES_PER_SQUARE_NAUTICAL_MILE, AreaUnit.SquareStatuteMiles);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into acres.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into acres.
        ///   <code lang="VB" title="[New Example]">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to acres and output the result
        /// Debug.WriteLine(Area1.ToAcres().ToString())
        /// Debug.WriteLine(Area2.ToAcres().ToString())
        /// Debug.WriteLine(Area3.ToAcres().ToString())
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to acres and output the result
        /// Console.WriteLine(Area1.ToAcres().ToString());
        /// Console.WriteLine(Area2.ToAcres().ToString());
        /// Console.WriteLine(Area3.ToAcres().ToString());
        ///   </code>
        ///   </example>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToAcres()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return this;
                case AreaUnit.SquareCentimeters:
                    return new Area(_value * ACRES_PER_SQUARE_CENTIMETER, AreaUnit.Acres);
                case AreaUnit.SquareMeters:
                    return new Area(_value * ACRES_PER_SQUARE_METER, AreaUnit.Acres);
                case AreaUnit.SquareFeet:
                    return new Area(_value * ACRES_PER_SQUARE_FOOT, AreaUnit.Acres);
                case AreaUnit.SquareInches:
                    return new Area(_value * ACRES_PER_SQUARE_INCH, AreaUnit.Acres);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * ACRES_PER_SQUARE_KILOMETER, AreaUnit.Acres);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * ACRES_PER_SQUARE_STATUTE_MILE, AreaUnit.Acres);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * ACRES_PER_SQUARE_NAUTICAL_MILE, AreaUnit.Acres);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into square centimeters.
        /// </summary>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <example>
        /// This example converts various three <strong>Area</strong> objects, each with a
        /// different unit type, into square centimeters.
        ///   <code lang="VB" title="[New Example]">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square centimeters and output the result
        /// Debug.WriteLine(Area1.ToSquareCentimeters().ToString())
        /// Debug.WriteLine(Area2.ToSquareCentimeters().ToString())
        /// Debug.WriteLine(Area3.ToSquareCentimeters().ToString())
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square centimeters and output the result
        /// Console.WriteLine(Area1.ToSquareCentimeters().ToString());
        /// Console.WriteLine(Area2.ToSquareCentimeters().ToString());
        /// Console.WriteLine(Area3.ToSquareCentimeters().ToString());
        ///   </code>
        ///   </example>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToSquareCentimeters()
        {
            switch (_units)
            {
                case AreaUnit.Acres:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_ACRE, AreaUnit.Acres);
                case AreaUnit.SquareCentimeters:
                    return this;
                case AreaUnit.SquareMeters:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_METER, AreaUnit.Acres);
                case AreaUnit.SquareFeet:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_FOOT, AreaUnit.Acres);
                case AreaUnit.SquareInches:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_INCH, AreaUnit.Acres);
                case AreaUnit.SquareKilometers:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_KILOMETER, AreaUnit.Acres);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_STATUTE_MILE, AreaUnit.Acres);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_value * SQUARE_CENTIMETERS_PER_SQUARE_NAUTICAL_MILE, AreaUnit.Acres);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current instance to an Imperial unit type which minimizes numeric
        /// value.
        /// </summary>
        /// <returns>An <strong>Area</strong> converted to Imperial units. (i.e. feet, inches,
        /// miles)</returns>
        /// <example>
        /// This example converts a measurement of 10560 feet into 1 square statute mile using
        /// the <strong>ToMetricUnitType</strong> method.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(27878400, AreaUnit.SquareFeet)
        /// Dim Area2 As Area = Area1.ToImperialUnitType()
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square statute mile
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(27878400, AreaUnit.SquareFeet);
        /// Area Area2 = Area1.ToImperialUnitType();
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square statute mile
        ///   </code>
        ///   </example>
        /// <remarks>This method is used to make an area measurement easier to read by choosing
        /// another unit type. For example, "27, 878, 400 square feet" would be easier to
        /// understand as "1 square statute mile." This method converts the current instance to
        /// Metric unit which brings the <strong>Value</strong> closest to 1, then returns the
        /// new value. This method will perform a conversion regardless of the current unit
        /// type.</remarks>
        public Area ToImperialUnitType()
        {
            // Start with the largest possible unit
            Area temp = ToSquareStatuteMiles();
            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToSquareFeet();
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToSquareInches();
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToSquareCentimeters();
            return temp;
        }

        /// <summary>
        /// Converts the current instance to a Metric unit type which minimizes numeric
        /// value.
        /// </summary>
        /// <returns>An <strong>Area</strong> converted to Metric units. (i.e. centimeter, meter,
        /// kilometer)</returns>
        /// <example>
        /// This example converts a measurement of 0.0001 kilometers into 1 meter using the
        ///   <strong>ToMetricUnitType</strong> method.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(0.0001, AreaUnit.SquareKilometers)
        /// Dim Area2 As Area = Area1.ToMetricUnitType()
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square meter
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(0.0001, AreaUnit.SquareKilometers);
        /// Area Area2 = Area1.ToMetricUnitType();
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square meter
        ///   </code>
        ///   </example>
        /// <remarks>This method is used to make an area measurement easier to read by choosing
        /// another unit type. For example, "0.0002 kilometers" would be easier to read as "2
        /// meters." This method converts the current instance to Metric unit which brings the
        /// <strong>Value</strong> closest to 1, then returns the new value. This method will
        /// perform a conversion regardless of the current unit type.</remarks>
        public Area ToMetricUnitType()
        {
            // Start with the largest possible unit
            Area temp = ToSquareKilometers();
            // If the value is less than one, bump down
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToSquareMeters();
            // And so on until we find the right unit
            if (Math.Abs(temp.Value) < 1.0)
                temp = temp.ToSquareCentimeters();
            return temp;
        }

        /// <summary>
        /// Converts the current instance to a Metric or Imperial unit type depending on the
        /// local culture.
        /// </summary>
        /// <returns>An <strong>Area</strong> converted to Metric or Imperial units, depending on the
        /// local culture.</returns>
        /// <example>
        /// See
        ///   <see cref="ToImperialUnitType"><strong>ToImperialUnitType</strong></see> and
        ///   <see cref="ToMetricUnitType"><strong>ToMetricUnitType</strong></see> methods
        /// for examples.
        ///   </example>
        /// <remarks>This method is used to make an area measurement easier to read by choosing
        /// another unit type. For example, "0.0002 kilometers" would be easier to read as "2
        /// meters." This method converts the current instance to either a Metric or an Imperial
        /// unit (depending on the local culture) which brings the <strong>Value</strong> closest
        /// to 1. This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            return ToImperialUnitType();
        }

        /// <summary>
        /// Converts the current instance into the specified unit type.
        /// </summary>
        /// <param name="value">An <strong>AreaUnit</strong> value specifying the unit type to convert to.</param>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <example>
        /// This example uses the <strong>ToUnitType</strong> method to convert an area
        /// measurement of 27, 878, 400 square feet into 1 square statute mile.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(27878400, AreaUnit.SquareFeet)
        /// Dim Area2 As Area = Area1.ToUnitType(AreaUnit.SquareStatuteMiles)
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square statute mile
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 As New Area(27878400, AreaUnit.SquareFeet);
        /// Area Area2 As Area = Area1.ToUnitType(AreaUnit.SquareStatuteMiles);
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square statute mile
        ///   </code>
        ///   </example>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToUnitType(AreaUnit value)
        {
            switch (value)
            {
                case AreaUnit.Acres:
                    return ToAcres();
                case AreaUnit.SquareCentimeters:
                    return ToSquareCentimeters();
                case AreaUnit.SquareFeet:
                    return ToSquareFeet();
                case AreaUnit.SquareInches:
                    return ToSquareInches();
                case AreaUnit.SquareKilometers:
                    return ToSquareKilometers();
                case AreaUnit.SquareMeters:
                    return ToSquareMeters();
                case AreaUnit.SquareNauticalMiles:
                    return ToSquareNauticalMiles();
                case AreaUnit.SquareStatuteMiles:

                    return ToSquareStatuteMiles();
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Empty;
            }
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <strong>String</strong> containing the Area in the specified format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a Area measurement
        /// using a custom format.
        ///   <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString("v.v uuu"))
        /// ' Output: 75.0 square statute miles
        ///   </code>
        ///   <code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea As New Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString("v.v uuu"));
        /// // Output: 75.0 square statute miles
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
        /// Adds the specified area to the current instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Area</strong> structure containing the summed values.</returns>
        /// <example>
        /// This example demonstrates how two areas of different unit types can be safely added
        /// together. A value of 144 square inches (which is the same as one square foot) is
        /// added to one square foot, producing two square feet.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(1, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(144, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Add(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 2 square feet
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(1, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(144, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Add(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 2 square feet
        ///   </code>
        ///   </example>
        /// <remarks>This method can add any <strong>Area</strong> object to the current instance. If
        /// the unit type of the <strong>Value</strong> parameter does not match that of the
        /// current instance, the value is converted to the unit type of the current instance
        /// before adding.</remarks>
        public Area Add(Area value)
        {
            return new Area(_value + value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Subtracts the specified area from the current instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        /// This example demonstrates how two areas of different unit types can be safely
        /// subtracted. A value of 144 square inches (which is the same as one square foot) is
        /// subtracted from one square foot, producing a result of zero.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(1, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(144, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Subtract(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 0 square feet
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(1, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(144, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Subtract(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 0 square feet
        ///   </code>
        ///   </example>
        /// <remarks>This method will subtract any <strong>Area</strong> object from the current
        /// instance. If the unit type of the <strong>Value</strong> parameter does not match that
        /// of the current instance, the value is converted to the unit type of the current
        /// instance before subtracting.</remarks>
        public Area Subtract(Area value)
        {
            return new Area(_value - value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Multiplies the specified area with the current instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Area</strong> structure containing the product of the two
        /// values.</returns>
        /// <example>
        /// This example demonstrates how two areas can be multiplied together. A value of 50
        /// square inches is multiplied by two square inches, producing a result of 100 square
        /// inches.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(50, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(2, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Multiply(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 100 square inches
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(50, AreaUnit.SquareInches);
        /// Area Area2 = new Area(2, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Multiply(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 100 square inches
        ///   </code>
        ///   </example>
        /// <remarks>This method will multiply any <strong>Area</strong> object from the current
        /// instance. If the unit type of the <strong>Value</strong> parameter does not match that
        /// of the current instance, the value is converted to the unit type of the current
        /// instance before multiplication.</remarks>
        public Area Multiply(Area value)
        {
            return new Area(_value * value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Divides the current instance by the specified area.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        /// This example demonstrates how two areas can be divided. A value of 100 square
        /// inches is divided by two square inches, producing a result of 50 square inches.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(100, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(2, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Divide(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 50 square inches
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(100, AreaUnit.SquareInches);
        /// Area Area2 = new Area(2, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Divide(Area2);
        /// Debug.WriteLine(Area3.ToString());
        /// // Output: 50 square inches
        ///   </code>
        ///   </example>
        /// <remarks>This method will devide the current instance by any <strong>Area</strong> object.
        /// If the unit type of the <strong>Value</strong> parameter does not match that of the
        /// current instance, the value is converted to the unit type of the current instance
        /// before devision.</remarks>
        public Area Divide(Area value)
        {
            return new Area(_value / value.ToUnitType(_units).Value, _units);
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        /// This example uses the <strong>Increment</strong> method to increase an area's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Increment</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1 = Area1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1.Increment()
        /// ' NOTE: Area1 will still be 1 square meter, not 2!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1 = Area1.Increment();
        /// // Incorrect use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1.Increment();
        /// // NOTE: Area1 will still be 1 square meter, not 2!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method increases the <strong>Value</strong> property by 1.0, returned as
        /// a new instance. The <strong>Units</strong> property is preserved.</para>
        ///   <para><font color="red"><font color="red">NOTE: Since the <strong>Area</strong>
        /// class is immutable, this method will not modify the current
        /// instance.</font></font></para></remarks>
        public Area Increment()
        {
            return new Area(_value + 1.0, _units);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        /// This example uses the <strong>Decrement</strong> method to decrease an area's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Decrement</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1 = Area1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1.Increment()
        /// ' NOTE: Area1 will still be 1 square meter, not 0!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1 = Area1.Decrement();
        /// // Incorrect use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1.Decrement();
        /// // NOTE: Area1 will still be 1 square meter, not 0!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method decreases the <strong>Value</strong> property by 1.0, returned as
        /// a new instance. The <strong>Units</strong> property is preserved.</para>
        ///   <para><font color="red">NOTE: Since the <strong>Area</strong> class is immutable,
        /// this method will not modify the current instance.</font></para></remarks>
        public Area Decrement()
        {
            return new Area(_value - 1.0, _units);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the <strong>Value</strong> parameter.</returns>
        /// <remarks>If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.</remarks>
        public bool IsLessThan(Area value)
        {
            return CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates if the current instance is smaller than or equal to the specified
        /// value.
        /// </summary>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the <strong>Value</strong> parameter.</returns>
        /// <remarks>If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.</remarks>
        public bool IsLessThanOrEqualTo(Area value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        /// <summary>
        /// Indicates if the current instance is larger than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// larger than the <strong>Value</strong> parameter.</returns>
        /// <remarks>If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.</remarks>
        public bool IsGreaterThan(Area value)
        {
            return CompareTo(value) > 0;
        }

        /// <summary>
        /// Indicates if the current instance is larger than or equal to the specified
        /// value.
        /// </summary>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// larger than or equal to the <strong>Value</strong> parameter.</returns>
        /// <remarks>If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.</remarks>
        public bool IsGreaterThanOrEqualTo(Area value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion Math Methods

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Creates a new instance using the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new Area object containing the parsed <see cref="Value">value</see> and
        /// <see cref="Units">unit</see> type.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
        /// 2. The Area unit type was not recognized or not specified.</exception>
        ///
        /// <example>
        /// This example demonstrates how the Parse method can convert several string formats
        /// into a Area object.
        ///   <code lang="VB">
        /// Dim NewArea As Area
        /// ' Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km")
        /// ' Create a Area of 14, 387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14, 387 statute miles").ToSquareInches()
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ")
        ///   </code>
        ///   <code lang="CS">
        /// Area NewArea;
        /// // Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km");
        /// // Create a Area of 14, 387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14, 387 statute miles").ToInches();
        /// // Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ");
        ///   </code>
        ///   </example>
        /// <remarks>This powerful method is typically used to convert a string-based Area
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Area</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.</remarks>
        public static Area Parse(string value)
        {
            return new Area(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Creates a new instance using the specified string and culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing an area measurement.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object specifying which numeric and text formats to use during parsing.</param>
        /// <returns></returns>
        /// <example>
        ///   <code lang="VB" title="[New Example]">
        /// Dim NewArea As Area
        /// ' Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km", CultureInfo.CurrentCulture)
        /// ' Create a Area of 14, 387 miles, then convert it into inches
        /// NewArea = Area.Parse("14, 387 statute miles", CultureInfo.CurrentCulture).ToSquareInches()
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ", CultureInfo.CurrentCulture)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Area NewArea;
        /// // Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km", CultureInfo.CurrentCulture);
        /// // Create a Area of 14, 387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14, 387 statute miles", CultureInfo.CurrentCulture).ToInches();
        /// // Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ", CultureInfo.CurrentCulture);
        ///   </code>
        ///   </example>
        /// <remarks>This powerful method is typically used to convert a string-based Area
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Area</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.</remarks>
        public static Area Parse(string value, CultureInfo culture)
        {
            return new Area(value, culture);
        }

        /// <summary>
        /// Returns a random distance between 0 and 1, 000 square meters.
        /// </summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Area Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random distance between 0 and 1, 000 square meters.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Area Random(Random generator)
        {
            return new Area(generator.NextDouble() * 1000, AreaUnit.SquareMeters).ToLocalUnitType();
        }

        #endregion Static Methods

        #region Overrides

        /// <summary>
        /// Compares the current instance with the specified object.
        /// </summary>
        /// <param name="obj">An <strong>Area</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the two objects have the
        /// same value.</returns>
        public override bool Equals(object obj)
        {
            // If the type is the same, compare the values
            if (obj is Area)
                return Equals((Area)obj);

            // Not equal
            return false;
        }

        /// <summary>
        /// Returns a unique code for the current instance.
        /// </summary>
        /// <returns>An <strong>Integer</strong> representing a unique code for the current
        /// instance.</returns>
        /// <remarks>Since the <strong>Area</strong> class is immutable, this property may be used
        /// safely with hash tables.</remarks>
        public override int GetHashCode()
        {
            return ToSquareMeters().Value.GetHashCode();
        }

        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the current Area in the default format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a Area
        /// measurement.
        ///   <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString())
        /// ' Output: 75 sq. statute miles
        ///   </code>
        ///   <code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea = nre Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString());
        /// // Output: 75 sq. statute miles
        ///   </code>
        ///   </example>
        /// <remarks>The default format used is "<strong>v uu</strong>" where <strong>v</strong>
        /// represents the numerical portion of the area and <strong>uu</strong> is the unit
        /// type.</remarks>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion Overrides

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Area operator +(Area left, Area right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Area operator -(Area left, Area right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Area operator *(Area left, Area right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Area operator /(Area left, Area right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Area left, Area right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Area left, Area right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Area left, Area right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Area left, Area right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Area left, Area right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Area left, Area right)
        {
            return left.CompareTo(right) > 0;
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="DotSpatial.Positioning.Area"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Area(string value)
        {
            return Parse(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Area"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(Area value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IComparable<Area> Members

        /// <summary>
        /// Compares the current instance to the specified area.
        /// </summary>
        /// <param name="other">An <strong>Area</strong> object to compare with.</param>
        /// <returns>An <strong>Integer</strong>: 0 if the object's values are equivalent, -1 if the
        /// current instance is smaller, or 1 if the current instance is larger.</returns>
        /// <remarks>If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.</remarks>
        public int CompareTo(Area other)
        {
            return _value.CompareTo(other.ToUnitType(_units).Value);
        }

        #endregion IComparable<Area> Members

        #region IEquatable<Area> Members

        /// <summary>
        /// Compares the current instance to the specified <strong>Area</strong>
        /// object.
        /// </summary>
        /// <param name="value">A <strong>Area</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks><para>This method will compare the <em>value</em> of the current instance against
        /// the <strong>Value</strong> parameter. If the <strong>Value</strong> parameter's
        /// unit type does not match the current instance, it will be converted to the current
        /// instance's unit type before performing the comparison.</para>
        ///   <para><em>NOTE: This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Area value)
        {
            return _value == value.ToUnitType(Units).Value;
        }

        /// <summary>
        /// Compares the current instance to the specified <strong>Area</strong>
        /// object.
        /// </summary>
        /// <param name="value">A <strong>Area</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>integer</strong> specifies the precision for the comparison.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks><para>This method will compare the <em>value</em> of the current instance against
        /// the <strong>Value</strong> parameter. If the <strong>Value</strong> parameter's
        /// unit type does not match the current instance, it will be converted to the current
        /// instance's unit type before performing the comparison.</para>
        ///   <para><em>NOTE: This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Area value, int decimals)
        {
            return Math.Round(Value, decimals) == Math.Round(value.ToUnitType(Units).Value, decimals);
        }

        #endregion IEquatable<Area> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and local culture.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <strong>String</strong> containing the Area in the specified format.</returns>
        /// <example>
        /// This example uses the ToString method to populate a TextBox with a Area measurement
        /// using a custom format and culture information.
        ///   <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString("v.v uuu", CultureInfo.CurrentCulture))
        /// ' Output: 75.0 square statute miles
        ///   </code>
        ///   <code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea As New Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString("v.v uuu", CultureInfo.CurrentCulture));
        /// // Output: 75.0 square statute miles
        ///   </code>
        ///   </example>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            string subFormat;
            //Dim IsDecimalHandled As Boolean
            try
            {
                // Use the default if "g" is passed
                if (String.Compare(format, "g", true, CultureInfo.InvariantCulture) == 0)
                {
                    format = "#" + culture.NumberFormat.NumberGroupSeparator + "##0.00 uu";
                }

                // Convert the localized format string to a US format
                format = format.Replace("v", "0").ToUpper(CultureInfo.InvariantCulture);
                // First, replace commas and dots with silly symbols
                //				format = format.Replace(culture.NumberFormat.NumberDecimalSeparator, "#DECIMAL#");
                //				format = format.Replace(culture.NumberFormat.NumberGroupSeparator, "#GROUP#");
                //				// And change them to the local culture
                //				format = format.Replace("#DECIMAL#", ".");
                //				format = format.Replace("#GROUP#", ",");

                // Replace the "d" with "h" since degrees is the same as hours
                format = Value.ToString(format, culture);
                // Is there a units specifier?
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
                            switch (_units)
                            {
                                case AreaUnit.Acres:
                                    format = format.Replace("U", "A");
                                    break;
                                case AreaUnit.SquareCentimeters:
                                    format = format.Replace("U", "cm²");
                                    break;
                                case AreaUnit.SquareFeet:
                                    format = format.Replace("U", "ft²");
                                    break;
                                case AreaUnit.SquareInches:
                                    format = format.Replace("U", "in²");
                                    break;
                                case AreaUnit.SquareKilometers:
                                    format = format.Replace("U", "km²");
                                    break;
                                case AreaUnit.SquareMeters:
                                    format = format.Replace("U", "m²");
                                    break;
                                case AreaUnit.SquareStatuteMiles:
                                    format = format.Replace("U", "mi²");
                                    break;
                                case AreaUnit.SquareNauticalMiles:
                                    format = format.Replace("U", "nm²");
                                    break;
                            }
                            break;
                        case 2:
                            switch (_units)
                            {
                                case AreaUnit.Acres:
                                    format = format.Replace("UU", "ac");
                                    break;
                                case AreaUnit.SquareCentimeters:
                                    format = format.Replace("UU", "sq. cm");
                                    break;
                                case AreaUnit.SquareFeet:
                                    format = format.Replace("UU", "sq. ft");
                                    break;
                                case AreaUnit.SquareInches:
                                    format = format.Replace("UU", "sq. in");
                                    break;
                                case AreaUnit.SquareKilometers:
                                    format = format.Replace("UU", "sq. km");
                                    break;
                                case AreaUnit.SquareMeters:
                                    format = format.Replace("UU", "sq. m");
                                    break;
                                case AreaUnit.SquareStatuteMiles:
                                    format = format.Replace("UU", "sq. mi");
                                    break;
                                case AreaUnit.SquareNauticalMiles:
                                    format = format.Replace("UU", "sq. nmi");
                                    break;
                            }
                            break;
                        case 3:
                            if (Value == 1)
                            {
                                switch (_units)
                                {
                                    case AreaUnit.Acres:
                                        format = format.Replace("UUU", "acre");
                                        break;
                                    case AreaUnit.SquareCentimeters:
                                        format = format.Replace("UUU", "square centimeter");
                                        break;
                                    case AreaUnit.SquareFeet:
                                        format = format.Replace("UUU", "square foot");
                                        break;
                                    case AreaUnit.SquareInches:
                                        format = format.Replace("UUU", "square inch");
                                        break;
                                    case AreaUnit.SquareKilometers:
                                        format = format.Replace("UUU", "square kilometer");
                                        break;
                                    case AreaUnit.SquareMeters:
                                        format = format.Replace("UUU", "square meter");
                                        break;
                                    case AreaUnit.SquareStatuteMiles:
                                        format = format.Replace("UUU", "square mile");
                                        break;
                                    case AreaUnit.SquareNauticalMiles:
                                        format = format.Replace("UUU", "square nautical mile");
                                        break;
                                }
                            }
                            else
                            {
                                switch (_units)
                                {
                                    case AreaUnit.Acres:
                                        format = format.Replace("UUU", "acres");
                                        break;
                                    case AreaUnit.SquareCentimeters:
                                        format = format.Replace("UUU", "square centimeters");
                                        break;
                                    case AreaUnit.SquareFeet:
                                        format = format.Replace("UUU", "square feet");
                                        break;
                                    case AreaUnit.SquareInches:
                                        format = format.Replace("UUU", "square inches");
                                        break;
                                    case AreaUnit.SquareKilometers:
                                        format = format.Replace("UUU", "square kilometers");
                                        break;
                                    case AreaUnit.SquareMeters:
                                        format = format.Replace("UUU", "square meters");
                                        break;
                                    case AreaUnit.SquareStatuteMiles:
                                        format = format.Replace("UUU", "square miles");
                                        break;
                                    case AreaUnit.SquareNauticalMiles:
                                        format = format.Replace("UUU", "square nautical miles");
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
#if PocketPC
                throw new ArgumentException(Properties.Resources.Area_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Area_InvalidFormat, "format", ex);
#endif
            }
            //catch
            //{
            //    throw new ArgumentException(Properties.Resources.Area_InvalidFormat), "format");
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

            _units = (AreaUnit)Enum.Parse(typeof(AreaUnit), reader.ReadElementContentAsString(), true);
            _value = reader.ReadElementContentAsDouble();
        }

        #endregion IXmlSerializable Members
    }

    /// <summary>
    /// Indicates the unit of measure for area measurements.
    /// </summary>
    /// <seealso cref="Area.Value">Value Property (Area Class)</seealso>
    ///
    /// <seealso cref="Area.Units">Units Property (Area Class)</seealso>
    ///
    /// <example>
    /// This example uses the <strong>AreaUnit</strong> enumeration to create a new
    ///   <strong>Area</strong> object.
    ///   <code lang="VB" title="[New Example]">
    /// Dim Area1 As New Area(1, AreaUnit.SquareKilometers)
    ///   </code>
    ///   <code lang="CS" title="[New Example]">
    /// Area Area1 = new Area(1, AreaUnit.SquareKilometers);
    ///   </code>
    ///   </example>
    /// <remarks>This enumeration is most frequently used by the Units property of the Area
    /// structure to describe an area measurement.</remarks>
    public enum AreaUnit
    {
        /// <summary>Metric System. Kilometers (thousands of meters).</summary>
        SquareKilometers,
        /// <summary>Metric System. 1/1000th of a square kilometer.</summary>
        SquareMeters,
        /// <summary>Metric System. 1/100th of a square meter.</summary>
        SquareCentimeters,
        /// <summary>Imperial System. A statute mile, most often referred to just as "mile."</summary>
        SquareStatuteMiles,
        /// <summary>Nautical miles, also known as "sea miles".</summary>
        SquareNauticalMiles,
        /// <summary>Imperial System. Feet.</summary>
        SquareFeet,
        /// <summary>Imperial System. Inches.</summary>
        SquareInches,
        /// <summary>Imperial System. Inches.</summary>
        Acres
    }
}