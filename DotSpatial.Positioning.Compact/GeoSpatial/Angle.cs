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
	/// <summary>Represents an angular measurement around a circle.</summary>
	/// <remarks>
	/// 	<para>This class serves as the base class for angular measurement classes within
	///     the framework, such as the <strong>Azimuth</strong>, <strong>Elevation</strong>,
	///     <strong>Latitude</strong> and <strong>Longitude</strong> classes. An "angular
	///     measurement" is a measurement around a circle. Typically, angular measurements are
	///     between 0° and 360°.</para>
	/// 	<para>Angles can be represented in two forms: decimal and sexagesimal. In decimal
	///     form, angles are represented as a single number. In sexagesimal form, angles are
	///     represented in three components: hours, minutes, and seconds, very much like a
	///     clock.</para>
	/// 	<para>Upon creating an <strong>Angle</strong> object, other properties such as
	///     <strong>DecimalDegrees</strong>, <strong>DecimalMinutes</strong>,
	///     <strong>Hours</strong>, <strong>Minutes</strong> and <strong>Seconds</strong> are
	///     calculated automatically.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because they are
	///     immutable (properties can only be modified via constructors).</para>
	/// </remarks>
	/// <seealso cref="Azimuth">Azimuth Class</seealso>
	/// <seealso cref="Elevation">Elevation Class</seealso>
	/// <seealso cref="Latitude">Latitude Class</seealso>
	/// <seealso cref="Longitude">Longitude Class</seealso>
	/// <example>
	///     These examples create new instances of Angle objects. 
	///     <code lang="VB" description="Create an angle of 90°">
	/// Dim MyAngle As New Angle(90)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 90°">
	/// Angle MyAngle = new Angle(90);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 90°">
	/// Angle MyAngle = new Angle(90);
	///     </code>
	/// 	<code lang="VB" description="Create an angle of 105°30'21.4">
	/// Dim MyAngle1 As New Angle(105, 30, 21.4)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 105°30'21.4">
	/// Angle MyAngle = new Angle(105, 30, 21.4);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 105°30'21.4">
	/// Angle MyAngle = new Angle(105, 30, 21.4);
	///     </code>
    /// </example>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.AngleConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Angle : IFormattable, IComparable<Angle>, IEquatable<Angle>, ICloneable<Angle>, IXmlSerializable
    {      
        private double _DecimalDegrees;

        #region Constants

        private const int MaximumPrecisionDigits = 12;
        private const double fromGon = 1.0 / .9;

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
		/// Dim MyAngle As Angle = Angle.Minimum
		///     </code>
		/// 	<code lang="CS">
		/// Angle MyAngle = Angle.Minimum;
		///     </code>
		/// 	<code lang="C++">
		/// Angle MyAngle = Angle.Minimum;
		///     </code>
		/// </example>
		/// <value>An Angle with a value of -359.999999°.</value>
		public static readonly Angle Minimum = new Angle(-359.99999999);

		/// <summary>Represents an angle with no value.</summary>
		/// <remarks>
		/// This member is typically used to initialize an angle variable to zero. When an
		/// angle has a value of zero, its <see cref="IsEmpty">IsEmpty</see> property returns
		/// <strong>True</strong>.
		/// </remarks>
		/// <value>An Angle containing a value of zero (0°).</value>
		/// <seealso cref="IsEmpty">IsEmpty Property</seealso>
		public static readonly Angle Empty = new Angle(0.0);

		/// <summary>
		/// Represents an angle with infinite value.
		/// </summary>
		/// <remarks>
		/// In some cases, the result of an angular calculation may be infinity. This member
		/// is used in such cases. The <see cref="DecimalDegrees">DecimalDegrees</see> property is
		/// set to Double.PositiveInfinity.
		/// </remarks>
		public static readonly Angle Infinity = new Angle(double.PositiveInfinity);

		/// <summary>Represents the maximum value of an angle in one turn of a circle.</summary>
		/// <remarks>
		/// This member is typically used for looping through the entire range of possible
		/// angles, or to test the range of a value. It is possible to create angular values below
		/// this value, such as 720°.
		/// </remarks>
		/// <example>
		///     This example creates an angle representing the maximum allowed value of 359.9999°. 
		///     <code lang="VB">
		/// Dim MyAngle As Angle = Angle.Maximum
		///     </code>
		/// 	<code lang="CS">
		/// Angle MyAngle = Angle.Maximum;
		///     </code>
		/// </example>
		public static readonly Angle Maximum = new Angle(359.99999999);

        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Angle Invalid = new Angle(double.NaN);

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance with the specified decimal degrees.</summary>
        /// <example>
        ///     This example demonstrates how to create an angle with a measurement of 90°. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(90);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        public Angle(double decimalDegrees)
        {
            // Set the decimal degrees value
            _DecimalDegrees = decimalDegrees;
        }

        /// <summary>Creates a new instance with the specified degrees.</summary>
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        /// <param name="hours">
        /// An <strong>Integer</strong> indicating the amount of degrees, typically between 0
        /// and 360.
        /// </param>
        public Angle(int hours)
        {
            _DecimalDegrees = ToDecimalDegrees(hours);
        }

        /// <summary>Creates a new instance with the specified hours, minutes and 
        /// seconds.</summary>
        /// <example>
        ///     This example demonstrates how to create an angular measurement of 34°12'29.2 in
        ///     hours, minutes and seconds. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(34, 12, 29.2)
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(34, 12, 29.2);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        public Angle(int hours, int minutes, double seconds)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>Creates a new instance with the specified hours and decimal minutes.</summary>
        /// <example>
        ///     This example demonstrates how an angle can be created when only the hours and
        ///     minutes (in decimal form) are known. This creates a value of 12°42.345'. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(12, 42.345)
        ///     </code>
        /// 	<code lang="VB">
        /// Angle MyAngle = new Angle(12, 42.345);
        ///     </code>
        /// </example>
        /// <remarks>An <strong>Angle</strong> containing the specified value.</remarks>
        public Angle(int hours, double decimalMinutes)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>Creates a new instance by converting the specified string.</summary>
        /// <remarks>
        /// This constructor parses the specified string into an <strong>Angle</strong>
        /// object using the current culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.
        /// </remarks>
        /// <seealso cref="Angle.Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example creates a new instance by parsing a string. (NOTE: The double-quote is
        ///     doubled up to represent a single double-quote in the string.) 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle("123°45'67.8""")
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle("123°45'67.8\"");
        ///     </code>
        /// </example>
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Angle(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <remarks>
        /// This constructor parses the specified string into an <strong>Angle</strong>
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
        public Angle(string value, CultureInfo culture)
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
                        // Return a blank Angle
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
                        else if (Values[0].Length == 7 && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.CurrentCulture) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 3), culture),
                                int.Parse(Values[0].Substring(3, 2), culture),
                                double.Parse(Values[0].Substring(5, 2), culture));
                            return;
                        }
                        else if (Values[0].Length == 8 && Values[0][0] == '-' && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.CurrentCulture) == -1)
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
                        _DecimalDegrees = ToDecimalDegrees(int.Parse(Values[0], culture),
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
        public Angle(XmlReader reader)
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
        /// Dim MyAngle As New Angle(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties 
        /// Debug.WriteLine(MyAngle.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°30'
        /// Angle MyAngle = New Angle(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties 
        /// Console.WriteLine(MyAngle.DecimalDegrees)
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
        /// Dim MyAngle As New Angle(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyAngle.DecimalMinutes)
        /// ' Output: "10.5"
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Angle MyAngle = new Angle(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyAngle.DecimalMinutes)
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
        /// Dim MyAngle As New Angle(60.5)
        /// Debug.WriteLine(MyAngle.Hours)
        /// ' Output: 60
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(60.5);
        /// Console.WriteLine(MyAngle.Hours);
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
        /// Dim MyAngle As New Angle(45.5)
        /// Debug.WriteLine(MyAngle.Minutes)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(45.5);
        /// Console.WriteLine(MyAngle.Minutes);
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
        /// Dim MyAngle As New Angle(45, 10.5)
        /// Debug.WriteLine(MyAngle.Seconds)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyAngle As New Angle(45, 10.5);
        /// Console.WriteLine(MyAngle.Seconds);
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

        /// <summary>Indicates whether the value is invalid or unspecified.</summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_DecimalDegrees); }
        }

        /// <summary>Indicates whether the value has been normalized and is within the 
        /// allowed bounds of 0° and 360°.</summary>
        public bool IsNormalized
        {
            get { return _DecimalDegrees >= 0 && _DecimalDegrees < 360; }
        }

        #endregion

        #region Public Methods

        /// <returns>An <strong>Angle</strong> containing the largest value.</returns>
        /// <summary>Returns the object with the largest value.</summary>
        /// <param name="value">An <strong>Angle</strong> object to compare to the current instance.</param>
        public Angle GreaterOf(Angle value)
        {
            if (_DecimalDegrees > value.DecimalDegrees)
                return this;
            else
                return value;
        }

		/// <summary>Returns the object with the smallest value.</summary>
		/// <returns>The <strong>Angle</strong> containing the smallest value.</returns>
		/// <param name="value">An <strong>Angle</strong> object to compare to the current instance.</param>
		public Angle LesserOf(Angle value)
		{
			if(_DecimalDegrees < value.DecimalDegrees)
				return this;
			else
				return value;
		}

        /// <summary>Returns an angle opposite of the current instance.</summary>
        /// <returns>An <strong>Angle</strong> representing the mirrored value.</returns>
        /// <remarks>
        /// This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Angle</strong> of 45° then calculates its mirror
        ///     of 225°. (45 + 180) 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(45)
        /// Dim Angle2 As Angle = Angle1.Mirror()
        /// Debug.WriteLine(Angle2.ToString())
        /// ' Output: 225
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(45);
        /// Angle Angle2 = Angle1.Mirror();
        /// Console.WriteLine(Angle2.ToString());
        /// // Output: 225
        ///     </code>
        /// </example>
        public Angle Mirror()
        {
            return new Angle(_DecimalDegrees + 180.0).Normalize();
        }

        /// <summary>Modifies a value to its equivalent between 0° and 360°.</summary>
        /// <returns>An <strong>Angle</strong> representing the normalized angle.</returns>
        /// <remarks>
        /// 	<para>This function is used to ensure that an angular measurement is within the
        ///     allowed bounds of 0° and 360°. If a value of 360° or 720° is passed, a value of 0°
        ///     is returned since 360° and 720° represent the same point on a circle. For the Angle
        ///     class, this function is the same as "value Mod 360".</para>
        /// </remarks>
        /// <seealso cref="Normalize(double)">Normalize(Angle) Method</seealso>
        /// <example>
        ///     This example demonstrates how normalization is used. The Stop statement is hit.
        ///     This example demonstrates how the Normalize method can ensure that an angle fits
        ///     between 0° and 359.9999°. This example normalizes 725° into 5°. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(720)
        /// MyAngle = MyAngle.Normalize()
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(720);
        /// MyAngle = MyAngle.Normalize();
        ///     </code>
        /// 	<code lang="VB">
        /// Dim MyValue As New Angle(725)
        /// MyValue = MyValue.Normalize()
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyValue = new Angle(725);
        /// MyValue = MyValue.Normalize();
        ///     </code>
        /// </example>
        public Angle Normalize()
        {
            double _Value = _DecimalDegrees;
            while (_Value < 0)
            {
                _Value += 360.0;
            }
            return new Angle(_Value % 360);
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
		/// Dim MyAngle As New Angle(90)
		/// Dim MyRadians As Radian = MyAngle.ToRadians()
		///     </code>
		/// 	<code lang="CS">
		/// Angle MyAngle = new Angle(90);
		/// Radian MyRadians = MyAngle.ToRadians();
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
        ///     string output by this method can be converted back into an Angle object using the
        ///     <strong>Parse</strong> method or <strong>Angle(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString(string, IFormatProvider)">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(45, 16.772)
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyAngle As New Angle(45, 16.772);
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///     </code>
        /// </example>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the smallest integer greater than the specified value.</summary>
        public Angle Ceiling()
        {
            return new Angle(Math.Ceiling(_DecimalDegrees));
        }

        /// <summary>Returns the largest integer which is smaller than the specified value.</summary>
        public Angle Floor()
        {
            return new Angle(Math.Floor(_DecimalDegrees));
        }

#if !Framework20 || PocketPC
		internal static int Truncate(double value)
		{
			return value > 0 
				? (int)(value - (value - Math.Floor(value))) 
				: (int)(value - (value - Math.Ceiling(value)));
		}
#endif

        /// <summary>Returns a new instance whose Seconds property is evenly divisible by 15.</summary>
        /// <returns>An <strong>Angle</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        public Angle RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new instance whose value is rounded the specified number of decimals.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of decimals to round off to.</param>
        /// <returns></returns>
        public Angle Round(int decimals)
        {
            return new Angle(Math.Round(_DecimalDegrees, decimals));
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <returns>An <strong>Angle</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        /// <param name="interval">
        /// A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.
        /// </param>
        public Angle RoundSeconds(double interval)
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
                return new Angle(Hours, Minutes, NewSeconds);
            }
            // return the new value
            return new Angle(Hours, Minutes, NewSeconds);
        }

        #endregion

        #region Overrides

        /// <summary>Compares the current value to another Angle object's value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.
        /// </returns>
        /// <remarks>This </remarks>
        /// <param name="obj">
        /// An <strong>Angle</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.
        /// </param>
        public override bool Equals(object obj)
        {
            // Convert objects to an Angle as needed before comparison
            if (obj is Angle)
                return Equals((Angle)obj);

            // Nothing else will work, so False
            return false;
        }

        /// <summary>Returns a unique code for this instance.</summary>
		/// <remarks>
		/// Since the <strong>Angle</strong> class is immutable, this property may be used
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
        ///     "d.dddd°." Any string output by this method can be converted back into an Angle
        ///     object using the <strong>Parse</strong> method or <strong>Angle(string)</strong>
        ///     constructor.</para>
        /// </remarks>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example outputs a value of 90 degrees in the default format of ###.#°. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(90)
        /// Debug.WriteLine(MyAngle.ToString)
        /// ' Output: "90°"
        ///     </code>
        /// 	<code lang="CS">
        /// Angle MyAngle = new Angle(90);
        /// Debug.WriteLine(MyAngle.ToString());
        /// // Output: "90°"
        ///     </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <summary>Converts the specified value to its equivalent between 0° and 360°.</summary>
        /// <returns>
        /// An Angle containing a value equivalent to the value specified, but between 0° and
        /// 360°.
        /// </returns>
        /// <param name="decimalDegrees">A <strong>Double</strong> value to be normalized.</param>
        public static Angle Normalize(double decimalDegrees)
        {
            return new Angle(decimalDegrees).Normalize();
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
		/// Dim MyRadian As Radian = Angle.ToRadians(90)
		///     </code>
		/// 	<code lang="CS">
		/// Radian MyRadian = Angle.ToRadians(90);
		///     </code>
		/// </example>
		public static Radian ToRadians(Angle value)
		{
            return value.ToRadians();
		}

		/// <summary>Converts a value in radians into an angular measurement.</summary>
		/// <remarks>
		/// 	This function is typically used in conjunction with the
		/// 	<see cref="Angle.ToRadians()">ToRadians</see>
		/// 	method after a trigonometric function has completed. The converted value is stored in
		/// 	the <see cref="DecimalDegrees">DecimalDegrees</see> property.
		/// </remarks>
		/// <seealso cref="Angle.ToRadians()">ToRadians</seealso>
		/// <seealso cref="Radian">Radian Class</seealso>
		/// <example>
		///     This example uses the <strong>FromRadians</strong> method to convert a value of one
		///     radian into an <strong>Angle</strong> of 57°. 
		///     <code lang="VB">
		/// ' Create a new angle equal to one radian
		/// Dim MyRadians As New Radian(1)
		/// Dim MyAngle As Angle = Angle.FromRadians(MyRadians)
		/// Debug.WriteLine(MyAngle.ToString())
		/// ' Output: 57°
		///     </code>
		/// 	<code lang="CS">
		/// // Create a new angle equal to one radian
		/// Radian MyRadians = new Radian(1);
		/// Angle MyAngle = Angle.FromRadians(MyRadians);
		/// Console.WriteLine(MyAngle.ToString());
		/// // Output: 57°
		///     </code>
		/// </example>
        public static Angle FromRadians(Radian radians)
        {
            return radians.ToAngle();
        }

        public static Angle FromRadians(double radians)
        {
            return new Angle(Radian.ToDegrees(radians));
        }

        /// <summary>
        /// Convers a sexagesimal number into an Angle.
        /// </summary>
        /// <param name="dms">A Double value, a number in the form of DDD.MMSSSSS format</param>
        /// <returns>An <strong>Angle</strong> object.</returns>
        public static Angle FromSexagesimal(double dms)
        {
            // Shift the decimal left 2 places and save it.
            double dmsX100 = dms * 100;
            // Get the integral portion for the hours
            int hours = (int)Math.Floor(dms);
            // Subtract the hours from the sexagesimal number, leaving just the fractional portion,
            // shift the decimal left 2 places and truncate for the minutes
            int minutes = (int)Math.Floor(Math.Round((dms - (double)hours) * 100));
            // Subtract the integral portion of the shifted sexagesimal number for the shifted sexagesimal number,
            // leaving just the fractional portion, shift the decimal left 2 places and truncate for the seconds.
            double seconds = ((dmsX100) - Math.Floor(dmsX100)) * 100;

            // Create an Angle from the hours, minutes and seconds
            return new Angle(hours, Math.Abs(minutes), seconds);
        }

        /// <returns>The <strong>Angle</strong> containing the smallest value.</returns>
        /// <summary>Returns the object with the smallest value.</summary>
        /// <param name="value1">A <strong>Angle</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Angle</strong> object to compare to value1.</param>
        public static Angle LesserOf(Angle value1, Angle value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>Returns the object with the largest value.</summary>
        /// <returns>A <strong>Angle</strong> containing the largest value.</returns>
        /// <param name="value1">A <strong>Angle</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Angle</strong> object to compare to value1</param>
        public static Angle GreaterOf(Angle value1, Angle value2)
        {
            return value1.GreaterOf(value2);
        }

        /// <summary>Returns a random angle between 0° and 360°.</summary>
        /// <returns>An <strong>Angle</strong> containing a random value.</returns>
        public static Angle Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random Angle between 0° and 360°
        /// </summary>
        /// <returns>An <strong>Angle</strong> containing a random value.</returns>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        public static Angle Random(Random generator)
        {
            return new Angle(generator.NextDouble() * 360.0);
        }

        #endregion

        #region Conversions

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Angle.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Angle(double value)
		{
			return new Angle(value);
		}

        /// <summary>
        /// Converts a decimal degree measurement as a Single into an Angle.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Angle(float value)
        {
            return new Angle(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a measurement in Radians into an Angle.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Angle(Radian value)
        {
            return value.ToAngle();
        }

		/// <summary>
		/// Converts a decimal degree measurement as a Angle into an Double.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator double(Angle value)
		{
			return value.DecimalDegrees;
		}

		/// <summary>
		/// Converts a decimal degree measurement as a Angle into a Single.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator float(Angle value)
		{
			return Convert.ToSingle(value.DecimalDegrees);
		}

		/// <summary>
		/// Converts a measurement in degrees as an Integer into an Angle.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Angle(int value)
		{
			return new Angle(value);
		}

        public static explicit operator Angle(Azimuth value)
        {
            return new Angle(value.DecimalDegrees);
        }

        public static explicit operator Angle(Elevation value)
        {
            return new Angle(value.DecimalDegrees);
        }

        public static explicit operator Angle(Latitude value)
        {
            return new Angle(value.DecimalDegrees);
        }

        public static explicit operator Angle(Longitude value)
        {
            return new Angle(value.DecimalDegrees);
        }


		/// <summary>
		/// Converts a measurement in the form of a formatted String into an Angle.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Angle(string value)
		{
			return new Angle(value, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Converts an Angle into a String.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <remarks>This operator calls the ToString() method using the current culture.</remarks>
		public static explicit operator string(Angle value)
		{
			return value.ToString("g", CultureInfo.CurrentCulture);
		}

		#endregion

        #region Operators

        public static Angle operator +(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees + right.DecimalDegrees);
        }

        public static Angle operator +(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees + right);
        }

        public static Angle operator -(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees - right.DecimalDegrees);
        }

        public static Angle operator -(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees - right);
        }

        public static Angle operator *(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees * right.DecimalDegrees);
        }

        public static Angle operator *(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees * right);
        }

        public static Angle operator /(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees / right.DecimalDegrees);
        }

        public static Angle operator /(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees / right);
        }

        public static bool operator ==(Angle left, Angle right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator ==(Angle left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        public static bool operator !=(Angle left, Angle right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator !=(Angle left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        public static bool operator >(Angle left, Angle right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        public static bool operator >(Angle left, double right)
        {
            return left.DecimalDegrees > right;
        }

        public static bool operator >=(Angle left, Angle right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        public static bool operator >=(Angle left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        public static bool operator <(Angle left, Angle right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        public static bool operator <(Angle left, double right)
        {
            return left.DecimalDegrees < right;
        }

        public static bool operator <=(Angle left, Angle right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        public static bool operator <=(Angle left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>Returns the current instance increased by one.</summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Angle</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Increment</strong> method to increase an angle's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Increment</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Angle1 As New Angle(89)
        /// Angle1 = Angle1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Angle1 = New Angle(89)
        /// Angle1.Increment()
        /// ' NOTE: Angle1 will still be 89°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Angle Angle1 = new Angle(89);
        /// Angle1 = Angle1.Increment();
        ///  
        /// // Incorrect use of Increment
        /// Angle Angle1 = new Angle(89);
        /// Angle1.Increment();
        /// // NOTE: Angle1 will still be 89°!
        ///     </code>
        /// </example>
        public Angle Increment()
        {
            return new Angle(_DecimalDegrees + 1.0);
        }

        /// <summary>Increases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Angle</strong> containing the summed values.</returns>
        /// <example>
        ///     This example adds 45° to the current instance of 45°, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(45)
        /// Angle1 = Angle1.Add(45)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(45);
        /// Angle1 = Angle1.Add(45);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        public Angle Add(double value)
        {
            return new Angle(_DecimalDegrees + value);
        }

        public Angle Add(Angle value)
        {
            return new Angle(_DecimalDegrees + value.DecimalDegrees);
        }

        /// <summary>Returns the current instance decreased by one.</summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Angle</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Decrement</strong> method to decrease an angle's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Decrement</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Angle1 As New Angle(91)
        /// Angle1 = Angle1.Decrement()
        ///  
        /// ' Incorrect use of Decrement
        /// Dim Angle1 = New Angle(91)
        /// Angle1.Increment()
        /// ' NOTE: Angle1 will still be 91°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Angle Angle1 = new Angle(91);
        /// Angle1 = Angle1.Decrement();
        ///  
        /// // Incorrect use of Decrement
        /// Angle Angle1 = new Angle(91);
        /// Angle1.Decrement();
        /// // NOTE: Angle1 will still be 91°!
        ///     </code>
        /// </example>
        public Angle Decrement()
        {
            return new Angle(_DecimalDegrees - 1.0);
        }

        /// <summary>Decreases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Angle</strong> containing the new value.</returns>
        /// <example>
        ///     This example subtracts 30° from the current instance of 90°, returning 60°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(90)
        /// Angle1 = Angle1.Subtract(30)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(90);
        /// Angle1 = Angle1.Subtract(30);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        public Angle Subtract(double value)
        {
            return new Angle(_DecimalDegrees - value);
        }

        public Angle Subtract(Angle value)
        {
            return new Angle(_DecimalDegrees - value.DecimalDegrees);
        }

        /// <summary>Multiplies the current instance by the specified value.</summary>
        /// <returns>A new <strong>Angle</strong> containing the product of the two numbers.</returns>
        /// <example>
        ///     This example multiplies 30° with three, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(30)
        /// Angle1 = Angle1.Multiply(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(30);
        /// Angle1 = Angle1.Multiply(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        public Angle Multiply(double value)
        {
            return new Angle(_DecimalDegrees * value);
        }

        public Angle Multiply(Angle value)
        {
            return new Angle(_DecimalDegrees * value.DecimalDegrees);
        }

        /// <summary>Divides the current instance by the specified value.</summary>
        /// <returns>An <strong>Angle</strong> containing the new value.</returns>
        /// <example>
        ///     This example divides 90° by three, returning 30°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(90)
        /// Angle1 = Angle1.Divide(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(90);
        /// Angle1 = Angle1.Divide(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        public Angle Divide(double value)
        {
            return new Angle(_DecimalDegrees / value);
        }

        public Angle Divide(Angle value)
        {
            return new Angle(_DecimalDegrees / value.DecimalDegrees);
        }

        /// <summary>Indicates if the current instance is smaller than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        public bool IsLessThan(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        public bool IsLessThanOrEqualTo(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        public bool IsGreaterThan(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        public bool IsGreaterThanOrEqualTo(Angle value)
        {
            return _DecimalDegrees >= value.DecimalDegrees;
        }

        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _DecimalDegrees >= value;
        }

        #endregion

        #region Static methods

        /// <summary>Converts the specified string into an Angle object.</summary>
		/// <returns>
		/// 	A new <strong>Angle</strong> object populated with the specified 
		/// 	values.
		/// </returns>
		/// <remarks>
		/// 	<para>This method parses the specified string into an <strong>Angle</strong> object
		///     using the current culture. This constructor can parse any strings created via the
		///     <strong>ToString</strong> method.</para>
		/// </remarks>
		/// <seealso cref="ToString()">ToString Method</seealso>
		/// <example>
		///     This example creates a new angular measurement using the <strong>Parse</strong>
		///     method. 
		///     <code lang="VB">
		/// Dim NewAngle As Angle = Angle.Parse("123.45°")
		///     </code>
		/// 	<code lang="CS">
		/// Angle NewAngle = Angle.Parse("123.45°");
		///     </code>
		/// </example>
		/// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
		/// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
		/// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
		/// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
		public static Angle Parse(string value)
		{
			return new Angle(value, CultureInfo.CurrentCulture);
		}
		
		/// <remarks>
		/// 	<para>This powerful method is typically used to process data from a data store or a
		///     value input by the user in any culture. This function can accept any format which
		///     can be output by the ToString method.</para>
		/// </remarks>
		/// <returns>A new <strong>Angle</strong> object equivalent to the specified string.</returns>
		/// <summary>
		/// Converts the specified string into an <strong>Angle</strong> object using the
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
		public static Angle Parse(string value, CultureInfo culture)
		{
			return new Angle(value, culture);
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

        #endregion

        #region ICloneable<Angle> Members

        /// <summary>Creates a copy of the current instance.</summary>
        /// <returns>An <strong>Angle</strong> of the same value as the current instance.</returns>
        public Angle Clone()
        {
            return new Angle(_DecimalDegrees);
        }

        #endregion

        #region IComparable<Angle> Members

        /// <summary>Returns a value indicating the relative order of two objects.</summary>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>
        ///		This method allows collections of <strong>Azimuth</strong> objects to be sorted.
        ///		The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.
        /// </remarks>
        /// <param name="other">An <strong>Angle</strong> object to compare with.</param>
        public int CompareTo(Angle other)
        {
            return _DecimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion

        #region IEquatable<Angle> Members

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
        /// <seealso cref="Equals(Angle,int)">Equals Method</seealso>
        /// <example>
        ///     These examples compare two fractional values using specific numbers of digits for
        ///     comparison. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 2) Then
        ///      Debug.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// ' Equals will return True
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 1) Then
        ///      Debug.WriteLine("The values are the same to one digit of precision.");
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if(Angle1.Equals(Angle2, 2))
        ///      Console.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// // Equals will return True
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if(Angle1.Equals(Angle2, 1))
        ///      Console.WriteLine("The values are the same to one digits of precision.");
        ///     </code>
        /// </example>
        public bool Equals(Angle angle)
        {
            return _DecimalDegrees.Equals(angle.DecimalDegrees);
        }

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
        /// <seealso cref="Equals(Angle,int)">Equals Method</seealso>
        /// <example>
        ///     These examples compare two fractional values using specific numbers of digits for
        ///     comparison. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 2) Then
        ///      Debug.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// ' Equals will return True
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 1) Then
        ///      Debug.WriteLine("The values are the same to one digit of precision.");
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if(Angle1.Equals(Angle2, 2))
        ///      Console.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// // Equals will return True
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if(Angle1.Equals(Angle2, 1))
        ///      Console.WriteLine("The values are the same to one digits of precision.");
        ///     </code>
        /// </example>
        public bool Equals(Angle angle, int decimals)
        {
            return Math.Round(_DecimalDegrees, decimals).Equals(Math.Round(angle.DecimalDegrees, decimals));
        }

        #endregion
        
        #region IFormattable Members

        /// <summary>Outputs the angle as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "d.dddd" is used. Any string
        ///     output by this method can be converted back into an Angle object using the
        ///     <strong>Parse</strong> method or <strong>Angle(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyAngle As New Angle(45, 16.772)
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyAngle As New Angle(45, 16.772);
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
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
            string NewFormat = null;
            bool IsDecimalHandled = false;
            try
            {
                // Is it infinity?
                if (double.IsPositiveInfinity(_DecimalDegrees))
                    return "+" + Properties.Resources.Common_Infinity;
                // Is it infinity?
                if (double.IsNegativeInfinity(_DecimalDegrees))
                    return "-" + Properties.Resources.Common_Infinity;
                if (double.IsNaN(_DecimalDegrees))
                    return "NaN";

                // Use the default if "g" is passed
                if (String.Compare(format, "g", StringComparison.OrdinalIgnoreCase) == 0)
                    format = "d.dddd°";

                // Replace the "d" with "h" since degrees is the same as hours
                format = format.ToUpper(CultureInfo.InvariantCulture).Replace("D", "H");

                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal))
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

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            /* This class conforms to GML version 3.0.  
             * 
             * <gml:angle uom="degrees">123.45678</gml:angle>
             * 
             */

            // Write the start element
            writer.WriteStartElement(Xml.GmlXmlPrefix, "angle", Xml.GmlXmlNamespace);

            // Write the units-of-measure (degrees)
            writer.WriteAttributeString("uom", "degrees");

            // Write the angle value
            writer.WriteString(_DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));

            // Close up the element
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            /* This class conforms to GML version 3.0.  
             * 
             * <gml:angle uom="degrees">123.45678</gml:angle>
             * 
            */

            // Move to the <gml:angle> element
            if (!reader.IsStartElement("angle", Xml.GmlXmlNamespace))
                reader.ReadToDescendant("angle", Xml.GmlXmlNamespace);

            // Read in the element content
            // I'm going to assume for now that the unit of measure is degrees.
            _DecimalDegrees = reader.ReadElementContentAsDouble();
        }

        #endregion
    }
}