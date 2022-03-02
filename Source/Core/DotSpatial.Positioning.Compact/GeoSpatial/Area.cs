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
	/// Represents the measurement of surface area of a polygon on Earth's
	/// surface.
	/// </summary>
	/// <remarks>
	/// 	<para>This structure is used to represent measurements of arbitrary polygons on
	///     Earth's surface. Measurements can be converted to different unit types, such as
	///     acres, square kilometers, and square miles.</para>
	/// 	<para>Instances of this structure are guaranteed to be thread-safe because they are
	///     immutable (properties can only be modified via constructors).</para>
	/// </remarks>
	/// <example>
	///     This example demonstrates how to create an <strong>Area</strong> structure and
	///     convert it to another unit type.
	///     <code lang="VB">
	/// ' Declare a Area of 50 meters
	/// Dim Area1 As New Area(50, AreaUnit.SquareMeters)
	/// ' Convert it into acres
	/// Dim Area2 As Area = Area2.ToAcres()
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a Area of 50 meters
	/// Area Area1 = new Area(50, AreaUnit.SquareMeters);
	/// // Convert it into acres
	/// Area Area2 = Area2.ToAcres();
	///     </code>
	/// </example>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.AreaConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
	public struct Area : IFormattable, IComparable<Area>, IEquatable<Area>, IXmlSerializable
    {
        private double _Value;
		private AreaUnit _Units;

        #region Constants

        private const double AcresPerSquareMeter = 0.0002471054;
		private const double AcresPerSquareCentimeter = 2.471054e-8;
		private const double AcresPerSquareStatuteMile = 640;
		private const double AcresPerSquareKilometer = 247.1054;
		private const double AcresPerSquareInch = 1.594225e-7;
		private const double AcresPerSquareNauticalMile = 847.547736;
		private const double AcresPerSquareFoot = 2.29568411e-5;
		private const double SquareFeetPerSquareMeter = 10.76391;
		private const double SquareFeetPerSquareCentimeter = 0.001076391;
		private const double SquareFeetPerSquareStatuteMile = 27878400;
		private const double SquareFeetPerSquareKilometer = 10763910.4;
		private const double SquareFeetPerSquareInch = 0.00694444444;
		private const double SquareFeetPerSquareNauticalMile = 36919179.4;
		private const double SquareFeetPerAcre = 43560;
		private const double SquareInchesPerSquareMeter = 1550.003;
		private const double SquareInchesPerSquareCentimeter = 0.1550003;
		private const double SquareInchesPerSquareStatuteMile = 4014489600;
		private const double SquareInchesPerSquareKilometer = 1.5500031e09;
		private const double SquareInchesPerSquareFoot = 144;
		private const double SquareInchesPerSquareNauticalMile = 5.31636183e9;
		private const double SquareInchesPerAcre = 6272640;
		private const double SquareStatuteMilesPerSquareMeter = 3.861022e-7;
		private const double SquareStatuteMilesPerSquareCentimeter = 3.861022e-11;
		private const double SquareStatuteMilesPerSquareKilometer = 0.3861022;
		private const double SquareStatuteMilesPerSquareInch = 2.490977e-10;
		private const double SquareStatuteMilesPerSquareFoot = 3.58700643e-8;
		private const double SquareStatuteMilesPerSquareNauticalMile = 1.32429334 ;
		private const double SquareStatuteMilesPerAcre = 0.0015625;
		private const double SquareNauticalMilesPerSquareMeter = 2.9155335e-07;
		private const double SquareNauticalMilesPerSquareCentimeter = 2.9155335e-11;
		private const double SquareNauticalMilesPerSquareKilometer = 0.29155335;
		private const double SquareNauticalMilesPerSquareInch = 1.88098559e-10;
		private const double SquareNauticalMilesPerSquareFoot = 2.70861925e-8;
		private const double SquareNauticalMilesPerSquareStatuteMile = 0.755119709;
		private const double SquareNauticalMilesPerAcre = 0.00117987455;
		private const double SquareCentimetersPerSquareStatuteMile = 2.58998811e10;
		private const double SquareCentimetersPerSquareKilometer = 10000000000;
		private const double SquareCentimetersPerSquareFoot = 929.0304;
		private const double SquareCentimetersPerSquareInch = 6.4516;
		private const double SquareCentimetersPerSquareMeter = 10000;
		private const double SquareCentimetersPerSquareNauticalMile = 34299040000;
		private const double SquareCentimetersPerAcre = 40468564.2;
		private const double SquareMetersPerSquareStatuteMile = 2589988.11;
		private const double SquareMetersPerSquareCentimeter = 0.0001;
		private const double SquareMetersPerSquareKilometer = 1000000;
		private const double SquareMetersPerSquareFoot = 0.09290304;
		private const double SquareMetersPerSquareInch = 0.00064516;
		private const double SquareMetersPerAcre = 4046.85642;
		private const double SquareMetersPerSquareNauticalMile = 3429904;
		private const double SquareKilometersPerSquareMeter = 0.000001;
		private const double SquareKilometersPerSquareCentimeter = 1e-10;
		private const double SquareKilometersPerSquareStatuteMile = 2.589988;
		private const double SquareKilometersPerSquareFoot = 9.290304e-8;
		private const double SquareKilometersPerSquareInch = 6.4516e-10;
		private const double SquareKilometersPerSquareNauticalMile = 3.429904;
		private const double SquareKilometersPerAcre = 0.004046856;
		
        #endregion

		#region Fields

		/// <summary>Represents an area with no value.</summary>
		public static readonly Area Empty = new Area(0.0, AreaUnit.SquareMeters).ToLocalUnitType();

		/// <summary>Represents an area of infinite value.</summary>
		/// <remarks>
		/// In some rare cases, the result of a mathematical formula might be infinity. This
		/// field is used to represent such values when they exist.
		/// </remarks>
		public static readonly Area Infinity = new Area(double.PositiveInfinity, AreaUnit.SquareMeters).ToLocalUnitType();

		/// <summary>Represents the largest possible area which can be stored.</summary>
        public static readonly Area Maximum = new Area(double.MaxValue, AreaUnit.SquareKilometers).ToLocalUnitType();

		/// <summary>Represents the smallest possible area which can be stored.</summary>
        public static readonly Area Minimum = new Area(double.MinValue, AreaUnit.SquareKilometers).ToLocalUnitType();

        /// <summary>
        /// Represents an invalid or unspecified area.
        /// </summary>
        public static readonly Area Invalid = new Area(double.NaN, AreaUnit.SquareMeters);

		#endregion

		#region  Constructors

		/// <summary>Creates a new instance using the specified value and unit type.</summary>
		/// <example>
		///     This example uses a constructor to create a new <strong>Area</strong> of fifty
		///     square kilometers. 
		///     <code lang="VB">
		/// Dim MyArea As New Area(50, AreaUnit.SquareKilometers)
		///     </code>
		/// 	<code lang="CS">
		/// Area MyArea = new Area(50, AreaUnit.SquareKilometers);
		///     </code>
		/// </example>
		public Area(double value, AreaUnit units)
		{
			_Value = value;
			_Units = units;
		}

		/// <summary>Creates a new instance using the specified string.</summary>
		/// <remarks>
		/// This powerful constructor is used to convert an area measurement in the form of a
		/// string into an object, such as one entered by a user or read from a file. This
		/// constructor can accept any output created via the <see cref="ToString()">ToString</see>
		/// method.
		/// </remarks>
		/// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
		/// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
		/// 2. The Area unit type was not recognized or not specified.</exception>
		/// <example>
		///     This example demonstrates how the to use this constructor. 
		///     <code lang="VB">
		/// Dim MyArea As Area
		/// ' Create a Area of 50 square kilometers
		/// MyArea = New Area("50 sq. km")
		/// ' Create a Area of 14,387 miles, then convert it into square inches
		/// MyArea = New Area("14,387 sq. statute miles").ToSquareInches()
		/// ' Create a Area of 50 square feet
		/// MyArea = New Area("    50 sq '       ")
		///     </code>
		/// 	<code lang="CS">
		/// Area MyArea;
		/// ' Create a Area of 50 square kilometers
		/// MyArea = new Area("50 sq. km");
		/// ' Create a Area of 14,387 miles, then convert it into square inches
		/// MyArea = new Area("14,387 sq. statute miles").ToSquareInches();
		/// ' Create a Area of 50 square feet
		/// MyArea = new Area("    50 sq '       ");
		///     </code>
		/// </example>
		/// <returns>An <strong>Area</strong> object.</returns>
		/// <seealso cref="Parse(System.String)">Parse(string) Method</seealso>
		public Area(string value)
			: this(value, CultureInfo.CurrentCulture)
		{}

		/// <summary>Creates a new instance using the specified string and culture.</summary>
		/// <remarks>
		/// This powerful constructor is used to convert an area measurement in the form of a
		/// string into an object, such as one entered by a user or read from a file. This
		/// constructor can accept any output created via the <see cref="ToString()">ToString</see>
		/// method.
		/// </remarks>
		/// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
		/// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
		/// 2. The Area unit type was not recognized or not specified.</exception>
		/// <example>
		///     This example demonstrates how the to use this constructor. 
		///     <code lang="VB">
		/// Dim MyArea As Area
		/// ' Create a Area of 50 square kilometers
		/// MyArea = New Area("50 sq. km", CultureInfo.CurrentCulture)
		/// ' Create a Area of 14,387 miles, then convert it into square inches
		/// MyArea = New Area("14,387 sq. statute miles", CultureInfo.CurrentCulture).ToSquareInches()
		/// ' Create a Area of 50 square feet
		/// MyArea = New Area("    50 sq '       ", CultureInfo.CurrentCulture)
		///     </code>
		/// 	<code lang="CS">
		/// Area MyArea;
		/// ' Create a Area of 50 square kilometers
		/// MyArea = new Area("50 sq. km", CultureInfo.CurrentCulture);
		/// ' Create a Area of 14,387 miles, then convert it into square inches
		/// MyArea = new Area("14,387 sq. statute miles", CultureInfo.CurrentCulture).ToSquareInches();
		/// ' Create a Area of 50 square feet
		/// MyArea = new Area("    50 sq '       ", CultureInfo.CurrentCulture);
		///     </code>
		/// </example>
		/// <returns>An <strong>Area</strong> object.</returns>
		/// <seealso cref="Parse(System.String)">Parse(string) Method</seealso>
		public Area(string value, CultureInfo culture)
		{
			// Anything to do?
			if (value == null || value.Length == 0)
			{
				_Value = 0;
				_Units = AreaUnit.SquareCentimeters;
				return;					
			}

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

			string NumericPortion = null;
			int Count = 0;
			string Unit = null;

			try
			{
				// Convert to uppercase and remove commas
                value = value.Trim();
				if (String.Compare(value, Properties.Resources.Common_Infinity, true, culture) == 0)
				{
					_Value = double.PositiveInfinity;
					_Units = AreaUnit.SquareNauticalMiles;
					return;
				}
                if (String.Compare(value, Properties.Resources.Common_Empty, true, culture) == 0)
				{
					_Value = 0;
					_Units = AreaUnit.SquareCentimeters;
					return;					
				}

                // Clean up the value
                value = value.ToUpper(culture).Replace(culture.NumberFormat.NumberGroupSeparator, "");

				// Go until the first non-number character
				Count = 0;
				while (Count < value.Length)
				{
					string Digit = value.Substring(Count, 1);
					if (Digit == "0" || Digit == "1" || Digit == "2" || Digit == "3"
						|| Digit == "4" || Digit == "5" || Digit == "6" || Digit == "7"
						|| Digit == "8" || Digit == "9"
						|| Digit == culture.NumberFormat.NumberGroupSeparator
						|| Digit == culture.NumberFormat.NumberDecimalSeparator)
						// Allow continuation
						Count = Count + 1;
					else
						// Non-numeric character!
						break;
				}
				Unit = value.Substring(Count).Trim();
				// Get the numeric portion
				if (Count > 0)
					NumericPortion = value.Substring(0, Count);
				else
					NumericPortion = "0";
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
				if(!double.TryParse(NumericPortion, NumberStyles.Any, culture, out _Value))
					throw new ArgumentException(Properties.Resources.Area_InvalidNumericPortion, "value");
#endif
				// Try to interpret the measurement
				// Remove any notion of "square"
				Unit = Unit.Replace("SQUARE", "S").Replace("SQ.", "S").Replace("SQ", "S").Replace(" ", "").Trim();

                switch (Unit)
                {
                    case "A":
                    case "AC":
                    case "ACRE":
                    case "ACRES":
					    _Units = AreaUnit.Acres;
                        break;
                    case "SCM":
                    case "SCM.":
                    case "SCENTIMETER":
                    case "SCENTIMETERS":
                    case "SCENTIMETRE":
                    case "SCENTIMETRES":
					    _Units = AreaUnit.SquareCentimeters;
                        break;
                    case "SM":
                    case "SM.":
                    case "SMETERS":
                    case "SMETRES":
                    case "SMETRE":
                    case "SMETER":
					    _Units = AreaUnit.SquareMeters;
                        break;
                    case "SKM":
                    case "SKM.":
                    case "SKILOMETRES":
                    case "SKILOMETERS":
                    case "SKILOMETRE":
                    case "SKILOMETER":
					    _Units = AreaUnit.SquareKilometers;
                        break;
				    case "SMI":
                    case "SMI.":
                    case "SMILE":
                    case "SMILES":
                    case "SSTATUTEMILES":
					    _Units = AreaUnit.SquareStatuteMiles;
                        break;
                    case "SNM":
                    case "SNM.":
                    case "SNAUTICALMILE":
					case "SNAUTICALMILES":
					    _Units = AreaUnit.SquareNauticalMiles;
                        break;
                    case "SIN":
                    case "SIN.":
                    case "S\"":
                    case "SINCHES":
					case "SINCH":
					    _Units = AreaUnit.SquareInches;
                        break;
                    case "SFT":
                    case "SFT.":
                    case "S'":
                    case "SFOOT":
                    case "SFEET":
					    _Units = AreaUnit.SquareFeet;
                        break;
                    default:
                        if (_Value == 0)
                        {
					        if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
						        _Units = AreaUnit.SquareMeters;
					        else
						        _Units = AreaUnit.SquareFeet;
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
        /// <param name="reader"></param>
        public Area(XmlReader reader)
        {
            // Initialize all fields
            _Value = Double.NaN;
            _Units = 0;

            // Deserialize the object from XML
            ReadXml(reader);
        }

		#endregion

        #region Public Properties

        /// <summary>Returns the units portion of an area measurement.</summary>
        /// <value>An <strong>AreaUnit</strong> value. Default is <strong>Meters</strong>.</value>
        /// <remarks>
        /// 	<para>Each area measurement consists of a numeric value paired with a unit type
        ///     describing the value. It is not possible to create an area measurement without also
        ///     specifying a value.</para>
        /// </remarks>
        /// <seealso cref="Value">Value Property</seealso>
        public AreaUnit Units
        {
            get
            {
                return _Units;
            }
        }

        /// <summary>Returns the numeric portion of an area measurement.</summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>
        /// This property is paired with the <strong>Units</strong> property to form a
        /// complete area measurement.
        /// </remarks>
        /// <seealso cref="Units">Units Property</seealso>
        public double Value
        {
            get
            {
                return _Value;
            }
        }

        /// <summary>Indicates if the value of the current instance is zero.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the <strong>Value</strong>
        /// property is zero.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return _Value == 0;
            }
        }

        /// <summary>Indicates if the current instance is using a Metric unit.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the <strong>Units</strong>
        /// property is <strong>SquareCentimeters</strong>, <strong>SquareMeters</strong> or
        /// <strong>SquareKilometers</strong>.
        /// </value>
        /// <remarks>
        /// This property is typically used to see if an area measurement is in a unit type
        /// used by a specific culture. Area measurements can be adjusted to either Metric or
        /// Imperial units using the <strong>ToMetricUnitType</strong> and
        /// <strong>ToImperialUnitType</strong> methods.
        /// </remarks>
        public bool IsMetric
        {
            get
            {
                return _Units == AreaUnit.SquareCentimeters
                    || _Units == AreaUnit.SquareMeters
                    || _Units == AreaUnit.SquareKilometers;
            }
        }

        /// <summary>Indicates if the current instance represents an infinite value.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance
        /// represents an infinite value.
        /// </value>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_Value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>Converts the current measurement into square feet.</summary>
        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.
        /// </remarks>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square feet.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareKilometers)
        /// ' Convert the Area measurements to square feet and output the result 
        /// Debug.WriteLine(Area1.ToSquareFeet().ToString())
        /// Debug.WriteLine(Area2.ToSquareFeet().ToString())
        /// Debug.WriteLine(Area3.ToSquareFeet().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareInches);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareKilometers);
        /// // Convert the Area measurements to square feet and output the result 
        /// Console.WriteLine(Area1.ToSquareFeet().ToString());
        /// Console.WriteLine(Area2.ToSquareFeet().ToString());
        /// Console.WriteLine(Area3.ToSquareFeet().ToString());
        ///     </code>
        /// </example>
        public Area ToSquareFeet()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareFeetPerAcre, AreaUnit.SquareFeet);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareFeetPerSquareCentimeter, AreaUnit.SquareFeet);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareFeetPerSquareMeter, AreaUnit.SquareFeet);
                case AreaUnit.SquareFeet:
                    return this;
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareFeetPerSquareInch, AreaUnit.SquareFeet);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareFeetPerSquareKilometer, AreaUnit.SquareFeet);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareFeetPerSquareStatuteMile, AreaUnit.SquareFeet);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareFeetPerSquareNauticalMile, AreaUnit.SquareFeet);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <summary>Converts the current measurement into square inches.</summary>
        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.
        /// </remarks>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square inches.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareKilometers)
        /// ' Convert the Area measurements to square inches and output the result 
        /// Debug.WriteLine(Area1.ToSquareInches().ToString())
        /// Debug.WriteLine(Area2.ToSquareInches().ToString())
        /// Debug.WriteLine(Area3.ToSquareInches().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareKilometers);
        /// // Convert the Area measurements to square inches and output the result 
        /// Console.WriteLine(Area1.ToSquareInches().ToString());
        /// Console.WriteLine(Area2.ToSquareInches().ToString());
        /// Console.WriteLine(Area3.ToSquareInches().ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        public Area ToSquareInches()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareInchesPerAcre, AreaUnit.SquareInches);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareInchesPerSquareCentimeter, AreaUnit.SquareInches);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareInchesPerSquareMeter, AreaUnit.SquareInches);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareInchesPerSquareFoot, AreaUnit.SquareInches);
                case AreaUnit.SquareInches:
                    return this;
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareInchesPerSquareKilometer, AreaUnit.SquareInches);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareInchesPerSquareStatuteMile, AreaUnit.SquareInches);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareInchesPerSquareNauticalMile, AreaUnit.SquareInches);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into square kilometers.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square kilometers.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square kilometers and output the result 
        /// Debug.WriteLine(Area1.ToSquareKilometers().ToString())
        /// Debug.WriteLine(Area2.ToSquareKilometers().ToString())
        /// Debug.WriteLine(Area3.ToSquareKilometers().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square kilometers and output the result 
        /// Console.WriteLine(Area1.ToSquareKilometers().ToString());
        /// Console.WriteLine(Area2.ToSquareKilometers().ToString());
        /// Console.WriteLine(Area3.ToSquareKilometers().ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        public Area ToSquareKilometers()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareKilometersPerAcre, AreaUnit.SquareKilometers);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareKilometersPerSquareCentimeter, AreaUnit.SquareKilometers);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareKilometersPerSquareMeter, AreaUnit.SquareKilometers);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareKilometersPerSquareFoot, AreaUnit.SquareKilometers);
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareKilometersPerSquareInch, AreaUnit.SquareKilometers);
                case AreaUnit.SquareKilometers:
                    return this;
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareKilometersPerSquareStatuteMile, AreaUnit.SquareKilometers);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareKilometersPerSquareNauticalMile, AreaUnit.SquareKilometers);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into square meters.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square meters.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square meters and output the result 
        /// Debug.WriteLine(Area1.ToSquareMeters().ToString())
        /// Debug.WriteLine(Area2.ToSquareMeters().ToString())
        /// Debug.WriteLine(Area3.ToSquareMeters().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square meters and output the result 
        /// Console.WriteLine(Area1.ToSquareMeters().ToString());
        /// Console.WriteLine(Area2.ToSquareMeters().ToString());
        /// Console.WriteLine(Area3.ToSquareMeters().ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        public Area ToSquareMeters()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareMetersPerAcre, AreaUnit.SquareMeters);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareMetersPerSquareCentimeter, AreaUnit.SquareMeters);
                case AreaUnit.SquareMeters:
                    return this;
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareMetersPerSquareFoot, AreaUnit.SquareMeters);
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareMetersPerSquareInch, AreaUnit.SquareMeters);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareMetersPerSquareKilometer, AreaUnit.SquareMeters);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareMetersPerSquareStatuteMile, AreaUnit.SquareMeters);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareMetersPerSquareNauticalMile, AreaUnit.SquareMeters);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. You
        /// may convert from any unit type to any other unit type.
        /// </remarks>
        /// <summary>Converts the current measurement into square nautical miles.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square nautical miles.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square nautical miles and output the result 
        /// Debug.WriteLine(Area1.ToSquareNauticalMiles().ToString())
        /// Debug.WriteLine(Area2.ToSquareNauticalMiles().ToString())
        /// Debug.WriteLine(Area3.ToSquareNauticalMiles().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square nautical miles and output the result 
        /// Console.WriteLine(Area1.ToSquareNauticalMiles().ToString());
        /// Console.WriteLine(Area2.ToSquareNauticalMiles().ToString());
        /// Console.WriteLine(Area3.ToSquareNauticalMiles().ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        /// <seealso cref="ToSquareStatuteMiles">ToSquareStatuteMiles Method</seealso>
        public Area ToSquareNauticalMiles()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareNauticalMilesPerAcre, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareNauticalMilesPerSquareCentimeter, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareNauticalMilesPerSquareMeter, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareNauticalMilesPerSquareFoot, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareNauticalMilesPerSquareInch, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareNauticalMilesPerSquareKilometer, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareNauticalMilesPerSquareStatuteMile, AreaUnit.SquareNauticalMiles);
                case AreaUnit.SquareNauticalMiles:
                    return this;
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <returns>A new <strong>Area</strong> object containing the converted 
        /// value.</returns>
        /// <remarks>
        /// This method will perform a conversion regardless of the current unit type. A
        /// "statute mile" is frequently referred to as "mile" by itself.
        /// </remarks>
        /// <summary>Converts the current measurement into square miles.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square miles.
        ///     <code lang="VB">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square statute miles and output the result 
        /// Debug.WriteLine(Area1.ToSquareStatuteMiles().ToString())
        /// Debug.WriteLine(Area2.ToSquareStatuteMiles().ToString())
        /// Debug.WriteLine(Area3.ToSquareStatuteMiles().ToString())
        ///     </code>
        /// 	<code lang="CS">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square statute miles and output the result 
        /// Console.WriteLine(Area1.ToSquareStatuteMiles().ToString());
        /// Console.WriteLine(Area2.ToSquareStatuteMiles().ToString());
        /// Console.WriteLine(Area3.ToSquareStatuteMiles().ToString());
        ///     </code>
        /// </example>
        /// <seealso cref="ToSquareFeet">ToSquareFeet Method</seealso>
        /// <seealso cref="ToSquareInches">ToSquareInches Method</seealso>
        /// <seealso cref="ToSquareKilometers">ToSquareKilometers Method</seealso>
        /// <seealso cref="ToSquareMeters">ToSquareMeters Method</seealso>
        /// <seealso cref="ToSquareNauticalMiles">ToSquareNauticalMiles Method</seealso>
        public Area ToSquareStatuteMiles()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareStatuteMilesPerAcre, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * SquareStatuteMilesPerSquareCentimeter, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareStatuteMilesPerSquareMeter, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareStatuteMilesPerSquareFoot, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareStatuteMilesPerSquareInch, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareStatuteMilesPerSquareKilometer, AreaUnit.SquareStatuteMiles);
                case AreaUnit.SquareStatuteMiles:
                    return this;
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareStatuteMilesPerSquareNauticalMile, AreaUnit.SquareStatuteMiles);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <summary>Converts the current measurement into acres.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into acres. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to acres and output the result 
        /// Debug.WriteLine(Area1.ToAcres().ToString())
        /// Debug.WriteLine(Area2.ToAcres().ToString())
        /// Debug.WriteLine(Area3.ToAcres().ToString())
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to acres and output the result 
        /// Console.WriteLine(Area1.ToAcres().ToString());
        /// Console.WriteLine(Area2.ToAcres().ToString());
        /// Console.WriteLine(Area3.ToAcres().ToString());
        ///     </code>
        /// </example>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToAcres()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return this;
                case AreaUnit.SquareCentimeters:
                    return new Area(_Value * AcresPerSquareCentimeter, AreaUnit.Acres);
                case AreaUnit.SquareMeters:
                    return new Area(_Value * AcresPerSquareMeter, AreaUnit.Acres);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * AcresPerSquareFoot, AreaUnit.Acres);
                case AreaUnit.SquareInches:
                    return new Area(_Value * AcresPerSquareInch, AreaUnit.Acres);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * AcresPerSquareKilometer, AreaUnit.Acres);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * AcresPerSquareStatuteMile, AreaUnit.Acres);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * AcresPerSquareNauticalMile, AreaUnit.Acres);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <summary>Converts the current measurement into square centimeters.</summary>
        /// <example>
        ///     This example converts various three <strong>Area</strong> objects, each with a
        ///     different unit type, into square centimeters. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Create Areas of different unit types
        /// Dim Area1 As New Area(10, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(20, AreaUnit.SquareStatuteMiles)
        /// Dim Area3 As New Area(50, AreaUnit.SquareInches)
        /// ' Convert the Area measurements to square centimeters and output the result 
        /// Debug.WriteLine(Area1.ToSquareCentimeters().ToString())
        /// Debug.WriteLine(Area2.ToSquareCentimeters().ToString())
        /// Debug.WriteLine(Area3.ToSquareCentimeters().ToString())
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Create Areas of different unit types
        /// Area Area1 = new Area(10, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(20, AreaUnit.SquareStatuteMiles);
        /// Area Area3 = new Area(50, AreaUnit.SquareInches);
        /// // Convert the Area measurements to square centimeters and output the result 
        /// Console.WriteLine(Area1.ToSquareCentimeters().ToString());
        /// Console.WriteLine(Area2.ToSquareCentimeters().ToString());
        /// Console.WriteLine(Area3.ToSquareCentimeters().ToString());
        ///     </code>
        /// </example>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        public Area ToSquareCentimeters()
        {
            switch (_Units)
            {
                case AreaUnit.Acres:
                    return new Area(_Value * SquareCentimetersPerAcre, AreaUnit.Acres);
                case AreaUnit.SquareCentimeters:
                    return this;
                case AreaUnit.SquareMeters:
                    return new Area(_Value * SquareCentimetersPerSquareMeter, AreaUnit.Acres);
                case AreaUnit.SquareFeet:
                    return new Area(_Value * SquareCentimetersPerSquareFoot, AreaUnit.Acres);
                case AreaUnit.SquareInches:
                    return new Area(_Value * SquareCentimetersPerSquareInch, AreaUnit.Acres);
                case AreaUnit.SquareKilometers:
                    return new Area(_Value * SquareCentimetersPerSquareKilometer, AreaUnit.Acres);
                case AreaUnit.SquareStatuteMiles:
                    return new Area(_Value * SquareCentimetersPerSquareStatuteMile, AreaUnit.Acres);
                case AreaUnit.SquareNauticalMiles:
                    return new Area(_Value * SquareCentimetersPerSquareNauticalMile, AreaUnit.Acres);
                default:
                    // This should never happen!  Included only to satisfy the compiler
                    return Area.Empty;
            }
        }

        /// <summary>
        /// Converts the current instance to an Imperial unit type which minimizes numeric
        /// value.
        /// </summary>
        /// <returns>
        /// An <strong>Area</strong> converted to Imperial units. (i.e. feet, inches,
        /// miles)
        /// </returns>
        /// <remarks>
        /// 	<para>This method is used to make an area measurement easier to read by choosing
        ///     another unit type. For example, "27,878,400 square feet" would be easier to
        ///     understand as "1 square statute mile." This method converts the current instance to
        ///     Metric unit which brings the <strong>Value</strong> closest to 1, then returns the
        ///     new value. This method will perform a conversion regardless of the current unit
        ///     type.</para>
        /// </remarks>
        /// <example>
        ///     This example converts a measurement of 10560 feet into 1 square statute mile using
        ///     the <strong>ToMetricUnitType</strong> method. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(27878400, AreaUnit.SquareFeet)
        /// Dim Area2 As Area = Area1.ToImperialUnitType()
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square statute mile
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(27878400, AreaUnit.SquareFeet);
        /// Area Area2 = Area1.ToImperialUnitType();
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square statute mile
        ///     </code>
        /// </example>
        public Area ToImperialUnitType()
        {
            // Start with the largest possible unit
            Area Temp = ToSquareStatuteMiles();
            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToSquareFeet();
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToSquareInches();
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToSquareCentimeters();
            return Temp;
        }

        /// <summary>
        /// Converts the current instance to a Metric unit type which minimizes numeric
        /// value.
        /// </summary>
        /// <returns>
        /// An <strong>Area</strong> converted to Metric units. (i.e. centimeter, meter,
        /// kilometer)
        /// </returns>
        /// <remarks>
        /// This method is used to make an area measurement easier to read by choosing
        /// another unit type. For example, "0.0002 kilometers" would be easier to read as "2
        /// meters." This method converts the current instance to Metric unit which brings the
        /// <strong>Value</strong> closest to 1, then returns the new value. This method will
        /// perform a conversion regardless of the current unit type.
        /// </remarks>
        /// <example>
        ///     This example converts a measurement of 0.0001 kilometers into 1 meter using the
        ///     <strong>ToMetricUnitType</strong> method. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(0.0001, AreaUnit.SquareKilometers)
        /// Dim Area2 As Area = Area1.ToMetricUnitType()
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square meter
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(0.0001, AreaUnit.SquareKilometers);
        /// Area Area2 = Area1.ToMetricUnitType();
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square meter
        ///     </code>
        /// </example>
        public Area ToMetricUnitType()
        {
            // Start with the largest possible unit
            Area Temp = ToSquareKilometers();
            // If the value is less than one, bump down
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToSquareMeters();
            // And so on until we find the right unit
            if (Math.Abs(Temp.Value) < 1.0)
                Temp = Temp.ToSquareCentimeters();
            return Temp;
        }

        /// <summary>
        /// Converts the current instance to a Metric or Imperial unit type depending on the
        /// local culture.
        /// </summary>
        /// <returns>
        /// An <strong>Area</strong> converted to Metric or Imperial units, depending on the
        /// local culture.
        /// </returns>
        /// <remarks>
        /// This method is used to make an area measurement easier to read by choosing
        /// another unit type. For example, "0.0002 kilometers" would be easier to read as "2
        /// meters." This method converts the current instance to either a Metric or an Imperial
        /// unit (depending on the local culture) which brings the <strong>Value</strong> closest
        /// to 1. This method will perform a conversion regardless of the current unit type.
        /// </remarks>
        /// <example>
        /// See
        /// <see cref="ToImperialUnitType"><strong>ToImperialUnitType</strong></see> and
        /// <see cref="ToMetricUnitType"><strong>ToMetricUnitType</strong></see> methods
        /// for examples.
        /// </example>
        public Area ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            else
                return ToImperialUnitType();
        }

        /// <summary>Converts the current instance into the specified unit type.</summary>
        /// <returns>A new <strong>Area</strong> object containing the converted value.</returns>
        /// <remarks>This method will perform a conversion regardless of the current unit type.</remarks>
        /// <example>
        ///     This example uses the <strong>ToUnitType</strong> method to convert an area
        ///     measurement of 27,878,400 square feet into 1 square statute mile.
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(27878400, AreaUnit.SquareFeet)
        /// Dim Area2 As Area = Area1.ToUnitType(AreaUnit.SquareStatuteMiles)
        /// Debug.WriteLine(Area2.ToString())
        /// ' Output: 1 square statute mile
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 As New Area(27878400, AreaUnit.SquareFeet);
        /// Area Area2 As Area = Area1.ToUnitType(AreaUnit.SquareStatuteMiles);
        /// Console.WriteLine(Area2.ToString());
        /// // Output: 1 square statute mile
        ///     </code>
        /// </example>
        /// <param name="value">An <strong>AreaUnit</strong> value specifying the unit type to convert to.</param>
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
                    return Area.Empty;
            }
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the Area in the specified format.</returns>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        /// <example>
        ///     This example uses the ToString method to populate a TextBox with a Area measurement
        ///     using a custom format. 
        ///     <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString("v.v uuu"))
        /// ' Output: 75.0 square statute miles
        ///     </code>
        /// 	<code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea As New Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString("v.v uuu"));
        /// // Output: 75.0 square statute miles
        ///     </code>
        /// </example>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }


        #region Math Methods

        /// <summary>Adds the specified area to the current instance.</summary>
        /// <returns>A new <strong>Area</strong> structure containing the summed values.</returns>
        /// <remarks>
        /// This method can add any <strong>Area</strong> object to the current instance. If
        /// the unit type of the <strong>Value</strong> parameter does not match that of the
        /// current instance, the value is converted to the unit type of the current instance
        /// before adding.
        /// </remarks>
        /// <example>
        ///     This example demonstrates how two areas of different unit types can be safely added
        ///     together. A value of 144 square inches (which is the same as one square foot) is
        ///     added to one square foot, producing two square feet. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(1, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(144, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Add(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 2 square feet
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(1, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(144, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Add(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 2 square feet
        ///     </code>
        /// </example>
        public Area Add(Area value)
        {
            return new Area(_Value + value.ToUnitType(_Units).Value, _Units);
        }

        /// <summary>Subtracts the specified area from the current instance.</summary>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <remarks>
        /// This method will subtract any <strong>Area</strong> object from the current
        /// instance. If the unit type of the <strong>Value</strong> parameter does not match that
        /// of the current instance, the value is converted to the unit type of the current
        /// instance before subtracting.
        /// </remarks>
        /// <example>
        ///     This example demonstrates how two areas of different unit types can be safely
        ///     subtracted. A value of 144 square inches (which is the same as one square foot) is
        ///     subtracted from one square foot, producing a result of zero. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(1, AreaUnit.SquareFeet)
        /// Dim Area2 As New Area(144, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Subtract(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 0 square feet
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(1, AreaUnit.SquareFeet);
        /// Area Area2 = new Area(144, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Subtract(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 0 square feet
        ///     </code>
        /// </example>
        public Area Subtract(Area value)
        {
            return new Area(_Value - value.ToUnitType(_Units).Value, _Units);
        }

        /// <remarks>
        /// This method will multiply any <strong>Area</strong> object from the current
        /// instance. If the unit type of the <strong>Value</strong> parameter does not match that
        /// of the current instance, the value is converted to the unit type of the current
        /// instance before multiplication.
        /// </remarks>
        /// <summary>Multiplies the specified area with the current instance.</summary>
        /// <returns>
        /// A new <strong>Area</strong> structure containing the product of the two
        /// values.
        /// </returns>
        /// <example>
        ///     This example demonstrates how two areas can be multiplied together. A value of 50
        ///     square inches is multiplied by two square inches, producing a result of 100 square
        ///     inches. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(50, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(2, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Multiply(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 100 square inches
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(50, AreaUnit.SquareInches);
        /// Area Area2 = new Area(2, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Multiply(Area2);
        /// Console.WriteLine(Area3.ToString());
        /// // Output: 100 square inches
        ///     </code>
        /// </example>
        public Area Multiply(Area value)
        {
            return new Area(_Value * value.ToUnitType(_Units).Value, _Units);
        }

        /// <remarks>
        /// This method will devide the current instance by any <strong>Area</strong> object.
        /// If the unit type of the <strong>Value</strong> parameter does not match that of the
        /// current instance, the value is converted to the unit type of the current instance
        /// before devision.
        /// </remarks>
        /// <summary>Divides the current instance by the specified area.</summary>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        ///     This example demonstrates how two areas can be divided. A value of 100 square
        ///     inches is divided by two square inches, producing a result of 50 square inches. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Area1 As New Area(100, AreaUnit.SquareInches)
        /// Dim Area2 As New Area(2, AreaUnit.SquareInches)
        /// Dim Area3 As Area = Area1.Divide(Area2)
        /// Debug.WriteLine(Area3.ToString())
        /// ' Output: 50 square inches
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area Area1 = new Area(100, AreaUnit.SquareInches);
        /// Area Area2 = new Area(2, AreaUnit.SquareInches);
        /// Area Area3 = Area1.Divide(Area2);
        /// Debug.WriteLine(Area3.ToString());
        /// // Output: 50 square inches
        ///     </code>
        /// </example>
        public Area Divide(Area value)
        {
            return new Area(_Value / value.ToUnitType(_Units).Value, _Units);
        }

        /// <summary>Returns the current instance increased by one.</summary>
        /// <remarks>
        /// 	<para>This method increases the <strong>Value</strong> property by 1.0, returned as
        ///     a new instance. The <strong>Units</strong> property is preserved.</para>
        /// 	<para><font color="red"><font color="red">NOTE: Since the <strong>Area</strong>
        ///     class is immutable, this method will not modify the current
        ///     instance.</font></font></para>
        /// </remarks>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        ///     This example uses the <strong>Increment</strong> method to increase an area's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Increment</strong> is called while ignoring the return value. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1 = Area1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1.Increment()
        /// ' NOTE: Area1 will still be 1 square meter, not 2!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1 = Area1.Increment();
        ///  
        /// // Incorrect use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1.Increment();
        /// // NOTE: Area1 will still be 1 square meter, not 2!
        ///     </code>
        /// </example>
        public Area Increment()
        {
            return new Area(_Value + 1.0, _Units);
        }

        /// <remarks>
        /// 	<para>This method decreases the <strong>Value</strong> property by 1.0, returned as
        ///     a new instance. The <strong>Units</strong> property is preserved.</para>
        /// 	<para><font color="red">NOTE: Since the <strong>Area</strong> class is immutable,
        ///     this method will not modify the current instance.</font></para>
        /// </remarks>
        /// <summary>Returns the current instance decreased by one.</summary>
        /// <returns>A new <strong>Area</strong> structure containing the new value.</returns>
        /// <example>
        ///     This example uses the <strong>Decrement</strong> method to decrease an area's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Decrement</strong> is called while ignoring the return value. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1 = Area1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Area1 As New Area(1, AreaUnit.SquareMeters)
        /// Area1.Increment()
        /// ' NOTE: Area1 will still be 1 square meter, not 0!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1 = Area1.Decrement();
        ///  
        /// // Incorrect use of Increment
        /// Area Area1 = new Area(1, AreaUnit.SquareMeters);
        /// Area1.Decrement();
        /// // NOTE: Area1 will still be 1 square meter, not 0!
        ///     </code>
        /// </example>
        public Area Decrement()
        {
            return new Area(_Value - 1.0, _Units);
        }

        /// <summary>Indicates if the current instance is smaller than the specified value.</summary>
        /// <remarks>
        /// If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.
        /// </remarks>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the <strong>Value</strong> parameter.
        /// </returns>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        public bool IsLessThan(Area value)
        {
            return CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates if the current instance is smaller than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the <strong>Value</strong> parameter.
        /// </returns>
        /// <remarks>
        /// If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.
        /// </remarks>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        public bool IsLessThanOrEqualTo(Area value)
        {
            return CompareTo(value) < 0 || Equals(value);
        }

        /// <summary>Indicates if the current instance is larger than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// larger than the <strong>Value</strong> parameter.
        /// </returns>
        /// <remarks>
        /// If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.
        /// </remarks>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        public bool IsGreaterThan(Area value)
        {
            return CompareTo(value) > 0;
        }

        /// <summary>
        /// Indicates if the current instance is larger than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// larger than or equal to the <strong>Value</strong> parameter.
        /// </returns>
        /// <remarks>
        /// If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.
        /// </remarks>
        /// <param name="value">An <strong>Area</strong> to compare with the current instance.</param>
        public bool IsGreaterThanOrEqualTo(Area value)
        {
            return CompareTo(value) > 0 || Equals(value);
        }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>Creates a new instance using the specified string.</summary>
        /// <remarks>
        /// This powerful method is typically used to convert a string-based Area
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Area</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.
        /// </remarks>
        /// <returns>
        /// A new Area object containing the parsed <see cref="Value">value</see> and
        /// <see cref="Units">unit</see> type.
        /// </returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">Parse method requires a valid Area measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">1. The numeric portion of the Area measurement was not recognized.<br/>
        /// 2. The Area unit type was not recognized or not specified.</exception>
        /// <example>
        ///     This example demonstrates how the Parse method can convert several string formats
        ///     into a Area object. 
        ///     <code lang="VB">
        /// Dim NewArea As Area
        /// ' Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km")
        /// ' Create a Area of 14,387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14,387 statute miles").ToSquareInches()
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ")
        ///     </code>
        /// 	<code lang="CS">
        /// Area NewArea;
        /// // Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km");
        /// // Create a Area of 14,387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14,387 statute miles").ToInches();
        /// // Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ");
        ///     </code>
        /// </example>
        public static Area Parse(string value)
        {
            return new Area(value, CultureInfo.CurrentCulture);
        }

        /// <summary>Creates a new instance using the specified string and culture.</summary>
        /// <remarks>
        /// This powerful method is typically used to convert a string-based Area
        /// measurement, such as one entered by a user or read from a file, into a
        /// <strong>Area</strong> object. This method will accept any output created via the
        /// <see cref="ToString()">ToString</see> method.
        /// </remarks>
        /// <example>
        /// 	<code lang="VB" title="[New Example]">
        /// Dim NewArea As Area
        /// ' Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km", CultureInfo.CurrentCulture)
        /// ' Create a Area of 14,387 miles, then convert it into inches
        /// NewArea = Area.Parse("14,387 statute miles", CultureInfo.CurrentCulture).ToSquareInches()
        /// ' Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ", CultureInfo.CurrentCulture)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Area NewArea;
        /// // Create a Area of 50 kilometers
        /// NewArea = Area.Parse("50 km", CultureInfo.CurrentCulture);
        /// // Create a Area of 14,387 miles, then convert it into square inches
        /// NewArea = Area.Parse("14,387 statute miles", CultureInfo.CurrentCulture).ToInches();
        /// // Parse an untrimmed measurement into 50 feet
        /// NewArea = Area.Parse("    50 '       ", CultureInfo.CurrentCulture);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>String</strong> describing an area measurement.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object specifying which numeric and text formats to use during parsing.</param>
        public static Area Parse(string value, CultureInfo culture)
        {
            return new Area(value, culture);
        }

        /// <summary>Returns a random distance between 0 and 1,000 square meters.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        public static Area Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>Returns a random distance between 0 and 1,000 square meters.</summary>
        /// <returns>A <strong>Distance</strong> containing a random value, converted to local units.</returns>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        public static Area Random(Random generator)
        {
            return new Area(generator.NextDouble() * 1000, AreaUnit.SquareMeters).ToLocalUnitType();
        }

        #endregion

        #region Overrides

        /// <summary>Compares the current instance with the specified object.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the two objects have the
        /// same value.
        /// </returns>
        /// <param name="obj">An <strong>Area</strong> object to compare with.</param>
        public override bool Equals(object obj)
        {
            // If the type is the same, compare the values
            if (obj is Area)
                return Equals((Area)obj);

            // Not equal
            return false;
        }

        /// <summary>Returns a unique code for the current instance.</summary>
        /// <returns>
        /// An <strong>Integer</strong> representing a unique code for the current
        /// instance.
        /// </returns>
        /// <remarks>
        /// Since the <strong>Area</strong> class is immutable, this property may be used
        /// safely with hash tables.
        /// </remarks>
        public override int GetHashCode()
        {
            return ToSquareMeters().Value.GetHashCode();
        }

        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the current Area in the default format.</returns>
        /// <remarks>
        /// The default format used is "<strong>v uu</strong>" where <strong>v</strong>
        /// represents the numerical portion of the area and <strong>uu</strong> is the unit
        /// type.
        /// </remarks>
        /// <example>
        ///     This example uses the ToString method to populate a TextBox with a Area
        ///     measurement. 
        ///     <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString())
        /// ' Output: 75 sq. statute miles
        ///     </code>
        /// 	<code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea = nre Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString());
        /// // Output: 75 sq. statute miles
        ///     </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion

        #region Operators

        public static Area operator +(Area left, Area right) 
		{
			return left.Add(right);
		}

		public static Area operator -(Area left, Area right) 
		{
			return left.Subtract(right);
		}

		public static Area operator *(Area left, Area right) 
		{
			return left.Multiply(right);
		}

		public static Area operator /(Area left, Area right) 
		{
			return left.Divide(right);
		}

		public static bool operator <(Area left, Area right) 
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(Area left, Area right) 
		{
			return left.CompareTo(right) < 0 || left.Equals(right);
		}

		public static bool operator ==(Area left, Area right) 
		{
			return left.Equals(right);
		}

		public static bool operator !=(Area left, Area right) 
		{
			return !(left == right);
		}

		public static bool operator >=(Area left, Area right) 
		{
			return left.CompareTo(right) > 0 || left.Equals(right);
		}
	
		public static bool operator >(Area left, Area right) 
		{
			return left.CompareTo(right) > 0;
		}

		#endregion

		#region Conversions

		public static explicit operator Area(string value) 
		{
			return Area.Parse(value, CultureInfo.CurrentCulture);
		}

		public static explicit operator string(Area value)
		{
			return value.ToString("g", CultureInfo.CurrentCulture);
		}

		#endregion

        #region IComparable<Area> Members

        /// <summary>Compares the current instance to the specified area.</summary>
        /// <returns>
        /// An <strong>Integer</strong>: 0 if the object's values are equivalent, -1 if the
        /// current instance is smaller, or 1 if the current instance is larger.
        /// </returns>
        /// <remarks>
        /// If the <strong>Value</strong> parameter's unit type does not match the current
        /// instance, it will be converted to the current instance's unit type before performing
        /// the comparison.
        /// </remarks>
        /// <param name="other">An <strong>Area</strong> object to compare with.</param>
        public int CompareTo(Area other)
        {
            return _Value.CompareTo(other.ToUnitType(_Units).Value);
        }

        #endregion

        #region IEquatable<Area> Members

        /// <summary>
        /// Compares the current instance to the specified <strong>Area</strong>
        /// object.
        /// </summary>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks>
        /// 	<para>This method will compare the <em>value</em> of the current instance against
        ///     the <strong>Value</strong> parameter. If the <strong>Value</strong> parameter's
        ///     unit type does not match the current instance, it will be converted to the current
        ///     instance's unit type before performing the comparison.</para>
        /// 	<para><em>NOTE: This method compares objects by value, not by
        ///     reference.</em></para>
        /// </remarks>
        /// <param name="value">A <strong>Area</strong> object to compare with.</param>
        public bool Equals(Area value)
        {
            return _Value == value.ToUnitType(Units).Value;
        }

        /// <summary>
        /// Compares the current instance to the specified <strong>Area</strong>
        /// object.
        /// </summary>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        /// <remarks>
        /// 	<para>This method will compare the <em>value</em> of the current instance against
        ///     the <strong>Value</strong> parameter. If the <strong>Value</strong> parameter's
        ///     unit type does not match the current instance, it will be converted to the current
        ///     instance's unit type before performing the comparison.</para>
        /// 	<para><em>NOTE: This method compares objects by value, not by
        ///     reference.</em></para>
        /// </remarks>
        /// <param name="value">A <strong>Area</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>integer</strong> specifies the precision for the comparison.</param>
        public bool Equals(Area value, int decimals)
        {
            return Math.Round(Value, decimals) == Math.Round(value.ToUnitType(Units).Value, decimals);
        }       

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and local culture.
        /// </summary>
        /// <returns>A <strong>String</strong> containing the Area in the specified format.</returns>
        /// <remarks>This method allows a custom format to be applied to the ToString method.  Numeric formats
        /// will be adjusted to the machine's local UI culture.</remarks>
        /// <example>
        ///     This example uses the ToString method to populate a TextBox with a Area measurement
        ///     using a custom format and culture information. 
        ///     <code lang="VB">
        /// ' Declare a area of 75 square statute miles
        /// Dim MyArea As New Area(75, AreaUnit.SquareStatuteMiles)
        /// ' Output the result using the default format
        /// Debug.WriteLine(MyArea.ToString("v.v uuu", CultureInfo.CurrentCulture))
        /// ' Output: 75.0 square statute miles
        ///     </code>
        /// 	<code lang="CS">
        /// // Declare a area of 75 square statute miles
        /// Area MyArea As New Area(75, AreaUnit.SquareStatuteMiles);
        /// // Output the result using the default format
        /// Console.WriteLine(MyArea.ToString("v.v uuu", CultureInfo.CurrentCulture));
        /// // Output: 75.0 square statute miles
        ///     </code>
        /// </example>
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
                            switch (_Units)
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
                            switch (_Units)
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
                                switch (_Units)
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
                                switch (_Units)
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

            _Units = (AreaUnit)Enum.Parse(typeof(AreaUnit), reader.ReadElementContentAsString(), true);
            _Value = reader.ReadElementContentAsDouble();
        }

        #endregion
    }

	/// <summary>Indicates the unit of measure for area measurements.</summary>
	/// <remarks>
	/// This enumeration is most frequently used by the Units property of the Area
	/// structure to describe an area measurement.
	/// </remarks>
	/// <seealso cref="Area.Value">Value Property (Area Class)</seealso>
	/// <seealso cref="Area.Units">Units Property (Area Class)</seealso>
	/// <example>
	///     This example uses the <strong>AreaUnit</strong> enumeration to create a new
	///     <strong>Area</strong> object.
	///     <code lang="VB" title="[New Example]">
	/// Dim Area1 As New Area(1, AreaUnit.SquareKilometers)
	///     </code>
	/// 	<code lang="CS" title="[New Example]">
	/// Area Area1 = new Area(1, AreaUnit.SquareKilometers);
	///     </code>
	/// </example>
	public enum AreaUnit: int
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
