using System;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
	/// <summary>Represents a vertical angular measurement between -90° and 90°.</summary>
	/// <remarks>
	/// This class is used to indicate a vertical angle where 90° represents a point
	/// directly overhead, 0° represents the horizon (striaght ahead), and -90° represents a
	/// point straight down. This class is typically combined with an <strong>Elevation</strong>
	/// object (which measures a horizontal angle) to form a three-dimensional direction to an
	/// object in space, such as a GPS satellite.
	/// </remarks>
	/// <example>
	/// These examples create new instances of <strong>Elevation</strong> objects.
	/// </example>
	/// <seealso cref="Angle">Angle Class</seealso>
	/// <seealso cref="Azimuth">Azimuth Class</seealso>
	/// <seealso cref="Latitude">Latitude Class</seealso>
	/// <seealso cref="Longitude">Longitude Class</seealso>
	/// <example>
	///     These examples create new instances of Elevation objects. 
	///     <code lang="VB" description="Create an angle of 90°">
	/// Dim MyElevation As New Elevation(90)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 90°">
	/// Elevation MyElevation = new Elevation(90);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 90°">
	/// Elevation MyElevation = new Elevation(90);
	///     </code>
	/// 	<code lang="VB" description="Create an angle of 105°30'21.4">
	/// Dim MyElevation1 As New Elevation(105, 30, 21.4)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 105°30'21.4">
	/// Elevation MyElevation = new Elevation(105, 30, 21.4);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 105°30'21.4">
	/// Elevation MyElevation = new Elevation(105, 30, 21.4);
	///     </code>
	/// </example>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.ElevationConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Elevation : IFormattable, IComparable<Elevation>, IEquatable<Elevation>, ICloneable<Elevation>, IXmlSerializable
    {
        private double _DecimalDegrees;

        #region Constants

        private const int MaximumPrecisionDigits = 13;

        #endregion

        #region Fields

        /// <summary>Represents the minimum value of an angle in one turn of a circle.</summary>
		/// <remarks>
		/// This member is typically used for looping through the entire range of possible
		/// angles. It is possible to create angular values below this value, such as -720°.
		/// </remarks>
		/// <example>
		///     This example creates an angle representing the minimum allowed value. 
		///     <code lang="VB">
		/// Dim MyElevation As Elevation = Elevation.Minimum
		///     </code>
		/// 	<code lang="CS">
		/// Elevation MyElevation = Elevation.Minimum;
		///     </code>
		/// 	<code lang="C++">
		/// Elevation MyElevation = Elevation.Minimum;
		///     </code>
		/// </example>
		/// <value>An Elevation with a value of -359.999999°.</value>
		public static readonly Elevation Minimum = new Elevation(-90);
		/// <summary>Represents an angle with no value.</summary>
		/// <remarks>
		/// This member is typically used to initialize an angle variable to zero. When an
		/// angle has a value of zero, its <see cref="IsEmpty">IsEmpty</see> property returns
		/// <strong>True</strong>.
		/// </remarks>
		/// <value>An Elevation containing a value of zero (0°).</value>
		/// <seealso cref="IsEmpty">IsEmpty Property</seealso>
		public static readonly Elevation Empty = new Elevation(0.0);
		/// <summary>
		/// Represents an angle with infinite value.
		/// </summary>
		/// <remarks>
		/// In some cases, the result of an angular calculation may be infinity. This member
		/// is used in such cases. The <see cref="DecimalDegrees">DecimalDegrees</see> property is
		/// set to Double.PositiveInfinity.
		/// </remarks>
		public static readonly Elevation Infinity = new Elevation(double.PositiveInfinity);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Elevation Invalid = new Elevation(double.NaN);
		/// <summary>Represents the maximum value of an angle in one turn of a circle.</summary>
		/// <remarks>
		/// This member is typically used for looping through the entire range of possible
		/// angles, or to test the range of a value. It is possible to create angular values below
		/// this value, such as 720°.
		/// </remarks>
		/// <example>
		///     This example creates an angle representing the maximum allowed value of 359.9999°. 
		///     <code lang="VB">
		/// Dim MyElevation As Elevation = Elevation.Maximum
		///     </code>
		/// 	<code lang="CS">
		/// Elevation MyElevation = Elevation.Maximum;
		///     </code>
		/// </example>
		public static readonly Elevation Maximum = new Elevation(90.0);
		/// <summary>Represents the point directly overhead.</summary>
		/// <value>An <strong>Elevation</strong> object.</value>
		public static readonly Elevation Zenith = new Elevation(90.0);
		/// <value>An <strong>Elevation</strong> object.</value>
		/// <summary>Represents a vertical direction halfway up from the horizon.</summary>
		public static readonly Elevation HalfwayAboveHorizon = new Elevation(45.0);
		/// <value>An <strong>Elevation</strong> object.</value>
		/// <summary>Represents a vertical direction halfway below the horizon.</summary>
		public static readonly Elevation HalfwayBelowHorizon = new Elevation(-45.0);
		/// <value>An <strong>Elevation</strong> object.</value>
		public static readonly Elevation Horizon = new Elevation(0.0);
		/// <summary>Represents the point directly below one's feet.</summary>
		/// <value>An <strong>Elevation</strong> object.</value>
		public static readonly Elevation Nadir = new Elevation(-90.0);

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance with the specified decimal degrees.</summary>
        /// <example>
        ///     This example demonstrates how to create an angle with a measurement of 90°. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        public Elevation(double decimalDegrees)
        {
            // Set the decimal degrees value
            _DecimalDegrees = decimalDegrees;
        }

        /// <summary>Creates a new instance with the specified degrees.</summary>
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        /// <param name="hours">
        /// An <strong>Integer</strong> indicating the amount of degrees, typically between 0
        /// and 360.
        /// </param>
        public Elevation(int hours)
        {
            _DecimalDegrees = ToDecimalDegrees(hours);
        }

        /// <summary>Creates a new instance with the specified hours and decimal minutes.</summary>
        /// <example>
        ///     This example demonstrates how an angle can be created when only the hours and
        ///     minutes (in decimal form) are known. This creates a value of 12°42.345'. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(12, 42.345)
        ///     </code>
        /// 	<code lang="VB">
        /// Elevation MyElevation = new Elevation(12, 42.345);
        ///     </code>
        /// </example>
        /// <remarks>An <strong>Elevation</strong> containing the specified value.</remarks>
        public Elevation(int hours, double decimalMinutes)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>Creates a new instance with the specified hours, minutes and 
        /// seconds.</summary>
        /// <example>
        ///     This example demonstrates how to create an angular measurement of 34°12'29.2 in
        ///     hours, minutes and seconds. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(34, 12, 29.2)
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(34, 12, 29.2);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        public Elevation(int hours, int minutes, double seconds)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>Creates a new instance by converting the specified string.</summary>
        /// <remarks>
        /// This constructor parses the specified string into an <strong>Elevation</strong>
        /// object using the current culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.
        /// </remarks>
        /// <seealso cref="Elevation.Parse(string, CultureInfo)">Parse Method</seealso>
        /// <example>
        ///     This example creates a new instance by parsing a string. (NOTE: The double-quote is
        ///     doubled up to represent a single double-quote in the string.) 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation("123°45'67.8""")
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation("123°45'67.8\"");
        ///     </code>
        /// </example>
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Elevation(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <remarks>
        /// This constructor parses the specified string into an <strong>Elevation</strong>
        /// object using a specific culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.
        /// </remarks>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <summary>
        /// Creates a new instance by converting the specified string using the specified
        /// culture.
        /// </summary>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public Elevation(string value, CultureInfo culture)
        {
            // Is the value null or empty?
            if (value == null || value.Length == 0)
            {
                // Yes. Set to zero
                _DecimalDegrees = 0;
                return;
            }

            // Default to the current culture
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            // Yes. First, clean up the strings
            try
            {
                // Clean up the string
                StringBuilder NewValue = new StringBuilder(value);
                NewValue.Replace("°", " ").Replace("'", " ").Replace("\"", " ").Replace("  ", " ");
                // Now split the values into an array
                string[] Values = NewValue.ToString().Trim().Split(' ');
                // How many elements are in the array?
                switch (Values.Length)
                {
                    case 0:
                        // Return a blank Elevation
                        _DecimalDegrees = 0.0;
                        return;
                    case 1: // Decimal degrees
                        // Is it infinity?
                        if (String.Compare(Values[0], Properties.Resources.Common_Infinity, true, culture) == 0)
                        {
                            _DecimalDegrees = double.PositiveInfinity;
                            return;
                        }
                        // Is it empty?
                        else if (String.Compare(Values[0], Properties.Resources.Common_Empty, true, culture) == 0)
                        {
                            _DecimalDegrees = 0.0;
                            return;
                        }
                        // Look at the number of digits, this might be HHHMMSS format.
                        else if (Values[0].Length == 7 && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 3), culture),
                                int.Parse(Values[0].Substring(3, 2), culture),
                                double.Parse(Values[0].Substring(5, 2), culture));
                            return;
                        }
                        else if (Values[0].Length == 8 && Values[0][0] == '-' && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 4), culture),
                                int.Parse(Values[0].Substring(4, 2), culture),
                                double.Parse(Values[0].Substring(6, 2), culture));
                            return;
                        }
                        else
                        {
                            _DecimalDegrees = double.Parse(Values[0], culture);
                            return;
                        }
                    case 2: // Hours and decimal minutes
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            float.Parse(Values[1], culture));
                        return;
                    default: // Hours, minutes and seconds  (most likely)
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1 || Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            int.Parse(Values[1], culture),
                            double.Parse(Values[2], culture));
                        return;
                }
            }
            catch (Exception ex)
            {
#if PocketPC
                throw new ArgumentException(Properties.Resources.Angle_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Angle_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader"></param>
        public Elevation(XmlReader reader)
        {
            // Initialize all fields
            _DecimalDegrees = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the value of the angle as decimal degrees.</summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>This property returns the value of the angle as a single number.</remarks>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <example>
        ///     This example demonstrates how the
        ///     <see cref="DecimalDegrees"><strong>DecimalDegrees</strong></see> property is
        ///     calculated automatically when creating an angle using hours, minutes and seconds. 
        ///     <code lang="VB">
        /// ' Create an angle of 20°30'
        /// Dim MyElevation As New Elevation(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties 
        /// Debug.WriteLine(MyElevation.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°30'
        /// Elevation MyElevation = New Elevation(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties 
        /// Console.WriteLine(MyElevation.DecimalDegrees)
        /// // Output: "20.5"  the same as 20°30'
        ///     </code>
        /// </example>
        public double DecimalDegrees
        {
            get
            {
                return _DecimalDegrees;
            }
        }

        /// <summary>Returns the minutes and seconds as a single numeric value.</summary>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="DecimalDegrees">DecimalDegrees Property</seealso>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>
        /// This property is used when minutes and seconds are represented as a single
        /// decimal value.
        /// </remarks>
        /// <example>
        ///     This example demonstrates how the <strong>DecimalMinutes</strong> property is
        ///     automatically calculated when creating a new angle. 
        ///     <code lang="VB">
        /// ' Create an angle of 20°10'30"
        /// Dim MyElevation As New Elevation(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyElevation.DecimalMinutes)
        /// ' Output: "10.5"
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Elevation MyElevation = new Elevation(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyElevation.DecimalMinutes)
        /// // Output: "10.5"
        ///     </code>
        /// </example>
        public double DecimalMinutes
        {
            get
            {
#if Framework20 && !PocketPC
                return Math.Round(
                    (Math.Abs(
                        _DecimalDegrees - Math.Truncate(_DecimalDegrees)) * 60.0),
                    // Apparently we must round to two less places to preserve accuracy
                        MaximumPrecisionDigits - 2);
#else
                return Math.Round(
                    (Math.Abs(
                        _DecimalDegrees - Truncate(_DecimalDegrees)) * 60.0), MaximumPrecisionDigits - 2);
#endif
            }
        }

        /// <summary>Returns the integer hours (degrees) portion of an angular 
        /// measurement.</summary>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <value>An <strong>Integer</strong> value.</value>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Minutes">Minutes</see>
        /// and <see cref="Seconds">Seconds</see> properties to create a full angular measurement.
        /// This property is the same as <strong>DecimalDegrees</strong> without any fractional
        /// value.
        /// </remarks>
        /// <example>
        ///     This example creates an angle of 60.5° then outputs the value of the
        ///     <strong>Hours</strong> property, 60. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(60.5)
        /// Debug.WriteLine(MyElevation.Hours)
        /// ' Output: 60
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(60.5);
        /// Console.WriteLine(MyElevation.Hours);
        /// // Output: 60
        ///     </code>
        /// </example>
        public int Hours
        {
            get
            {
#if Framework20 && !PocketPC
                return (int)Math.Truncate(_DecimalDegrees);
#else
				return Truncate(_DecimalDegrees);
#endif
            }
        }

        /// <summary>Returns the integer minutes portion of an angular measurement.</summary>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Seconds">Seconds</see> properties to create a sexagesimal
        /// measurement.
        /// </remarks>
        /// <value>An <strong>Integer</strong>.</value>
        /// <example>
        ///     This example creates an angle of 45.5° then outputs the value of the
        ///     <strong>Minutes</strong> property, 30. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(45.5)
        /// Debug.WriteLine(MyElevation.Minutes)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(45.5);
        /// Console.WriteLine(MyElevation.Minutes);
        /// // Output: 30
        ///     </code>
        /// </example>
        public int Minutes
        {
            get
            {
                return Convert.ToInt32(
                    Math.Abs(
#if Framework20 && !PocketPC
Math.Truncate(
#else
                        Truncate(
#endif
Math.Round(
                    // Calculations appear to support one less digit than the maximum allowed precision
                                (_DecimalDegrees - Hours) * 60.0, MaximumPrecisionDigits - 1))));
            }
        }


        /// <summary>Returns the seconds minutes portion of an angular measurement.</summary>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Minutes">Minutes</see> properties to create a sexagesimal
        /// measurement.
        /// </remarks>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <value>A <strong>Double</strong> value.</value>
        /// <example>
        ///     This example creates an angle of 45°10.5' then outputs the value of the
        ///     <strong>Seconds</strong> property, 30. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 10.5)
        /// Debug.WriteLine(MyElevation.Seconds)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyElevation As New Elevation(45, 10.5);
        /// Console.WriteLine(MyElevation.Seconds);
        /// // Output: 30
        ///     </code>
        /// </example>
        public double Seconds
        {
            get
            {
                return Math.Round(
                                (Math.Abs(_DecimalDegrees - Hours) * 60.0 - Minutes) * 60.0,
                    // This property appears to support one less digit than the maximum allowed
                                MaximumPrecisionDigits - 4);
            }
        }

        /// <summary>Indicates if the current instance has a non-zero value.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property is zero.
        /// </value>
        /// <seealso cref="Empty">Empty Field</seealso>
        public bool IsEmpty
        {
            get
            {
                return (_DecimalDegrees == 0);
            }
        }

        /// <summary>Indicates if the current instance represents an infinite value.</summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_DecimalDegrees);
            }
        }

        /// <summary>
        /// Indicates if the current instance is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_DecimalDegrees); }
        }

        /// <summary>Indicates whether the current instance has been normalized and is within the 
        /// allowed bounds of -90° and 90°.</summary>
        public bool IsNormalized
        {
            get { return _DecimalDegrees >= -90 && _DecimalDegrees <= 90; }
        }

        #endregion

        #region Public Methods

        /// <overloads>Converts a measurement to its equivalent value between -90 and 
        /// 90 degrees.</overloads>
        public Elevation Normalize()
        {
            // Is the value not a number, infinity, or already normalized?
            if (double.IsInfinity(_DecimalDegrees)
                || double.IsNaN(_DecimalDegrees)
                || IsNormalized)
                return this;

            // Calculate the number of times the degree value winds completely 
            // through a hemisphere
            int HemisphereFlips = Convert.ToInt32(Math.Floor(_DecimalDegrees / 180.0));

            // If the value is in the southern hemisphere, apply another flip
            if (_DecimalDegrees < 0)
                HemisphereFlips++;

            // Calculate the new value
            double NewValue = _DecimalDegrees % 180;

            // if the value is > 90, return 180 - X
            if (NewValue > 90)
                NewValue = 180 - NewValue;

            // If the value id < -180, return -180 - X
            else if (NewValue < -90.0)
                NewValue = -180.0 - NewValue;

            // Account for flips around hemispheres by flipping the sign
            if (HemisphereFlips % 2 != 0)
                return new Elevation(-NewValue);
            else
                return new Elevation(NewValue);
        }

        /// <summary>Returns the smallest integer greater than the specified value.</summary>
        public Elevation Ceiling()
        {
            return new Elevation(Math.Ceiling(_DecimalDegrees));
        }

        /// <summary>Returns the largest integer which is smaller than the specified value.</summary>
        public Elevation Floor()
        {
            return new Elevation(Math.Floor(_DecimalDegrees));
        }

#if !Framework20 || PocketPC
		internal static int Truncate(double value)
		{
			return value > 0 
				? (int)(value - (value - Math.Floor(value))) 
				: (int)(value - (value - Math.Ceiling(value)));
		}
#endif


        /// <summary>
        /// Returns a new instance whose value is rounded the specified number of decimals.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of decimals to round off to.</param>
        /// <returns></returns>
        public Elevation Round(int decimals)
        {
            return new Elevation(Math.Round(_DecimalDegrees, decimals));
        }

        /// <summary>Returns a new instance whose Seconds property is evenly divisible by 15.</summary>
        /// <returns>An <strong>Elevation</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        public Elevation RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        /// <param name="interval">
        /// A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.
        /// </param>
        public Elevation RoundSeconds(double interval)
        {
            // Interval must be > 0
            if (interval == 0)
#if PocketPC
                throw new ArgumentOutOfRangeException(Properties.Resources.Angle_InvalidInterval);
#else
                throw new ArgumentOutOfRangeException("interval", interval, Properties.Resources.Angle_InvalidInterval);
#endif
            // Get the amount in seconds
            double NewSeconds = Seconds;
            //double HalfInterval = interval * 0.5;
            // Loop through all intervals to find the right rounding
            for (double value = 0; value < 60; value += interval)
            {
                // Calculate the value of the next interval
                double NextInterval = value + interval;
                // Is the seconds value greater than the next interval?
                if (NewSeconds > NextInterval)
                    // Yes.  Continue on
                    continue;
                // Is the seconds value closer to the current or next interval?
                if (NewSeconds < (value + NextInterval) * 0.5)
                    // Closer to the current interval, so adjust it
                    NewSeconds = value;
                else
                    NewSeconds = NextInterval;
                // Is the new value 60?  If so, make it zero
                if (NewSeconds == 60)
                    NewSeconds = 0;
                // Return the new value
                return new Elevation(Hours, Minutes, NewSeconds);
            }
            // return the new value
            return new Elevation(Hours, Minutes, NewSeconds);
        }

        /// <summary>Returns the object with the smallest value.</summary>
        /// <returns>The <strong>Elevation</strong> containing the smallest value.</returns>
        /// <param name="value">An <strong>Elevation</strong> object to compare to the current instance.</param>
        public Elevation LesserOf(Elevation value)
        {
            if (_DecimalDegrees < value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <returns>An <strong>Elevation</strong> containing the largest value.</returns>
        /// <summary>Returns the object with the largest value.</summary>
        /// <param name="value">An <strong>Elevation</strong> object to compare to the current instance.</param>
        public Elevation GreaterOf(Elevation value)
        {
            if (_DecimalDegrees > value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <summary>Returns an angle opposite of the current instance.</summary>
        /// <returns>An <strong>Elevation</strong> representing the mirrored value.</returns>
        /// <remarks>
        /// This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Elevation</strong> of 45° then calculates its mirror
        ///     of 225°. (45 + 180) 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(45)
        /// Dim Elevation2 As Elevation = Elevation1.Mirror()
        /// Debug.WriteLine(Elevation2.ToString())
        /// ' Output: 225
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(45);
        /// Elevation Elevation2 = Elevation1.Mirror();
        /// Console.WriteLine(Elevation2.ToString());
        /// // Output: 225
        ///     </code>
        /// </example>
        public Elevation Mirror()
        {
            return new Elevation(_DecimalDegrees + 180.0).Normalize();
        }

        /// <summary>Converts the current instance into radians.</summary>
        /// <returns>A <see cref="Radian">Radian</see> object.</returns>
        /// <remarks>
        /// 	<para>This function is typically used to convert an angular measurement into
        ///  radians before performing a trigonometric function.
        /// 		</para>
        /// </remarks>
        /// <seealso cref="Radian">Radian Class</seealso>
        /// <overloads>Converts an angular measurement into radians before further processing.</overloads>
        /// <example>
        ///     This example converts a measurement of 90° into radians. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        /// Dim MyRadians As Radian = MyElevation.ToRadians()
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        /// Radian MyRadians = MyElevation.ToRadians();
        ///     </code>
        /// </example>
        public Radian ToRadians()
        {
            return Radian.FromDegrees(_DecimalDegrees);
        }

        /// <summary>Outputs the angle as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "d.dddd°" is used. Any
        ///     string output by this method can be converted back into an Elevation object using the
        ///     <strong>Parse</strong> method or <strong>Elevation(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString(string, IFormatProvider)">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 16.772)
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyElevation As New Elevation(45, 16.772);
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///     </code>
        /// </example>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        /// <summary>Compares the current value to another Elevation object's value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.
        /// </returns>
        /// <remarks>This </remarks>
        /// <param name="obj">
        /// An <strong>Elevation</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.
        /// </param>
        public override bool Equals(object obj)
        {
#if !PocketPC || Framework20
            // ?
            if (Object.ReferenceEquals(obj, null))
                return false;
#endif
            // Convert objects to an Elevation as needed before comparison
            if (obj is Elevation || obj is double || obj is string || obj is int || obj is float)
                return _DecimalDegrees.Equals(((Elevation)obj).DecimalDegrees);

            // Nothing else will work, so False
            return false;
        }

        /// <summary>Returns a unique code for this instance.</summary>
        /// <remarks>
        /// Since the <strong>Elevation</strong> class is immutable, this property may be used
        /// safely with hash tables.
        /// </remarks>
        /// <returns>
        /// An <strong>Integer</strong> representing a unique code for the current
        /// instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _DecimalDegrees.GetHashCode();
        }

        /// <summary>Outputs the angle as a string using the default format.</summary>
        /// <returns><para>A <strong>String</strong> created using the default format.</para></returns>
        /// <remarks>
        /// 	<para>This method formats the current instance using the default format of
        ///     "d.dddd°." Any string output by this method can be converted back into an Elevation
        ///     object using the <strong>Parse</strong> method or <strong>Elevation(string)</strong>
        ///     constructor.</para>
        /// </remarks>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example outputs a value of 90 degrees in the default format of ###.#°. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        /// Debug.WriteLine(MyElevation.ToString)
        /// ' Output: "90°"
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        /// Debug.WriteLine(MyElevation.ToString());
        /// // Output: "90°"
        ///     </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods


        /// <overloads>Converts a measurement to its equivalent value between -90 and 
        /// 90 degrees.</overloads>
        public static Elevation Normalize(double decimalDegrees)
        {
            return new Elevation(decimalDegrees).Normalize();
        }

        /// <summary>Converts arbitrary hour, minute and seconds into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal 
        /// places, the maximum precision allowed by this type.
        /// </remarks>
        /// <seealso cref="Latitude.DecimalDegrees">DecimalDegrees Property</seealso>
        /// <seealso cref="Latitude.Normalize()">Normalize Method</seealso>
        /// <example>
        ///     This example converts a value of 10°30'0" into decimal degrees (10.5). 
        ///     <code lang="VB" title="ToDecimalDegrees Example (VB)">
        /// Dim MyValue As Double = Latitude.ToDecimalDegrees(10, 30, 0)
        ///     </code>
        /// 	<code lang="CS" title="ToDecimalDegrees Example (C#)">
        /// double MyValue = Latitude.ToDecimalDegrees(10, 30, 0);
        ///     </code>
        /// </example>
        public static double ToDecimalDegrees(int hours, int minutes, double seconds)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + minutes / 60.0 + seconds / 3600.0)
                : (hours + minutes / 60.0 + seconds / 3600.0);
        }

        /// <summary>Converts arbitrary hour and decimal minutes into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal 
        /// places, the maximum precision allowed by this type.
        /// </remarks>
        public static double ToDecimalDegrees(int hours, double decimalMinutes)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + decimalMinutes / 60.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + decimalMinutes / 60.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + decimalMinutes / 60.0)
                : (hours + decimalMinutes / 60.0);
        }

        /// <summary>Converts an hour value into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// The specified value will be converted to a double value.
        /// </remarks>
        public static double ToDecimalDegrees(int hours)
        {
            return Convert.ToDouble(hours);
        }

        /// <summary>Returns a random angle between 0° and 360°.</summary>
        /// <returns>An <strong>Elevation</strong> containing a random value.</returns>
        public static Elevation Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random Elevation between 0° and 360° using the specified random number
        /// seed.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        /// <returns>An <strong>Elevation</strong> containing a random value.</returns>
        public static Elevation Random(Random generator)
        {
            return new Elevation(generator.NextDouble() * 360.0);
        }


        /// <returns>The <strong>Elevation</strong> containing the smallest value.</returns>
        /// <summary>Returns the object with the smallest value.</summary>
        /// <param name="value1">A <strong>Elevation</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Elevation</strong> object to compare to value1.</param>
        public static Elevation LesserOf(Elevation value1, Elevation value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>Returns the object with the largest value.</summary>
        /// <returns>A <strong>Elevation</strong> containing the largest value.</returns>
        /// <param name="value1">A <strong>Elevation</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Elevation</strong> object to compare to value1.</param>
        public static Elevation GreaterOf(Elevation value1, Elevation value2)
        {
            return value1.GreaterOf(value2);
        }

        /// <remarks>
        /// 	<para>This function is typically used to convert an angular measurement into
        ///  radians before performing a trigonometric function.</para>
        /// </remarks>
        /// <returns>A <see cref="Radian"><strong>Radian</strong></see> object.</returns>
        /// <summary>Converts an angular measurement into radians.</summary>
        /// <example>
        ///     This example shows a quick way to convert an angle of 90° into radians. 
        ///     <code lang="VB">
        /// Dim MyRadian As Radian = Elevation.ToRadians(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Radian MyRadian = Elevation.ToRadians(90);
        ///     </code>
        /// </example>
        public static Radian ToRadians(Elevation value)
        {
            return value.ToRadians();
        }

        /// <summary>Converts a value in radians into an angular measurement.</summary>
        /// <remarks>
        /// 	This function is typically used in conjunction with the
        /// 	<see cref="Elevation.ToRadians()">ToRadians</see>
        /// 	method after a trigonometric function has completed. The converted value is stored in
        /// 	the <see cref="DecimalDegrees">DecimalDegrees</see> property.
        /// </remarks>
        /// <seealso cref="Elevation.ToRadians(Elevation)">ToRadians</seealso>
        /// <seealso cref="Radian">Radian Class</seealso>
        /// <example>
        ///     This example uses the <strong>FromRadians</strong> method to convert a value of one
        ///     radian into an <strong>Elevation</strong> of 57°. 
        ///     <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyElevation As Elevation = Elevation.FromRadians(MyRadians)
        /// Debug.WriteLine(MyElevation.ToString())
        /// ' Output: 57°
        ///     </code>
        /// 	<code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Elevation MyElevation = Elevation.FromRadians(MyRadians);
        /// Console.WriteLine(MyElevation.ToString());
        /// // Output: 57°
        ///     </code>
        /// </example>
        public static Elevation FromRadians(Radian radians)
        {
            return new Elevation(radians.ToDegrees());
        }

        public static Elevation FromRadians(double radians)
        {
            return new Elevation(Radian.ToDegrees(radians));
        }


        /// <summary>Converts the specified string into an Elevation object.</summary>
        /// <returns>
        /// 	A new <strong>Elevation</strong> object populated with the specified 
        /// 	values.
        /// </returns>
        /// <remarks>
        /// 	<para>This method parses the specified string into an <strong>Elevation</strong> object
        ///     using the current culture. This constructor can parse any strings created via the
        ///     <strong>ToString</strong> method.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <example>
        ///     This example creates a new angular measurement using the <strong>Parse</strong>
        ///     method. 
        ///     <code lang="VB">
        /// Dim NewElevation As Elevation = Elevation.Parse("123.45°")
        ///     </code>
        /// 	<code lang="CS">
        /// Elevation NewElevation = Elevation.Parse("123.45°");
        ///     </code>
        /// </example>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public static Elevation Parse(string value)
        {
            return new Elevation(value, CultureInfo.CurrentCulture);
        }

        /// <remarks>
        /// 	<para>This powerful method is typically used to process data from a data store or a
        ///     value input by the user in any culture. This function can accept any format which
        ///     can be output by the ToString method.</para>
        /// </remarks>
        /// <returns>A new <strong>Elevation</strong> object equivalent to the specified string.</returns>
        /// <summary>
        /// Converts the specified string into an <strong>Elevation</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public static Elevation Parse(string value, CultureInfo culture)
        {
            return new Elevation(value, culture);
        }

        #endregion

        #region Operators

        public static Elevation operator +(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees + right.DecimalDegrees);
        }

        public static Elevation operator +(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees + right);
        }

        public static Elevation operator -(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees - right.DecimalDegrees);
        }

        public static Elevation operator -(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees - right);
        }

        public static Elevation operator *(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees * right.DecimalDegrees);
        }

        public static Elevation operator *(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees * right);
        }

        public static Elevation operator /(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees / right.DecimalDegrees);
        }

        public static Elevation operator /(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees / right);
        }

        public static bool operator ==(Elevation left, Elevation right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator ==(Elevation left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        public static bool operator !=(Elevation left, Elevation right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator !=(Elevation left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        public static bool operator >(Elevation left, Elevation right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        public static bool operator >(Elevation left, double right)
        {
            return left.DecimalDegrees > right;
        }

        public static bool operator >=(Elevation left, Elevation right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        public static bool operator >=(Elevation left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        public static bool operator <(Elevation left, Elevation right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        public static bool operator <(Elevation left, double right)
        {
            return left.DecimalDegrees < right;
        }

        public static bool operator <=(Elevation left, Elevation right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        public static bool operator <=(Elevation left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>Returns the current instance increased by one.</summary>
        /// <returns>An <strong>Elevation</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Elevation</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Increment</strong> method to increase an Elevation's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Increment</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Elevation1 As New Elevation(89)
        /// Elevation1 = Elevation1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Elevation1 = New Elevation(89)
        /// Elevation1.Increment()
        /// ' NOTE: Elevation1 will still be 89°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Elevation Elevation1 = new Elevation(89);
        /// Elevation1 = Elevation1.Increment();
        ///  
        /// // Incorrect use of Increment
        /// Elevation Elevation1 = new Elevation(89);
        /// Elevation1.Increment();
        /// // NOTE: Elevation1 will still be 89°!
        ///     </code>
        /// </example>
        public Elevation Increment()
        {
            return new Elevation(_DecimalDegrees + 1.0);
        }

        /// <summary>Increases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Elevation</strong> containing the summed values.</returns>
        /// <example>
        ///     This example adds 45° to the current instance of 45°, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(45)
        /// Elevation1 = Elevation1.Add(45)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(45);
        /// Elevation1 = Elevation1.Add(45);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        public Elevation Add(double value)
        {
            return new Elevation(_DecimalDegrees + value);
        }

        public Elevation Add(Elevation value)
        {
            return new Elevation(_DecimalDegrees + value.DecimalDegrees);
        }

        /// <summary>Returns the current instance decreased by one.</summary>
        /// <returns>An <strong>Elevation</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Elevation</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Decrement</strong> method to decrease an Elevation's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Decrement</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Elevation1 As New Elevation(91)
        /// Elevation1 = Elevation1.Decrement()
        ///  
        /// ' Incorrect use of Decrement
        /// Dim Elevation1 = New Elevation(91)
        /// Elevation1.Increment()
        /// ' NOTE: Elevation1 will still be 91°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Elevation Elevation1 = new Elevation(91);
        /// Elevation1 = Elevation1.Decrement();
        ///  
        /// // Incorrect use of Decrement
        /// Elevation Elevation1 = new Elevation(91);
        /// Elevation1.Decrement();
        /// // NOTE: Elevation1 will still be 91°!
        ///     </code>
        /// </example>
        public Elevation Decrement()
        {
            return new Elevation(_DecimalDegrees - 1.0);
        }

        /// <summary>Decreases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Elevation</strong> containing the new value.</returns>
        /// <example>
        ///     This example subtracts 30° from the current instance of 90°, returning 60°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(90)
        /// Elevation1 = Elevation1.Subtract(30)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(90);
        /// Elevation1 = Elevation1.Subtract(30);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        public Elevation Subtract(double value)
        {
            return new Elevation(_DecimalDegrees - value);
        }

        public Elevation Subtract(Elevation value)
        {
            return new Elevation(_DecimalDegrees - value.DecimalDegrees);
        }

        /// <summary>Multiplies the current instance by the specified value.</summary>
        /// <returns>A new <strong>Elevation</strong> containing the product of the two numbers.</returns>
        /// <example>
        ///     This example multiplies 30° with three, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(30)
        /// Elevation1 = Elevation1.Multiply(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(30);
        /// Elevation1 = Elevation1.Multiply(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        public Elevation Multiply(double value)
        {
            return new Elevation(_DecimalDegrees * value);
        }

        public Elevation Multiply(Elevation value)
        {
            return new Elevation(_DecimalDegrees * value.DecimalDegrees);
        }

        /// <summary>Divides the current instance by the specified value.</summary>
        /// <returns>An <strong>Elevation</strong> containing the new value.</returns>
        /// <example>
        ///     This example divides 90° by three, returning 30°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(90)
        /// Elevation1 = Elevation1.Divide(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(90);
        /// Elevation1 = Elevation1.Divide(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        public Elevation Divide(double value)
        {
            return new Elevation(_DecimalDegrees / value);
        }

        public Elevation Divide(Elevation value)
        {
            return new Elevation(_DecimalDegrees / value.DecimalDegrees);
        }

        /// <summary>Indicates if the current instance is smaller than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        public bool IsLessThan(Elevation value)
        {
            return _DecimalDegrees < value.DecimalDegrees;
        }

        public bool IsLessThan(double value)
        {
            return _DecimalDegrees < value;
        }

        /// <remarks>
        /// This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.
        /// </remarks>
        /// <summary>
        /// Indicates if the current instance is smaller than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.
        /// </returns>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        public bool IsLessThanOrEqualTo(Elevation value)
        {
            return _DecimalDegrees <= value.DecimalDegrees;
        }

        public bool IsLessThanOrEqualTo(double value)
        {
            return _DecimalDegrees <= value;
        }

        /// <summary>Indicates if the current instance is larger than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        public bool IsGreaterThan(Elevation value)
        {
            return _DecimalDegrees > value.DecimalDegrees;
        }

        public bool IsGreaterThan(double value)
        {
            return _DecimalDegrees > value;
        }

        /// <summary>
        /// Indicates if the current instance is larger than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.
        /// </returns>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        public bool IsGreaterThanOrEqualTo(Elevation value)
        {
            return _DecimalDegrees >= value.DecimalDegrees;
        }

        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _DecimalDegrees >= value;
        }

        #endregion

		#region Conversions

		/// <summary>
		/// Converts a measurement in Radians into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Elevation(Radian value)
		{
            return new Elevation(value.ToDegrees());
		}

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Elevation(double value)
		{
			return new Elevation(value);
		}

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Elevation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Elevation(float value)
        {
            return new Elevation(Convert.ToDouble(value));
        }

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator double(Elevation value)
		{
			return value.DecimalDegrees;
		}

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator float(Elevation value)
		{
			return Convert.ToSingle(value.DecimalDegrees);
		}

		/// <summary>
		/// Converts a measurement in degrees as an Integer into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Elevation(int value)
		{
			return new Elevation(value);
		}

        public static explicit operator Elevation(Angle value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        public static explicit operator Elevation(Azimuth value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        public static explicit operator Elevation(Latitude value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        public static explicit operator Elevation(Longitude value)
        {
            return new Elevation(value.DecimalDegrees);
        }

		/// <summary>
		/// Converts a measurement in the form of a formatted String into an Elevation.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Elevation(string value)
		{
			return new Elevation(value, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Converts an Elevation into a String.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <remarks>This operator calls the ToString() method using the current culture.</remarks>
		public static explicit operator string(Elevation value)
		{
			return value.ToString("g", CultureInfo.CurrentCulture);
		}

		#endregion

        #region ICloneable<Elevation> Members

        /// <summary>Creates a copy of the current instance.</summary>
        /// <returns>An <strong>Elevation</strong> of the same value as the current instance.</returns>
        public Elevation Clone()
        {
            return new Elevation(_DecimalDegrees);
        }

        #endregion

        #region IFormattable Members

        /// <summary>Outputs the angle as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "d.dddd" is used. Any string
        ///     output by this method can be converted back into an Elevation object using the
        ///     <strong>Parse</strong> method or <strong>Elevation(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 16.772)
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyElevation As New Elevation(45, 16.772);
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///     </code>
        /// </example>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0
                || format == "g" || format == "G")
                format = "e";

            if (format == "e")
            {
                if (DecimalDegrees < 0)
                    return "Below the horizon";
                else if (DecimalDegrees < 30)
                    return "Near the horizon";
                else if (DecimalDegrees < 60)
                    return "Halfway up from the horizon";
                else if (DecimalDegrees < 80)
                    return "Almost directly overhead";
                else
                    return "Directly overhead";
            }

            // Parse as a normal angle
            int StartChar = 0;
            int EndChar = 0;
            string SubFormat = null;
            string NewFormat = null;
            bool IsDecimalHandled = false;
            try
            {
                // Is it infinity?
                if (double.IsPositiveInfinity(DecimalDegrees))
                    return "+" + Properties.Resources.Common_Infinity;
                // Is it infinity?
                if (double.IsNegativeInfinity(DecimalDegrees))
                    return "-" + Properties.Resources.Common_Infinity;
                if (double.IsNaN(DecimalDegrees))
                    return "NaN";
                // Prevent null values
                if (format == null) format = "g";
                // Use the default if "g" is passed
                format = format.ToLower(culture);
                if (format == "g")
                    format = "d.dddd°";
                // Replace the "d" with "h" since degrees is the same as hours
                format = format.Replace("d", "h")
                    // Convert the format to uppercase
                    .ToUpper(culture);
                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator))
                    throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
                // Is there an hours specifier?
                StartChar = format.IndexOf("H");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("H");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("H", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, DecimalDegrees.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Hours.ToString(NewFormat, culture));
                    }
                }
                // Is there an hours specifier°
                StartChar = format.IndexOf("M");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("M");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("M", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (IsDecimalHandled)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
                        }
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, DecimalMinutes.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Minutes.ToString(NewFormat, culture));
                    }
                }
                // Is there an hours specifier°
                StartChar = format.IndexOf("S");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("S");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("S", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (IsDecimalHandled)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
                        }
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                }
                // If nothing then return zero
                if (String.Compare(format, "°", true, culture) == 0)
                    return "0°";
                return format;
            }
            catch
            {
                throw new ArgumentException(Properties.Resources.Angle_InvalidToStringFormat);
            }
        }

        #endregion

        #region IEquatable<Elevation> Members

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <returns>
        /// 	<para>A <strong>Boolean</strong>, <strong>True</strong> if the
        ///     <strong>DecimalDegrees</strong> property of the current instance matches the
        ///     specified instance's <strong>DecimalDegrees</strong> property.</para>
        /// </returns>
        /// <remarks>
        /// 	<para>This is typically used in cases where precision is only significant for a few
        ///     digits and exact comparison is not necessary.</para>
        /// 	<para><em>NOTE: This method compares objects by value, not by
        ///     reference.</em></para>
        /// </remarks>
        /// <seealso cref="Equals(Elevation)">Equals Method</seealso>
        /// <example>
        ///     These examples compare two fractional values using specific numbers of digits for
        ///     comparison. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Elevation1 As New Elevation(90.15);
        /// Dim Elevation2 As New Elevation(90.12);
        /// If Elevation1.Equals(Elevation2, 2) Then
        ///      Debug.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// ' Equals will return True
        /// Dim Elevation1 As New Elevation(90.15);
        /// Dim Elevation2 As New Elevation(90.12);
        /// If Elevation1.Equals(Elevation2, 1) Then
        ///      Debug.WriteLine("The values are the same to one digit of precision.");
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Elevation Elevation1 = new Elevation(90.15);
        /// Elevation Elevation2 = new Elevation(90.12);
        /// if(Elevation1.Equals(Elevation2, 2))
        ///      Console.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// // Equals will return True
        /// Elevation Elevation1 = new Elevation(90.15);
        /// Elevation Elevation2 = new Elevation(90.12);
        /// if(Elevation1.Equals(Elevation2, 1))
        ///      Console.WriteLine("The values are the same to one digits of precision.");
        ///     </code>
        /// </example>
        public bool Equals(Elevation value, int decimals)
        {
            return Equals(value.DecimalDegrees, decimals);
        }

        public bool Equals(Elevation value)
        {
            return Equals(value.DecimalDegrees);
        }

        #endregion

        #region IComparable<Elevation> Members

        /// <summary>Returns a value indicating the relative order of two objects.</summary>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>
        ///		This method allows collections of <strong>Azimuth</strong> objects to be sorted.
        ///		The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.
        /// </remarks>
        /// <param name="other">An <strong>Elevation</strong> object to compare with.</param>
        public int CompareTo(Elevation other)
        {
            return _DecimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Text)
                _DecimalDegrees = reader.ReadContentAsDouble();
            else
                _DecimalDegrees = reader.ReadElementContentAsDouble();
        }

        #endregion
    }
}
